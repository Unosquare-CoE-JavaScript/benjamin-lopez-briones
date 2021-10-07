# Chapter 1: What Is JavaScript?
The name JavaScript is probably the most mistaken and misunderstood programming language name.
Why? Because this language was originally designed to appeal to an audience of mostly Java programmers, and because the word “script” was popular at the time to refer to lightweight programs. These lightweight “scripts” would be the first ones to embed inside of pages on this new thing called the web!

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
The term “paradigm” in programming language context refers to a broad (almost universal) mindset and approach to structuring code. Within a paradigm, there are myriad variations of style and form that distinguish programs.

Typical paradigm-level code categories include procedural, object-oriented (OO/classes), and functional (FP):

*Procedural style organizes code in a top-down, linear progression through a pre-determined set of operations, usually collected together in related units called procedures.
*OO style organizes code by collecting logic and data together into units called classes.
*FP style organizes code into functions (pure computations as opposed to procedures), and the adaptations of those functions as values.

Paradigms are neither right nor wrong. They’re orientations that guide and mold how programmers approach problems and solutions, how they structure and maintain their code.

But many languages also support code patterns that can come from, and even mix and match from, different paradigms. So called “multi-paradigm languages” offer ultimate flexibility. In some cases, a single program can even have two or more expressions of these paradigms sitting side by side. 
JavaScript is most definitely a multi-paradigm language. You can write procedural, class-oriented, or FP-style code.

## Backwards & Forwards
One of the most foundational principles that guides JavaScript is preservation of backwards compatibility.
Backwards compatibility means that once something is accepted as valid JS, there will not be a future change to the language that causes that code to become invalid JS.
Code written in 1995—however primitive or limited it may have been!—should still work today.

The idea is that JS developers can write code with confidence that their code won’t stop working unpredictably because a browser update is released. This makes the decision to choose JS for a program a more wise and safe investment, for years into the future.

There are some small exceptions to this rule. JS has had some backwards-incompatible changes, but TC39 is extremely cautious in doing so. They study existing code on the web (via browser data gathering) to estimate the impact of such breakage, and browsers ultimately decide and vote on whether they’re willing to take the heat from users for a very smallscale breakage weighed against the benefits of fixing or improving some aspect of the language for many more sites (and users).

Being forwards-compatible means that including a new addition to the language in a program would not cause that program to break if it were run in an older JS engine. JS is not forwards-compatible, despite many wishing such, and even incorrectly believing the myth that it is.
HTML and CSS, by contrast, are forwards-compatible but not backwards-compatible.

It may seem desirable for forwards-compatibility to be included in programming language design, but it’s generally impractical to do so. Markup (HTML) or styling (CSS) are declarative in nature, so it’s much easier to “skip over” unrecognized declarations with minimal impact to other recognized declarations.

## Jumping the Gaps
Since JS is not forwards-compatible, it means that there is always the potential for a gap between code that you can write that’s valid JS, and the oldest engine that your site or application needs to support. If you run a program that uses an ES2019 feature in an engine from 2016, you’re very likely to see the program break and crash.

