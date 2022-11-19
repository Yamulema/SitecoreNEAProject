using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RestSharp;

namespace Neambc.Neamb.Foundation.Rakuten.Manager
{
    public interface IStoreProcessChecker {
        string GetEtag();
        bool CanContinueImportProcess(Parameter etagFromApi);
        void SaveEtagCache(Parameter etagFromApi);
    }
}