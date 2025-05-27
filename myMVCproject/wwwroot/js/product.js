var dataTable;

$(document).ready(function () {
    //console.log("test")
    loadDataTable();
})

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/admin/product/getall"
        },
        "columns": [
            { data: 'name', "width": "15%" },
            { data: 'description', "width": "15%" },
            { data: 'price', "width": "15%" },
            { data: 'category.name', "width": "10%" },
            { data: 'publisher', "width": "15%" },
            { data: 'publishDate', "width": "10%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="d-flex justify-content-center align-items-left gap-3">
                    <a href="/admin/product/upsert?id=${data}" class="btn btn-primary mx-2" style="min-width: 120px;"> <i class="bi bi-pencil-square"></i> Edit</a>
                    <a onClick=Delete('/admin/product/delete/${data}') class="btn btn-danger mx-2" style="min-width: 120px;"> <i class="bi bi-trash-fill"></i> Delete</a>
                    </div>`
                },
                "width": "25%"
            }
        ]
    });
}

function Delete(url) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        Swal.fire(
                            "Deleted!",
                            data.message,
                            "success"
                        );
                        dataTable.ajax.reload();
                    }
                    else {
                        Swal.fire(
                            "Error!",
                            data.message,
                            "error"
                        );
                    }
                }
            });
        }
    });
}