using Sitecore.Web;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Seiumb.Feature.Language.Infrastructure.Pipelines
{
    public class StripLanguage : Sitecore.Pipelines.PreprocessRequest.StripLanguage
    {
		private readonly ArrayList _validLanguages = new ArrayList();

		public void AddValidLanguage(string language)
        {
            _validLanguages.Add(language.ToLower());
        }

        public override void Process(
          Sitecore.Pipelines.PreprocessRequest.PreprocessRequestArgs args)
        {            
            if (args != null
              && HttpContext.Current != null
              && !string.IsNullOrWhiteSpace(HttpContext.Current.Request.FilePath))
            {
                var prefix = WebUtil.ExtractLanguageName(
                  HttpContext.Current.Request.FilePath);

                if ((!string.IsNullOrWhiteSpace(prefix))
                  && !_validLanguages.Contains(prefix.ToLower()))
                {
                    return;
                }
            }

            base.Process(args);
        }
    }
}