using System.Diagnostics.CodeAnalysis;

using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

using SMSRelay.Azure.Model;
using SMSRelay.Core.Model;

namespace SMSRelay.Functions;

public class ReceiveMessage
{
    private readonly ILogger _logger;

    public ReceiveMessage(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<ReceiveMessage>();
    }

    [SuppressMessage("Style", "IDE0060:Remove unused parameter")]
    [Function("ReceiveMessage")]
    [CosmosDBOutput("smsrelay", "messages", Connection = Constants.AppParamDbConnection)]
    public RelayedMessage Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "relay")] HttpRequestData req,
        [FromBody] ReceivedMessage message)
    {
        _logger.LogInformation("Received message {} for relay", message.Id);
        return new()
        {
            TextMessageId = message.Id,
            Sender = message.Sender,
            Receiver = message.RecipientPhoneNumber,
            Body = message.Body,
            TextMessageReceivedAt = message.ReceivedAt,
            ReceivedAt = DateTime.UtcNow,
            Status = RelayStatus.NotRelayed
        };
    }
}
