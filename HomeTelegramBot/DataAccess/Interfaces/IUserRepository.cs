using Telegram.Bot.Types;

namespace HomeTelegramBot.DataAccess.Interfaces
{
    public interface IUserRepository
    {
        bool IsUserAuthorized(User user);
    }
}