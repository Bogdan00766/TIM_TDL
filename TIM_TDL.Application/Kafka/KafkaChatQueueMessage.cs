using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIM_TDL.Application.Kafka
{
    public class KafkaChatQueueMessage
    {
        public int ConnectorId { get; set; }
        public string Question { get; set; }
    }
}
