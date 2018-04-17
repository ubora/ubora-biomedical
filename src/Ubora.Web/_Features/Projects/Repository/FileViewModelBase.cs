using System;
using System.Linq;

namespace Ubora.Web._Features.Projects.Repository
{
    public abstract class FileViewModelBase
    {
        public string Comment { get; set; }
        public string FileName { get; set; }
        public int RevisionNumber { get; set; }

        public bool Has3DFileExtension()
        {
            var extension = System.IO.Path.GetExtension(FileName);

            var supported3dFileExtensions = new[] { ".stl", ".amf", ".nxz" };
            return supported3dFileExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase);
        }
    }
}
