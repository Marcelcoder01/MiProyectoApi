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


namespace MiProyectoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MainController : ControllerBase
    {
        private readonly ILogger<MainController> _logger;
        public MainController(ILogger<MainController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public ActionResult<IEnumerable<CustomerDto>> GetCustomer()
        {
            _logger.LogInformation("Obtener lista de clientes");
            return Ok(CustomerStore.CustomerList);
           
        }

        [HttpGet("{id:int}", Name = "GetCustomer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<CustomerDto> GetCustomer(int id)
        {
            if(id==0)
            {
                _logger.LogError("Error al traer el cliente con Id " + id);
                return BadRequest();
            }
            var customer = CustomerStore.CustomerList.FirstOrDefault(c=>c.Id == id);

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
        public ActionResult<CustomerDto> AddCustomer([FromBody] CustomerDto customerDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
      
            if(customerDto == null)
            {
                return BadRequest(customerDto);
            }
            if(customerDto.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            customerDto.Id=CustomerStore.CustomerList.OrderByDescending(c=>c.Id).FirstOrDefault().Id + 1;
            CustomerStore.CustomerList.Add(customerDto);

            return CreatedAtRoute("GetCustomer", new{id=customerDto.Id}, customerDto);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public IActionResult DeleteCustomer(int id)
        {
            if(id==0)
            {
                return BadRequest();
            }
            var customer = CustomerStore.CustomerList.FirstOrDefault(c=>c.Id == id);
            if(customer == null)
            {
                return NotFound();
            }
            CustomerStore.CustomerList.Remove(customer);
            return NoContent();
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public IActionResult UpdateCustomer(int id, [FromBody] CustomerDto customerDto)
        {
            if(customerDto == null || id != customerDto.Id)
            {
                return BadRequest();
            }
            var customer = CustomerStore.CustomerList.FirstOrDefault(c=>c.Id == id);
            customer.Name = customerDto.Name;
            customer.Surname = customerDto.Surname;
            customer.Photo = customerDto.Photo;
            customer.CreatedBy = customerDto.CreatedBy;

        return NoContent();
        }
      }
}