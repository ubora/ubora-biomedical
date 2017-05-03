﻿using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects
{
    // TODO: Jäi poolikuks
    public class EditProductSpecificationCommand : ICommand
    {
        public string Functionality { get; set; }
        public string Performance { get; set; }
        public string Usability { get; set; }
        public string Safety { get; set; }
    }

    public class EditProductSpecificationCommandHandler : ICommandHandler<EditProductSpecificationCommand>
    {
        private readonly IDocumentSession _documentSession;

        public EditProductSpecificationCommandHandler(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        public ICommandResult Handle(EditProductSpecificationCommand command)
        {
            throw new NotImplementedException();
        }
    }

    public class ProductSpecificationEditedEvent : UboraEvent
    {
        public string Functionality { get; set; }
        public string Performance { get; set; }
        public string Usability { get; set; }
        public string Safety { get; set; }

        public ProductSpecificationEditedEvent(UserInfo initiatedBy) : base(initiatedBy)
        {
        }

        public override string GetDescription()
        {
            return "Product specification edited.";
        }
    }
}
