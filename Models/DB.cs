using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class DB
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Server { get; set; }
        public String Port { get; set; }
        public String Login { get; set; }
        public String Password { get; set; }
        public Boolean isMonitored { get; set; }
        public int Timer { get; set; }
        public DateTime LastCheck { get; set; }

        [ForeignKey("DBId")]
        public virtual List<Entity> Entities { get; set; }
    }
}
