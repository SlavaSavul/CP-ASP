using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPFilmsRaiting.Models
{
    public class CommentModel
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string FilmId { get; set; }
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
        public FilmModel Film { get; set; }
    }
}
