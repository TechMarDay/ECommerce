var commonController = function () {
    this.initialize = function () {
        loadData();
    }

    function loadData() {
        $.ajax({
            type: 'GET',
            contentType: "application/json",
            url: '/giohang/items',
            success: function (res) {
                $("#number_cart_header").text(res.length);
            },
            error: function (err) {
                console.log(err);
            }
        });
    }

}