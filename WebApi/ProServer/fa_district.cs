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
    
    public partial class fa_district
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public fa_district()
        {
            this.fa_district1 = new HashSet<fa_district>();
            this.fa_user = new HashSet<fa_user>();
            this.fa_user1 = new HashSet<fa_user>();
        }
    
        public int ID { get; set; }
        public Nullable<int> PARENT_ID { get; set; }
        public string NAME { get; set; }
        public string CODE { get; set; }
        public decimal IN_USE { get; set; }
        public int LEVEL_ID { get; set; }
        public string ID_PATH { get; set; }
        public string REGION { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<fa_district> fa_district1 { get; set; }
        public virtual fa_district fa_district2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<fa_user> fa_user { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<fa_user> fa_user1 { get; set; }
    }
}
