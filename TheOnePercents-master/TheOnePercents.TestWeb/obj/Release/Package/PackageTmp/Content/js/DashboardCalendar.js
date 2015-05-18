selStart = moment().startOf('day');
selEnd = moment().add(1, 'd').startOf('day');

$(document).ready(function () {

    // page is now ready, initialize the calendar...

    $('#calendar').fullCalendar({
        header: {
            left: 'prev,next today',
            center: 'title',
            right: ''
        },
        titleFormat: "MMMM YYYY",
        editable: true,
        selectable: true,
        unselectAuto: false,

        dayClick: function (date, jsEvent, view) {
            $('#calendar').fullCalendar('gotoDate', date);
        },

        select: function (start, end, jsEvent, view)
        {
            selStart = start;
            selEnd = end;
            var days = selEnd.diff(selStart, 'days');

            $('input[type=checkbox]').each(function () { $(this).prop("checked", false).button("refresh"); });

            $('#calendar').fullCalendar('clientEvents', function (event) {
                
                if (event.start <= end && event.start >= start && event.end <= end && event.end >= start && event.rendering != "background") {
                    $('#cb' + event.title + 'TaskBtn').prop("checked", true).button("refresh");
                }
            });
            
        },

        eventDrop: function (event, delta, revertFunc) {
            var userTaskID = event.id;
            var data = {
                UserTaskID: userTaskID,
                TaskRefID: taskArray[event.title],
                TaskCompleted: true,
                TaskDate: event.start.toDate(),
            };

            $.ajax({
                url: '/api/UserTasks/' + userTaskID,
                type: 'PUT',
                contentType: 'application/json',
                data: JSON.stringify(data),
                success: function (result) {
                    console.log(result);

                    var name = event.title;
                    var taskID = taskArray[name];
                    if (tasks[taskID - 1].snake) {
                        $('#calendar').fullCalendar('removeEvents', 'snake' + userTaskID);
                        var snakeevent = { id: 'snake' + userTaskID, title: name, start: event.start, end: event.end, allDay: true, rendering: 'background', backgroundColor: "red" };
                        $('#calendar').fullCalendar('renderEvent', snakeevent, true);
                    }
                },
                error: function() {
                    alert("error!");
                },
            });
        },
        //eventRender: function(event, element) {
        //    //element.find('span.fc-event-title').html(element.find('span.fc-event-title').text()); 
        //},
    });

    $('.fc-prev-button').click(getUserTasksPoints);

    $('.fc-next-button').click(getUserTasksPoints);

    $('.fc-today-button').click(getUserTasksPoints);

    getTasks();

});

function addInputTo(container, id, val, func)
{
    var inputToAdd = $("<input/>", { type: "button", id: "field", value: val });
    inputToAdd.bind('click', func);

    container.append(inputToAdd);
}

function addCheckboxTo(container, id, val, func)
{
    $('<input />', { type: 'checkbox', id: 'cb' + id, value: val }).appendTo(container);
    $('<label />', { 'for': 'cb' + id, text: val }).appendTo(container);

    $('#cb' + id).bind('change', func);
}

function checkTask()
{
    var name = $(this).attr("value");
    if($(this).is(":checked"))
    {
        addTask(name);
    } else
    {
        console.log("unchecked");
        removeTask(name);
    }
}

function findCalendarEvent(date, name) {
    return $('#calendar').fullCalendar('clientEvents', function (event) {
        if (event.start <= date && event.start >= date && event.rendering != "background" && event.title == name) {
            return event;
        }
    })[0];
}

function addTask(name)
{
    var days = selEnd.diff(selStart, 'days');
    for (i = 0; i < days; i++)
    {
        var id = taskArray[name];
        var selectedDate = moment(selStart).add(i, 'd');

        putUserTask(id, name, selectedDate.toDate(), true);
    }
}

function removeTask(name)
{
    var days = selEnd.diff(selStart, 'days');
    for(i =0; i < days; i++)
    {
        var selectedDate = moment(selStart).add(i, 'd');
        var event = findCalendarEvent(selectedDate, name);
        if (event !== undefined) {
            deleteUserTask(event.id);
        }
    }
}

var taskArray = new Array();
var tasks = {};

function getTasks()
{
    $.ajax({ // ajax call starts
        url: '/api/Task/GetActiveTask', // JQuery loads serverside.php
        dataType: 'json', // Choosing a JSON datatype
        success: function (data) {
            tasks = data;
            $.each(data, function (i, val) {
                addCheckboxTo($('#tasks'), val.name + "TaskBtn", val.name, checkTask);
                taskArray[val.name] = val.taskID;
            });
            $("#dialog-form").buttonset();
            getUserTasks();
        },
        error: function () {
            alert("error!");
        }
    });
}

function addEvent(name, date, userTaskID)
{
    var keys = Object.keys(taskArray);
    var event = { id: userTaskID, title: name, start: date, end: moment(date).add(1, 'd'), allDay: true };
    $('#calendar').fullCalendar('renderEvent', event, true);
    if (tasks[taskArray[name]-1].snake) {
        var snakeevent = { id: 'snake' + userTaskID, title: name, start: date, end: moment(date).add(1, 'd'), allDay: true, rendering: 'background', backgroundColor: "red" };
        $('#calendar').fullCalendar('renderEvent', snakeevent, true);
    }
}

function removeEvent(id)
{
    var name = $('#calendar').fullCalendar('clientEvents', id)[0].title;
    var taskID = taskArray[name];
    if (tasks[taskID - 1].snake)
    {
        $('#calendar').fullCalendar('removeEvents', 'snake' + id);
    }
    $('#calendar').fullCalendar('removeEvents', id);
}

function getUserTasks()
{
    $.ajax({
        url: '/api/UserTasks/GetUserTasks',
        dataType: 'json',
        success: function(data) {
            $.each(data, function (i, val) {
                var keys = Object.keys(taskArray);
                var name = keys[val.taskRefID-1];
                addEvent(name, val.taskDate, val.userTaskID);
            });
            $('#calendar').fullCalendar('select', selStart, selEnd);
            getUserTasksPoints();
        },
        error: function () {
            alert("error!");
        }
    });
}

function getUserTasksPoints()
{
    var date = $("#calendar").fullCalendar('getDate');

    var data = {
        from: moment(date).startOf('month').toDate(),
        to: moment(date).endOf('month').toDate(),
    };

    var points = $.ajax({
        url: '/api/UserTasks/PostUserTasksPointsFromTo',
        contentType: 'application/json; charset=utf-8',
        type: 'POST',
        data: JSON.stringify(data),
        success: function (result) {
            console.log(result);
            $("#TaskPoints").text(result + " points");
        },
        error: function () {
            alert("error!");
        }
    });
}

function putUserTask(taskID, name, date, completed)
{
    var data = {
        TaskRefID: taskID,
        TaskCompleted: completed,
        TaskDate: date
    };

    $.ajax({
        url: '/api/UserTasks/PostUserTasks',
        contentType: 'application/json; charset=utf-8',
        type: 'POST',
        data: JSON.stringify(data),
        success: function (results) {
            console.log("put task " + taskID);
            addEvent(name, date, results.userTaskID);
        },
        error: function () {
            alert("error!");
        }
    });
}

function deleteUserTask(id)
{
    $.ajax({
        url: '/api/UserTasks/' + id,
        type: 'DELETE',
        success: function (results) {
            console.log("Deleted usertask " + id);
            //Remove from calendar
            removeEvent(id);
        },
        error: function () {
            alert("error!");
        }
    });
}