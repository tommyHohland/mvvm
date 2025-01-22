using System.ComponentModel.DataAnnotations;

namespace MVVM.Models
{
    public  class Customer
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        public string CompanyName { get; set; }

        public ICollection<Project> Projects { get; set; } = new List<Project>();
    }
}
