using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Infrastructure
{
    public class Project
    {
        public int Id { get; set; }
        
        [StringLength(50)]
        public string Name { get; set; }
        public DateTime StartDate   { get; set; }
        public DateTime EndDate { get; set; }
        
        public String? Description { get; set; }
        public List<Student>? Students { get; set; }
        public List<Supervisor>? Supervisors { get; set;}
        public User CreatedBy { get; set; }

        public List<Tag>? Tags { get; set; }
        
    }
}