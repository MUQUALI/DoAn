$(document).ready(async function() {
    /*async function getRevenueOfYear(year) {
        let result = await apiGetData("GET", "api/Report/GetSalesOfYear", {year: year});
        return result;
    }*/

    var ctx = document.getElementById('saleChart');
    var revenueOfYearData = await apiGetData("GET", "/api/Report/GetSalesOfYear", {year: 2021});
    var costOfYearData = await apiGetData("GET", "/api/Report/GetCostsOfYear", {year: 2021});
    var profitOfYearData = await apiGetData("GET", "/api/Report/GetProfitOfYear", {year: 2021});
    console.log(revenueOfYearData);
    var myChart = new Chart(ctx, {
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
                text: 'Biểu đồ doanh thu trong năm'
            }
        }
    });
})


