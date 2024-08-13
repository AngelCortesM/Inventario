using Inventario.Models;
using Inventario.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Inventario.Controllers
{
    public class FacturaController : Controller
    {
        // GET: Factura
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List()
        {
            List<ListFacturaViewModel> lst = new List<ListFacturaViewModel>();
            using (CrudMVCRazorEntities db = new CrudMVCRazorEntities())
            {
                lst = (from f in db.factura
                       join c in db.cliente on f.cliente_id equals c.id into clientes
                       from cliente in clientes.DefaultIfEmpty()
                       select new ListFacturaViewModel
                       {
                           FacturaId = f.id,
                           ClienteId = f.cliente_id,
                           NombreCliente = cliente != null ? cliente.nombre + " " + cliente.apellido : "No disponible",
                           Fecha = f.fecha,
                           FechaPago = f.fecha_pago,
                           Estado = f.estado,
                           NumeroFactura = f.numero_factura,
                           MetodoPago = f.metodo_pago,
                           Observaciones = f.observaciones,
                           Iva = f.iva,
                           Descuento = f.descuento,
                           Base = f.@base,
                           Total = f.total
                       }).ToList();
            }
            return View(lst);
        }

        public ActionResult Detalles(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);  // Manejar id nulo
            }
            FacturaDetallesViewModel model;

            using (var db = new CrudMVCRazorEntities())
            {
                // Obtener los datos de la factura
                var factura = db.factura
                    .Where(f => f.id == id)
                    .Select(f => new FacturaDetallesViewModel
                    {
                        FacturaId = f.id,
                        ClienteId = f.cliente_id,
                        NombreCliente = f.cliente.nombre + " " + f.cliente.apellido,
                        Fecha = f.fecha,
                        FechaPago = f.fecha_pago,
                        Estado = f.estado,
                        NumeroFactura = f.numero_factura,
                        MetodoPago = f.metodo_pago,
                        Observaciones = f.observaciones,
                        Iva = f.iva,
                        Descuento = f.descuento,
                        Base = f.@base,
                        Total = f.total,
                        Productos = f.facturahasproducto
                            .Select(fp => new ProductoDetailsViewModel
                            {
                                ProductoId = fp.producto_id,
                                // obtener la información del producto
                                NombreProducto = db.producto
                                    .Where(p => p.id == fp.producto_id)
                                    .Select(p => p.nombre)
                                    .FirstOrDefault(),
                                Cantidad = fp.cantidad,
                                PrecioUnitario = fp.precio_unitario,
                                Base = fp.@base,
                                Iva = fp.iva,
                                ValorIva = fp.valor_iva,
                                Descuento = fp.descuento,
                                ValorDescuento = fp.valor_descuento,
                                PrecioTotal = fp.precio_total
                            }).ToList()
                    }).FirstOrDefault();

                if (factura == null)
                {
                    return HttpNotFound();
                }

                model = factura;
            }

            return View(model);
        }

    }
}
