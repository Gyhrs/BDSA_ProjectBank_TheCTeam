using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Infrastructure
{
    public abstract class User
    {
        [EmailAddress]
        [Key]
        public string Email { get; set; }
        
        [StringLength(50)]
        public string Name { get; set; }
        
        
    }
}