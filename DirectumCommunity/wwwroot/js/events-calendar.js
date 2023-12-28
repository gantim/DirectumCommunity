document.addEventListener('DOMContentLoaded', function() {
    var calendarEl = document.getElementById('calendar');
    var calendar = new FullCalendar.Calendar(calendarEl, {
        themeSystem: 'bootstrap5',
        initialView: 'timeGridWeek',
        allDaySlot: false,
        locale: 'ru',
        buttonIcons: true,
        headerToolbar: {
            start: 'today prev,next',
            center: 'title',
            end: ''
        },
        dayHeaderContent: (args) => {
            return moment(args.date).locale('ru').format('dddd').toUpperCase();
        },
        events: [
            {
                title: 'Событие 1',
                start: '2023-12-28T09:00:00', // Начало события в определенное время
                end: '2023-12-28T10:13:00' // Окончание события в определенное время
            },
            {
                title: 'Событие 2',
                start: '2023-12-29T11:00:00',
                end: '2023-12-29T13:00:00'
            },
            // Добавьте больше событий с временными значениями
        ]
    });
    calendar.render();
});