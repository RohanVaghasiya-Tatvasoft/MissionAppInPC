using MissionApp.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissionApp.Entities.ViewModels
{
    public class VolunteerStoryDetailsVM
    {
        public Story? StoryDetails { get; set; }

        public User? UserOfStory { get; set; }

        public User? User { get; set; }
        public Mission Mission { get; set; }

        public IEnumerable<User> UserList { get; set; }

        public IEnumerable<StoryMedium> StoryMedia { get; set; }
        public User UserInfo { get; set; }
    }
}
