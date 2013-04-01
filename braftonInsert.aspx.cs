using BraftonAPI;
using BraftonData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.OpenAccess;
using Telerik.OpenAccess.Query;

public partial class braftonInsert : System.Web.UI.Page
{
    TransactionProvider transactionProvider;
    IObjectScope objectScope;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        if (Session["transactionProvider"] != null)
        {
            transactionProvider = (TransactionProvider)Session["transactionProvider"];
        }
        else
        {
            transactionProvider = TransactionProvider.OpenAccess;
        }

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        ApiContext context = new ApiContext(new Guid("e5c5c78b-9e7a-4759-8dd6-ceaf46a2b855"), "http://api.brafton.com/");
        var news = context.News;
        var categories = context.Categories;
        //IList<newsItem> newsItems = new List<newsItem>();
        //IList<category> categoryItems = new List<category>();
        objectScope = BraftonContext.GetNewObjectScope();

        foreach (category item in categories)
        {
            bool isNew = false;
            objectScope.Transaction.Begin();
            Category cat = GetCategory(item, out isNew);
            cat = LoadCategoryItem(item, cat);
            if (isNew)
                objectScope.Add(cat);
            objectScope.Transaction.Commit();
        }

        foreach (newsItem item in news)
        {
            var cats = item.categories;
            objectScope.Transaction.Begin();
            bool isNew = false;
            NewsItem _item = GetNewsItem(item, out isNew);
            _item = LoadNewsItem(item, _item);
            if (isNew)
                objectScope.Add(_item);
            objectScope.Transaction.Commit();
        }

        string query = "select * from NewsItemExtent AS x";

        IQuery queryForExecution = objectScope.GetOqlQuery(query);
        queryForExecution.ForwardsOnly = false;
        IQueryResult res = queryForExecution.Execute();
        if (res.Count > 0)
        {
            for (int i = 0; i < res.Count; i++)
            {
                NewsItem item = (NewsItem)res[i];
                ltrlItems.Text += "<a href=\"news/" + item.Url + "\">" + item.Headline + "</a><br /><p>" + item.Extract + "</p>";
            }
        }
    }

    protected NewsItem LoadNewsItem(newsItem newsFeed, NewsItem dataItem)
    {
        IList<Category> categories = new List<Category>();
        dataItem.Categories.Clear();
        bool isNew;
        foreach(category cat in newsFeed.categories)
        {
            Category category = GetCategory(cat, out isNew);
            category = LoadCategoryItem(cat, category);
            dataItem.Categories.Add(category);
        }

        dataItem.NewsID = newsFeed.id;
        dataItem.ByLine = newsFeed.byLine;
        dataItem.ClientQuote = newsFeed.clientQuote;
        dataItem.CreatedDate = newsFeed.createdDate;
        dataItem.Encoding = newsFeed.encoding;
        dataItem.Extract = newsFeed.extract;
        dataItem.Format = newsFeed.format;
        dataItem.Headline = newsFeed.headline;
        dataItem.HtmlMetaDescripiton = newsFeed.htmlMetaDescription;
        dataItem.HtmlMetaKeywords = newsFeed.htmlMetaKeywords;
        dataItem.HtmlMetaLanguage = newsFeed.htmlMetaLanguage;
        dataItem.HtmlTitle = newsFeed.htmlTitle;
        dataItem.LastModifiedDate = newsFeed.lastModifiedDate;
        dataItem.Priority = newsFeed.priority;
        dataItem.PublishDate = newsFeed.publishDate;
        dataItem.Source = newsFeed.source;
        dataItem.State = (int)newsFeed.state;
        dataItem.TweetText = newsFeed.tweetText;
        dataItem.Txt = newsFeed.text;
        dataItem.Url = Regex.Replace(newsFeed.headline.Trim().ToLower(), @"[^\p{L}\-\!\$\(\)\=\@\d_\'\.]+", "-");
        dataItem.Tags = newsFeed.tags;
        //dataItem.Categories = categories;

        return dataItem;
    }

    protected Category LoadCategoryItem(category cat, Category dataItem)
    {
        dataItem.CategoryID = cat.id;
        dataItem.CategoryName = cat.name;
        dataItem.Url = Regex.Replace(cat.name.Trim().ToLower(), @"[^\p{L}\-\!\$\(\)\=\@\d_\'\.]+", "-");
        return dataItem;
    }

    protected NewsItem GetNewsItem(newsItem item, out bool isNew)
    {
        NewsItem news;
        isNew = false;
        var result = from n in objectScope.Extent<NewsItem>()
                     where n.NewsID == item.id
                     select n;
        if (result.Count() <= 0)
        {
            news = new NewsItem();
            isNew = true;
        }
        else
        {
            news = (NewsItem)result.First();
        }
        return news;
    }

    protected Category GetCategory(category cat, out bool isNew)
    {
        Category category;
        isNew = false;
        var result = from c in objectScope.Extent<Category>()
                     where c.CategoryID == cat.id
                     select c;
        if (result.Count() <= 0)
        {
            category = new Category();
            isNew = true;
        }
        else
            category = (Category)result.First();
        return category;
    }
}
