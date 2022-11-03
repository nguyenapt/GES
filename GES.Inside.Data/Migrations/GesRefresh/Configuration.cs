using System.Collections.Generic;
using System.Net;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;
using GES.Inside.Data.Models.Auth;
using Microsoft.AspNet.Identity;

namespace GES.Inside.Data.Migrations.GesRefresh
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<GesRefreshDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            MigrationsDirectory = @"Migrations\GesRefresh";
        }

        protected override void Seed(GesRefreshDbContext context)
        {
            var oldDbContext = new DataContexts.GesEntities();

            var oldUsers = oldDbContext.G_Users.Include(d => d.G_Individuals1).Where(d => d.G_Individuals1 != null && !string.IsNullOrEmpty(d.G_Individuals1.Email)).OrderBy(t => t.Username).ThenByDescending(d => d.LastLogIn).ThenByDescending(d => d.Created).ToList();

            var listUsers = context.Users.Where(d => d.OldUserId != null).ToList();

            var listNewUser = new List<GesUser>();
            var userName = "";
            var duplicateAccount = 0;
            oldUsers.ForEach(d =>
            {
                var passwordHasd = new PasswordHasher();
                var password = passwordHasd.HashPassword(d.Password);

                //check duplicate UserName:
                // if the userName is duplicated => add a number to userName
                if (userName != d.Username.Trim().ToLower())
                {
                    userName = d.Username.Trim().ToLower();
                    duplicateAccount = 0;
                }
                else
                {
                    duplicateAccount += 1;
                }

                var newUSerName = (duplicateAccount > 0) ? d.Username + "_" + duplicateAccount.ToString() : d.Username.Trim();

                var email = string.Empty;
                var phoneNumber = string.Empty;
                if (d.G_Individuals1 != null)
                {
                    email = d.G_Individuals1.Email?.Trim().ToLower();
                    phoneNumber = d.G_Individuals1.Phone;
                }

                if (GES.Common.Helpers.UtilHelper.IsEmailAddress(email))
                {
                    var addedUser = listUsers.FirstOrDefault(u => u.OldUserId == d.G_Users_Id);
                    var existsUserName = listUsers.FirstOrDefault(u => u.UserName.Equals(d.Username,StringComparison.OrdinalIgnoreCase));// fixbug: add duplicate username

                    if (addedUser == null)
                    {
                        if (existsUserName != null)
                        {
                            if (email == existsUserName.Email)
                            {
                                var newUserId = existsUserName.Id.ToString();

                                listNewUser.Add(new GesUser
                                {
                                    Id = newUserId,
                                    UserName = newUSerName,
                                    LockoutEnabled = false,
                                    PasswordHash = password,
                                    SecurityStamp = "",
                                    Email = email,
                                    PhoneNumber = phoneNumber,
                                    OldUserId = d.G_Users_Id,
                                    LastLogIn = d.LastLogIn
                                });
                            }
                        }
                        else
                        {
                            var newUserId = Guid.NewGuid().ToString();

                            listNewUser.Add(new GesUser { Id = newUserId, UserName = newUSerName, LockoutEnabled = false, PasswordHash = password, SecurityStamp = "", Email = email, PhoneNumber = phoneNumber, OldUserId = d.G_Users_Id, LastLogIn = d.LastLogIn });
                        }
                        
                    }
                }
            });

            //remove account with duplicate Email (keep account with lastest login)
            var listDuplicateEmail =
                    listNewUser
                    .GroupBy(c => c.Email)
                    .Where(grp => grp.Count() > 1)
                    .Select(grp => grp.Key).ToList();

            if (listDuplicateEmail.Any())
            {
                foreach (var emailItem in listDuplicateEmail)
                {
                    var list = listNewUser.Where(d => d.Email == emailItem).OrderByDescending(d => d.LastLogIn).ThenByDescending(d => d.OldUserId).ToList();
                    var keepEmail = list.FirstOrDefault();

                    foreach (var itemDelete in list)
                    {
                        if (itemDelete.Id != keepEmail.Id)
                        {
                            listNewUser.Remove(itemDelete);
                        }
                    }
                }
            }
            
            context.Users.AddOrUpdate(listNewUser.ToArray());

            context.Roles.AddOrUpdate(new GesRole { Id = "1", Name = "Admin" }, new GesRole { Id = "2", Name = "SuperAdmin" });

            context.PersonalSettingCategories.AddOrUpdate(new PersonalSettingCategories { PersonalSettingCategoryId = 1, Name = "Dashboard - Portfolios selected" }, new PersonalSettingCategories { PersonalSettingCategoryId = 2, Name = "Dashboard - Indices selected" });

            context.SaveChanges();
        }
    }
}
