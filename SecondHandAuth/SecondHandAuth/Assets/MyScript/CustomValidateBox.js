$(document).ready(function() {
    var validateBox = $(".err-mes");
    validateBox.each(function (index) {
        if (!validateBox[index].innerHTML) {
            validateBox[index].style.display = "none";
        }
    })
})