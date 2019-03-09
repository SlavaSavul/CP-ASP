using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CPFilmsRaiting.Models
{
    public class ApplicationUser
    {
        public string Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[0-9])\S{1,16}$", ErrorMessage = "Invalid password")]
        public string Password { get; set; }

        public string Role { get; set; }

        public List<CommentModel> Comments { get; set; }
        public List<RaitingModel> Raitings { get; set; }
    }
}
