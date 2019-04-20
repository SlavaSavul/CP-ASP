using CPFilmsRaiting.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPFilmsRaiting.Data.Repositories
{
    public class FilmRepository : IRepository<FilmModel>
    {
        ApplicationDbContext _context { get; set; }

        public FilmRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Create(FilmModel item)
        {
            if (_context.Films.FirstOrDefault(film => film.Name == item.Name) == null)
            {
                _context.Films.Add(item);
                _context.SaveChanges();
            }
        }

        public void Delete(string id)
        {
            FilmModel film = _context.Films
               .Include(f => f.Genres)
               .Include(f => f.Comments)
               .FirstOrDefault(f => f.Id == id);
            _context.Genres.RemoveRange(film.Genres);
            _context.Comments.RemoveRange(film.Comments);
            _context.Films.Remove(film);
            _context.SaveChanges();
        }

        public FilmModel Get(string id)
        {
            FilmModel film = _context.Films
                .Include(f => f.Comments)
                .Include(f => f.Genres)
                .FirstOrDefault(f => f.Id == id);
            film.Comments = film.Comments.OrderByDescending(c => c.Date).ToList();
            return film;

        }

        public IEnumerable<FilmModel> GetAllWithInclude()
        {
            return _context.Films
                .Include(film => film.Comments)
                .Include(film => film.Raitings)
                .Include(film => film.Genres);
        }

        public IEnumerable<FilmModel> GetAll()
        {
            return _context.Films;
        }

        public void Update(FilmModel item)
        {
            FilmModel film = _context.Films
                .Include(f => f.Comments)
                .Include(f => f.Raitings)
                .Include(f => f.Genres)
                .FirstOrDefault(f => f.Id == item.Id);

            film.Name = item.Name;
            film.Description = item.Description;
            film.Date = item.Date;
            film.IMDbRaiting = item.IMDbRaiting;
            film.PosterURL = item.PosterURL;
            film.IMDbRaiting = item.IMDbRaiting;
            film.Date = item.Date;

            List<string> deleted = film.Genres.Select(q => q.Genre).Except(item.Genres.Select(p => p.Genre)).ToList();
            List<string> inserted = item.Genres.Select(q => q.Genre).Except(film.Genres.Select(p => p.Genre)).ToList();

            film.Genres.RemoveAll(p=> deleted.IndexOf(p.Genre) >= 0);
            film.Genres.InsertRange(0, inserted.Select(g => new GenreModel() { Genre = g }).ToList());

            _context.SaveChanges();
        }
    }
}
