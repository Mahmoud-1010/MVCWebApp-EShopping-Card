var dataTable

$(document).ready(function () {
    loadData();
})

function loadData() {
    dataTable = $("#mytable").DataTable({
        "ajax": {
            "url": "/Admin/Orders/GetData"
        },
        "columns": [
            { "data": "id" },
            { "data": "applicationUser.name" },
            { "data": "applicationUser.phoneNumber" },
            { "data": "applicationUser.email" },
            { "data": "orderStatus" },
            { "data": "totalPrice" },
            
            {
                "data": "id",
                "render": function (data) {
                    return '<a href="/Admin/Orders/Details?orderid=${data}" class="btn btn-primary" > v dfvDetails</a>'
                }
            }
        ]
    });
}
//<a href="/Admin/Orders/Details?orderid=${data}" class="btn btn-primary" > Details</a>