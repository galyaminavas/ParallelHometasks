using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebPagesDownload
{
    public class Downloader
    {
        public async Task<String> DownloadPage(string address)
        {
            using (WebClient client = new WebClient())
            {
                String content = "";
                try
                {
                    content = await client.DownloadStringTaskAsync(address);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception: " + e.Message + "\nOn page: " + address);
                }
                return content;
            }
        }

        public String[] FindLinks(string mainPage)
        {
            //<a href="http://...">
            Regex tagRX = new Regex(@"<a href=("")http(s)?://[a-zA-Z0-9-_\.~/:#\?=%&;\+@!\$\*',\[\]\(\)]*("")>");
            var matches = tagRX.Matches(mainPage);
            string[] links = new string[matches.Count];
            string pattern1 = @"<a href=("")";
            string pattern2 = @"("")>";
            Regex rgx1 = new Regex(pattern1);
            Regex rgx2 = new Regex(pattern2);
            for (int i = 0; i < matches.Count; i++)
            {
                links[i] = matches[i].ToString();
                links[i] = rgx1.Replace(links[i], "");
                links[i] = rgx2.Replace(links[i], "");
            }
            return links;
        }
    }
}
