using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceNewsMobile.Models
{
    class News
    {
        private string sourceName;
        private string title;
        private string description;
        private string url;
        private string urlToImage;
        private string publishedAt;

        public News(string sourceName, string title, string description, string url, string urlToImage, string publishedAt)
        {
            this.sourceName = sourceName;
            this.title = title;
            this.description = description;
            this.url = url;
            this.urlToImage = urlToImage;
            this.publishedAt = publishedAt;
        }

        public string SourceName => sourceName;
        public string Title => title;
        public string Description => description;
        public string Url => url;
        public string UrlToImage => urlToImage;
        public string PublishedAt => publishedAt;
    }
}
