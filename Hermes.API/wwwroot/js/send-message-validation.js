document.addEventListener("DOMContentLoaded", function () {
    var form = document.getElementById("sendForm");
    var messageInput = document.getElementById("messageInput");
    var sendButton = document.getElementById("send");

    // Hide the send button initially
    sendButton.classList.add('d-none');

    function updateSendButton() {
        var messageContent = messageInput.value.trim();
        if (messageContent.length > 0 && messageContent.length <= 1000) {
            sendButton.classList.remove('d-none');
        } else {
            sendButton.classList.add('d-none');
        }
    }

    // Attached evenlistener, it so the button disappears each time when text is removed
    messageInput.addEventListener('input', updateSendButton);

    form.addEventListener("submit", function (event) {
        if (messageInput.value.length > 1000) {
            event.preventDefault();
            alert("Message is too long. Please limit it to 1000 characters.");
        }
    });
});
