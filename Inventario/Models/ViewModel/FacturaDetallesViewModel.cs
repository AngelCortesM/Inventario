using System;
using System.Collections.Generic;

namespace Inventario.Models.ViewModel
{
    public class FacturaDetallesViewModel
    {
        // Datos del encabezado de la factura
        public int FacturaId { get; set; }

        public int? ClienteId { get; set; }
        public string NombreCliente { get; set; }
        public DateTime? Fecha { get; set; }
        public DateTime? FechaPago { get; set; }
        public string Estado { get; set; }
        public string NumeroFactura { get; set; }
        public string MetodoPago { get; set; }
        public string Observaciones { get; set; }
        public decimal? Iva { get; set; }
        public decimal? Descuento { get; set; }
        public decimal? Base { get; set; }
        public decimal? Total { get; set; }

        // Lista de productos asociados a la factura
        public List<ProductoDetailsViewModel> Productos { get; set; }
    }

    public class ProductoDetailsViewModel
    {
        public int? FacturaId { get; set; }
        public int? ProductoId { get; set; }

        public string NombreProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal? Base { get; set; }
        public decimal? Iva { get; set; }
        public decimal? ValorIva { get; set; }
        public decimal? Descuento { get; set; }
        public decimal? ValorDescuento { get; set; }
        public decimal? PrecioTotal { get; set; }
    }
}