using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace EventGridWebhookProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventGridController : ControllerBase
    {
        [HttpPost]
        public IActionResult HandleEvent([FromBody] JsonElement eventGridEvent)
        {
            // Validate the Event Grid handshake
            if (eventGridEvent.GetProperty("eventType").GetString() == "Microsoft.EventGrid.SubscriptionValidationEvent")
            {
                var validationCode = eventGridEvent.GetProperty("data").GetProperty("validationCode").GetString();
                return Ok(new { validationResponse = validationCode });
            }

            // Log the event details
            Console.WriteLine($"Event received: {eventGridEvent}");

            // Process the event
            // Example: Check for a custom event type
            var eventType = eventGridEvent.GetProperty("eventType").GetString();
            if (eventType == "CustomEventType")
            {
                // Perform custom logic
                Console.WriteLine("Custom event received!");
            }

            return Ok();
        }

    }
}
