# Chapter 1: What Is JavaScript?
The name JavaScript is probably the most mistaken and misunderstood programming language name.
Why? Because this language was originally designed to appeal to an audience of mostly Java programmers, and because the word “script” was popular at the time to refer to lightweight programs. These lightweight “scripts” would be the first ones to embed inside of pages on this new thing called the web!

JavaScript was a marketing ploy to try to position this language as a palatable alternative to writing the heavier and more well-known Java of the day.

There are some superficial resemblances between JavaScript’s code and Java code. Those similarities don’t particularly come from shared development, but from both languages targeting developers with assumed syntax expectations from C.

Oracle (via Sun), the company that still owns and runs Java, also owns the official trademark for the name “JavaScript”; For these reasons, some have suggested we use JS instead of JavaScript.

The official name of the language specified by TC39 and formalized by the ECMA standards body is ECMAScript.

## Language Specification
TC39, the technical steering committee that manages JS. Their primary task is managing the official specification for the language.
Contrary to some established and frustratingly perpetuated myth, there are not multiple versions of JavaScript in the wild. There’s just one JS, the official standard as maintained by TC39 and ECMA. Back in the early 2000s, when Microsoft maintained a forked and reverse-engineered (and not entirely compatible) version of JS called “JScript”.

All major browsers and device makers have committed to keeping their JS implementations compliant with this one central specification.

## The Web Rules Everything About (JS)
How JS is implemented for web browsers is, in all practicality, the only reality that matters. For example, TC39 planned to add a contains(..) method for Arrays, but it was found that this name conflicted with old JS frameworks still in use on some sites, so they changed the name to a non-conflicting includes(..). The same happened with a comedic/tragic JS community crisis dubbed “smooshgate,” where the planned flatten(..) method was eventually renamed flat(..).

## Not All (Web) JS…

Is this code a JS program?

```javascript
alert("Hello, JS!");
```

Depends on how you look at things. The alert(..) function shown here is not included in the JS specification, but it is in all web JS environments.

Various JS environments (like browser JS engines, Node.js, etc.) add APIs into the global scope of your JS programs that give you environment-specific capabilities, like being able to pop an alert-style box in the user’s browser. In fact, a wide range of JS-looking APIs, like fetch(..), getCurrentLocation(..), and getUserMedia(..), are all web APIs that look like JS.

Another common example is console.log(..) (and all the other console.* methods!). These are not specified in JS, but because of their universal utility are defined by pretty much every JS environment, according to a roughly agreed consensus.

So alert(..) and console.log(..) are not defined by JS.
But they look like JS.

## It’s Not Always JS

The developer console is not trying to pretend to be a JS compiler that handles your entered code exactly the same way the JS engine handles a .js file. It’s trying to make it easy for you to quickly enter a few lines of code and see the results immediately.

Don’t trust what behavior you see in a developer console as representing exact to-the-letter JS semantics; think of the console as a “JS-friendly” environment.

## Many Faces
The term “paradigm” in programming language context refers to a broad (almost universal) mindset and approach to structuring code. Within a paradigm, there are myriad variations of style and form that distinguish programs.

Typical paradigm-level code categories include procedural, object-oriented (OO/classes), and functional (FP):

*Procedural style organizes code in a top-down, linear progression through a pre-determined set of operations, usually collected together in related units called procedures.
*OO style organizes code by collecting logic and data together into units called classes.
*FP style organizes code into functions (pure computations as opposed to procedures), and the adaptations of those functions as values.

Paradigms are neither right nor wrong. They’re orientations that guide and mold how programmers approach problems and solutions, how they structure and maintain their code.

But many languages also support code patterns that can come from, and even mix and match from, different paradigms. So called “multi-paradigm languages” offer ultimate flexibility. In some cases, a single program can even have two or more expressions of these paradigms sitting side by side. 
JavaScript is most definitely a multi-paradigm language. You can write procedural, class-oriented, or FP-style code.

## Backwards & Forwards
One of the most foundational principles that guides JavaScript is preservation of backwards compatibility.
Backwards compatibility means that once something is accepted as valid JS, there will not be a future change to the language that causes that code to become invalid JS.
Code written in 1995—however primitive or limited it may have been!—should still work today.

The idea is that JS developers can write code with confidence that their code won’t stop working unpredictably because a browser update is released. This makes the decision to choose JS for a program a more wise and safe investment, for years into the future.

There are some small exceptions to this rule. JS has had some backwards-incompatible changes, but TC39 is extremely cautious in doing so. They study existing code on the web (via browser data gathering) to estimate the impact of such breakage, and browsers ultimately decide and vote on whether they’re willing to take the heat from users for a very smallscale breakage weighed against the benefits of fixing or improving some aspect of the language for many more sites (and users).

Being forwards-compatible means that including a new addition to the language in a program would not cause that program to break if it were run in an older JS engine. JS is not forwards-compatible, despite many wishing such, and even incorrectly believing the myth that it is.
HTML and CSS, by contrast, are forwards-compatible but not backwards-compatible.

It may seem desirable for forwards-compatibility to be included in programming language design, but it’s generally impractical to do so. Markup (HTML) or styling (CSS) are declarative in nature, so it’s much easier to “skip over” unrecognized declarations with minimal impact to other recognized declarations.

## Jumping the Gaps
Since JS is not forwards-compatible, it means that there is always the potential for a gap between code that you can write that’s valid JS, and the oldest engine that your site or application needs to support. If you run a program that uses an ES2019 feature in an engine from 2016, you’re very likely to see the program break and crash.

