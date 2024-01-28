using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebSalesMvc.Models;
using WebSalesMvc.Services;
using WebSalesMvc.Services.Exceptions;
using WebSalesMvcWithAngular.Services.Exceptions;

namespace WebSalesMvc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly DepartmentService _departmentService;
        public DepartmentsController(DepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet]
        [Route("get-departments")]
        public async Task<ActionResult<List<Department>>> GetDepartments()
        {
            try
            {
                var departments = await _departmentService.FindAllAsync();

                if (departments == null || departments.Count == 0)
                {
                    return NoContent();
                }

                return Ok(departments);
            }
            catch (NotFoundException ex)
            {
                return NotFound("Nenhum departamento encontrado.");
            }
            catch (UnauthorizedException ex)
            {
                return Unauthorized("Sem autorização.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro interno da aplicação.");
            }
        }

        [HttpGet("{id}")]
        [Route("get-department/{id}")]
        public async Task<IActionResult> GetDepartmentsById(int? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest("ID não fornecido.");
                }

                var department = await _departmentService.FindByIdAsync(id.Value);

                if (department == null)
                {
                    return NotFound($"Departmento com o ID {id} não foi encontrado.");
                }

                return Ok(department);

            }
            catch (UnauthorizedException ex)
            {
                return Unauthorized("Sem autorização.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro interno da aplicação. ");
            }
        }


        [HttpPost]
        [Route("post-department")]
        public async Task<IActionResult> Create([FromBody] Department department)
        {
            try
            {
                if (department == null)
                {
                    return BadRequest("Departmento não fornecido.");
                }

                if (ModelState.IsValid)
                {
                    await _departmentService.InsertAsync(department);
                    return CreatedAtAction("Details", new { id = department.Id }, department);
                }
                else
                {
                    return UnprocessableEntity(ModelState);
                }
            }
            catch (UnauthorizedException ex)
            {
                return Unauthorized("Sem autorização.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro interno da aplicação");
            }
        }

        [HttpPut("{id}")]
        [Route("edit-department/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromBody] Department department)
        {
            try
            {
                if (id != department.Id)
                {
                    return BadRequest("ID fornecido difere do ID que vai ser atualizado.");
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        await _departmentService.UpdateAsync(department);

                        return Ok();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!DepartmentExists(department.Id))
                        {
                            return NotFound("Departamento não encontrado.");
                        }
                        else
                        {
                            return StatusCode(500, "Erro de simultaneidade.");
                        }
                    }
                }
                else
                {
                    return UnprocessableEntity(ModelState);
                }
            }
            catch (UnauthorizedException ex)
            {
                return Unauthorized("Sem autorização.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro interno da aplicação.");
            }
        }


        [HttpDelete("{id}")]
        [Route("confirm-delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            try
            {
                await _departmentService.DeleteAsync(id);

                return NoContent();
            }
            catch (NotFoundException)
            {
                return NotFound("Departmento não encontrado.");
            }
            catch (UnauthorizedException ex)
            {
                return Unauthorized("Sem autorização.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro interno da aplicação.");
            }
        }
        private bool DepartmentExists(int id)
        {
            return _departmentService.DepartmentExistsAsync(id);
        }

    }
}
    
