using Azure.Messaging.ServiceBus;
using MassTransit;
using MassTransit.Scheduling; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBus.Handler.QuotesImportProcessor
{
    public class QuotesImportService 
    {
        private IMessageScheduler _scheduler;

        public IBus Instance { get; set; }

        public IMessageScheduler Scheduler => _scheduler ??= new MessageScheduler(new ServiceBusScheduleMessageProvider(Instance), Instance.Topology);
        public virtual void Start() {
            ((IBusControl)Instance).Start();
        }
        public virtual void Stop() { ((IBusControl)Instance).Stop(); }
 
    }
}
