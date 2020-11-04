namespace FlatFilesConverter.Data
{
    public interface IUserDAO
    {
        User GetUser(string username);
        int AuthenticateUser(User user);
        bool RegisterUser(User user);
    }
}