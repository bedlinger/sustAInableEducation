using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using sustAInableEducation_backend.Models;
using sustAInableEducation_backend.Repository;

namespace sustAInableEducation_backend.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class AccountController : ControllerBase
{
    private readonly IAIService _ai;
    private readonly ApplicationDbContext _context;
    private readonly ApplicationUser _user;
    private readonly string _userId;


    public AccountController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IAIService ai)
    {
        _context = context;
        _userId = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        _user = _context.Users.Find(_userId)!;
        _ai = ai;
    }

    [HttpGet]
    public ActionResult<ApplicationUser> GetAccount()
    {
        return _user;
    }

    [HttpPost("profileImage")]
    public async Task<ApplicationUser> SignUp(ImageRequest imageRequest)
    {
        _user.ProfileImage =
            await _ai.GenerateProfileImage(_user.AnonUserName, imageRequest.Style).ConfigureAwait(false);
        _context.SaveChanges();
        return _user;
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout(SignInManager<ApplicationUser> signInManager)
    {
        await signInManager.SignOutAsync().ConfigureAwait(false);
        return NoContent();
    }

    [HttpPost("changeEmail")]
    public async Task<IActionResult> ChangeEmail(ChangeEmailRequest request,
        UserManager<ApplicationUser> userManager)
    {
        var passwordResult = await userManager.CheckPasswordAsync(_user, request.Password);
        if (!passwordResult) return Unauthorized();

        var userNameResult = await userManager.SetUserNameAsync(_user, request.NewEmail);
        var emailResult = await userManager.SetEmailAsync(_user, request.NewEmail);
        if (!userNameResult.Succeeded || !emailResult.Succeeded)
            return BadRequest(userNameResult.Succeeded ? emailResult.Errors : userNameResult.Errors);

        return NoContent();
    }

    [HttpPost("changePassword")]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequest request,
        UserManager<ApplicationUser> userManager)
    {
        var result = await userManager.ChangePasswordAsync(_user, request.OldPassword, request.NewPassword)
            .ConfigureAwait(false);
        if (!result.Succeeded) return BadRequest(result.Errors);

        return NoContent();
    }
}