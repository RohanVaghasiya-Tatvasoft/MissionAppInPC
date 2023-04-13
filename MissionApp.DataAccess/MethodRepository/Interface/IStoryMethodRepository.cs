using MissionApp.Entities.Models;
using MissionApp.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissionApp.DataAccess.MethodRepository.Interface
{
    public interface IStoryMethodRepository
    {
        public User UserOfStory(int storyId);
        public List<SavedStory> GetStory(int missionId, int userId);
    }
}
