using Neambc.Neamb.Foundation.Product.Model;
using Sitecore.Data.Items;
using System.Collections.Generic;
using System.Collections.Specialized;
using Sitecore.Foundation.SitecoreExtensions.Extensions;
using Sitecore.Data;
using System;
using System.Linq;
using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Neamb.Foundation.Product.Interfaces;
using Neambc.Seiumb.Foundation.Sitecore.Utility;
using Sitecore.Links;
using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Seiumb.Foundation.Sitecore;

namespace Neambc.Neamb.Foundation.Product.Manager
{
    [Service(typeof(IMultiOfferManager))]
    public class MultiOfferManager: IMultiOfferManager
    {
        private readonly ISessionAuthenticationManager _sessionAuthenticationManager;
        private readonly ILinkActionTypeManager _linkActionTypeManager;
        private readonly ISessionManager _sessionManager;
        private readonly IAuthenticationAccountManager _authenticationAccountManager;
        private readonly ILog _log;

        public MultiOfferManager (ISessionAuthenticationManager sessionAuthenticationManager, 
            ILinkActionTypeManager linkActionTypeManager,
            ISessionManager sessionManager,
            IAuthenticationAccountManager authenticationAccountManager,
            ILog log
            )
        {
            _sessionAuthenticationManager = sessionAuthenticationManager;
            _linkActionTypeManager = linkActionTypeManager;
            _sessionManager = sessionManager;
            _authenticationAccountManager = authenticationAccountManager;
            _log = log;
        }
        /// <summary>
        /// Get the radio buttons form configured in Sitecore and match with the query string values sent in the URL
        /// </summary>
        /// <param name="multiproductItem">Multi product page</param>
        /// <param name="queryStringValues">Request query parameters in the URL</param>
        /// <returns></returns>
        public List<ProductMultiOfferRadioOptionGroup> GetRadioButtonForm(Item multiproductItem, 
            NameValueCollection queryStringValues)
        {
            if (multiproductItem == null)
            {
                throw new ArgumentException("MultiOfferManager, GetRadioButtonForm error multiproductItem null", "multiproductItem");
            }
            
            //Get the radio button groups configured in Sitecore and select in the multi offer product page.
            int counter = 1;
            var radioOptions = ((Sitecore.Data.Fields.MultilistField)multiproductItem.Fields[Templates.ProductMultiOffer.Fields.RadioGroups]).GetItems();
            var groups = new List<ProductMultiOfferRadioOptionGroup>();
            bool foundRadioChecked = false;

            if (radioOptions == null || radioOptions.Count()<1)
            {
                throw new ArgumentException("MultiOfferManager, GetRadioButtonForm error radioOptions null or less that one", "radioOptions");
            }
            int i = 0;
            //Foreach groups selected in the page
            foreach (var itemRadioOptionGroup in radioOptions)
            {
                foundRadioChecked = false;
                //Set the group radio button properties
                ProductMultiOfferRadioOptionGroup group = new ProductMultiOfferRadioOptionGroup
                {
                    RadioGroupDescription = itemRadioOptionGroup[Templates.PmoRadioButtonGroup.Fields.GroupDescription],
                    RadioGroupParameter = itemRadioOptionGroup[Templates.PmoRadioButtonGroup.Fields.GroupParameter],
                    Item = itemRadioOptionGroup,
                    GroupId = $"group{counter}"
                };

                List<ProductMultiOfferRadioOption> radioOptionsList = new List<ProductMultiOfferRadioOption>();
                //Get children for each group item to get the radio button options
                var radioOptionItems = itemRadioOptionGroup.GetChildren();
                //Foreach radio option item
                i = 0;
                foreach (Item radioOptionItem in radioOptionItems)
                {
                    i++;
                    //Set the radio buttion properties
                    ProductMultiOfferRadioOption radioOption = new ProductMultiOfferRadioOption
                    {
                        RadioDisplayText = radioOptionItem[Templates.PmoRadioButtonOption.Fields.RadioDisplayText],
                        RadioValueText = radioOptionItem[Templates.PmoRadioButtonOption.Fields.RadioValueText],
                        RadioIdentifier= $"{itemRadioOptionGroup[Templates.PmoRadioButtonGroup.Fields.GroupParameter]}{i}",
                        IsDefaulSelectedContent = radioOptionItem.Fields[Templates.PmoRadioButtonOption.Fields.DefaultSelection].IsChecked(),
                        ParameterId = radioOptionItem[Templates.PmoRadioButtonOption.Fields.ParameterId],
                        ParameterMatchValue = radioOptionItem[Templates.PmoRadioButtonOption.Fields.ParameterMatchValue],
                        Item = radioOptionItem
                    };
                    //Get the query parameter sent in the url to set the radio button as checked.
                    if (queryStringValues != null)
                    {
                        var parameterIdUrl = queryStringValues[radioOptionItem[Templates.PmoRadioButtonOption.Fields.ParameterId]];
                        if (parameterIdUrl == radioOptionItem[Templates.PmoRadioButtonOption.Fields.ParameterMatchValue] && !foundRadioChecked)
                        {
                            foundRadioChecked = true;
                            radioOption.IsDefaulSelected = true;
                        }
                    }
                    radioOptionsList.Add(radioOption);

                }
                group.RadioOptions = radioOptionsList;
                //See if there is radio button by default
                group.IsMandatory= radioOptionsList.Count(item => item.IsDefaulSelectedContent)==0;
                groups.Add(group);
            }
            return groups;
        }

