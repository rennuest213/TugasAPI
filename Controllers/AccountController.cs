using API.Context;
using API.Repository.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Controllers
{
    [Authorize]
    [Route("api/akun")]
    [ApiController]
    public class AccountController : Controller
    {
        private UserRepository _repository;
        public IConfiguration _configuration;

        public AccountController(UserRepository repository, IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string email, string password)
        {

            try
            {

                var data = _repository.Login(email, password);
                if (data == null)
                {
                    return Ok(new
                    {
                        StatusCode = 200,
                        Message = "Login Gagal"
                    });
                }
                else
                {
                    /*return Ok(new
                    {
                        StatusCode = 200,
                        Message = "Login Berhasil!",
                        Data = new
                        {
                            data.Employee.Email,
                            data.Employee.FullName,
                            data.Role.Name
                        }
                    });*/
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
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = ex.Message,
                });
            }
        }

        [HttpPost]
        public IActionResult Register(string fullname, string email, DateTime birthday, string password)
        {
            try
            {
                var data = _repository.Register(fullname, email, birthday, password);
                if (data == 0)
                {
                    return Ok(new
                    {
                        Message = "Akun Gagal Dibuat",
                        StatusCode = 200
                    });
                }
                else
                {
                    return Ok(new
                    {
                        Message = "Akun Berhasil Dibuat",
                        StatusCode = 200,
                    });
                }

            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = ex.Message,
                });
            }
        }

        [HttpPut]
        public IActionResult ChangePassword(string Email, string Password, string ConfirmPassword)
        {
            try
            {
                var data = _repository.ChangePassword(Email, Password, ConfirmPassword);
                if (data == 0)
                {
                    return Ok(new
                    {
                        Message = "Gagal ganti password!",
                        StatusCode = 200
                    });
                }
                else
                {
                    return Ok(new
                    {
                        Message = "Password berhasil diganti!",
                        StatusCode = 200,
                    });
                }

            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = ex.Message,
                });
            }
        }

        [HttpPut("{ForgotPassword}")]
        public IActionResult ForgotPassword(string email, string fullName, string newPassword)
        {
            try
            {
                var data = _repository.ForgotPassword(email, fullName, newPassword);
                if (data == 0)
                {
                    return Ok(new
                    {
                        Message = "Gagal ganti password!",
                        StatusCode = 200
                    });
                }
                else
                {
                    return Ok(new
                    {
                        Message = "Password berhasil diganti!",
                        StatusCode = 200,
                    });
                }

            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = ex.Message,
                });
            }
        }
    }
}
