# Chapter 3: The Scope Chain

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

The lookup process thus determined that students is a RED(1) marble, because we had not yet found a matching variable name as we traversed the scope chain, until we arrived at the final RED(1) global scope. Similarly, studentID in the if-statement is determined to be a BLUE(2) marble.
This suggestion of a runtime lookup process works well for conceptual understanding, but it’s not actually how things usually work in practice.
The color of a marble’s bucket (aka, meta information of what scope a variable originates from) is usually determined during the initial compilation processing. Because lexical scope is pretty much finalized at that point, a marble’s color will not change based on anything that can happen later during runtime. Since the marble’s color is known from compilation, and it’s immutable, this information would likely be stored with (or at least accessible from) each variable’s entry in the AST; that information is then used explicitly by the executable instructions that constitute the program’s runtime.
In other words, Engine (from Chapter 2) doesn’t need to lookup through a bunch of scopes to figure out which scope bucket a variable comes from. That information is already known! Avoiding the need for a runtime lookup is a key optimization benefit of lexical scope. The runtime operates more performantly without spending time on all these lookups.
But I said “…usually determined…” just a moment ago, with respect to figuring out a marble’s color during compilation. So in what case would it ever not be known during compilation?
So the ultimate determination of whether the variable was ever appropriately declared in some accessible bucket may need to be deferred to the runtime.

## Shadowing
Our running example for these chapters uses different variable names across the scope boundaries. Since they all have unique names, in a way it wouldn’t matter if all of them were just stored in one bucket (like RED(1)).
Where having different lexical scope buckets starts to matter more is when you have two or more variables, each in different scopes, with the same lexical names. A single scope cannot have two or more variables with the same name; such multiple references would be assumed as just one variable. 
So if you need to maintain two or more variables of the same name, you must use separate (often nested) scopes. And in that case, it’s very relevant how the different scope buckets are laid out.
Consider:

```javascript
var studentName = "Suzy";
function printStudent(studentName) {
    studentName = studentName.toUpperCase();
    console.log(studentName);
}
printStudent("Frank");
// FRANK
printStudent(studentName);
// SUZY
console.log(studentName);
// Suzy
```

The studentName variable on line 1 (the var studentName = .. statement) creates a RED(1) marble. The same named variable is declared as a BLUE(2) marble on line 3, the parameter in the printStudent(..) function definition. What color marble will studentName be in the studentName = studentName.toUpperCase() assignment statement and the console.log(studentName) statement? All three studentName references will be BLUE(2).

With the conceptual notion of the “lookup,” we asserted that it starts with the current scope and works its way outward/upward, stopping as soon as a matching variable is found.

The BLUE(2) studentName is found right away. The RED(1) studentName is never even considered.
This is a key aspect of lexical scope behavior, called shadowing. The BLUE(2) studentName variable (parameter) shadows the RED(1) studentName. So, the parameter is shadowing the (shadowed) global variable. Repeat that sentence to yourself a few times to make sure you have the terminology straight! That’s why the re-assignment of studentName affects only the inner (parameter) variable: the BLUE(2) studentName, not the global RED(1) studentName.

When you choose to shadow a variable from an outer scope, one direct impact is that from that scope inward/downward (through any nested scopes) it’s now impossible for any marble to be colored as the shadowed variable—(RED(1), in this case). In other words, any studentName identifier reference will correspond to that parameter variable, never the global studentName variable. It’s lexically impossible to reference the global studentName anywhere inside of the printStudent(..) function (or from any nested scopes).

## Copying Is Not Accessing

Consider:

```javascript
    var special = 42;
    function lookingFor(special) {
        var another = {
            special: special
        };
        function keepLooking() {
            var special = 3.141592;
            console.log(special);
            console.log(another.special); // Ooo, tricky!
            console.log(window.special);
        }
        keepLooking();
    }

lookingFor(112358132134);
// 3.141592
// 112358132134
// 42
```

Oh! So does this another object technique disprove my claim that the special parameter is “completely inaccessible” from inside keepLooking()? No, the claim is still correct.

special: special is copying the value of the special parameter variable into another container (a property of the same name). Of course, if you put a value in another container, shadowing no longer applies (unless another was shadowed, too!). But that doesn’t mean we’re accessing the parameter special; it means we’re accessing the copy of the value it had at that moment, by way of another container (object property). We cannot reassign the BLUE(2) special parameter to a different value from inside keepLooking().

Another “But…!?” you may be about to raise: what if I’d used objects or arrays as the values instead of the numbers (112358132134, etc.)? Would us having references to objects instead of copies of primitive values “fix” the inaccessibility? 

No. Mutating the contents of the object value via a reference copy is not the same thing as lexically accessing the variable itself. We still can’t reassign the BLUE(2) special parameter.

## Illegal Shadowing
Not all combinations of declaration shadowing are allowed. let can shadow var, but var cannot shadow let:

```javascript
function something() {
    var special = "JavaScript";
    {
    let special = 42; // totally fine shadowing
    // ..
    }
}

function another() {
    // ..
    {
        let special = "JavaScript";
        {
            var special = "JavaScript";
            // ^^^ Syntax Error
            // ..
        }
    }
}
```

Notice in the another() function, the inner var special declaration is attempting to declare a function-wide special, which in and of itself is fine (as shown by the something() function).

The syntax error description in this case indicates that special has already been defined, but that error message is a little misleading—again, no such error happens in something(), as shadowing is generally allowed just fine. The real reason it’s raised as a SyntaxError is because the var is basically trying to “cross the boundary” of (or hop over) the let declaration of the same name, which is not allowed.

That boundary-crossing prohibition effectively stops at each function boundary, so this variant raises no exception:

```javascript
    function another() {
        // ..
        {
            let special = "JavaScript";
            ajax("https://some.url",function callback(){
                // totally fine shadowing
                var special = "JavaScript";
                // ..
            });
        }
    }
```

Summary: let (in an inner scope) can always shadow an outer scope’s var. var (in an inner scope) can only shadow an outer scope’s let if there is a function boundary in between.

## Function Name Scope

As you’ve seen by now, a function declaration looks like this:

```javascript
    function askQuestion() {
        // ..
    }
    var askQuestion = function(){
        // ..
    };
```

The same is true for the variable askQuestion being created. But since it’s a function expression—a function definition used as value instead of a standalone declaration—the function itself will not “hoist”.
One major difference between function declarations and function expressions is what happens to the name identifier of the function.
A function expression with a name identifier is referred to as a “named function expression,” but one without a name identifier is referred to as an “anonymous function expression.” Anonymous function expressions clearly have no name identifier that affects either scope.

Arrow Functions
ES6 added an additional function expression form to the language, called “arrow functions”:

```javascript
var askQuestion = () => {
// ..
};
```

The => arrow function doesn’t require the word function to define it. Also, the ( .. ) around the parameter list is optional in some simple cases. Likewise, the { .. } around the function body is optional in some cases. And when the { .. } are omitted, a return value is sent out without using a return keyword.

Arrow functions are lexically anonymous, meaning they have no directly related identifier that references the function.
The assignment to askQuestion creates an inferred name of “askQuestion”, but that’s not the same thing as being non-anonymous:

```javascript
var askQuestion = () => {
// ..
};
askQuestion.name; // askQuestion
```

The common but incorrect claim that arrow functions somehow behave differently with respect to lexical scope from standard function functions.
This is incorrect. Other than being anonymous (and having no declarative form), => arrow functions have the same lexical scope rules as function functions do. An arrow function, with or without { .. } around its body, still creates a separate, inner nested bucket of scope. Variable declarations inside this nested scope bucket behave the same as in a function scope. 
