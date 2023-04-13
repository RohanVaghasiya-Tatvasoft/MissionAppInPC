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
        public UserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        //---------------------------------------------------------------- Other Views ---------------------------------------------------------------//
        public IActionResult PolicyPage()
        {
            return View();
        }

        public IActionResult Timesheet()
        {
            return View();
        }

//---------------------------------------------------------------------- User Profile Edit ----------------------------------------------------//
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

    }
}
