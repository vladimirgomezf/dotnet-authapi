using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_authapi.Models
{
    public class UserRepository : IUserRepository
    {
        private readonly authapiContext _context;

        public UserRepository (authapiContext context)
        {
            _context = context;
        }

        public User Create(User user)
        {
            _context.Users.Add(user);
            user.Id = _context.SaveChanges();

            return user;
        }

        public User GetByEmail (string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }

        public User GetByiD(int id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }
    }
}
