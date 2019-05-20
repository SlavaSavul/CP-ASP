using CPFilmsRaiting.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPFilmsRaiting.Data.Repositories
{
    public class UsersRepository : IRepository<ApplicationUser>
    {
        ApplicationDbContext _context { get; set; }

        public bool IsExists(string name, string password)
        {
            ApplicationUser applicationUser = _context.Users
                .FirstOrDefault(user => user.Email == name);
            return applicationUser != null && applicationUser.Password == password;
        }

        public UsersRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<ApplicationUser> GetAll()
        {
            throw new NotImplementedException();
        }

        public ApplicationUser GetByEmail(string email)
        {
            return _context.Users
                    .FirstOrDefault(user => user.Email == email);
        }

        public ApplicationUser Get(string id)
        {
            return _context.Users
                    .FirstOrDefault(user => user.Id == id);
        }

        public void Create(ApplicationUser item)
        {
            _context.Users.Add(item);
            _context.SaveChanges();
        }

        public void Update(ApplicationUser item)
        {
            throw new NotImplementedException();
        }

        public void Delete(string id)
        {
            throw new NotImplementedException();
        }
    }
}
