using Marten;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Web._Features.Feedback
{
    public class SendFeedbackCommand : UserCommand
    {
        public string Feedback { get; set; }
        public string FromPath { get; set; }

        internal class Handler : ICommandHandler<SendFeedbackCommand>
        {
            private readonly IDocumentSession _documentSession;

            public Handler(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
            }

            public ICommandResult Handle(SendFeedbackCommand cmd)
            {
                var feedback = new Feedback(cmd.Feedback, cmd.FromPath, cmd.Actor);

                _documentSession.Store(feedback);
                _documentSession.SaveChanges();

                return new CommandResult();
            }
        }
    }
}