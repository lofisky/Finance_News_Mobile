using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceNewsMobile.Models
{
    class NewsList
    {
        public List<News> newsList;

        public void addNews(News news)
        {
            newsList.Add(news);
        }

        public List<News> getNewsList => newsList;
    }
}
