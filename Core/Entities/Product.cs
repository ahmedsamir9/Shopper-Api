using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public int NumberInStock { get; set; }
        public float Price { get; set; }
        public float Rate { get; set; }
        public int categoryID { get; set; } 
        //[NotMapped]
        //[Display(Name = "Upload Image")]
        //public IFormFile ImageFile { get; set; }
        public Category category { get; set; }
    }
}
