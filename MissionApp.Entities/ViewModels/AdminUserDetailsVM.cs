using MissionApp.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissionApp.Entities.ViewModels
{
    public class AdminUserDetailsVM
    {
        public IEnumerable<User> UserLists { get; set; } = new List<User>();
    }
}
