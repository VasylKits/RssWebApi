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
    {
        var response = await _userService.Register(userRegistrationModel);    

        return (response.Response == null)
            ? BadRequest(response)
            : Ok(response);
    }

    [Route("Login")]
    [HttpPost]
    public async Task<IActionResult> Login([FromBody] UserLoginModel userLoginModel)
    {
        var response = await _userService.Login(userLoginModel);

        return (response.Response == null)
            ? BadRequest(response)
            : Ok(response);
    }
}