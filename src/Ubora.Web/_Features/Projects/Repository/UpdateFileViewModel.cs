using System;

namespace Ubora.Web._Features.Projects.Repository
{
    public class UpdateFileViewModel
    {
        public Guid FileId { get; set; }
        public string FileName { get; set; }
        public AddFileViewModel AddFileViewModel { get; set; }
    }
}
