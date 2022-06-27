using MessageBus.Handler.Contracts;
using System;
using System.Web;

namespace MessageBus.Handler.QuotesImportProcessor
{

    public class QuotesImportedEventHandler : EventConsumer<IQuotesEvent>
    {
        public override async Task OnEventReceived(IEventContext<IQuotesEvent> context)
        {
            if (context != null)
            {
                Console.WriteLine("Message received");
                Console.WriteLine(context.Message);
            }
        }

    }
}
