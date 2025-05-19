using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceNewsMobile.Models
{
    public class NewsList
    {
        public string Status { get; set; }
        public int TotalResults { get; set; }
        public List<News> Articles { get; set; } = new List<News>();

        public void AddNews(News news)
        {
            Articles.Add(news);
        }
    }
}
