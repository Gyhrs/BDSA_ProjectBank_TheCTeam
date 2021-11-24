using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Infrastructure
{
    public class Tag
    {
        [Key]
        public string Name { get; set; }
        public List<Project> Projects { get; set; }
    }
}
