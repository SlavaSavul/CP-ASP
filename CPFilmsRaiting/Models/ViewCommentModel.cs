using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPFilmsRaiting.Models
{
    public class ViewCommentModel
    {
        public string Id { get; set; }

        public string FilmId { get; set; }

        public string UserName { get; set; }

        public string Description { get; set; }

        public DateTime Date { get; set; }

        public ViewCommentModel() {}
        public ViewCommentModel(CommentModel commnetModel, string userName) {
            Id = commnetModel.Id;
            FilmId = commnetModel.FilmId;
            Description = commnetModel.Description;
            Date = commnetModel.Date;
            UserName = userName;
        }
    }
}
