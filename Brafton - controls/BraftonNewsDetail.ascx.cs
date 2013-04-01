using BraftonData;
using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Telerik.Cms.Web.UI;
using Telerik.OpenAccess;
using System.Reflection;

public partial class Controls_BraftonNewsDetail : System.Web.UI.UserControl
{

    private string _providerName = "/";

    [Category("Appearance")]
    [WebEditor("Telerik.Cms.Engine.WebControls.ContentSelector, Telerik.Cms.Engine")]
    public Guid ArticleContent
    {
        get
        {
            object obj = ViewState["ArticleContent"];
            if (obj != null)
                return (Guid)obj;
            return Guid.Empty;
        }
        set
        {
            ViewState["ArticleContent"] = value;
        }
    }

    public string ProviderName
    {
        get
        {
            return this._providerName;
        }
        set
        {
            this._providerName = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        string newsUrl = Request.Params["demo"];
        if (!string.IsNullOrEmpty(newsUrl))
        {
            if (newsUrl.Contains(".aspx")){
                int index = newsUrl.IndexOf(".aspx");
                int count = newsUrl.Length - index;
                if (count < 0)
                    count = 0;
                newsUrl = newsUrl.Remove(index, count);
            }
            IObjectScope scope = BraftonContext.GetNewObjectScope();
            var result = from n in scope.Extent<NewsItem>()
                         where n.Url == newsUrl
                         select n;
            if (result.Count() > 0)
            {
                this.articleContent.ContentID = this.ArticleContent;
                string html = this.articleContent.SharedContent.Content.ToString();
                this.articleContent.Content = ReplaceTokens((NewsItem)result.First(), html);
            }
        }
    }

    protected string ReplaceTokens(NewsItem item, string content)
    {
        string tokenized = content;
        foreach (PropertyInfo prop in item.GetType().GetProperties())
        {
            object propValue = prop.GetValue(item, null);
            if ((propValue != null) && !string.IsNullOrEmpty(propValue.ToString())){
                tokenized = tokenized.Replace("%%" + prop.Name + "%%", propValue.ToString());
            }
        }
        return tokenized;
    }
}
