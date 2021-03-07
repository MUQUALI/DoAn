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

function apiPostData(url, formData) {
    let promise = new Promise(function (resolve, reject) {
        let xhttp = new XMLHttpRequest();
        xhttp.onreadystatechange = function () {
            if (this.readyState == 4 && this.status == 200) {
                let res = JSON.parse(this.responseText);
                resolve(res);
            }

        }

        xhttp.open("POST", url, true);
        //xhttp.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
        xhttp.send(formData);
    });
    return promise;
}

function apiPostJsonData(url, data) {
    // post data json
    let promise = new Promise(function (resolve, reject) {
        let xhttp = new XMLHttpRequest();
        xhttp.onreadystatechange = function () {
            if (this.readyState == 4 && this.status == 200) {
                let res = JSON.parse(this.responseText);
                resolve(res);
            }

        }

        xhttp.open("POST", url, true);
        xhttp.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
        xhttp.send(data);
    });
    return promise;
}

function apiPostFormData(url, formData) {
    let promise = new Promise(function (resolve, reject) {
        let xhttp = new XMLHttpRequest();
        xhttp.onreadystatechange = function () {
            if (this.readyState == 4 && this.status == 200) {
                let res = JSON.parse(this.responseText);
                resolve(res);
            }

        }

        xhttp.open("POST", url, true);
        xhttp.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
        xhttp.send(formData);
    });
    return promise;
}