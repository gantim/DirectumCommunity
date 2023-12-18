$(document).ready(function(){
    const hubConnection = new signalR.HubConnectionBuilder()
        .withUrl("/birthday")
        .build();
    
    hubConnection.on("Birthday", function(isBirthdayToday)
    {
        let id = $('#employeeId').val();
        $.ajax({
            url: '/Employees/GetBirthdayNotification',
            type: 'GET',
            data: { id: id },
            success: function (data) {
                let modalInfo = $("#birthdayNotificationModal");
                modalInfo.empty();
                modalInfo.html(data);
                $('#birthdayModal').modal('show');
            }
        });
    });
    
    hubConnection.start()
        .then(function () {
            let id = $('#personId').val();
            hubConnection.invoke("BirthdayNotification", id)
                .catch(function (err) {
                    return console.error(err.toString());
                });
        })
        .catch(function (err) {
            return console.error(err.toString());
        });
    
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

    $('#monthButtonSub').click(function() {
        $('#monthButtonSub').addClass('active');
        $('#yearButtonSub').removeClass('active');
    });

    $('#yearButtonSub').click(function() {
        $('#yearButtonSub').addClass('active');
        $('#monthButtonSub').removeClass('active');
    });
});