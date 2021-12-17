## Chapter 7: Using Closures

Recall the main conclusion of Chapter 6: the least exposure principle (POLE) encourages us to use block (and function) scoping to limit the scope exposure of variables. This helps keep code understandable and maintainable, and helps avoid many scoping pitfalls (i.e., name collision, etc.).

Closure builds on this approach: for variables we need to use over time, instead of placing them in larger outer scopes, we can encapsulate (more narrowly scope) them but still preserve access from inside functions, for broader use. Functions remember these referenced scoped variables via closure.

If you’ve ever written a callback that accesses variables outsid its own scope… guess what!? That’s closure.

Closure is one of the most important language characteristics ever invented in programming—it underlies major programming paradigms, including Functional Programming (FP), modules, and even a bit of class-oriented design. Getting comfortable with closure is required for mastering JS and effectively leveraging many important design patterns throughout your code.

Addressing all aspects of closure requires a daunting mountain of discussion and code throughout this chapter. Make sure to take your time and ensure you’re comfortable witheach bit before moving onto the next.

## See the Closure

Closure is originally a mathematical concept, from lambda calculus. But I’m not going to list out math formulas or use a bunch of notation and jargon to define it.

Instead, I’m going to focus on a practical perspective. We’ll start by defining closure in terms of what we can observe in different behavior of our programs, as opposed to if closure was not present in JS. However, later in this chapter, we’re going to flip closure around to look at it from an alternative perspective.

Closure is a behavior of functions and only functions. If you aren’t dealing with a function, closure does not apply. An object cannot have closure, nor does a class have closure (though its functions/methods might). Only functions have closure.

For closure to be observed, a function must be invoked, and specifically it must be invoked in a different branch of the scope chain from where it was originally defined. A function executing in the same scope it was defined would not exhibit any observably different behavior with or without closure being possible; by the observational perspective and definition, that is not closure.

```javascript
// outer/global scope: RED(1)
function lookupStudent(studentID) {
    // function scope: BLUE(2)
    var students = [
        { id: 14, name: "Kyle" },
        { id: 73, name: "Suzy" },
        { id: 112, name: "Frank" },
        { id: 6, name: "Sarah" }
    ];
    return function greetStudent(greeting){
        // function scope: GREEN(3)
        var student = students.find(
            student => student.id == studentID
        );
        return `${ greeting }, ${ student.name }!`;
    };
}
var chosenStudents = [
    lookupStudent(6),
    lookupStudent(112)
];
// accessing the function's name:
chosenStudents[0].name;
// greetStudent
chosenStudents[0]("Hello");
// Hello, Sarah!
chosenStudents[1]("Howdy");
// Howdy, Frank!
```

The first thing to notice about this code is that the lookupStudent(..) outer function creates and returns an inner function called greetStudent(..). lookupStudent(..) is called twice, producing two separate instances of its inner greetStudent(..) function, both of which are saved into the chosenStudents array.

We verify that’s the case by checking the .name property of the returned function saved in chosenStudents[0], and it’s indeed an instance of the inner greetStudent(..). After each call to lookupStudent(..) finishes, it would seem like all its inner variables would be discarded and GC’d (garbage collected). The inner function is the only thing that seems to be returned and preserved. But here’s where the behavior differs in ways we can start to observe. While greetStudent(..) does receive a single argument as the parameter named greeting, it also makes reference to both students and studentID, identifiers which come from the enclosing scope of lookupStudent(..). Each of those references from the inner function to the variable in an outer scope is called a closure. In academic terms, each instance of greetStudent(..) closes over the outer variables students and studentID.

Closure allows greetStudent(..) to continue to access those outer variables even after the outer scope is finished (when each call to lookupStudent(..) completes). Instead of the instances of students and studentID being GC’d, they stay around in memory. At a later time when either instance of the greetStudent(..) function is invoked, those variables are still there, holding their current values. If JS functions did not have closure, the completion of each lookupStudent(..) call would immediately tear down its scope and GC the students and studentID variables. When we later called one of the greetStudent(..) functions, what would then happen?

If greetStudent(..) tried to access what it thought was a BLUE(2) marble, but that marble did not actually exist (anymore), the reasonable assumption is we should get a ReferenceError, right? But we don’t get an error. The fact that the execution of chosenStudents[0]("Hello") works and returns us the message “Hello, Sarah!”, means it was still able to access the students and studentID variables. This is a direct observation of closure.

### Pointed Closure

