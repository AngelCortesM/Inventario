using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.WebPages.Html;

using System.Web.Mvc;

namespace Inventario.Models.ViewModel
{
    public class FacturaViewModel
    {
        public int? Id { get; set; }
        public int? ClienteId { get; set; }
        public string ClienteNombre { get; set; } // Agregado para mostrar el nombre del cliente
        public DateTime Fecha { get; set; }
        public DateTime? FechaPago { get; set; }
        public string Estado { get; set; }
        public string NumeroFactura { get; set; }
        public string MetodoPago { get; set; }
        public string Observaciones { get; set; }
        public decimal Iva { get; set; }
        public decimal? Descuento { get; set; }
        public decimal? DescuentoValue { get; set; }
        public decimal Base { get; set; }
        public decimal Total { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> ProductoList { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> ClienteList { get; set; }
        public List<ProductoViewModel> Productos { get; set; } // Agregado para el listado de productos
            

    }


}