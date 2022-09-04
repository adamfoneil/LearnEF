using LearnEF.Database.Conventions;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearnEF.Database
{
    public class Order : BaseTable
    {        
        [ForeignKey(nameof(Customer))]
        public int CustomerId { get; set; }
        
        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "money")]
        public decimal UnitPrice { get; set; }
        
        public decimal ExtPrice => Quantity * UnitPrice;
    }
}
