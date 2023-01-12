// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName=ECTrigger
// [  { "id": "'1",  "eventType": "yourEventType",  "eventTime":"10:59:00.000", "subject": "subjectx", 
//     "data":{"make": "Volvo", "model": "XC60 T8"}, "dataVersion": "1.0" }  ]
// Header    aeg-event-type: Notification

using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;

namespace ThingzEGTrigger
{
    public static class EGtrigger2
    {
        [FunctionName("EGtrigger2")]
        public static void Run([EventGridTrigger]EventGridEvent eventGridEvent, ILogger log)
        {
            log.LogInformation("C# EG trigger function processed a request:" + eventGridEvent.Data.ToString());
        }
    }
}
