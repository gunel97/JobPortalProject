function deleteJob(id) {
    var row = document.getElementById(`jobListRow${id}`);
    fetch(`/job/softdelete/${id}`)
        .then(response => response.text())
        .then(data => {
            row.remove();
        });
};

function deactivateJob(id, element) {
    const checkboxText = document.getElementById(`checkboxText${id}`);
    fetch(`/job/deactivate/${id}`)
        .then(response => response.text())
        .then(data => {
            if (element.checked) {
                checkboxText.innerText = 'Active';
            }
            else {
                checkboxText.innerText = 'Deactive';
            }
        });
};

function deleteJobBenefit(id) {
    console.log(id);
    var row = document.getElementById('jobBenefitRow');

    fetch(`/job/deletejobbenefit?id=${id}`, {
        method: 'POST'
    })
        .then(response => response.json())
        .then(data => {
            console.log(data);
            row.innerHTML = data.benefitHtml;
        })
        .catch(error => console.error('Error:', error));
};

function deleteJobResponsibility(id) {
    console.log(id);
    var row = document.getElementById('jobResponsibilityRow');

    fetch(`/job/deletejobresponsibility?id=${id}`, {
        method: 'POST'
    })
        .then(response => response.json())
        .then(data => {
            console.log(data);
            row.innerHTML = data.responsibilityHtml;
        })
        .catch(error => console.error('Error:', error));
};

let respIndex = 0;
let benIndex = 0;
const removeButtonResp = document.getElementById('removeButtonResp');
const removeButtonBen = document.getElementById('removeButtonBen');
const responsibilityTitle = document.getElementById("responsibilityTitle");
const benefitTitle = document.getElementById("benefitTitle");

function addR(translations) {
    console.log(translations);
    console.log("test");
    const rContainer = document.getElementById("rContainer");

    responsibilityTitle.innerText = "Job Responsibilities"
    const divElementOfRespTranslations = document.createElement('div');
    divElementOfRespTranslations.className = 'col-md-12';
    divElementOfRespTranslations.id = `respRow${respIndex}`;

    for (let i = 0; i < translations.length; i++) {
        const divElement = document.createElement('div');
        divElement.className = 'form-inner mb-25';
        var htmlTextR = `<label for="jobtitle">
            Job Responsibility <img src="${translations[i].languageIcon}" alt=""> *</label>
                                <div class="input-area">
                                    <img src="/images/icon/company-2.svg" alt="">
                                    <input name="Responsibilities[${respIndex}].JobResponsibilityTranslations[${i}].Value" placeholder="Senior UI/UX Engineer">
                                    <input type="hidden" name="Responsibilities[${respIndex}].JobResponsibilityTranslations[${i}].LanguageId" value="${i + 1}" />
                                    </div>`;
        divElement.innerHTML += htmlTextR;
        divElementOfRespTranslations.appendChild(divElement);
        console.log(htmlTextR);
    }
    respIndex++;
    rContainer.appendChild(divElementOfRespTranslations);

    removeButtonResp.innerHTML = `<div class="form-inner">
            <button onclick='removeResp()'  class="primry-btn-2 lg-btn w-unset" type="button">Remove </button>
            </div>`;

};

function removeResp() {
    var testR = respIndex - 1;
    var rowResp = document.getElementById(`respRow${testR}`);
    rowResp.remove();
    respIndex--;
    console.log(testR);
    if (testR == 0) {
        removeButtonResp.innerHTML = "";
        responsibilityTitle.innerHTML = "";
    }
};

function addB(translations) {
    const bContainer = document.getElementById("bContainer");
    benefitTitle.innerText = "Job Extra Benefits:";

    const divElementOfBenTranslations = document.createElement('div');
    divElementOfBenTranslations.className = 'col-md-12';
    divElementOfBenTranslations.id = `benRow${benIndex}`;

    for (let i = 0; i < translations.length; i++) {
        const divElement = document.createElement('div');
        divElement.className = 'form-inner mb-25';
        var htmlTextB = `<label for="jobtitle">
            Job Benefit <img src="${translations[i].languageIcon}" alt=""> *</label>
                                <div class="input-area">
                                    <img src="/images/icon/company-2.svg" alt="">
                                    <input name="ExtraBenefits[${benIndex}].JobExtraBenefitTranslations[${i}].Value" placeholder="Senior UI/UX Engineer">
                                    <input type="hidden" name="ExtraBenefits[${benIndex}].JobExtraBenefitTranslations[${i}].LanguageId" value="${i + 1}" />
                                    </div>`;
        divElement.innerHTML += htmlTextB;
        divElementOfBenTranslations.appendChild(divElement);
    };

    benIndex++;
    bContainer.appendChild(divElementOfBenTranslations);

    removeButtonBen.innerHTML = `<div class="form-inner">
            <button onclick='removeBen()'  class="primry-btn-2 lg-btn w-unset" type="button">Remove </button>
            </div>`;
};

function removeBen() {
    var testB = benIndex - 1;
    var benRow = document.getElementById(`benRow${testB}`);
    benRow.remove();
    benIndex--;;
    if (testB == 0) {
        removeButtonBen.innerHTML = "";
        benefitTitle.innerHTML = "";
    }
};

