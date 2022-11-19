using System;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.Rakuten.Manager;
using Neambc.Neamb.Feature.Rakuten.Model;
using System.Collections.Generic;
using System.Linq;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Sitecore.Data;
using Sitecore.Data.Fields;

namespace Neambc.Neamb.Feature.Rakuten.Repositories
{
    [Service(typeof(ICategoryRepository))]
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IGlobalConfigurationManager _globalConfigurationManager;
        private readonly IRakutenLog _rakutenLog;

        public CategoryRepository(IGlobalConfigurationManager globalConfigurationManager,
            IRakutenLog rakutenLog)
        {
            _globalConfigurationManager = globalConfigurationManager;
            _rakutenLog = rakutenLog;
        }

        public List<Category> GetCategories()
        {
            try
            {
                var allCategories = new List<Category>();
                var categories = Sitecore.Context.Database.GetItem(new ID(_globalConfigurationManager.RakutenCategoriesParentID));
                if (categories == null) return allCategories;

                foreach (Sitecore.Data.Items.Item item in categories.Children)
                {
                    var category = new Category
                    {
                        Guid = item.ID.ToGuid(),
                        Id = item.Fields[Foundation.Rakuten.Templates.RakutenCategory.Fields.Id].Value,
                        Name = item.Fields[Foundation.Rakuten.Templates.RakutenCategory.Fields.Name].Value,
                        NeambEnabled = ((CheckboxField)item.Fields[Foundation.Rakuten.Templates.RakutenCategory.Fields.NeambEnable]).Checked,
                        SeiumbEnabled = ((CheckboxField)item.Fields[Foundation.Rakuten.Templates.RakutenCategory.Fields.SeiumbEnable]).Checked,
                        Subcategories = new List<Category>()
                    };

                    category.Subcategories.Add(new Category
                    {
                        Id = category.Id,
                        Guid = category.Guid,
                        Name = "All " + category.Name,
                        NeambEnabled = category.NeambEnabled,
                        SeiumbEnabled = category.SeiumbEnabled
                    });

                    category.Subcategories.AddRange(item.Children.Select(subcategory => new Category
                    {
                        Guid = subcategory.ID.ToGuid(),
                        Id = subcategory.Fields[Foundation.Rakuten.Templates.RakutenCategory.Fields.Id].Value,
                        Name = subcategory.Fields[Foundation.Rakuten.Templates.RakutenCategory.Fields.Name].Value,
                        NeambEnabled = ((CheckboxField)subcategory.Fields[Foundation.Rakuten.Templates.RakutenCategory.Fields.NeambEnable]).Checked,
                        SeiumbEnabled = ((CheckboxField)subcategory.Fields[Foundation.Rakuten.Templates.RakutenCategory.Fields.SeiumbEnable]).Checked,
                    }).ToList());

                    allCategories.Add(category);
                }
                return allCategories;
            }
            catch (Exception ex)
            {
                _rakutenLog.Error("CategoryRepository - GetCategories", ex);
                return null;
            }
        }
    }
}