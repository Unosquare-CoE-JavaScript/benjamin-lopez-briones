# Chapter 1: What's the Scope?
The first of three pillars in the JS language: the scope system and its function closures, as well as the power of the module design pattern.
the first of three pillars in the JS language: the scope system and its function closures, as well as the power of the module design pattern.
JS functions are themselves first-class values; they can be assigned and passed around just like numbers or strings. But since these functions hold and access variables, they maintain their original scope no matter where in the program the functions are eventually executed. This is called closure.
Modules are a code organization pattern characterized by public methods that have privileged access (via closure) to hidden variables and functions in the internal scope of the module.

## Compiled vs. Interpreted
Code compilation is a set of steps that process the text of your code and turn it into a list of instructions the computer can understand. Typically, the whole source code is transformed at once, and those resulting instructions are saved as output (usually in a file) that can later be executed. Interpretation performs a similar task to compilation, in that it transforms your program into machine-understandable  instructions. But the processing model is different. Unlike a program being compiled all at once, with interpretation the source code is transformed line by line; each line or statement is executed before immediately proceeding to processing the next line of the source code.

Modern JS engines actually employ numerous variations of both compilation and interpretation in the handling of JS programs.

### Compiling code
Scope is primarily determined during compilation, so understanding how compilation and execution relate is key in mastering scope.
In classic compiler theory, a program is processed by a compiler in three basic stages:

1. Tokenizing/Lexing: breaking up a string of characters into meaningful (to the language) chunks, called tokens. For instance, consider the program: var a = 2;. This program would likely be broken up into the following tokens: var, a, =, 2, and ;. Whitespace may or may not be persisted as a token, depending on whether it’s meaningful or not.

2. Parsing: taking a stream (array) of tokens and turning it into a tree of nested elements, which collectively represent the grammatical structure of the program. This is called an Abstract Syntax Tree (AST).
For example, the tree for var a = 2; might start with a top-level node called VariableDeclaration, with a child node called Identifier (whose value is a), and another child called AssignmentExpression which itself has a child called NumericLiteral (whose value is 2).

3. Code Generation: taking an AST and turning it into executable code. This part varies greatly depending on the language, the platform it’s targeting, and other factors.

JS engines don’t have the luxury of an abundance of time to perform their work and optimizations, because JS compilation doesn’t happen in a build step ahead of time, as with other languages. It usually must happen in mere microseconds (or less!) right before the code is executed. To ensure the fastest performance under these constraints, JS engines use all kinds of tricks.

### Syntax Errors from the Start
Consider this program:
```javascript
var greeting = "Hola";
console.log(greeting);
greeting = ."Hi"; // SyntaxError: unexpected token .
```
This program produces no output ("Hello" is not printed), but instead throws a SyntaxError about the unexpected . token right before the "Hi" string. Since the syntax error happens after the well-formed console.log(..) statement, if JS was executing top-down line by line, one would expect the "Hello" message being printed before the syntax error being thrown. That doesn’t happen. 
In fact, the only way the JS engine could know about the syntax error on the third line, before executing the first and second lines, is by the JS engine first parsing the entire program before any of it is executed.

### Early Errors
Next, consider:
```javascript
console.log("Howdy");
saySomething("Hello","Hi");
// Uncaught SyntaxError: Duplicate parameter name not
// allowed in this context
function saySomething(greeting,greeting) {
    "use strict";
    console.log(greeting);
}
```
The "Howdy" message is not printed, despite being a wellformed statement. Instead, just like the snippet in the previous section, the SyntaxError here is thrown before the program is executed. In this case, it’s because strict-mode (opted in for only the saySomething(..) function here) forbids, among many other things, functions to have duplicate parameter names; this has always been allowed in non-strict-mode.The error thrown is not a syntax error in the sense of being a malformed string of tokens (like ."Hi" prior), but in strict-mode is nonetheless required by the specification to be thrown as an “early error” before any execution begins.

### Hoisting
Finally, consider:

