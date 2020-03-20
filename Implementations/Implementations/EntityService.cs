using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Implementations
{
    
    public class EntityService
    {
        TimeSeriesContext context;
        private AttributeService ats;

        public EntityService(TimeSeriesContext context) {
            this.context = context;
            ats = new AttributeService(context);
        }

        public void AddElement(ViewEntity viewEntity, int dbId)
        {
            try
            {
                context.Entities.Add(new Entity
                {
                    Name = viewEntity.Name,
                    DBId = dbId,
                    Attributes = new List<Models.Attribute>(),
                    TimeSeries = new List<TimeSeriesPointEntity>()
                });
                context.SaveChanges(); 
                foreach (var att in viewEntity.Attributes)
                {
                    ats.AddElement(att, context.Entities.FirstOrDefault(rec => rec.Name == viewEntity.Name && rec.DBId == dbId).Id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
