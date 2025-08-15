using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBoutiqueDataLayer.Entities
{
    public class Item
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int QuantityInStock { get; set; } = 0;
        public string Size { get; set; }
        public string Color { get; set; }
        public string ImageUrl{ get; set; }

        [ForeignKey("CategoryId")]
        public ICollection<Category> Categories { get; set; }
    }
}
