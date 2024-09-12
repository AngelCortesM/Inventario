using System;
using System.ComponentModel;

namespace Inventario.Models.ViewModel
{
    public class ProveedorViewModel
    {
        [DisplayName("ID")]
        public int Id { get; set; }

        [DisplayName("Nit")]
        public string Nit { get; set; }

        [DisplayName("Nombre")]
        public string Nombre { get; set; }

        [DisplayName("Celular")]
        public string Celular { get; set; }

        [DisplayName("Dirección")]
        public string Direccion { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("Teléfono Fijo")]
        public string Telefono_fijo { get; set; }

        [DisplayName("Sitio Web")]
        public string Sitio_web { get; set; }

        [DisplayName("Ciudad")]
        public string Ciudad { get; set; }

        [DisplayName("Pais")]
        public string Pais { get; set; }

        [DisplayName("Fecha de Registro")]
        public DateTime? Fecha_registro { get; set; }
    }
}