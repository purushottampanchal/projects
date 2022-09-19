using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DB_Test.Models
{
    public class Post
    { 
        [Required]
        [Display(Name ="ID")]
        public int ID { get; set; }

        public string Name { get; set; }



    }
}
