// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(function () {
  const searchBox = document.getElementById('search-box');
  if (searchBox) {
    searchBox.focus();

    const endString = searchBox.value.length;
    searchBox.setSelectionRange(endString, endString);
  }
});
