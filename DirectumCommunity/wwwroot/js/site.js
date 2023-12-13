$(document).ready(function(){
    $('.employee-details').click(function() {
        /*var lastName = $(this).find('div:nth-child(2)').text().trim().split('\n')[0];
        var firstName = $(this).find('div:nth-child(2)').text().trim().split('\n')[1];
        var middleName = $(this).find('div:nth-child(2)').text().trim().split('\n')[2];
        var department = $(this).find('div:nth-child(3)').text().trim().split(',')[0];
        var jobTitle = $(this).find('div:nth-child(3)').text().trim().split(',')[1];

        $('#employeeModalTitle').text(lastName + ' ' + firstName + ' ' + middleName);
        $('#employeeModalBody').html('<p>' + department + ', ' + jobTitle + '</p>');*/

        $('#employeeModal').modal('show');
    });
});