using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ViewAttribute
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int EntityId { get; set; }
        public virtual List<ViewAttribute_TimeSeriesPointAttribute> TimeSeriesPoints { get; set; }
    }
}
