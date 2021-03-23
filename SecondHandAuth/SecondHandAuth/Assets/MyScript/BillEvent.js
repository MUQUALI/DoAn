
$("#btn_add_product").click(function () {
    if (validateDetail()) {
        var url = window.location.origin;
        url += "/api/Bill/AddInfoProduct";

        var qty = $("#txt_qty").val();
        var custom = $("#sl-custom").val();
        var code = $("#product_code").val();

        var data = {
            code: code,
            custom: custom,
            qty: qty
        }

        apiGetData("GET", url, data).then(function (data) {
            addDetailToTable(data);
            calculateTotal($("#body_detail").children()); // phải để vào đây vì bất đồng bộ
        })
    }
    else {
        return;
    }
});

function addDetailToTable(data) {
    
    var listDetail = $("#body_detail").children();

    if (listDetail.length > 0) {
        var rowFind = false;
        
        $(listDetail).each(function (i, item) {
            console.log($(item).children()[0].innerHTML);
            var rowCode = $(item).children()[0].innerHTML;
            var rowSize = $(item).children()[2].innerHTML;
            var rowColor = $(item).children()[3].innerHTML;
            if (rowCode === data.ProductCode && rowSize === (data.Size + "") && rowColor === data.Color) {
                // cộng thêm chuỗi vì data.Size là số
                rowFind = item;
            }
        })
        if (rowFind) {
            var updateQty = parseInt($(rowFind).children()[5].innerHTML) + parseInt(data.Quantity);
            $(rowFind).children()[5].innerHTML = updateQty;
            var updateTotal = parseInt($(rowFind).children()[5].innerHTML) * parseInt(data.UnitPrice);
            $(rowFind).children()[6].innerHTML = updateTotal;
        }
        else {
            var html = "<tr><td>" + data.ProductCode + "</td><td>" + data.ProductName + "</td><td>"
                + data.Size + "</td><td>" + data.Color + "</td><td>" + data.UnitPrice + "</td><td>" + data.Quantity
                + "</td><td>" + data.Money
                + "</td><td><a href='javascript:void(0)' onclick='deleteDetail(this)' ><i class='fa fa-trash'></i></a>" + "</td></tr>";

            $("#body_detail").append(html);
        }
    }
    else {
        var html = "<tr><td>" + data.ProductCode + "</td><td>" + data.ProductName + "</td><td>"
                + data.Size + "</td><td>" + data.Color + "</td><td>" + data.UnitPrice + "</td><td>" + data.Quantity
                + "</td><td>" + data.Money
                + "</td><td><a href='javascript:void(0)' onclick='deleteDetail(this)' ><i class='fa fa-trash'></i></a>" + "</td></tr>";

        $("#body_detail").html(html);
    }
    
}

function calculateTotal(rows) {
    var qtyTotal = 0; // tính tổng số lượng ở hàng cuối
    var moneyTotal = 0; //  tính tổng tiền hàng cuối

    $(rows).each(function (index, row) {
        qtyTotal += parseInt($(row).children()[5].innerHTML);
        moneyTotal += parseInt($(row).children()[6].innerHTML);
    });

    var rowTotal = "<tr class='text-danger'><th>Tổng Cộng</th><th></th><th></th><th></th><th></th><th id='qty_total'>"
            + qtyTotal + "</th><th id='monney_total'>" + moneyTotal + "</th></tr>";

    $("#total_detail").html(rowTotal);
}

function deleteDetail(icon) {
    var cf = confirm("Bạn muốn xóa dòng này ?");
    if (cf) {
        $(icon).parent().parent().remove();
        calculateTotal($("#body_detail").children());
    }
    else {
        return;
    }
}

$("#product_code").focusout(function () {
    var url = window.location.origin;
    url += "/api/Bill/GetProductInfo";
    let args = {
        code : $("#product_code").val()
    }
    apiGetData("GET", url, args).then(function (data) {
        if (data == null) {
            $("#txt_name").val("Không tồn tại mã sản phẩm !");
            $("#btn_add_product").attr("disabled", true);
            return;
        }
        $("#txt_name").val(data.ProductName);

        var html = "";
        if (data.ListCustomOfProduct.length > 0) {
            $(data.ListCustomOfProduct).each(function (index, item) {
                html += "<option value='" + item.CustomID + "'>" +item.Size + " - " + item.Color + "</option>"
            })
            $("#sl-custom").html(html);
        }
        $("#btn_add_product").attr("disabled", false);
    });
})

