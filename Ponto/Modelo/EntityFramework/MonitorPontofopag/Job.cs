//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Modelo.EntityFramework.MonitorPontofopag
{
    using System;
    using System.Collections.Generic;
    
    public partial class Job
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Job()
        {
            this.JobParameter = new HashSet<JobParameter>();
            this.State = new HashSet<State>();
        }
    
        public int Id { get; set; }
        public Nullable<int> StateId { get; set; }
        public string StateName { get; set; }
        public string InvocationData { get; set; }
        public string Arguments { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public Nullable<System.DateTime> ExpireAt { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<JobParameter> JobParameter { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<State> State { get; set; }
    }
}
