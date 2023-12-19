let currentDate = new Date();
let dateType = 1;
const months = [
    "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь",
    "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"
];
const daysOfWeek = ['Вс', 'Пн', 'Вт', 'Ср', 'Чт', 'Пт', 'Сб'];
const calendar = document.getElementById('calendar');

function createCalendar(year, month) {
    const getDays = (year, month) => {
        return new Date(year, month, 0).getDate();
    };
    
    calendar.innerHTML = '';
    
    if (dateType === 1) {

        const daysInMonth = getDays(year, month);
        
        for (let i = 1; i <= daysInMonth; i++) {

            const date = new Date(year, month, i);
            const dayOfWeek = daysOfWeek[date.getDay()];
            const dayContainer = document.createElement('div');
            dayContainer.classList.add('calendar-day');

            if (new Date().getDate() === i) {
                dayContainer.style.backgroundColor = '#FF860040';
            }

            const day = document.createElement('p');
            const week = document.createElement('p');
            day.textContent = i;
            week.textContent = dayOfWeek;

            dayContainer.appendChild(day);
            dayContainer.appendChild(week);

            calendar.appendChild(dayContainer);
        }

        getAllSubstitutionsInMonth(year, month, daysInMonth);
    }
    else {
        for (let i = 1; i <= 12; i++) {
            const date = new Date(year, i, 0);
            const monthName = months[date.getMonth()];
            const monthContainer = document.createElement('div');
            monthContainer.classList.add('calendar-month');

            if (new Date().getDate() === i) {
                monthContainer.style.backgroundColor = '#FF860040';
            }
            monthContainer.textContent = monthName;

            calendar.appendChild(monthContainer);
        }
    }
}

function createEmployeeRows(jsonData, days) {
    const tableBody = $('#substitutionsTable tbody');
    tableBody.empty();
    jsonData.forEach(employee => {
        const newRow = $('<tr>');
        if (employee.avatar) {
            newRow.append(`<td><div class="row align-items-center">
        <div class="col-md-auto">
            <img class="substitutions-avatar" src="data:image/jpeg;base64,${employee.avatar}" alt="${employee.name}"/>
        </div>
        <div class="col">
            <div class="row">${employee.name}</div>
            <div class="row">${employee.department}</div>
        </div>
    </div></td>`);
        }
        else{
            newRow.append(`<td><div class="row align-items-center">
        <div class="col-md-auto">
             <img class="substitutions-avatar" src="${defaultAvatar}" alt="${employee.name}"/>
        </div>
        <div class="col">
            <div class="row">${employee.name}</div>
            <div class="row">${employee.department}</div>
        </div>
    </div></td>`);
        }
        
        for (let i = 1; i <= days; i++) {
            newRow.append(`<td id="cell-${employee.id}-${i}"></td>`);
        }
        tableBody.append(newRow);
    });
}

function highlightCells(jsonData, days) {
    jsonData.forEach(employee => {
        employee.substitutions.forEach(substitution => {
            const startDate = new Date(substitution.startDate);
            const endDate = new Date(substitution.endDate);

            const startDay = startDate.getDate();
            const endDay = endDate.getDate();

            for (let day = startDay; day <= endDay && day <= days; day++) {
                const cellId = `#cell-${employee.id}-${day}`;
                
                if(substitution.typeReason === 1) {
                    $(cellId).css('border', 'none');
                    $(cellId).css('background-color', '#CC3A3A');
                    $(cellId).attr({
                        'data-bs-toggle': 'tooltip',
                        'data-bs-html': 'true',
                        'title': `<div><p>Замещает: ${employee.name}, ${employee.department}</p><p>Причина: ${substitution.reason}</p></div>`
                    });
                }
                
                if(substitution.typeReason === 2){
                    $(cellId).css('border', 'none');
                    $(cellId).css('background-color', '#23CC71');
                    $(cellId).attr({
                        'data-bs-toggle': 'tooltip',
                        'data-bs-html': 'true',
                        'title': `<div><p>Замещает: ${employee.name}, ${employee.department}</p><p>Причина: ${substitution.reason}</p></div>`
                    });
                }
            }
        });
    });

    $('[data-bs-toggle="tooltip"]').tooltip();
}

function updateDisplay() {
    let formattedDate;
    let year = currentDate.getFullYear();
    
    if (dateType === 2) {
        formattedDate = year.toString();
    } else {
        const monthName = months[currentDate.getMonth()];
        formattedDate = `${monthName} ${year}`;
    }
    $('#currentDateBox').text(formattedDate);
    
    let month = currentDate.getMonth() + 1;
    
    createCalendar(year, month);
}

function previous() {
    if (dateType === 2) {
        currentDate.setFullYear(currentDate.getFullYear() - 1);
    } else {
        currentDate.setMonth(currentDate.getMonth() - 1);
    }
    updateDisplay();
}

function next() {
    if (dateType === 2) {
        currentDate.setFullYear(currentDate.getFullYear() + 1);
    } else {
        currentDate.setMonth(currentDate.getMonth() + 1);
    }
    updateDisplay();
}

function getAllSubstitutionsInMonth(year, month, days) {
    $.ajax({
        url: '/Substitutions/GetAllSubstitutionsInMonth',
        method: 'GET',
        data: { year: year, month: month },
        dataType: 'json',
        success: function (data) {
            createEmployeeRows(data, days);
            highlightCells(data, days);
        },
        error: function (error) {
            console.error('Произошла ошибка:', error);
        }
    });
}

$(document).ready(function(){
    updateDisplay();
    
    $('#leftButton').click(function() {
        previous();
    });

    $('#rightButton').click(function() {
        next();
    });

    $('#monthButtonSub').click(function() {
        $('#monthButtonSub').addClass('active');
        $('#yearButtonSub').removeClass('active');
        dateType = 1;
        updateDisplay();
    });

    $('#yearButtonSub').click(function() {
        $('#yearButtonSub').addClass('active');
        $('#monthButtonSub').removeClass('active');
        dateType = 2;
        updateDisplay();
    });
});