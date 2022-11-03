using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GES.Inside.Web.Models
{
    public class GesUserViewModel
    {
        [Display(Name = "Id")]
        public string Id { get; set; }

        [Display(Name = "User Name")]
        [Required]
        [Remote("CheckUserNameExist", "AccountMgmt", AdditionalFields = "Id", ErrorMessage = "User name already exists!")]
        public string UserName { get; set; }

        [Display(Name = "Email")]
        [Required]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Not valid email")]
        [Remote("CheckEmailExist", "AccountMgmt", AdditionalFields = "Id", ErrorMessage = "Email already exists!")]
        public string Email { get; set; }

        [Display(Name = "Select Roles")]
        public string[] SelectedRoles { get; set; }
        public IEnumerable<SelectListItem> Roles { get; set; }

        [Display(Name = "Password")]
        [Required]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Locked")]
        public string LockoutEnabled { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Phone")]
        public string Phone { get; set; }

        [Display(Name = "Work Phone")]
        public string WorkPhone { get; set; }

        [Display(Name = "Mobile Phone")]
        public string MobilePhone { get; set; }

        [Display(Name = "Fax")]
        public string Fax { get; set; }

        [Display(Name = "Comment")]
        public string Comment { get; set; }

        [Display(Name = "Organization Name")]
        public string OrgName { get; set; }
        
        [Display(Name = "Select  Organization")]
        [Required]
        public long? OrgId { get; set; }


        [Display(Name = "Old User Name")]
        public string OldUserName { get; set; }
        [Display(Name = "Is Locked")]
        public bool IsLocked { get; set; }

        [Display(Name = "Select Claim")]
        public string[] SelectedClaims { get; set; }

        [Display(Name = "Last Login")]
        public DateTime? LastLogIn { get; set; }

        public string LastLogInString
        {
            get { return LastLogIn!=null?LastLogIn.Value.ToString("yyyy-MM-dd H:mm:ss tt") :""; }
        }

        public IEnumerable<SelectListItem> Claims { get; set; }
        
        public IEnumerable<SelectListItem> Organizations  { get; set; }


    }
}