using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ViewTimeSeriesPointEntity
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public int Value { get; set; }
        public int EntityId { get; set; }
    }
}
