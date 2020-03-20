using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ViewDB
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
        public virtual List<ViewEntity> Entities { get; set; }

        public override string ToString()
        {
            return $"Название: {Name} \t Сервер: {Server} \t Порт: {Port} \t Пользователь: {Login}";
        }
    }
}
