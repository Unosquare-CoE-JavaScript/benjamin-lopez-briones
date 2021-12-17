# Chapter 6: Limiting Scope Exposure

## Least Exposure

Software engineering articulates a fundamental discipline, typically applied to software security, called “The Principle of Least Privilege” (POLP). ¹ And a variation of this principle that applies to our current discussion is typically labeled as “Least Exposure” (POLE).

POLP expresses a defensive posture to software architecture: components of the system should be designed to function with least privilege, least access, least exposure. If each piece is connected with minimum-necessary capabilities, the overall system is stronger from a security standpoint, because a compromise or failure of one piece has a minimized impact on the rest of the system.

If POLP focuses on system-level component design, the POLE Exposure variant focuses on a lower level; we’ll apply it to how scopes interact with each other.

In following POLE, what do we want to minimize the exposure of? Simply: the variables registered in each scope. Think of it this way: why shouldn’t you just place all the variables of your program out in the global scope? That probably immediately feels like a bad idea, but it’s worth considering why that is. When variables used by one part of the program are exposed to another part of the program, via scope, there are three main hazards that often arise:

- Naming Collisions: if you use a common and useful variable/function name in two different parts of the program, but the identifier comes from one shared scope (like the global scope), then name collision occurs, and it’s very likely that bugs will occur as one part uses the variable/function in a way the other part doesn’t expect. For example, imagine if all your loops used a single global i index variable, and then it happens that one loop in a function is running during an iteration of a loop from another function, and now the shared i variable gets an unexpected value.

- Unexpected Behavior: if you expose variables/functions whose usage is otherwise private to a piece of the program, it allows other developers to use them in ways you didn’t intend, which can violate expected behavior and cause bugs.

- Unintended Dependency: if you expose variables/functions unnecessarily, it invites other developers to use and depend on those otherwise private pieces. While that doesn’t break your program today, it creates a refactoring hazard in the future, because now you cannot as easily refactor that variable or function without potentially breaking other parts of the software that you don’t control.

POLE, as applied to variable/function scoping, essentially says, default to exposing the bare minimum necessary, keeping everything else as private as possible. Declare variables in as small and deeply nested of scopes as possible, rather than placing everything in the global (or even outer function) scope.

Consider:

```javascript
function diff(x,y) {
    if (x > y) {
        let tmp = x;
        x = y;
        y = tmp;
    }
    return y - x;
}
diff(3,7); // 4
diff(7,5); // 2
```

In this diff(..) function, we want to ensure that y is greater than or equal to x, so that when we subtract (y - x), the result is 0 or larger. If x is initially larger (the result would be negative!), we swap x and y using a tmp variable, to keep the result positive.

In this simple example, it doesn’t seem to matter whether tmp is inside the if block or whether it belongs at the function level—it certainly shouldn’t be a global variable! However, following the POLE principle, tmp should be as hidden in scope as possible. So we block scope tmp (using let) to the if block.

### Hiding in Plain (Function) Scope

What about hiding var or function declarations in scopes? That can easily be done by wrapping a function scope around a declaration. Let’s consider an example where function scoping can be useful.

The mathematical operation “factorial” (notated as “6!”) is the multiplication of a given integer against all successively lower integers down to 1—actually, you can stop at 2 since multiplying 1 does nothing. In other words, “6!” is the same as “6 * 5!”, which is the same as “6 * 5 * 4!”, and so on. Because of the nature of the math involved, once any given integer’s factorial (like “4!”) has been calculated, we shouldn’t need to do that work again, as it’ll always be the same answer.

So if you naively calculate factorial for 6, then later want to calculate factorial for 7, you might unnecessarily re-calculate the factorials of all the integers from 2 up to 6. If you’re willing to trade memory for speed, you can solve that wasted computation by caching each integer’s factorial as it’s calculated:

```javascript
var cache = {};
function factorial(x) {
    if (x < 2) return 1;
    if (!(x in cache)) {
        cache[x] = x * factorial(x - 1);
    }
    return cache[x];
}
factorial(6);
// 720
cache;
// {
// "2": 2,
// "3": 6,
// "4": 24,
// "5": 120,
// "6": 720
// }
factorial(7);
// 5040
```

We’re storing all the computed factorials in cache so that across multiple calls to factorial(..), the previous computations remain. But the cache variable is pretty obviously a private detail of how factorial(..) works, not something that should be exposed in an outer scope—especially not the global scope.

However, fixing this over-exposure issue is not as simple as hiding the cache variable inside factorial(..), as it might seem. Since we need cache to survive multiple calls, it must be located in a scope outside that function. So what can we do?

Define another middle scope (between the outer/global scope and the inside of factorial(..)) for cache to be located:

