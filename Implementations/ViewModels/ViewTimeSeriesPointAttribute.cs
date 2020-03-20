using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ViewTimeSeriesPointAttribute
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public virtual List<ViewAttribute_TimeSeriesPointAttribute> Values { get; set; }
    }
}
