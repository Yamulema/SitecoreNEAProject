using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neambc.Neamb.Feature.Contest.Enums;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Contest.Model
{
    public class ContestVote : IRenderingModel
    {
        public Rendering Rendering { get; set; }
        public Item Item { get; set; }
        public Item PageItem { get; set; }
        public Item Datasource { get; set; }
        public string ContestId { get; set; }
        public IList<Tuple<Submission, int>> SubmissionVotes { get; set; }
        public ContestStatus Status { get; set; }
        public HtmlString SocialShare { get; set; }
        public Pagination Pagination { get; set; }
        

        public ContestVote(Rendering rendering)
        {
            Initialize(rendering);
        }

        public void Initialize(Rendering rendering)
        {
            Rendering = rendering;
            Item = rendering.Item;
            PageItem = PageContext.Current.Item;
            Datasource = rendering.Item ?? PageContext.Current.Item;
            SubmissionVotes = new List<Tuple<Submission, int>>();
        }
    }
}