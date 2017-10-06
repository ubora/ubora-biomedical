export default class DragNDropFileUploads {
  constructor() {
    Dropzone.autoDiscover = false;

    this.dropzone = new Dropzone('div#my-dropzone', {
      url: `${window.location.href}/AddFile`,
      autoProcessQueue: false,
      paramName: () => 'ProjectFiles',
      uploadMultiple: true,
      maxFilesize: 30,
      addRemoveLinks: true,
    });
  }

  init() {
    this.dropzone.on('sending', (file, xhr, formData) => {
      if ($('#FolderName').val() != null) {
        formData.append('FolderName', $('#FolderName').val());
      }
      formData.append('Comment', $('#Comment').val());
    });

    this.dropzone.on('success', (file, response) => {
      if (response.length > 0) {
        this.removeFile(file);
        $('#myform').html(response);
      } else {
        window.location.reload();
      }
    });

    this.dropzone.on('complete', (file) => {
      console.log(`Hello${file}`);
    });
  }

  onClick() {
    $('#btnSubmit').on('click', () => {
      // Client validation
      if ($('#FolderName').val() == null) {
        $('span[data-valmsg-for="FolderName"]').html('<span class="field-validation-error" data-valmsg-for="FolderName" data-valmsg-replace="true">The FolderName field is required.</span>');
      } else {
        $('span[data-valmsg-for="FolderName"]').html('<span class="field-validation-error" data-valmsg-for="FolderName" data-valmsg-replace="true"></span>');
        this.dropzone.processQueue();
      }
    });
  }
}

const dropzoneElement = document.querySelector('.dropzone');
if (dropzoneElement) {
  const fileupload = new DragNDropFileUploads();
  fileupload.init();
  fileupload.onClick();
}
