let imageClient = document.querySelector('#images__client');
let defaultImageSelect = document.querySelector('#defaultImageSelect');
let SizeImagesToDelete = 0;

const MAX_HEIGHT = 250;
const MAX_WIDTH = 250;
const DEFAULT_SELECT_ENUM = 'None';

const canvas = document.createElement('canvas');
const ctx = canvas.getContext('2d');
canvas.width = Math.floor(MAX_WIDTH);
canvas.height = Math.floor(MAX_HEIGHT);

const uploadFiles = document.getElementById('upload');
uploadFiles.addEventListener('change', (event) => processNewImages(event));

function processNewImages(_event) {
  const selectedFiles = _event.target.files;
  if (!selectedFiles) return;

  deleteAllChildren();

  for (let i = 0; i < selectedFiles.length; i++) {
    const selectedFile = selectedFiles[i];

    let newImage = getNewImage();
    newImage.name = selectedFile.name;

    const reader = new FileReader();
    reader.addEventListener('load', () => {
      newImage.src = reader.result;
    });

    reader.readAsDataURL(selectedFile);
  }
}

function deleteAllChildren() {
  Array.from(imageClient.getElementsByClassName('image__client')).forEach(function (element, index) {
    const parent = element.parentNode;
    imageClient.removeChild(parent);
  });

  deleteNotExistentDefaultImage();
}

function deleteClientItemImage(itemImageId) {
  Array.from(imageClient.getElementsByTagName('img')).forEach(function (element, index) {
    if (element.id == itemImageId) {
      const parent = element.parentNode;
      imageClient.removeChild(parent);

      return;
    }
  });
}

function getNewImage() {
  let image = new Image();
  image.addEventListener('load', (event) => {
    drawImage(event.target);
    drawElements(event.target);
    drawNewDefaultImageSelect(event.target);
  });

  return image;
}

function drawImage(_newImage) {
  ctx.drawImage(_newImage, 0, 0, MAX_WIDTH, MAX_HEIGHT);
}

function drawElements(_newImage) {
  let span = getSpan();
  let deleteLink = getDeleteLinkElement();
  let spanTimes = getSpanTimesElement();

  deleteLink.appendChild(spanTimes);
  span.appendChild(deleteLink);

  let img = getImageElement(_newImage);
  span.appendChild(img);

  imageClient.appendChild(span);
}

function getSpan() {
  let span = document.createElement('span');
  span.setAttribute('class', 'col-6 col-md-4');

  return span;
}

function getDeleteLinkElement() {
  let deleteLink = document.createElement('a');
  deleteLink.setAttribute('id', 0);
  deleteLink.setAttribute('arial-label', 'Close');
  deleteLink.setAttribute(
    'class',
    'close ' +
      'position-absolute ' +
      'translate-middle ' +
      'd - block ' +
      'badge ' +
      'badge-danger ' +
      'border border-light ' +
      'rounded-circle ' +
      'bg-danger ' +
      'p-2 ' +
      'opacity-75 '
  );

  deleteLink.addEventListener('click', (event) => {
    deleteItemImage(event.target.id);
  });

  return deleteLink;
}

function getSpanTimesElement() {
  let spanTimes = document.createElement('span');
  spanTimes.setAttribute('aria-hidden', 'true');
  spanTimes.innerHTML = '&times;';

  return spanTimes;
}

function getImageElement(_image) {
  let img = document.createElement('img');
  img.setAttribute('width', MAX_WIDTH);
  img.setAttribute('height', MAX_HEIGHT);
  img.setAttribute('class', 'image__client');
  img.setAttribute('alt', _image.name);
  img.src = canvas.toDataURL('image/jpeg');
  img.name = _image.name;

  return img;
}

function clearUploadImage(_id) {
  if (!_id) return;

  let uploadFiles = document.getElementById('upload');
  uploadFiles.value = '';

  deleteAllChildren();
}

function drawNewDefaultImageSelect(_newImage) {
  var option = document.createElement('option');
  option.value = _newImage.name;
  option.text = _newImage.name;
  defaultImageSelect.appendChild(option);
}

function canDeleteDefaultImageSelect(_name) {
  return _name != DEFAULT_SELECT_ENUM;
}

function deleteNotExistentDefaultImage() {
  let imageNameList = [];

  Array.from(imageClient.getElementsByTagName('img')).forEach(function (element, index) {
    imageNameList.push(element.name);
  });

  Array.from(defaultImageSelect.getElementsByTagName('option')).forEach(function (element, index) {
    if (!imageNameList.includes(element.value) && canDeleteDefaultImageSelect(element.value)) {
      defaultImageSelect.removeChild(element);
    }
  });
}

function createInputForDeletingImages(itemImageId) {
  let inputForDeletingImage = document.createElement('input');
  inputForDeletingImage.type = 'hidden';
  inputForDeletingImage.name = 'IdImagesToDelete[' + SizeImagesToDelete + ']';
  inputForDeletingImage.value = itemImageId;

  imageClient.appendChild(inputForDeletingImage);
  SizeImagesToDelete++;
}

function deleteItemImage(itemImageId) {
  const title = 'Deletar imagem!';

  if (itemImageId == 0) {
    openModal({ title: title, message: 'Você tem certeza que deseja remover TODAS as imagens adicionadas até o momento?' })
      .then((result) => {
        if (result) {
          clearUploadImage(itemImageId);
          deleteNotExistentDefaultImage();
        } else {
          //
        }
      })
      .catch((error) => {
        console.error('Error:', error);
      });
  } else {
    openModal({ title: title, message: 'Você tem certeza que deseja deletar a imagem de ID: ' + itemImageId + ' ?' })
      .then((result) => {
        if (result) {
          createInputForDeletingImages(itemImageId);
          deleteClientItemImage(itemImageId);
          deleteNotExistentDefaultImage();
        } else {
          console.info('User clicked "No"');
        }
      })
      .catch((error) => {
        console.error('Error:', error);
      });
  }
}
