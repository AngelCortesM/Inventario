using System;

namespace Inventario.Models.ViewModel
{
    public class ListProveedorViewModel
    {
        public int Id { get; set; }
        public string Nit { get; set; }
        public string Nombre { get; set; }
        public string Celular { get; set; }
        public string Direccion { get; set; }
        public string Email { get; set; }
        public string Telefono_fijo { get; set; }
        public string Sitio_web { get; set; }
        public string Ciudad { get; set; }
        public string Pais { get; set; }
        public DateTime? Fecha_registro { get; set; }
    }
}