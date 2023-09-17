using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiProyectoApi.Models.Dto;

namespace MiProyectoApi.Datos
{
    public static class CustomerStore
    {
        public static List<CustomerDto> CustomerList= new List<CustomerDto>
        {
            new CustomerDto{Id=1, Name="Manuel", Surname="Gomez", Photo="aquivalafoto", CreatedBy=1},
            new CustomerDto{Id=2, Name="Javier", Surname="Gonzalez", Photo="aquivalafoto2", CreatedBy=2},
            new CustomerDto{Id=3, Name="Alberto", Surname="Gutierrez", Photo="aquivalafoto3", CreatedBy=3},
        };
    }
}