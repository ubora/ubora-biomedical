using System;
using System.Collections.Generic;

namespace Ubora.Domain.Infrastructure
{
    public class BlobLocation
    {
        public static class ContainerNames
        {
            public const string Projects = "projects";
            public const string Users = "users";
        }

        public string ContainerName { get; private set; }
        public string BlobPath { get; private set; }

        public BlobLocation(string containerName, string blobPath)
        {
            ContainerName = containerName;
            BlobPath = blobPath;
        }

        public override bool Equals(object obj)
        {
            var other = obj as BlobLocation;
            if (other == null)
            {
                return false;
            }

            return ContainerName.Equals(other.ContainerName, StringComparison.OrdinalIgnoreCase)
                && BlobPath.Equals(other.BlobPath, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            var hashCode = -1407821204;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ContainerName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(BlobPath);
            return hashCode;
        }
    }
}