For new and incompatible syntax, the solution is transpiling. Transpiling is a contrived and community-invented term to describe using a tool to convert the source code of a program from one form to another (but still as textual source code). 
Typically, forwards-compatibility problems related to syntax are solved by using a transpiler (the most common one being Babel (https://babeljs.io)) to convert from that newer JS syntax version to an equivalent older syntax.

It’s strongly recommended that developers use the latest version of JS so that their code is clean and communicates its ideas most effectively.

Developers should focus on writing the clean, new syntax forms, and let the tools take care of producing a forwardscompatible version of that code that is suitable to deploy and run on the oldest-supported JS engine environments.

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
This code uses an ES2019 feature, the finally(..) method on the promise prototype. If this code were used in a preES2019 environment, the finally(..) method would not exist, and an error would occur.

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

Transpilation and polyfilling are two highly effective techniques for addressing that gap between code that uses the latest stable features in the language and the old environments a site or application needs to still support.

##  What’s in an Interpretation?

For much of the history of programming languages, “interpreted” languages and “scripting” languages have been looked down on as inferior compared to their compiled counterparts. 
The reasons for this acrimony are numerous, including the perception that there is a lack of performance optimization, as well as dislike of certain language characteristics, such as scripting languages generally using dynamic typing instead of the “more mature” statically typed languages. 

Languages regarded as “compiled” usually produce a portable (binary) representation of the program that is distributed for execution later. Since we don’t really observe that kind of model with JS (we distribute the source code, not the binary form), many claim that disqualifies JS from the category.

The real reason it matters to have a clear picture on whether JS is interpreted or compiled relates to the nature of how errors are handled.
Historically, scripted or interpreted languages were executed in generally a top-down and line-by-line fashion; there’s typically not an initial pass through the program to process it before execution begins.

In scripted or interpreted languages, an error on line 5 of a program won’t be discovered until lines 1 through 4 have already executed.

Compare that to languages which do go through a processing step (typically, called parsing) before any execution occurs. In this processing model, an invalid command (such as broken syntax) on line 5 would be caught during the parsing phase, before any execution has begun, and none of the program would run. For catching syntax (or otherwise “static”) errors, generally it’s preferred to know about them ahead of any doomed partial execution.
So what do “parsed” languages have in common with “compiled” languages? First, all compiled languages are parsed. So a parsed language is quite a ways down the road toward being compiled already. In classic compilation theory, the last remaining step after parsing is code generation: producing an executable form.

In other words, parsed languages usually also perform code generation before execution, so it’s not that much of a stretch to say that, in spirit, they’re compiled languages.

JS is a parsed language. The parsed JS is converted to an optimized (binary) form, and that “code” is subsequently executed (Figure 2); the engine does not commonly switch back into line-by-line execution (like Figure 2) mode after it has finished all the hard work of parsing—most languages/engines wouldn’t, because that would be highly inefficient.
To be specific, this “compilation” produces a binary byte code (of sorts), which is then handed to the “JS virtual machine” to execute.

Another wrinkle is that JS engines can employ multiple passes of JIT (Just-In-Time) processing/optimization on the generated code (post parsing), which again could reasonably be labeled either “compilation” or “interpretation” depending on perspective.

Consider the entire flow of a JS source program:
1. After a program leaves a developer’s editor, it gets transpiled by Babel, then packed by Webpack (and perhaps
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

This subset is valid JS written in ways that are somewhat uncommon in normal coding, but which signal certain important typing information to the engine that allow it to make key optimizations. ASM.js was introduced as one way of addressing the pressures on the runtime performance of JS. 
But it’s important to note that ASM.js was never intended to be code that was authored by developers, but rather a representation of a program having been transpiled from another language (such as C), where these typing “annotations” were inserted automatically by the tooling.

WASM is similar to ASM.js in that its original intent was to provide a path for non-JS programs (C, etc.) to be converted to a form that could run in the JS engine.

WASM is a representation format more akin to Assembly (hence, its name) that can be processed by a JS engine by skipping the parsing/compilation that the JS engine normally does. The parsing/compilation of a WASM-targeted program happen ahead of time (AOT).

An initial motivation for WASM was clearly the potential performance improvements. While that continues to be a focus, WASM is additionally motivated by the desire to bring more parity for non-JS languages to the web platform.

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
Interestingly, if a file has strict mode turned on, the functionlevel strict mode pragmas are disallowed. So you have to pick
one or the other. The only valid reason to use a per-function approach to strict mode is when you are converting an existing non-strict mode program file and need to make the changes little by little over time.

Many have wondered if there would ever be a time when JS made strict mode the default? The answer is no. Remember backwards compatibility.

Virtually all transpiled code ends up in strict mode even if the original source code isn’t written as such. Most JS code in production has been transpiled, so that means most JS is already adhering to strict mode. It’s possible to undo that assumption, but you really have to go out of your way to do so, so it’s highly unlikely.

ES6 modules assume strict mode, so all code in such files is automatically defaulted to strict mode.

## Defined

JS is an implementation of the ECMAScript standard which is guided by the TC39 committee and hosted by ECMA.
JS is a multi-paradigm language, meaning the syntax and capabilities allow a developer to mix and match (and bend and reshape!) concepts from various major paradigms, such as procedural, object-oriented (OO/classes), and functional (FP). JS is a compiled language, meaning the tools (including the JS engine) process and verify a program (reporting any errors!) before it executes.

