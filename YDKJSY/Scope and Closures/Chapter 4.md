# Chapter 4: Around the Global Scope

The global scope of a JS program is a rich topic, with much more utility and nuance than you would likely assume. This chapter first explores how the global scope is (still) useful and relevant to writing JS programs today, then looks at differences in where and how to access the global scope in different JS environments.
Fully understanding the global scope is critical in your mastery of using lexical scope to structure your programs.

## Why Global Scope?

First, if you’re directly using ES modules (not transpiling them into some other module-bundle format), these files are loaded individually by the JS environment. Each module then imports references to whichever other modules it needs to access. The separate module files cooperate with each other exclusively through these shared imports, without needing any shared outer scope.

Second, if you’re using a bundler in your build process, all the files are typically concatenated together before delivery to the browser and JS engine, which then only processes one big file. Even with all the pieces of the application co-located in a single file, some mechanism is necessary for each piece to register a name to be referred to by other pieces, as well as some facility for that access to occur.

In some build setups, the entire contents of the file are wrapped in a single enclosing scope, such as a wrapper function, universal module (UMD—see Appendix A), etc. 

Each piece can register itself for access from other pieces by way of local variables in that shared scope. For example:

```javascript
(function wrappingOuterScope(){
    var moduleOne = (function one(){
    // ..
    })();

    var moduleTwo = (function two(){
        // ..
        function callModuleOne() {
            moduleOne.someMethod();
        }
    // ..
    })();

})();
```

As shown, the moduleOne and moduleTwo local variables inside the wrappingOuterScope() function scope are declared so that these modules can access each other for their cooperation. While the scope of wrappingOuterScope() is a function and not the full environment global scope, it does act as a sort of “application-wide scope,” a bucket where all the top-level identifiers can be stored, though not in the real global scope.

And finally, the third way: whether a bundler tool is used for an application, or whether the (non-ES module) files are simply loaded in the browser individually (via <script></script> tags or other dynamic JS resource loading), if there is no single surrounding scope encompassing all these pieces, the global scope is the only way for them to cooperate with each other:

```javascript
var moduleOne = (function one(){
    // ..
})();
var moduleTwo = (function two(){
    // ..
    function callModuleOne() {
        moduleOne.someMethod();
    }
    // ..
})();
```

Here, since there is no surrounding function scope, these moduleOne and moduleTwo declarations are simply dropped into the global scope. This is effectively the same as if the files hadn’t been concatenated, but loaded separately.

If these files are loaded separately as normal standalone js files in a browser environment, each top-level variable declaration will end up as a global variable, since the global scope is the only shared resource between these two separate files—they’re independent programs, from the perspective of the JS engine.

The global scope is also where:

JS exposes its built-ins:
- primitives: undefined, null, Infinity, NaN.
- natives: Date(), Object(), String(), etc.
- global functions: eval(), parseInt(), etc.
- namespaces: Math, Atomics, JSON.
- friends of JS: Intl, WebAssembly.

The environment hosting the JS engine exposes its own built-ins:
- console (and its methods).
- the DOM (window, document, etc).
- timers (setTimeout(..), etc).
- web platform APIs: navigator, history, geolocation, WebRTC, etc.

These are just some of the many globals your programs will interact with.

## Where Exactly is this Global Scope?

It might seem obvious that the global scope is located in the outermost portion of a file; that is, not inside any function or other block. But it’s not quite as simple as that. Different JS environments handle the scopes of your programs, especially the global scope, differently. It’s quite common for JS developers to harbor misconceptions without even realizing it. 
### Browser “Window” 

With respect to treatment of the global scope, the most pure environment JS can be run in is as a standalone .js file loaded in a web page environment in a browser. I don’t mean “pure” as in nothing automatically added—lots may be added!— but rather in terms of minimal intrusion on the code or interference with its expected global scope behavior.
Consider this .js file:

```javascript
var studentName = "Kyle";
function hello() {
    console.log(`Hello, ${ studentName }!`);
}
hello();
// Hello, Kyle!
```

This code may be loaded in a web page environment using an inline <script> </script> tag, a <script src=..></script> script tag in the markup, or even a dynamically created <script></script> DOM element. In all three cases, the studentName and hello identifiers are declared in the global scope.

That means if you access the global object (commonly, window in the browser), you’ll find properties of those same names there:

```javascript
var studentName = "Kyle";
function hello() {
    console.log(`Hello, ${ window.studentName }!`);
}
window.hello();
// Hello, Kyle!
```

That’s the default behavior one would expect from a reading of the JS specification: the outer scope is the global scope and studentName is legitimately created as global variable.

### Globals Shadowing Globals

