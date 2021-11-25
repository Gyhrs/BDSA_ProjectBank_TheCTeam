using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Infrastructure
{
    public class Student : StudyBankUser
    {
        [StringLength(50)]
        public string Program { get; set; }

        public Project? Project { get; set; }
    }
}
