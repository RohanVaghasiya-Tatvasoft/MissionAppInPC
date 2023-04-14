using MissionApp.DataAccess.GenericRepository.Interface;
using MissionApp.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MissionApp.DataAccess.Repository.Interface
{
    public interface ITimesheetRepository : IGenericRepository<Timesheet>
    {
        IEnumerable<Timesheet> GetTimeSheetData(Expression<Func<Timesheet, bool>> predicate);
    }
}
