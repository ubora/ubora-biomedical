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
      previewsContainer: '.dropzone-previews',
      clickable: '.dropzone-previews',
    });
  }

  init() {
    const summaryValidationElement = document.querySelector('#my-dropzone > div.text-danger.validation-summary-valid');

    this.dropzone.on('addedfile', (file) => {
      // Custom file upload validation and avoid error 404 document type when big image
      if (file.size > 31457280) {
        this.dropzone.removeFile(file);
        console.log(file);
        summaryValidationElement.innerHTML =
                    `<span class="field-validation-error" data-valmsg-for="ProjectFiles" data-valmsg-replace="true">Please upload a smaller file. The maximum file size is 30MB. ${
                      file.name
                    } file size was ${
                      file.size
                    } btyes</span>`;
      }
    });

    this.dropzone.on('success', (file, response) => {
      // Server side validation(using ajax)
      if (response.success === false) {
        for (let i = 0; i < response.errors.length; i += 1) {
          this.dropzone.removeFile(file);
          summaryValidationElement.innerHTML =
                        `<div class="text-danger validation-summary-errors" data-valmsg-summary="true"><ul><li>${
                          response.errors[i]
                        }</li></ul></div>`;
        }
      } else {
        window.location.reload();
      }
    });

    const submitButton = document.querySelector('#btnSubmit');

    submitButton.addEventListener(
      'click',
      () => {
        // Client side validation(using jquery validation unobtrusive)
        if ($('form#my-dropzone').valid() && this.dropzone.files.length < 6 && this.dropzone.files.length > 0) {
          this.dropzone.processQueue();
        }

        // Custom file upload validation
        if (this.dropzone.files.length > 5) {
          summaryValidationElement.innerHTML =
                        '<span class="field-validation-error" data-valmsg-for="ProjectFiles" data-valmsg-replace="true">You can not upload any more files. Its 5 maximum number of files.</span>';
        } else if (this.dropzone.files.length < 1) {
          summaryValidationElement.innerHTML =
                        '<span class="field-validation-error" data-valmsg-for="ProjectFiles" data-valmsg-replace="true">Please select a file to upload!</span>';
        } else {
          summaryValidationElement.innerHTML = '<span class="field-validation-error" data-valmsg-for="ProjectFiles" data-valmsg-replace="true"></span>';
        }
      },
    );
  }
}

const dropzoneElement = document.querySelector('.dropzone');
if (dropzoneElement) {
  const fileupload = new DragAndDropFileUploads();
  fileupload.init();
}
