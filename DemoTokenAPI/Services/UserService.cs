using DemoTokenAPI.Models;

namespace DemoTokenAPI.Services
{
    public class UserService
    {
        public List<User> UserAccount { get; private set; } 
        public UserService() { 
            UserAccount = new List<User>();
            UserAccount.Add(new User
            {
                Id = 1,
                Email = "admin@mail.com",
                Password = "Test1234",
                UserName = "Administrator",
                IsAdmin = true
            });
            UserAccount.Add(new User
            {
                Id = 2,
                Email = "user@mail.com",
                Password = "Test1234",
                UserName = "Simple User",
                IsAdmin = false
            });
        }

        public List<User> GetAllUsers()
        {
            return UserAccount;
        }

        public User GetById(int id)
        {
            return UserAccount.First(x => x.Id == id);
        }

        public User Login(string email, string password)
        {
            try
            {
                return UserAccount.First(x => x.Email == email
                                            && x.Password == password);
            }
            catch(Exception ex)
            {
                throw new ArgumentNullException("Email ou MDP Incorrect");
            }
            
        }
    }
}
