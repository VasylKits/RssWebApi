using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using RssManagementWebApi.DTOs;
using RssManagementWebApi.Models;
using RssManagementWebApi.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;

namespace RssManagementWebApi.Services.Implementations;

public class JwtService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IConfiguration _configuration;

    public JwtService(UserManager<IdentityUser> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<IBaseResponse<string>> Register(UserRegistrationModel userRegistrationModel)
    {
        var userExist = await _userManager.FindByEmailAsync(userRegistrationModel.Email);

        if (userExist != null)
        {
            return new BaseResponse<string>
            {
                ErrorMessage = "Email already exist",
                IsError = true
            };
        }

        var newUser = new IdentityUser
        {
            Email = userRegistrationModel.Email,
            UserName = userRegistrationModel.Name
        };

        var isCreated = await _userManager.CreateAsync(newUser, userRegistrationModel.Password);

        if (!isCreated.Succeeded)
        {

            return new BaseResponse<string>
            {
                IsError = true,
                ErrorMessage = "Invalid payload"
            };
        }

        return new BaseResponse<string>
        {
            IsError = false,
            Response = $"Welcome {newUser.UserName}!"
        };
    }

    public async Task<IBaseResponse<string>> Login(UserLoginModel userLoginModel)
    {
        var existingUser = await _userManager.FindByEmailAsync(userLoginModel.Email);

        if (existingUser == null)
        {
            return new BaseResponse<string>
            {
                IsError = true,
                ErrorMessage = "Invalid payload"
            };
        }

        var isCorrect = await _userManager.CheckPasswordAsync(existingUser, userLoginModel.Password);

        if (!isCorrect)
        {
            return new BaseResponse<string>
            {
                IsError = true,
                ErrorMessage = "Invalid credentials"
            };
        }

        var jwtToken = GenerateJwtToken(existingUser);

        return new BaseResponse<string>
        {
            Response = jwtToken,
            IsError = false
        };
    }

    #region private
    private string GenerateJwtToken(IdentityUser user)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();

        var key = Encoding.UTF8.GetBytes(_configuration.GetSection("JwtConfig:Secret").Value);

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("Id", user.Id),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Email, value: user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString())
            }),

            Expires = DateTime.Now.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };

        var token = jwtTokenHandler.CreateToken(tokenDescriptor);
        return jwtTokenHandler.WriteToken(token);
    }
    #endregion
}