using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CPFilmsRaiting.Models
{
    public class FilmGenresModel
    {
        public string Id { get; set; }

        public string GenreId { get; set; }
        public GenreModel Genre { get; set; }

        public string FilmId { get; set; }
        public FilmModel Film { get; set; }
    }
}
