using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Entity
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public int DBId { get; set; }

        [ForeignKey("EntityId")]
        public virtual List<TimeSeriesPointEntity> TimeSeries { get; set; }

        [ForeignKey("EntityId")]
        public virtual List<Attribute> Attributes { get; set; }
    }
}