```javascript
// outer/global scope
function hideTheCache() {
    // "middle scope", where we hide `cache`
    var cache = {};
    return factorial;
    // **********************
    function factorial(x) {
        // inner scope
        if (x < 2) return 1;
        if (!(x in cache)) {
            cache[x] = x * factorial(x - 1);
        }
        return cache[x];
    }
}
var factorial = hideTheCache();
factorial(6);
// 720
factorial(7);
// 5040
```

The hideTheCache() function serves no other purpose than to create a scope for cache to persist in across multiple calls to factorial(..). But for factorial(..) to have access to cache, we have to define factorial(..) inside that same scope. Then we return the function reference, as a value from hideTheCache(), and store it in an outer scope variable, also named factorial. Now as we call factorial(..) (multiple times), its persistent cache stays hidden ye accessible only to factorial(...).

### Invoking Function Expressions Immediately

Notice that we surrounded the entire function expression in a set of ( .. ), and then on the end, we added that second () parentheses set; that’s actually calling the function expression we just defined. Moreover, in this case, the first set of surrounding ( .. ) around the function expression is not strictly necessary, but we used them for readability sake anyway.

So, in other words, we’re defining a function expression that’s then immediately invoked. This common pattern has a name: Immediately Invoked Function Expression (IIFE). An IIFE is useful when we want to create a scope to hide variables/functions. Since it’s an expression, it can be used in any place in a JS program where an expression is allowed.

For comparison, here’s an example of a standalone IIFE:

```javascript
// outer scope
(function(){
// inner hidden scope
})();
// more outer scope
```

Unlike earlier with hideTheCache(), where the outer surrounding (..) were noted as being an optional stylistic choice, for a standalone IIFE they’re required; they distinguish the function as an expression, not a statement. For consistency, however, always surround an IIFE function with ( .. ).

## Function Boundaries

Beware that using an IIFE to define a scope can have some unintended consequences, depending on the code around it. Because an IIFE is a full function, the function boundary alters the behavior of certain statements/constructs.

For example, a return statement in some piece of code would change its meaning if an IIFE is wrapped around it, because now the return would refer to the IIFE’s function. Non-arrow function IIFEs also change the binding of a this keyword—more on that in the Objects & Classes book. And statements like break and continue won’t operate across an IIFE function boundary to control an outer loop or block.

## Scoping with Blocks

You should by this point feel fairly comfortable with the merits of creating scopes to limit identifier exposure.

So far, we looked at doing this via function (i.e., IIFE) scope. But let’s now consider using let declarations with nested blocks. In general, any { .. } curly-brace pair which is a statement will act as a block, but not necessarily as a scope.

A block only becomes a scope if necessary, to contain its block-scoped declarations (i.e., let or const). Consider:

```javascript
{
    // not necessarily a scope (yet)
    // ..
    // now we know the block needs to be a scope
    let thisIsNowAScope = true;
    for (let i = 0; i < 5; i++) {
        // this is also a scope, activated each
        // iteration

        if (i % 2 == 0) {
            // this is just a block, not a scope
            console.log(i);
        }
    }
}
// 0 2 
```

Not all { .. } curly-brace pairs create blocks (and thus are eligible to become scopes):

- Object literals use { .. } curly-brace pairs to delimit their key-value lists, but such object values are not scopes.
- class uses { .. } curly-braces around its body definition, but this is not a block or scope.
- A function uses { .. } around its body, but this is not technically a block—it’s a single statement for the function body. It is, however, a (function) scope.
- The { .. } curly-brace pair on a switch statement (around the set of case clauses) does not define a block/scope.

Other than such non-block examples, a { .. } curly-brace pair can define a block attached to a statement (like an if or for), or stand alone by itself—see the outermost { .. } curly brace pair in the previous snippet. An explicit block of this sort—if it has no declarations, it’s not actually a scope— serves no operational purpose, though it can still be useful as a semantic signal.

```javascript
if (somethingHappened) {
    // this is a block, but not a scope
    {
        // this is both a block and an
        // explicit scope
        let msg = somethingHappened.message();
        notifyOthers(msg);
    }
    // ..
    recoverFromSomething();
}
```

Here, the { .. } curly-brace pair inside the if statement is an even smaller inner explicit block scope for msg, since that variable is not needed for the entire if block. Most developers would just block-scope msg to the if block and move on. And to be fair, when there’s only a few lines to consider, it’s a toss-up judgement call. But as code grows, these over-exposure issues become more pronounced.

## var and let

Stylistically, var has always, from the earliest days of JS, signaled “variable that belongs to a whole function.” As we asserted in “Lexical Scope”, var attaches to the nearest enclosing function scope, no matter where it appears.
That’s true even if var appears inside a block:

```javascript
function diff(x,y) {
    if (x > y) {
        var tmp = x; // `tmp` is function-scoped
        x = y;
        y = tmp;
    }
    return y - x;
}
```

