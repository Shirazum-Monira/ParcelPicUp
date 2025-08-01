using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using parcelPicUp.Models;
using parcelPicUp.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace parcelPicUp.Controllers
{
    public class UserRolesController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserRolesController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }

        public async Task<IActionResult> Manage(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var roles = _roleManager.Roles.ToList();
            var userRoles = await _userManager.GetRolesAsync(user);

            var model = new ManageUserRolesViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                Roles = roles.Select(role => new RoleSelection
                {
                    RoleName = role.Name,
                    IsSelected = userRoles.Contains(role.Name)
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Manage(ManageUserRolesViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            var existingRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, existingRoles);

            var selectedRoles = model.Roles.Where(r => r.IsSelected).Select(r => r.RoleName);
            await _userManager.AddToRolesAsync(user, selectedRoles);

            return RedirectToAction("Index");
        }
    }
}
