# Chapter 5: The (Not So) Secret Lifecycle of Variables

Just knowing which scope a variable comes from is only part of the story. If a variable declaration appears past the first statement of a scope, how will any references to that identifier before the declaration behave? What happens if you try to declare the same variable twice in a scope?

JS’s particular flavor of lexical scope is rich with nuance in how and when variables come into existence and become available to the program.

## When Can I Use a Variable?

At what point does a variable become available to use within its scope? There may seem to be an obvious answer: after the variable has been declared/created. Right? Not quite.
Consider:

```javascript
greeting();
// Hello!
function greeting() {
    console.log("Hello!");
}
```

This code works fine. You may have seen or even written code like it before. But why can you access? the identifier greeting from line 1 (to retrieve and execute a function reference), even though the greeting() function declaration doesn’t occur until line 4?

Recall Chapter 1 points out that all identifiers are registered to their respective scopes during compile time. Moreover, every identifier is created at the beginning of the scope it belongs to, every time that scope is entered. The term most commonly used for a variable being visible from the beginning of its enclosing scope, even though its declaration may appear further down in the scope, is called hoisting. 

But hoisting alone doesn’t fully answer the question. We can see an identifier called greeting from the beginning of the scope, but why can we call the greeting() function before it’s been declared? In other words, how does the variable greeting have any value (the function reference) assigned to it, from the moment the scope starts running?

The answer is a special characteristic of formal function declarations, called function hoisting. When a function declaration’s name identifier is registered at the top of its scope, it’s additionally auto-initialized to that function’s reference. That’s why the function can be called throughout the entire scope!

One key detail is that both function hoisting and var-flavored variable hoisting attach their name identifiers to the nearest enclosing function scope (or, if none, the global scope), not a block scope.

## Hoisting: Declaration vs. Expression

Function hoisting only applies to formal function declarations (specifically those which appear outside of blocks—see “FiB” in Chapter 6), not to function expression assignments.

Consider:

```javascript
greeting();
// TypeError
var greeting = function greeting() {
    console.log("Hello!");
};
```

Line 1 (greeting();) throws an error. But the kind of error thrown is very important to notice. A TypeError means we’re trying to do something with a value that is not allowed. 

Notice that the error is not a ReferenceError. JS isn’t telling us that it couldn’t find greeting as an identifier in the scope. It’s telling us that greeting was found but doesn’t hold a function reference at that moment. Only functions can be  invoked, so attempting to invoke some non-function valueresults in an error.

But what does greeting hold, if not the function reference? In addition to being hoisted, variables declared with var are also automatically initialized to undefined at the beginning of their scope—again, the nearest enclosing function, or the global. Once initialized, they’re available to be used (assigned to, retrieved from, etc.) throughout the whole scope. So on that first line, greeting exists, but it holds only the default undefined value. It’s not until line 4 that greeting gets assigned the function reference.

Pay close attention to the distinction here. A function declaration is hoisted and initialized to its function value (again, called function hoisting). A var variable is also hoisted, and then auto-initialized to undefined. Any subsequent function expression assignments to that variable don’t happen until that assignment is processed during runtime execution. 
In both cases, the name of the identifier is hoisted. But the function reference association isn’t handled at initialization time (beginning of the scope) unless the identifier was created in a formal function declaration.

Variable Hoisting
Let’s look at another example of variable hoisting:

```javascript
greeting = "Hello!";
console.log(greeting);
// Hello!
var greeting = "Howdy!";
```

Though greeting isn’t declared until line 5, it’s available to be assigned to as early as line 1. Why?

There’s two necessary parts to the explanation:
- the identifier is hoisted,
- and it’s automatically initialized to the value undefined from the top of the scope.

## Re-declaration?

Consider:

```javascript
var studentName = "Frank";
console.log(studentName);
// Frank
var studentName;
console.log(studentName); // ???
```

What do you expect to be printed for that second message? Many believe the second var studentName has re-declared the variable (and thus “reset” it), so they expect undefined to be printed.
But is there such a thing as a variable being “re-declared” in the same scope? No.

If you consider this program from the perspective of the hoisting metaphor, the code would be re-arranged like this for execution purposes:

```javascript
var studentName;
var studentName; // clearly a pointless no-op!
studentName = "Frank";
console.log(studentName);
// Frank
console.log(studentName);
// Frank
```

Since hoisting is actually about registering a variable at the beginning of a scope, there’s nothing to be done in the middle of the scope where the original program actually had the second var studentName statement. It’s just a no-op(eration), a pointless statement.

A repeated var declaration of the same identifier name in a scope is effectively a do-nothing operation. Here’s another illustration, this time across a function of the same name:

```javascript
var greeting;
function greeting() {
    console.log("Hello!");
}
// basically, a no-op
var greeting;
typeof greeting; // "function"

var greeting = "Hello!";
typeof greeting; // "string"
```
The first greeting declaration registers the identifier to the scope, and because it’s a var the auto-initialization will be undefined. The function declaration doesn’t need to re-register the identifier, but because of function hoisting it overrides the auto-initialization to use the function reference.

The second var greeting by itself doesn’t do anything since greeting is already an identifier and function hoisting already took precedence for the auto-initialization.

Actually assigning "Hello!" to greeting changes its value from the initial function greeting() to the string; var itself doesn’t have any effect.

What about repeating a declaration within a scope using let or const?

```javascript
let studentName = "Frank";
console.log(studentName);
let studentName = "Suzy";
```

This program will not execute, but instead immediately throw a SyntaxError. Depending on your JS environment, the error message will indicate something like: “studentName has already been declared.” In other words, this is a case where attempted “re-declaration” is explicitly not allowed!

