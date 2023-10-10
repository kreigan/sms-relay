namespace SMSRelay.Core;

public static class Constants
{
    public const string AppParamDbConnection = "AZURE_COSMOS_RESOURCEENDPOINT";
    public const string AppParamVaultConnection = "AZURE_KEYVAULT_RESOURCEENDPOINT";

    public const string CosmosDbName = "smsrelay";
    public const string CosmosDbAccountsName = "accounts";
    public const string CosmosDbMessagesName = "messages";
}
