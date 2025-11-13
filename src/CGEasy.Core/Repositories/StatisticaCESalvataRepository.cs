using CGEasy.Core.Data;
using CGEasy.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace CGEasy.Core.Repositories
{
    public class StatisticaCESalvataRepository
    {
        private readonly LiteDbContext _context;

        public StatisticaCESalvataRepository(LiteDbContext context)
        {
            _context = context;
        }

        public List<StatisticaCESalvata> GetAll()
        {
            return _context.StatisticheCESalvate.FindAll().OrderByDescending(s => s.DataCreazione).ToList();
        }

        public StatisticaCESalvata? GetById(int id)
        {
            return _context.StatisticheCESalvate.FindById(id);
        }

        public void Insert(StatisticaCESalvata statistica)
        {
            _context.StatisticheCESalvate.Insert(statistica);
        }

        public bool Update(StatisticaCESalvata statistica)
        {
            return _context.StatisticheCESalvate.Update(statistica);
        }

        public bool Delete(int id)
        {
            return _context.StatisticheCESalvate.Delete(id);
        }
    }
}

