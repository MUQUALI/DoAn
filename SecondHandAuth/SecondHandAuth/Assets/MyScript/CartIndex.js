$(document).ready(function () {
    var cartID = window.localStorage.getItem("cartID");
    if (cartID) {
        apiGetData("GET", "/api/Cart/GetMyCart", { CartID: cartID }).then(function (data) {
            renderCartItem(data);
            subQty();
        })
    }

    function renderCartItem(data) {
        var html = "";
        var subIc = "<i class='fa fa-minus'></i>";
        var plusIc = "<i class='fa fa-plus'></i>"
        for(item of data.ListDetail) {
            html += "<tr><td><img width='100' src='/ImagesUpload/" + item.image + "'/></td><td>" + item.productName + "</td><td>"
                    + item.Size + " - " + item.Color
                    + "</td><td class='row justify-content-center'>" +
                    "<button type='button' data-id='" + item.productID + "' data-custom='" + item.custom + "' class='btn btn-primary ic-sub rounded-circle'>" + subIc + "</button>" +
                    "<input class='form-control col-4 ml-1 mr-1 text-center' readonly type='text' value='" + item.qty +
                    "' /><button class='btn btn-primary rounded-circle' type='button'>"+plusIc+"</button></td><td class='text-right'>"
                    + item.price + "</td><td class='text-right'>" + item.qty * item.price + "</td><td class='text-right'>"
                    + "<button class='btn btn-sm btn-danger'><i class='fa fa-trash' ></i> </button> " + "</td></tr>";
        }
        $("#cart-body").html(html);
        $("#sub-total").html(data.TotalMoney);
        $("#total").html(data.TotalMoney + 25000);
    }

    function subQty() {
        $(".ic-sub ").click(function () {
            if ($(this).next().val() <= 1) {
                alert("Số lượng sản phẩm tối thiểu là 1");
                return;
            }
            var rows = $(this).parent().parent().children();
            $(this).next().val($(this).next().val() - 1);
            var cartID = window.localStorage.getItem("cartID");

            var customID = $(this).attr("data-custom");
            var productCode = $(this).attr("data-id");

            var formData = new FormData();
            formData.append("cartID", cartID);
            formData.append("code", productCode);
            formData.append("qty", -1);
            formData.append("custom", customID);

            apiPostData("/api/cart/UpdateCart", formData).then(function (data) {
                data = JSON.parse(data);
                $("#sub-total").html(data.totalMoney);
                $("#total").html(data.totalMoney + 25000);
                $(rows[5]).html(data.money);
            })
        })
    }
})