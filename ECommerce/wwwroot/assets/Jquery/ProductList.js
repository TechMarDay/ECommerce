$('body').on('click', '.btn-add-cart', function (e) {
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
