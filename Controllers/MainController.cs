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
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;




namespace MiProyectoApi.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class MainController : ControllerBase
    {

        private readonly ILogger<MainController> _logger;
        private readonly AppDbContext _db; 
        public MainController(ILogger<MainController> logger, AppDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        
[HttpPost("login")] // Ruta para la autenticación POST
    public IActionResult Login([FromBody] LoginRequestDto requestDto)
    {
        Console.WriteLine("Solicitud Recibida por la API");
        if (requestDto == null || string.IsNullOrEmpty(requestDto.Username) || string.IsNullOrEmpty(requestDto.Password))
        {
            return BadRequest("Credenciales inválidas");
        }

        // Aquí debes implementar la lógica de autenticación.
        // Verifica las credenciales del usuario y autentica si son válidas.
        if (EsCredencialValida(requestDto.Username, requestDto.Password))
        {
            // Si la autenticación es exitosa, crea un objeto ClaimsIdentity con información del usuario.
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, requestDto.Username),
                // Puedes agregar otras reclamaciones personalizadas aquí
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // Crea un objeto ClaimsPrincipal con el ClaimsIdentity.
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            // Inicia sesión al usuario utilizando el middleware de autenticación de cookies.
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

            // Redirige al usuario a la página de inicio o a donde desees.
            return Ok("Autenticación exitosa");
            

        }
        else
        {
            // La autenticación falló
            return BadRequest("Credenciales inválidas");
        }
    }

    private bool EsCredencialValida(string username, string password)
    {
        // Verifica si el nombre de usuario y la contraseña son correctos
        return (username == "Admin" && password == "codificacion1");
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

            return RedirectToAction("Index");
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