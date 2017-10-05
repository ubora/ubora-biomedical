export class FileUploads {
    constructor() {
        Dropzone.autoDiscover = false;
        const myDropzone = new Dropzone("div#my-dropzone", {
            url: '/Projects/fc4c02c1-a7f0-4963-ae0a-458544ecc06e/Repository/AddFile',
            autoProcessQueue: false,
            paramName: parameterName,
            uploadMultiple: true
        });

        myDropzone.on('sending', (file, xhr, formData) => {
            if ($("#FolderName").val() != null) {
                formData.append('FolderName', $("#FolderName").val());
            }
            formData.append('Comment', $("#Comment").val());
        });

        $('#btnSubmit').on('click', (file) => {
            if ($("#FolderName").val() == null) {
                $('span[data-valmsg-for="FolderName"]').html('<span class="field-validation-error" data-valmsg-for="FolderName" data-valmsg-replace="true">The FolderName field is required.</span>');
            } else {
                $('span[data-valmsg-for="FolderName"]').html('<span class="field-validation-error" data-valmsg-for="FolderName" data-valmsg-replace="true"></span>');
                myDropzone.processQueue();
            }
        });

        myDropzone.on('success', (file, response) => {
            if (response.length > 0) {
                this.removeFile(file);
                $('#myform').html(response);
            } else {
                window.location.reload();
            }
        });

        //Ignore to appending [n] to name
        const parameterName = () => {
            return "ProjectFiles";
        }
    }
}

const dropzoneElement = document.querySelector('.dropzone');
if (dropzoneElement) {
    new FileUploads();
}