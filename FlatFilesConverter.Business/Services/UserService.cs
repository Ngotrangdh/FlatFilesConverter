using FlatFileConverter.Data;

namespace FlatFilesConverter.Business.Services
{
    public class UserService
    {
        public static int IsAuthenticated(User user)
        {
            return UserDAO.IsUserAuthenticated(user);
        }
        
        public static bool RegisterUser(User user)
        {
            return UserDAO.RegisterUser(user);
        }

        public static User GetUser(string username)
        {
            return UserDAO.GetUser(username);
        }
         
    }
}
