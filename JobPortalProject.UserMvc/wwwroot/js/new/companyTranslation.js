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
        }
        );
}
