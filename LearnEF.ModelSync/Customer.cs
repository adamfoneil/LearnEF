using LearnEF.ModelSync.Conventions;
using System.ComponentModel.DataAnnotations;

namespace LearnEF.ModelSync
{
    public class Customer : BaseTable
    {
        [Key]        
        [MaxLength(100)]        
        public string Name { get; set; }        
    }
}