An unusual consequence of the difference between a global variable and a global property of the same name is that, within just the global scope itself, a global object property can be shadowed by a global variable:

```javascript
window.something = 42;
let something = "Kyle";
console.log(something);
// Kyle
console.log(window.something);
// 42
```

The let declaration adds a something global variable but not a global object property (see Chapter 3). The effect then is that the something lexical identifier shadows the something global object property.
It’s almost certainly a bad idea to create a divergence between the global object and the global scope. Readers of your code will almost certainly be tripped up.
A simple way to avoid this gotcha with global declarations: always use var for globals.

### DOM Globals

One surprising behavior in the global scope you may encounter with browser-based JS applications: a DOM element with an id attribute automatically creates a global variable that references it.

Consider this markup:

```html
<ul id="my-todo-list">
<li id="first">Write a book</li>
..
</ul>
```

And the JS for that page could include:
```javascript
first;
// <li id="first">..</li>
window["my-todo-list"];
// <ul id="my-todo-list">..</ul>
```

If the id value is a valid lexical name (like first), the lexical variable is created. If not, the only way to access that global is through the global object (window[..]). The auto-registration of all id-bearing DOM elements as global variables is an old legacy browser behavior that nevertheless must remain because so many old sites still rely

## Web Workers

Web Workers are a web platform extension on top of browser JS behavior, which allows a JS file to run in a completely separate thread (operating system wise) from the thread that’s running the main JS program. 

Since these Web Worker programs run on a separate thread, they’re restricted in their communications with the main application thread, to avoid/limit race conditions and other complications. Web Worker code does not have access to the DOM, for example. Some web APIs are, however, made available to the worker, such as navigator.

Since a Web Worker is treated as a wholly separate program, it does not share the global scope with the main JS program. 

However, the browser’s JS engine is still running the code, so we can expect similar purity of its global scope behavior. Since there is no DOM access, the window alias for the global scope doesn’t exist

In a Web Worker, the global object reference is typically made using self:

```javascript
var studentName = "Kyle";
let studentID = 42;
function hello() {
    console.log(`Hello, ${ self.studentName }!`);
}
self.hello();
// Hello, Kyle!
self.studentID;
// undefined
```

Just as with main JS programs, var and function declarations create mirrored properties on the global object (aka, self), where other declarations (let, etc) do not.

## ES Modules (ESM)

ES6 introduced first-class support for the module pattern (covered in Chapter 8). One of the most obvious impacts of using ESM is how it changes the behavior of the observably top-level scope in a file.

Recall this code snippet from earlier (which we’ll adjust to ESM format by using the export keyword):

```javascript
var studentName = "Kyle";
function hello() {
console.log(`Hello, ${ studentName }!`);
}
hello();
// Hello, Kyle!
export hello;
``` 

If that code is in a file that’s loaded as an ES module, it will still run exactly the same. However, the observable effects, from the overall application perspective, will be different. 

Despite being declared at the top level of the (module) file, in the outermost obvious scope, studentName and hello are not global variables. Instead, they are module-wide, or if you prefer, “module-global.” 

However, in a module there’s no implicit “module-wide scope object” for these top-level declarations to be added to as properties, as there is when declarations appear in the top-level of non-module JS files. This is not to say that global variables cannot exist or be accessed in such programs.

ESM encourages a minimization of reliance on the global scope, where you import whatever modules you may need for the current module to operate. As such, you less often see usage of the global scope or its global object.

## Global This

Reviewing the JS environments we’ve looked at so far, a program may or may not:

- Declare a global variable in the top-level scope with var or function declarations—or let, const, and class.
- Also add global variables declarations as properties of the global scope object if var or function are used for the declaration.
- Refer to the global scope object (for adding or retrieving global variables, as properties) with window, self, or global.

As of ES2020, JS has finally defined a standardized reference to the global scope object, called globalThis. So, subject to the recency of the JS engines your code runs in, you can use globalThis in place of any of those other approaches.
We could even attempt to define a cross-environment polyfill that’s safer across pre-globalThis JS environments, such as:

```javascript
const theGlobalScopeObject =
(typeof globalThis != "undefined") ? globalThis :
(typeof global != "undefined") ? global :
(typeof window != "undefined") ? window :
(typeof self != "undefined") ? self :
(new Function("return this"))();
```

## Globally Aware

The global scope is present and relevant in every JS program, even though modern patterns for organizing code into modules de-emphasizes much of the reliance on storing identifiers in that namespace. 

Still, as our code proliferates more and more beyond the confines of the browser, it’s especially important we have a solid grasp on the differences in how the global scope (and global scope object!) behave across different JS environments.