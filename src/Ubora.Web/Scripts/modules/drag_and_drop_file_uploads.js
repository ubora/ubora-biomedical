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
      previewsContainer: '#my-dropzone > div.dropzone-previews.dropzone',
      clickable: '#my-dropzone > div.dropzone-previews.dropzone'
    });
  }

  init() {
    const summaryValidationElement = document.querySelector('#my-dropzone > div.text-danger.validation-summary-valid');

    this.dropzone.on('success', (file, response) => {
      // Server side validation(using ajax)
      if (response.errors !== undefined) {
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

        // Custom file upload validation
        let isValidFileSize = true;

        if (this.dropzone.files.length < 1) {
          summaryValidationElement.innerHTML =
            '<span class="field-validation-error" data-valmsg-for="ProjectFiles" data-valmsg-replace="true">Please select a file to upload!</span>';
        }

        for (var value of this.dropzone.files) {

          if (this.dropzone.files.length > 5) {
            summaryValidationElement.innerHTML =
              '<span class="field-validation-error" data-valmsg-for="ProjectFiles" data-valmsg-replace="true">You can not upload any more files. Its 5 maximum number of files.</span>';
          } else {
            summaryValidationElement.innerHTML = '<span class="field-validation-error" data-valmsg-for="ProjectFiles" data-valmsg-replace="true"></span>';
          }

          if (value.size > 4000000) {
            isValidFileSize = false;
          }
        }

        if (!isValidFileSize) {
          summaryValidationElement.innerHTML =
            `<span class="field-validation-error" data-valmsg-for="ProjectFiles" data-valmsg-replace="true">Please upload a smaller file. The maximum file size is 4MB. </span>`;
        }

        // Client side validation(using jquery validation unobtrusive) and upload.
        if ($('form#my-dropzone').valid() && this.dropzone.files.length < 6 && this.dropzone.files.length > 0 && isValidFileSize === true) {
          this.dropzone.processQueue();
        }
      }
    );
  }
}

const fileupload = new DragAndDropFileUploads();
fileupload.init();
