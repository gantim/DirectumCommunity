$(document).ready(function() {
    $('#fioFilter').select2({
        placeholder: 'ФИО',
        allowClear: true,
        language: 'ru'
    });

    $('#jobTitleFilter').select2({
        placeholder: 'Должность',
        allowClear: true,
        language: 'ru'
    });

    $('#departmentFilter').select2({
        placeholder: 'Отдел',
        allowClear: true,
        language: 'ru'
    });
});