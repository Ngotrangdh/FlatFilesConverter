using FlatFileConverter.Data;

namespace FlatFilesConverter.Business.Authentication
{
    public class UserService
    {
        public static bool IsAuthenticated(User user)
        {
            return UserDAO.IsUserAuthenticated(user);
        }
        
        public static bool RegisterUser(User user)
        {
            return UserDAO.RegisterUser(user);
        }
    }
}
