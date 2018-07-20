using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleTask.DTO
{
    public class ArticleOutDTO
    {
        public long Id { set; get; }
        public string Title { set; get; }
        public string Text { set; get; }
    }
}