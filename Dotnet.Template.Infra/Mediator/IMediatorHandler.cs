using Dotnet.Template.Infra.Messaging;

namespace Dotnet.Template.Infra.Mediator
{
    public interface IMediatorHandler
    {
        Task<CommandResponse<TResult>> SendCommandAsync<TResult>(Command<TResult> command);
    }
}
