using MessageBus.Handler.Contracts;
using MassTransit; 

namespace MessageBus.Handler.QuotesImportProcessor
{
    public abstract  class EventConsumer<T> where T : class, IQuotesEvent
    {
        public abstract Task OnEventReceived(IEventContext<T> context);
        public virtual async Task OnEventReceived(ConsumeContext<T> context)
        {
            await OnEventReceived(new EventContext<T>(context));
        }
    }

    public class EventContext<T> : IEventContext<T> where T: class
    {
        public T Message { get; set; }
        public Guid? CorrelationId { get; set; }
        public EventContext(ConsumeContext<T> context)
        {
            this.Message = context.Message;
            this.CorrelationId = context.CorrelationId;
         
        }
    }

    public interface IEventContext<out T>
    {
        public T Message { get; }
        public Guid? CorrelationId { get; }
    }
}
