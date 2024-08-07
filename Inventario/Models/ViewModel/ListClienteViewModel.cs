using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inventario.Models.ViewModel
{
    public class ListClienteViewModel
    {
        public int Id { get; set; }
        public string Cedula { get; set; }
        public string Direccion { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public DateTime? Fecha { get; set; }
        public DateTime? FechaUp { get; set; }
    }

}