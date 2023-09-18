using System.Reflection;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Net;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MiProyectoApi.Models;
using MiProyectoApi.Models.Dto;
using MiProyectoApi.Datos;
using Microsoft.EntityFrameworkCore;


namespace MiProyectoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MainController : ControllerBase
    {
        private readonly ILogger<MainController> _logger;
        private readonly AppDbContext _db; // PARA usar el APPDbContext con la Api Rest
        public MainController(ILogger<MainController> logger, AppDbContext db)
        {
            _logger = logger;
            _db = db;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomer()
        {
            _logger.LogInformation("Obtener lista de clientes");
            return Ok(await _db.Customers.ToListAsync());
           
        }

        [HttpGet("{id:int}", Name = "GetCustomer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CustomerDto>> GetCustomer(int id)
        {
            if(id==0)
            {
                _logger.LogError("Error al traer el cliente con Id " + id);
                return BadRequest();
            }
            // var customer = CustomerStore.CustomerList.FirstOrDefault(c=>c.Id == id);
            var customer = await _db.Customers.FirstOrDefaultAsync(c=> c.Id == id);

            if(customer == null)
            {
                return NotFound();
            }
            return Ok(customer);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CustomerDto>> AddCustomer([FromBody] CustomerCreateDto customerDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
      
            if(customerDto == null)
            {
                return BadRequest(customerDto);
            }
   
            // customerDto.Id=CustomerStore.CustomerList.OrderByDescending(c=>c.Id).FirstOrDefault().Id + 1;
            // CustomerStore.CustomerList.Add(customerDto);

            Customer modelo = new()
            {
                Name = customerDto.Name,
                Surname = customerDto.Surname,
                Photo = customerDto.Photo,
                CreatedBy = customerDto.CreatedBy,
                Date = DateTime.Now,
            };

            await _db.Customers.AddAsync(modelo);
            await _db.SaveChangesAsync();

            return CreatedAtRoute("GetCustomer", new{id=modelo.Id}, modelo);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<IActionResult> DeleteCustomer(int id)
        {
            if(id==0)
            {
                return BadRequest();
            }
            var customer = await _db.Customers.FirstOrDefaultAsync(c=>c.Id == id);
            if(customer == null)
            {
                return NotFound();
            }
            _db.Customers.Remove(customer);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] CustomerUpdateDto customerDto)
        {
            if(customerDto == null || id != customerDto.Id)
            {
                return BadRequest();
            }

            Customer modelo = new()
            {
                Id = customerDto.Id,
                Name = customerDto.Name,
                Surname = customerDto.Surname,
                Photo = customerDto.Photo,
                CreatedBy = customerDto.CreatedBy,
                Date = DateTime.Now,
    
            };

            _db.Customers.Update(modelo);
            await _db.SaveChangesAsync();


        return NoContent();
        }
      }
}