When Compiler asks Scope Manager about a declaration, if that identifier has already been declared, and if either/both declarations were made with let, an error is thrown. The intended signal to the developer is “Stop relying on sloppy re-declaration!”

## Constants?

The const keyword is more constrained than let. Like let, const cannot be repeated with the same identifier in the same scope. But there’s actually an overriding technical reason why that sort of “re-declaration” is disallowed, unlike let which disallows “re-declaration” mostly for stylistic reasons. The const keyword requires a variable to be initialized, so omitting an assignment from the declaration results in a
SyntaxError:

```javascript
const empty; // SyntaxError
```
const declarations create variables that cannot be re-assigned:

```javascript
const studentName = "Frank";
console.log(studentName);
// Frank
studentName = "Suzy"; // TypeError
```

The studentName variable cannot be re-assigned because it’s declared with a const. So if const declarations cannot be re-assigned, and const declarations always require assignments, then we have a clear technical reason why const must disallow any “re-declarations”: any const “re-declaration” would also necessarily be a const re-assignment, which can’t be allowed!

## Loops

So it’s clear from our previous discussion that JS doesn’t really want us to “re-declare” our variables within the same scope. That probably seems like a straightforward admonition, until you consider what it means for repeated execution of declaration statements in loops. Consider:

```javascript
var keepGoing = true;
while (keepGoing) {
    let value = Math.random();
    if (value > 0.5) {
        keepGoing = false;
    }
}
```

Is value being “re-declared” repeatedly in this program? Will we get errors thrown? No.

All the rules of scope (including “re-declaration” of letcreated variables) are applied per scope instance. In other words, each time a scope is entered during execution, everything resets. Each loop iteration is its own new scope instance, and within each scope instance, value is only being declared once.

So there’s no attempted “re-declaration,” and thus no error.

What about “re-declaration” with other loop forms, like for-loops?

```javascript
for (let i = 0; i < 3; i++) {
    let value = i * 10;
    console.log(`${ i }: ${ value }`);
}
// 0: 0
// 1: 10
// 2: 20
```

It should be clear that there’s only one value declared per scope instance. But what about i? Is it being “re-declared”? To answer that, consider what scope i is in. It might seem like it would be in the outer (in this case, global) scope, but it’s not. It’s in the scope of for-loop body, just like value is. In fact, you could sorta think about that loop in this more verbose equivalent form:

```javascript
{
// a fictional variable for illustration
    let $$i = 0;
    for ( /* nothing */; $$i < 3; $$i++) {
        // here's our actual loop `i`!
        let i = $$i;
        let value = i * 10;
        console.log(`${ i }: ${ value }`);
    }
// 0: 0
// 1: 10
// 2: 20
}
```

Now it should be clear: the i and value variables are both declared exactly once per scope instance. No “re-declaration” here.
What about other for-loop forms?

```javascript
for (let index in students) {
// this is fine
}
for (let student of students) {
// so is this
}
```

Same thing with for..in and for..of loops: the declared variable is treated as inside the loop body, and thus is handled per iteration (aka, per scope instance). No “re-declaration.”.

## Uninitialized Variables (aka, TDZ)

With var declarations, the variable is “hoisted” to the top of its scope. But it’s also automatically initialized to the undefined value, so that the variable can be used throughout the entire scope.

However, let and const declarations are not quite the same in this respect.
Consider:

```javascript
console.log(studentName);
// ReferenceError
let studentName = "Suzy";
```

The result of this program is that a ReferenceError is thrown on the first line.
That error message is quite indicative of what’s wrong: studentName exists on line 1, but it’s not been initialized, so it cannot be used yet. Let’s try this:

```javascript
studentName = "Suzy"; // let's try to initialize it!
// ReferenceError
console.log(studentName);
let studentName;
```

Oops. We still get the ReferenceError, but now on the first line where we’re trying to assign to (aka, initialize!) this so-called “uninitialized” variable studentName.

The real question is, how do we initialize an uninitialized variable? For let/const, the only way to do so is with an assignment attached to a declaration statement. An assignment by itself is insufficient! Consider:

```javascript
let studentName = "Suzy";
console.log(studentName); // Suzy
```

Here, we are initializing the studentName (in this case, to "Suzy" instead of undefined) by way of the let declaration statement form that’s coupled with an assignment.

Remember that we’ve asserted a few times so far that Compiler ends up removing any var/let/const declarators, replacing them with the instructions at the top of each scope to register the appropriate identifiers. 

So if we analyze what’s going on here, we see that an additional nuance is that Compiler is also adding an instruction in the middle of the program, at the point where the variable studentName was declared, to handle that declaration’s autoinitialization. We cannot use the variable at any point prior to that initialization occuring. The same goes for const as itdoes for let.

The term coined by TC39 to refer to this period of time from the entering of a scope to where the auto-initialization of thevariable occurs is: Temporal Dead Zone (TDZ).
The TDZ is the time window where a variable exists but is still uninitialized, and therefore cannot be accessed in any way.

There’s a common misconception that TDZ means let and const do not hoist. This is an inaccurate, or at least slightly misleading, claim. They definitely hoist.

The actual difference is that let/const declarations do not automatically initialize at the beginning of the scope, the way var does. The debate then is if the auto-initialization is part of hoisting, or not? I think auto-registration of a variable at the top of the scope (i.e., what I call “hoisting”) and auto-initialization at the top of the scope (to undefined) are distinct operations and shouldn’t be lumped together under the single term “hoisting.”

