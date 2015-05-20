using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeTorch.Resources.Web
{
    public interface IDbResourceProvider
    {
        void ClearResourceCache();
    }
}
