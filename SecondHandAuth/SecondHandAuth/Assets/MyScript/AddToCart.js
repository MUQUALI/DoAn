
$("#add-to-cart").click(function () {
    var userId = window.localStorage.getItem("UserID");
    apiGetData("GET", "/api/Cart/CheckLogin", {UserId: userId}).then(async function (data) {
        if (data.Username == null) {
            // chưa đăng nhập
            var carts = [];
            var item = await getItemAddToCart();
            if (!window.localStorage.getItem("myCart")) {
                carts.push(item);
                window.localStorage.setItem("myCart", JSON.stringify(carts));
                window.localStorage.setItem("totalItem", item.qty);
                $("#cartQTY").html(item.qty);
            }
            else {
                carts = JSON.parse(window.localStorage.getItem("myCart"));

                updateCart(carts, item);
            }
        }
        // đã đăng nhập
        else {
            var carts = JSON.parse(window.localStorage.getItem("myCart"));
            var itemAdd = await getItemAddToCart();
            // có giỏ hàng trong localStorage
            if(carts) {
                updateCart(carts, itemAdd);
                carts = JSON.parse(window.localStorage.getItem("myCart"));

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
                })
            }
            else {
                var cartID = window.localStorage.getItem("cartID");
                var itemEdit= await getItemAddToCart();
                // đã đăng nhập, có giỏ hàng trong db
                if(cartID) {
                    var Model = {
                        CartID: cartID,
                        ItemAddToCart: itemEdit
                    }

                    apiPostJsonData("/api/Cart/EditCart", JSON.stringify(Model)).then(function(data) {
                        var objData = JSON.parse(data);
                        $("#cartQTY").html(objData.totalItem);
                        window.localStorage.setItem("totalItem", objData.totalItem);
                        window.localStorage.setItem("cartID", objData.cartID);
                    });
                }
                // đã đăng nhập, chưa có giỏ hàng trong db
                else {
                    var Model = {
                        UserID: window.localStorage.getItem("UserID"),
                        ItemAddToCart: itemEdit,
                        TotalMoney: parseInt(itemEdit.price) * parseInt(itemEdit.qty)
                    }

                    apiPostJsonData("/api/Cart/AddToCart", JSON.stringify(Model)).then(function(data) {
                        var objData = JSON.parse(data);
                        $("#cartQTY").html(objData.totalItem);
                        window.localStorage.setItem("totalItem", objData.totalItem);
                        window.localStorage.setItem("cartID", objData.cartID);
                    })
                }
            }
        }
    })
})

async function getItemAddToCart() {
    var productID = $("#product-id").val();
    var productName = $("#product-name").val();
    var price = await apiGetData("GET", "/api/Product/GetPriceOfProduct", {id: productID});
    var custom = $("#sl-custom").val();
    var qty = $("#txtQty").val();
    
    var product = {
        productID: productID,
        productName: productName,
        price: price,
        custom: custom,
        qty: qty
    }
    return product;
}

function updateCart(carts, item) {
    var itemIndex = carts.findIndex(function(cartItem) {
        return cartItem.code === item.code && cartItem.custom == item.custom;
    });
    if(itemIndex >= 0) {
        carts[itemIndex].qty = parseInt(carts[itemIndex].qty) + parseInt(item.qty);
        var updateTotal = parseInt(window.localStorage.getItem("totalItem")) + parseInt(item.qty);
        window.localStorage.setItem("totalItem", updateTotal);
        $("#cartQTY").html(updateTotal);
    }
    else {
        carts.push(item);
        var updateTotal = parseInt(window.localStorage.getItem("totalItem")) + parseInt(item.qty);
        window.localStorage.setItem("totalItem", updateTotal);
        $("#cartQTY").html(updateTotal);
    }
    window.localStorage.setItem("myCart", JSON.stringify(carts));
}

function getTotalMoney(carts) {
    var total = 0;
    for(item of carts) {
        total += parseInt(item.price) * parseInt(item.qty) ;
    }
    return total;
}
