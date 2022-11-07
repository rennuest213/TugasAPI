using API.Context;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Controllers
{
    [Route("api/token")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly MyContext _context;

        public TokenController(IConfiguration configuration, MyContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        //Get user
        private async Task<User> GetUser(string email, string password)
        {
            return _context.Users.FirstOrDefault(u => u.Employee.Email.Equals(email) && u.Password.Equals(password));
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
           
                var data = await GetUser(email, password);

                if (data != null)
                {
                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserId", data.Id.ToString()),
                        new Claim("FullName", data.Employee.FullName),
                        new Claim("RoleName", data.Role.Name),
                        new Claim("Email", data.Employee.Email)
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(10),
                        signingCredentials: signIn);

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }
            }
            

        
    }
}
