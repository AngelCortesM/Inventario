using Inventario.Models;
using Inventario.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Inventario.Controllers
{
    public class FacturaController : Controller
    {
        private readonly CrudMVCRazorEntities db = new CrudMVCRazorEntities();

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
            // Verificar si la lista está vacía
            if (!lst.Any())
            {
                // Si la lista está vacía, puedes redirigir a otra acción, mostrar un mensaje, etc.

                ViewBag.Message = "No hay facturas disponibles.";
                return RedirectToAction("Add", "Factura");
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

        public ActionResult Add()
        {
            using (var db = new CrudMVCRazorEntities())
            {
                var productos = db.producto.ToList();
                var clientes = db.cliente.ToList();

                var productoSelectList = productos.Select(p => new SelectListItem
                {
                    Value = p.id.ToString(),
                    Text = p.nombre // Solo el nombre del producto en el dropdown
                }).ToList();

                var clienteSelectList = clientes.Select(c => new SelectListItem
                {
                    Value = c.id.ToString(),
                    Text = c.nombre
                }).ToList();

                // Pasar el costo al ViewBag
                ViewBag.ProductoCostos = productos.Select(p => new
                {
                    Id = p.id,
                    Costo = p.costo
                }).ToList();

                var viewModel = new FacturaViewModel
                {
                    ProductoList = productoSelectList,
                    ClienteList = clienteSelectList,
                };

                return View(viewModel);
            }
        }

        [HttpPost]
        public ActionResult Save(FacturaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)) });
            }

            if (model.Productos == null || !model.Productos.Any())
            {
                return Json(new { success = false, errors = new[] { "La lista de productos no puede estar vacía." } });
            }

            try
            {
                using (var db = new CrudMVCRazorEntities())
                {
                    using (var transaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            // Crear la nueva factura
                            var oFactura = new factura
                            {
                                cliente_id = model.ClienteId,
                                fecha = model.Fecha,
                                fecha_pago = model.FechaPago,
                                estado = model.Estado,
                                numero_factura = model.NumeroFactura,
                                metodo_pago = model.MetodoPago,
                                observaciones = model.Observaciones,
                                iva = model.Iva,
                                descuento = model.Descuento,
                                @base = model.Base,
                                total = model.Total
                            };

                            db.factura.Add(oFactura);
                            db.SaveChanges();

                            // Crear los detalles de la factura
                            foreach (var producto in model.Productos)
                            {
                                // Verificar si el producto existe
                                if (!db.producto.Any(p => p.id == producto.Id))
                                {
                                    return Json(new { success = false, errors = new[] { $"El producto con ID {producto.Id} no existe." } });
                                }

                                var precioUnitario = producto.Costo ?? 0;
                                var valorIva = precioUnitario * 0.19m; // IVA del 19%
                                var descuento = model.Descuento; // Porcentaje de descuento desde el formulario
                                var valorDescuento = precioUnitario * (descuento / 100); // Valor del descuento

                                var oDetalle = new facturahasproducto
                                {
                                    factura_id = oFactura.id,
                                    producto_id = producto.Id,
                                    cantidad = (int)(producto.Cantidad ?? 0),
                                    precio_unitario = precioUnitario,
                                    @base = precioUnitario,
                                    iva = 19, // Asegúrate de que este valor es correcto
                                    valor_iva = valorIva,
                                    descuento = descuento,
                                    valor_descuento = valorDescuento,
                                    precio_total = (producto.Cantidad ?? 0) * (precioUnitario + valorIva - valorDescuento)
                                };

                                db.facturahasproducto.Add(oDetalle);
                            }

                            db.SaveChanges();
                            transaction.Commit();

                            // Redirigir a la vista de índice de facturas
                            return Json(new { success = true, redirectUrl = Url.Action("Index", "Factura") });
                        }
                        catch (DbUpdateException dbEx)
                        {
                            transaction.Rollback();
                            var errorMessage = "Se produjo un error al actualizar la base de datos.";
                            if (dbEx.InnerException != null)
                            {
                                errorMessage += $" Detalles: {dbEx.InnerException.Message}";
                                // Muestra detalles internos en la consola del navegador
                                Console.WriteLine($"Error de base de datos: {dbEx.InnerException.Message}");
                            }
                            return Json(new { success = false, errors = new[] { errorMessage } });
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            return Json(new { success = false, errors = new[] { $"Se produjo un error al guardar los detalles de la factura: {ex.Message}" } });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, errors = new[] { "Se produjo un error al guardar la factura: " + ex.Message } });
            }
        }
    }
}