using Models;
using Service.Implementations;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class DBService
    {
        private TimeSeriesContext context;
        private DBRequests dBRequests;
        private EntityService es;

        public DBService(TimeSeriesContext context)
        {
            this.context = context;
            dBRequests = new DBRequests(context);
            es = new EntityService(context);
        }

        public bool AddElement(string name, string server, string port, string login, string password, int timer)
        {
            try
            {
                DB element = context.DBs.FirstOrDefault(rec => rec.Name == name && rec.Server == server);
                if (element != null)
                {
                    return false;
                }
                StopCreatingTimeSeriesPoints();
                ViewDB viewdb = dBRequests.RequestStruct(new ViewDB
                {
                    Name = name,
                    Server = server,
                    Port = port,
                    Login = login,
                    Password = password,
                    Entities = new List<ViewEntity>()
                });
                context.DBs.Add(new DB
                {
                    Name = name,
                    Server = server,
                    Port = port,
                    Login = login,
                    Password = password,
                    isMonitored = true,
                    LastCheck = DateTime.Now,
                    Timer = timer,
                    Entities = new List<Entity>()
                });
                context.SaveChanges();
                foreach (var en in viewdb.Entities)
                {
                    es.AddElement(en, context.DBs.FirstOrDefault(rec => rec.Name == name && rec.Server == server).Id);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ViewDB GetElement(int id)
        {
            DB element = context.DBs.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new ViewDB
                {
                    Id = element.Id,
                    Name = element.Name,
                    Password = element.Password,
                    Server = element.Server,
                    Port = element.Port,
                    Login = element.Login,
                    Timer = element.Timer,
                    isMonitored = element.isMonitored,
                    LastCheck = element.LastCheck,
                    Entities = context.Entities
                    .Where(rec => rec.DBId == element.Id)
                    .Select(rec => new ViewEntity
                    {
                        Id = rec.Id,
                        Name = rec.Name,
                        DBId = rec.DBId,
                        Attributes = context.Attributes
                    .Where(recA => recA.EntityId == rec.Id)
                    .Select(recA => new ViewAttribute
                    {
                        Id = recA.Id,
                        Name = recA.Name,
                        Type = recA.Type,
                        EntityId = recA.EntityId,
                        TimeSeriesPoints = context.Attribute_TimeSeriesPointAttributes
                    .Where(recATSP => recATSP.AttributeId == recA.Id)
                    .Select(recATSP => new ViewAttribute_TimeSeriesPointAttribute
                    {
                        Id = recATSP.Id,
                        AttributeId = recATSP.AttributeId,
                        TimeSeriesPointAttributeId = recATSP.TimeSeriesPointAttributeId,
                        Value = recATSP.Value
                    }).ToList()
                    }).ToList(),
                        TimeSeries = context.TimeSeriesPointEntities
                    .Where(recTSPE => recTSPE.EntityId == rec.Id)
                    .Select(recTSPE => new ViewTimeSeriesPointEntity
                    {
                        Id = recTSPE.Id,
                        EntityId = recTSPE.EntityId,
                        Time = recTSPE.Time,
                        Value = recTSPE.Value
                    }).ToList()
                    }).ToList()
                };
            }
            return null;
        }

        public List<ViewDB> GetList() {
            List<ViewDB> result = context.DBs
                    .Select(element => new ViewDB
                    {
                        Id = element.Id,
                        Name = element.Name,
                        Password = element.Password,
                        Server = element.Server,
                        Port = element.Port,
                        Login = element.Login,
                        Timer = element.Timer,
                        isMonitored = element.isMonitored,
                        LastCheck = element.LastCheck,
                        Entities = context.Entities
                    .Where(rec => rec.DBId == element.Id)
                    .Select(rec => new ViewEntity
                    {
                        Id = rec.Id,
                        Name = rec.Name,
                        DBId = rec.DBId,
                        Attributes = context.Attributes
                    .Where(recA => recA.EntityId == rec.Id)
                    .Select(recA => new ViewAttribute
                    {
                        Id = recA.Id,
                        Name = recA.Name,
                        Type = recA.Type,
                        EntityId = recA.EntityId,
                        TimeSeriesPoints = context.Attribute_TimeSeriesPointAttributes
                    .Where(recATSP => recATSP.AttributeId == recA.Id)
                    .Select(recATSP => new ViewAttribute_TimeSeriesPointAttribute
                    {
                        Id = recATSP.Id,
                        AttributeId = recATSP.AttributeId,
                        TimeSeriesPointAttributeId = recATSP.TimeSeriesPointAttributeId,
                        Value = recATSP.Value
                    }).ToList()
                    }).ToList(),
                        TimeSeries = context.TimeSeriesPointEntities
                    .Where(recTSPE => recTSPE.EntityId == rec.Id)
                    .Select(recTSPE => new ViewTimeSeriesPointEntity
                    {
                        Id = recTSPE.Id,
                        EntityId = recTSPE.EntityId,
                        Time = recTSPE.Time,
                        Value = recTSPE.Value
                    }).ToList()
                    }).ToList()
                    }).ToList();
            return result;
        }

        public void StopCreatingTimeSeriesPoints()
        {
            try
            {
                if (context.DBs.FirstOrDefault(rec => rec.isMonitored == true) != null)
                {
                    context.DBs.FirstOrDefault(rec => rec.isMonitored == true).isMonitored = false;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void RunCreatingTimeSeriesPoints(string name, string server, int timer)
        {
            try
            {
                context.DBs.FirstOrDefault(rec => rec.Name == name && rec.Server == server).isMonitored = true;
                context.DBs.FirstOrDefault(rec => rec.Name == name && rec.Server == server).Timer = timer;
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
