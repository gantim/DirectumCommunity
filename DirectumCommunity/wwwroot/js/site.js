﻿$(document).ready(function(){
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
        showLoading();
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
            },
            complete: function(data) {
                hideLoading();
            }
        });
    });

    $('.employee-info-current').click(function () {
        showLoading();
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
            },
            complete: function(data) {
                hideLoading();
            }
        });
    });

    $('.notify-img').click(function () {
        showLoading();
        let id = $(this).data('employee-notify-id');
        $.ajax({
            url: '/Notification/GetNotifications',
            type: 'GET',
            data: { id: id },
            success: function (data) {
                let modalInfo = $("#infoNotificationModal");
                modalInfo.empty();
                modalInfo.html(data);
                $('#notificationModal').modal('show');
            },
            complete: function(data) {
                hideLoading();
            }
        });
    });

    $(document).on('click', '#makeRead', function () {
        showLoading();
        let id = $('.notify-img').data('employee-notify-id');
        $.ajax({
            url: '/Notification/MakeAsRead',
            type: 'POST',
            data: { id: id },
            success: function (data) {
                location.reload();
            },
            complete: function(data) {
                hideLoading();
            }
        });
    });
});

function showLoading() {
    $('#loadingOverlay').show();
}

function hideLoading() {
    $('#loadingOverlay').hide();
}