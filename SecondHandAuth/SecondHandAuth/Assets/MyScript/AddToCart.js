
$("#add-to-cart").click(function () {
    var userId = window.localStorage.getItem("UserID");
    apiGetData("GET", "/api/Cart/CheckLogin", {UserId: userId}).then(async function (data) {
        if (data.Username == null) {
            // chưa đăng nhập
            var carts = [];
            var item = await getItemAddToCart();
            var maxQty = await apiGetData("GET", "/api/Cart/GetMaxQtyOfProduct", {ProductID: item.productID, FK_Custom: item.custom});
            if(item.qty > maxQty) {
                alert("Bạn đã mua quá số lượng cho phép!");
                return;
            }

            if (!window.localStorage.getItem("myCart")) {
                carts.push(item);
                window.localStorage.setItem("myCart", JSON.stringify(carts));
                window.localStorage.setItem("totalItem", item.qty);
                $("#cartQTY").html(item.qty);
            }
            else {
                carts = JSON.parse(window.localStorage.getItem("myCart"));

                var inCart = carts.find(data => data.productID == item.productID &&  data.custom == item.custom);
                if(item.qty > (maxQty - inCart.qty)) {
                    alert("không thể mua thêm nữa !");
                    return;
                }
                updateCart(carts, item);
            }
        }
        // đã đăng nhập
        else {
            var carts = JSON.parse(window.localStorage.getItem("myCart"));
            var itemAdd = await getItemAddToCart();
            // check max qty
            var maxQty = await apiGetData("GET", "/api/Cart/GetMaxQtyOfProduct", {ProductID: itemAdd.productID, FK_Custom: itemAdd.custom});
            
            // có giỏ hàng trong localStorage
            if(carts) {
                updateCart(carts, itemAdd);
                carts = JSON.parse(window.localStorage.getItem("myCart"));

                var inCart = carts.find(data => data.productID == item.productID);
                if(item.qty > (maxQty - inCart.qty)) {
                    alert("Bạn đã mua quá số lượng cho phép!");
                    return;
                }

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
                        if(objData.errQty) {
                            alert(objData.errQty);
                            return;
                        }
                        $("#cartQTY").html(objData.totalItem);
                        window.localStorage.setItem("totalItem", objData.totalItem);
                        window.localStorage.setItem("cartID", objData.cartID);
                    });
                }
                // đã đăng nhập, chưa có giỏ hàng localStoarage và db
                else {
                    var Model = {
                        UserID: window.localStorage.getItem("UserID"),
                        ItemAddToCart: itemEdit,
                        TotalMoney: parseInt(itemEdit.price) * parseInt(itemEdit.qty)
                    }

                    if(itemEdit.qty > maxQty) {
                        alert("Bạn đã mua quá số lượng cho phép!");
                        return;
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

    if (qty.match(/\D/g)) {
        alert("Số lượng chỉ nhập số nguyên dương !");
        $("#txtQty").focus();
        return false;
    }
    
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

$("#btn-buy").click(async function () {
    //window.sessionStorage.setItem("address", $("#txt_address").val());
    if(!window.localStorage.getItem("UserID")) {
        window.location.href = window.location.origin + "/Cart/Index";
    }
    // có giỏ hàng nhưng số lượng sp = 0
    if(window.localStorage.getItem("cartID") && window.localStorage.getItem("totalItem") === "0") {
       $("#add-to-cart").trigger("click");
    }

    if(window.localStorage.getItem("cartID") && window.localStorage.getItem("totalItem") !== "0") {
        window.location.href = window.location.origin + "/Cart/Index";
    }
    else {
        var ItemAdd = await getItemAddToCart();
        var maxQty = await apiGetData("GET", "/api/Cart/GetMaxQtyOfProduct", {ProductID: ItemAdd.productID, FK_Custom: ItemAdd.custom});

        if(ItemAdd.qty > maxQty) {
            alert("Hạn mức mua tối đa là: " + maxQty);
            return;
        }
        var Model = {
            UserID: window.localStorage.getItem("UserID"),
            ItemAddToCart: ItemAdd,
            TotalMoney: parseInt(ItemAdd.price) * parseInt(ItemAdd.qty)
        }

        apiPostJsonData("/api/Cart/AddToCart", JSON.stringify(Model)).then(function(data) {
            var objData = JSON.parse(data);
            $("#cartQTY").html(objData.totalItem);
            window.localStorage.setItem("totalItem", objData.totalItem);
            window.localStorage.setItem("cartID", objData.cartID);
            window.location.href = window.location.origin + "/Cart/Index";
        })
    }
})
