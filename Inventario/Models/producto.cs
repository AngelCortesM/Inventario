//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Inventario.Models
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public partial class producto
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public producto()
        {
            this.facturahasproducto = new HashSet<facturahasproducto>();
        }
    
        public Nullable<decimal> cantidad { get; set; }
        public int id { get; set; }
        public Nullable<int> proveedor_id { get; set; }
        public Nullable<decimal> valor { get; set; }
        public string descripcion { get; set; }
        public string estado { get; set; }
        public Nullable<System.DateTime> fecha { get; set; }
        public string nombre { get; set; }
        public string barras { get; set; }
        public string categoria { get; set; }
        public Nullable<decimal> costo { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<facturahasproducto> facturahasproducto { get; set; }
        public virtual proveedor proveedor { get; set; }

        internal static object Select(Func<object, SelectListItem> value)
        {
            throw new NotImplementedException();
        }
    }
}
