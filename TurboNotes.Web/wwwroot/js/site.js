const connection = new signalR.HubConnectionBuilder()
    .withUrl("/notificationHub")
    .withAutomaticReconnect()
    .build();
document.addEventListener("DOMContentLoaded", () => {
    const savedNotifications = JSON.parse(localStorage.getItem("notifications")) || [];
    savedNotifications.forEach(notification => addNotification(notification));
});

function addNotification(notification) {
    const notificationsList = document.getElementById("notificationsList");
    
    const existingNotification = Array.from(notificationsList.children).find(li =>
        li.dataset.noteId === notification.id.toString());

    if (existingNotification) {
        existingNotification.querySelector(".notification-text").textContent = notification.message;
        existingNotification.dataset.notificationType = notification.type;
    } else {
        const li = document.createElement("li");
        li.dataset.noteId = notification.id;
        li.dataset.notificationType = notification.type;

        const textSpan = document.createElement("span");
        textSpan.textContent = notification.message;
        textSpan.classList.add("notification-text");
        li.appendChild(textSpan);
        
        const closeButton = document.createElement("button");
        closeButton.textContent = "✖";
        closeButton.classList.add("ms-2");
        closeButton.onclick = function () {
            li.remove();
            updateNotificationCount();
            
            let savedNotifications = JSON.parse(localStorage.getItem("notifications")) || [];
            savedNotifications = savedNotifications.filter(n => n.id !== notification.id);
            localStorage.setItem("notifications", JSON.stringify(savedNotifications));
        };

        li.appendChild(closeButton);
        notificationsList.appendChild(li);
    }

    updateNotificationCount();
}

connection.on("ReceiveNotification", (notification) => {
    let savedNotifications = JSON.parse(localStorage.getItem("notifications")) || [];
    
    savedNotifications = savedNotifications.filter(n => n.id !== notification.id);
    
    savedNotifications.push({
        id: notification.id,
        message: notification.message,
        type: notification.type,
        deadline: notification.deadline
    });

    localStorage.setItem("notifications", JSON.stringify(savedNotifications));
    addNotification(notification);
});

function updateNotificationCount() {
    const notificationsList = document.getElementById("notificationsList");
    const notificationCount = document.getElementById("notificationCount");
    const count = notificationsList.children.length;
    notificationCount.textContent = count;
    notificationCount.style.display = count > 0 ? "inline" : "none";
}

connection.start().catch(err => console.error("Connection error: " + err));