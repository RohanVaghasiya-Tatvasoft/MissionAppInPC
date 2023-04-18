using MissionApp.Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissionApp.Entities.ViewModels
{
    public class UserProfileVM
    {
        [Required]
        public string? FirstName { get; set; }

        [Required]
        public string? LastName { get; set; }

        public string? WhyIVolunteer { get; set; }

        public string? Avatar { get; set; }

        public string? EmployeeId { get; set; }

        public string? Manager { get; set; }

        public string? Department { get; set; }

        [Required]
        public int CityId { get; set; }

        [Required]
        public int CountryId { get; set; }

        [Required]
        public string? ProfileText { get; set; }

        public string? LinkedInUrl { get; set; }

        public string? Availibility { get; set; }

        public string? Title { get; set; }

        public IEnumerable<UserSkill>? UserSkillList { get; set; }
        public IEnumerable<City>? Cities { get; set; }
        public IEnumerable<Country>? Countries { get; set; }
        public IEnumerable<Skill>? SkillsList { get; set; }

        [Required]
        public string? OldPassword { get; set; }

        [Required]
        public string NewPassword { get; set; } = null!;

        [NotMapped]
        [Compare("NewPassword")]
        [Required]
        public string? ConfirmPassword { get; set; }
        public User? UserInfo { get; set; }
    }
}
