
$("#btn_send").click(function () {
    var formData = new FormData();
    var userId = window.localStorage.getItem("UserID");
    formData.append("userId", userId);
    formData.append("postId", $("#post_id").val());
    formData.append("content", $("#content_mes").val());

    apiPostData("/api/Comment/SendComment", formData).then(function (data) {
        if (data != null) {
            var html = "<div class='col-10 mt-1 offset-1 p-2 border border-primary rounded'>" + data.Time + "<br>"
                + data.Author + ": " + data.Content + "</div>";
            if (data.UserID === parseInt(userId)) {
                html += "<div class='col-1 mt-1'><i data-id='" + data.CommentID + "' class='fa fa-trash remove_mes' onclick='initRemove(" + data.CommentID + ")'></i></div>";
            }
            $("#list_mes").append(html);
        }
        else {
            alert("Không thể comment !");
        }
    })
})

function initRemove(id) {
    var selectedItem = $(event.target).parent();
    var formData = new FormData();
    formData.append("commentId", id);

    apiPostData("/api/Comment/RemoveComment", formData).then(function (data) {
        if (data === '200') {
            $(selectedItem).prev().remove();
            $(selectedItem).remove();
        }
        else {
            alert("Không thể xóa!");
        }
    })
}

$(".remove_mes").click(function () {
    var cmtId = $(this).attr("data-id");
    var selectedItem = $(this).parent();
    var formData = new FormData();
    formData.append("commentId", cmtId);

    apiPostData("/api/Comment/RemoveComment", formData).then(function (data) {
        if (data === '200') {
            $(selectedItem).prev().remove();
            $(selectedItem).remove();
        }
        else {
            alert("Không thể xóa!");
        }
    })
})