Because of how terse the syntax for => arrow functions is, it’s easy to forget that they still create a scope (as asserted n “Arrow Functions” in Chapter 3). The student => student.id == studentID arrow function is creating another scope bubble inside the greetStudent(..) function scope.

Building on the metaphor of colored buckets and bubbles from Chapter 2, if we were creating a colored diagram for this code, there’s a fourth scope at this innermost nesting level, so we’d need a fourth color; perhaps we’d pick ORANGE(4) for that scope:

```javascript
var student = students.find( student =>
    // function scope: ORANGE(4)
    student.id == studentID
);
```

The BLUE(2) studentID reference is actually inside the ORANGE(4) scope rather than the GREEN(3) scope of greetStudent(..); also, the student parameter of the arrow function is ORANGE(4), shadowing the GREEN(3) student.

The consequence here is that this arrow function passed as a callback to the array’s find(..) method has to hold the closure over studentID, rather than greetStudent(..) holding that closure. That’s not too big of a deal, as everything still works as expected. It’s just important not to skip over the fact that even tiny arrow functions can get in on the closure party.

### Adding Up Closures

Let’s examine one of the canonical examples often cited for closure:

```javascript
function adder(num1) {
    return function addTo(num2){
        return num1 + num2;
    };
}
var add10To = adder(10);
var add42To = adder(42);
add10To(15); // 25
add42To(9); // 51
```
Each instance of the inner addTo(..) function is closing over its own num1 variable (with values 10 and 42, respectively), so those num1’s don’t go away just because adder(..) finishes. When we later invoke one of those inner addTo(..) instances, such as the add10To(15) call, its closed-over num1 variable still exists and still holds the original 10 value. The operation is thus able to perform 10 + 15 and return the answer 25.

An important detail might have been too easy to gloss over in that previous paragraph, so let’s reinforce it: closure is associated with an instance of a function, rather than its single lexical definition. In the preceding snippet, there’s just one inner addTo(..) function defined inside adder(..), so it might seem like that would imply a single closure.

But actually, every time the outer adder(..) function runs, a new inner addTo(..) function instance is created, and for each new instance, a new closure. So each inner function instance (labeled add10To(..) and add42To(..) in our program) has its own closure over its own instance of the scope environment from that execution of adder(..).

### Live Link, Not a Snapshot

In both examples from the previous sections, we read the value from a variable that was held in a closure. That makes it feel like closure might be a snapshot of a value at some given moment. Indeed, that’s a common misconception. 

Closure is actually a live link, preserving access to the full variable itself. We’re not limited to merely reading a value; the closed-over variable can be updated (re-assigned) as well! By closing over a variable in a function, we can keep using that variable (read and write) as long as that function reference exists in the program, and from anywhere we want to invoke that function. This is why closure is such a powerful technique used widely across so many areas of programming.

Now let’s examine an example where the closed-over variableis updated:

```javascript
function makeCounter() {
    var count = 0;
    return getCurrent(){
        count = count + 1;
        return count;
    };
}
var hits = makeCounter();
// later
hits(); // 1
// later
hits(); // 2
hits(); // 3
```

The count variable is closed over by the inner getCurrent() function, which keeps it around instead of it being subjected to GC. The hits() function calls access and update this variable, returning an incrementing count each time.

Though the enclosing scope of a closure is typically from a function, that’s not actually required; there only needs to be an inner function present inside an outer scope:

```javascript
var hits;
{ // an outer scope (but not a function)
    let count = 0;
    hits = function getCurrent(){
        count = count + 1;
        return count;
    };
}
hits(); // 1
hits(); // 2
hits(); // 3
```

## What If I Can’t See It?

The emphasis in our definition of closure is observability. If a closure exists (in a technical, implementation, or academic sense) but it cannot be observed in our programs, does it matter? No. To reinforce this point, let’s look at some examples that are not observably based on closure. For example, invoking a function that makes use of lexical scope lookup:

```javascript
function say(myName) {
    var greeting = "Hello";
    output();
    function output() {
        console.log(`${ greeting }, ${ myName }!`);
    }
}

say("Kyle");
// Hello, Kyle!
```

The inner function output() accesses the variables greeting and myName from its enclosing scope. But the invocation of output() happens in that same scope, where of course greeting and myName are still available; that’s just lexical scope, not closure. Any lexically scoped language whose functions didn’t support closure would still behave this same way. In fact, global scope variables essentially cannot be (observably) closed over, because they’re always accessible from everywhere. No function can ever be invoked in any part of the scope chain that is not a descendant of the global scope.
Consider:

