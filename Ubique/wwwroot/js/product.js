$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/product/getall' },
        "columns": [
            { data: 'name', "width": "25%" },
            { data: 'brand', "width": "15%" },
            { data: 'price', "width": "10%" },
            { data: "subCategory.name", "width": "20%" },
            { data: 'subCategory.category.name', "width": "15%" }
        ]
    });
}