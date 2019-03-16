using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CPFilmsRaiting.Models
{
    public class ApplicationUser
    {
        [Key]
        public string Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(@"^([a-z0-9]){4,8}$", ErrorMessage = "Invalid password")]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }

        public List<CommentModel> Comments { get; set; }
        public List<RaitingModel> Raitings { get; set; }
    }
}
