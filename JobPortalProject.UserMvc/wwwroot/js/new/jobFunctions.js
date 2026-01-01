function applyJob(id, element) {
    console.log(id);
    fetch(`/candidate/applyJob/${id}`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application-json'
        }
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                alert('Applied');
                element.innerHTML = '<span><img src="/images/icon/apply-ellipse.svg" alt=""></span>Applied';
                return;
            }
            else {
                alert(data.message);
            }
        })
        .catch(error => console.error('Error:', error));
};

