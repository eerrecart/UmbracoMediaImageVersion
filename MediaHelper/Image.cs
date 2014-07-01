using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Url.Handler;
using Umbraco.Core.Models;
using Umbraco.Web;
using Examine;
using Examine.Providers;

namespace MediaHelper
{
    public static class UmbracoDataHelper
    {
        static UmbracoHelper helper = new UmbracoHelper(UmbracoContext.Current);
        static BaseSearchProvider searcher = ExamineManager.Instance.SearchProviderCollection["ExternalSearcher"];
        
        public static string GetImageURL(string imageId, string crop)
        {
            
            try
            {
                var criteria = searcher.CreateSearchCriteria();

                criteria.Id(int.Parse(imageId));

                var results = searcher.Search(criteria);
                
                if (results.Any())
                {
                    var result = results.FirstOrDefault();

                    string filename = System.IO.Path.GetFileName(result.Fields["umbracoFile"]);

                    string newfilename = filename.Replace("." + result.Fields["umbracoExtension"], string.Format("_{0}.jpg", crop));

                    string url = result.Fields["umbracoFile"].Replace(filename, newfilename);
                                    
                    url += "?v=" + result.Fields["updateDate"];

                    return url;

                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                //log error
                return string.Empty;
            }
        }
    }
}
