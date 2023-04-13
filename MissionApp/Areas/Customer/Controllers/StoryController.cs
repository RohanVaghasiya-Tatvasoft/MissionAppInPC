using Microsoft.AspNetCore.Mvc;
using MissionApp.DataAccess.GenericRepository.Interface;
using MissionApp.DataAccess.MethodRepository.Interface;
using MissionApp.Entities.Models;
using MissionApp.Entities.ViewModels;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;

namespace MissionApp.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class StoryController : Controller  
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IStoryMethodRepository _storyMethodRepo;
        public StoryController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnviornment, IStoryMethodRepository storyMethodRepo)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnviornment;
            _storyMethodRepo = storyMethodRepo;
        }
//--------------------------------------------------------- Story Listing Page View ---------------------------------------------------------//
        public IActionResult StoryListingPage()
        {
            return View();
        }
//-------------------------------------------------------- Story Card Listing View ----------------------------------------------------------//

        public IActionResult StoryCardView(int pg = 1)
        {
            StoryListingVM storyListingVM = new StoryListingVM {
                Stories = _unitOfWork.Story.GetAll(),
                MissionThemes = _unitOfWork.MissionTheme.GetAll(),
                Users = _unitOfWork.User.GetAll(),
                Missions = _unitOfWork.Mission.GetAll()
            };

            List<Story> stories = (List<Story>)_unitOfWork.Story.GetAll();
            const int pageSize = 3;
            if (pg < 1)
            {
                pg = 1;
            }
            int recsCount = stories.Count;

            var pager = new PaginationVM(recsCount, pg, pageSize);

            int recSkip = (pg - 1) * pageSize;

            stories = stories.Skip(recSkip).Take(pager.PageSize).ToList();

            this.ViewBag.userPager = pager;

            storyListingVM.Stories = stories;

            ViewBag.missionCount = recsCount;

            return PartialView("_StoryCards" , storyListingVM);
        }
