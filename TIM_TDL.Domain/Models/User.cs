using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIM_TDL.Domain.Models
{
    public class User : Entity
    {
        public string Email { get; set; }
        public byte[] Password { get; set; }
        public List<Job> Jobs { get; set; }
    }
}
