// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

if (document.getElementById('callApiButton')) {

    document.getElementById('callApiButton').addEventListener('click', function () {
        fetch('/api/claims')
            .then(response => response.json())
            .then(data => {
                let resultDiv = document.getElementById('apiResult');
                resultDiv.innerHTML = '<h3>API Call Results</h3><ul>';
                data.forEach(claim => {
                    debugger;
                    resultDiv.innerHTML += `<li>${claim.type}: ${claim.value}</li>`;
                });
                resultDiv.innerHTML += '</ul>';
            })
            .catch(error => console.error('Error:', error));
    });
}