```javascript
var students = [
{ id: 14, name: "Kyle" },
{ id: 73, name: "Suzy" },
{ id: 112, name: "Frank" },
{ id: 6, name: "Sarah" }
];
function getFirstStudent() {
return function firstStudent(){
return students[0].name;
};
}
var student = getFirstStudent();
student();
// Kyle
```

The inner firstStudent() function does reference students, which is a variable outside its own scope. But since students happens to be from the global scope, no matter where that function is invoked in the program, its ability to access students is nothing more special than normal lexical scope.
All function invocations can access global variables, regardless of whether closure is supported by the language or not. Global variables don’t need to be closed over.

## Observable Definition

We’re now ready to define closure:

Closure is observed when a function uses variable(s) from outer scope(s) even while running in a scope where those variable(s) wouldn’t be accessible.

The key parts of this definition are:
- Must be a function involved
- Must reference at least one variable from an outer scope
- Must be invoked in a different branch of the scope chain from the variable(s)

This observation-oriented definition means we shouldn’t dismiss closure as some indirect, academic trivia. Instead, we should look and plan for the direct, concrete effects closure has on our program behavior.

## The Closure Lifecycle and Garbage Collection (GC)

Since closure is inherently tied to a function instance, its closure over a variable lasts as long as there is still a reference to that function. If ten functions all close over the same variable, and over time nine of these function references are discarded, the lone remaining function reference still preserves that variable. Once that final function reference is discarded, the last closure over that variable is gone, and the variable itself is GC’d.

This has an important impact on building efficient and performant programs. Closure can unexpectedly prevent the GC of a variable that you’re otherwise done with, which leads to run-away memory usage over time. That’s why it’s important to discard function references (and thus their closures) when they’re not needed anymore.
Consider:

```javascript
function manageBtnClickEvents(btn) {
    var clickHandlers = [];
    return function listener(cb){
        if (cb) {
            let clickHandler = function onClick(evt){
                console.log("clicked!");
                cb(evt);
            };

            clickHandlers.push(clickHandler);
            btn.addEventListener("click", clickHandler);
        }
        else {
            // passing no callback unsubscribes
            // all click handlers
            for (let handler of clickHandlers) {
                btn.removeEventListener("click",handler);
            }
            clickHandlers = [];
        }
    };
}

// var mySubmitBtn = ..
var onSubmit = manageBtnClickEvents(mySubmitBtn);
onSubmit(function checkout(evt){
    // handle checkout
});
onSubmit(function trackAction(evt){
    // log action to analytics
});
// later, unsubscribe all handlers:
onSubmit();
```


In this program, the inner onClick(..) function holds a closure over the passed in cb (the provided event callback). That means the checkout() and trackAction() function expression references are held via closure (and cannot be GC’d) for as long as these event handlers are subscribed. When we call onSubmit() with no input on the last line, all event handlers are unsubscribed, and the clickHandlers array is emptied. Once all click handler function references are discarded, the closures of cb references to checkout() and trackAction() are discarded. When considering the overall health and efficiency of the program, unsubscribing an event handler when it’s no longer needed can be even more important than the initial subscription.

### Per Variable or Per Scope?

Another question we need to tackle: should we think of closure as applied only to the referenced outer variable(s), or does closure preserve the entire scope chain with all its variables?

In other words, in the previous event subscription snippet, is the inner onClick(..) function closed over only cb, or is it also closed over clickHandler, clickHandlers, and btn? Conceptually, closure is per variable rather than per scope. Ajax callbacks, event handlers, and all other forms of function closures are typically assumed to close over only what they explicitly reference. But the reality is more complicated than that.

Another program to consider:

```javascript
function manageStudentGrades(studentRecords) {
    var grades = studentRecords.map(getGrade);
    return addGrade;
    // ************************
    function getGrade(record){
        return record.grade;
    }
    function sortAndTrimGradesList() {
        // sort by grades, descending
        grades.sort(function desc(g1,g2){
            return g2 - g1;
        });
        // only keep the top 10 grades
        grades = grades.slice(0,10);
    }
    function addGrade(newGrade) {
        grades.push(newGrade);
        sortAndTrimGradesList();
        return grades;
    }
}

var addNextGrade = manageStudentGrades([
    { id: 14, name: "Kyle", grade: 86 },
    { id: 73, name: "Suzy", grade: 87 },
    { id: 112, name: "Frank", grade: 75 },
    // ..many more records..
    { id: 6, name: "Sarah", grade: 91 }
]);

// later
addNextGrade(81);
addNextGrade(68);
// [ .., .., ... ]
```

