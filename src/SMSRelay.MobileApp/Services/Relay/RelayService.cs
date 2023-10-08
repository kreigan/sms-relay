﻿using SMSRelay.Core.Model;
using SMSRelay.MobileApp.Model;
using SMSRelay.MobileApp.Services.Settings;

namespace SMSRelay.MobileApp.Services.Relay;

public class RelayService : IRelayService
{
    private ISettingsService _settingsService;

    public RelayService(ISettingsService settingsService)
    {
        _settingsService = settingsService;
    }

    public ReceivedMessage Relay(TextMessage receivedMessage)
    {
        throw new NotImplementedException();
    }
}