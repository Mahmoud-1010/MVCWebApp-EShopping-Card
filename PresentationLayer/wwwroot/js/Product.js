var dataTable

$(document).ready(function () {
    loadData();
})

function loadData() {
    dataTable = $(#productTable).dataTable({
        "ajax": {
            "url":"/Admin/Orders/GetData"
        },
        "columns": [
            {"data":"name" },
            {"data":"description" },
            {"data":"price" },
            {"data":"categoryId" }
        ]
    });
}