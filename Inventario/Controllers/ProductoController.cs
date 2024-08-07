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
                     join p in db.proveedor on d.proveedor_id equals p.id into proveedores
                     from proveedor in proveedores.DefaultIfEmpty() // Para manejar casos donde no hay proveedor
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
                        Proveedor_id = d.proveedor_id,
                         ProveedorNombre = proveedor != null ? proveedor.nombre : "No disponible"
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
        //editar
        public ActionResult Edit(int Id)
        {
            ProductoViewModel model = new ProductoViewModel();
            using (CrudMVCRazorEntities db = new CrudMVCRazorEntities())
            {
                var oProducto = db.producto.Find(Id);
                if (oProducto != null)
                {
                    model.Cantidad = oProducto.cantidad;
                    model.Proveedor_id = oProducto.proveedor_id;
                    model.Valor = oProducto.valor;
                    model.Descripcion = oProducto.descripcion;
                    model.Estado = oProducto.estado;
                    model.Fecha = oProducto.fecha;
                    model.Nombre = oProducto.nombre;
                    model.Barras = oProducto.barras;
                    model.Categoria = oProducto.categoria;
                    model.Costo = oProducto.costo;
                    model.Id = oProducto.id;
                }


            }
            return View(model);
        }


        [HttpPost]
        public ActionResult Update(ProductoViewModel model)
        {
            try
            {
                using (CrudMVCRazorEntities db = new CrudMVCRazorEntities())
                {
                    // Encuentra el producto por su Id
                    var oProducto = db.producto.Find(model.Id);

                    // Verifica si el producto fue encontrado
                    if (oProducto != null)
                    {
                        // Asignación de propiedades del modelo al objeto producto
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

                        // Marca el objeto producto como modificado
                        db.Entry(oProducto).State = System.Data.Entity.EntityState.Modified;

                        // Guarda los cambios en la base de datos
                        db.SaveChanges();
                    }
                }
                    return Content("1");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult Delete(int Id)
        {
            try
            {
                using (CrudMVCRazorEntities db = new CrudMVCRazorEntities())
                {
                    var oProducto = db.producto.Find(Id);
                    // Asignación de propiedades del modelo al objeto proveedor


                    db.producto.Remove(oProducto);
                    db.SaveChanges();
                }

                return Content("1");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
    }
}