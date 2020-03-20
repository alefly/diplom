using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class TimeSeriesPointAttribute
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }

        [ForeignKey("TimeSeriesPointAttributeId")]
        public virtual List<Attribute_TimeSeriesPointAttribute> Values { get; set; }
    }
}
