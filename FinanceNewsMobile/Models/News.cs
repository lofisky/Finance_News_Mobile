namespace FinanceNewsMobile.Models
{
    public class News
    {
        public Source Source { get; set; }
        public string Author {  get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string UrlToImage { get; set; }
        public string PublishedAt { get; set; }
        public string Content { get; set; }


        public string FormattedPublishedAt
        {
            get
            {
                if (DateTime.TryParse(PublishedAt, out var dt))
                {
                    return dt.ToLocalTime().ToString("yyyy-MM-dd HH:mm");
                }
                return PublishedAt;
            }
        }
    }
}
