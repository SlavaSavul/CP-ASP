using CPFilmsRaiting.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPFilmsRaiting.Data.Repositories
{
    public class RaitingRepository : IRepository<RaitingModel>
    {
        ApplicationDbContext _context { get; set; }

        public RaitingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Create(RaitingModel item)
        {
            _context.Raiting.Add(item);
            _context.SaveChanges();
        }

        public void Delete(string id)
        {
            throw new NotImplementedException();
        }

        public RaitingModel Get(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<RaitingModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(RaitingModel item)
        {
            throw new NotImplementedException();
        }
    }
}
