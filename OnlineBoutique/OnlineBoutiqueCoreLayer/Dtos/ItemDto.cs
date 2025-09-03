using Microsoft.AspNetCore.Http;
using OnlineBoutiqueDataLayer.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBoutiqueCoreLayer.Dtos
{
    public class ItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int QuantityInStock { get; set; } = 0;
        public string Size { get; set; }
        public string Color { get; set; }
        public IFormFile Image { get; set; }

        public int CategoryId { get; set; }
    }
}
