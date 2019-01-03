using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProInterface
{
    public interface IConfig : IZ_Config
    {
        Dictionary<int,string> ConfigGetSelectListItem(string code);
    }
}
