// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/notificationHub")
    .build();

connection.on("ReceiveNotification", (message) => {
    const li = document.createElement("li");
    li.textContent = message;
    document.getElementById("notificationsList").appendChild(li);
});

connection.start().catch(err => console.error(err.toString()));