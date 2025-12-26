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

