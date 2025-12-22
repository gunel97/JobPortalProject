let eduIndex = 0;
const removeButtonEdu = document.getElementById('removeButtonEdu');
const saveButtonEdu = document.getElementById('saveButtonEdu');

function addEducation(models) {
    console.log(models);
    const educationRow = document.getElementById("educationRow");
    const divElement = document.createElement('div');

    divElement.className = 'row';
    divElement.id = `eduRow${eduIndex}`;
    var htmlTextEdu = `    <div class="col-lg-12">
                                                <div class="info-title">
                                                    <h6>Academic Information:</h6>
                                                    <div class="dash"></div>
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <div class="form-inner mb-25">
                                                    <label>Education Level*</label>
                                                    <div class="input-area">
                                                        <img src="/images/icon/qualification-2.svg" alt="">
                                                     <input name="models[${eduIndex}].EducationTypeId" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <div class="form-inner mb-25">
                                                    <label>My Major*</label>
                                                    <div class="input-area">
                                                        <img src="/images/icon/major.svg" alt="">
                                                        <input type="text" name="models[${eduIndex}].MajorName" id="institute" placeholder="Type Your Major Name...">
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <div class="form-inner mb-25">
                                                    <label for="institute">Institute/University*</label>
                                                    <div class="input-area">
                                                        <img src="/images/icon/univercity.svg" alt="">
                                                        <input  type="text" name="models[${eduIndex}].SchoolName" id="institute" placeholder="Type Your Institute Name...">
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <div class="form-inner mb-20">
                                                    <label for="datepicker10">Starting Period*</label>
                                                    <div class="input-area">
                                                        <img src="/images/icon/calender2.svg" alt="">
                                                        <input name="models[${eduIndex}].StartDate" id="datepicker10" placeholder="DD/MM/YY">
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <div class="form-inner mb-20">
                                                    <label for="datepicker11">Ending Period*</label>
                                                    <div class="input-area">
                                                        <img src="/images/icon/calender2.svg" alt="">
                                                        <input name="models[${eduIndex}].EndDate" id="datepicker11" placeholder="DD/MM/YY">
                                                    </div>
                                                </div>
                                            </div>`;

    divElement.innerHTML += htmlTextEdu;
    educationRow.appendChild(divElement);
    console.log(htmlTextEdu);

    eduIndex++;

    saveButtonEdu.innerHTML = `<div class="form-inner">
                                            <button class="primry-btn-2 lg-btn w-unset" type="submit">Save</button>
                                        </div>`;

    if (eduIndex > 1) {
        removeButtonEdu.innerHTML = `<div class="form-inner">
        <button onclick='removeEdu()'  class="primry-btn-2 lg-btn w-unset" type="button">Remove </button>
        </div>`;
    }
}

function removeEdu() {
    if (eduIndex == 1) {
        return;
    }

    var test = eduIndex - 1;
    var rowEdu = document.getElementById(`eduRow${test}`);
    rowEdu.remove();
    eduIndex--;
    if (test == 1) {
        removeButton.innerHTML = "";
    }
}