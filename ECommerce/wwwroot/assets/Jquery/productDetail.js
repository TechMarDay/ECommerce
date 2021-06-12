var sliderItems = document.getElementsByClassName('product-detail-slider-item');

var activeSliderItems = document.getElementsByClassName('product-detail-slider-active');
var sliderLength = sliderItems.length;
var activeSliderLength = activeSliderItems.length;
var productDetailImage = document.getElementById('product-detail-image');

for (var i = 0; i < sliderLength; i++) {
    sliderItems[i].addEventListener('mouseover', function () {
        if (activeSliderLength > 0) {
            activeSliderItems[0].classList.remove('product-detail-slider-active');
        }   

        this.classList.add('product-detail-slider-active');
        productDetailImage.src = this.querySelector('.product-detail-slider-item__img').src;
    });
}