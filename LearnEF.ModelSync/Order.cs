using AO.Models;
using LearnEF.ModelSync.Conventions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearnEF.ModelSync
{
    public class Order : BaseTable
    {
        [Key]
        [References(typeof(Customer), CascadeDelete = false)]
        public int CustomerId { get; set; }

        [Key]
        [References(typeof(Product), CascadeDelete = false)]
        public int ProductId { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "money")]
        public decimal UnitPrice { get; set; }

        public decimal ExtPrice => Quantity * UnitPrice;
    }
}