$("#txt_qty").focusout(function () {
    var curentQty = parseInt($("#txt_qty").val());

    var url = window.location.origin;
    url += "/api/Bill/GetMaxQtyOfProduct";
    let args = {
        code: $("#product_code").val(),
        custom: $("#sl-custom").val()
    }
    apiGetData("GET", url, args).then(function (data) {
        if (curentQty > data) {
            alert("Hạn mức mua là: " + data);
            $("#btn_add_product").attr("disabled", true);
            return;
        }
        $("#btn_add_product").attr("disabled", false);
    })
})

async function validateDetail() {
    $("#product_code").focusout;

    var url = window.location.origin;
    url += "/api/Bill/GetMaxQtyOfProduct";
    let args = {
        code: $("#product_code").val(),
        custom: $("#sl-custom").val()
    }
    var maxQty = await apiGetData("GET", url, args);
    

    var code = $("#product_code").val();
    var qty = $("#txt_qty").val();
    var name = $("#txt_name").val();
    var custom = $("#sl-custom").val();

    if(parseInt(maxQty) > parseInt(qty)) {
        return false;
    }

    if (!code) {
        alert("Hãy nhập mã sản phẩm !");
        return false;
    }

    if (name === "Không tồn tại mã sản phẩm !") {
        alert("Vui lòng nhập đúng mã sản phẩm !");
        return false;
    }

    if (!qty) {
        alert("Hãy nhập số lượng !");
        return false;
    }

    if (qty == 0) {
        alert("Số lượng phải khác 0");
        return false;
    }

    if (qty.match(/\D/g)) {
        alert("Số lượng chỉ nhập số nguyên !");
        $("#txt_qty").focus();
        return false;
    }
    if (!custom) {
        alert("hãy chọn Size - Màu !");
    }

    return true;
}

function validateCreateOrder() {
    if (!$("#txt_sup").val()) {
        alert("Bạn chưa điền nhà cung cấp !");
        return false;
    }

    if ($("#body_detail").children().length <= 0) {
        alert("Bạn chưa thêm sản phẩm nào");
        return false;
    }

    return true;
}

// click save button
$("#btn_save").click(function () {
    if (!validateCreateOrder()) {
        return;
    }

    var form = $("#frm_order")[0];
    // listDetail
    var listDetail = [];

    $("#body_detail").children().each(function (i, row) {
        let itemDetail = {
            ProductID: $(row).children()[0].innerHTML,
            ProductName: $(row).children()[1].innerHTML,
            Size: $(row).children()[2].innerHTML,
            Color: $(row).children()[3].innerHTML,
            UnitPrice: $(row).children()[4].innerHTML,
            Quantity: $(row).children()[5].innerHTML,
            Money: $(row).children()[6].innerHTML,
        }
        listDetail.push(itemDetail);
    });

    form.elements["ListDetail"].value = JSON.stringify(listDetail);
    form.elements["TotalMoney"].value = $("#monney_total").html();
    form.submit();
})

// click xem chi tiết hóa đơn
$(".ic_detail").click(function (e) {
    $("tr").removeClass("bg-warning");

    var rowSelect = $(this).parent().parent();
    $(rowSelect).addClass("bg-warning");

    var id = $(rowSelect).children()[0].innerHTML;

    $("#head_detail").removeClass("invisible");

    var url = window.location.origin;
    url += "/api/Order/GetDetaislFromID";

    apiGetData("GET", url, { ID: id }).then(function (data) {
        var rowHtml = "";
        for (item of data) {
            rowHtml += "<tr><td>" + item.ProductCode + "</td><td>" + item.ProductName
                + "</td><td>" + item.Size + "</td><td>" + item.Color + "</td><td>" + item.UnitPrice
                + "</td><td>" + item.Quantity + "</td><td>" + item.Money + "</td></tr>";
        }
        $("#body_detail").html(rowHtml);

        calculateTotal($("#body_detail").children());
    })
})