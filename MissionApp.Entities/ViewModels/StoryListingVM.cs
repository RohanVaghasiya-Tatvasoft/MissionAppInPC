using MissionApp.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissionApp.Entities.ViewModels
{
    public class StoryListingVM
    {
        public IEnumerable<Story> Stories { get; set; }

        public IEnumerable<Mission> Missions { get; set; }

        public IEnumerable<User> Users { get; set; }

        public IEnumerable<MissionTheme> MissionThemes { get; set; }

        public IEnumerable<MissionApplication> MissionApplications { get; set; }
        public User? UserInfo { get; set; }
    }
}
