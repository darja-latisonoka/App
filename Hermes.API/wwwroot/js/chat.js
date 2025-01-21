document.addEventListener('DOMContentLoaded', function () {
    const chatWindow = document.getElementById('messages') || undefined;
    const messageInput = document.getElementById('messageInput') || undefined;
    const sendButton = document.getElementById('send') || undefined;
    const sendForm = document.getElementById('sendForm') || undefined;
    const interlocutorId = document.getElementById('userId')?.dataset.userId;

    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/chatHub")
        .build();

    connection.on("ReceiveMessage", function (senderId, message) {
        if (interlocutorId !== senderId) {
            return;
        }

        const messageElement = document.createElement('li');
        messageElement.className = 'chat__message chat__message--received';

        const messageContent = document.createElement('p');
        messageContent.className = 'chat__message-content';
        messageContent.textContent = message;

        messageElement.appendChild(messageContent);

        chatWindow.appendChild(messageElement);
    });

    connection
        .start()
        .then(function () {
            if (sendButton) {
                sendButton.disabled = false;
            }
        })
        .catch(function (err) {
            return console.error(err.toString());
        });

    sendForm?.addEventListener('submit', function (event) {
        event.preventDefault();

        const message = messageInput.value;

        if (message) {
            connection.invoke("SendMessage", interlocutorId, message).catch(function (err) {
                return console.error(err.toString());
            });

            messageInput.value = '';

            const messageElement = document.createElement('li');
            messageElement.className = 'chat__message chat__message--sent';

            const messageContent = document.createElement('p');
            messageContent.className = 'chat__message-content';
            messageContent.textContent = message;

            messageElement.appendChild(messageContent);

            chatWindow.appendChild(messageElement);
        }
    });

    connection.on("ChangedOnlineStatus", function (userId, isOnline) {
        const userStatus = document.getElementById(userId + '-status');
        userStatus.className = isOnline ? 'user-preview__status user-preview__status--online' : 'user-preview__status user-preview__status--offline';
    });
});
