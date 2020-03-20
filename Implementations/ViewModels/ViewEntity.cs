using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ViewEntity
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public int DBId { get; set; }
        public virtual List<ViewTimeSeriesPointEntity> TimeSeries { get; set; }
        public virtual List<ViewAttribute> Attributes { get; set; }
    }
}
