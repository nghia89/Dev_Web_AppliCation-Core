var connection = new signalR.HubConnectionBuilder()
    .withUrl("/signalRHub")
    .configureLogging(signalR.LogLevel.Information)
    .build();
connection.start().catch(err => console.error(err.toString()));

connection.on("ReceiveMessage", (message) => {debugger
    var template = $('#announcement-template').html();
    var html = Mustache.render(template, {
        Content: message.content,
        Id: message.id,
        Title: message.title,
        FullName: message.fullName,
        Avatar: message.avatar
    });
    $('#annoncementList').prepend(html);
    $('#badge_number_notifi').show();
    var total = $('#totalAnnouncement').text();
    if (total === null || total === "")
        total = 0;
    var totalAnnounce = parseInt(total) + 1;

    $('#totalAnnouncement').text(totalAnnounce);
});