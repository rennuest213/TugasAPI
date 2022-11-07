using API.Context;
using API.Repository.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private UserRepository _repository;

        public AccountController(UserRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
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
                    return Ok(new
                    {
                        StatusCode = 200,
                        Message = "Login Berhasil!",
                        Data = new
                        {
                            data.Employee.Email,
                            data.Employee.FullName,
                            data.Role.Name
                        }
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
