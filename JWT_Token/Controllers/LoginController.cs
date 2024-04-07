using JWT_Token.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace JWT_Token.Controllers;

[EnableCors("Hotjar")]
[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    private IConfiguration _config;

    public LoginController(IConfiguration config)
    {
        _config = config;
    }

    private Users AuthenticateUser(Users users)
    {
        Users _user = new Users();
        if(users.UserName == "admin" && users.Password == "reddy@011") 
        {
            _user = new Users { UserName = "Ayyappa Reddy", };
        }

        return _user;
    }

    private string GenerateToken(Users users)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256) ;

        var token = new JwtSecurityToken(_config["Jwt:Issuer"],
            _config["Jwt:Audience"],
            null,
            expires: DateTime.Now.AddMinutes(1),
            signingCredentials: credentials
          );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    [AllowAnonymous]
    [HttpPost]
    public IActionResult Login(Users users)
    {
        IActionResult response = Unauthorized();

        var user = AuthenticateUser(users);
        if(user is not null) 
        {
            var token = GenerateToken(users);
            response = Ok(new { token = token });
        }

        return response;
    }
}

