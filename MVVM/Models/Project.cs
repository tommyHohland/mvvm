using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MVVM.Models
{
    internal class Project
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        [Required]
        public DateTime DateStart { get; set; }

        [Required]
        public DateTime DateEnd { get; set; }

        [Required]
        public int Priority { get; set; }

        [ForeignKey(nameof(Employee))]
        public int ID_Manager { get; set; }
        public Employee Manager { get; set; }

        [ForeignKey(nameof(Models.Executor))]
        public int ID_executor { get; set; }
        public Executor Executor { get; set; }

        [ForeignKey(nameof(Models.Customer))]
        public int ID_customer { get; set; }
        public Customer Customer { get; set; }
        public ICollection<TeamOfWorker> TeamsOfWorkers { get; set; } = new List<TeamOfWorker>();
    }
}
