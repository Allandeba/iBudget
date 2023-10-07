function openModal({ title = 'Are you sure?', message = 'Body message' }) {
  return new Promise((resolve, reject) => {
    const modalContainer = createModalContainer(title, message);

    const yesButton = modalContainer.querySelector('.btn-outline-danger');
    const noButton = modalContainer.querySelector('.btn-danger');

    yesButton.addEventListener('click', () => {
      document.body.removeChild(modalContainer);
      removeBackdrop();
      resolve(true);
    });

    noButton.addEventListener('click', () => {
      document.body.removeChild(modalContainer);
      removeBackdrop();
      resolve(false);
    });

    document.body.appendChild(modalContainer);
    $('#confirmationModal').modal('show');
  });
}

function createElement(tag, classes = []) {
  const element = document.createElement(tag);
  element.classList.add(...classes);
  return element;
}

function appendChildren(parent, children) {
  children.forEach((child) => parent.appendChild(child));
}

function createModalHeader(title) {
  const modalHeader = createElement('div', ['modal-header', 'd-flex', 'justify-content-center']);
  const heading = createElement('p', ['heading']);
  heading.textContent = title;
  modalHeader.appendChild(heading);
  return modalHeader;
}

function createModalBody(message) {
  const modalBody = createElement('div', ['modal-body']);
  const icon = createElement('i', ['fas', 'fa-times', 'fa-4x', 'animated', 'rotateIn']);
  const bodyText = createElement('p');
  bodyText.textContent = message;
  appendChildren(modalBody, [icon, bodyText]);
  return modalBody;
}

function createModalFooter() {
  const modalFooter = createElement('div', ['modal-footer', 'flex-center']);
  const yesButton = createElement('a', ['btn', 'btn-outline-danger']);
  yesButton.textContent = 'Yes';
  const noButton = createElement('a', ['btn', 'btn-danger', 'waves-effect']);
  noButton.setAttribute('data-dismiss', 'modal');
  noButton.textContent = 'No';
  appendChildren(modalFooter, [yesButton, noButton]);
  return modalFooter;
}

function createModalContent(title, message) {
  const modalContent = createElement('div', ['modal-content', 'text-center']);
  appendChildren(modalContent, [createModalHeader(title), createModalBody(message), createModalFooter()]);
  return modalContent;
}

function createModalDialog(title, message) {
  const modalDialog = createElement('div', ['modal-dialog', 'modal-sm', 'modal-notify', 'modal-danger']);
  modalDialog.role = 'document';
  modalDialog.appendChild(createModalContent(title, message));
  return modalDialog;
}

function createModalContainer(title, message) {
  const modalDiv = createElement('div');
  modalDiv.classList.add('modal', 'fade');
  modalDiv.id = 'confirmationModal';
  modalDiv.tabIndex = '-1';
  modalDiv.role = 'dialog';
  modalDiv.setAttribute('aria-labelledby', 'ModalLabel');
  modalDiv.setAttribute('aria-hidden', 'true');
  modalDiv.appendChild(createModalDialog(title, message));
  return modalDiv;
}

function removeBackdrop() {
  const backdrop = document.querySelector('.modal-backdrop');
  if (backdrop) {
    document.body.removeChild(backdrop);
  }
}
