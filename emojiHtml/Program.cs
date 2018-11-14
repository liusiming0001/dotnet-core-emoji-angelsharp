using AngleSharp.Parser.Html;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;

namespace emojiHtml
{
    class Program
    {
        public class emoji
        {
            public string category { get; set; }
            public string[] list { get; set; }
        }
        static void Main(string[] args)
        {
            HtmlParser htmlParser = new HtmlParser();
            HttpClient client = new HttpClient();
            //下载html代码
            var str = client.GetStringAsync("https://www.copyandpasteemoji.com/").Result;
            Console.Write(str);
            var dom = htmlParser.Parse(str);
            //查找emoji的分类名称
            var category = dom.QuerySelectorAll("div.row div.col-lg-12 h2");
            foreach (var item in category)
            {
                Console.WriteLine(item.InnerHtml);
            }
            //找到emoji的容器
            var tabcontent = dom.QuerySelectorAll("div.tab_content");
            //存入list中
            List<emoji> output = new List<emoji>();
            for (int i = 0; i < tabcontent.Length; i++)
            {
                var emojis = tabcontent[i].QuerySelectorAll("span.iconos");
                var itemList = new string[emojis.Length];
                for (int j = 0; j < emojis.Length; j++)
                {
                    itemList[j] = emojis[j].InnerHtml;
                }
                output.Add(new emoji
                {
                    category = category[i].InnerHtml,
                    list = itemList
                });
            }
            //存json文件中
            string savepath = @"C:/Users/LSM-PC/Desktop/emojiJson";
            string filename = "emoji.json";
            string filepath = savepath + "/" + filename;
            if (!Directory.Exists(savepath))
            {
                Directory.CreateDirectory(savepath);
            }
            if (!File.Exists(filepath))
            {
                FileStream fs1 = new FileStream(filepath, FileMode.Create, FileAccess.ReadWrite);
                fs1.Close();
            }
            File.WriteAllText(filepath, JsonConvert.SerializeObject(output));
            Console.ReadKey();
        }
    }
}
