using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM.Models
{
    public  class Employee
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        public string FullName { get; set; }

        [Required]
        [MaxLength(255)]
        public string Email { get; set; }

        public ICollection<TeamOfWorker> TeamsOfWorkers { get; set; } = new List<TeamOfWorker>();
    }
}
