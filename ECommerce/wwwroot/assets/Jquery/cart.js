var cartController = function () {
    this.initialize = function () {
        loadData();
        registerEvents();
    }

    function registerEvents() {
        $('body').on('click', '.btn_plus_cart', function (e) {
            e.preventDefault();
            const id = $(this).data('id');
            const quantity = parseInt($('#cart_quantity_' + id).val()) + 1;
            updateCart(id, quantity);
        });

        $('body').on('click', '.btn_minus_cart', function (e) {
            e.preventDefault();
            const id = $(this).data('id');
            const quantity = parseInt($('#cart_quantity_' + id).val()) - 1;
            updateCart(id, quantity);
        });

        $('body').on('click', '.btn_remove_cart', function (e) {
            e.preventDefault();
            const id = $(this).data('id');
            updateCart(id, 0);
        });
    }

    function updateCart(id, quantity) {
        $.ajax({
            type: 'PUT',
            url: '/giohang/',
            data: { id: id, quantity: quantity },
            //contentType: "application/json",
            //dataType: 'json',
            success: function (res) {
                $('#cart_quantity_' + id).val(quantity);
                $('#number_cart_header').text(res.length);
                loadData();
            },
            error: function (err) {
                console.log(err);
            }
        });
    }

    function loadData() {
        $.ajax({
            type: 'GET',
            contentType: "application/json",
            url: '/giohang/items',
            success: function (res) {

                if (res.length === 0) {
                    $('.cart_content').hide();
                    $('.cart_empty').show();
                }
                var html = "";
                var total = 0;
                $.each(res, function (i, item) {
                    var amount = item.price * item.quantity;
                    html += "<tr>"
                        + "<td>" + item.name + "</td>"
                        + "<td> <img width='60px' src='/" + item.image + "' /> </td>"
                        + "<td>" + numberWithCommas(item.price) + " vnđ </td>"
                        + "<td><input type='text' class='cart_quantity' id='cart_quantity_" + item.productId + "'  value='" + item.quantity +"'/>"
                        + "<button class='btn_minus_cart' data-id='" + item.productId + "'> -</button> "
                        + "<button class='btn_plus_cart'  data-id='" + item.productId + "'> +</button> </td > "
                        + "<td>" + numberWithCommas(amount) + " vnđ</td>"
                        + "<td><button class='btn_remove_cart' data-id='" + item.productId + "'>X</button></td>"
                        + "</tr>";  

                    total += amount;
                });

                var cartBodyElement = $('#cart_body');
                cartBodyElement.html(html);
                $("#number_order").text(res.length);
                $("#total_order").text(numberWithCommas(total));
            },
            error: function (err) {
                console.log(err);
            }
        });
    }

    function numberWithCommas(x) {
        return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    }
}