For new and incompatible syntax, the solution is transpiling. Transpiling is a contrived and community-invented term to describe using a tool to convert the source code of a program from one form to another (but still as textual source code). 
Typically, forwards-compatibility problems related to syntax are solved by using a transpiler (the most common one being Babel (https://babeljs.io)) to convert from that newer JS syntax version to an equivalent older syntax.

It’s strongly recommended that developers use the latest version of JS so that their code is clean and communicates its ideas most effectively.

Developers should focus on writing the clean, new syntax forms, and let the tools take care of producing a forwardscompatible version of that code that is suitable to deploy and run on the oldest-supported JS engine environments.

## Filling the Gaps
If the forwards-compatibility issue is not related to new syntax, but rather to a missing API method that was only
recently added, the most common solution is to provide a definition for that missing API method that stands in and
acts as if the older environment had already had it natively defined. This pattern is called a polyfill (aka “shim”).
Consider this code:
```javascript
// getSomeRecords() returns us a promise for some
// data it will fetch
var pr = getSomeRecords();
// show the UI spinner while we get the data
startSpinner();
pr
.then(renderRecords) // render if successful
.catch(showError) // show an error if not
.finally(hideSpinner) // always hide the spinner
```
This code uses an ES2019 feature, the finally(..) method on the promise prototype. If this code were used in a preES2019 environment, the finally(..) method would not exist, and an error would occur.

A polyfill for finally(..) in pre-ES2019 environments could look like this:

```javascript
if (!Promise.prototype.finally) {
    Promise.prototype.finally = function f(fn){
        return this.then(
            function t(v){
                return Promise.resolve( fn() )
                .then(function t(){
                    return v;
                });
            },
            function c(e){
                return Promise.resolve( fn() )
                .then(function t(){
                    throw e;
                });
            }
        );
    };
}
```

The if statement protects the polyfill definition by preventing it from running in any environment where the JS engine has already defined that method.

Transpilers like Babel typically detect which polyfills your code needs and provide them automatically for you.
Always write code using the most appropriate features to communicate its ideas and intent effectively. In general, this means using the most recent stable JS version.

Transpilation and polyfilling are two highly effective techniques for addressing that gap between code that uses the latest stable features in the language and the old environments a site or application needs to still support.

##  What’s in an Interpretation?

For much of the history of programming languages, “interpreted” languages and “scripting” languages have been looked down on as inferior compared to their compiled counterparts. 
The reasons for this acrimony are numerous, including the perception that there is a lack of performance optimization, as well as dislike of certain language characteristics, such as scripting languages generally using dynamic typing instead of the “more mature” statically typed languages. 

Languages regarded as “compiled” usually produce a portable (binary) representation of the program that is distributed for execution later. Since we don’t really observe that kind of model with JS (we distribute the source code, not the binary form), many claim that disqualifies JS from the category.

The real reason it matters to have a clear picture on whether JS is interpreted or compiled relates to the nature of how errors are handled.
Historically, scripted or interpreted languages were executed in generally a top-down and line-by-line fashion; there’s typically not an initial pass through the program to process it before execution begins.

In scripted or interpreted languages, an error on line 5 of a program won’t be discovered until lines 1 through 4 have already executed.

Compare that to languages which do go through a processing step (typically, called parsing) before any execution occurs. In this processing model, an invalid command (such as broken syntax) on line 5 would be caught during the parsing phase, before any execution has begun, and none of the program would run. For catching syntax (or otherwise “static”) errors, generally it’s preferred to know about them ahead of any doomed partial execution.
So what do “parsed” languages have in common with “compiled” languages? First, all compiled languages are parsed. So a parsed language is quite a ways down the road toward being compiled already. In classic compilation theory, the last remaining step after parsing is code generation: producing an executable form.

In other words, parsed languages usually also perform code generation before execution, so it’s not that much of a stretch to say that, in spirit, they’re compiled languages.

JS is a parsed language. The parsed JS is converted to an optimized (binary) form, and that “code” is subsequently executed (Figure 2); the engine does not commonly switch back into line-by-line execution (like Figure 2) mode after it has finished all the hard work of parsing—most languages/engines wouldn’t, because that would be highly inefficient.
To be specific, this “compilation” produces a binary byte code (of sorts), which is then handed to the “JS virtual machine” to execute.

Another wrinkle is that JS engines can employ multiple passes of JIT (Just-In-Time) processing/optimization on the generated code (post parsing), which again could reasonably be labeled either “compilation” or “interpretation” depending on perspective.

Consider the entire flow of a JS source program:
1. After a program leaves a developer’s editor, it gets transpiled by Babel, then packed by Webpack (and perhaps
half a dozen other build processes), then it gets delivered
in that very different form to a JS engine.
2. The JS engine parses the code to an AST.
3. Then the engine converts that AST to a kind-of byte
code, a binary intermediate representation (IR), which
is then refined/converted even further by the optimizing
JIT compiler.
4. Finally, the JS VM executes the program.

Since JS is compiled, we are informed of static errors (such as malformed syntax) before our code is executed.

##  Web Assembly (WASM)

This subset is valid JS written in ways that are somewhat uncommon in normal coding, but which signal certain important typing information to the engine that allow it to make key optimizations. ASM.js was introduced as one way of addressing the pressures on the runtime performance of JS. 
But it’s important to note that ASM.js was never intended to be code that was authored by developers, but rather a representation of a program having been transpiled from another language (such as C), where these typing “annotations” were inserted automatically by the tooling.

WASM is similar to ASM.js in that its original intent was to provide a path for non-JS programs (C, etc.) to be converted to a form that could run in the JS engine.

WASM is a representation format more akin to Assembly (hence, its name) that can be processed by a JS engine by skipping the parsing/compilation that the JS engine normally does. The parsing/compilation of a WASM-targeted program happen ahead of time (AOT).

An initial motivation for WASM was clearly the potential performance improvements. While that continues to be a focus, WASM is additionally motivated by the desire to bring more parity for non-JS languages to the web platform.

In other words, WASM relieves the pressure to add features to JS that are mostly/exclusively intended to be used by transpiled programs from other languages.

Ironically, even though WASM runs in the JS engine, the JS language is one of the least suitable languages to source WASM programs with, because WASM relies heavily on static typing information. Even TypeScript (TS)—ostensibly, JS + static types—is not quite suitable (as it stands) to transpile to WASM, though language variants like AssemblyScript are attempting to bridge the gap between JS/TS and WASM.

##  Strictly Speaking
Back in 2009 with the release of ES5, JS added strict mode as an opt-in mechanism for encouraging better JS programs.

Strict mode shouldn’t be thought of as a restriction on what you can’t do, but rather as a guide to the best way to do things so that the JS engine has the best chance of optimizing and efficiently running the code.

Most strict mode controls are in the form of early errors, meaning errors that aren’t strictly syntax errors but are still thrown at compile time (before the code is run). For example, strict mode disallows naming two function parameters the same, and results in an early error. Some other strict mode controls are only observable at runtime, such as how this defaults to undefined instead of the global object.

Strict mode is like a linter reminding you how JS should be written to have the highest quality and best chance at performance. If you find yourself feeling handcuffed, trying to work around strict mode, that should be a blaring red warning flag that you need to back up and rethink the whole approach.

Something to be aware of is that even a stray ; all by itself appearing before the strict mode pragma will render the pragma useless; no errors are thrown because it’s valid JS to have a string literal expression in a statement position, but it also will silently not turn on strict mode!

Strict mode can alternatively be turned on per-function scope.
```javascript
function someOperations() {
// whitespace and comments are fine here
"use strict";
// all this code will run in strict mode
}
```
Interestingly, if a file has strict mode turned on, the functionlevel strict mode pragmas are disallowed. So you have to pick
one or the other. The only valid reason to use a per-function approach to strict mode is when you are converting an existing non-strict mode program file and need to make the changes little by little over time.

Many have wondered if there would ever be a time when JS made strict mode the default? The answer is no. Remember backwards compatibility.

Virtually all transpiled code ends up in strict mode even if the original source code isn’t written as such. Most JS code in production has been transpiled, so that means most JS is already adhering to strict mode. It’s possible to undo that assumption, but you really have to go out of your way to do so, so it’s highly unlikely.

ES6 modules assume strict mode, so all code in such files is automatically defaulted to strict mode.

## Defined

JS is an implementation of the ECMAScript standard which is guided by the TC39 committee and hosted by ECMA.
JS is a multi-paradigm language, meaning the syntax and capabilities allow a developer to mix and match (and bend and reshape!) concepts from various major paradigms, such as procedural, object-oriented (OO/classes), and functional (FP). JS is a compiled language, meaning the tools (including the JS engine) process and verify a program (reporting any errors!) before it executes.

# Chapter 2: Surveying JS

## Each File is a Program

Almost every website (web application) you use is comprised of many different JS files (typically with the .js file extension). It’s tempting to think of the whole thing (the application) as one program. But JS sees it differently.
In JS, each standalone file is its own separate program. 
The reason this matters is primarily around error handling. Since JS treats files as programs, one file may fail (during parse/compile or execution) and that will not necessarily prevent the next file from being processed.

It’s important to ensure that each file works properly, and that to whatever extent possible, they handle failure in other files as gracefully as possible.

Many projects use build process tools that end up combining separate files from the project into a single file to be delivered to a web page. When this happens, JS treats this single combined file as the entire program. The only way multiple standalone .js files act as a single program is by sharing their state via the “global scope.” They mix together in this global scope namespace, so at runtime they act as as whole. 

Since ES6, JS has also supported a module format in addition to the typical standalone JS program format. Modules are also file-based. If a file is loaded via module-loading mechanism such as an import statement or a <script type=module> tag, all its code is treated as a single module.

JS does in fact treat each module separately. Similar to how “global scope” allows standalone files to mix together at runtime, importing a module into another allows runtime interoperation between them.

Regardless of which code organization pattern is used for a file  you should still think of each file as its own (mini) program, which may then cooperate with other (mini) programs.

## Values
The most fundamental unit of information in a program is a value. Values are data. They’re how the program maintains state. Values come in two forms in JS: primitive and object.

Values are embedded in programs using literals:
```javascript
    greeting("My name is Kyle.");
```

In this program, the value "My name is Kyle." is a primitive string literal; strings are ordered collections of characters, the choice of which quote character is entirely stylistic.

Another option to delimit a string literal is to use the backtick ` character. However, this choice is not merely stylistic; there’s a behavioral difference as well. 

Consider:
```javascript
console.log("My name is ${ firstName }.");
// My name is ${ firstName }.
console.log('My name is ${ firstName }.');
// My name is ${ firstName }.
console.log(`My name is ${ firstName }.`);
// My name is Kyle.
```
Assuming this program has already defined a variable firstName with the string value "Kyle", the \`-delimited string then resolves the variable expression (indicated with ${ .. }) to its current value. This is called interpolation, reserve \` only for strings that will include interpolated expressions. Other than strings, JS programs often contain other primitive literal values such as booleans and numbers:
```javascript
while (false) {
    console.log(3.141592);
}
```
while represents a loop type, a way to repeat operations while its condition is true.

Another variation on numbers is the bigint (big-integer) primitive type. Numbers are most often used in programs for counting steps, such as loop iterations, and accessing information in numeric positions (i.e., an array index).

```javascript
console.log(`My name is ${ names[1] }.`);
// My name is Kyle.
```

We used 1 for the element in the second position, instead of 2, because like in most programming languages, JS array indices are 0-based (0 is the first position). In addition to strings, numbers, and booleans, two other primitive values in JS programs are null and undefined. While there are differences between them (some historic and some contemporary), for the most part both values serve the purpose of indicating emptiness (or absence) of a value.

However, it’s safest and best to use only undefined as the single empty value, even though null seems attractive in that it’s shorter to type!

```javascript
while (value != undefined) {
    console.log("Still got something!");
}
```

The final primitive value to be aware of is a symbol, which is a special-purpose value that behaves as a hidden unguessable value. Symbols are almost exclusively used as special keys on objects:

```javascript
hitchhikersGuide[ Symbol("meaning of life") ];
// 42
```
You won’t encounter direct usage of symbols very often in typical JS programs. They’re mostly used in low-level code such as in libraries and frameworks.

## Arrays And Objects
Besides primitives, the other value type in JS is an object value. A rrays are a special type of object that’s comprised of an ordered and numerically indexed list of data:

```javascript
names = [ "Frank", "Kyle", "Peter", "Susan" ];
names.length;
// 4
names[0];
// Frank
names[1];
// Kyle J
```

JS arrays can hold any value type, either primitive or object (including other arrays). Even functions are values that can be held in arrays or objects. Functions, like arrays, are a special kind (aka, sub-type) of object.

Objects are more general: an unordered, keyed collection of any various values. In other words, you access the element by a string location name (aka “key” or “property”) rather than by its numeric position (as with arrays). For example:

```javascript
name = {
    first: "Kyle",
    last: "Simpson",
    age: 39,
    specialties: [ "JS", "Table Tennis" ]
};

console.log(`My name is ${ name.first }.`);
```

## Value Type Determination
For distinguishing values, the typeof operator tells you its built-in type, if primitive, or "object" otherwise:
```javascript
typeof 42; // "number"
typeof "abc"; // "string"
typeof true; // "boolean"
typeof undefined; // "undefined"
typeof null; // "object" -- oops, bug!
typeof { "a": 1 }; // "object"
typeof [1,2,3]; // "object"
typeof function hello(){}; // "function"
```

typeof null unfortunately returns "object" instead of the expected "null".

Converting from one value type to another, such as from string to number, is referred to in JS as “coercion". Primitive values and object values behave differently when they’re assigned or passed around.

## Declaring and Using Variables 
To be explicit about something that may not have been obvious in the previous section: in JS programs, values can either appear as literal values , or they can be held in variables.

```javascript
var name = "Kyle";
var age;
```

The var keyword declares a variable to be used in that part of the program, and optionally allows initial value assignment.
Another similar keyword is let:

```javascript
let name = "Kyle";
let age;
```

The let keyword has some differences to var, with the most obvious being that let allows a more limited access to the variable than var. This is called “block scoping” as opposed to regular or function scoping.
Consider:

```javascript
var adult = true;
if (adult) {
    var name = "Kyle";
    let age = 39;
    console.log("Shhh, this is a secret!");
}
console.log(name);
// Kyle
console.log(age);
// Error!
```
The attempt to access age outside of the if statement results in an error, because age was block-scoped to the if, whereas name was not. 
Block-scoping is very useful for limiting how widespread variable declarations are in our programs. But var is still useful in that it communicates “this variable will be seen by a wider scope”.

A third declaration form is const. It’s like let but has an additional limitation that it must be given a value at the moment it’s declared, and cannot be re-assigned a different value later.
Consider:

```javascript
const myBirthday = true;
let age = 39;
if (myBirthday) {
    age = age + 1; // OK!
    myBirthday = false; // Error!
}
```

Const declared variables are not “unchangeable”, they just cannot be re-assigned. The best semantic use of a const is when you have a simple primitive value that you want to give a useful name to, such as using myBirthday instead of true. This makes programs easier to read.

Besides var / let / const, there are other syntactic forms that declare identifiers (variables) in various scopes. For example:
```javascript
function hello(name) {
    console.log(`Hello, ${ name }.`);
}
hello("Kyle");
// Hello, Kyle.
```

The identifier hello is created in the outer scope, and it’s also automatically associated so that it references the function. But the named parameter name is created only inside the function, and thus is only accessible inside that function’s scope.

## Functions
The word “function” has a variety of meanings in programming. For example, in the world of Functional Programming, “function” has a precise mathematical definition and implies a strict set of rules to abide by. 
In JS, we should consider “function” to take the broader meaning of another related term: “procedure.” A procedure is a collection of statements that can be invoked one or more times, may be provided some inputs, and may give back one or more outputs.
From the early days of JS, function definition looked like:
```javascript
function awesomeFunction(coolThings) {
    // ..
    return amazingStuff;
}
```
This is called a function declaration because it appears as a statement by itself, not as an expression in another statement. In contrast to a function declaration statement, a function expression can be defined and assigned like this:

```javascript
// let awesomeFunction = ..
// const awesomeFunction = ..
var awesomeFunction = function(coolThings) {
// ..
return amazingStuff;
};
```

This function is an expression that is assigned to the variable awesomeFunction. Different from the function declaration form, a function expression is not associated with its identifier until that statement during runtime.
It’s extremely important to note that in JS, functions are values that can be assigned. Not all languages treat functions as values, but it’s essential for a language to support the functional programming pattern, as JS does.

Functions also can return values using the return keyword:
```javascript
function greeting(myName) {
    return `Hello, ${ myName }!`;
}
var msg = greeting("Kyle");
console.log(msg); // Hello, Kyle!
```
You can only return a single value, but if you have more values to return, you can wrap them up into a single object/array.
Since functions are values, they can be assigned as properties on objects:

```javascript
var whatToSay = {
    greeting() {
        console.log("Hello!");
    },
    question() {
        console.log("What's your name?");
    },
    answer() {
        console.log("My name is Kyle.");
    }
};
whatToSay.greeting();
// Hello!
```

## Comparisons
Making decisions in programs requires comparing values to determine their identity and relationship to each other. JS has several mechanisms to enable value comparison.

### Equal…ish
The most common comparison in JS programs asks the question, “Is this X value the same as that Y value?”.
Sometimes an equality comparison intends exact matching, but other times the desired comparison is a bit broader, allowing closely similar or interchangeable matching. In other words, we must be aware of the nuanced differences between an equality comparison and an equivalence comparison.

Yes, most values participating in an === equality comparison will fit with that exact same intuition. Consider some examples:

```javascript
3 === 3.0; // true
"yes" === "yes"; // true
null === null; // true
false === false; // true
42 === "42"; // false
"hello" === "Hello"; // false
true === 1; // false
0 === null; // false
"" === null; // false
null === undefined; // false
```
Another way ===’s equality comparison is often described is, “checking both the value and the type”. All value comparisons in JS consider the type of the values being compared. . Specifically, === disallows any sort of type conversion (aka, “coercion”) in its comparison, where other JS comparisons do allow coercion.

But the === operator does have some nuance to it, a fact many JS developers gloss over, to their detriment. The === operator is designed to lie in two cases of special values: NaN and -0.
Consider:

```javascript
NaN === NaN; // false
0 === -0; // true
```

In the case of NaN, the === operator lies and says that an occurrence of NaN is not equal to another NaN. In the case of -0 (yes, this is a real, distinct value you can use intentionally in your programs!), the === operator lies and says it’s equal to the regular 0 value. Since the lying about such comparisons can be bothersome, it’s best to avoid using === for them. For NaN comparisons, use the Number.isNaN(..) utility, which does not lie. For -0 comparison, use the Object.is(..) utility.

The story gets even more complicated when we consider comparisons of object values (non-primitives). Consider:

```javascript
[ 1, 2, 3 ] === [ 1, 2, 3 ]; // false
{ a: 42 } === { a: 42 } // false
(x => x * 2) === (x => x * 2) // false
```

It may seem reasonable to assume that an equality check considers the nature orcontents of the value. But when it comes to objects, a content-aware comparison is generally referred to as “structural equality.” JS does not define === as structural equality for object values.

Instead, === uses identity equality for object values. In JS, all object values are held by reference, are assigned and passed by reference-copy, are compared by reference (identity) equality.
Consider:

```javascript
var x = [ 1, 2, 3 ];
// assignment is by reference-copy, so
// y references the *same* array as x,
// not another copy of it.
var y = x;
y === x; // true
y === [ 1, 2, 3 ]; // false
x === [ 1, 2, 3 ]; // false
```
In this snippet, y === x is true because both variables hold a reference to the same initial array.

The array structure and contents don’t matter in this comparison, only the reference identity. JS does not provide a mechanism for structural equality comparison of object values, only reference identity comparison.

### Coercive Comparisons
Coercion means a value of one type being converted to its respective representation in another type (to whatever extent possible). The == operator performs an equality comparison similarly to how the === performs it.

And if the comparison is between the same value type, both == and === do exactly the same thing, no difference whatsoever. If the value types being compared are different, the == differs from === in that it allows coercion before the comparison.

Consider:
```javascript
42 == "42"; // true
1 == true; // true
```

In both comparisons, the value types are different, so the == causes the non-number values ("42" and true) to be converted to numbers (42 and 1, respectively) before the comparisons are made.

There’s a pretty good chance that you’ll use relational comparison operators like <, > (and even <= and >=). Just like ==, these operators will perform as if they’re “strict” if the types being relationally compared already match, but they’ll allow coercion first (generally, to numbers) if the types differ.
Consider:

```javascript
var arr = [ "1", "10", "100", "1000" ];
for (let i = 0; i < arr.length && arr[i] < 500; i++) {
    // will run 3 times
}
```

The i < arr.length comparison is “safe” from coercion because i and arr.length are always numbers. The arr[i] < 500 invokes coercion, though, because the arr[i] values are all strings.

These relational operators typically use numeric comparisons, except in the case where both values being compared are already strings; in this case, they use alphabetical (dictionarylike) comparison of the strings:

```javascript
var x = "10";
var y = "9";
x < y; // true, watch out!
```

There’s no way to get these relational operators to avoid coercion, other than to just never use mismatched types in the comparisons. The wiser approach is not to avoid coercive comparisons, but to embrace and learn their ins and outs.

## How We Organize in JS
Two major patterns for organizing code (data and behavior) are used broadly across the JS ecosystem: classes and modules. These patterns are not mutually exclusive; many programs can and do use both. Being proficient in JS requires understanding both patterns and where they are appropriate (and not!).

### Classes
The terms “object-oriented,” “class-oriented,” and “classes” are all very loaded full of detail and nuance

A class in a program is a definition of a “type” of custom data structure that includes both data and behaviors that operate on that data. Classes define how such a data structure works, but classes are not themselves concrete values.

Consider:
```javascript
class Page {
    constructor(text) {
        this.text = text;
    }
    print() {
        console.log(this.text);
    }
}
class Notebook {
    constructor() {
        this.pages = [];
    }
    addPage(text) {
        var page = new Page(text);
        this.pages.push(page);
    }
    print() {
        for (let page of this.pages) {
            page.print();
        }
    }
}
var mathNotes = new Notebook();
mathNotes.addPage("Arithmetic: + - * / ...");
mathNotes.addPage("Trigonometry: sin cos tan ...");
mathNotes.print();
// ..
```
In the Page class, the data is a string of text stored in a this.text member property. The behavior is print(), a method that dumps the text to the console. For the Notebook class, the data is an array of Page instances. The behavior is addPage(..), a method that instantiates new Page pages and adds them to the list, as well as print().

The class mechanism allows packaging data (text and pages) to be organized together with their behaviors (e.g., addPage(..) and print()). The same program could have been built without any class definitions, but it would likely have been much less organized, harder to read and reason about, and more susceptible to bugs and subpar maintenance.

### Class Inheritance
Another aspect inherent to traditional “class-oriented” design, though a bit less commonly used in JS, is “inheritance” (and “polymorphism”).

```javascript
class Publication {
    constructor(title,author,pubDate) {
        this.title = title;
        this.author = author;
        this.pubDate = pubDate;
    }
    print() {
        console.log(`
            Title: ${ this.title }
            By: ${ this.author }
            ${ this.pubDate }
            `);
    }
}
```
This Publication class defines a set of common behavior that any publication might need. Now let’s consider more specific types of publication, like Book and BlogPost:

```javascript
class Book extends Publication {
    constructor(bookDetails) {
        super(
            bookDetails.title,
            bookDetails.author,
            bookDetails.publishedOn
        );
        this.publisher = bookDetails.publisher;
        this.ISBN = bookDetails.ISBN;
    }
    print() {
        super.print();
        console.log(`
            Publisher: ${ this.publisher }
            ISBN: ${ this.ISBN }
        `);
    }
}
```

Book use the extends clause to extend the general definition of Publication to include additional behavior. The super(..) call in each constructor delegates to the parent Publication class’s constructor for its initialization work, and then they do more specific things according to their respective publication type.

Now consider using these child classes:
```javascript
var YDKJS = new Book({
    title: "You Don't Know JS",
    author: "Kyle Simpson",
    publishedOn: "June 2014",
    publisher: "O'Reilly",
    ISBN: "123456-789"
});
YDKJS.print();
// Title: You Don't Know JS
// By: Kyle Simpson
// June 2014
// Publisher: O'Reilly
// ISBN: 123456-789
```

Notice that both child class instances have a print() method, which was an override of the inherited print() method from the parent Publication class. Each of those overridden childclass print() methods call super.print() to invoke the inherited version of the print() method.
The fact that both the inherited and overridden methods can have the same name and co-exist is called polymorphism.

## Modules
The module pattern has essentially the same goal as the class pattern, which is to group data and behavior together into logical units. Also like classes, modules can “include” or “access” the data and behaviors of other modules, for cooperation sake.
But modules have some important differences from classes.

### Classic Modules
From the early days of JS, modules was an important and common pattern that was leveraged in countless JS programs. 
The key hallmarks of a classic module are an outer function (that runs at least once), which returns an “instance” of the module with one or more functions exposed that can operate on the module instance’s internal (hidden) data. Because a module of this form is just a function, and calling it produces an “instance” of the module, another description for these functions is “module factories”.
Consider the classic module form of the earlier Publication, Book.

```javascript
function Publication(title,author,pubDate) {
    var publicAPI = {
        print() {
            console.log(`
                Title: ${ title }
                By: ${ author }
                ${ pubDate }
            `);
        }
    };
    return publicAPI;
}
function Book(bookDetails) {
    var pub = Publication(
        bookDetails.title,
        bookDetails.author,
        bookDetails.publishedOn
    );
    var publicAPI = {
        print() {
            pub.print();
            console.log(`
                Publisher: ${ bookDetails.publisher }
                ISBN: ${ bookDetails.ISBN }
            `);
        }
    };
    return publicAPI;
}
```
Comparing these forms to the class forms, there are more similarities than differences. The class form stores methods and data on an object instance, which must be accessed with the this. prefix. With modules, the methods and data are accessed as identifier variables in scope, without any this. prefix. With class, the “API” of an instance is implicit in the class definition—also, all data and methods are public. 
With the module factory function, you explicitly create and return an object with any publicly exposed methods, and any data or other unreferenced methods remain private inside the factory function.
There are other variations to this factory function form AMD (Asynchronous Module Definition), UMD (Universal Module Definition), and CommonJS (classic Node.js-style modules).

Consider also the usage (aka, “instantiation”) of these module factory functions:

```javascript
var YDKJS = Book({
    title: "You Don't Know JS",
    author: "Kyle Simpson",
    publishedOn: "June 2014",
    publisher: "O'Reilly",
    ISBN: "123456-789"
});
YDKJS.print();
// Title: You Don't Know JS
// By: Kyle Simpson
// June 2014
// Publisher: O'Reilly
// ISBN: 123456-789
```

The only observable difference here is the lack of using new, calling the module factories as normal functions.

### ES Modules
ES modules (ESM), introduced to the JS language in ES6, are meant to serve much the same spirit and purpose as the existing classic modules just described, especially taking into account important variations and use cases from AMD, UMD, and CommonJS. The implementation approach does, however, differ significantly. 
First, there’s no wrapping function to define a module. The wrapping context is a file. ESMs are always file-based; onefile, one module.
Second, you don’t interact with a module’s “API” explicitly, but rather use the export keyword to add a variable or method to its public API definition. If something is defined in a module but not exported, then it stays hidden.
Third, you just import it to use its single instance. ESMs are, in effect, “singletons,” in that there’s only one instance ever created, at first import in your program, and all other imports just receive a reference to that same single instance.

In our running example, we do assume multiple-instantiation, so these following snippets will mix both ESM and classic modules.

Consider the file publication.js:

```javascript
function printDetails(title,author,pubDate) {
    console.log(`
        Title: ${ title }
        By: ${ author }
        ${ pubDate }
    `);
}
export function create(title,author,pubDate) {
    var publicAPI = {
        print() {
            printDetails(title,author,pubDate);
        }
    };
    return publicAPI;
}
```

To import and use this module, from another ES module like blogpost.js:

```javascript
import { create as createPub } from "publication.js";
function printDetails(pub,URL) {
    pub.print();
    console.log(URL);
}
export function create(title,author,pubDate,URL) {
    var pub = createPub(title,author,pubDate);
    var publicAPI = {
        print() {
            printDetails(pub,URL);
        }
    };
    return publicAPI;
}
```
And finally, to use this module, we import into another ES module like main.js:

```javascript
import { create as newBlogPost } from "blogpost.js";

var forAgainstLet = newBlogPost(
    "For and against let",
    "Kyle Simpson",
    "October 27, 2014",
    "https://davidwalsh.name/for-and-against-let"
);

forAgainstLet.print();
// Title: For and against let
// By: Kyle Simpson
// October 27, 2014
// https://davidwalsh.name/for-and-against-let
```

As shown, ES modules can use classic modules internally if they need to support multiple-instantiation.

#Chapter 3: Digging to the Roots of JS

This chapter digs much deeper than you’re likely used to thinking about a programming language.

## Iteration

Since programs are essentially built to process data (and make decisions on that data), the patterns used to step through the data have a big impact on the program’s readability.
The iterator pattern has been around for decades, and suggests a “standardized” approach to consuming data from a source one chunk at a time. The idea is that it’s more common and helpful to iterate the data source—to progressively handle the collection of data by processing the first part, then the next, and so on, rather than handling the entire set all at once.

The iterator pattern defines a data structure called an “iterator” that has a reference to an underlying data source which exposes a method like next(). Calling next() returns the next piece of data.

You don’t always know how many pieces of data that you will need to iterate through, so the pattern typically indicates completion by some special value or exception once you iterate through the entire set and go past the end.
The importance of the iterator pattern is in adhering to a standard way of processing data iteratively, ES6 standardized a specific protocol for the iterator pattern directly in the language. The protocol defines a next() method whose return is an object called an iterator result; the object has value and done properties, where done is a boolean that is false until the iteration over the underlying data source is complete.

### Consuming Iterators
With the ES6 iteration protocol in place, it’s workable to consume a data source one value at a time, checking after each next() call for done to be true to stop the iteration. But this approach is rather manual, so ES6 also included several mechanisms (syntax and APIs) for standardized consumption of these iterators.

One such mechanism is the for..of loop:

```javascript
// given an iterator of some data source:
var it = /* .. */;
// loop over its results one at a time
for (let val of it) {
    console.log(`Iterator value: ${ val }`);
}
// Iterator value: ..
// Iterator value: ..
// ..
```

Another mechanism that’s often used for consuming iterators is the ... operator. This operator actually has two symmetrical forms: spread and rest. The spread form is an iterator-consumer.

To spread an iterator, you have to have something to spread it into. There are two possibilities in JS: an array or an argument list for a function call.

An array spread:

```javascript
// spread an iterator into an array,
// with each iterated value occupying
// an array element position.
var vals = [ ...it ];
```

A function call spread:

```javascript
// spread an iterator into a function,
// call with each iterated value
// occupying an argument position.
doSomethingUseful( ...it );
```

In both cases, the iterator-spread form of ... follows the iterator-consumption protocol (the same as the for..of loop) to retrieve all available values from an iterator.

## Iterables
The iterator-consumption protocol is technically defined for consuming iterables. The protocol automatically creates an iterator instance from an iterable, and consumes just that iterator instance to its completion. This means a single iterable could be consumed more than once; each time, a new iterator instance would be created and used.

ES6 defined the basic data structure/collection types in JS as iterables. This includes strings, arrays, maps, sets, and others.

Consider:

```javascript
// an array is an iterable
var arr = [ 10, 20, 30 ];
for (let val of arr) {
    console.log(`Array value: ${ val }`);
}
// Array value: 10
// Array value: 20
// Array value: 30
```

Since arrays are iterables, we can shallow-copy an array using iterator consumption via the ... spread operator:

```javascript
var arrCopy = [ ...arr ];
```

A Map data structure uses objects as keys, associating a value (of any type) with that object. Maps have a different default iteration than seen here, in that the iteration is not just over the map’s values but instead its entries. An entry is a tuple (2-element array) including both a key and a value.

Consider:

```javascript
// given two DOM elements, `btn1` and `btn2`
var buttonNames = new Map();
buttonNames.set(btn1,"Button 1");
buttonNames.set(btn2,"Button 2");
for (let [btn,btnName] of buttonNames) {
    btn.addEventListener("click",function onClick(){
        console.log(`Clicked ${ btnName }`);
    });
}
```

In the for..of loop over the default map iteration, we use the [btn,btnName] syntax (called “array destructuring”) to break down each consumed tuple into the respective key-value pairs.

For the most part, all built-in iterables in JS have three iterator forms available: keys-only (keys()), values-only (values()), and entries (entries()). Beyond just using built-in iterables, you can also ensure your own data structures adhere to the iteration protocol.

## Closure
Closure is when a function remembers and continues to access variables from outside its scope, even when the function is executed in a different scope.

Perhaps without realizing it, almost every JS developer has made use of closure. In fact, closure is one of the most pervasive programming functionalities across a majority of languages. It might even be as important to understand as variables or loops; that’s how fundamental it is. We need to be able to recognize where closure is used in programs.

We see two definitional characteristics here. First, closure is part of the nature of a function. Objects don’t get closures, functions do. Second, to observe a closure, you must execute a function in a different scope than where that function was originally defined.

Consider:
```javascript
function greeting(msg) {
return function who(name) {
console.log(`${ msg }, ${ name }!`);
};
}
var hello = greeting("Hello");
var howdy = greeting("Howdy");
hello("Kyle");
// Hello, Kyle!
hello("Sarah");
// Hello, Sarah!
howdy("Grant");
// Howdy, Grant!
```

First, the greeting(..) outer function is executed, creating an instance of the inner function who(..); that function closes over the variable msg, which is the parameter from the outer scope of greeting(..). When that inner function is returned, its reference is assigned to the hello variable in the outer scope. Then we call greeting(..) a second time, creating a new inner function instance, with a new closure over a new msg, and return that reference to be assigned to howdy.

When the greeting(..) function finishes running, normally we would expect all of its variables to be garbage collected (removed from memory). We’d expect each msg to go away, but they don’t. The reason is closure. Since the inner function instances are still alive (assigned to hello and howdy, respectively), their closures are still preserving the msg variables. These closures are not a snapshot of the msg variable’s value; they are a direct link and preservation of the variable itself. That means closure can actually observe (or make!) updates to these variables over time. Closure is most common when working with asynchronous code, such as with callbacks. 

Consider:

```javascript
function getSomeData(url) {
    ajax(url,function onResponse(resp){
        console.log(
            `Response (from ${ url }): ${ resp }`
        );
    });
}
getSomeData("https://some.url/wherever");
// Response (from https://some.url/wherever): ...
```
The inner function onResponse(..) is closed over url, and thus preserves and remembers it until the Ajax call returns and executes onResponse(..). Even though getSomeData(..) finishes right away, the url parameter variable is kept alive in the closure for as long as needed.
It’s not necessary that the outer scope be a function—it usually is, but not always—just that there be at least one variable in an outer scope accessed from an inner function:

```javascript
for (let [idx,btn] of buttons.entries()) {
    btn.addEventListener("click",function onClick(){
        console.log(`Clicked on button (${ idx })!`);
    });
}
```

Because this loop is using let declarations, each iteration gets new block-scoped (aka, local) idx and btn variables.
Closure is one of the most prevalent and important programming patterns in any language. But that’s especially true of JS; it’s hard to imagine doing anything useful without leveraging closure in one way or another.

## this Keyword
One of JS’s most powerful mechanisms is also one of its most misunderstood: the this keyword. One common misconception is that a function’s this refers to the function itself. Because of how this works in other languages, another  misconception is that this points the instance that a methodbelongs to. Both are incorrect.

As discussed previously, when a function is defined, it is attached to its enclosing scope via closure. Scope is the set of rules that controls how references to variables are resolved.But functions also have another characteristic besides their scope that influences what they can access. This characteristic is best described as an execution context, and it’s exposed to the function via its this keyword. 
Scope is static and contains a fixed set of variables available at the moment and location you define a function

this is not a fixed characteristic of a function based on the function’s definition, but rather a dynamic characteristic that’s determined each time the function is called. One way to think about the execution context is that it’s a tangible object whose properties are made available to a function while it executes. Compare that to scope, which  can also be thought of as an object; except, the scope object is hidden inside the JS engine, it’s always the same for that function, and its properties take the form of identifiervariables available inside the function.

```javascript
function classroom(teacher) {
    return function study() {
        console.log(
            `${ teacher } says to study ${ this.topic }`
        );
    };
}
var assignment = classroom("Kyle");
```

The outer classroom(..) function makes no reference to a this keyword, so it’s just like any other function we’ve seen so far. But the inner study() function does reference this, which makes it a this-aware function. In other words, it’s a function that is dependent on its execution context.

The inner study() function returned by classroom("Kyle") is assigned to a variable called assignment. So how can assignment() (aka study()) be called?

```javascript
assignment();
// Kyle says to study undefined -- Oops :(
```

In this snippet, we call assignment() as a plain, normal function, without providing it any execution context. Since this program is not in strict mode, context-aware functions that are called without any context specified default the context to the global object (window in the browser).

The benefit of this-aware functions—and their dynamic context—is the ability to more flexibly re-use a single function with data from different objects. A function that closes over a scope can never reference a different scope or set of variables. But a function that has dynamic this context awareness can be quite helpful for certain tasks.

## Prototypes

Where this is a characteristic of function execution, a prototype is a characteristic of an object, and specifically resolution of a property access. Think about a prototype as a linkage between two objects; the linkage is hidden behind the scenes, though there are ways to expose and observe it. This prototype linkage occurs when an object is created.

A series of objects linked together via prototypes is called the “prototype chain.” The purpose of this prototype linkage  is so that accesses against B for properties/methods that B does not have, are delegated to A to handle. Delegation of property/method access allows two (or more!) objects to cooperate with each other to perform a task.

Consider defining an object as a normal literal:

```javascript
var homework = {
    topic: "JS"
};
```

The homework object only has a single property on it: topic. However, its default prototype linkage connects to the Object.prototype object, which has common built-in methods on it like toString() and valueOf(), among others.
We can observe this prototype linkage delegation from homework to Object.prototype: 

```javascript
homework.toString(); // [object Object] 
```

homework.toString() works even though homework doesn’t have a toString() method defined.

## Object Linkage
To define an object prototype linkage, you can create the object using the Object.create(..) utility:

```javascript
var homework = {
    topic: "JS"
};
var otherHomework = Object.create(homework);
otherHomework.topic; // "JS"
```

The first argument to Object.create(..) specifies an object to link the newly created object to, and then returns the newly created (and linked!) object.
Delegation through the prototype chain only applies for accesses to lookup the value in a property. If you assign to a property of an object, that will apply directly to the object regardless of where that object is prototype linked to. Object.create(null) creates an object that is not prototype linked anywhere, so it’s purely just a standalone object.

## this Revisited
We covered the this keyword earlier, but its true importance shines when considering how it powers prototype-delegated function calls. Indeed, one of the main reasons this supports dynamic context based on how the function is called is so that method calls on objects which delegate through the prototype chain still maintain the expected this.
Consider:

```javascript
var homework = {
    study() {
        console.log(`Please study ${ this.topic }`);
    }
};
var jsHomework = Object.create(homework);
jsHomework.topic = "JS";
jsHomework.study();
// Please study JS
var mathHomework = Object.create(homework);
mathHomework.topic = "Math";
mathHomework.study();
// Please study Math 
```

The two objects jsHomework and mathHomework each prototype link to the single homework object, which has the study() function. jsHomework and mathHomework are each given their own topic property.

jsHomework.study() delegates to homework.study(), but its this (this.topic) for that execution resolves to jsHomework because of how the function is called, so this.topic is "JS". Similarly for mathHomework.study() delegating to homework.study() but still resolving this to mathHomework, and thus this.topic as "Math". The preceding code snippet would be far less useful if this was resolved to homework. Yet, in many other languages, it would seem this would be homework because the study() method is indeed defined on homework. Unlike many other languages, JS’s this being dynamic is a critical component of allowing prototype delegation.

# Chapter 4: The Bigger Picture

## Pillar 1: Scope and Closure

The organization of variables into units of scope (functions, blocks) is one of the most foundational characteristics of any language; Scopes are like buckets, and variables are like marbles you put into those buckets. The scope model of a language is like the rules that help you determine which color marbles go in which matching-color buckets.

Scopes nest inside each other, only variables at that level of scope nesting, or in higher/outer scopes, are accessible; variables from lower/inner scopes are hidden and inaccessible. This is how scopes behave in most languages, which is called lexical scope.

JS is lexically scoped, though many claim it isn’t, because of two particular characteristics of its model that are not present in other lexically scoped languages. The first is commonly called hoisting: when all variables declared anywhere in a scope are treated as if they’re declared at the beginning of the scope. The other is that var-declared variables are function scoped, even if they appear inside a block.

let/const declarations have a peculiar error behavior called the “Temporal Dead Zone” (TDZ) which results in observable but unusable
variables. Though TDZ can be strange to encounter, it’s also not an invalidation of lexical scoping.

Closure is a natural result of lexical scope when the language has functions as first-class values, as JS does. When a function makes reference to variables from an outer scope, and that function is passed around as a value and executed in other scopes, it maintains access to its original scope variables; this is closure.

## Pillar 2: Prototypes
The second pillar of the language is the prototypes system.
JS is one of very few languages where you have the option to create objects directly and explicitly, without first defining their structure in a class.

Classes are just one pattern you can build on top of such power. But another approach, in a very different direction, is to simply embrace objects as objects, forget classes altogether, and let objects cooperate through the prototype chain. This is called behavior delegation. And the rest goes to functional programming (FP), as the sort of “anticlass” way of designing programs.

## Pillar 3: Types and Coercion
The third pillar of JS is by far the most overlooked part of JS’s nature. The vast majority of developers have strong misconceptions about how types work in programming languages, and especially how they work in JS.

Arguably, this pillar is more important than the other two, in the sense that no JS program will do anything useful if it doesn’t properly leverage JS’s value types, as well as the conversion (coercion) of values between types.

