
//using TestiranjeProjekat.Data;
//using System.Collections.Generic;

//namespace TestiranjeProjekat.Service
//{
//    public interface IUserService
//    {
//        IEnumerable<User> GetAllUsers();
//        User GetUserById(int id);
//        void AddUser(User user);
//        void UpdateUser(User user);
//        void DeleteUser(int id);
//    }
//    public class UserService : IUserService
//    {
        
//        private readonly ApplicationDbContext _context;
//        public UserService(ApplicationDbContext context)
//        {
//            _context = context;
//        }
//        public void AddUser(User user)
//        {
//            _context.Users.Add(user);
//            _context.SaveChanges();

//        }

//        public void DeleteUser(int id)
//        {
//            throw new System.NotImplementedException();
//        }

//        public IEnumerable<User> GetAllUsers()
//        {
//            throw new System.NotImplementedException();
//        }

//        public User GetUserById(int id)
//        {
//            throw new System.NotImplementedException();
//        }

//        public void UpdateUser(User user)
//        {
//            throw new System.NotImplementedException();
//        }
//    }
//}