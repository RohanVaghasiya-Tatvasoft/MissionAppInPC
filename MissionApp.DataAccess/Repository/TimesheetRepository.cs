using Microsoft.EntityFrameworkCore;
using MissionApp.DataAccess.GenericRepository;
using MissionApp.DataAccess.GenericRepository.Interface;
using MissionApp.DataAccess.Repository.Interface;
using MissionApp.Entities.Data;
using MissionApp.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MissionApp.DataAccess.Repository
{
    public class TimesheetRepository : GenericRepository<Timesheet>, ITimesheetRepository
    {
        public TimesheetRepository(ApplicationDbContext context) : base(context)
        {
        }

        public IEnumerable<Timesheet> GetTimeSheetData(Expression<Func<Timesheet, bool>> predicate)
        {
            IQueryable<Timesheet> query = dbSet;
            query = query.Where(predicate).Include(t => t.Mission);
            return query.ToList();
        }
    }
}
