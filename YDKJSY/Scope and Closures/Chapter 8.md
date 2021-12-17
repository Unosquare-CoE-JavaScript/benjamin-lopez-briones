# Chapter 8: The Module Pattern

We’ve examined every angle of lexical scope, from the breadth of the global scope down through nested block scopes, into the intricacies of the variable lifecycle. Then we leveraged lexical scope to understand the full power of closure.

## Encapsulation and Least Exposure (POLE)

Encapsulation is often cited as a principle of object-oriented (OO) programming, but it’s more fundamental and broadly applicable than that. The goal of encapsulation is the bundling or co-location of information (data) and behavior (functions) that together serve a common purpose.

Independent of any syntax or code mechanisms, the spirit of encapsulation can be realized in something as simple as using separate files to hold bits of the overall program with common purpose. If we bundle everything that powers a list  of search results into a single file called “search-list.js”, we’reencapsulating that part of the program.

The recent trend in modern front-end programming to organize applications around Component architecture pushes encapsulation even further. For many, it feels natural to consolidate everything that constitutes the search results list—even beyond code, including presentational markup and styling— into a single unit of program logic, something tangible we can interact with. And then we label that collection the “SearchList” component.

Another key goal is the control of visibility of certain aspects of the encapsulated data and functionality. Recall from Chapter 6 the least privilege principle (POLE), which seeks to defensively guard against various dangers of scope over-exposure; these affect both variables and functions. In JS, we most often implement visibility control through the mechanics of lexical scope.

The idea is to group alike program bits together, and selectively limit programmatic access to the parts we consider private details. What’s not considered private is then marked as public, accessible to the whole program.

The natural effect of this effort is better code organization. It’s easier to build and maintain software when we know where things are, with clear and obvious boundaries and connection points. It’s also easier to maintain quality if we avoid the pitfalls of over-exposed data and functionality. These are some of the main benefits of organizing JS programs into modules.

## What Is a Module?

A module is a collection of related data and functions (often referred to as methods in this context), characterized by a division between hidden private details and public accessibledetails, usually called the “public API.”

A module is also stateful: it maintains some information over time, along with functionality to access and update that information.

### Namespaces (Stateless Grouping)

If you group a set of related functions together, without data, then you don’t really have the expected encapsulation a module implies. The better term for this grouping of stateless functions is a namespace:

```javascript
// namespace, not module
var Utils = {
    cancelEvt(evt) {
        evt.preventDefault();
        evt.stopPropagation();
        evt.stopImmediatePropagation();
    },
    wait(ms) {
        return new Promise(function c(res){
            setTimeout(res,ms);
        });
    },
    isValidEmail(email) {
        return /[^@]+@[^@.]+\.[^@.]+/.test(email);
    }
};
```

Utils here is a useful collection of utilities, yet they’re all state-independent functions. Gathering functionality together is generally good practice, but that doesn’t make this a module. Rather, we’ve defined a Utils namespace and organized the functions under it. 

### Data Structures (Stateful Grouping)

Even if you bundle data and stateful functions together, if you’re not limiting the visibility of any of it, then you’re stopping short of the POLE aspect of encapsulation; it’s not particularly helpful to label that a module.
Consider:

```javascript
// data structure, not module
var Student = {
    records: [
        { id: 14, name: "Kyle", grade: 86 },
        { id: 73, name: "Suzy", grade: 87 },
        { id: 112, name: "Frank", grade: 75 },
        { id: 6, name: "Sarah", grade: 91 }
    ],
    getName(studentID) {
        var student = this.records.find(
            student => student.id == studentID
        );
        return student.name;
    }
};
Student.getName(73);
// Suzy
```

Since records is publicly accessible data, not hidden behind a public API, Student here isn’t really a module. Student does have the data-and-functionality aspect of encapsulation, but not the visibility-control aspect. It’s best to label this an instance of a data structure.

### Modules (Stateful Access Control)

To embody the full spirit of the module pattern, we not only need grouping and state, but also access control through visibility (private vs. public). Let’s turn Student from the previous section into a module.

