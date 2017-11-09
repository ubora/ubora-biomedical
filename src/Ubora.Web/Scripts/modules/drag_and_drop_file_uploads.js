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
        document.querySelector('.dz-progress').remove();
        document.querySelector('.dz-error-mark').remove();
        document.querySelector('.dz-success-mark').remove();
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
      return `<span class="field-validation-error" data-valmsg-for="ProjectFiles" data-valmsg-replace="true">${text} </span>`;
    }

    if (this.dropzone.files.length < 1) {
      element.innerHTML = setError('Please select a file to upload!');
    }

    let isValidFileSize = true;

    for (var file of this.dropzone.files) {

      if (this.dropzone.files.length > 5) {
        element.innerHTML = setError('You can not upload any more files. Its 5 maximum number of files.');
      } else {
        element.innerHTML = setError('');
      }

      if (file.size > 4000000) {
        isValidFileSize = false;
      }
    }

    if (!isValidFileSize) {
      element.innerHTML = setError('Please upload a smaller file. The maximum file size is 4MB.');
    }

    const unobtrusiveValidation = $('form#my-dropzone').valid();
    if (unobtrusiveValidation &&
      this.dropzone.files.length < 6 &&
      this.dropzone.files.length > 0 &&
      isValidFileSize === true) {
      return true;
    } else {
      return false;
    }
  }
}

const fileupload = new DragAndDropFileUploads();
fileupload.init();