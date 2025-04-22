document.addEventListener("DOMContentLoaded", function () {
    const editorIds = window.CKEditorFields || [];

    const editorConfig = {
        toolbar: [
            'heading', '|',
            'bold', 'italic', 'underline', 'strikethrough', 'removeFormat', '|',
            'link', 'bulletedList', 'numberedList', '|',
            'undo', 'redo', 'alignment'
        ]
    };

    editorIds.forEach(id => {
        const el = document.querySelector(`#${id}`);
        if (el) {
            ClassicEditor
                .create(el, editorConfig)
                .catch(error => {
                    console.error(`CKEditor init failed for #${id}:`, error);
                });
        }
    });
});
