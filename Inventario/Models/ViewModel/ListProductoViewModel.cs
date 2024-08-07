using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inventario.Models.ViewModel
{
    public class ListProductoViewModel
    {
        public decimal? Cantidad { get; set; } // Cantidad del producto
        public int Id { get; set; } // Identificador único del producto (clave primaria)
        public int? Proveedor_id { get; set; } // Identificador del proveedor (clave foránea)
        public decimal? Valor { get; set; } // Valor del producto
        public string Descripcion { get; set; } // Descripción del producto
        public string Estado { get; set; } // Estado del producto
        public DateTime? Fecha { get; set; } // Fecha relacionada con el producto
        public string Nombre { get; set; } // Nombre del producto
        public string Barras { get; set; } // Código de barras del producto
        public string Categoria { get; set; } // Categoría del producto
        public decimal? Costo { get; set; } // Costo del producto
        public string ProveedorNombre { get; set; } // Id del proveedor
    }
}