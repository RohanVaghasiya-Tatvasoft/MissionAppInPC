using MissionApp.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissionApp.Entities.ViewModels
{
    public class StoryAddVM
    {
        public List<Mission> Missions { get; set; }
        public int MissionId { get; set; }
        public List<User> Users { get; set; }
        public List<Story> Stories { get; set; }
        public List<MissionApplication> MissionApplications { get; set; }
        public Story story { get; set; }
        public User user { get; set; }
        public string Result { get; set; }
        public List<StoryMedium> StoryMediums { get; set; }
    }
}
