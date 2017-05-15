using HomeTelegramBot.DataAccess.Interfaces;
using Telegram.Bot.Types;

namespace HomeTelegramBot.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        public bool IsUserAuthorized(User user)
        {
            //implement later
            return true;
        }
    }
}