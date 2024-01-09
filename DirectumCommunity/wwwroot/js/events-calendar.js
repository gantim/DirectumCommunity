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
        eventDidMount: function(info) {
            var tooltip = new bootstrap.Popover(info.el, {
                title: info.event.title,
                content: info.event.extendedProps.description,
                trigger: 'hover',
                placement: 'auto',
                container: 'body',
                html: true,
                delay: { "show": 100, "hide": 50000 }
            });

            $(document).on('mouseenter', '.popover-link', function(event) {
                $('.popover-link').popover('hide');
                var popover = new bootstrap.Popover(event.target, {
                    content: info.event.extendedProps.members,
                    trigger: 'hover',
                    placement: 'auto',
                    container: 'body',
                    html: true,
                    delay: { "show": 100, "hide": 1500 }
                });
                popover.show();
            });

            $(document).on('click', '#calendar', function(event) {
                var popover = $(event.target).closest('.popover');
                if (!popover.length) {
                    $('.popover').popover('hide');
                }
            });
        },
        events: window.location.protocol + '//' + window.location.host + '/EventsCalendar/GetMeetings'
    });
    calendar.render();
});