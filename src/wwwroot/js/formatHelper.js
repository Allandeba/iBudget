const documentType = document.getElementById('Document_DocumentType');
const documentValue = document.getElementById('Document_Document');
const phone = document.getElementById('Contact_Phone');

function applyFormatCPF() {
    if (!documentValue) return;

    documentValue.value = documentValue.value
        .replace(/\D/g, '')
        .replace(/(\d{3})(\d)/, '$1.$2')
        .replace(/(\d{3})(\d)/, '$1.$2')
        .replace(/(\d{3})(\d{1,2})$/, '$1-$2');
}

function applyFormatCNPJ() {
    if (!documentValue) return;

    documentValue.value = documentValue.value
        .replace(/\D/g, '')
        .replace(/^(\d{2})(\d)/, '$1.$2')
        .replace(/^(\d{2})\.(\d{3})(\d)/, '$1.$2.$3')
        .replace(/\.(\d{3})(\d)/, '.$1/$2')
        .replace(/(\d{4})(\d)/, '$1-$2');

}

function applyFormatRG() {
    if (!documentValue) return;

    documentValue.value = documentValue.value
        .replace(/\D/g, '')
        .replace(/(\d{1})(\d)/, '$1.$2')
        .replace(/(\d{3})(\d)/, '$1.$2')
        .replace(/(\d{3})(\d)/, '$1.$2');

}

function applyFormatPhone() {
    if (!phone) return;

    phone.value = phone.value
        .replace(/\D/g, '')
        .replace(/(\d{2})(\d)/, '+$1 $2')
        .replace(/(\d{2})(\d)/, '($1) $2')
        .replace(/(\d)(\d{4})$/, '$1-$2');

}

function applyDocumentFormat() {
    if (documentType) {
        switch (parseInt(documentType.options[documentType.selectedIndex].value)) {
            case 0:
                applyFormatCPF();
                break;

            case 1:
                applyFormatRG();
                break;

            case 2:
                applyFormatCNPJ();
                break;
        }
    }


}

if (documentValue)
    documentValue.addEventListener('input', function () {
        applyDocumentFormat();
    });

if (phone)
    phone.addEventListener('input', function () {
        applyFormatPhone();
    });

window.addEventListener('DOMContentLoaded', function () {
    applyDocumentFormat();
    applyFormatPhone();
});
