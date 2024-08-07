using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Inventario.Models.ViewModel
{
    public class ClienteViewModel
    {
        [DisplayName("ID")]
        public int Id { get; set; }

        [DisplayName("Cédula")]
        public string Cedula { get; set; }

        [DisplayName("Dirección")]
        public string Direccion { get; set; }

        [DisplayName("Nombre")]
        public string Nombre { get; set; }

        [DisplayName("Teléfono")]
        public string Telefono { get; set; }

        [DisplayName("Apellido")]
        public string Apellido { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("Fecha de Creación")]
        public DateTime? Fecha { get; set; }

        [DisplayName("Fecha de Actualización")]
        public DateTime? FechaUp { get; set; }
    }
}