using CPFilmsRaiting.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPFilmsRaiting.Data
{
    public class UnitOfWork : IDisposable
    {
        ApplicationDbContext _context { get; set; }
        FilmRepository filmRepository;
        CommentsRepository commentsRepository;
        RaitingRepository raitingRepository;
        UsersRepository usersRepository;
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public FilmRepository Films
        {
            get
            {
                if (filmRepository == null)
                    filmRepository = new FilmRepository(_context);
                return filmRepository;
            }
        }

        public CommentsRepository Comments
        {
            get
            {
                if (commentsRepository == null)
                    commentsRepository = new CommentsRepository(_context);
                return commentsRepository;
            }
        }

        public RaitingRepository Raitings
        {
            get
            {
                if (raitingRepository == null)
                    raitingRepository = new RaitingRepository(_context);
                return raitingRepository;
            }
        }

        public UsersRepository Users
        {
            get
            {
                if (usersRepository == null)
                    usersRepository = new UsersRepository(_context);
                return usersRepository;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
