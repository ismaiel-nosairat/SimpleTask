using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SimpleTask.Models
{
    public class User
    {
        [Key]
        public long Id { set; get; }
        [Required]
        [StringLength(30)]
        public string UserName { set; get; }
        [Required]
        [StringLength(30)]
        public string Password { set; get; }
        [Required]
        [StringLength(30)]
        public string FullName { set; get; }
    }
}