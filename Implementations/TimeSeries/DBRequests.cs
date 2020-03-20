using Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class DBRequests
    {
        TimeSeriesContext context;
        TimeSeriesPointEntityService tspeService;

        public DBRequests(TimeSeriesContext context) {
            this.context = context;
            tspeService = new TimeSeriesPointEntityService(context);
        }
        public ViewDB RequestStruct(ViewDB viewdb)
        {
            try
            {
                string connString = $"Server={viewdb.Server};Port={viewdb.Port};User Id={viewdb.Login};Password={viewdb.Password};Database={viewdb.Name};";
                using var conn = new NpgsqlConnection(connString);
                conn.Open();
                using (var cmd = new NpgsqlCommand("select table_name, column_name, data_type from information_schema.columns where table_schema = 'public'", conn))
                using (var reader = cmd.ExecuteReader())
                    while(reader.Read())
                    {
                        string entity = reader.GetString(0), attribute = reader.GetString(1), type = reader.GetString(2);
                        if (viewdb.Entities.FirstOrDefault(rec => rec.Name == entity) == null) {
                            viewdb.Entities.Add(new ViewEntity { Name = entity, Attributes = new List<ViewAttribute>() });
                        }
                        viewdb.Entities.FirstOrDefault(rec => rec.Name == entity).Attributes.Add(new ViewAttribute
                        {
                            Name = attribute,
                            Type = type
                        });
                    }
                
                conn.Close();
                return viewdb;
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        public void RequestsCreateTSPEntities(ViewDB viewdb)
        {
            try
            {
                string connString = $"Server={viewdb.Server};Port={viewdb.Port};User Id={viewdb.Login};Password={viewdb.Password};Database={viewdb.Name};";
                using var conn = new NpgsqlConnection(connString);
                conn.Open();
                ViewEntity prev = new ViewEntity { Name = "" };
                int valueTSPE;
                foreach (var entity in viewdb.Entities)
                {
                    valueTSPE = 0;
                    using (var cmd = new NpgsqlCommand($"select id from {entity.Name}", conn))
                    using (var reader = cmd.ExecuteReader())
                        while (reader.Read())
                        {
                            valueTSPE++;
                        }
                    tspeService.AddElement(valueTSPE, entity.Id);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
