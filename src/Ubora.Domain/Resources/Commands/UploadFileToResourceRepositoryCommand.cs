using System;
using System.IO;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Resources.Commands
{
    public class UploadFileToResourceRepositoryCommand : UserCommand
    {
        public Guid FileId { get; set; }
        public Guid ResourcePageId { get; set; }
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public Stream FileStream { get; set; }
    }
}