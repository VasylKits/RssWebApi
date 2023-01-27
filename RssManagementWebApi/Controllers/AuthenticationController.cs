using Microsoft.AspNetCore.Mvc;
using RssManagementWebApi.DTOs;
using RssManagementWebApi.Services.Implementations;

namespace RssManagementWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly JwtService _userService;

    public AuthenticationController(JwtService userService)
    {
        _userService = userService;
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] UserRegistrationModel userRegistrationModel)
        => Ok(await _userService.Register(userRegistrationModel));

    [Route("Login")]
    [HttpPost]
    public async Task<IActionResult> Login([FromBody] UserLoginModel userLoginModel)
        => Ok(await _userService.Login(userLoginModel));
}