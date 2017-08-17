using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IbStats.Helpers
{
    public static class ChartHelper
    {
        public static MvcHtmlString DrawSparkline(this HtmlHelper helper, int width, int height, IEnumerable<int> plotData,
            string bgColor, string lineColor, string labelColor)
        {
            int? max = plotData.Max();
            int? min = plotData.Min();
            string list = String.Join(",", plotData);
            string sparkUrl = "<img src='http://chart.apis.google.com/" +
                 "chart?cht=lc&chf=bg,s,{6}&cgh=0,50,1,0&chds={0},{1}&chs={2}x{3}&chd=t:{4}&chco={8}" +
                 "&chls=1,1,0&chm=o,{7},0,20,4&chxt=r,x,y&chxs=0,{7},11,0,_|1,{7},1,0,_|2,{7},1,0,_&chxl=0:|{5}|1:||2:||'/>";
            string sparkLine = String.Format(sparkUrl, min, max, width, height, list, plotData.Last(), bgColor, labelColor, lineColor);
            return MvcHtmlString.Create(sparkLine);
        }

        public static MvcHtmlString DrawSparkline(this HtmlHelper helper, int width, int height, IEnumerable<float> plotData,
            string bgColor, string lineColor, string labelColor)
        {
            string sparkLine = "";
            if (plotData.Count() > 0)
            {
                float? max = 20;//plotData.Max();
                float? min = -20;//plotData.Min();
                string list = String.Join(",", plotData);
                string sparkUrl = "<img src='http://chart.apis.google.com/" +
                     "chart?cht=lc&chf=bg,s,{6}&cgh=0,50,1,0&chds={0},{1}&chs={2}x{3}&chd=t:{4}&chco={8}" +
                     "&chls=1,1,0&chm=o,{7},0,20,4&chxt=r,x,y&chxs=0,{7},11,0,_|1,{7},1,0,_|2,{7},1,0,_&chxl=0:||1:||2:||'/>";
                sparkLine = String.Format(sparkUrl, min, max, width, height, list, plotData.Last(), bgColor, labelColor, lineColor);
            }
            else
            {
                sparkLine = "N/A";
            }

                
            return MvcHtmlString.Create(sparkLine);
        }

    }
}