using LearnEF.Database.Conventions;
using System.ComponentModel.DataAnnotations;

namespace LearnEF.Database
{
    public class Customer : BaseTable
    {
        [Required]
        [MaxLength(100)]        
        public string? Name { get; set; }        

        public ICollection<Order> Orders { get; set; }        
    }
}