        /// <summary>
        /// Get the post parameter values with the token replacement
        /// </summary>
        /// <param name="renderingItem">Multiproduct page item</param>
        /// <param name="fieldPostParametersId">Post parameter field id</param>
        /// <returns>Post parameter with keys and values</returns>
        public Dictionary<string, string> GetPostParamsValues(Item renderingItem, ID fieldPostParametersId)
        {
            if (renderingItem == null)
            {
                throw new ArgumentException("MultiOfferManager, GetPostParamsValues error renderingItem null", "renderingItem");
            }
            if (fieldPostParametersId == ID.Null || fieldPostParametersId == ID.Undefined)
            {
                throw new ArgumentException("MultiOfferManager, GetPostParamsValues error fieldPostParametersId null", "fieldPostParametersId");
            }
            var parameters = new Dictionary<string, string>();
            var accountMembership = _sessionAuthenticationManager.GetAccountMembership();
            var rawText = renderingItem[fieldPostParametersId] ?? string.Empty;
            var textField = rawText.Replace("{", String.Empty).Replace("}", String.Empty);

            //Process the post parameter lines to replace the values
            foreach (var entry in textField.Split(new[] { "\r\n" }, StringSplitOptions.None))
            {
                var parameter = entry.Split(new[] { ":" }, StringSplitOptions.None);
                if (parameter.Length == 2)
                    parameters.Add(parameter.First(), _linkActionTypeManager.ReplaceToken(parameter.Last(), accountMembership.Mdsid, null));
            }

            return parameters;
        }

        /// <summary>
        /// In case not authenticated redirect to the login page and return to the previous page once authenticated
        /// </summary>
        /// <param name="requestPage">Previous page full url</param>
        /// <param name="absolutePath">Previous page absolute url</param>
        /// <returns>Login page</returns>
        public string HandleRedirectUrlForLoginNotAuthenticated(string requestPage, string absolutePath)
        {
            if (string.IsNullOrEmpty(requestPage))
            {
                throw new ArgumentException("MultiOfferManager, HandleRedirectUrlForLoginNotAuthenticated error requestPage null", "requestPage");
            }
            if (string.IsNullOrEmpty(absolutePath))
            {
                throw new ArgumentException("MultiOfferManager, HandleRedirectUrlForLoginNotAuthenticated error absolutePath null", "absolutePath");
            }

            //Remove the ref parameter
            var depuredUrl = QueryStringHelper.RemoveQueryStringByKey(requestPage, "ref");

            //Save in session the previous page
            _sessionAuthenticationManager.SaveRequestedPageLogin(depuredUrl);
            _sessionAuthenticationManager.SaveRequestedPageLoginAbsolutePath(absolutePath);
            //Get the login page to return it as string
            var pathLoginPage = LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(Templates.LoginPage.ID));
            return $"{pathLoginPage}?multioffer=true";

        }

