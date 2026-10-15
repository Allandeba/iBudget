const form = document.getElementById('formDemo');
const button = document.getElementById('demoLoginButton');
const status = document.getElementById('demoLoginStatus');

if (form && button && status) {
    const reset = () => {
        button.disabled = false;
        button.textContent = 'Explorar demonstração';
        status.textContent = '';
    };

    form.addEventListener('submit', (event) => {
        if (button.disabled) {
            event.preventDefault();
            return;
        }

        button.disabled = true;
        button.textContent = 'Abrindo demonstração…';
        status.textContent = 'Abrindo demonstração…';
    });

    window.addEventListener('pageshow', reset);
}
