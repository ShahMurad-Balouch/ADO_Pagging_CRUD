$(document).ready(function () {
    $('#myTable').DataTable({
        "serverSide": true,
        "processing": true,
        "paging": true,
        "ajax": {
            "url": "/Home/Indexx",
            "type": "POST",
            "dataType": "json",
            "data": function (d) {
                // Modify DataTables default request parameters if needed
                d.start = 2;
                d.length = 14;
                d.search.value = d.search.value || '';
                d.order[0].column = d.order[0].column || 0;
                d.order[0].dir = d.order[0].dir || 'asc';
            },
            "dataSrc": function (da) {
                // Ensure data is received and processed
                console.log('Data received:', da);
                return da.data; // Assuming the server response contains an array of data under the "data" property
            },
            "error": function (xhr, status, error) {
                console.error('Error status:', status);
                console.error('Error message:', error);
                console.error('Response:', xhr.responseText);
            }
        },
        "columns": [
            { "data": "Stu_Id", "name": "Stu_Id", "autoWidth": true },
            { "data": "FirstName", "name": "FirstName", "autoWidth": true },
            { "data": "LastName", "name": "LastName", "autoWidth": true },
            { "data": "Age", "name": "Age", "autoWidth": true },
            { "data": "Gender", "name": "Gender", "autoWidth": true }
        ]
    });
});
