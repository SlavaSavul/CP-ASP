using CPFilmsRaiting.Data;
using CPFilmsRaiting.Models;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPFilmsRaiting.Services
{
    public class DbService
    {
        UnitOfWork _unitOfWork;

        public DbService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Update(FilmModel film)
        {
            _unitOfWork.Films.Update(film);
        }

        public ApplicationUser GetUser(string emal)
        {
           return _unitOfWork.Users.GetByEmail(emal);
        }

        public List<LikeModel> GetUserLikes(string userId)
        {
            return _unitOfWork.Likes.GetByUserId(userId);
        }

        public void Create(FilmModel film)
        {
            _unitOfWork.Films.Create(film);
        }

        public void CreateComment(CommentModel comment)
        {
            _unitOfWork.Comments.Create(comment);
        }

        public void CreateLike(string userId, LikeModel likeModel)
        {
            _unitOfWork.Likes.Create(new LikeModel() {UserId = userId, FilmId = likeModel.FilmId});
        }

        public void CreateUser(ApplicationUser user)
        {
            _unitOfWork.Users.Create(user);
        }

        public bool IsUserExists(string email, string password)
        {
            return _unitOfWork.Users.IsExists(email, password);
        }

        public List<CommentModel> GetComments(string id)
        {
            return _unitOfWork.Comments.GetComments(id).ToList();
        }

        public FilmModel Get(string id)
        {
            return _unitOfWork.Films.Get(id);
        }

        public List<string> GetGanres()
        {   
            List<GenreModel> a = _unitOfWork.Films.GetAllWithInclude().SelectMany(f => f.Genres).ToList();

            return a.Select(g => g.Genre).Distinct().ToList();
        }

        public void Delete(string id)
        {
            _unitOfWork.Films.Delete(id);
        }

        public object Get(int page, int limit, int year,double raiting, string name, string genres, bool favorite, string userId )
        {
            int count = 0;
            IEnumerable<FilmModel> films = _unitOfWork.Films.GetAllWithInclude();
            IEnumerable<FilmModel> result = films.ToList();
            count = films.Count();

            if (favorite)
            {
                var favoriteFilms = new List<FilmModel>();
                var likes = _unitOfWork.Likes.GetByUserId(userId);
                foreach (LikeModel like in likes)
                {
                    favoriteFilms.Add(_unitOfWork.Films.GetWithoutInclude(like.FilmId));
                }
                result = favoriteFilms;
            }

            if (year > 0)
            {
                result = result.Where(f => f.Date.Year == year);
            }

            if (raiting > 0)
            {
                result = result.Where(f => ((double)f.IMDbRaiting) >= raiting);
            }

            if (name != "")
            {
                result = result.Where(f => f.Name.Contains(name));
            }

            if (genres != "")
            {
                List<string> genresList = genres.Split(',').ToList();
                result = result.Where(film => {
                    List<string> filmGenres = film.Genres.Select(g => g.Genre).ToList();
                    List<string> exists = filmGenres.Where(g => genresList.Any(g2 => g.Equals(g2))).ToList();
                    return exists.Count() > 0;
                });
            }

            count = result.Count();
            if ( page > 0 && limit > 0 )
            {
                result = result.Skip((page - 1) * limit).Take(limit);
            }
            var response = new
            {
                films = result,
                metaData = new
                {
                    page,
                    limit,
                    count
                }
            };

            return count != 0 ? response : null;
        }

        public List<ViewCommentModel> getComments(List<CommentModel> list)
        {
            List<ViewCommentModel> viewList = new List<ViewCommentModel>();
            foreach (var i in list)
            {
                viewList.Add(new ViewCommentModel(i, getUserNameById(i.UserId)));
            }
            return viewList;
        }
        public string getUserNameById(string id)
        {
            return _unitOfWork.Users.Get(id).Email;
        }
    }
}
