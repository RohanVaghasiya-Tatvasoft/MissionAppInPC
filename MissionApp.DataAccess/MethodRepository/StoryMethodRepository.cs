using MissionApp.DataAccess.MethodRepository.Interface;
using MissionApp.Entities.Data;
using MissionApp.Entities.Models;
using MissionApp.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissionApp.DataAccess.MethodRepository
{
    public class StoryMethodRepository : IStoryMethodRepository
    {
        private readonly ApplicationDbContext _context;
        public StoryMethodRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<SavedStory> GetStory(int missionId, int userId)
        {
            var query = (from st in _context.Stories
                         join md in _context.StoryMedia
                         on st.StoryId equals md.StoryId into g
                         from md in g.DefaultIfEmpty()
                         where st.MissionId == missionId && st.UserId == userId && st.Status == "DRAFT"
                         orderby st.StoryId descending
                         select new SavedStory
                         {
                             StoryId = st.StoryId,
                             Title = st.Title,
                             Description = st.Description,
                             PublishedAt = st.PublishedAt,
                             Path = md.Path,
                             Type = md.Type
                         }).ToList();
            return query;
        }

        public User UserOfStory(int storyId)
        {
            User? FindingStoryCreator = _context.Stories.Where(s => s.StoryId == storyId).Select(s => s.User).FirstOrDefault();
            return FindingStoryCreator;
        }
    }
}
