document.addEventListener('DOMContentLoaded', function () {
    const checkboxes = document.querySelectorAll('input[name="userIds"]');
    const createGroupButton = document.getElementById('createGroupButton');

    function updateButtonState() {
        const anyChecked = Array.from(checkboxes).some(checkbox => checkbox.checked);
        createGroupButton.disabled = !anyChecked;
    }

    checkboxes.forEach(checkbox => {
        checkbox.addEventListener('change', updateButtonState);
    });

    updateButtonState();
});
