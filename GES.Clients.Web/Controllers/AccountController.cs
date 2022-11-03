using System.Linq;
using System.Web;
using System.Web.Mvc;
using GES.Clients.Web.Models;
using GES.Inside.Data.Services.Auth;
using GES.Inside.Data.Services.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using GES.Common.Logging;

using GES.Inside.Data.DataContexts;

namespace GES.Clients.Web.Controllers
{
    public class AccountController : GesControllerBase
    {

        private IOldUserService _oldUserService;
        private IG_IndividualsService _individualsService;

        #region Constructor
        public AccountController(IGesLogger logger, IOldUserService oldUserService, IG_IndividualsService individualsRepository)
            :base(logger)
        {
            _oldUserService = oldUserService;
            _individualsService = individualsRepository;
        }
        #endregion

        // GET: User
        public ActionResult UserProfile()
        {
            var usermager = HttpContext.GetOwinContext().GetUserManager<GesUserManager>();

            var currentUser = usermager.FindById(User.Identity.GetUserId());

            return this.SafeExecute(() =>
            {
                var individual = _individualsService.GetIndividualByUserId(currentUser.OldUserId ?? -1);

                var userModel = new GesUserViewModel
                {
                    UserName = currentUser.UserName,
                    Email = currentUser.Email,
                    FirstName = individual.FirstName,
                    LastName = individual.LastName,
                    Phone = individual.Phone,
                    MobilePhone = individual.MobilePhone,
                    WorkPhone = individual.WorkPhone
                };

                return View(userModel);
            }, $"Error when get the individual information with userId {currentUser?.OldUserId}");
        }

        [HttpPost]
        public JsonResult GeneralInfor(GesUserViewModel model)
        {
            var usermager = HttpContext.GetOwinContext().GetUserManager<GesUserManager>();
            var currentUser = usermager.FindById(User.Identity.GetUserId());

            currentUser.Email = model.Email;
            var result = usermager.Update(currentUser);

            if (result.Succeeded)
            {
                this.SafeExecute(() =>
                {
                    var individual = _individualsService.GetIndividualByUserId(currentUser.OldUserId ?? -1);
                    if (individual != null)
                    {
                        individual.FirstName = model.FirstName;
                        individual.LastName = model.LastName;
                        individual.Email = model.Email;
                        individual.Phone = model.Phone;
                        individual.MobilePhone = model.MobilePhone;
                        individual.WorkPhone = model.WorkPhone;

                        _individualsService.Update(individual, true);

                        // update session
                        Session["FirstName"] = model.FirstName;
                        Session["LastName"] = model.LastName;
                    }
                }, $"Error when get the individual information with userId {currentUser?.OldUserId}");
                
                return Json(new
                {
                    success = true,
                    message = "Your profile has been updated successfully.",
                    error = ""
                });
            }

            return Json(new
            {
                success = false,
                message = "",
                error = result.Errors.FirstOrDefault()
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ChangePassword(GesUserPasswordViewModel model)
        {
            var usermager = HttpContext.GetOwinContext().GetUserManager<GesUserManager>();

            var user = usermager.FindById(User.Identity.GetUserId());
            if (user == null)
            {
                return Json(new
                {
                    success = false,
                    message = "",
                    error = "User not found!"
                });
            }
            var result = usermager.ChangePasswordAsync(user.Id, model.OldPassword, model.NewPassword);
            if (result.Result.Succeeded)
            {
                G_Users oldUser = this.SafeExecute(() => _oldUserService.GetUserById(user.OldUserId ?? -1), $"Error when get the the user information with userId {user?.OldUserId}");

                if (oldUser != null)
                {
                    oldUser.Password = model.NewPassword;

                    this.SafeExecute(() => _oldUserService.Update(oldUser, true), $"Error when update the user information");
                }

                return Json(new
                {
                    success = true,
                    message = "Password has been updated successfully.",
                    error = ""
                });
            }
            var errorMsg = string.Join("; ", result.Result.Errors);
            return Json(new
            {
                success = false,
                message = "",
                error = errorMsg
            });
        }

    }
}