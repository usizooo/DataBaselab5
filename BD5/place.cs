//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BD5
{
    using System;
    using System.Collections.Generic;
    
    public partial class place
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public place()
        {
            this.availablegames = new HashSet<availablegames>();
            this.orders = new HashSet<orders>();
            this.technicalproblems = new HashSet<technicalproblems>();
        }
    
        public int ID_place { get; set; }
        public int specifications_ID { get; set; }
        public string nameofcomputer { get; set; }
        public string employmentstatus { get; set; }
        public int price { get; set; }
        public int devices_ID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<availablegames> availablegames { get; set; }
        public virtual devices devices { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<orders> orders { get; set; }
        public virtual specifications specifications { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<technicalproblems> technicalproblems { get; set; }
    }
}