The outer function manageStudentGrades(..) takes a list of student records, and returns an addGrade(..) function reference, which we externally label addNextGrade(..). Each time we call addNextGrade(..) with a new grade, we get back a current list of the top 10 grades, sorted numerically descending (see sortAndTrimGradesList()).

From the end of the original manageStudentGrades(..) call, and between the multiple addNextGrade(..) calls, the grades variable is preserved inside addGrade(..) via closure; that’s how the running list of top grades is maintained. Remember, it’s a closure over the variable grades itself, not the array it holds.

That’s not the only closure involved, however. Can you spot other variables being closed over? Did you spot that addGrade(..) references sortAndTrimGradesList? That means it’s also closed over that identifier, which happens to hold a reference to the sortAndTrimGradesList() function. That second inner function has to stay around so that addGrade(..) can keep calling it, which also means any variables it closes over stick around—though, in this case, nothing extra is closed over there.

What else is closed over? Consider the getGrade variable (and its function); is it closed over? It’s referenced in the outer scope of manageStudentGrades(..) in the .map(getGrade) call. But it’s not referenced in addGrade(..) or sortAndTrimGradesList().

What about the (potentially) large list of student records we pass in as studentRecords? Is that variable closed over? If it is, the array of student records is never getting GC’d, which leads to this program holding onto a larger amount of memory than we might assume. But if we look closely again, none of the inner functions reference studentRecords. 

According to the per variable definition of closure, since getGrade and studentRecords are not referenced by the inner functions, they’re not closed over. They should be freely available for GC right after the manageStudentGrades(..) call completes.

Indeed, try debugging this code in a recent JS engine, like v8 in Chrome, placing a breakpoint inside the addGrade(..) function. You may notice that the inspector does not list the studentRecords variable. That’s proof, debugging-wise anyway, that the engine does not maintain studentRecords via closure.

## An Alternative Perspective

Closure is the link-association that connects that function to the scope/variables outside of itself, no matter where that function goes. Let’s recall a code example from earlier in this chapter, again with relevant scope bubble colors annotated:

```javascript
// outer/global scope: RED(1)
function adder(num1) {
    // function scope: BLUE(2)
    return function addTo(num2){
        // function scope: GREEN(3)
        return num1 + num2;
    };
}
var add10To = adder(10);
var add42To = adder(42);
add10To(15); // 25
add42To(9); // 51
```

Our current perspective suggests that wherever a function is passed and invoked, closure preserves a hidden link back to the original scope to facilitate the access to the closed-over variables.

But there’s another way of thinking about closure, and more precisely the nature of functions being passed around, that may help deepen the mental models. This alternative model de-emphasizes “functions as first-class values,” and instead embraces how functions (like all non-primitive values) are held by reference in JS, and assigned/passed by reference-copy—see.

Instead of thinking about the inner function instance of addTo(..) moving to the outer RED(1) scope via the return
and assignment, we can envision that function instances actually just stay in place in their own scope environment, of course with their scope-chain intact. What gets sent to the RED(1) scope is just a reference to the in-place function instance, rather than the function instance itself. Figure 5 depicts the inner function instances remaining in place, pointed to by the RED(1) addTo10 and addTo42 references, respectively.

When addTo10(15) is called, the addTo(..) function instance (still in place in its original BLUE(2) scope environment) is invoked. Since the function instance itself never  moved, of course it still has natural access to its scope chain.Same with the addTo42(9) call—nothing special here beyond lexical scope.

So what then is closure, if not the magic that lets a function maintain a link to its original scope chain even as that function moves around in other scopes? In this alternative model, functions stay in place and keep accessing their original scope chain just like they always could.  Closure instead describes the magic of keeping alive a function instance, along with its whole scope environment andchain, for as long as there’s at least one reference to that function instance floating around in any other part of the program.

## Closer to Closure

We explored two models for mentally tackling closure:
- Observational: closure is a function instance remembering its outer variables even as that function is passed to and invoked in other scopes.
- Implementational: closure is a function instance and its scope environment preserved in-place while any references to it are passed around and invoked from other scopes.

Summarizing the benefits to our programs:

- Closure can improve efficiency by allowing a function instance to remember previously determined information instead of having to compute it each time.
- Closure can improve code readability, bounding scope-exposure by encapsulating variable(s) inside function instances, while still making sure the information in those variables is accessible for future use. The resultant narrower, more specialized function instances are cleaner to interact with, since the preserved information doesn’t need to be passed in every invocation.