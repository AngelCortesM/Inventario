using Inventario.Models;
using Inventario.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Inventario.Controllers
{
    public class ProductoController : Controller
    {
        // GET: Producto
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List()
        {
            List<ListProductoViewModel> lst = new List<ListProductoViewModel>();
            using (CrudMVCRazorEntities db = 
                new CrudMVCRazorEntities())
            {
               lst = 
                   ( from d in db.producto
                    select new ListProductoViewModel
                    {
                        Id = d.id,
                        Cantidad = d.cantidad,
                        Valor = d.valor,
                        Descripcion = d.descripcion,
                        Estado = d.estado,
                        Fecha = d.fecha,
                        Nombre = d.nombre,
                        Barras = d.barras,
                        Categoria = d.categoria,
                        Costo = d.costo,
                        ProveedorId = d.proveedor_id
                    }).ToList();

            }
            return View(lst);
        }

        public ActionResult New() {
            return View();
                }

        // Acción para obtener la lista de proveedores
        public ActionResult GetProveedor()
        {
            using (CrudMVCRazorEntities db = new CrudMVCRazorEntities())
            {
                var proveedores = db.proveedor.Select(p => new
                {
                    Id = p.id,
                    Nombre = p.nombre
                }).ToList();

                // Devolver los proveedores en formato JSON
                return Json(proveedores, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public ActionResult Save(ProductoViewModel model)
        {
            try
            {
                using (CrudMVCRazorEntities db= new CrudMVCRazorEntities())
                {
                    var oProducto = new producto();
                    oProducto.cantidad = model.Cantidad;
                    oProducto.proveedor_id = model.Proveedor_id;
                    oProducto.valor = model.Valor;
                    oProducto.descripcion = model.Descripcion;
                    oProducto.estado = model.Estado;
                    oProducto.fecha = model.Fecha;
                    oProducto.nombre = model.Nombre;
                    oProducto.barras = model.Barras;
                    oProducto.categoria = model.Categoria;
                    oProducto.costo = model.Costo;

                    // Agregar el nuevo producto a la base de datos
                    db.producto.Add(oProducto);
                    db.SaveChanges();
                }
                return Content("1");
            }
            catch(Exception ex)
            {
                return Content(ex.Message);
            }
        }
    }
}