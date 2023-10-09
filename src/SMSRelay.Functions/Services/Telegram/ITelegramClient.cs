using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Telegram.Bot;

namespace SMSRelay.Functions.Services.Telegram;

public interface ITelegramClient
{

}

public class TelegramClient : ITelegramClient
{
    private readonly ITelegramBotClient _botClient;


}