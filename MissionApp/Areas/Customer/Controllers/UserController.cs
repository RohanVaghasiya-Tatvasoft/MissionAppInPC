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
            UserProfileVM userProfile = new()
            {
                UserInfo = GetThisUser()
            };
            return View(userProfile);
        }
        #endregion

        //---------------------------------------------------------------- TimeSheet ---------------------------------------------------------------//
        #region TimeSheet--->
        public IActionResult Timesheet()
        {
            User user = GetThisUser();

            IEnumerable<MissionApplication> draftMissAppListForTime = _unitOfWork.MissionApplication.GetAccToFilter(m => m.UserId == user.UserId && m.ApprovalStatus == "APPROVE" && m.Mission.MissionType == "TIME");
            IEnumerable<MissionApplication> draftMissAppListForGoal = _unitOfWork.MissionApplication.GetAccToFilter(m => m.UserId == user.UserId && m.ApprovalStatus == "APPROVE" && m.Mission.MissionType == "GOAL");
            IEnumerable<Timesheet> dataOfTimeBasedMission = _unitOfWork.Timesheet.GetTimeSheetData(timeData => timeData.Mission.MissionType == "Time" && timeData.DeletedAt == null);
            IEnumerable<Timesheet> dataOfGoalBasedMission = _unitOfWork.Timesheet.GetTimeSheetData(goalData => goalData.Mission.MissionType == "Goal" && goalData.DeletedAt == null);

            TimesheetVM obj = new();
            obj.UserInfo = user;
            obj.MissionApplicationForTime = draftMissAppListForTime;
            obj.MissionApplicationForGoal = draftMissAppListForGoal;
            obj.TimesheetsForTime = dataOfTimeBasedMission;
            obj.TimesheetsForGoal = dataOfGoalBasedMission;

            obj.Missions = _unitOfWork.Mission.GetAll();
            obj.Cities = _unitOfWork.City.GetAll();

            return View(obj);
        }

        [HttpPost]
        public IActionResult ChangeInTimesheet(TimesheetVM obj)
        {
            User user = GetThisUser();
            var hours = obj.Hours;
            var minutes = obj.Minutes;
            TimeSpan time = new(hours, minutes, 0);

            if (obj.TimeSheetId == 0)
            {
                Timesheet data = new()
                {
                    UserId = user.UserId,
                    MissionId = obj.MissionId,
                    Time = time,
                    Action = obj.Action,
                    DateVolunteered = obj.DateVolunteered,
                    Notes = obj.Notes,
                    Status = "SUBMIT_FOR_APPROVAL",
                };
                _unitOfWork.Timesheet.Add(data);
                _unitOfWork.Save();
                TempData["success"] = "Data added successfully!";
                return RedirectToAction("Timesheet");
            }
            else
            {
                Timesheet updatedData = _unitOfWork.Timesheet.GetFirstOrDefault(timeSheetData => timeSheetData.TimesheetId == obj.TimeSheetId);
                if (updatedData != null)
                {
                    updatedData.MissionId = obj.MissionId;
                    updatedData.Action = obj.Action;
                    updatedData.Time = time;
                    updatedData.Notes = obj.Notes;
                    updatedData.DateVolunteered = obj.DateVolunteered;
                    updatedData.UpdatedAt = DateTime.Now;

                    _unitOfWork.Timesheet.Update(updatedData);
                    _unitOfWork.Save();

                    TempData["success"] = "Data updated successfully!";
                    return RedirectToAction("VolTimeSheet");
                }
            }

            TempData["error"] = "Something went wrong!";
            return RedirectToAction("Timesheet");
        }


        public IActionResult GetTimeSheetData(long timesheetId)
        {
            Timesheet timesheetData = _unitOfWork.Timesheet.GetFirstOrDefault(m => m.TimesheetId == timesheetId);
            return Json(timesheetData);
        }

        [HttpPost]
        public IActionResult DeleteTimeSheetData(long timesheetId)
        {
            if (timesheetId > 0)
            {
                Timesheet deletingData = _unitOfWork.Timesheet.GetFirstOrDefault(m => m.TimesheetId == timesheetId);
                deletingData.DeletedAt = DateTime.Now;
                _unitOfWork.Timesheet.Update(deletingData);
                _unitOfWork.Save();

                return RedirectToAction("VolunteerTimesheet");
            }

            TempData["error"] = "Opps! something went wrong";
            return RedirectToAction("VolunteerTimesheet");
        }

        [HttpGet]
        public IActionResult getDate(int missionId)
        {
            var mission = _unitOfWork.Mission.GetAccToFilter(m => m.MissionId == missionId);
            return Json(mission);
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
                UserSkillList = userSkills,
                UserInfo = user,
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

            string imgExt = Path.GetExtension(avatar.FileName);
            if (imgExt == ".jpg" || imgExt == ".png" || imgExt == ".jpeg")
            {
                if (user.Avatar != null)
                {
                    string ImageName = user.UserId + Path.GetFileName(avatar.FileName);
                    var imgSaveTo = Path.Combine(_webHostEnvironment.WebRootPath, "/Avatars/", ImageName);
                    string finalPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/" + imgSaveTo);

                    if (!imgSaveTo.Equals(user.Avatar))
                    {
                        string alrExists = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/" + user.Avatar);
                        if (System.IO.File.Exists(alrExists))
                        {
                            System.IO.File.Delete(alrExists);
                        }

                        using (FileStream stream = new(finalPath, FileMode.Create))
                        {
                            avatar.CopyTo(stream);
                        }
                        user.Avatar = imgSaveTo;
                        user.UpdatedAt = DateTime.Now;
                        _unitOfWork.User.Update(user);
                    }
                }
                else
                {
                    string ImageName = user.UserId + Path.GetFileName(avatar.FileName);
                    var imgSaveTo = Path.Combine(_webHostEnvironment.WebRootPath, "/Avatars/", ImageName);
                    string finalPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/" + imgSaveTo);

                    using (FileStream stream = new(finalPath, FileMode.Create))
                    {
                        avatar.CopyTo(stream);
                    }
                    user.Avatar = imgSaveTo;
                    user.UpdatedAt = DateTime.Now;
                    _unitOfWork.User.Update(user);
                }
            }
            _unitOfWork.Save();
            return RedirectToAction(nameof(UserProfile));
        }

        [HttpPost]
        public IActionResult ChangePassword(UserProfileVM obj)
        {
            User user = GetThisUser();

            if (user != null && obj.OldPassword != null && obj.NewPassword != null)
            {
                if (user.Password == obj.OldPassword)
                {
                    user.Password = obj.NewPassword;
                    user.UpdatedAt = DateTime.Now;
                    _unitOfWork.User.Update(user);
                    _unitOfWork.Save();

                    return Json(1);
                }
            }
            return Json(0);
        }

        public JsonResult FilterCity(int countryId)
        {
            User user = GetThisUser();
            var cityId = user.CityId;
            IEnumerable<City> cityList = _unitOfWork.City.GetAccToFilter(m => m.CountryId == countryId);
            return new JsonResult(new { CityId = cityId, Cities = cityList });

        }
    }
}
