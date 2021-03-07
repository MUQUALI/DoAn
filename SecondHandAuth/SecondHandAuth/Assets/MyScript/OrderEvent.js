
$("#btn_add_product").click(function () {
    if (validateDetail()) {
        var url = window.location.origin;
        url += "/api/Order/GetDetail";

        var size = $("#txt_size").val();
        var color = $("#txt_color").val();
        var code = $("#product_code").val();

        var formData = new FormData();
        formData.append('size', size);
        formData.append('color', color);
        formData.append('code', code);

        apiPostData(url, formData).then(function (data) {
            addDetailToTable(data, size, color);
            calculateTotal($("#body_detail").children()); // phải để vào đây vì bất đồng bộ
        })
    }
    else {
        return;
    }
});

function addDetailToTable(data, size, color) {
    
    var listDetail = $("#body_detail").children();

    if (listDetail.length > 0) {
        var rowFind = false;
        
        $(listDetail).each(function (i, item) {
            console.log($(item).children()[0].innerHTML);
            var rowCode = $(item).children()[0].innerHTML;
            var rowSize = $(item).children()[2].innerHTML;
            var rowColor = $(item).children()[3].innerHTML;
            if (rowCode === data.Code && rowSize == size && rowColor === color) {
                rowFind = item;
            }
        })
        if (rowFind) {
            var updateQty = parseInt($(rowFind).children()[5].innerHTML) + parseInt($("#txt_qty").val());
            $(rowFind).children()[5].innerHTML = updateQty;
            var updateTotal = parseInt($(rowFind).children()[5].innerHTML) * parseInt(data.BuyPrice);
            $(rowFind).children()[6].innerHTML = updateTotal;
        }
        else {
            var qty = $("#txt_qty").val();
            var total = parseInt(qty) * parseInt(data.BuyPrice);
            var html = "<tr><td>" + data.Code + "</td><td>" + data.DetailName + "</td><td>"
                + size + "</td><td>" + color + "</td><td>" + data.BuyPrice + "</td><td>" + qty
                + "</td><td>" + total
                + "</td><td><a href='javascript:void(0)' onclick='deleteDetail(this)' ><i class='fa fa-trash'></i></a>" + "</td></tr>";

            $("#body_detail").append(html);
        }
    }
    else {
        var qty = $("#txt_qty").val();
        var total = parseInt(qty) * parseInt(data.BuyPrice);
        var html = "<tr><td>" + data.Code + "</td><td>" + data.DetailName + "</td><td>"
            + size + "</td><td>" + color + "</td><td>" + data.BuyPrice + "</td><td>" + qty
            + "</td><td>" + total
            + "</td><td><a href='javascript:void(0)' onclick='deleteDetail(this)' ><i class='fa fa-trash'></i></a>" + "</td></tr>";

        $("#body_detail").append(html);
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
    url += "/api/Product/GetProductName";
    let args = {
        code : $("#product_code").val()
    }
    apiGetData("GET", url, args).then(function (data) {
        $("#txt_name").val(data);
        if (data !== "Không tồn tại mã sản phẩm !") {
            $("#btn_add_product").attr("disabled", false);
        }
    });
})

function validateDetail() {
    var qty = $("#txt_qty").focus;
    var code = $("#product_code").val();
    var qty = $("#txt_qty").val();
    var size = $("#txt_size").val();
    var color = $("#txt_color").val();
    var name = $("#txt_name").val();

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

    if (qty.match(/\D/g)) {
        alert("Số lượng chỉ nhập số nguyên !");
        $("#txt_qty").focus();
        return false;
    }

    if (!size) {
        alert("Hãy nhập size !");
        return false;
    }

    if (size.match(/\D/g)) {
        alert("Size chỉ nhập số nguyên !");
        $("#txt_size").focus();
        return false;
    }

    if (!color) {
        alert("Hãy nhập màu !");
        return false;
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