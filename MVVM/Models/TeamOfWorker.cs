using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MVVM.Models
{
    public  class TeamOfWorker
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey(nameof(Project))]
        public int ID_Project { get; set; }
        public Project Project { get; set; }

        [ForeignKey(nameof(Models.Employee))]
        public int ID_Employee { get; set; }
        public Employee Employee { get; set; }
    }
}
