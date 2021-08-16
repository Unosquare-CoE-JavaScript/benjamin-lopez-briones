// TODO: write the validation functions

const isValidName = name => {
    if(typeof name == "string" && name.trim() !== '' && name.length >= 3)
        return true;
    return false;
};

const hoursAttended = (attended, length) => {
    const validateString = x => x.trim() !== '';
    
    if(typeof attended == "string"){
        if(!validateString(attended)) return false;
    }
    else if (typeof attended != "number"){
        return false;
    }
    
    if(typeof length == "string"){
        if(!validateString(length)) return false;
    }
    else if (typeof length != "number"){
        return false;
    }
    
    let N1 = Number(attended);
    let N2 = Number(length);

    if(isNaN(N1) || isNaN(N2)) return false;

    if((N1 < 0 && N2 < 0)) return false;

    if(!Number.isInteger(N1) || !Number.isInteger(N2)) return false;

    return N1 <= N2;
};

// tests:
console.log(isValidName("Frank") === true);
console.log(hoursAttended(6,10) === true);
console.log(hoursAttended(6,"10") === true);
console.log(hoursAttended("6",10) === true);
console.log(hoursAttended("6","10") === true);

console.log(isValidName(false) === false);
console.log(isValidName(null) === false);
console.log(isValidName(undefined) === false);
console.log(isValidName("") === false);
console.log(isValidName("  \t\n") === false);
console.log(isValidName("X") === false);
console.log(hoursAttended("",6) === false);
console.log(hoursAttended(6,"") === false);
console.log(hoursAttended("","") === false);
console.log(hoursAttended("foo",6) === false);
console.log(hoursAttended(6,"foo") === false);
console.log(hoursAttended("foo","bar") === false);
console.log(hoursAttended(null,null) === false);
console.log(hoursAttended(null,undefined) === false);
console.log(hoursAttended(undefined,null) === false);
console.log(hoursAttended(undefined,undefined) === false);
console.log(hoursAttended(false,false) === false);
console.log(hoursAttended(false,true) === false);
console.log(hoursAttended(true,false) === false);
console.log(hoursAttended(true,true) === false);
console.log(hoursAttended(10,6) === false);
console.log(hoursAttended(10,"6") === false);
console.log(hoursAttended("10",6) === false);
console.log(hoursAttended("10","6") === false);
console.log(hoursAttended(6,10.1) === false);
console.log(hoursAttended(6.1,10) === false);
console.log(hoursAttended(6,"10.1") === false);
console.log(hoursAttended("6.1",10) === false);
console.log(hoursAttended("6.1","10.1") === false);