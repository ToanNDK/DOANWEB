using DoAnWeb2024.Models;
using DoAnWeb2024.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DoAnWeb2024.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<AppUserModel> _userManage;
        private SignInManager<AppUserModel> _signInManager;
        public AccountController(SignInManager<AppUserModel> signInManager,UserManager<AppUserModel> userManage ) 
        {
            _signInManager = signInManager;
            _userManage = userManage;
        }
        
        public IActionResult Login(string returnUrl)
		{
            return View(new LoginViewModel { ReturnUrl=returnUrl});
        }
        [HttpPost]
		public async Task<IActionResult> Login(LoginViewModel LoginVM)
        {
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(LoginVM.UserName, LoginVM.Password, false, false);
                if (result.Succeeded)
                {
                    return Redirect(LoginVM.ReturnUrl ?? "/"); 
                }
                ModelState.AddModelError("", "Sai tên đăng nhập và mật khẩu");
            }
            return View(LoginVM);
        }
		public IActionResult Create()
		{
			return View();
		}
		[HttpPost]
        
		public  async Task<IActionResult> Create(UserModel user)
        {
            if (ModelState.IsValid)
            {
                AppUserModel newUser = new AppUserModel { UserName = user.UserName, Email=user.Email };
                IdentityResult result = await _userManage.CreateAsync(newUser,user.Password);
                if (result.Succeeded)
                {
                    TempData["success"] = "Tạo tài khoản thành công !";
                    return Redirect("/account/login");
                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            
            return View(user);
        }
        public async Task<IActionResult> Logout(string returnUrl ="/")
        {
            await _signInManager.SignOutAsync();
            return Redirect(returnUrl);
        }

    }
}
