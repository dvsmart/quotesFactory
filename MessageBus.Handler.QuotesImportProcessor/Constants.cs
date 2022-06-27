using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBus.Handler.QuotesImportProcessor
{
    internal class Constants
    {
        // connection string to your Service Bus namespace
        static string ConnectionString = " Endpoint=sb://servicebus-quotes-import.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=JExvllXk3uhBYuPLf77wAJg63P8N2vv6B5kNwGbyaP8=";

        // name of your Service Bus topic
        static string TopicName = "importertopic"; 
    }
}
