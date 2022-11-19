using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using Neambc.Neamb.Foundation.Analytics.Gtm;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Enums;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.MBCData.Model;
using Neambc.Neamb.Foundation.MBCData.Services.ProductEligibility;
using Neambc.Neamb.Foundation.MBCData.Services.SecurityManagement;
using Neambc.Neamb.Foundation.Membership.Model;
using Neambc.Neamb.Foundation.Product.Interfaces;
using Neambc.Neamb.Foundation.Product.Model;
using Neambc.Seiumb.Feature.Product.Models;
using Neambc.Seiumb.Foundation.Authentication.Constants;
using Neambc.Seiumb.Foundation.Authentication.Interfaces;
using Neambc.Seiumb.Foundation.Sitecore.Extensions;
using Neambc.Seiumb.Foundation.WebServices.Managers;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Foundation.SitecoreExtensions.Extensions;
using Sitecore.Links;
using ActionTypeEnum = Neambc.Seiumb.Feature.Product.Models.ActionTypeEnum;

namespace Neambc.Seiumb.Feature.Product.Repositories
{

    [Service(typeof(IProductRepository))]
    public class ProductRepository : IProductRepository
    {
        private readonly IPartnerFactory _partnerFactory;
        private readonly INeambServiceManager _neambServiceManager;
        private readonly IProductRestManagerSeiumb _productRestManagerSeiumb;
        private readonly IProductGtmManager _productGtmManager;
        private readonly IPdfManager _pdfManager;
        private readonly IGlobalConfigurationManager _globalConfigurationManager;
        private readonly string URLCarPartner = "/api/ProductApi/GetTrueCarPartner";
        private readonly string URLEfulfillment = " /api/ProductApi/DownloadEfulfillmentPdf";
        private readonly string _cacheKeyGroup = "Efulfillment";
        private readonly ISecurityService _securityService;
        private readonly IMdsLoggingManager _mdsLoggingManager;
        private readonly ISeiumbProfileManager _seiumbProfileManager;
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        public ProductRepository(IPartnerFactory partnerFactory, INeambServiceManager neambServiceManager, IProductGtmManager productGtmManager, IProductRestManagerSeiumb productRestManagerSeiumb,
            IPdfManager pdfManager, IGlobalConfigurationManager globalConfigurationManager,ISecurityService securityService, IMdsLoggingManager mdsLoggingManager,
            ISeiumbProfileManager seiumbProfileManager, IUserRepository userRepository)
        {
            _partnerFactory = partnerFactory;
            _neambServiceManager = neambServiceManager;
            _productGtmManager = productGtmManager;
            _productRestManagerSeiumb = productRestManagerSeiumb;
            _pdfManager = pdfManager;
            _globalConfigurationManager = globalConfigurationManager;
            _securityService = securityService;
            _mdsLoggingManager = mdsLoggingManager;
            _seiumbProfileManager = seiumbProfileManager;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Get the information to be evaluated in Product Lite section
        /// </summary>
        /// <returns></returns>
        public UserStateProduct GetUserStateProductLiteData(Item renderingItem)
        {
            var seiuProfile = _seiumbProfileManager.GetProfile();
            var productCodeItem = Context.Database.GetItem(new ID(renderingItem[Templates.ProductLite.Fields.ProgramCode]));
            var productCode = productCodeItem.Fields[Templates.ProgramCode.Fields.ProgramCode].Value;

            var productName = !string.IsNullOrEmpty(renderingItem.DisplayName) ? renderingItem.DisplayName : renderingItem.Name;
            var notCold = new List<string> { UserStatusCons.HOT, UserStatusCons.WARM_HOT, UserStatusCons.WARM_COLD }.Contains(seiuProfile.Status);
            if (notCold) ExecuteMdsLoggingProcessView(productCode, seiuProfile.MdsId);

            var userStateProduct = new UserStateProduct
            {
                ProductCode = productCode,
                UserData = new UserData(),
                AuthenticationStatus = _userRepository.GetUserStatus(),
                ContextItem = renderingItem,
                HasCheckEligibility = renderingItem.Fields[Templates.ProductLite.Fields.Eligibility].IsChecked()
            };
            //Check eligibility
            if (userStateProduct.AuthenticationStatus.Equals(UserStatusCons.COLD))
            {
                userStateProduct.LoginDesktopText = renderingItem[Templates.ProductLite.Fields.DesktopLoginButtonText];
                LinkField mobileLink = renderingItem.Fields[Templates.ProductLite.Fields.MobileLoginButton];
                if (mobileLink != null && mobileLink.TargetItem != null)
                {
                    userStateProduct.LoginMobileLinkUrl = LinkManager.GetItemUrl(mobileLink.TargetItem);
                    userStateProduct.LoginMobileText = mobileLink.Text;
                }
                LinkField registrationLink = renderingItem.Fields[Templates.ProductLite.Fields.RegistrationButton];
                if (registrationLink != null && registrationLink.TargetItem != null)
                {
                    userStateProduct.RegistrationLinkUrl = LinkManager.GetItemUrl(registrationLink.TargetItem);
                    userStateProduct.RegistrationText = registrationLink.Text;
                }
                string href = LinkManager.GetItemUrl(Context.Item);
                ProductCtaBase productGtm = new ProductCtaBase { ProductName = productName, CtaText = userStateProduct.LoginDesktopText };
                var gtmColdLogin = _productGtmManager.GetGtmFunction(ComponentTypeEnum.AnonymousSeiumb, renderingItem, href, productGtm);
                string trackingFunction = GetGoalProductTracking(renderingItem, ConstantsSeiumb.Primary);
                gtmColdLogin = $"{gtmColdLogin};{trackingFunction}";

                userStateProduct.ActionColdClick = gtmColdLogin;
                productGtm = new ProductCtaBase { ProductName = productName, CtaText = userStateProduct.RegistrationText };
                userStateProduct.ActionColdClickRegister = _productGtmManager.GetGtmFunction(ComponentTypeEnum.AnonymousSeiumb, renderingItem, href, productGtm);

                productGtm = new ProductCtaBase { ProductName = productName, CtaText = userStateProduct.LoginMobileText };
                var gtmColdLoginMobile = _productGtmManager.GetGtmFunction(ComponentTypeEnum.AnonymousSeiumb, renderingItem, href, productGtm);
                gtmColdLoginMobile = $"{gtmColdLoginMobile}{trackingFunction}";

                userStateProduct.ActionColdClickMobile = gtmColdLoginMobile;
            }
            else if (userStateProduct.AuthenticationStatus.Equals(UserStatusCons.HOT) || userStateProduct.AuthenticationStatus.Equals(UserStatusCons.WARM_HOT) || userStateProduct.AuthenticationStatus.Equals(UserStatusCons.WARM_COLD))
            {
                userStateProduct.UserData.MdsIndvId = seiuProfile.MdsId.PadLeft(9, '0');
                userStateProduct.UserData.MdsId = seiuProfile.MdsId;
                userStateProduct.UserData.SeiuLocNum = seiuProfile.SeiuLocalNumber;
                userStateProduct.UserData.Address = seiuProfile.StreetAddress;
                userStateProduct.UserData.City = seiuProfile.City;
                userStateProduct.UserData.FirstName = seiuProfile.FirstName;
                userStateProduct.UserData.LastName = seiuProfile.LastName;
                userStateProduct.UserData.LoginUserId = seiuProfile.Email;
                userStateProduct.UserData.Phone = seiuProfile.Phone;
                userStateProduct.UserData.StateCode = seiuProfile.StateCode;
                userStateProduct.UserData.Zipcode = seiuProfile.ZipCode;
                userStateProduct.ContextItem = renderingItem;
                if (userStateProduct.HasCheckEligibility)
                    try
                    {
                        int.TryParse(userStateProduct.UserData.MdsId, out var mdsidInt);
                        userStateProduct.HasEligibility = _productRestManagerSeiumb.GetEligibility(mdsidInt);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(this + "Error in Restricted Locals service call-" + DateTime.Now, ex, this);
                    }

                LinkField ctalink1 = renderingItem.Fields[Templates.ProductLite.Fields.CTA1Link];
                LinkField ctalink2 = renderingItem.Fields[Templates.ProductLite.Fields.CTA2Link];
                LinkField ctalink3 = renderingItem.Fields[Templates.ProductLite.Fields.CTA3Link];
                var linkMobile = renderingItem.LinkFieldUrl(Templates.ProductLite.Fields.MobileLoginButton);

                //ACTION 1
                if (renderingItem.FieldHasValue(Templates.ProductLite.Fields.CTA1Link))
                {
                    var calltype1 = renderingItem[Templates.ProductLite.Fields.CTA1Type];
                    var type1 = "";
                    type1 = string.IsNullOrEmpty(calltype1) ? "Link" : Context.Database.GetItem(new ID(calltype1))[Templates.NameValueItem.Fields.Value];

                    userStateProduct.Action1Text = SetActionText(userStateProduct, Templates.ProductLite.Fields.CTA1Link, renderingItem);

                    if (type1 == ActionTypeEnum.Efulfillment.ToString())
                        try
                        {
                            userStateProduct.MaterialId = OracleProvider.ExecuteQueryMaterialOracle(userStateProduct.ProductCode);
                            if (string.IsNullOrEmpty(userStateProduct.MaterialId))
                                Log.Error(this +
                                    "The search to Oracle database, product_mapping table does not return result with product code " +
                                    userStateProduct.ProductCode,
                                    this);
                        }
                        catch (Exception ex)
                        {
                            Log.Error(this + "Error in execution of select in product_mapping table" + DateTime.Now, ex, this);
                        }
                    userStateProduct.ActionFirstText = ctalink1.Text;
                    userStateProduct.ActionFirstTitle = ctalink1.Title;

                    var productGtm = new ProductCtaBase { ProductName = productName, CtaText = userStateProduct.ActionFirstText };

                    if (userStateProduct.AuthenticationStatus.Equals(UserStatusCons.HOT))
                    {
                        var href = "";

                        if (type1 == ActionTypeEnum.DataPass.ToString())
                        {
                            GetDatapassAction(ref userStateProduct);
                            if (productCode.Equals("494 02"))
                            {
                                userStateProduct.ActionFirstClick =
                                    $"operationprocedure('{productCode}','{seiuProfile.MdsId}');gettruecarurl();return false;";
                                href = URLCarPartner;
                            }
                            else
                            {
                                userStateProduct.ActionFirstClick =
                                    $"operationprocedure('{productCode}','{seiuProfile.MdsId}');$('#partnerformdesktop').submit();return false;";
                                href = new Uri(userStateProduct.Action).GetLeftPart(UriPartial.Path);
                            }
                            userStateProduct.ActionType1 = ActionTypeEnum.DataPass;
                        }
                        else if (type1 == ActionTypeEnum.Efulfillment.ToString())
                        {
                            userStateProduct.ActionFirstClick = $"downloadpdf('{userStateProduct.MaterialId}','{seiuProfile.MdsId}');return false;";
                            userStateProduct.ActionFirstTarget = "_blank";
                            href = URLEfulfillment;
                        }
                        //Link
                        else
                        {
                            userStateProduct.ActionType1 = ActionTypeEnum.CtaAction;
                            userStateProduct.ActionFirstTarget = "_blank";
                            userStateProduct.ActionFirstClick = $"executelink('{renderingItem.ID.Guid:N}','{Templates.ProductLite.Fields.CTA1Link.Guid:N}'," +
                                $"'{userStateProduct.ActionFirstTarget}','{Templates.ProductLite.Fields.CTA1PostData.Guid:N}');" +
                                $"operationprocedure('{productCode}','{seiuProfile.MdsId}');";
                            LinkField field = renderingItem.Fields[Templates.ProductLite.Fields.CTA1Link];
                            var isInternal = field.IsInternal;
                            href = isInternal ? userStateProduct.Action1Text :
                                !string.IsNullOrEmpty(userStateProduct.Action1Text) ? new Uri(userStateProduct.Action1Text).GetLeftPart(UriPartial.Path) : "";
                        }
                        //set the gtm tracking
                        var gtmFunction = _productGtmManager.GetGtmFunction(ComponentTypeEnum.CtaSeiumb, renderingItem, href, productGtm, StatusEnum.Hot);
                        var trackingGoal = GetGoalProductTracking(renderingItem, ConstantsSeiumb.Primary);
                        userStateProduct.ActionFirstClick = $"{trackingGoal};{gtmFunction};{userStateProduct.ActionFirstClick}";
                    }
                    else if (userStateProduct.AuthenticationStatus.Equals(UserStatusCons.WARM_HOT) || userStateProduct.AuthenticationStatus.Equals(UserStatusCons.WARM_COLD))
                    {
                        var trackingGoal = GetGoalProductTracking(renderingItem, ConstantsSeiumb.Primary);
                        var actionClickAppend = $"{trackingGoal};";
                        var href = LinkManager.GetItemUrl(Context.Item);
                        if (userStateProduct.AuthenticationStatus.Equals(UserStatusCons.WARM_HOT))
                        {
                            var gtmActionClick = _productGtmManager.GetGtmFunction(ComponentTypeEnum.CtaSeiumb, renderingItem, href, productGtm, StatusEnum.WarmHot);
                            actionClickAppend += $"{gtmActionClick}";
                        }

                        userStateProduct.LoginModalOnClickFirstV2 = actionClickAppend;
                        var contextitemid = renderingItem.ID.Guid.ToString("N");
                        var calllinkid = Templates.ProductLite.Fields.CTA1Link.Guid.ToString("N");
                        var actiontarget = "_blank";
                        var postparameterid = Templates.ProductLite.Fields.CTA1PostData.Guid.ToString("N");

                        if (type1 == ActionTypeEnum.DataPass.ToString())
                        {
                            GetDatapassAction(ref userStateProduct);

                            if (productCode.Equals("494 02"))
                            {
                                href = URLCarPartner;
                                userStateProduct.LoginModalUrlFirst =
                                    $"{linkMobile}?id={renderingItem.ID}&actionurl={href}&actionkind={"5"}&actiontitle={ctalink1.Text}&actionprocedurepar1={productCode}" +
                                    $"&actionprocedurepar2={seiuProfile.MdsId}&materialid={""}&actiondatapass={HttpUtility.UrlEncode(userStateProduct.Action)}&productname={productName}" +
                                    $"&actiontype={"1"}&contextitemid={contextitemid}&calllinkid={calllinkid}&actiontarget={actiontarget}&postparameterid={postparameterid}";

                                userStateProduct.LoginModalOnClickFirst =
                                    $"{actionClickAppend}setvariables('{href}','5','{ctalink1.Text}','{productCode}','{seiuProfile.MdsId}','','{productName}','1','{contextitemid}','{calllinkid}','{actiontarget}','{postparameterid}')";
                            }
                            else
                            {
                                href = !string.IsNullOrEmpty(userStateProduct.Action) ? new Uri(userStateProduct.Action).GetLeftPart(UriPartial.Path) : "";
                                userStateProduct.LoginModalUrlFirst =
                                    $"{linkMobile}?id={renderingItem.ID}&actionurl={href}&actionkind={"3"}&actiontitle={ctalink1.Text}&actionprocedurepar1={productCode}" +
                                    $"&actionprocedurepar2={seiuProfile.MdsId}&materialid={""}&actiondatapass={HttpUtility.UrlEncode(userStateProduct.Action)}&productname={productName}" +
                                    $"&actiontype={"1"}&contextitemid={contextitemid}&calllinkid={calllinkid}&actiontarget={actiontarget}&postparameterid={postparameterid}";
                                userStateProduct.LoginModalOnClickFirst =
                                    $"{actionClickAppend}setvariables('{href}','3','{ctalink1.Text}','{productCode}','{seiuProfile.MdsId}','','{productName}','1','{contextitemid}','{calllinkid}','{actiontarget}','{postparameterid}')";
                            }
                        }
                        else if (type1 == ActionTypeEnum.Efulfillment.ToString())
                        {
                            href = URLEfulfillment;
                            userStateProduct.LoginModalUrlFirst =
                                $"{linkMobile}?id={renderingItem.ID}&actionurl={href}&actionkind={"2"}&actiontitle={ctalink1.Text}&actionprocedurepar1={productCode}" +
                                $"&actionprocedurepar2={seiuProfile.MdsId}&materialid={userStateProduct.MaterialId}&actiondatapass={""}&productname={productName}&actiontype={"1"}" +
                                $"&contextitemid={contextitemid}&calllinkid={calllinkid}&actiontarget={actiontarget}&postparameterid={postparameterid}";
                            userStateProduct.LoginModalOnClickFirst =
                                $"{actionClickAppend}setvariables('{href}','2','{ctalink1.Text}','','{seiuProfile.MdsId}','{userStateProduct.MaterialId}','{productName}','1','{contextitemid}','{calllinkid}','{actiontarget}','{postparameterid}')";
                        }
                        else
                        {
                            userStateProduct.LoginModalUrlFirst =
                                $"{linkMobile}?id={renderingItem.ID}&actionurl={userStateProduct.Action1Text}&actionkind={"1"}&actiontitle={ctalink1.Text}" +
                                $"&actionprocedurepar1={productCode}&actionprocedurepar2={seiuProfile.MdsId}&materialid={""}&actiondatapass={""}&productname={productName}" +
                                $"&actiontype={"1"}&contextitemid={contextitemid}&calllinkid={calllinkid}&actiontarget={actiontarget}&postparameterid={postparameterid}";
                            userStateProduct.LoginModalOnClickFirst =
                                $"{actionClickAppend}setvariables('{userStateProduct.Action1Text}','1','{ctalink1.Text}','{productCode}','{seiuProfile.MdsId}','','{productName}','1','{contextitemid}','{calllinkid}','{actiontarget}','{postparameterid}')";
                        }
                    }

                    userStateProduct.HasAction1 = true;
                }

                //ACTION 2
                if (renderingItem.FieldHasValue(Templates.ProductLite.Fields.CTA2Link))
                {
                    var calltype2 = renderingItem[Templates.ProductLite.Fields.CTA2Type];

                    var type2 = "";
                    if (string.IsNullOrEmpty(calltype2))
                    {
                        type2 = "Link";
                    }
                    else
                    {
                        var calltype2Item = Context.Database.GetItem(new ID(calltype2));
                        type2 = calltype2Item[Templates.NameValueItem.Fields.Value];
                    }
                    userStateProduct.Action2Text = SetActionText(userStateProduct, Templates.ProductLite.Fields.CTA2Link, renderingItem);

                    if (type2 == ActionTypeEnum.Efulfillment.ToString())
                        try
                        {
                            userStateProduct.MaterialId = OracleProvider.ExecuteQueryMaterialOracle(userStateProduct.ProductCode);
                            if (string.IsNullOrEmpty(userStateProduct.MaterialId))
                            {
                                Log.Error(this +
                                    "The search to Oracle database, product_mapping table does not return result with product code " +
                                    userStateProduct.ProductCode,
                                    this);
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Error(this + "Error in execution of select in product_mapping table" + DateTime.Now, ex, this);
                        }
                    userStateProduct.ActionSecondText = ctalink2.Text;
                    userStateProduct.ActionSecondTitle = ctalink2.Title;

                    var productGtm = new ProductCtaBase { ProductName = productName, CtaText = userStateProduct.ActionSecondText };
                    if (userStateProduct.AuthenticationStatus.Equals(UserStatusCons.HOT))
                    {
                        var href = "";
                        if (type2 == ActionTypeEnum.DataPass.ToString())
                        {
                            GetDatapassAction(ref userStateProduct);
                            if (productCode.Equals("486 03"))
                                userStateProduct.ActionSecondClick =
                                    $"cnsinvitation('partnerformdesktop');operationprocedure('{productCode}','{seiuProfile.MdsId}');$('#partnerformdesktop').submit();" +
                                    "resetinvitation('partnerformdesktop');return false;";
                            href = !string.IsNullOrEmpty(userStateProduct.Action) ? new Uri(userStateProduct.Action).GetLeftPart(UriPartial.Path) : "";
                            userStateProduct.ActionType2 = ActionTypeEnum.DataPass;
                        }
                        else if (type2 == ActionTypeEnum.Efulfillment.ToString())
                        {
                            userStateProduct.ActionSecondClick = $"downloadpdf('{userStateProduct.MaterialId}','{seiuProfile.MdsId}');return false;";
                            userStateProduct.ActionSecondTarget = "_blank";
                            href = URLEfulfillment;
                        }
                        else
                        {
                            userStateProduct.Action2Text = SetActionText(userStateProduct, Templates.ProductLite.Fields.CTA2Link, renderingItem);
                            userStateProduct.ActionSecondTarget = "_blank";
                            userStateProduct.ActionSecondClick = $"executelink('{renderingItem.ID.Guid:N}','{Templates.ProductLite.Fields.CTA2Link.Guid:N}'," +
                                $"'{userStateProduct.ActionSecondTarget}','{Templates.ProductLite.Fields.CTA2PostData.Guid:N}');" +
                                $"operationprocedure('{productCode}','{seiuProfile.MdsId}');";
                            userStateProduct.ActionType2 = ActionTypeEnum.CtaAction;
                            LinkField field = renderingItem.Fields[Templates.ProductLite.Fields.CTA2Link];
                            var isInternal = field.IsInternal;
                            href = isInternal ? userStateProduct.Action2Text :
                                !string.IsNullOrEmpty(userStateProduct.Action2Text) ? new Uri(userStateProduct.Action2Text).GetLeftPart(UriPartial.Path) : "";
                        }
                        //set the gtm tracking                       
                        var gtmFunction = _productGtmManager.GetGtmFunction(ComponentTypeEnum.CtaSecondarySeiumb, renderingItem, href, productGtm, StatusEnum.Hot);
                        var trackingGoal = GetGoalProductTracking(renderingItem, ConstantsSeiumb.Secondary);
                        userStateProduct.ActionSecondClick = $"{trackingGoal};{gtmFunction};{userStateProduct.ActionSecondClick}";
                    }
                    else if (userStateProduct.AuthenticationStatus.Equals(UserStatusCons.WARM_HOT) || userStateProduct.AuthenticationStatus.Equals(UserStatusCons.WARM_COLD))
                    {
                        var trackingGoal = GetGoalProductTracking(renderingItem, ConstantsSeiumb.Secondary);
                        var clickActionSecAppend = $"{trackingGoal};";
                        var href = LinkManager.GetItemUrl(Context.Item);
                        if (userStateProduct.AuthenticationStatus.Equals(UserStatusCons.WARM_HOT))
                            clickActionSecAppend += $"{_productGtmManager.GetGtmFunction(ComponentTypeEnum.CtaSeiumb, renderingItem, href, productGtm, StatusEnum.WarmHot)}";

                        userStateProduct.LoginModalOnClickSecondV2 = clickActionSecAppend;
                        var contextitemid = renderingItem.ID.Guid.ToString("N");
                        var calllinkid = Templates.ProductLite.Fields.CTA2Link.Guid.ToString("N");
                        var actiontarget = "_blank";
                        var postparameterid = Templates.ProductLite.Fields.CTA2PostData.Guid.ToString("N");

                        if (type2 == ActionTypeEnum.DataPass.ToString())
                        {
                            GetDatapassAction(ref userStateProduct);

                            href = !string.IsNullOrEmpty(userStateProduct.Action) ? new Uri(userStateProduct.Action).GetLeftPart(UriPartial.Path) : "";
                            if (productCode.Equals("486 03"))
                            {
                                userStateProduct.LoginModalUrlSecond =
                                    $"{linkMobile}?id={renderingItem.ID}&actionurl={href}&actionkind={"4"}&actiontitle={ctalink2.Text}&actionprocedurepar1={productCode}" +
                                    $"&actionprocedurepar2={seiuProfile.MdsId}&materialid={""}&actiondatapass={HttpUtility.UrlEncode(userStateProduct.Action)}&productname={productName}" +
                                    $"&actiontype={"2"}&contextitemid={contextitemid}&calllinkid={calllinkid}&actiontarget={actiontarget}&postparameterid={postparameterid}";
                                userStateProduct.LoginModalOnClickSecond =
                                    $"{clickActionSecAppend}setvariables('{href}','4','{ctalink2.Text}','{productCode}','{seiuProfile.MdsId}','','{productName}','2','{contextitemid}','{calllinkid}','{actiontarget}','{postparameterid}')";
                            }
                            //this was added
                            else
                            {
                                userStateProduct.LoginModalUrlSecond =
                                    $"{linkMobile}?id={renderingItem.ID}&actionurl={href}&actionkind={"3"}&actiontitle={ctalink2.Text}&actionprocedurepar1={productCode}" +
                                    $"&actionprocedurepar2={seiuProfile.MdsId}&materialid={""}&actiondatapass={HttpUtility.UrlEncode(userStateProduct.Action)}&productname={productName}" +
                                    $"&actiontype={"2"}&contextitemid={contextitemid}&calllinkid={calllinkid}&actiontarget={actiontarget}&postparameterid={postparameterid}";
                                userStateProduct.LoginModalOnClickSecond =
                                    $"{clickActionSecAppend}setvariables('{href}','3','{ctalink2.Text}','{productCode}','{seiuProfile.MdsId}','','{productName}','2','{contextitemid}'," +
                                    $"'{calllinkid}','{actiontarget}','{postparameterid}')";
                            }
                        }
                        else if (type2 == ActionTypeEnum.Efulfillment.ToString())
                        {
                            href = URLEfulfillment;
                            userStateProduct.LoginModalUrlSecond =
                                $"{linkMobile}?id={renderingItem.ID}&actionurl={href}&actionkind={"2"}&actiontitle={ctalink2.Text}&actionprocedurepar1={productCode}" +
                                $"&actionprocedurepar2={seiuProfile.MdsId}&materialid={userStateProduct.MaterialId}&actiondatapass={""}&productname={productName}&actiontype={"2"}" +
                                $"&contextitemid={contextitemid}&calllinkid={calllinkid}&actiontarget={actiontarget}&postparameterid={postparameterid}";
                            userStateProduct.LoginModalOnClickSecond =
                                $"{clickActionSecAppend}setvariables('{href}','2','{ctalink2.Text}','','{seiuProfile.MdsId}','{userStateProduct.MaterialId}','{productName}','2'," +
                                $"'{contextitemid}','{calllinkid}','{actiontarget}','{postparameterid}')";
                        }
                        else
                        {
                            userStateProduct.LoginModalUrlSecond =
                                $"{linkMobile}?id={renderingItem.ID}&actionurl={userStateProduct.Action2Text}&actionkind={"1"}&actiontitle={ctalink2.Text}" +
                                $"&actionprocedurepar1={productCode}&actionprocedurepar2={seiuProfile.MdsId}&materialid={""}&actiondatapass={""}&productname={productName}" +
                                $"&actiontype={"2"}&contextitemid={contextitemid}&calllinkid={calllinkid}&actiontarget={actiontarget}&postparameterid={postparameterid}";
                            userStateProduct.LoginModalOnClickSecond =
                                $"{clickActionSecAppend}setvariables('{userStateProduct.Action2Text}','1','{ctalink2.Text}','{productCode}','{seiuProfile.MdsId}','','{productName}','2','{contextitemid}','{calllinkid}','{actiontarget}','{postparameterid}')";
                        }
                    }

                    userStateProduct.HasAction2 = true;
                }

                //ACTION 3
                if (renderingItem.FieldHasValue(Templates.ProductLite.Fields.CTA3Link))
                {
                    var calltype3 = renderingItem[Templates.ProductLite.Fields.CTA3Link];
                    var type3 = string.IsNullOrEmpty(calltype3) ? "Link" : (Context.Database.GetItem(new ID(calltype3)))[Templates.NameValueItem.Fields.Value];
                    userStateProduct.ActionThirdText = ctalink3.Text;
                    userStateProduct.ActionThirdTitle = ctalink3.Title;
                    if (type3 == "Link")
                    {
                        userStateProduct.Action3Text = SetActionText(userStateProduct, Templates.ProductLite.Fields.CTA3Link, renderingItem);
                        userStateProduct.ActionThirdUrl = userStateProduct.Action3Text;
                        userStateProduct.ActionThirdTarget = "_blank";
                        var trackingFunction = GetGoalProductTracking(renderingItem, ConstantsSeiumb.Third);
                        userStateProduct.ActionThirdClick = $"executelink('{renderingItem.ID.Guid:N}','{Templates.ProductLite.Fields.CTA3Link.Guid:N}'," +
                            $"'{userStateProduct.ActionThirdTarget}','{Templates.ProductLite.Fields.CTA3PostData.Guid:N}');{trackingFunction};" +
                            $"operationprocedure('{productCode}','{seiuProfile.MdsId}');";
                        //userStateProduct.ActionThirdClick = $"operationprocedure('{productCode}','{mdsid}');";
                        userStateProduct.ActionType3 = ActionTypeEnum.CtaAction;
                    }
                    userStateProduct.HasAction3 = true;
                }
            }
            return userStateProduct;
        }

        /// <summary>
        /// Get the action tracking according the button primary or secondary
        /// </summary>
        /// <param name="contextItem">Context item</param>
        /// <param name="actionType">action type</param>
        /// <returns></returns>
        private string GetGoalProductTracking(Item contextItem, string actionType) {
            if (actionType.Equals(ConstantsSeiumb.Primary))
                return $"trackingGoalProduct('{contextItem.ID}', '{Templates.ProductLite.Fields.CTA1Goal}')";
            return actionType.Equals(ConstantsSeiumb.Secondary)
                ? $"trackingGoalProduct('{contextItem.ID}', '{Templates.ProductLite.Fields.CTA2Goal}')"
                : $"trackingGoalProduct('{contextItem.ID}', '{Templates.ProductLite.Fields.CTA3Goal}')";
        }

        private void GetDatapassAction(ref UserStateProduct userStateProduct)
        {
            var partner = _partnerFactory.GetPartner(userStateProduct.ProductCode);
            if (partner == null) return;
            userStateProduct.Action = partner.GetActionPrimary(new Dictionary<string, string> { { "ref", userStateProduct.UserData.MdsId } });
            userStateProduct.Token = partner.GetToken();
        }

        private string SetActionText(UserStateProduct userStateProduct, ID callActionLink, Item contextItem)
        {
            var urlaction = contextItem.LinkFieldUrl(callActionLink);
            var result = string.Empty;

            if (urlaction.Contains("[mdsid]"))
            {
                result = urlaction.Replace("[mdsid]", userStateProduct.UserData.MdsIndvId);
            }
            else if (urlaction.Contains("[mdsid_afinium]"))
            {
                var encryptedMdsid = _neambServiceManager.EncryptPartner(userStateProduct.UserData.MdsIndvId);
                result = urlaction.Replace("[mdsid_afinium]", HttpUtility.UrlEncode(encryptedMdsid));
            }
            else if (urlaction.Contains("[afinium]"))
            {
                var encryptedMdsid = _securityService.AesEncrypt(userStateProduct.UserData.MdsIndvId, Token.Afinium);
                if (string.IsNullOrEmpty(encryptedMdsid)) throw new ApplicationException("Error with afinium encryption process Seiumb");
                result = urlaction.Replace("[afinium]", encryptedMdsid);
            }
            else
            {
                result = urlaction;
            }

            if (result.Contains("[cellcode]"))
            {
                var cellCode = HttpContext.Current.Session[ConstantsSeiumb.SourceCode] != null ? HttpContext.Current.Session[ConstantsSeiumb.SourceCode].ToString() : string.Empty;
                result = result.Replace("[cellcode]", HttpUtility.UrlEncode(cellCode));
            }

            if (result.Contains("[campcode]"))
            {
                var campaignCode = HttpContext.Current.Session[ConstantsSeiumb.CampaignCode] != null ? HttpContext.Current.Session[ConstantsSeiumb.CampaignCode].ToString() : null;
                result = result.Replace("[campcode]", HttpUtility.UrlEncode(campaignCode));
            }
            return result;
        }


        /// <summary>
        /// Get the items selected in multilist with search 
        /// </summary>
        /// <returns>List of itemss</returns>
        public ProductList GetProductListData(Item contextItem)
        {
            var productList = new ProductList { ContextItem = contextItem, Items = new List<Item>() };
            var productItemList = contextItem[Templates.ProductList.Fields.ProductItemList];
            if (string.IsNullOrEmpty(productItemList)) return productList;
            var productListValues = productItemList.Split('|');
            foreach (var itemString in productListValues)
                if (!Context.PageMode.IsExperienceEditor) {
                    var db = Context.Database;
                    var itemBdd = db.GetItem(new ID(itemString));
                    if (itemBdd != null) productList.Items.Add(itemBdd);
                }
                else
                {
                    var itemBdd = Context.Database.GetItem(new ID(itemString));
                    productList.Items.Add(itemBdd);
                }
            return productList;
        }

        /// <summary>
        /// GTet the PDF that is download in Efulfillment option
        /// </summary>
        /// <param name="materialId">Material Id</param>

        /// <returns>PDF as byte array</returns>
        public string GetPdfFile(string materialId)
        {
            var urlPdf = "";
            var seiuProfile = _seiumbProfileManager.GetProfile();
            var uniqueName = $"{_cacheKeyGroup}:{seiuProfile.MdsId}-{materialId}";
            var pdfFile = _pdfManager.VerifyExistencePdfFile(uniqueName, _globalConfigurationManager.UrlEfulfillmentS3Seiumb, false);
            int.TryParse(seiuProfile.MdsId, out var mdsidInt);
            if (pdfFile == null)
            {
                var pdfRequest = new PdfRequest
                {
                    ProductIemId = materialId,
                    Email = seiuProfile.Email,
                    PdTransDate = DateTime.Now.ToString("MM/dd/yyyy"),
                    PdFirstName = seiuProfile.FirstName,
                    PdLastName = seiuProfile.LastName,
                    PdDob = GetDob(seiuProfile.DateOfBirth),
                    PdMdsid = mdsidInt,
                    PdAddress = seiuProfile.StreetAddress,
                    PdCity = seiuProfile.City,
                    PdState = seiuProfile.StateCode,
                    PdZip = seiuProfile.ZipCode,
                    PdMemberType = seiuProfile.MembershipType
                };
                pdfFile = _pdfManager.GetPdfFile(materialId, uniqueName, pdfRequest,
                _globalConfigurationManager.UrlEfulfillmentS3Seiumb, "", "", false);
            }
            if (pdfFile == null || pdfFile.Length <= 0) return urlPdf;
            var fileName = _pdfManager.GetPdfUrl(uniqueName, false);
            if (!string.IsNullOrEmpty(fileName)) urlPdf = fileName;
            return urlPdf;
        }

        private string GetDob(string dob) {
            return DateTime.TryParseExact(dob, "MMddyyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var result) ? 
                $"{result:MM/dd/yyyy}" : string.Empty;
        }

        /// <summary>
		/// Insert into mds when the user click in cta action in product component
		/// </summary>
		/// <param name="productcode">Product code</param>
		public void ExecuteMdsLoggingProcessCta(string productcode, string mdsid)
        {
            ExecuteMdsLoggingProcessInner(productcode, ConstantsSeiumb.CtaClickProductCode, "C", mdsid);
        }
        /// <summary>
        /// Insert into mds when the user visited the product page
        /// </summary>
        /// <param name="productcode">Product code</param>
        public void ExecuteMdsLoggingProcessView(string productcode, string mdsid)
        {
            ExecuteMdsLoggingProcessInner(productcode, ConstantsSeiumb.ViewProductCode, "V", mdsid);
        }

        /// <summary>
        /// Insert into mds when the user click in efulfillment click in product component
        /// </summary>
        /// <param name="materialid">Material id</param>
        public void ExecuteMdsLoggingProcessMaterial(string materialid, string mdsid)
        {
            ExecuteMdsLoggingProcessInner(string.Empty, ConstantsSeiumb.MaterialClickProductCode, string.Empty, mdsid, materialid);
        }

        public void ExecuteMdsLoggingProcessInner(string productcode, string nameForSession, string typeProcess, string mdsid, string materialid = "") {
            var cellCode = HttpContext.Current.Session[ConstantsSeiumb.SourceCode] != null ? HttpContext.Current.Session[ConstantsSeiumb.SourceCode].ToString() : null;
            _mdsLoggingManager.ExecuteMdsLoggingProcess(productcode,nameForSession,typeProcess,mdsid,cellCode,materialid);
        }
    }
}