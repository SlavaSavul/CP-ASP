using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CPFilmsRaiting.Models
{
    public class CommentModel
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string FilmId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public FilmModel Film { get; set; }
        public ApplicationUser User { get; set; }
    }
}
