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


$('body').on('click', '.btn-add-cart-detail', function (e) {
    e.preventDefault();
    const id = $(this).data('id');
    $.ajax({
        type: 'POST',
        contentType: "application/json",
        url: '/giohang/',
        data: JSON.stringify({ id: id }),
        contentType: "application/json",
        success: function (res) {
            $('#number_cart_header').text(res.length);
            alert("Bạn đã thêm vào giỏ hang thành công!")
        },
        error: function (err) {
            console.log(err);
        }
    });
});

$('body').on('click', '.btn-buy-detail', function (e) {
    e.preventDefault();
    const id = $(this).data('id');
    $.ajax({
        type: 'POST',
        contentType: "application/json",
        url: '/giohang/',
        data: JSON.stringify({ id: id }),
        contentType: "application/json",
        success: function (res) {
            debugger;
            $('#number_cart_header').text(res.length);
            window.location = "/giohang";
        },
        error: function (err) {
            console.log(err);
        }
    });
});