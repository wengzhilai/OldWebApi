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
    
    public partial class fa_files
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public fa_files()
        {
            this.fa_bulletin = new HashSet<fa_bulletin>();
            this.fa_user_event = new HashSet<fa_user_event>();
            this.fa_task_flow_handle = new HashSet<fa_task_flow_handle>();
        }
    
        public int ID { get; set; }
        public string NAME { get; set; }
        public string PATH { get; set; }
        public Nullable<int> USER_ID { get; set; }
        public int LENGTH { get; set; }
        public Nullable<System.DateTime> UPLOAD_TIME { get; set; }
        public string REMARK { get; set; }
        public string URL { get; set; }
        public string FILE_TYPE { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<fa_bulletin> fa_bulletin { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<fa_user_event> fa_user_event { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<fa_task_flow_handle> fa_task_flow_handle { get; set; }
    }
}
