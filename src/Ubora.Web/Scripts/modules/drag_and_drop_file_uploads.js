export default class DragAndDropFileUploads {
    constructor() {
        Dropzone.autoDiscover = false;

        this.dropzone = new Dropzone('form#my-dropzone', {
            url: `${window.location.href}/AddFile`,
            autoProcessQueue: false,
            paramName: () => 'ProjectFiles',
            uploadMultiple: true,
            addRemoveLinks: true,
            parallelUploads: 5,
            maxFilesize: 30,
            previewsContainer: ".dropzone-previews",
            clickable: '.dropzone-previews'
        });
    }

    init() {
        this.dropzone.on("addedfile", (file) => {
            console.log(file.size);
            const projectFilesValidationElement = document.querySelector('span[data-valmsg-for="ProjectFiles"]');
            if (file.size > 31457280) {
                this.dropzone.removeFile(file);
                projectFilesValidationElement.innerHTML =
                    '<span class="field-validation-error" data-valmsg-for="ProjectFiles" data-valmsg-replace="true">Please upload a smaller files</span>';
            }
        });

        this.dropzone.on('sending', (file, xhr, formData) => {
            const folderName = document.getElementById('FolderName').value;
            if (folderName !== 'Select a folder') {
                formData.append('FolderName', folderName);
            }
            formData.append('Comment', document.getElementById('Comment').value);
        });

        this.dropzone.on('success', (file, response) => {
            // Server side validation
            if (response.length > 0) {
                this.dropzone.removeFile(file);
                const formElement = document.querySelector('#myform');
                formElement.innerHTML = response;
            } else {
                window.location.reload();
            }
        });

        this.onClick();
    }

    onClick() {
        const submitButton = document.querySelector('#btnSubmit');
        // Client side validation
        submitButton.addEventListener('click',
            () => {
                const projectFilesValidationElement = document.querySelector('span[data-valmsg-for="ProjectFiles"]');

                if ($('form#my-dropzone').valid()) {
                    this.dropzone.processQueue();
                } else if (this.dropzone.files.length > 5) {
                    projectFilesValidationElement.innerHTML =
                        '<span class="field-validation-error" data-valmsg-for="ProjectFiles" data-valmsg-replace="true">You can not upload any more files</span>';
                } else if (this.dropzone.files.length < 1) {
                    projectFilesValidationElement.innerHTML = '<span class="field-validation-error" data-valmsg-for="ProjectFiles" data-valmsg-replace="true">Please select a file to upload!</span>';
                }
            });
    }
}

const dropzoneElement = document.querySelector('.dropzone');
if (dropzoneElement) {
    const fileupload = new DragAndDropFileUploads();
    fileupload.init();
}
