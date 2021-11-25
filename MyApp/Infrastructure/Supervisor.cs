using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Infrastructure
{
    public class Supervisor : StudyBankUser
    {
        public List<Project>? Projects { get; set; }
    }
}
