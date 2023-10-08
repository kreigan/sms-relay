﻿using SMSRelay.Core.Model;
using SMSRelay.MobileApp.Model;

namespace SMSRelay.MobileApp.Services.Relay;

public interface IRelayService
{
    ReceivedMessage Relay(TextMessage receivedMessage);
}