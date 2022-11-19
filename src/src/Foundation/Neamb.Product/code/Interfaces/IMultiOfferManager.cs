using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using Neambc.Neamb.Foundation.MBCData.Model;
using Neambc.Neamb.Foundation.Product.Model;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Neambc.Neamb.Foundation.Product.Interfaces
{
    public interface IMultiOfferManager
    {
        List<ProductMultiOfferRadioOptionGroup> GetRadioButtonForm(Item multiproductItem, NameValueCollection queryStringValues);
        Dictionary<string, string> GetPostParamsValues(Item renderingItem, ID fieldPostParametersId);
        string HandleRedirectUrlForLoginNotAuthenticated(string requestPage, string absolutePath);
        void SaveRedirectPreviousPage(string redirectPage);
        string GetProductId(Item multiOfferItem, string dataform);
        string GetRedirectPreviousPage();
    }
}