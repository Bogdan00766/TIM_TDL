﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TIM_TDL.Domain.Utils;

namespace TIM_TDL.Application.Dtos.Job
{
    public class CreateJobDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public DataTypes.JobStatus Status { get; set; }
        public int UserId { get; set; }
    }
}