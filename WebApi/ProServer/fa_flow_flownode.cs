//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProServer
{
    using System;
    using System.Collections.Generic;
    
    public partial class fa_flow_flownode
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public fa_flow_flownode()
        {
            this.fa_flow_flownode_flow = new HashSet<fa_flow_flownode_flow>();
        }
    
        public int ID { get; set; }
        public string NAME { get; set; }
        public string HANDLE_URL { get; set; }
        public string SHOW_URL { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<fa_flow_flownode_flow> fa_flow_flownode_flow { get; set; }
    }
}
