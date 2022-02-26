using System;
using System.Collections.Generic;
using System.Text;

namespace WinDynamicDesktop.Core.Builders
{
    public class PageBuilder<TInterface>
    {
        public TInterface Query(TInterface t)
        {
            return t;
        }
    }
}
