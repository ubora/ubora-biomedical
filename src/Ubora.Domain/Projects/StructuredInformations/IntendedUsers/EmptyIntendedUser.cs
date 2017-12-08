using System;
using System.Collections.Generic;
using System.Text;

namespace Ubora.Domain.Projects.StructuredInformations.IntendedUsers
{
    public class EmptyIntendedUser : IntendedUser
    {
        public override string Key => "empty";

        public override string ToDisplayName()
        {
            return "";
        }
    }
}
