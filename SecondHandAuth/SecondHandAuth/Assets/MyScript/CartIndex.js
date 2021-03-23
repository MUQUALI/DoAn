$(document).ready(function () {
    var cartID = window.localStorage.getItem("cartID");
    if (cartID) {
        apiGetData("GET", "/api/Cart/GetMyCart", { CartID: cartID }).then(function (data) {
            renderCartItem(data);
            subQty();
            addQty();
            deleteItem();
        })
    }
    else {
        var carts = JSON.parse(window.localStorage.getItem("myCart"));
        
        // có giỏ hàng trong localStorage
        var Model = {
            UserID: window.localStorage.getItem("UserID"),
            TotalMoney: getTotalMoney(carts),
            ListDetail: carts
        }
                
        apiPostJsonData("/api/Cart/AddToCart", JSON.stringify(Model)).then(function(data) {
            var objData = JSON.parse(data);
            $("#cartQTY").html(objData.totalItem);
            window.localStorage.removeItem("myCart");
            window.localStorage.setItem("totalItem", objData.totalItem);
            window.localStorage.setItem("cartID", objData.cartID);

            window.location.href = window.location.origin + "/Cart/Index";
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
                    "' /><button class='btn btn-primary rounded-circle ic-add' data-id='" + item.productID + "' data-custom='" + item.custom + "' type='button'>"+plusIc+"</button></td><td class='text-right'>"
                    + item.price + "</td><td class='text-right'>" + item.qty * item.price + "</td><td class='text-right'>"
                    + "<button data-id='" + item.productID + "' data-custom='" + item.custom + "' class='del btn btn-sm btn-danger'><i class='fa fa-trash' ></i> </button> " + "</td></tr>";
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
                window.localStorage.setItem("totalItem", data.totalItem);
                $("#cartQTY").html(data.totalItem);
            })
        })
    }

    function deleteItem() {
        $(".del").click(function () {
            var productID = $(this).attr("data-id");
            var customID = $(this).attr("data-custom");
            var cartId = window.localStorage.getItem("cartID");

            var formData = new FormData();
            formData.append("cartID", cartId);
            formData.append("code", productID);
            formData.append("custom", customID);
            $(this).parent().parent().remove();
            apiPostData("/api/Cart/DeleteItem", formData).then(function (data) {
                data = JSON.parse(data);
                $("#sub-total").html(data.totalMoney);
                $("#total").html(data.totalMoney + 25000);
                window.localStorage.setItem("totalItem", data.totalItem);
                $("#cartQTY").html(data.totalItem);

                if ($(this).parent().parent().children().length <= 0) {
                    $("#cart-body").html("không có sản phẩm nào trong giỏ");
                }
            })
        })
    }

    function addQty() {
        $(".ic-add").click(async function () {
            var productID = $(this).attr("data-id");
            var custom = parseInt($(this).attr("data-custom"));
            var maxQty = await apiGetData("GET", "/api/Cart/GetMaxQtyOfProduct", {ProductID: productID, FK_Custom: custom});

            var curentQty = parseInt($(this).prev().val());
            if(curentQty + 1 > maxQty) {
                alert("Lượng mua tối đa là: "+ maxQty);
                return;
            }
            $(this).prev().val(curentQty + 1);
            var rows = $(this).parent().parent().children();

            var cartID = window.localStorage.getItem("cartID");

            var customID = $(this).attr("data-custom");
            var productCode = $(this).attr("data-id");

            var formData = new FormData();
            formData.append("cartID", cartID);
            formData.append("code", productCode);
            formData.append("qty", 1);
            formData.append("custom", customID);

            apiPostData("/api/cart/UpdateCart", formData).then(function (data) {
                data = JSON.parse(data);
                $("#sub-total").html(data.totalMoney);
                $("#total").html(data.totalMoney + 25000);
                $(rows[5]).html(data.money);
                window.localStorage.setItem("totalItem", data.totalItem);
                $("#cartQTY").html(data.totalItem);
            })
        })
        }

    function getTotalMoney(carts) {
        var total = 0;
        for(item of carts) {
            total += parseInt(item.price) * parseInt(item.qty) ;
        }
        return total;
    }
})