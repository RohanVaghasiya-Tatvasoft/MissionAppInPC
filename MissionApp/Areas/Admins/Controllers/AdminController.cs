using Microsoft.AspNetCore.Mvc;
using MissionApp.DataAccess.GenericRepository.Interface;
using MissionApp.Entities.Models;
using MissionApp.Entities.ViewModels;

namespace MissionApp.Areas.Admins.Controllers
{
    [Area("Admins")]
    public class AdminController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        public AdminController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult AdminUserDetails()
        {
            IEnumerable<User> userLists = _unitOfWork.User.GetAll();
            AdminUserDetailsVM obj = new();
            obj.UserLists = userLists;
            return View(obj);
        }
    }
}
