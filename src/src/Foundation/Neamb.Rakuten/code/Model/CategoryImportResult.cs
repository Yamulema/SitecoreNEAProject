using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lucene.Net.Support;
using Sitecore.Data.Items;

namespace Neambc.Neamb.Foundation.Rakuten.Model
{
    public class CategoryImportResult
    {
        public List<Item> NewItems { get; set; }
        public List<Item> UpdatedItems { get; set; }
        public List<Item> DeletedItems { get; set; }
        public List<string> Errors { get; set; }
        public CategoryImportResult() {
            NewItems= new List<Item>();
            UpdatedItems = new List<Item>();
            DeletedItems = new List<Item>();
            Errors = new List<string>();
        }
    }
}