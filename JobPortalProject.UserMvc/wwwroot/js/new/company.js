function cancelCompanyTranslationUpdate(a) {
    console.log(a);
const companyTranslation = document.getElementById("companyTranslation");
companyTranslation.innerHTML="";
}

function updateCompanyTranslation(id) {
    console.log(id);

    const companyTranslation = document.getElementById("companyTranslation");
    fetch(`/company/updateCompanyTranslation/${id}`)
        .then(response => response.text())
        .then(html => {
            console.log(html);
            companyTranslation.innerHTML = "";
            companyTranslation.innerHTML = html;
        });
}

function deleteWorkingField(id, languageId) {
    console.log(id);
    console.log(languageId);
    var row = document.getElementById('workingFieldRow');

    fetch(`/company/deleteWorkingField?id=${id}&languageId=${languageId}`, {
        method: 'POST'
    })
        .then(response => response.json())
        .then(data => {
            console.log(data);
            row.innerHTML = data;
        })
        .catch(error => console.error('Error:', error));
}
