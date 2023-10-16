using System.Text;
using System.Text.Json;

using SMSRelay.Core.Model;
using SMSRelay.MobileApp.Services.Settings;

namespace SMSRelay.MobileApp.Services.Relay;

public class RelayService : IRelayService
{
    private readonly ISettingsService _settingsService;
    private readonly JsonSerializerOptions _options;
    private readonly HttpClient _httpClient;

    public RelayService(ISettingsService settingsService)
    {
        _settingsService = settingsService;
        
        _options = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        _httpClient = new HttpClient()
        {
            BaseAddress = new Uri(_settingsService.RemoteRelayReceiverUri)
        };
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _settingsService.RemoteRelayReceiverApiSecret);
    }

    public async Task<bool> RelayAsync(ReceivedMessage message, CancellationToken cancellationToken) => await Send(message, cancellationToken);

    private async Task<bool> Send(ReceivedMessage message, CancellationToken cancellationToken)
    {
        var request = new StringContent(JsonSerializer.Serialize(message, _options), Encoding.UTF8, "application/json");
        HttpResponseMessage response = await _httpClient.PostAsync(null as string, request, cancellationToken);
        return response.IsSuccessStatusCode;
    }
}
