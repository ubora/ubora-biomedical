using System;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects.CommercialDossiers.Events;
using Ubora.Domain.Projects.Workpackages.Events;

namespace Ubora.Domain.Projects.CommercialDossiers
{
    public class UserManual 
    {
        public UserManual(BlobLocation location, string fileName, long fileSize)
        {
            Location = location;
            FileName = fileName;
            FileSize = fileSize;
        }

        public BlobLocation Location { get; }
        public string FileName { get; }
        public long FileSize { get; }
    }

    public class CommercialDossier : Entity<CommercialDossier>, IProjectEntity
    {
        public Guid Id { get; private set; }
        public Guid ProjectId { get; private set; }

        public string ProductName { get; private set; }
        public string CommercialName { get; private set; }
        public QuillDelta Description { get; private set; }
        public UserManual UserManual { get; private set; }
        public BlobLocation Logo { get; private set; }

        private void Apply(WorkpackageFiveOpenedEvent e)
        {
            ProjectId = e.ProjectId;
            ProductName = "";
            CommercialName = "";
            Description = new QuillDelta();
        }

        private void Apply(CommercialNameChangedEvent e) 
        {
            if (CommercialName == e.Value)
                throw new InvalidOperationException("Was not actually changed.");
            CommercialName = e.Value ?? throw new InvalidOperationException("NULL not allowed.");
        }

        private void Apply(DescriptionEditedEvent e) 
        {
            if (Description == e.Value)
                throw new InvalidOperationException("Was not actually changed.");
            Description = e.Value ?? throw new InvalidOperationException("NULL not allowed.");
        }

        private void Apply(ProductNameChangedEvent e) 
        {
            if (ProductName == e.Value)
                throw new InvalidOperationException("Was not actually changed.");
            ProductName = e.Value ?? throw new InvalidOperationException("NULL not allowed.");
        }

        private void Apply(LogoChangedEvent e) 
        {
            if (Logo == e.Value)
                throw new InvalidOperationException("Was not actually changed.");
            Logo = e.Value;
        }

        private void Apply(UserManualChangedEvent e) 
        {
            if (UserManual?.Location == e.Location)
                throw new InvalidOperationException("Was not actually changed.");
            UserManual = new UserManual(e.Location, e.FileName, e.FileSize);
        }
    }
}