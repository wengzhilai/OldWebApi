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
    
    public partial class fa_role_query_authority
    {
        public int ROLE_ID { get; set; }
        public int QUERY_ID { get; set; }
        public string NO_AUTHORITY { get; set; }
    
        public virtual fa_query fa_query { get; set; }
        public virtual fa_role fa_role { get; set; }
    }
}