        /// <summary>
        /// Save the previous page that call the product multi offer
        /// </summary>
        /// <param name="redirectPage">Previous page url</param>
        public void SaveRedirectPreviousPage(string redirectPage)
        {
            if (string.IsNullOrEmpty(redirectPage))
            {
                throw new ArgumentException("MultiOfferManager, SaveRedirectPreviousPage error redirectPage null", "redirectPage");
            }
            var isValidPage = _authenticationAccountManager.IsValidRedirection(redirectPage);
            if (isValidPage)
            {
                _sessionManager.StoreInSession<string>(ConstantsNeamb.RedirectUrlMultioffer, redirectPage);
            }
        }

        /// <summary>
        /// Get the product id that is required to send as post parameter to the partner url
        /// </summary>
        /// <param name="multiOfferItem">Multi offer product page id</param>
        /// <param name="dataform">Form values with the radio button values selected on it</param>
        /// <returns>Product id</returns>
        public string GetProductId(Item multiOfferItem, string dataform)
        {
            if (multiOfferItem == null)
            {
                throw new ArgumentException("MultiOfferManager, GetProductId error multiOfferItem null", "multiOfferItem");
            }
            if (string.IsNullOrEmpty(dataform))
            {
                throw new ArgumentException("MultiOfferManager, GetProductId error dataform null", "dataform");
            }

            var valuesForm = dataform.Split('&');
            string calculatedProductIdValue = "";
            string productParamValue = "";

            var mapping = new List<ProductMultiOfferMappingItem>();
            //Get the product mapping field select on the multi offer product page
            var productMapping = ((Sitecore.Data.Fields.MultilistField)multiOfferItem.Fields[Templates.ProductMultiOffer.Fields.ProductMapping]).GetItems();
            foreach (Item productMappingItem in productMapping)
            {
                ProductMultiOfferMappingItem productMultiOfferMapping = new ProductMultiOfferMappingItem
                {
                    CalculatedProductId = productMappingItem[Templates.ProductMappingOption.Fields.CalculatedProductId],
                    ResultProductId = productMappingItem[Templates.ProductMappingOption.Fields.ResultProductId],
                    ProductParam = productMappingItem[Templates.ProductMappingOption.Fields.ProductParams],
                };
                mapping.Add(productMultiOfferMapping);
            }
            //Get the radio buttons configured
            var radioButtonGroups= GetRadioButtonForm(multiOfferItem, null);
            foreach (var radioButtonGroupItem in radioButtonGroups)
            {
                var valueFormFound = valuesForm.FirstOrDefault(item=> item.StartsWith($"{radioButtonGroupItem.RadioGroupParameter}="));
                if(!string.IsNullOrEmpty(valueFormFound))
                {
                    string[] parameter = valueFormFound.Split(new[] { "=" }, StringSplitOptions.None);
                    if (parameter.Length == 2)
                    {
                        var radioFound =radioButtonGroupItem.RadioOptions.FirstOrDefault(item => item.ParameterId== parameter[0] && item.RadioValueText == parameter[1]);
                        calculatedProductIdValue = $"{calculatedProductIdValue}[{radioFound.RadioValueText}]";
                        productParamValue = $"{productParamValue}[{parameter[0]}]";
                    }
                }
                else
                {
                    var radioDefault=radioButtonGroupItem.RadioOptions.FirstOrDefault(item=> item.IsDefaulSelectedContent);
                    if(radioDefault!=null)
                    {
                        calculatedProductIdValue = $"{calculatedProductIdValue}[{radioDefault.RadioValueText}]";
                        productParamValue = $"{productParamValue}[{radioDefault.ParameterId}]";
                    }
                    else
                    {
                        //case no default in content and no form post value
                        return null;
                    }
                }
            }
            
            //Get the mapping according the value form option names and form option values
            var foundMapping = mapping.FirstOrDefault(item => item.CalculatedProductId == calculatedProductIdValue && item.ProductParam == productParamValue);
            if (foundMapping != null)
            {
                return foundMapping.ResultProductId;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get from session the previous page url that called the product multi offer page
        /// </summary>
        /// <returns>Previous page url</returns>
        public string GetRedirectPreviousPage()
        {
            //Get from session. In case that is null then return the Home page url
            var redirectPage = _sessionManager.RetrieveFromSession<string>(ConstantsNeamb.RedirectUrlMultioffer);

            if (!string.IsNullOrEmpty(redirectPage))
            {
                return redirectPage;

            }
            else
            {
                var startItemPath = LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(Templates.HomePage.ID));
                return startItemPath;
            }
        }
    }
}