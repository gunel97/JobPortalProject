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
    var element = document.getElementById(`workingAreaItemRow${id}`);
    fetch(`/company/deleteWorkingField/${id}`, {
        method: "Post"
    })
        .then(response => response.text())
        .then(data => {
            element.remove();
        });
}

function deleteAddress(id) {
    console.log(id);
    var element = document.getElementById(`address${id}`);
    fetch(`/company/deleteAddress/${id}`, {
        method: "Post"
    })
        .then(response => response.text())
        .then(data => {
            element.remove();
        });
}