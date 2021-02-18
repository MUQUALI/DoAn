const LIMIT = 5;
const DISPLAY_PAGE = 5;
const URL = '/api/Account/ReadPagination';

$("#btn_save").click(function () {
    if (!validateFormAccount()) {
        return;
    }
    var formData = $("#account_form");
    var url;
    if ($('#txt_id').val()) {
        url = '/api/Account/Edit';
    }
    else {
        url = '/api/Account/Create';
    }

    $.ajax({
        type: 'Post',
        url: url,
        data: formData.serialize(),
        success: function (data) {
            if (data === '200') {
                formData.trigger("reset");
                $("#modal_account").modal('hide');
                alert("Thêm mới thành công !");
                renderPage();
            }
            else if (data === '202') {
                formData.trigger("reset");
                $("#modal_account").modal('hide');
                alert("Cập nhật thành công !");
                renderPage();
            }
            else if (data === '300') {
                alert("Username đã tồn tại !");
            }
            else if (data === '201') {
                alert("Không tôn tại mã nhân viên!");
            }
            else {
                alert("Đã có lỗi gì đó !");
            }
        }
    })
})

function validateFormAccount() {
    var ipUsername = $("#txt_username");
    var ipPassword = $("#txt_password");
    var specials = ipUsername.val().match(/\W/g);
    if (specials) {
        alert("username ko được chứa ký tự đặc biệt như là: " + specials);
        return false;
    }
    if (!ipUsername.val()) {
        alert("hãy nhập username !");
        ipUsername.focus();
        return false;
    }
    if (!ipPassword.val() && !$('#txt_id').val()) {
        alert("hãy nhập password !");
        ipPassword.focus();
        return false;
    }
    return true;
}


function renderPage(key = '') {
    $('.pagination').html('');
    $.ajax({
        type: 'GET',
        url: '/api/Account/GetTotalRecord',
        data: {
            key: key
        },
        success: function (totalRecord) {
            initPagination(totalRecord, DISPLAY_PAGE, LIMIT, key);
        }
    })
}

function renderData(data) {
    $("#area_render").children().remove();
    $(data).each(function (index, item) {
        var tr = $('<tr></tr>');
        var tdUsername = $('<td>' + item.Username + '</td>');
        var tdRule = '';
        if (item.FK_RuleID == 1) {
            tdRule = $('<td>Chủ cửa hàng</td>');
        }
        else if (item.FK_RuleID == 2) {
            tdRule = $('<td>Phó cửa hàng</td>');
        }
        else if (item.FK_RuleID == 3) {
            tdRule = $('<td>Nhân viên</td>');
        }
        else {
            tdRule = $('<td>Khách hàng</td>');
        }
        var tdDes = $('<td>' + item.Description + '</td>');
        var tdMaKH;
        var tdMaNV;
        if (item.FK_CustomerID == null) {
            tdMaKH = $('<td></td>');
        }
        else {
            tdMaKH = $('<td>' + item.FK_CustomerID + '</td>');
        }
        if (item.FK_EmpID == null) {
            tdMaNV = $('<td></td>');
        }
        else {
            tdMaNV = $('<td>' + item.FK_EmpID + '</td>');
        }
        var tdAction = $('<td>'
            + '<a href="#"><i class="fas fa-eye" title="chi tiết"></i></a>' +
            '&nbsp <a class="btn_edit" data-id="' + item.PK_AccountID +'" href="javascript:void(0)"><i class="fas fa-edit" title="sửa"></i></a>' +
            '&nbsp <a class="btn_delete" data-id="' + item.PK_AccountID + '" href="javascript:void(0)"><i class="fas fa-trash" title="xóa"></i></a>' +
            '</td>')
        // append
        $(tr).append(tdUsername);
        $(tr).append(tdRule);
        $(tr).append(tdDes);
        $(tr).append(tdMaNV);
        $(tr).append(tdMaKH);
        $(tr).append(tdAction);
        $('#area_render').append(tr);
    })
    editPopup();
    deletePopup();
}

renderPage();

// edit account
function editPopup() {
    $('.btn_edit').click(function () {
        $('#txt_username').attr('disabled', true);

        var row = $(this).parents('tr').children();
        var id = $(this).attr('data-id');
        setEditPopup(row, id);

        $('#modal_account').modal('show');
    })
}

function setEditPopup(row, id) {
    $("#account_form").trigger('reset');
    $('#txt_username').val($(row[0]).html());
    var textRule = $(row[1]).html();
    if (textRule == "Nhân viên") {
        $('#sl_rule').val(3);
    }
    else if (textRule == "Phó cửa hàng") {
        $('#sl_rule').val(2);
    }
    else {
        $('#sl_rule').val(4);
    }
    $('#txt_description').val($(row[2]).html());
    $('#txt_nhanvien').val($(row[3]).html());
    $('#txt_id').val(id);
}

function showCreatePopup() {
    $("#account_form").trigger('reset');
    $('#txt_username').attr('disabled', false);
    $('#modal_account').modal('show');
}

function deletePopup() {
    $('.btn_delete').click(function () {
        var ask = confirm("Bạn có chắc muốn xóa chứ ?");
        if (ask) {
            //var id = parseInt($(this).attr('data-id'));
            var id = $(this).attr('data-id');
            var formData = new FormData();
            formData.append('ID', id);
            $.ajax({
                type: 'POST',
                data: JSON.stringify({ ID: id }),
                url: '/api/Account/Delete',
                contentType: 'application/json',
                success: function (data) {
                    if (data === '200') {
                        alert('xóa thành công !');
                        renderPage();
                    }
                    else {
                        alert('Có lỗi gì đó !');
                    }
                }
            })
        }
    })
}

$("#btn_search").click(function () {
    renderPage($("#search_area").val());
})




