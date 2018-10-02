using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DataDecipher.WebApp.Data;

namespace DataDecipher.WebApp.Models
{
    public class ApplicationUserListViewModel: ApplicationUser
    {
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public List<SelectListItem> AvailableRoles { get; set; }

        public List<SelectListItem> AvailableOrganizations { get; set; }

        [Display(Name = "Role")]
        public string ApplicationRoleId { get; set; }
    }
}
