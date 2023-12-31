﻿using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

using SMSRelay.Core;
using SMSRelay.Core.Model;

namespace SMSRelay.Forwarder;

public class RelayMessage
{
    private readonly ILogger _logger;

    public RelayMessage(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<RelayMessage>();
    }

    [Function("RelayMessage")]
    public void Run([CosmosDBTrigger(
        databaseName: "smsrelay",
        containerName: "messages",
        Connection = Constants.AppParamDbConnection,
        CreateLeaseContainerIfNotExists = true)] IReadOnlyList<SavedMessage> messages,
    FunctionContext functionContext)
    {
        var messagesToRelay = messages.Where(m => m.Status == RelayStatus.NotRelayed).ToList();
        _logger.LogInformation("{} messages to relay", messagesToRelay.Count);
        _logger.LogDebug("Message: {}", messagesToRelay[0]);
    }
}