//------------------------------------------------------------------- Story Add View Page ------------------------------------------------------//
        public IActionResult StoryAddPage()
        {
            User user = GetThisUser();
            StoryAddVM storyAddVM = new();
            storyAddVM.Missions = (List<Mission>)_unitOfWork.Mission.GetAll();

            List<MissionApplication> draftMissionApplicationList = new();
            List<MissionApplication> approvedMissionApplicationList = _unitOfWork.MissionApplication.GetAccToFilter(u =>u.UserId == user.UserId && u.ApprovalStatus == "APPROVE");

            foreach(var mission in approvedMissionApplicationList)
            {
                var story = _unitOfWork.Story.GetFirstOrDefault(u=> u.UserId == user.UserId && u.MissionId == mission.MissionId);
                if(story != null && (story.Status == "DRAFT" || story.Status == "DECLINED"))
                {
                    draftMissionApplicationList.Add(mission);
                }
                else if(story == null)
                {
                    draftMissionApplicationList.Add(mission);
                }
            }

            try
            {
                storyAddVM.MissionApplications = draftMissionApplicationList;
                storyAddVM.story = _unitOfWork.Story.GetFirstOrDefault(u => u.UserId == user.UserId && u.Status == "DRAFT");
                storyAddVM.StoryMediums = _unitOfWork.StoryMedia.GetAccToFilter(u => u.StoryId == storyAddVM.story.StoryId);
            }
            catch
            {

            }

            return View(storyAddVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult StoryAddPage(int missionId, string storyTitle, string storyDate, string storyDescription, string videoURL, List<IFormFile> files, string[] preloaded)
        {
            var user = GetThisUser();
            var userId = user.UserId;

            StoryAddVM storyAddVM = new();

            var AlreadyPostedStory = _unitOfWork.Story.GetFirstOrDefault(u=> u.MissionId == missionId && u.UserId == userId && u.Status == "DRAFT");
            storyAddVM.Result = "false";

            if(AlreadyPostedStory == null)
            {
                Story story = new Story
                {
                    Title = storyTitle,
                    Description = storyDescription,
                    MissionId = missionId,
                    UserId = userId,
                    Status = "DRAFT",
                    PublishedAt = DateTime.Parse(storyDate)
                };
                _unitOfWork.Story.Add(story);
                _unitOfWork.Save();
            }

            else if(AlreadyPostedStory != null)
            {
                AlreadyPostedStory.Title = storyTitle;
                AlreadyPostedStory.Description = storyDescription;
                AlreadyPostedStory.PublishedAt = DateTime.Parse(storyDate);

                _unitOfWork.Save();
            }

            var thisStory = _unitOfWork.Story.GetFirstOrDefault(u=> u.UserId == user.UserId && u.MissionId == missionId);
            var media = _unitOfWork.StoryMedia.GetAccToFilter(u => u.StoryId == thisStory.StoryId);

            if (videoURL != null)
            {
                foreach(var videoUrl in media)
                {
                    if (videoUrl.Type == "VIDEO")
                    {
                        if (videoUrl != null)
                        {
                            _unitOfWork.StoryMedia.Remove(videoUrl);
                        }
                    }
                }
                StoryMedium storyMedia = new()
                {
                    StoryId = thisStory.StoryId,
                    Type = "VIDEO",
                    Path = videoURL
                };
                _unitOfWork.StoryMedia.Add(storyMedia);
                _unitOfWork.Save();
            }
            else
            {
                foreach (var videoUrl in media)
                {
                    if(videoUrl.Type == "VIDEO")
                    {
                        if(videoUrl != null)
                        {
                            _unitOfWork.StoryMedia.Remove(videoUrl);
                        }
                    }
                }
                _unitOfWork.Save();
            }

            foreach(var img in media)
            {
                if (img.Type != "VIDEO")
                {
                    if (preloaded.Length < 1)
                    {
                        string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/StoryImages/", img.Path);

                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }

                        _unitOfWork.StoryMedia.Remove(img);
                        _unitOfWork.Save();
                    }
                    else
                    {
                        bool flag = false;

                        for (int i = 0; i < preloaded.Length; i++)
                        {
                            var imgName = preloaded[i].Substring(13);

                            if (imgName.Equals(img.Path))
                            {
                                flag = true;
                            }
                        }
                        if (!flag)
                        {
                            string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/StoryImages/", img.Path);

                            if (System.IO.File.Exists(imagePath))
                            {
                                System.IO.File.Delete(imagePath);
                            }

                            _unitOfWork.StoryMedia.Remove(img);
                            _unitOfWork.Save();
                        }

                    }
                }
            }

            var data = _unitOfWork.Story.GetFirstOrDefault(m => m.UserId == user.UserId && m.MissionId == missionId);
            foreach (IFormFile img in files)
            {
                string imgExt = Path.GetExtension(img.FileName);
                if (imgExt == ".jpg" || imgExt == ".png" || imgExt == ".jpeg")
                {
                    string ImageName = Guid.NewGuid().ToString() + Path.GetExtension(img.FileName);
                    var imgSaveTo = Path.Combine(_webHostEnvironment.WebRootPath, "StoryImages", ImageName);
                    /*var stream = new FileStream(imgSaveTo, FileMode.Create);
                    img.CopyTo(stream);*/
                    using (FileStream stream = new(imgSaveTo, FileMode.Create))
                    {
                        img.CopyTo(stream);
                    }

                    StoryMedium storyMedium = new();
                    storyMedium.StoryId = data.StoryId;
                    storyMedium.Type = imgExt;
                    storyMedium.Path = ImageName;

                    _unitOfWork.StoryMedia.Add(storyMedium);
                    _unitOfWork.Save();
                }
            }

            StoryAddVM model = new();

            List<MissionApplication> draftMissionApplicationList = new();
            List<MissionApplication> approvedMissionApplicationList = _unitOfWork.MissionApplication.GetAccToFilter(u => u.UserId == user.UserId && u.ApprovalStatus == "APPROVE");

            foreach (var mission in approvedMissionApplicationList)
            {
                var story = _unitOfWork.Story.GetFirstOrDefault(u => u.UserId == user.UserId && u.MissionId == mission.MissionId);
                if (story != null && (story.Status == "DRAFT" || story.Status == "DECLINED"))
                {
                    draftMissionApplicationList.Add(mission);
                }
                else if (story == null)
                {
                    draftMissionApplicationList.Add(mission);
                }
            }

            model.MissionApplications = draftMissionApplicationList;
            model.Missions = (List<Mission>)_unitOfWork.Mission.GetAll();

            return View("StoryAddPage", model);
        }

        [HttpPost]
        public IActionResult GetMissionDetails(int missionId)
        {
            User user = GetThisUser();
            var query = _storyMethodRepo.GetStory(missionId, user.UserId);

            return Json(query);
        }


        [HttpPost]
        public IActionResult SubmitStory(int missionId)
        {
            User user = GetThisUser();
            Story storyOfUser = _unitOfWork.Story.GetFirstOrDefault(m => m.MissionId == missionId && m.UserId == user.UserId && m.Status == "DRAFT");
            if (storyOfUser != null)
            {
                storyOfUser.Status = "PENDING";

                _unitOfWork.Story.Update(storyOfUser);
                _unitOfWork.Save();
            }
            return RedirectToAction("StoryListingPage");
        }

        public User GetThisUser()
        {
            var identity = User.Identity as ClaimsIdentity;
            var email = identity?.FindFirst(ClaimTypes.Email)?.Value;

            var user = _unitOfWork.User.GetFirstOrDefault(m => m.Email == email);
            return user;
        }

//---------------------------------------------------------- Volunteer Story Details Page -----------------------------------------------------------//
        public IActionResult VolunteerStoryDetails(int storyId, long views)
        {
            var storyForView = _unitOfWork.Story.GetFirstOrDefault(m => m.StoryId == storyId);

            if (storyForView.Views < views)
            {
                storyForView.Views = views;
                _unitOfWork.Story.Update(storyForView);
                _unitOfWork.Save();
            }


            User user = GetThisUser();
            List<User> ListOfUsers = (List<User>)_unitOfWork.User.GetAll();
            User? FindingStoryCreator = _storyMethodRepo.UserOfStory(storyId);

            VolunteerStoryDetailsVM storyDetails = new()
            {
                User = user,
                UserList = ListOfUsers,
                UserOfStory = FindingStoryCreator,
                StoryDetails = _unitOfWork.Story.GetFirstOrDefault(m => m.StoryId == storyId),
                StoryMedia = _unitOfWork.StoryMedia.GetAccToFilter(m => m.StoryId == storyId)
            };

            return View(storyDetails);
        }

        public void RecommandToCoworker(int[]? userIds, int sId, long totalViews)
        {
            var thisUser = GetThisUser();
            Story thisStory = _unitOfWork.Story.GetFirstOrDefault(m => m.StoryId == sId);

            if (userIds != null)
            {
                foreach (var id in userIds)
                {
                    var user = _unitOfWork.User.GetFirstOrDefault(m => m.UserId == id);
                    StoryInvite obj = new StoryInvite{
                        StoryId = sId,
                        FromUserId = user.UserId,
                        ToUserId = thisUser.UserId
                    };
                    _unitOfWork.StoryInvite.Add(obj);
                    _unitOfWork.Save();

                    var inviteLink = Url.Action("VolunteerStoryDetails", "Story", new { storyId = sId, views = totalViews }, Request.Scheme);
                    var fromAddress = new MailAddress("job.rohanvaghasiya@gmail.com", "Mission App");
                    var toAddress = new MailAddress(user.Email);
                    var subject = "Story Invite";
                    var body = $"Hello <b>{@user.FirstName} {@user.LastName}</b> ,<br /><br /> Your Colleague <b>{thisUser.FirstName} {thisUser.LastName}</b>sent you an intrested story <b><i>{thisStory.Title}</i></b><br /><br />Click the following link to read the story,<br /><br /><a href='{inviteLink}'>{inviteLink}</a><br /><br />Regards,<br/>";
                    var msg = new MailMessage(fromAddress, toAddress)
                    {
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true
                    };

                    var smtpClient = new SmtpClient("smtp.gmail.com", 587)
                    {
                        Credentials = new NetworkCredential("job.rohanvaghasiya@gmail.com", "jpyutohvpsvbmhsf"),
                        EnableSsl = true,
                    };
                    smtpClient.Send(msg);
                }
            }
        }

    }
}
