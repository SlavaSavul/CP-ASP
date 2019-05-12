using CPFilmsRaiting.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPFilmsRaiting.Data.Repositories
{
    public class CommentsRepository : IRepository<CommentModel>
    {
        ApplicationDbContext _context { get; set; }

        public CommentsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<CommentModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CommentModel> GetComments(string id)
        {
            return _context
                .Comments
                .Where(c => c.FilmId == id)
                .OrderByDescending(c => c.Date);
        }

        public CommentModel Get(string id)
        {
            throw new NotImplementedException();
        }

        public void Create(CommentModel item)
        {
            _context.Comments.Add(item);
            _context.SaveChanges();
        }

        public void Update(CommentModel item)
        {
            throw new NotImplementedException();
        }

        public void Delete(string id)
        {
            throw new NotImplementedException();
        }
    }
}
