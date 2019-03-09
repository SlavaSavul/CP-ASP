using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPFilmsRaiting.Models
{
    public class FilmModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PosterURL { get; set; }

        public List<CommentModel> Comments { get; set; }
        public List<RaitingModel> Raitings { get; set; }
    }
}
