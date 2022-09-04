using LearnEF.Database.Conventions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearnEF.Database
{
    public class Product : BaseTable
    {
        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }

        [MaxLength(255)]
        public string? Description { get; set; }

        [Column(TypeName = "money")]
        public decimal UnitCost { get; set; }

        [Column(TypeName = "money")]
        public decimal UnitPrice { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
