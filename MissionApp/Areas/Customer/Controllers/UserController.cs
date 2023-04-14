using Microsoft.AspNetCore.Mvc;
using MissionApp.DataAccess.GenericRepository.Interface;
using MissionApp.Entities.Data;
using MissionApp.Entities.Models;
using MissionApp.Entities.ViewModels;
using System.Linq;
using System.Security.Claims;

namespace MissionApp.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public UserController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        //---------------------------------------------------------------- Other Views ---------------------------------------------------------------//
        #region Policy Page--->
        public IActionResult PolicyPage()
        {
            return View();
        }
        #endregion

        //---------------------------------------------------------------- TimeSheet ---------------------------------------------------------------//
        #region TimeSheet--->
        public IActionResult Timesheet()
        {
            User user = GetThisUser();
            TimesheetVM timesheetVM = new();

            timesheetVM.Missions = (List<Mission>)_unitOfWork.Mission.GetAll();
            timesheetVM.Cities = (List<City>)_unitOfWork.City.GetAll();
            timesheetVM.Timesheets = (List<Timesheet>)_unitOfWork.Timesheet.GetAll();

            List<MissionApplication> draftMissAppListForTime = _unitOfWork.MissionApplication.GetAccToFilter(m => m.UserId == user.UserId && m.ApprovalStatus == "APPROVE" && m.Mission.MissionType == "TIME");
            List<MissionApplication> draftMissAppListForGoal = _unitOfWork.MissionApplication.GetAccToFilter(m => m.UserId == user.UserId && m.ApprovalStatus == "APPROVE" && m.Mission.MissionType == "GOAL");
            IEnumerable<Timesheet> timeBasedData = _unitOfWork.Timesheet.GetTimeSheetData(timeData => timeData.Mission.MissionType == "Time" && timeData.DeletedAt == null);
            IEnumerable<Timesheet> goalBasedData = _unitOfWork.Timesheet.GetTimeSheetData(goalData => goalData.Mission.MissionType == "Goal" && goalData.DeletedAt == null);
            try
            {
                timesheetVM.MissionApplicationForTime = draftMissAppListForTime;
                timesheetVM.MissionApplicationForGoal = draftMissAppListForGoal;
            }
            catch { }

            return View(timesheetVM);
        }
        #endregion

        //---------------------------------------------------------------- Contact Us ---------------------------------------------------------------//
        #region Contact Us --->
        [HttpGet]
        public UserVM ContactUs(UserVM userView)
        {
            userView.UserInfo = GetThisUser();
            return userView;
        }

        [HttpPost]
        public void ContactUs(string subject, string message)
        {
            User user = GetThisUser();
            ContactUs contactUs = new()
            {
                UserId = user.UserId,
                Subject = subject,
                Message = message
            };

            _unitOfWork.ContactUs.Add(contactUs);
            _unitOfWork.Save();
        }
        #endregion

        //------------------------------------------------------------- User Profile Edit -----------------------------------------------------------//
        public IActionResult UserProfile()
        {
            User user = GetThisUser();
            IEnumerable<Country> CountryList = _unitOfWork.Country.GetAll();
            IEnumerable<City> CitiesList = _unitOfWork.City.GetAll();
            IEnumerable<Skill> SkillsList = _unitOfWork.Skill.GetAll();
            List<UserSkill> userSkills = _unitOfWork.UserSkill.GetAccToFilter(userSkills => userSkills.UserId == user.UserId);

            UserProfileVM userProfileVM = new()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                WhyIVolunteer = user.WhyIVolunteer,
                EmployeeId = user.EmployeeId,
                Department = user.Department,
                Avatar = user.Avatar,
                CityId = user.CityId,
                CountryId = user.CountryId,
                ProfileText = user.ProfileText,
                LinkedInUrl = user.LinkedInUrl,
                Title = user.Title,
                Countries = CountryList,
                Cities = CitiesList,
                SkillsList = SkillsList,
                UserSkillList = userSkills
            };

            return View(userProfileVM);
        }

        // ----------------------------------------------------------------- User Profile Post Method -------------------------------------------------//
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UserProfile(UserProfileVM userProfileVM, List<int> UpdatedSkills) 
        {
            User user = GetThisUser();
            var IdOfUserSkills = _unitOfWork.UserSkill.GetAccToFilter(userSkill => userSkill.UserId == user.UserId).Select(u => u.SkillId);

            if (user != null) 
            {
                user.FirstName = userProfileVM.FirstName;
                user.LastName = userProfileVM.LastName;
                user.LinkedInUrl = userProfileVM.LinkedInUrl;
                user.CityId = userProfileVM.CityId;
                user.CountryId = userProfileVM.CountryId;
                user.ProfileText = userProfileVM.ProfileText;
                user.EmployeeId = userProfileVM.EmployeeId;
                user.Department = userProfileVM.Department;
                user.Title = userProfileVM.Title;
                user.WhyIVolunteer = userProfileVM.WhyIVolunteer;
                user.UpdatedAt = DateTime.Now;

                _unitOfWork.User.Update(user);

                if (UpdatedSkills.Any())
                {
                    var AddSkills = UpdatedSkills.Except(IdOfUserSkills);
                    foreach (var skillId in AddSkills)
                    {
                        UserSkill addUserSkills = new()
                        {
                            UserId = user.UserId,
                            SkillId = skillId,
                        };
                        _unitOfWork.UserSkill.Add(addUserSkills);
                    }

                    var DeleteSkills = IdOfUserSkills.Except(UpdatedSkills);
                    foreach (var skillid in DeleteSkills)
                    {
                        UserSkill deleteUserSkill = _unitOfWork.UserSkill.GetFirstOrDefault(userSkill => userSkill.SkillId == skillid);
                        _unitOfWork.UserSkill.Remove(deleteUserSkill);
                    }
                }

                _unitOfWork.Save();
            }
            return RedirectToAction(nameof(UserProfile));
        }












        public User GetThisUser()
        {
            var identity = User.Identity as ClaimsIdentity;
            var email = identity?.FindFirst(ClaimTypes.Email)?.Value;

            var user = _unitOfWork.User.GetFirstOrDefault(m => m.Email == email);
            return user;
        }

        [HttpPost]
        public IActionResult ChangeAvatar(IFormFile avatar)
        {
            User user = GetThisUser();

            if (user.Avatar == null)
            {
                string imgExt = Path.GetExtension(avatar.Name);
                if (imgExt == ".jpg" || imgExt == ".png" || imgExt == ".jpeg")
                {
                    string ImageName = user.UserId + Path.GetExtension(avatar.Name);
                    var imgSaveTo = Path.Combine(_webHostEnvironment.WebRootPath, "StoryImages", ImageName);
                    /*var stream = new FileStream(imgSaveTo, FileMode.Create);
                    avatar.CopyTo(stream);*/
                    using (FileStream stream = new(imgSaveTo, FileMode.Create))
                    {
                        avatar.CopyTo(stream);
                    }

                    user.Avatar = ImageName;

                    _unitOfWork.User.Add(user);
                }
            }
            else
            {
                if (user.UserId + Path.GetExtension(avatar.Name) != user.Avatar)
                {
                    string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/StoryImages/", user.Avatar);

                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }

                    string ImageName = user.UserId + Path.GetExtension(avatar.Name);
                    var imgSaveTo = Path.Combine(_webHostEnvironment.WebRootPath, "StoryImages", ImageName);
                    /*var stream = new FileStream(imgSaveTo, FileMode.Create);
                    avatar.CopyTo(stream);*/
                    using (FileStream stream = new(imgSaveTo, FileMode.Create))
                    {
                        avatar.CopyTo(stream);
                    }

                    user.Avatar = ImageName;

                    _unitOfWork.User.Update(user);
                }

            }
            _unitOfWork.Save();
            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(string OldPassword, string NewPassword)
        {
            User user = GetThisUser();

            if (user != null && OldPassword != null && NewPassword != null)
            {
                if (user.Password == OldPassword)
                {
                    user.Password = NewPassword;
                    user.UpdatedAt = DateTime.Now;
                    _unitOfWork.User.Update(user);
                    _unitOfWork.Save();

                    return Json(1);
                }
            }
            return Json(0);
        }


    }
}
