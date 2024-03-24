const submitDIV = document.getElementById('submit');
const form = document.getElementsByTagName('form');

form[0].addEventListener('submit', function () {
    Array.from(submitDIV.children).forEach((element) => {
        element.classList.add('form-button-disable');
    });
});

document.addEventListener('DOMContentLoaded', function () {
    Array.from(submitDIV.children).forEach((element) => {
        element.classList.remove('form-button-disable');
    });
});
