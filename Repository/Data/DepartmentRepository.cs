using API.Context;
using API.Models;
using API.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Repository.Data
{
    public class DepartmentRepository : IRepository<Department, int>
    {
        private MyContext _context;

        public DepartmentRepository(MyContext context)
        {
            _context = context;
        }

        [HttpPost]  
        public int Create(Department entity)
        {
            _context.Departments.Add(entity);
            var result = _context.SaveChanges();
            return result;
        }

        public int Delete(int entity)
        {
            var data = _context.Departments.Find(entity);
            if (data != null)
            {
                _context.Remove(entity);
                var result = _context.SaveChanges();
                return result;
            }
            return 0;
        }

        public IEnumerable<Department> Get()
        {
            return _context.Departments;
        }

        public Department GetById(int id)
        {
            return _context.Departments.Find(id);
        }

        public int Update(Department entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            var result = (_context.SaveChanges());
            return result;
        }
    }
}
