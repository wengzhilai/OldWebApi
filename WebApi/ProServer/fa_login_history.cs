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
    
    public partial class fa_login_history
    {
        public int ID { get; set; }
        public Nullable<int> USER_ID { get; set; }
        public Nullable<System.DateTime> LOGIN_TIME { get; set; }
        public string LOGIN_HOST { get; set; }
        public Nullable<System.DateTime> LOGOUT_TIME { get; set; }
        public Nullable<int> LOGIN_HISTORY_TYPE { get; set; }
        public string MESSAGE { get; set; }
    }
}