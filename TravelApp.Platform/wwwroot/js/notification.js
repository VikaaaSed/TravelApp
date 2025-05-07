document.addEventListener("DOMContentLoaded", () => {
    document.querySelectorAll('.alert').forEach(alertEl => {
        bootstrap.Alert.getOrCreateInstance(alertEl);
    });
    document.querySelectorAll('.alert .btn-close').forEach(btn => {
        btn.addEventListener('click', () => {
            const alert = btn.closest('.alert');
            if (alert) {
                bootstrap.Alert.getOrCreateInstance(alert).close();
            }
        });
    });
    setTimeout(() => {
        document.querySelectorAll('.alert').forEach(alertEl => {
            bootstrap.Alert.getOrCreateInstance(alertEl).close();
        });
    }, 5000);
});