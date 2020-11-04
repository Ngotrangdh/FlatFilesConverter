using FlatFilesConverter.Data;
using System;

namespace FlatFilesConverter.Business.Services
{
    public class UserService
    {
        private readonly IUserDAO _userDAO;

        public UserService(): this(new UserDAO())
        {}

        public UserService(IUserDAO userDAO)
        {
            _userDAO = userDAO ?? throw new ArgumentNullException(nameof(userDAO));
        }

        public int AuthenticateUser(User user)
        {
            return _userDAO.AuthenticateUser(user);
        }

        public bool RegisterUser(User user)
        {
            return _userDAO.RegisterUser(user);
        }

        public User GetUser(string username)
        {
            return _userDAO.GetUser(username);
        }
    }
}
