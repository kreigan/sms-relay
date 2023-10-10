using System.Net;

using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

using SMSRelay.Core;
using SMSRelay.Core.Model;

namespace SMSRelay.Receiver;

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
        [FromBody] ReceivedMessage receivedMessage)
    {
        _logger.LogInformation("Received message {id} for relay", receivedMessage.Id);

        Container messagesContainer = _cosmosClient.GetContainer(Constants.CosmosDbName, "messages");

        var query = new QueryDefinition("SELECT TOP 1 * FROM messages m WHERE m.textMessageId = @textMessageId")
            .WithParameter("@textMessageId", receivedMessage.Id);

        HttpResponseData response = request.CreateResponse();

        using FeedIterator<SavedMessage> feed = messagesContainer.GetItemQueryIterator<SavedMessage>(query);
        if (feed.HasMoreResults)
        {
            FeedResponse<SavedMessage> existingMessages = await feed.ReadNextAsync();
            if (existingMessages.Count > 0)
            {
                SavedMessage existingMessage = existingMessages.First();

                string details = "Message with incoming ID {0} already exists in DB: {1}";
                _logger.LogInformation(details, receivedMessage.Id, existingMessage.Id);

                await response.WriteAsJsonAsync(new
                {
                    invocationId = request.FunctionContext.InvocationId,
                    errorMessage = string.Format(details, receivedMessage.Id, existingMessage.Id)
                });
                response.StatusCode = HttpStatusCode.Conflict;
                return response;
            }
        }

        SavedMessage message = new()
        {
            TextMessageId = receivedMessage.Id,
            Sender = receivedMessage.Sender,
            Receiver = receivedMessage.RecipientPhoneNumber,
            Body = receivedMessage.Body,
            TextMessageReceivedAt = receivedMessage.ReceivedAt,
            ReceivedAt = DateTime.UtcNow,
            Status = RelayStatus.NotRelayed
        };

        ItemResponse<SavedMessage> savedMessage = await messagesContainer.CreateItemAsync(message);
        _logger.LogInformation("Incoming message {} persisted with ID {}", receivedMessage.Id, savedMessage.Resource.Id);

        await response.WriteAsJsonAsync(savedMessage.Resource);
        return response;
    }
}
