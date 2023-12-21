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
            
            const date = new Date(year, month - 1, i);
            const dayOfWeek = daysOfWeek[date.getDay()];
            const dayContainer = document.createElement('div');
            dayContainer.classList.add('calendar-day');

            let myCurrentDate = new Date();
            
            if (myCurrentDate.getDate() === i && myCurrentDate.getMonth() + 1 === month) {
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
            
            let myCurrentDate = new Date();
            if (myCurrentDate.getMonth() + 1 === i && year === myCurrentDate.getFullYear()) {
                monthContainer.style.backgroundColor = '#FF860040';
            }
            monthContainer.textContent = monthName;

            calendar.appendChild(monthContainer);
        }

        getAllSubstitutionsInYear(year, null);
    }
}

function createEmployeeRows(jsonData, days, type) {
    const tableBody = $('#substitutionsTable tbody');
    tableBody.empty();
    jsonData.forEach(employee => {
        const newRow = $('<tr class="align-items-center">');
        if (employee.avatar) {
            newRow.append(`<td style="width: 250px; height: 70px;"><div class="row align-items-center">
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
            newRow.append(`<td style="width: 250px; height: 70px;"><div class="row align-items-center">
        <div class="col-md-auto">
             <img class="substitutions-avatar" src="${defaultAvatar}" alt="${employee.name}"/>
        </div>
        <div class="col">
            <div class="row">${employee.name}</div>
            <div class="row">${employee.department}</div>
        </div>
    </div></td>`);
        }
        
        if(type === 1) {
            for (let i = 1; i <= days; i++) {
                newRow.append(`<td id="cell-${employee.id}-${i}-${currentDate.getMonth() + 1}"></td>`);
            }
        }
        else{
            for (let i = 1; i <= 12; i++) {
                newRow.append(`<td style="height: 70px; padding: 0;" id="cell-${employee.id}-${i}"></td>`);
                let year = currentDate.getFullYear();
                let daysInMonth = new Date(year, i, 0).getDate();

                let cell = newRow.find(`#cell-${employee.id}-${i}`);

                let tableWrapper = $('<div class="table-wrapper"></div>');
                let table = $('<table class="table-in-cell"></table>');
                let tr = $('<tr></tr>');
                for (let j = 1; j <= daysInMonth; j++) {
                    tr.append(`<td class="col" id="day-${employee.id}-${i}-${j}"></td>`);
                }
                table.append(tr);
                tableWrapper.append(table);
                cell.append(tableWrapper);
            }
        }
        tableBody.append(newRow);
    });
}

function highlightCells(jsonData, days, type) {
    if(type === 1) {
        jsonData.forEach(employee => {
            employee.substitutions.forEach(substitution => {
                const startDate = new Date(substitution.startDate);
                const endDate = new Date(substitution.endDate);

                let currentDay = new Date(startDate);
                
                while (currentDay <= endDate && endDate.getMonth() === currentDate.getMonth()) {
                    const cellId = `#cell-${employee.id}-${currentDay.getDate()}-${currentDate.getMonth() + 1}`;

                    if (substitution.typeReason === 1) {
                        $(cellId).css('border', 'none');
                        $(cellId).css('background-color', '#CC3A3A');
                        $(cellId).attr({
                            'data-bs-toggle': 'tooltip',
                            'data-bs-html': 'true',
                            'title': `<div><p>Замещает: ${substitution.substituteName}, ${employee.department}</p><p>Причина: ${substitution.reason}</p></div>`
                        });
                    }

                    if (substitution.typeReason === 2) {
                        $(cellId).css('border', 'none');
                        $(cellId).css('background-color', '#23CC71');
                        $(cellId).attr({
                            'data-bs-toggle': 'tooltip',
                            'data-bs-html': 'true',
                            'title': `<div><p>Замещает: ${employee.name}, ${employee.department}</p><p>Причина: ${substitution.reason}</p></div>`
                        });
                    }

                    currentDay.setDate(currentDay.getDate() + 1);
                }
            });
        });
    }
    else {
        jsonData.forEach(employee => {
            employee.substitutionsMonth.forEach(substitutionMonth => {
                substitutionMonth.substitutions.forEach(substitution => {
                    const startDate = new Date(substitution.startDate);
                    const endDate = new Date(substitution.endDate);

                    let currentDay = startDate;

                    while (currentDay <= endDate) {
                        const cellId = `#day-${employee.id}-${currentDay.getMonth() + 1}-${currentDay.getDate()}`;

                        if (substitution.typeReason === 1) {
                            $(cellId).css('border', 'none');
                            $(cellId).css('background-color', '#CC3A3A');
                            $(cellId).attr({
                                'data-bs-toggle': 'tooltip',
                                'data-bs-html': 'true',
                                'title': `<div><p>Замещает: ${substitution.substituteName}, ${employee.department}</p><p>Причина: ${substitution.reason}</p></div>`
                            });
                        }

                        if (substitution.typeReason === 2) {
                            $(cellId).css('border', 'none');
                            $(cellId).css('background-color', '#23CC71');
                            $(cellId).attr({
                                'data-bs-toggle': 'tooltip',
                                'data-bs-html': 'true',
                                'title': `<div><p>Замещает: ${employee.name}, ${employee.department}</p><p>Причина: ${substitution.reason}</p></div>`
                            });
                        }

                        currentDay.setDate(currentDay.getDate() + 1);
                    }
                });
            });
        });
    }
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
    const isVacation = $('#isVacation').is(':checked') ? 1 : 0;
    const isMedicalLeave = $('#isMedicalLeave').is(':checked') ? 1 : 0;

    let requestData = {
        year: year,
        month: month,
        filter: {
            IsVacation: isVacation,
            IsMedicalLeave: isMedicalLeave
        }
    };
    
    let request = JSON.stringify(requestData);
    
    showLoading();
    
    $.ajax({
        url: '/Substitutions/GetAllSubstitutionsInMonth',
        method: 'POST',
        data: request,
        contentType: 'application/json',
        dataType: 'json',
        success: function (data) {
            createEmployeeRows(data, days, 1);
            highlightCells(data, days, 1);
        },
        error: function (error) {
            console.error('Произошла ошибка:', error);
        },
        complete: function(data) {
            hideLoading();
        }
    });
}

function getAllSubstitutionsInYear(year, days) {
    const isVacation = $('#isVacation').is(':checked') ? 1 : 0;
    const isMedicalLeave = $('#isMedicalLeave').is(':checked') ? 1 : 0;

    let requestData = {
        year: year,
        filter: {
            IsVacation: isVacation,
            IsMedicalLeave: isMedicalLeave
        }
    };

    let request = JSON.stringify(requestData);

    showLoading();

    $.ajax({
        url: '/Substitutions/GetAllSubstitutionsInYear',
        method: 'POST',
        data: request,
        contentType: 'application/json',
        dataType: 'json',
        success: function (data) {
            console.log(data);
            createEmployeeRows(data, days, 2);
            highlightCells(data, days, 2);
        },
        error: function (error) {
            console.error('Произошла ошибка:', error);
        },
        complete: function(data) {
            hideLoading();
        }
    });
}

$(document).ready(function(){
    $('#isMedicalLeave').prop('checked', true);
    $('#isVacation').prop('checked', true);
    
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

    $('#isVacation, #isMedicalLeave').change(function() {
        updateDisplay();
    });
});