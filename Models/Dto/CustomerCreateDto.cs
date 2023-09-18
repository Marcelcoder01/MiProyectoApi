using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MiProyectoApi.Models.Dto
{
    public class CustomerCreateDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        public string Photo { get; set; }
        public int CreatedBy { get; set; }
    }
}