using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CPFilmsRaiting.Models
{
    public class FilmModel
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string PosterURL { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public double IMDbRaiting { get; set; }

        public List<GenreModel> Genres { get; set; }
        public List<CommentModel> Comments { get; set; }
        public List<RaitingModel> Raitings { get; set; }
    }
}
