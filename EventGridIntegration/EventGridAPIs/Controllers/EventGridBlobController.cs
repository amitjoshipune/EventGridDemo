using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.EventGrid.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EventGridWebhookProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventGridBlobController : ControllerBase
    {
        [HttpPost]
        public IActionResult HandleEventGridEvents([FromBody] List<EventGridEvent> events)
        {
            foreach (var eventGridEvent in events)
            {
                Console.WriteLine($"Received event: {eventGridEvent.EventType}");

                // **Handle subscription validation**
                if (eventGridEvent.EventType == "Microsoft.EventGrid.SubscriptionValidationEvent")
                {
                    var validationData = JsonConvert.DeserializeObject<SubscriptionValidationEventData>(eventGridEvent.Data.ToString());
                    return Ok(new { validationResponse = validationData.ValidationCode });
                }

                // **Handle system events (e.g., blob events)**
                if (eventGridEvent.EventType.StartsWith("Microsoft.Storage."))
                {
                    Console.WriteLine($"Azure Storage Event Received: {eventGridEvent.Subject}");
                    Console.WriteLine($"Azure Storage Event Received: {eventGridEvent.Id}");
                    Console.WriteLine($"Azure Storage Event Received: {eventGridEvent.Data}");
                }

                // **Handle custom events**
                if (eventGridEvent.EventType == "CustomEventType")
                {
                    Console.WriteLine($"Custom event received: {eventGridEvent.Subject}");
                }
            }

            return Ok();
        }
    }
}
