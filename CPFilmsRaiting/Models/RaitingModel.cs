﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CPFilmsRaiting.Models
{
    public class RaitingModel
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string FilmId { get; set; }

        [Required]
        public int Value { get; set; }

        public ApplicationUser User { get; set; }
        public FilmModel Film { get; set; }
    }
}
