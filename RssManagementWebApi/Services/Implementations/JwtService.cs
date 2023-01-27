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
        var user_exist = await _userManager.FindByEmailAsync(userRegistrationModel.Email);

        if (user_exist != null)
        {
            return new BaseResponse<string>
            {
                ErrorMessage = "Email already exist",
                IsError = true
            };
        }

        var new_user = new IdentityUser
        {
            Email = userRegistrationModel.Email,
            UserName = userRegistrationModel.Email
        };

        var is_created = await _userManager.CreateAsync(new_user, userRegistrationModel.Password);

        if (is_created.Succeeded)
        {
            var token = GenerateJwtToken(new_user);

            return new BaseResponse<string>
            {
                IsError = false,
                Response = token
            };
        }

        return new BaseResponse<string>
        {
            IsError = true,
            ErrorMessage = "Invalid payload"
        };
    }

    public async Task<IBaseResponse<string>> Login(UserLoginModel userLoginModel)
    {
        var existing_user = await _userManager.FindByEmailAsync(userLoginModel.Email);

        if (existing_user == null)
        {
            return new BaseResponse<string>
            {
                IsError = true,
                ErrorMessage = "Invalid payload"
            };
        }

        var isCorrect = await _userManager.CheckPasswordAsync(existing_user, userLoginModel.Password);

        if (!isCorrect)
        {
            return new BaseResponse<string>
            {
                IsError = true,
                ErrorMessage = "Invalid credentials"
            };
        }

        var jwtToken = GenerateJwtToken(existing_user);

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