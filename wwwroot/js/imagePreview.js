const MAX_HEIGHT = 250;
const MAX_WIDTH = 250;

const inputFile = document.querySelector('#upload');
const imgArea = document.querySelector('#images__client');

inputFile.addEventListener('change', function () {
  const image = this.files[0];
  const reader = new FileReader();
  reader.onload = () => {
    // Remove as imagens existentes
    const allImg = imgArea.querySelectorAll('img');
    allImg.forEach((item) => item.remove());

    const imgUrl = reader.result;
    const img = document.createElement('img');
    img.setAttribute('width', MAX_WIDTH);
    img.setAttribute('height', MAX_HEIGHT);
    img.setAttribute('class', 'image__client');
    img.src = imgUrl;
    imgArea.appendChild(img);
    imgArea.classList.add('active');
    imgArea.dataset.img = image.name;
  };
  reader.readAsDataURL(image);
});
