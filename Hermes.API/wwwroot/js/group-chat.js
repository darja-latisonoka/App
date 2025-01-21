const modal = document.getElementById("createGroupModal");

function openModal() {
    modal.className = 'create-group-modal create-group-modal--opened';
}

function closeModal() {
    modal.className = "create-group-modal";
}

const groupItems = document.querySelectorAll('.create-group-modal__item');

groupItems.forEach(item => {
    const checkbox = item.querySelector('.create-group-modal__checkbox');

    checkbox.addEventListener('change', function () {
        if (this.checked) {
            item.classList.add('create-group-modal__item--selected');
        } else {
            item.classList.remove('create-group-modal__item--selected');
        }
    });
});

document.addEventListener('DOMContentLoaded', function () {
    const chatWindow = document.getElementById('messages') || undefined;
    const messageInput = document.getElementById('messageInput') || undefined;
    const sendButton = document.getElementById('send') || undefined;
    const sendForm = document.getElementById('sendForm') || undefined;
    const currentGroupId = document.getElementById('groupId')?.dataset.userId;
    const currentUsername = document.getElementById('currentUsername')?.dataset.username;

    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/chatHub")
        .build();

    connection.on("ReceiveGroupMessage", function (groupId, username, message) {
        if (groupId !== currentGroupId) {
            return;
        }

        const messageElement = document.createElement('li');
        messageElement.className = 'chat__message chat__message--received';

        const messageUsername = document.createElement('p');
        messageUsername.className = 'chat__message-username';
        messageUsername.textContent = username;

        const messageContent = document.createElement('p');
        messageContent.className = 'chat__message-content';
        messageContent.textContent = message;

        messageElement.appendChild(messageUsername);
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
            connection.invoke("SendGroupMessage", currentGroupId, message).catch(function (err) {
                return console.error(err.toString());
            });

            messageInput.value = '';

            const messageElement = document.createElement('li');
            messageElement.className = 'chat__message chat__message--sent';

            const messageUsername = document.createElement('p');
            messageUsername.className = 'chat__message-username';
            messageUsername.textContent = currentUsername;

            const messageContent = document.createElement('p');
            messageContent.className = 'chat__message-content';
            messageContent.textContent = message;

            messageElement.appendChild(messageUsername);
            messageElement.appendChild(messageContent);

            chatWindow.appendChild(messageElement);
        }
    });
});
