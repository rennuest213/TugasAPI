using API.Context;
using API.Models;
using API.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Repository.Data
{
    public class UserRepository
    {
        private MyContext _context;

        public UserRepository(MyContext context)
        {
            _context = context;
        }

        public User Login(string username, string password)
        {
            var data = _context.Users
                .Include(x => x.Employee)
                .Include(x => x.Role)
                .SingleOrDefault(x => x.Employee.Email.Equals(username) && x.Password.Equals(password));

            return data;
        }

        public int Register(string fullname, string email, DateTime birthday, string password)
        {
            Employee employee = new()
            {
                FullName = fullname,
                Email = email,
                BirthDate = birthday
            };
            _context.Employees.Add(employee);

            var validate = _context.Employees.SingleOrDefault(x => x.Email.Equals(email));
            if (validate == null)
            {
                var result = _context.SaveChanges();

                if (result > 0)
                {
                    var id = _context.Employees.SingleOrDefault(x => x.Email.Equals(email)).Id;
                    //var id = myContext.Employees.Where(x => x.Email.Equals(email)).Select(x => new {x.Email}).Select(x => new {x.Id});
                    User user = new User()
                    {
                        Id = id,
                        Password = password,
                        RoleId = 3
                    };
                    _context.Users.Add(user);
                    var resultUser = _context.SaveChanges();

                }
                return result;
            }
            return 0;
        }

        public int ChangePassword(string Email, string Password, string ConfirmPassword)
        {

            var data = _context.Users
                .Join(_context.Employees, u => u.Id, emp => emp.Id, (u, emp) => new { u, emp })
                .Join(_context.Roles, ur => ur.u.RoleId, r => r.Id, (ur, r) => new
                {
                    Email = ur.emp.Email,
                    Password = ur.u.Password,
                    RoleId = ur.u.RoleId,
                    UserId = ur.u.Id,
                    EmployeeId = ur.u.Id
                })
                .SingleOrDefault(x => x.Email.Equals(Email) && x.Password.Equals(Password));

            /*.Include(x => x.Employee)
            .Include(x => x.Role)                
            .SingleOrDefault(x => x.Employee.Email.Equals(email) && x.Password.Equals(oldPassword));*/
            //.Select(x => new { x.RoleId, x.Employee.Email, x.Password });
            if (data != null)
            {

                User user = new User()
                {
                    Id = data.UserId,
                    Password = ConfirmPassword,
                    RoleId = data.RoleId
                };

                _context.Entry(user).State = EntityState.Modified;
                var resultUser = _context.SaveChanges();

                return resultUser;
            }

            return 0;
        }

        public int ForgotPassword(string email, string newPassword)
        {
            var data = _context.Users
                .Join(_context.Employees, u => u.Id, emp => emp.Id, (u, emp) => new { u, emp })
                .Join(_context.Roles, ur => ur.u.RoleId, r => r.Id, (ur, r) => new
                {
                    Email = ur.emp.Email,
                    Password = ur.u.Password,
                    RoleId = ur.u.RoleId,
                    UserId = ur.u.Id,
                    EmployeeId = ur.u.Id
                })
                .SingleOrDefault(x => x.Email.Equals(email));

            if (data != null)
            {

                User user = new()
                {
                    Id = data.UserId,
                    Password = newPassword,
                    RoleId = data.RoleId,
                };

                _context.Entry(user).State = EntityState.Modified;
                var resultUser = _context.SaveChanges();
                return resultUser;
            }

            return 0;
        }
    }
}
