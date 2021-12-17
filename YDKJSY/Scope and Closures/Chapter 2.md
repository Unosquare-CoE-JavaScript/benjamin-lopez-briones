# Chapter 2: Illustrating Lexical Scope

To properly reason about our programs, it’s important to have a solid conceptual foundation of how scope works. If we rely on guesses and intuition, we may accidentally get the right answers some of the time, but many other times we’re far off. This isn’t a recipe for success.

Imagine you come across a pile of marbles, and notice that all the marbles are colored red, blue, or green. Let’s sort all the marbles, dropping the red ones into a red bucket, green into a green bucket, and blue into a blue bucket. After sorting, when you later need a green marble, you already know the green bucket is where to go to get it.

In this metaphor, the marbles are the variables in our program. The buckets are scopes (functions and blocks), which we just conceptually assign individual colors for our discussion purposes. The color of each marble is thus determined by which color scope we find the marble originally created in.

Let’s annotate the running program example from Chapter 1 with scope color labels:

```javascript
// outer/global scope: RED
var students = [
{ id: 14, name: "Kyle" },
{ id: 73, name: "Suzy" },
{ id: 112, name: "Frank" },
{ id: 6, name: "Sarah" }
];
function getStudentName(studentID) {
    // function scope: BLUE
    for (let student of students) {
        // loop scope: GREEN
        if (student.id == studentID) {
            return student.name;
        }
    }
}
var nextStudent = getStudentName(73);
console.log(nextStudent); // Suzy
```
We’ve designated three scope colors with code comments: RED (outermost global scope), BLUE (scope of function getStudentName(..)), and GREEN (scope of/inside the for loop). But it still may be difficult to recognize the boundaries of these scope buckets when looking at a code listing.

Scope bubbles are determined during compilation based on where the functions/blocks of scope are written, the nesting inside each other, and so on. Each scope bubble is entirely contained within its parent scope bubble—a scope is never partially in two different outer scopes. 
Each marble (variable/identifier) is colored based on which bubble (bucket) it’s declared in, not the color of the scope it may be accessed from.

As the JS engine processes a program (during compilation), and finds a declaration for a variable, it essentially asks, “Which color scope (bubble or bucket) am I currently in?” The variable is designated as that same color, meaning it belongs to that bucket/bubble. The GREEN(3) bucket is wholly nested inside of the BLUE(2) bucket, and similarly the BLUE(2) bucket is wholly nested inside the RED(1) bucket. Scopes can nest inside each other as shown, to any depth of nesting as your program needs. References (non-declarations) to variables/identifiers are allowed if there’s a matching declaration either in the current scope, or any scope above/outside the current scope, but not with declarations from lower/nested scopes. An expression in the RED(1) bucket only has access to RED(1) marbles, not BLUE(2) or GREEN(3). An expression in the BLUE(2) bucket can reference either BLUE(2) or RED(1) marbles, not GREEN(3). And an expression in the GREEN(3) bucket has access to RED(1), BLUE(2), and GREEN(3) marbles.

The key take-aways from marbles & buckets (and bubbles!): 
- Variables are declared in specific scopes, which can be thought of as colored marbles from matching-color buckets.
- Any variable reference that appears in the scope where it was declared, or appears in any deeper nested scopes, will be labeled a marble of that same color—unless an intervening scope “shadows” the variable declaration;
- The determination of colored buckets, and the marbles they contain, happens during compilation. This information is used for variable (marble color) “lookups” during code execution.

## Nested Scope
When it comes time to execute the getStudentName() function, Engine asks for a Scope Manager instance for that function’s scope, and it will then proceed to look up the parameter (studentID) to assign the 73 argument value to, and so on.
The function scope for getStudentName(..) is nested inside the global scope. The block scope of the for-loop is similarly nested inside that function scope. Scopes can be lexically nested to any arbitrary depth as the program defines. 
Each scope gets its own Scope Manager instance each time that scope is executed (one or more times). Each scope automatically has all its identifiers registered at the start of the scope being executed.

At the beginning of a scope, if any identifier came from a function declaration, that variable is automatically initialized to its associated function reference. And if any identifier came from a var declaration (as opposed to let/const), that variable is automatically initialized to undefined so that it can be used; otherwise, the variable remains uninitialized (aka, in its “TDZ”) and cannot be used until its full declaration-and-initialization are executed.
In the for (let student of students) { statement, students is a source reference that must be looked up. But how will that lookup be handled, since the scope of the function will not find such an identifier?
To explain, let’s imagine that bit of conversation playing out like this:

> Engine: Hey, Scope Manager (for the function), I have a source reference for students, ever heard of it?

> (Function) Scope Manager: Nope, never heard of it. Try the next outer scope.

> Engine: Hey, Scope Manager (for the global scope), I have a source reference for students, ever heard of it?

> (Global) Scope Manager: Yep, it was formally declared, here it is.

One of the key aspects of lexical scope is that any time an identifier reference cannot be found in the current scope, the next outer scope in the nesting is consulted; that process is repeated until an answer is found or there are no more scopes to consult.

## Lookup Failures
When Engine exhausts all lexically available scopes (moving outward) and still cannot resolve the lookup of an identifier, an error condition then exists. However, depending on the mode of the program (strict-mode or not) and the role of the variable (i.e., target vs. source;), this error condition will be handled differently.

## Undefined Mess
If the variable is a source, an unresolved identifier lookup is considered an undeclared (unknown, missing) variable, which always results in a ReferenceError being thrown. Also, if the variable is a target, and the code at that moment is running in strict-mode, the variable is considered undeclared and similarly throws a ReferenceError.
The error message for an undeclared variable condition, in most JS environments, will look like, “Reference Error: XYZ is not defined.” The phrase “not defined” seems almost identical to the word “undefined,” as far as the English language goes. But these two are very different in JS, and this error message unfortunately creates a persistent confusion.
“Not defined” really means “not declared”—or, rather, “undeclared,” as in a variable that has no matching formal declaration in any lexically available scope. By contrast, “undefined” really means a variable was found (declared), but the variable otherwise has no other value in it at the moment, so it defaults to the undefined value. 
To perpetuate the confusion even further, JS’s typeof operator returns the string "undefined" for variable references in either state:

```javascript
var studentName;
typeof studentName; // "undefined"
typeof doesntExist; // "undefined"
```

These two variable references are in very different conditions, but JS sure does muddy the waters. The terminology mess is confusing and terribly unfortunate. Unfortunately, JS developers just have to pay close attention to not mix up which kind of “undefined” they’re dealing with!

## Global… What!?

If the variable is a target and strict-mode is not in effect, a confusing and surprising legacy behavior kicks in. The troublesome outcome is that the global scope’s Scope Manager will just create an accidental global variable to fulfill that target assignment!
Consider:
```javascript
function getStudentName() {
    // assignment to an undeclared variable :(
    nextStudent = "Suzy";
}
getStudentName();
console.log(nextStudent);
// "Suzy" -- oops, an accidental-global variable!
```
Here’s how that conversation will proceed:
> Engine: Hey, Scope Manager (for the function), I have a target reference for nextStudent, ever heard of it?

> (Function) Scope Manager: Nope, never heard of it. Try the next outer scope.

> Engine: Hey, Scope Manager (for the global scope),

> I have a target reference for nextStudent, ever heard of it?

> (Global) Scope Manager: Nope, but since we’re in non-strict-mode, I helped you out and just created a global variable for you, here it is!

