var connection = new signalR.HubConnectionBuilder().withUrl("https://localhost:7003/SignalRHub").build();

var sendButton = document.getElementById("usersendbutton");
var usernameInput = document.getElementById("userusername");
var messageInput = document.getElementById("usermessage");
var messageList = document.getElementById("messagelist");

sendButton.disabled = true;
usernameInput.value = localStorage.getItem("chatUserName") || "";

connection.on("ReceiveMessage", function (user, message) {

    var now = new Date();
    var time = now.getHours().toString().padStart(2, "0") + ":" + now.getMinutes().toString().padStart(2, "0");

    var li = document.createElement("li");
    var span = document.createElement("span");
    span.style.fontWeight = "bold";
    span.textContent = user;
    li.appendChild(span);
    li.appendChild(document.createTextNode(": " + message + " - " + time))
    messageList.appendChild(li);
    messageList.scrollTop = messageList.scrollHeight;
});

connection.start().then(function () {
    sendButton.disabled = false;
}).catch(function (err) { return console.error(err.toString()); });

sendButton.addEventListener("click", function (event) {
    event.preventDefault();
    sendMessage();
});
messageInput.addEventListener("keydown", function (event) {
    if (event.key == "Enter") {
        event.preventDefault();
        sendMessage();
    }
});

function sendMessage() {
    var user = usernameInput.value.trim();
    var message = messageInput.value.trim();
    if (!user || !message) return;

    localStorage.setItem("chatUserName", user);
    connection.invoke("SendMessage", user, message).catch(function (err) { return console.error(err.toString()); });

    messageInput.value = "";
    messageInput.focus();
}
