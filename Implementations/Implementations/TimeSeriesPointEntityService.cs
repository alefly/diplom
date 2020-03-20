using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class TimeSeriesPointEntityService
    {
        TimeSeriesContext context;

        public TimeSeriesPointEntityService(TimeSeriesContext context)
        {
            this.context = context;
        }

        public void AddElement(int value, int entityId)
        {
            try
            {
                context.TimeSeriesPointEntities.Add(new TimeSeriesPointEntity
                {
                    Value = value,
                    EntityId = entityId,
                    Time = DateTime.Now
                });
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
