using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Seiumb.Foundation.Authentication.Enums;
using Neambc.Seiumb.Foundation.Authentication.Interfaces;
using Neambc.Seiumb.Foundation.Authentication.Models;
using Sitecore.Xml;

namespace Neambc.Seiumb.Foundation.Authentication.Managers
{
    [Service(typeof(IPartnerFactoryConfiguration))]
    public class PartnerFactoryConfiguration : IPartnerFactoryConfiguration
    {
        public IDictionary<PartnerType, IPartnerConfiguration> Partners { get; set; }

        public PartnerFactoryConfiguration()
        {
            Partners = new Dictionary<PartnerType, IPartnerConfiguration>();
        }
        public void AddPartner(XmlNode node)
        {
            var type = Enum.TryParse(XmlUtil.GetAttribute("partnerType", node), true, out PartnerType _type)
                ? _type
                : PartnerType.None;

            if (type != PartnerType.None)
            {
                Partners.Add(type, new PartnerConfiguration()
                {
                    ProgramCodes = XmlUtil.GetChildNodes("programCode", XmlUtil.GetChildNode("programCodes", node))
                            .Select(x => x?.InnerText),
                    Ctas = XmlUtil.GetChildNodes("cta", XmlUtil.GetChildNode("ctas", node))
                            .ToDictionary(x => Enum.TryParse(XmlUtil.GetAttribute("ctaType", x), true, out CtaType ctaType)
                                ? ctaType
                                : CtaType.None,
                                x => new Cta()
                                {
                                    Url = XmlUtil.GetChildValue("url", x),
                                    DefaultReturnUrl = XmlUtil.GetChildValue("defaultReturnUrl", x)
                                }),
                    Token = XmlUtil.GetChildNode("token", node)?.InnerText
                });
            }
        }
    }
}