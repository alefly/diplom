using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Attribute
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int EntityId { get; set; }

        [ForeignKey("AttributeId")]
        public virtual List<Attribute_TimeSeriesPointAttribute> TimeSeriesPoints { get; set; }

    }
}
