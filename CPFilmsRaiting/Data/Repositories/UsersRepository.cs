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

        public ApplicationUser FindUser(string name, string password)
        {
            return _context.Users
                .FirstOrDefault(user => user.Email == name && user.Password == password);
        }

        public UsersRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<ApplicationUser> GetAll()
        {
            throw new NotImplementedException();
        }

        public ApplicationUser Get(string id)
        {
            return _context.Users
                    .Include(user => user.Comments)
                    .Include(user => user.Raitings)
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
