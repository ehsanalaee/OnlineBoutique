using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBoutiqueDataLayer.Entities
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }

        [ForeignKey("Parent")]
        public int? ParentId { get; set; }
        public Category Parent { get; set; }

        public ICollection<Category> Children { get; set; }
        public ICollection<Item> Items { get; set; }
    }
}
