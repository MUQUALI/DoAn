function apiGetData(method, url, args) {
    let promise = new Promise(function (resolve, reject) {
        let xhttp = new XMLHttpRequest();
        xhttp.onreadystatechange = function () {
            if (this.readyState == 4 && this.status == 200) {
                let res = JSON.parse(this.responseText);
                resolve(res);
            }

        }
        if (args && method == "GET") {
            url += "?";
            for (key in args) {
                url += encodeURIComponent(key) + '=' + encodeURIComponent(args[key]) + "&";
            }
            url = url.substring(0, url.length - 1);
        }
        xhttp.open(method, url, true);
        xhttp.send();
    });
    return promise;
}