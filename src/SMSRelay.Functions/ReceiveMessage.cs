using System.Net;
using System.Text.Json;

using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

using SMSRelay.Core.Model;
using SMSRelay.Functions.Model;

namespace SMSRelay.Functions;

public class ReceiveMessage
{
    private readonly ILogger _logger;
    private readonly CosmosClient _cosmosClient;

    public ReceiveMessage(ILoggerFactory loggerFactory, CosmosClient cosmosClient)
    {
        _logger = loggerFactory.CreateLogger<ReceiveMessage>();
        _cosmosClient = cosmosClient;
    }

    [Function("ReceiveMessage")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger("post", Route = "relay")] HttpRequestData request,
        [FromBody] ReceivedMessage message)
    {
        _logger.LogInformation("Received message {id} for relay", message.Id);

        Container messagesContainer = _cosmosClient.GetContainer(Constants.CosmosDbName, Constants.CosmosDbMessagesName);

        var query = new QueryDefinition("SELECT * FROM messages m WHERE m.textMessageId = @textMessageId")
            .WithParameter("@textMessageId", message.Id);

        HttpResponseData response;

        using FeedIterator<RelayedMessage> feed = messagesContainer.GetItemQueryIterator<RelayedMessage>(query);
        if (feed.HasMoreResults)
        {
            FeedResponse<RelayedMessage> existingMessages = await feed.ReadNextAsync();
            if (existingMessages.Count > 0)
            {
                response = request.CreateResponse(HttpStatusCode.Conflict);
                response.WriteString($"Message with ID {message.Id} has already been accepted for relay");
                _logger.LogDebug("Found message in DB: {}", JsonSerializer.Serialize(existingMessages.First()));
                return response;
            }
        }

        RelayedMessage relayedMessage = new()
        {
            TextMessageId = message.Id,
            Sender = message.Sender,
            Receiver = message.RecipientPhoneNumber,
            Body = message.Body,
            TextMessageReceivedAt = message.ReceivedAt,
            ReceivedAt = DateTime.UtcNow,
            Status = RelayStatus.NotRelayed
        };

        ItemResponse<RelayedMessage> savedMessage = await messagesContainer.CreateItemAsync(relayedMessage);
        _logger.LogInformation("Received message {receivedId} persisted as message {id}", message.Id, relayedMessage.Id);

        response = request.CreateResponse();
        await response.WriteAsJsonAsync(savedMessage.Resource);
        return response;
    }
}
