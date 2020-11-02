using FlatFilesConverter.Data;

namespace FlatFilesConverter.Business.Services
{
    public class UserService
    {
        private UserDAO UserDAO => new UserDAO();

        public int IsAuthenticated(User user)
        {
            return UserDAO.IsUserAuthenticated(user);
        }

        public bool RegisterUser(User user)
        {
            return UserDAO.RegisterUser(user);
        }

        public User GetUser(string username)
        {
            return UserDAO.GetUser(username);
        }
    }
}
