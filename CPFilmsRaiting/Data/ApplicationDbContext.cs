﻿using CPFilmsRaiting.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPFilmsRaiting.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<GenreModel>().HasKey(g => new { g.FilmId, g.Genre });
        }


        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<CommentModel> Comments { get; set; }
        public DbSet<FilmModel> Films { get; set; }
        public DbSet<RaitingModel> Raiting { get; set; }
        public DbSet<GenreModel> Genres { get; set; }
    }
}
