using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neambc.Neamb.Foundation.Membership.Model;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Account.Models
{
    public class UpdateUserInformationDTO : IRenderingModel
    {
        public StatusEnum AccountStatus { get; set; }
        public string Submit { get; set; }
        public BackCTA BackCta { get; set; }
        public string AnonymousUser { get; set; }
        public ProfileDTO ProfileDto { get; set; }
        public Rendering Rendering { get; set; }
        public Item Item { get; set; }
        public Item PageItem { get; set; }
        public string DatasourceId { get; set; }
        public string OnClickEvent { get; set; }

        public UpdateUserInformationDTO()
        {

        }
        public UpdateUserInformationDTO(Rendering rendering)
        {
            Initialize(rendering);
        }
        public void Initialize(Rendering rendering)
        {
            Rendering = rendering;
            Item = rendering.Item;
            PageItem = PageContext.Current.Item;
        }
    }
}