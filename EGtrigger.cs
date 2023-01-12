using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using System.Xml.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;

public class EventItem
{
    public long DeviceId { get; set; }
    public string Image { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string Text { get; set; }
    public string Name { get; set; }
}

public class ThingItem
{
    public long Thingid { get; set; }
    public string Name { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string Image { get; set; }
    public string Text { get; set; }
    public string Status { get; set; }
}

namespace ThingzEGTrigger
{
    public static class EGtrigger
    {
        [FunctionName("EGtrigger")]
        public static void Run([EventGridTrigger] EventGridEvent eventGridEvent, ILogger log)
        {

            log.LogInformation(eventGridEvent.Data.ToString());

            var jsonEvent = JsonSerializer.Deserialize<EventItem>(eventGridEvent.Data.ToString());

            var ThingId = jsonEvent.DeviceId + 500;
            log.LogInformation("Thing Id: " + ThingId);

            ThingItem thingItem = new ThingItem
            {
                Thingid = jsonEvent.DeviceId + 500,
                Latitude = jsonEvent.Latitude,
                Longitude = jsonEvent.Longitude,
                Image = jsonEvent.Image,
                Text = jsonEvent.Text,
                Name = jsonEvent.Name
            };

            var jsonThing = JsonSerializer.Serialize<ThingItem>(thingItem);

            log.LogInformation("Thing >>>" + jsonThing.ToString());

            string logicappurl = System.Environment.GetEnvironmentVariable("LogicAppUrl", EnvironmentVariableTarget.Process);

            using (var client = new HttpClient())
            {
                var response = client.PostAsync(
                    logicappurl,
                    new StringContent(jsonThing, Encoding.UTF8, "application/json")).Result;

                log.LogInformation("Response >>>" + response.StatusCode);
            }

        }
    }
}