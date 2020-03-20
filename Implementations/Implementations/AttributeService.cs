using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Implementations
{
    public class AttributeService
    {
        TimeSeriesContext context;

        public AttributeService(TimeSeriesContext context)
        {
            this.context = context;
        }

        public void AddElement(ViewAttribute viewAttribute, int entityId)
        {
            try
            {
                context.Attributes.Add(new Models.Attribute
                {
                    Name = viewAttribute.Name,
                    EntityId = entityId,
                    Type = viewAttribute.Type,
                    TimeSeriesPoints = new List<Attribute_TimeSeriesPointAttribute>()
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
