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
    
    public partial class fa_function
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public fa_function()
        {
            this.fa_role = new HashSet<fa_role>();
        }
    
        public int ID { get; set; }
        public string REMARK { get; set; }
        public string FULL_NAME { get; set; }
        public string NAMESPACE { get; set; }
        public string CLASS_NAME { get; set; }
        public string METHOD_NAME { get; set; }
        public string DLL_NAME { get; set; }
        public string XML_NOTE { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<fa_role> fa_role { get; set; }
    }
}