We’ll start with a form I call the “classic module,” which was originally referred to as the “revealing module” when it first emerged in the early 2000s. Consider:

```javascript
var Student = (function defineStudent(){
    var records = [
        { id: 14, name: "Kyle", grade: 86 },
        { id: 73, name: "Suzy", grade: 87 },
        { id: 112, name: "Frank", grade: 75 },
        { id: 6, name: "Sarah", grade: 91 }
    ];

    var publicAPI = {
        getName
    };
    return publicAPI;
    
    // ************************
    function getName(studentID) {
        var student = records.find(
            student => student.id == studentID
        );
        return student.name;
    }
})();
Student.getName(73); // Suzy
```

Student is now an instance of a module. It features a public API with a single method: getName(..). This method is able to access the private hidden records data.

Notice that the instance of the module is created by the defineStudent() IIFE being executed. This IIFE returns an object (named publicAPI) that has a property on it referencing the inner getName(..) function. Naming the object publicAPI is stylistic preference on my part. The object can be named whatever you like (JS doesn’t care), or you can just return an object directly without assigning it to any internal named variable.

From the outside, Student.getName(..) invokes this exposed inner function, which maintains access to the inner records variable via closure.

You don’t have to return an object with a function as one of its properties. You could just return a function directly, in place of the object. That still satisfies all the core bits of a classic module.

By virtue of how lexical scope works, defining variables and functions inside your outer module definition function makes everything by default private. Only properties added to the public API object returned from the function will be exported for external public use.

The use of an IIFE implies that our program only ever needs a single central instance of the module, commonly referred to as a “singleton.” Indeed, this specific example is simple enough that there’s no obvious reason we’d need anything more than just one instance of the Student module.

### Module Factory (Multiple Instances)

```javascript
// factory function, not singleton IIFE
function defineStudent() {
        var records = [
            { id: 14, name: "Kyle", grade: 86 },
            { id: 73, name: "Suzy", grade: 87 },
            { id: 112, name: "Frank", grade: 75 },
            { id: 6, name: "Sarah", grade: 91 }
        ];

        var publicAPI = {
            getName
        };
        return publicAPI;
        // ************************
        function getName(studentID) var student = records.find(student => student.id == studentID);

        return student.name;
    }
}

var fullTime = defineStudent();
fullTime.getName(73); // Suzy
```

Rather than specifying defineStudent() as an IIFE, we just define it as a normal standalone function, which is commonly referred to in this context as a “module factory” function.

## Classic Module Definition

So to clarify what makes something a classic module:
- There must be an outer scope, typically from a module factory function running at least once.
- The module’s inner scope must have at least one piece of hidden information that represents state for the module.
- The module must return on its public API a reference to at least one function that has closure over the hidden module state (so that this state is actually preserved).

## Node CommonJS Modules

Unlike the classic module format described earlier, where you could bundle the module factory or IIFE alongside any other code including other modules, CommonJS modules are file-based; one module per file.

Let’s tweak our module example to adhere to that format:

```javascript
module.exports.getName = getName;

// ************************
var records = [
    { id: 14, name: "Kyle", grade: 86 },
    { id: 73, name: "Suzy", grade: 87 },
    { id: 112, name: "Frank", grade: 75 },
    { id: 6, name: "Sarah", grade: 91 }
];

function getName(studentID) {
    var student = records.find(student => student.id == studentID);
    return student.name;
}
```

The records and getName identifiers are in the top-level scope of this module, but that’s not the global scope. As such, everything here is by default private to the module.

To expose something on the public API of a CommonJS module, you add a property to the empty object provided as module.exports. In some older legacy code, you may run across references to just a bare exports, but for code clarity you should always fully qualify that reference with the module. prefix.

Some developers have the habit of replacing the default exports object, like this:

```javascript
// defining a new object for the API
module.exports = {
// ..exports..
};
```

There are some quirks with this approach, including unexpected behavior if multiple such modules circularly depend on each other. As such, I recommend against replacing the object. If you want to assign multiple exports at once, using object literal style definition, you can do this instead:

