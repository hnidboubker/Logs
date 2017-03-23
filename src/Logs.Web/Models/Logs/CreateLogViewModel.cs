﻿using System.ComponentModel.DataAnnotations;

namespace Logs.Web.Models.Logs
{
    public class CreateLogViewModel
    {
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Description")]
        [MinLength(10)]
        public string Description { get; set; }
    }
}