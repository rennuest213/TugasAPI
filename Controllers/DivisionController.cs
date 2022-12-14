using API.Models;
using API.Repository.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DivisionController : ControllerBase
    {
        private DivisionRepository _repository;

        public DivisionController(DivisionRepository repository)
        {
            _repository = repository;   
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            try
            {
                var data = _repository.Get();
                if (data == null)
                {
                    return Ok(new
                    {
                        StatusCode = 200,
                        Message = "Data Tidak Ada!"
                    });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = 200,
                        Message = "Data Ada",
                        Data = data
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

        [HttpGet("{id}")]
        public ActionResult GetById(int id)
        {
            try
            {
                var data = _repository.GetById(id);
                if (data == null)
                {
                    return Ok(new {
                        StatusCode = 200,
                        Message = "Data Tidak Ditemukan" 
                    });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = 200,
                        Message = "Data Ada",
                        Data = data
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
        public ActionResult Create(Division division)
        {
            try
            {
                var result = _repository.Create(division);
                if (result == 0)
                {
                    return Ok(new { 
                        Message = "Data Gagal Disimpan",
                        StatusCode = 200
                    });
                }
                else
                {
                    return Ok(new { 
                        Message = "Data Berhasil Disimpan",
                        StatusCode = 200,
                        Data = result
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
        public ActionResult Update(Division division)
        {
            try
            {
                var result = _repository.Update(division);
                if (result == 0)
                {
                    return Ok(new { 
                        Message = "Data Gagal Di-Update",
                        StatusCode = 200
                    });
                }
                else
                {
                    return Ok(new { 
                        Message = "Data Berhasil Di-Update",
                        StatusCode = 200,
                        Data = result
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

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            try
            {
                var result = _repository.Delete(id);
                if (result > 0)
                {
                    return Ok(new { 
                        Message = "Data Berhasil Dihapus",
                        StatusCode = 200,
                        Data = result,
                    });
                }
                else
                {
                    return Ok(new { 
                        Message = "Data Gagal Dihapus",
                        StatusCode = 200
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
