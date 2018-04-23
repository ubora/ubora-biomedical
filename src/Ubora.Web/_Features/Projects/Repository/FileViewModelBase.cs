using System;
using System.IO;
using System.Linq;

namespace Ubora.Web._Features.Projects.Repository
{
    public abstract class FileViewModelBase
    {
        private static readonly string[] Supported3DFileExtensions = new[] { ".stl", ".amf", ".nxz" };
        
        public string Comment { get; set; }
        public string FileName { get; set; }
        public int RevisionNumber { get; set; }

        public bool Has3DFileExtension()
        {
            var extension = Path.GetExtension(FileName);

            return Supported3DFileExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase);
        }
    }
}
