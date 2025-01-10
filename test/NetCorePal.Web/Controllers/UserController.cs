using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetCorePal.Extensions.Dto;

namespace NetCorePal.Web.Controllers;

/// <summary>
/// 
/// </summary>
public class UserController
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="dbContext"></param>
    /// <returns></returns>
    public async Task<ResponseData<string>> CreateUser([FromServices] ApplicationDbContext dbContext)
    {
        var user = new IdentityUser("test");
        //dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();
        return new ResponseData<string>(user.Id);
    }

    [HttpGet("/login")]
    public async Task<ResponseData<bool>> Login([FromServices] IHttpContextAccessor httpContextAccessor)
    {
        var context = httpContextAccessor.HttpContext!;
        var principal = new ClaimsPrincipal();

        var identity = new ClaimsIdentity(authenticationType:"test");
        identity.AddClaim(new Claim(ClaimTypes.Name, "test"));
        identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "test"));
        identity.AddClaim(new Claim(ClaimTypes.Role, "User"));
        principal.AddIdentity(identity);
        await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        return true.AsResponseData();
    }

    [HttpGet("/empty")]
    public async Task<ResponseData<bool>> Empty([FromServices] IHttpContextAccessor httpContextAccessor)
    {
        var context = httpContextAccessor.HttpContext!;
        return true.AsResponseData();
    }
    
    [HttpGet("/jwt")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async  Task<ResponseData<bool>> Jwt([FromServices] IHttpContextAccessor httpContextAccessor)
    {
        var context = httpContextAccessor.HttpContext!;
        return true.AsResponseData();
    }

}