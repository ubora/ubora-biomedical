﻿using System;
using System.Collections.Generic;
using System.Text;
using Marten;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Users.Commands
{
    public class DeleteUserCommand : UserCommand
    {
        public Guid UserId { get; set; }
        public class Handler : ICommandHandler<DeleteUserCommand>
        {
        

            private IDocumentSession _documentSession;

            public Handler(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
            }

            public ICommandResult Handle(DeleteUserCommand cmd)
            {

                var user = _documentSession.LoadOrThrow<UserProfile>(cmd.UserId);
                //Hard Delete 
                _documentSession.Delete(user.UserId);
                _documentSession.SaveChanges();
                return CommandResult.Success;
            }
        }
    }
}
