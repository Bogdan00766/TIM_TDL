using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TIM_TDL.Domain.Utils;

namespace TIM_TDL.Domain.Models
{
    public class Job : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime DueDate { get; set; }
        public DataTypes.JobStatus Status { get; set; }
        public User Owner { get; set; }
    }
}
