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
            throw new NotImplementedException();
        }

        public FilmModel Get(string id)
        {
            return _context.Films
                .Include(film => film.Comments)
                .Include(film => film.Raitings)
                .Include(film => film.Genres)
                .FirstOrDefault(film => film.Id == id);
        }

        public IEnumerable<FilmModel> GetAll()
        {
            return _context.Films;
        }

        public void Update(FilmModel item)
        {
            FilmModel film = _context.Films.Find(item.Id);
            film.Name = item.Name;
            film.Description = item.Description;
            film.Date = item.Date;
            film.IMDbRaiting = item.IMDbRaiting;
            film.PosterURL = item.PosterURL;

            _context.SaveChanges();
        }
    }
}
