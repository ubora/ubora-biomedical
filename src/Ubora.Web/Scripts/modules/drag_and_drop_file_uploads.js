export default class DragAndDropFileUploads {
    constructor() {
        Dropzone.autoDiscover = false;

        this.dropzone = new Dropzone('form#my-dropzone', {
            autoProcessQueue: false,
            paramName: () => 'ProjectFiles',
            uploadMultiple: true,
            addRemoveLinks: true,
            parallelUploads: 5,
            previewsContainer: '#my-dropzone > div.dropzone-previews.dropzone',
            clickable: '#my-dropzone > div.dropzone-previews.dropzone'
        });
    }

    init() {

        const summaryValidationElement = document.querySelector('#my-dropzone > div.text-danger.validation-summary-valid');

        const removeFileAndSetError = (file) => {
            this.dropzone.removeFile(file);
            summaryValidationElement.innerHTML =
                `<div class="text-danger validation-summary-errors" data-valmsg-summary="true"><ul><li>${
                response.errors[i]
                }</li></ul></div>`;
        }

        this.dropzone.on('success', (file, response) => {
            // Server side validation(using ajax)
            if (response.errors !== undefined) {
                for (let i = 0; i < response.errors.length; i += 1) {
                    removeFileAndSetError(file);
                }
            } else {
                window.location.reload();
            }
        });

        this.dropzone.on('addedfile',
            () => {
                $('.dz-progress').remove();
                $('.dz-error-mark').remove();
                $('.dz-success-mark').remove();
            });

        const submitButton = document.querySelector('#btnSubmit');
        submitButton.addEventListener(
            'click',
            () => {
                if (this.validate(summaryValidationElement)) {
                    this.dropzone.processQueue();
                }
            }

        );
    }

    validate(element) {

        const setError = (text) => {
            if (text === undefined) {
                return `<span class="field-validation-error" data-valmsg-for="ProjectFiles" data-valmsg-replace="true"></span>`;
            }
            return `<span class="field-validation-error" data-valmsg-for="ProjectFiles" data-valmsg-replace="true">${text} </span>`;
        }

        if (this.dropzone.files.length < 1) {
            element.innerHTML = setError('Please select a file to upload!');
            return false;
        } else if (this.dropzone.files.length > 5) {
            element.innerHTML = setError('You can not upload any more files. Its 5 maximum number of files.');
            return false;
        } else {
            element.innerHTML = setError();
        }

        let isValidFileSize = true;
        for (var file of this.dropzone.files) {
            if (file.size > 4000000) {
                isValidFileSize = false;
            }
        }

        if (!isValidFileSize) {
            element.innerHTML = setError('Please upload a smaller file. The maximum file size is 4MB.');
            return false;
        }

        if (!$('form#my-dropzone').valid()) {
            return false;
        }

        return true;
    }
}

const fileupload = new DragAndDropFileUploads();
fileupload.init();