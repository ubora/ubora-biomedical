export default class DragAndDropFileUploads {
    constructor() {
        Dropzone.autoDiscover = false;

        this.dropzone = new Dropzone('div#my-dropzone', {
            url: `${window.location.href}/AddFile`,
            autoProcessQueue: false,
            paramName: () => 'ProjectFiles',
            uploadMultiple: true,
            parallelUploads: 5,
            maxFilesize: 30,
            addRemoveLinks: true
        });
    }

    init() {
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
                const validationElement = document.querySelector('span[data-valmsg-for="FolderName"]');
                if (document.getElementById('FolderName').value === 'Select a folder') {
                    validationElement.innerHTML = '<span class="field-validation-error" data-valmsg-for="FolderName" data-valmsg-replace="true">The FolderName field is required.</span>';
                } else {
                    validationElement.innerHTML = '<span class="field-validation-error" data-valmsg-for="FolderName" data-valmsg-replace="true"></span>';
                    this.dropzone.processQueue();
                }
            });
    }
}

const dropzoneElement = document.querySelector('.dropzone');
if (dropzoneElement) {
    const fileupload = new DragAndDropFileUploads();
    fileupload.init();
}
