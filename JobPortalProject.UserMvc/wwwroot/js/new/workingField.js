function addWorkingAreaPartialView(id) {

    console.log(id);
    const workingFieldsDiv = document.getElementById("workingFieldsDiv");

    fetch(`/company/AddWorkingField/${id}`)
        .then(response => response.text())
        .then(html => {
            console.log(html);
            workingFieldsDiv.innerHTML += html;
        });
}

function addWorkingAreaTranslation(id) {
    console.log(id);
    const workingFieldsDiv = document.getElementById("workingFieldsDiv");

    fetch(`/company/AddWorkingFieldTranslation/${id}`)
        .then(response => response.text())
        .then(html => {
            console.log(html);
            workingFieldsDiv.innerHTML += html;
        });
}

function deleteWorkingField(id) {
    console.log(id);
    var element = document.getElementById("workingField");
    fetch(`/company/deleteWorkingField/${id}`, {
        method: "Post"
    })
        .then(response => response.text())
        .then(data => {
            element.remove();
        });
}