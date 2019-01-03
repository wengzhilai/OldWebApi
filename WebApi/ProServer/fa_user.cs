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
    
    public partial class fa_user
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public fa_user()
        {
            this.fa_district1 = new HashSet<fa_district>();
            this.fa_user_info1 = new HashSet<fa_user_info>();
            this.fa_module = new HashSet<fa_module>();
            this.fa_role = new HashSet<fa_role>();
        }
    
        public int ID { get; set; }
        public string NAME { get; set; }
        public string LOGIN_NAME { get; set; }
        public Nullable<int> ICON_FILES_ID { get; set; }
        public int DISTRICT_ID { get; set; }
        public Nullable<decimal> IS_LOCKED { get; set; }
        public Nullable<System.DateTime> CREATE_TIME { get; set; }
        public Nullable<int> LOGIN_COUNT { get; set; }
        public Nullable<System.DateTime> LAST_LOGIN_TIME { get; set; }
        public Nullable<System.DateTime> LAST_LOGOUT_TIME { get; set; }
        public Nullable<System.DateTime> LAST_ACTIVE_TIME { get; set; }
        public string REMARK { get; set; }
        public string REGION { get; set; }
    
        public virtual fa_district fa_district { get; set; }
        public virtual fa_user_info fa_user_info { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<fa_district> fa_district1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<fa_user_info> fa_user_info1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<fa_module> fa_module { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<fa_role> fa_role { get; set; }
    }
}