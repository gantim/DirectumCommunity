$(document).ready(function(){
    $('.employee-info').click(function () {
        let id = $(this).data('employee-id');
        $.ajax({
            url: '/Employees/GetEmployeeInfo',
            type: 'GET',
            data: { id: id },
            success: function (data) {
                let modalInfo = $("#employeeInfoModal");
                modalInfo.empty();
                modalInfo.html(data);
                $('#employeeModal').modal('show');
            }
        });
    });
});