```javascript
Object.assign(module.exports,{
    // .. exports ..
});
```

What’s happening here is defining the { .. } object literal with your module’s public API specified, and then Object.assign(..) is performing a shallow copy of all those properties onto the existing module.exports object, instead of replacing it This is a nice balance of convenience and safer module behavior.

To include another module instance into your module/program, use Node’s require(..) method. Assuming this module is located at “path/to/student.js”, this is how we can access it:

```javascript
var Student = require("/path/to/student.js");
Student.getName(73);
// Suzy
```

Student now references the public API of our example module.

CommonJS modules behave as singleton instances, similar to the IIFE module definition style presented before. No matter how many times you require(..) the same module, you just get additional references to the single shared module instance.

To effectively access only part of the API, the typical approach looks like this:

```javascript
var getName = require("/path/to/student.js").getName;
// or alternately:
var { getName } = require("/path/to/student.js");
```

Similar to the classic module format, the publicly exported methods of a CommonJS module’s API hold closures over the internal module details. That’s how the module singleton state is maintained across the lifetime of your program.

## Modern ES Modules (ESM)

he ESM format shares several similarities with the CommonJS format. ESM is file-based, and module instances are singletons, with everything private by default. One notable difference is that ESM files are assumed to be strict-mode, without needing a "use strict" pragma at the top. There’s no way to define an ESM as non-strict-mode.

Instead of module.exports in CommonJS, ESM uses an export keyword to expose something on the public API of the module. The import keyword replaces the require(..) statement. Let’s adjust “students.js” to use the ESM format:

```javascript
export getName;
// ************************
var records = [
    { id: 14, name: "Kyle", grade: 86 },
    { id: 73, name: "Suzy", grade: 87 },
    { id: 112, name: "Frank", grade: 75 },
    { id: 6, name: "Sarah", grade: 91 }
];

function getName(studentID) {
    var student = records.find(student => student.id == studentID);
    return student.name;
}
```

The only change here is the export getName statement. As before, export statements can appear anywhere throughout the file, though export must be at the top-level scope; it cannot be inside any other block or function.

ESM offers a fair bit of variation on how the export statements can be specified. For example:

```javascript
export function getName(studentID) {
    // ..
}
```

Even though export appears before the function keyword here, this form is still a function declaration that also happens to be exported. That is, the getName identifier isfunction hoisted (see Chapter 5), so it’s available throughout the whole scope of the module.

Another allowed variation:

```javascript
export default function getName(studentID) {
    // ..
}
```

This is a so-called “default export”, which has different semantics from other exports. In essence, a “default export” is a shorthand for consumers of the module when they import, giving them a terser syntax when they only need this single default API member.
Non-default exports are referred to as “named exports.”

The import keyword—like export, it must be used only at the top level of an ESM outside of any blocks or functions— also has a number of variations in syntax. The first is referred to as “named import”:

```javascript
import { getName } from "/path/to/students.js";
getName(73); // Suzy
```

As you can see, this form imports only the specifically named public API members from a module (skipping anything not named explicitly), and it adds those identifiers to the top-level scope of the current module. This type of import is a familiar style to those used to package imports in languages like Java.

Multiple API members can be listed inside the { .. } set, separated with commas. A named import can also be renamed with the as keyword:

```javascript
import { getName as getStudentName } from "/path/to/students.js";
getStudentName(73); // Suzy
```

If getName is a “default export” of the module, we can import it like this:

```javascript
import getName from "/path/to/students.js";
getName(73); // Suzy
```

The only difference here is dropping the { } around the import binding. If you want to mix a default import with other named imports:

```javascript
import { default as getName, /* .. others .. */ } from "/path/to/students.js";
getName(73); // Suzy
```

By contrast, the other major variation on import is called “namespace import”:

```javascript
import * as Student from "/path/to/students.js";
Student.getName(73); // Suzy
```

As is likely obvious, the * imports everything exported to the API, default and named, and stores it all under the single namespace identifier as specified. This approach most closely matches the form of classic modules for most of JS’s history.