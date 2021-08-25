const printRecords = (recordIds) => {
	var students = recordIds.map((studentID) => {
        return studentRecords.find((r) => {
            return r.id == studentID;
        });
    });

    students.sort((a,b) => {
        if(a.name < b.name) return -1;
        else if(a.name > b.name) return 1;
        else return 0;
    });

    students.map((student) => {
        console.log(student.name, "(" + student.id + "):", (student.paid ? 'Paid' : 'Not paid'));
    });
}

const paidStudentsToEnroll = () => {
	var notEnrolled = studentRecords.filter((rec) => {
        return (rec.paid && !currentEnrollment.includes(rec.id));
    });

    var idsNotEnroled = notEnrolled.map((rec) => {
        return rec.id;
    });

    return [...currentEnrollment, ...idsNotEnroled];
}

const remindUnpaid = (recordIds) => {
	var notPaid = recordIds.filter((id) => {
        var records = studentRecords.find((studentID) => {
            return studentID.id == id;
        });

        return !records.paid;
    });

    printRecords(notPaid);
}


// ********************************

var currentEnrollment = [ 410, 105, 664, 375 ];

var studentRecords = [
	{ id: 313, name: "Frank", paid: true, },
	{ id: 410, name: "Suzy", paid: true, },
	{ id: 709, name: "Brian", paid: false, },
	{ id: 105, name: "Henry", paid: false, },
	{ id: 502, name: "Mary", paid: true, },
	{ id: 664, name: "Bob", paid: false, },
	{ id: 250, name: "Peter", paid: true, },
	{ id: 375, name: "Sarah", paid: true, },
	{ id: 867, name: "Greg", paid: false, },
];

printRecords(currentEnrollment);
console.log("----");
currentEnrollment = paidStudentsToEnroll();
printRecords(currentEnrollment);
console.log("----");
remindUnpaid(currentEnrollment);

/*
	Bob (664): Not Paid
	Henry (105): Not Paid
	Sarah (375): Paid
	Suzy (410): Paid
	----
	Bob (664): Not Paid
	Frank (313): Paid
	Henry (105): Not Paid
	Mary (502): Paid
	Peter (250): Paid
	Sarah (375): Paid
	Suzy (410): Paid
	----
	Bob (664): Not Paid
	Henry (105): Not Paid
*/
