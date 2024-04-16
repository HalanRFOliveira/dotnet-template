using MediatR;

namespace Dotnet.Template.Infra.Messaging
{
    public abstract class Event : Message, INotification
    {
        protected Event()
        {
            TimeStamp = DateTime.Now;
        }

        public DateTime TimeStamp { get; private set; }
    }
}
