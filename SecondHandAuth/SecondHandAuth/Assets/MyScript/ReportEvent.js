$(document).ready(function() {
    var curentDate = new Date();
    var curentYear = curentDate.getFullYear();
    var curentMonth = curentDate.getMonth() + 1;

    var ctx = document.getElementById('saleChart');
    var saleChart2 = document.getElementById('saleChart2');

    var mychart1;
    var mychart2;

    renderSaleChart1(curentYear);

    renderSaleChart2(curentMonth, curentYear);

    async function renderSaleChart1(year) {
        var revenueOfYearData = await apiGetData("GET", "/api/Report/GetSalesOfYear", {year: year});
        var costOfYearData = await apiGetData("GET", "/api/Report/GetCostsOfYear", {year: year});
        var profitOfYearData = await apiGetData("GET", "/api/Report/GetProfitOfYear", {year: year});

        mychart1 = new Chart(ctx, {
            type: 'line',
            data: {
                labels: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'],
                datasets: [
                    {
                        label: 'Doanh thu theo tháng',
                        data: revenueOfYearData,
                        fill: false,
                        borderColor: 'rgb(75, 192, 192)',
                        lineTension: 0,
                        order: 1
                    },
                    {
                        label: 'Chi phí theo tháng',
                        data: costOfYearData,
                        fill: false,
                        borderColor: 'red',
                        lineTension: 0,
                        order: 2
                    },
                    {
                        label: 'lợi nhuận theo tháng',
                        data: profitOfYearData,
                        fill: false,
                        borderColor: 'green',
                        lineTension: 0,
                        order: 3
                    }
                ]
            },
            options: {
                scales: {
                    yAxes: [{
                        ticks: {
                            beginAtZero: true
                        }
                    }]
                },
                title: {
                    display: true,
                    text: 'Biểu đồ doanh thu trong năm ' + year
                }
            }
        });
    }

    async function renderSaleChart2(month, year) {
        var productLabels = await apiGetData("GET", "/api/Report/getListLableOfProduct", {month: month, year: year});
        var dataProduct = await apiGetData("GET", "/api/Report/GetQtySaleProduct", {month: month, year: year});
        mychart2 = new Chart(saleChart2, {
            type: 'bar',
            data: {
                labels: productLabels,
                datasets: [
                    {
                        label: 'Lượng bán',
                        data: dataProduct,
                        fill: false,
                        borderColor: 'rgb(75, 192, 192)',
                        backgroundColor: 'rgba(255, 159, 64, 0.2)',
                        lineTension: 0,
                        order: 1
                    }
                ]
            },
            options: {
                scales: {
                    yAxes: [{
                        ticks: {
                            beginAtZero: true
                        }
                    }]
                },
                title: {
                    display: true,
                    text: 'Biểu đồ lưu lượng hàng bán trong tháng ' + month
                }
            }
        });
    }

    $("#btn_filter").click(function() {
        // clear old data

        var month = $("#sl_month").val();
        var year = $("#sl_year").val();

        if(year != '') {
            mychart1.destroy();
            renderSaleChart1(year);
        }
        mychart2.destroy();
        renderSaleChart2(month, year);
    })
})



