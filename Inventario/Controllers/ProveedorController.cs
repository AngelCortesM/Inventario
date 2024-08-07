using Inventario.Models;
using Inventario.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Inventario.Controllers
{
    public class ProveedorController : Controller
    {
        // GET: Proveedor
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult List()
        {
            List<ListProveedorViewModel> lst = new List<ListProveedorViewModel>();
            using (CrudMVCRazorEntities db =
                new CrudMVCRazorEntities())
            {
                lst =
                   (from d in db.proveedor

                    select new ListProveedorViewModel
                    {
                        Id = d.id,
                        Nit = d.nit,
                        Nombre = d.nombre,
                        Celular = d.celular,
                        Direccion = d.direccion,
                        Email = d.email,
                        Telefono_fijo = d.telefono_fijo,
                        Sitio_web = d.sitio_web,
                        Ciudad = d.ciudad,
                        Pais = d.pais,
                        Fecha_registro = d.fecha_registro

                    }).ToList();

            }
            return View(lst);
        }

        public ActionResult New()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Save(ProveedorViewModel model)
        {
            try
            {
                using (CrudMVCRazorEntities db = new CrudMVCRazorEntities())
                {
                    var oProveedor = new proveedor();
                    // Asignación de propiedades del modelo al objeto proveedor

                    oProveedor.nit = model.Nit;
                    oProveedor.nombre = model.Nombre;
                    oProveedor.celular = model.Celular;
                    oProveedor.direccion = model.Direccion;
                    
                 
                    oProveedor.email = model.Email;
                    oProveedor.telefono_fijo = model.Telefono_fijo;
                    oProveedor.sitio_web = model.Sitio_web;
                    oProveedor.ciudad = model.Ciudad;
                    oProveedor.pais = model.Pais;
                    oProveedor.fecha_registro = model.Fecha_registro;


                    // Añadir el proveedor a la base de datos
                    db.proveedor.Add(oProveedor);
                    db.SaveChanges();
                }

                return Content("1");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }

        }

        public ActionResult Edit(int Id)
        {
            ProveedorViewModel model = new ProveedorViewModel();
            using (CrudMVCRazorEntities db = new CrudMVCRazorEntities())
            {
                var oProveedor = db.proveedor.Find(Id);
                model.Nit = oProveedor.nit;
                model.Nombre = oProveedor.nombre;
                model.Celular = oProveedor.celular;
                model.Direccion = oProveedor.direccion;
                
            
                model.Email = oProveedor.email;
                model.Telefono_fijo = oProveedor.telefono_fijo;
                model.Sitio_web = oProveedor.sitio_web;
                model.Ciudad = oProveedor.ciudad;
                model.Pais = oProveedor.pais;
                model.Fecha_registro = oProveedor.fecha_registro;
                model.Id = oProveedor.id;



            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Update(ProveedorViewModel model)
        {
            try
            {
                using (CrudMVCRazorEntities db = new CrudMVCRazorEntities())
                {
                    var oProveedor = db.proveedor.Find(model.Id);
                    // Asignación de propiedades del modelo al objeto proveedor

                    oProveedor.nit = model.Nit;
                    oProveedor.nombre = model.Nombre;
                    oProveedor.celular = model.Celular;
                    oProveedor.direccion = model.Direccion;
                    oProveedor.email = model.Email;
                    oProveedor.telefono_fijo = model.Telefono_fijo;
                    oProveedor.sitio_web = model.Sitio_web;
                    oProveedor.ciudad = model.Ciudad;
                    oProveedor.pais = model.Pais;
                    oProveedor.fecha_registro = model.Fecha_registro;

                    // Añadir el proveedor a la base de datos
                    db.Entry(oProveedor).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
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
                    var oProveedor = db.proveedor.Find(Id);
                    // Asignación de propiedades del modelo al objeto proveedor


                    db.proveedor.Remove(oProveedor);
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