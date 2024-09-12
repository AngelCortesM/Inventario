using System;
using System.ComponentModel;

namespace Inventario.Models.ViewModel
{
    public class ProductoViewModel
    {
        public decimal? Cantidad { get; set; }

        [DisplayName("ID")]
        public int Id { get; set; }

        [DisplayName("Proveedor ID")]
        public int? Proveedor_id { get; set; }

        [DisplayName("Valor")]
        public decimal? Valor { get; set; }

        [DisplayName("Descripción")]
        public string Descripcion { get; set; }

        [DisplayName("Estado")]
        public string Estado { get; set; }

        [DisplayName("Fecha")]
        public DateTime? Fecha { get; set; }

        [DisplayName("Nombre")]
        public string Nombre { get; set; }

        [DisplayName("Barras")]
        public string Barras { get; set; }

        [DisplayName("Categoría")]
        public string Categoria { get; set; }

        [DisplayName("Costo")]
        public decimal? Costo { get; set; }
    }
}