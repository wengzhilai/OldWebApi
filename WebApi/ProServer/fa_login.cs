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
    
    public partial class fa_login
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public fa_login()
        {
            this.fa_oauth = new HashSet<fa_oauth>();
        }
    
        public int ID { get; set; }
        public string LOGIN_NAME { get; set; }
        public string PASSWORD { get; set; }
        public string PHONE_NO { get; set; }
        public string EMAIL_ADDR { get; set; }
        public string VERIFY_CODE { get; set; }
        public Nullable<System.DateTime> VERIFY_TIME { get; set; }
        public Nullable<decimal> IS_LOCKED { get; set; }
        public Nullable<System.DateTime> PASS_UPDATE_DATE { get; set; }
        public string LOCKED_REASON { get; set; }
        public string REGION { get; set; }
        public Nullable<int> FAIL_COUNT { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<fa_oauth> fa_oauth { get; set; }
    }
}
