using CPFilmsRaiting.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPFilmsRaiting.Data.Repositories
{
    public class LikesRepository : IRepository<LikeModel>
    {
        ApplicationDbContext _context { get; set; }

        public LikesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<LikeModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<LikeModel> GetAll(string id)
        {
            throw new NotImplementedException();
        }

        public LikeModel Get(string id)
        {
            throw new NotImplementedException();
        }

        public LikeModel Get(LikeModel likeModel)
        {
            var item = _context.Likes.FirstOrDefault(l => l.FilmId == likeModel.FilmId && l.UserId == likeModel.UserId);
            return item;
        }

        public List<LikeModel> GetByUserId(string id)
        {
            var item = _context.Likes.Where(l => l.UserId == id).ToList();
            return item;
        }

        public void Create(LikeModel item)
        {
            var i = _context.Likes.FirstOrDefault(l => l.FilmId == item.FilmId && l.UserId == item.UserId);
            if (i != null)
            {
                _context.Likes.Remove(i);
            }
            else
            {
                _context.Likes.Add(item);
            }
            _context.SaveChanges();
        }

        public void Update(LikeModel item)
        {
            throw new NotImplementedException();
        }

        public void Delete(string id)
        {
            throw new NotImplementedException();
        }
    }
}
