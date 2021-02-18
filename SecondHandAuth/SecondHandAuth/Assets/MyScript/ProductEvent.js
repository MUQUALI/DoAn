var inputFile = $("#images_upload")

$(inputFile).change(function () {
    if (FileReader) {
        
        var uploads = $(inputFile).prop("files");
        if(uploads.length > 4) {
            alert("Tối đã được chọn 4 ảnh cho 1 sản phẩm");
            return;
        }
        // contructor file reader
        var reader = new FileReader();
        //set Event for reader
        reader.onload = function(event) {
            $("#image_main").attr("src", event.target.result);
        }
        // set upload[0] for main image
        reader.readAsDataURL(uploads[0]);
        // clear list sub upload
        $("#list_upload").empty();
        $(uploads).each(function (index, image) {
            // create subReader
            var subReader = new FileReader();
            // event sub reader
            subReader.onload = function (event) {
                if (index == 0) {
                    setSubImageUpload(event.target.result, true);
                }
                else {
                    setSubImageUpload(event.target.result);
                }

            }
            subReader.readAsDataURL(image);
        });
        
    }
    else {
        alert("Trình duyệt của bạn không hỗ trợ xem trước ảnh !");
    }
})

function setSubImageUpload(imageUrl, active) {
    if (active) {
        var imgTag = "<img class='sub_upload active' onclick='changeMainImg(this)' src='" + imageUrl + "'/>";
        $("#list_upload").append(imgTag);
        return;
    }
    var imgTag = "<img class='sub_upload' onclick='changeMainImg(this)' src='" + imageUrl + "'/>";
    $("#list_upload").append(imgTag);
}

$("#btn_reset").click(function () {
    $("#list_upload").empty();
    $("#err_dup").addClass("d-none");;
    $("#btn_save").attr("disabled", false);
    $("#image_main").attr("src", "/Assets/img/upload_img.jpg");
});

// event change main image
// không thể cho funtion này chạy bên ngoài vì các img chưa load song, do bất đồng bộ
function changeMainImg(img) {
    $("#image_main").attr("src", img.src);

    var oldItem = $(".active")[0];
    oldItem.classList.remove("active");

    img.className += " active";
}

// validate form create product
function validate() {
    var uploads = $(inputFile).prop("files");
    var chilImgs = $("#list_upload").children();
    if (uploads.length <= 0 && $(chilImgs).length <= 0) {
        alert("Bạn phải chọn ít nhất 1 ảnh cho sản phẩm");
        return false;
    }

    if ($("#txt_code").val() == "") {
        alert("Mã sản phẩm bắt buộc nhập");
        $("#txt_code").focus();
        return false;
    }

    if ($("#txt_name").val() == "") {
        alert("Tên sản phẩm bắt buộc nhập");
        $("#txt_name").focus();
        return false;
    }

    if ($("#cmb_type").val() == "") {
        alert("Kiểu dáng bắt buộc nhập");
        $("#cmb_type").focus();
        return false;
    }

    if ($("#cmb_firm").val() == "") {
        alert("Hãng sản xuất bắt buộc nhập");
        $("#cmb_firm").focus();
        return false;
    }

    if ($("#buy_price").val() == "") {
        alert("Giá mua bắt buộc nhập");
        $("#buy_price").focus();
        return false;
    }

    if ($("#sale_price").val() == "") {
        alert("Giá bán bắt buộc nhập");
        $("#sale_price").focus();
        return false;
    }
    return true;
}

// check duplicate code
$("#txt_code").focusout(function () {
    var codeVal = $("#txt_code").val();
    var url = "http://" + location.host + "/api/Product/DuplicateCode";

    apiGetData("GET", url, { code: codeVal }).then(function (data) {
        if (data == "300") {
            $("#err_dup").html("đã tồn tại mã sản phẩm");
            $("#err_dup").removeClass("d-none");
            $("#btn_save").attr("disabled", true);
        }
        else {
            $("#err_dup").html(data);
            $("#err_dup").addClass("d-none");;
            $("#btn_save").attr("disabled", false);
        }
    });
});

// click save button
$("#btn_save").click(function () {
    if (!validate()) {
        return;
    }

    var codeVal = $("#txt_code").val();
    var url = "http://" + location.host + "/api/Product/DuplicateCode";

    apiGetData("GET", url, { code: codeVal }).then(function (data) {
        if (data == "300") {
            $("#err_dup").html("đã tồn tại mã sản phẩm");
        }
        else {
            $("#err_dup").html(data);
            
            $("#frm_create").submit();
        }
    })
});

// validate kiểu dáng
$("#btn_save_type").click(function () {
    if (!$("#txt_typename").val()) {
        alert("Tên kiểu dáng là bắt buộc !");
        return;
    }
    $("#type_form").submit();
})

// validate hãng
$("#btn_save_firm").click(function () {
    if (!$("#txt_firm").val()) {
        alert("Tên hãng là bắt buộc !");
        return;
    }
    $("#firm_form").submit();
})

// click btn_edit
$("#btn_edit").click(function () {
    $("#images_upload").attr("disabled", false);
    $("#txt_name").attr("disabled", false);
    $("#cmb_type").attr("disabled", false);
    $("#cmb_firm").attr("disabled", false);
    $("#buy_price").attr("disabled", false);
    $("#sale_price").attr("disabled", false);
    $("#status").attr("disabled", false);
    $("#description").attr("disabled", false);

    $("#btn_save_edit").attr("disabled", false);
})

// click save edit
$("#btn_save_edit").click(function () {
    if (!validate()) {
        return;
    }

    $("#frm_create").submit();
});