Even though var is inside a block, its declaration is function-scoped (to diff(..)), not block-scoped.
Why not just use let in that same location? Because var is visually distinct from let and therefore signals clearly, “this variable is function-scoped.” Using let in the top-level scope, especially if not in the first few lines of a function, and when all the other declarations in blocks use let, does not visually draw attention to the difference with the function-scoped declaration.

## Where To let?

If you decide initially that a variable should be block-scoped, and later realize it needs to be elevated to be function-scoped, then that dictates a change not only in the location of that variable’s declaration, but also the declarator keyword used. The decision-making process really should proceed like that. 
If a declaration belongs in a block scope, use let. If it belongs in the function scope, use var.

But another way to sort of visualize this decision making is to consider the pre-ES6 version of a program. For example, let’s recall diff(..) from earlier:

```javascript
function diff(x,y) {
    var tmp;
    if (x > y) {
        tmp = x;
        x = y;
        y = tmp;
    }
    return y - x;
}
```

In this version of diff(..), tmp is clearly declared in the function scope. Is that appropriate for tmp? No. tmp is only needed for those few statements. It’s not needed for the return statement. It should therefore be block-scoped.
Prior to ES6, we didn’t have let so we couldn’t actually block-scope it. But we could do the next-best thing in signaling our intent:


```javascript
function diff(x,y) {
    if (x > y) {
        // `tmp` is still function-scoped, but
        // the placement here semantically
        // signals block-scoping
        var tmp = x;
        x = y;
        y = tmp;
    }
    return y - x;
}
```
Placing the var declaration for tmp inside the if statement signals to the reader of the code that tmp belongs to that block. Even though JS doesn’t enforce that scoping, the semantic signal still has benefit for the reader of your code.

## What’s the Catch?

So far we’ve asserted that var and parameters are function-scoped, and let/const signal block-scoped declarations. There’s one little exception to call out: the catch clause. Since the introduction of try..catch back in ES3 (in 1999), the catch clause has used an additional (little-known) blockscoping declaration capability:

```javascript
try {
    doesntExist();
}
catch (err) {
    console.log(err);
    // ReferenceError: 'doesntExist' is not defined
    // ^^^^ message printed from the caught exception
    let onlyHere = true;
    var outerVariable = true;
}
console.log(outerVariable); // true
console.log(err);
// ReferenceError: 'err' is not defined
// ^^^^ this is another thrown (uncaught) exception
```

The err variable declared by the catch clause is block-scoped to that block. This catch clause block can hold other blockscoped declarations via let. But a var declaration inside this block still attaches to the outer function/global scope.

## Function Declarations in Blocks (FiB)

So what about function declarations that appear directly inside blocks? As a feature, this is called “FiB.”
We typically think of function declarations like they’re the equivalent of a var declaration. So are they function-scoped like var is?

```javascript
if (false) {
    function ask() {
        console.log("Does this run?");
    }
}
ask();
```

What do you expect for this program to do? Three reasonable outcomes:
1. The ask() call might fail with a ReferenceError exception, because the ask identifier is block-scoped to the if block scope and thus isn’t available in the outer/global scope.
2. The ask() call might fail with a TypeError exception, because the ask identifier exists, but it’s undefined (since the if statement doesn’t run) and thus not a callable function.
3. The ask() call might run correctly, printing out the “Does it run?” message. Here’s the confusing part: depending on which JS environment you try that code snippet in, you may get different results! This is one of those few crazy areas where existing legacy behavior betrays a predictable outcome.

The JS specification says that function declarations inside of blocks are block-scoped, so the answer should be (1). However, most browser-based JS engines (including v8, which comes from Chrome but is also used in Node) will behave as (2), meaning the identifier is scoped outside the if block but the function value is not automatically initialized, so it remains undefined.

Why are browser JS engines allowed to behave contrary to the specification? Because these engines already had certain behaviors around FiB before ES6 introduced block scoping, and there was concern that changing to adhere to the specification might break some existing website JS code.

One of the most common use cases for placing a function declaration in a block is to conditionally define a function one way or another (like with an if..else statement) depending on some environment state. For example:

```javascript
if (typeof Array.isArray != "undefined") {
    function isArray(a) {
        return Array.isArray(a);
    }
}
else {
    function isArray(a) {
        return Object.prototype.toString.call(a) == "[object Array]";
    }
}
```

It’s tempting to structure code this way for performance reasons, since the typeof Array.isArray check is only performed once, as opposed to defining just one isArray(..) and putting the if statement inside it—the check would then run unnecessarily on every call.

## Blocked Over

The point of lexical scoping rules in a programming language is so we can appropriately organize our program’s variables, both for operational as well as semantic code communication purposes. And one of the most important organizational techniques is to ensure that no variable is over-exposed to unnecessary scopes (POLE). 
