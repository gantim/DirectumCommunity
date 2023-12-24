$(document).ready(function() {
    $('#fioFilter').select2({
        placeholder: 'ФИО',
        allowClear: true,
        language: 'ru',
        theme: 'bootstrap-5'
    });

    $('#jobTitleFilter').select2({
        placeholder: 'Должность',
        allowClear: true,
        language: 'ru',
        theme: 'bootstrap-5'
    });

    $('#departmentFilter').select2({
        placeholder: 'Отдел',
        allowClear: true,
        language: 'ru',
        theme: 'bootstrap-5'
    });
});