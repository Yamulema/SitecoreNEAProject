using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Items;

namespace Neambc.Neamb.Feature.Product.Interfaces
{
    public interface IStepperService
    {
        /// <summary>
        /// Runs all the enabled steps for a given Page Item.
        /// </summary>
        /// <param name="item"></param>
        void Run(Item item);
    }
}