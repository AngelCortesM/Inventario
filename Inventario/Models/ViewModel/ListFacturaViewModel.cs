using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inventario.Models.ViewModel
{
    public class ListFacturaViewModel
    {
        // Datos de la tabla factura
        public int FacturaId { get; set; } // Identificador único de la factura
        public int? ClienteId { get; set; } // Identificador del cliente
        public DateTime? Fecha { get; set; } // Fecha de la factura
        public DateTime? FechaPago { get; set; } // Fecha de pago
        public string Estado { get; set; } // Estado de la factura
        public string NumeroFactura { get; set; } // Número de la factura
        public string MetodoPago { get; set; } // Método de pago
        public string Observaciones { get; set; } // Observaciones de la factura
        public decimal? Iva { get; set; } // IVA
        public decimal? Descuento { get; set; } // Descuento
        public decimal? Base { get; set; } // Base imponible
        public decimal Total { get; set; } // Total de la factura
        public string NombreCliente { get; set; } // Nombre del Cliente
    }
}