(function () {
    window.setupImagePreview = function (inputId, previewId) {
        const input = document.getElementById(inputId);
        const preview = document.getElementById(previewId);
        const spinner = preview.closest('.image-preview-container')?.querySelector('.loading-spinner');

        if (!input || !preview || !spinner) return;

        const updatePreview = (url) => {
            url = (url || '').trim();

            if (url) {
                preview.style.display = 'none';
                spinner.style.display = 'block';

                const tempImg = new Image();
                tempImg.onload = () => {
                    preview.src = url;
                    preview.style.display = 'block';
                    spinner.style.display = 'none';
                };
                tempImg.onerror = () => {
                    preview.style.display = 'none';
                    spinner.style.display = 'none';
                    const container = preview.closest('.image-preview-container');
                    container?.classList.add('bg-danger', 'bg-opacity-10');
                    setTimeout(() => {
                        container?.classList.remove('bg-danger', 'bg-opacity-10');
                    }, 2000);
                };
                tempImg.src = url;
            } else {
                preview.style.display = 'none';
                spinner.style.display = 'none';
            }
        };

        updatePreview(input.value);
        input.addEventListener('input', () => updatePreview(input.value));
    };
})();