using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CPFilmsRaiting.Models
{
    public class GenreModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Genre { get; set; }

        //[Required]
        //public string FilmId { get; set; }

    }
}
