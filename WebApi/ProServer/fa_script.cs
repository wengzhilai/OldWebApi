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
    
    public partial class fa_script
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public fa_script()
        {
            this.fa_script_group_list = new HashSet<fa_script_group_list>();
            this.fa_script_task = new HashSet<fa_script_task>();
        }
    
        public int ID { get; set; }
        public string CODE { get; set; }
        public string NAME { get; set; }
        public string BODY_TEXT { get; set; }
        public string BODY_HASH { get; set; }
        public string RUN_WHEN { get; set; }
        public string RUN_ARGS { get; set; }
        public string RUN_DATA { get; set; }
        public string STATUS { get; set; }
        public string DISABLE_REASON { get; set; }
        public string SERVICE_FLAG { get; set; }
        public string REGION { get; set; }
        public decimal IS_GROUP { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<fa_script_group_list> fa_script_group_list { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<fa_script_task> fa_script_task { get; set; }
    }
}