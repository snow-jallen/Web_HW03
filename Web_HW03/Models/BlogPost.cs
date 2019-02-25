using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Web_HW03.Models
{
    public class BlogPost
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime Posted { get; set; }
        public List<PostTag> PostTags { get; set; }
        [NotMapped]
        public string TagsString { get; set; }
    }

    public class Tag
    {
        public int Id { get; set; }
        public string TagName { get; set; }
        public List<PostTag> PostTags { get; set; }
    }

    public class PostTag
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public BlogPost Post { get; set; }
        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
