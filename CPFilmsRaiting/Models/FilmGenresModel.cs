using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CPFilmsRaiting.Models
{
    public class FilmGenresModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string GenreId { get; set; }

        [Required]
        public string FilmId { get; set; }

        public FilmModel Film { get; set; }
        public GenreModel Genre { get; set; }

    }
}
