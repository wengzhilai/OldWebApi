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
    
    public partial class fa_script_task_log
    {
        public int ID { get; set; }
        public int SCRIPT_TASK_ID { get; set; }
        public System.DateTime LOG_TIME { get; set; }
        public decimal LOG_TYPE { get; set; }
        public string MESSAGE { get; set; }
        public string SQL_TEXT { get; set; }
    
        public virtual fa_script_task fa_script_task { get; set; }
    }
}
