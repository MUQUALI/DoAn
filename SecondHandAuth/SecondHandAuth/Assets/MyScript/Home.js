
function nextItem(button) {
    var countNext = $(button).attr("data-next");
    countNext++;

    var listItems = $(button).prev().children();

    var totalItem = listItems.length;
    var lastPage = Math.ceil(totalItem / 4);

    if (countNext > lastPage) {
        $(listItems)[0].style.marginLeft = "0%";
        countNext = 1;
        $(button).attr("data-next", countNext);
        return;
    }

    var percent = parseInt($(listItems)[0].style.marginLeft);

    if (percent) {
        percent -= 50;
        percent += "%";
    }
    else {
        percent = "-50%";
    }
    $(button).attr("data-next", countNext);
    $(listItems)[0].style.marginLeft = percent;
}