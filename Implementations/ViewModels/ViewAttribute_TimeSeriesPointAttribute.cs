using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ViewAttribute_TimeSeriesPointAttribute
    {
        public int Id { get; set; }
        public int AttributeId { get; set; }
        public int TimeSeriesPointAttributeId { get; set; }
        public int Value { get; set; }
    }
}