```javascript
function saySomething() {
    var greeting = "Hello";
    {
        greeting = "Howdy"; // error comes from here
        let greeting = "Hi";
        console.log(greeting);
    }
}
saySomething();
// ReferenceError: Cannot access 'greeting' before
// initialization
```
The noted ReferenceError occurs from the line with the statement greeting = "Howdy". What’s happening is that the greeting variable for that statement belongs to thedeclaration on the next line, let greeting = "Hi", rather than to the previous var greeting = "Hello" statement.
The only way the JS engine could know, at the line where the error is thrown, that the next statement would declare a block-scoped variable of the same name (greeting) is if the JS engine had already processed this code in an earlier pass, and already set up all the scopes and their variable associations. This processing of scopes and declarations can only accurately be accomplished by parsing the program before execution. 
The ReferenceError here technically comes from greeting = "Howdy" accessing the greeting variable too early, a conflict referred to as the Temporal Dead Zone (TDZ). Chapter 5 will cover this in more detail
### Compiler Speak
With awareness of the two-phase processing of a JS program (compile, then execute), let’s turn our attention to how the JS engine identifies variables and determines the scopes of a program as it is compiled. First, let’s examine a simple JS program to use for analysis over the next several chapters:

```javascript
var students = [
{ id: 14, name: "Kyle" },
{ id: 73, name: "Suzy" },
{ id: 112, name: "Frank" },
{ id: 6, name: "Sarah" }
];
function getStudentName(studentID) {
    for (let student of students) {
        if (student.id == studentID) {
            return student.name;
        }
    }
}
var nextStudent = getStudentName(73);
console.log(nextStudent);
// Suzy
```

Other than declarations, all occurrences of variables/identifiers in a program serve in one of two “roles”: either they’re the target of an assignment or they’re the source of a value. 
As you might guess from the “L” and the “R”, the acronyms mean “Left-Hand Side” and “Right-Hand Side”, as in left and right sides of an = assignment operator. However, assignment targets and sources don’t always literally appear on the left or right of an =, so it’s probably clearer to think in terms of target / source rather than left / right.) 
How do you know if a variable is a target? Check if there is a value that is being assigned to it; if so, it’s a target. If not, then the variable is a source. For the JS engine to properly handle a program’s variables, it must first label each occurrence of a variable as target or source. We’ll dig in now to how each role is determined.

### Targets
What makes a variable a target? Consider:
```javascript
students = [ // ..
```
This statement is clearly an assignment operation; remember, the var students part is handled entirely as a declaration at compile time, and is thus irrelevant during execution; we left it out for clarity and focus. Same with the nextStudent = getStudentName(73) statement.
But there are three other target assignment operations in the code that are perhaps less obvious. One of them:
```javascript
for (let student of students) {
```
That statement assigns a value to student for each iteration of the loop. Another target reference:
```javascript
getStudentName(73)
```
But how is that an assignment to a target? Look closely: the argument 73 is assigned to the parameter studentID.

### Sources
In for (let student of students), we said that student is a target, but students is a source reference. In the statement if (student.id == studentID), both student and studentID are source references. student is also a source reference in return student.name. In getStudentName(73), getStudentName is a source reference (which we hope resolves to a function reference value).
In console.log(nextStudent), console is a source reference, as is nextStudent.

## Lexical Scope

We’ve demonstrated that JS’s scope is determined at compile time; the term for this kind of scope is “lexical scope”. “Lexical” is associated with the “lexing” stage of compilation, as discussed earlier in this chapter. To narrow this chapter down to a useful conclusion, the key idea of “lexical scope” is that it’s controlled entirely by the placement of functions, blocks, and variable declarations, in relation to one another. 
If you place a variable declaration inside a function, the compiler handles this declaration as it’s parsing the function, and associates that declaration with the function’s scope. If a variable is block-scope declared (let / const), then it’s associated with the nearest enclosing { .. } block, rather than its enclosing function (as with var).
Furthermore, a reference (target or source role) for a variable must be resolved as coming from one of the scopes that are lexically available to it; otherwise the variable is said to be “undeclared” (which usually results in an error!). If the variable is not declared in the current scope, the next outer/enclosing scope will be consulted. This process of stepping out one level of scope nesting continues until either a matching variable declaration can be found, or the global scope is reached and there’s nowhere else to go.
It’s important to note that compilation doesn’t actually do anything in terms of reserving memory for scopes and variables. None of the program has been executed yet. Instead, compilation creates a map of all the lexical scopes that lays out what the program will need while it executes.
You can think of this plan as inserted code for use at runtime,which defines all the scopes (aka, “lexical environments”) and registers all the identifiers (variables) for each scope. In other words, while scopes are identified during compilation, they’re not actually created until runtime, each time a scope needs to run. In the next chapter, we’ll sketch out the conceptual foundations for lexical scope.