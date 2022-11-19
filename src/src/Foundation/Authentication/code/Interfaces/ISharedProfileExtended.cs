using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Seiumb.Foundation.Authentication.Interfaces
{
    public interface ISharedProfileExtended
    {
        T GetCustomComplexProperty<T>(string propertyName);
        void SetCustomComplexProperty<T>(string propertyName, T element, bool verifyExistence = false);
    }
}