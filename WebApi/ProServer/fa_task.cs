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
    
    public partial class fa_task
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public fa_task()
        {
            this.fa_task_flow = new HashSet<fa_task_flow>();
        }
    
        public int ID { get; set; }
        public Nullable<int> FLOW_ID { get; set; }
        public string TASK_NAME { get; set; }
        public Nullable<System.DateTime> CREATE_TIME { get; set; }
        public Nullable<int> CREATE_USER { get; set; }
        public string CREATE_USER_NAME { get; set; }
        public string STATUS { get; set; }
        public Nullable<System.DateTime> STATUS_TIME { get; set; }
        public string REMARK { get; set; }
        public string REGION { get; set; }
        public string KEY_ID { get; set; }
        public Nullable<System.DateTime> START_TIME { get; set; }
        public Nullable<System.DateTime> END_TIME { get; set; }
        public Nullable<System.DateTime> DEAL_TIME { get; set; }
        public string ROLE_ID_STR { get; set; }
    
        public virtual fa_flow fa_flow { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<fa_task_flow> fa_task_flow { get; set; }
    }
}
