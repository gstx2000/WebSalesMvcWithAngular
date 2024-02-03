using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebSalesMvc.Data;
using WebSalesMvc.Models;
using WebSalesMvc.Services;
using WebSalesMvc.Services.Exceptions;
using WebSalesMvcWithAngular.Services.Exceptions;

namespace WebSalesMvc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class CategoriesController : ControllerBase
    {
        private readonly WebSalesMvcContext _context;
        private readonly CategoryService _categoryService;
        private readonly DepartmentService _departmentService;

        public CategoriesController(WebSalesMvcContext context, CategoryService categoryService, DepartmentService departmentService)
        {
            _context = context;
            _categoryService = categoryService;
            _departmentService = departmentService;
        }
        [HttpGet]
        [Route("get-categories")]
        public async Task<ActionResult<List<Category>>> GetCategories()
        {
            try
            {
                var categories = await _categoryService.FindAllAsync();

                if (categories == null || categories.Count == 0)
                {
                    return NoContent();
                }

                return Ok(categories);
            }
            catch (NotFoundException ex)
            {
                return NotFound("Nenhuma categoria encontrada.");
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
        [Route("get-category/{id}")]
        public async Task<IActionResult> GetCategoriesById(int? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest("ID não fornecido.");
                }

                var category = await _categoryService.FindByIdAsync(id.Value);

                if (category == null)
                {
                    return NotFound($"Categoria com o ID {id} não foi encontrado.");
                }

                return Ok(category);

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
        [Route("post-category")]
        public async Task<IActionResult> Create([FromBody] Category category)
        {
            try
            {
                if (category == null)
                {
                    return BadRequest("Categoria não fornecida.");
                }
                
                var department = await _departmentService.FindByIdAsync(category.DepartmentId);

                if (department == null)
                {
                    return BadRequest("Departamento não encontrado.");
                }
                category.Department = department;

                if (ModelState.IsValid)
                {
                    await _categoryService.InsertAsync(category);
                    return CreatedAtAction("Details", new { id = category.Id }, category);
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
        [Route("edit-category/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromBody] Category category)
        {
            try
            {
                if (id != category.Id)
                {
                    return BadRequest("ID fornecido difere do ID que vai ser atualizado.");
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        await _categoryService.UpdateAsync(category);

                        return Ok();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!CategoryExists(category.Id))
                        {
                            return NotFound("Categoria não encontrada.");
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
        [Route("delete-category/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            try
            {
                await _categoryService.DeleteAsync(id);

                return NoContent();
            }
            catch (NotFoundException)
            {
                return NotFound("Categoria não encontrada.");
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
        private bool CategoryExists(int id)
        {
            return _context.Category.Any(e => e.Id == id);
        }  
    }
}
