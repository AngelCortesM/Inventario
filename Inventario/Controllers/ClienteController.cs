using Inventario.Models;
using Inventario.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Inventario.Controllers
{
    public class ClienteController : Controller
    {
        // GET: Cliente
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult List()
        {
            List<ListClienteViewModel> lst;

            using (CrudMVCRazorEntities db = new CrudMVCRazorEntities())
            {
                lst = (from d in db.cliente
                       select new ListClienteViewModel
                       {
                           Id = d.id,
                           Cedula = d.cedula,
                           Direccion = d.direccion,
                           Nombre = d.nombre,
                           Telefono = d.telefono,
                           Apellido = d.apellido,
                           Email = d.email,
                           Fecha = d.fecha,
                           FechaUp = d.fechaup
                       }).ToList();
            }

            // Verificar si la lista está vacía
            if (!lst.Any())
            {
                // Si la lista está vacía, puedes redirigir a otra acción, mostrar un mensaje, etc.
 
                ViewBag.Message = "No hay clientes disponibles.";
                return View("New");
            }

            return View(lst);
        }


        public ActionResult New()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Save(ClienteViewModel model)
        {
            try
            {
                using (CrudMVCRazorEntities db = new CrudMVCRazorEntities())
                {
                    var oCliente = new cliente();
                    // Asignación de propiedades del modelo al objeto cliente
              
                    oCliente.cedula = model.Cedula;
                    oCliente.direccion = model.Direccion;
                    oCliente.nombre = model.Nombre;
                    oCliente.telefono = model.Telefono;
                    oCliente.apellido = model.Apellido;
                    oCliente.email = model.Email;
                    oCliente.fecha = model.Fecha;
                    oCliente.fechaup = model.FechaUp;

                    // Añadir el cliente a la base de datos
                    db.cliente.Add(oCliente);
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
            ClienteViewModel model = new ClienteViewModel();
            using (CrudMVCRazorEntities db = new CrudMVCRazorEntities())
            {
                var oCliente = db.cliente.Find(Id);
                model.Cedula = oCliente.cedula;
                model.Direccion = oCliente.direccion;
                model.Nombre = oCliente.nombre;
                model.Telefono = oCliente.telefono;
                model.Apellido = oCliente.apellido;
                model.Email = oCliente.email;
             
                model.FechaUp = oCliente.fechaup;
                model.Id = oCliente.id;


            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Update(ClienteViewModel model)
        {
            try
            {
                using (CrudMVCRazorEntities db = new CrudMVCRazorEntities())
                {
                    var oCliente = db.cliente.Find(model.Id);
                    // Asignación de propiedades del modelo al objeto cliente

                    oCliente.cedula = model.Cedula;
                    oCliente.direccion = model.Direccion;
                    oCliente.nombre = model.Nombre;
                    oCliente.telefono = model.Telefono;
                    oCliente.apellido = model.Apellido;
                    oCliente.email = model.Email;
              
                    oCliente.fechaup = model.FechaUp;

                    // Añadir el cliente a la base de datos
                    db.Entry(oCliente).State = System.Data.Entity.EntityState.Modified;
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
                    var oCliente = db.cliente.Find(Id);
                    // Asignación de propiedades del modelo al objeto cliente


                    db.cliente.Remove(oCliente);
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