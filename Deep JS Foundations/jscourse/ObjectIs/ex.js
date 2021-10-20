// TODO: define polyfill for `Object.is(..)`
if(!Object.is || true){
    Object.is = function ObjectIs(x, y){
        const mightBNan = (x) => x !== x;

        let xNZero = x === 0 && (1/x)=== -Infinity ;
        let yNZero = y === 0 && (1/y) === -Infinity;

        if(xNZero || yNZero)
            return xNZero && yNZero;
        else if(mightBNan(x) && mightBNan(y))
            return true;
        else if(x === y)
            return true;
        return false;
    }
}


// tests:
console.log(Object.is(42,42) === true);
console.log(Object.is("foo","foo") === true);
console.log(Object.is(false,false) === true);
console.log(Object.is(null,null) === true);
console.log(Object.is(undefined,undefined) === true);
console.log(Object.is(NaN,NaN) === true);
console.log(Object.is(-0,-0) === true);
console.log(Object.is(0,0) === true);

console.log(Object.is(-0,0) === false); //este
console.log(Object.is(0,-0) === false);
console.log(Object.is(0,NaN) === false);
console.log(Object.is(NaN,0) === false);
console.log(Object.is(42,"42") === false);
console.log(Object.is("42",42) === false);
console.log(Object.is("foo","bar") === false);
console.log(Object.is(false,true) === false);
console.log(Object.is(null,undefined) === false);
console.log(Object.is(undefined,null) === false);
