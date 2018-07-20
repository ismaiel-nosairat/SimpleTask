using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SimpleTask.Models
{
    public class Article
    {
        [Key]
        public long Id { set; get; }
        [Required]
        [StringLength(50)]
        public string Title { set; get; }
        [Required]
        [StringLength(500)]
        public string Text { set; get; }

        public long UserId { set; get; }

        public virtual User User { set; get; }
    }
}