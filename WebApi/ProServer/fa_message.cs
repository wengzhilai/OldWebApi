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
    
    public partial class fa_message
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public fa_message()
        {
            this.fa_user_message = new HashSet<fa_user_message>();
        }
    
        public int ID { get; set; }
        public Nullable<int> MESSAGE_TYPE_ID { get; set; }
        public Nullable<int> KEY_ID { get; set; }
        public string TITLE { get; set; }
        public string CONTENT { get; set; }
        public Nullable<System.DateTime> CREATE_TIME { get; set; }
        public string CREATE_USERNAME { get; set; }
        public Nullable<int> CREATE_USERID { get; set; }
        public string STATUS { get; set; }
        public string PUSH_TYPE { get; set; }
        public Nullable<int> DISTRICT_ID { get; set; }
        public string ALL_ROLE_ID { get; set; }
    
        public virtual fa_message_type fa_message_type { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<fa_user_message> fa_user_message { get; set; }
    }
}
