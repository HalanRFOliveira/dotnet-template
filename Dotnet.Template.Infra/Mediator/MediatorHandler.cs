using Dotnet.Template.Infra.Messaging;
using MediatR;
using System.Runtime.CompilerServices;

namespace Dotnet.Template.Infra.Mediator
{
    public class MediatorHandler : IMediatorHandler
    {
        private readonly IMediator _mediator;

        public MediatorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }


        public Task<CommandResponse<TResult>> SendCommandAsync<TResult>(Command<TResult> command)
        {
            return _mediator.Send(command);
        }
    }
}
