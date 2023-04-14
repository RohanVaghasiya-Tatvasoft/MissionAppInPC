using MissionApp.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissionApp.Entities.ViewModels
{
    public class TimesheetVM
    {
        public List<Mission> Missions { get; set; }

        public int MissionId { get; set; }

        public List<User> Users { get; set; }

        public List<Timesheet> Timesheets { get; set; }

        public List<MissionApplication> MissionApplicationForTime { get; set; }

        public List<MissionApplication> MissionApplicationForGoal { get; set; }

        public List<City> Cities { get; set; }
    }
}
