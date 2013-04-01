using BraftonData;
using System;
using System.Data;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Telerik.Cms;
using Telerik.Cms.Web;
using Telerik.Cms.Web.UI;
using Telerik.OpenAccess;
using System.Xml;
using System.Text;
using Telerik.OpenAccess.Query;
using Webality.Data;

namespace Webality
{

    #region Enums
    public enum DisplayModes{
        DETAIL,
        LIST,
        RSS
    }
    #endregion

    /// <summary>
    /// Summary description for BraftonFeed
    /// </summary>
    public class BraftonFeed : SimpleControl
    {
        private Webality.DisplayModes _displayMode = Webality.DisplayModes.LIST;
        private IObjectScope _objectScope;
        private int _itemLimit = -1;
        private Repeater _newsFeed;
        private Repeater _catPinn;
        private Repeater _catOne;
        private Repeater _catTwo;
        private Repeater _catThree;
        private Repeater _catFour;
        private string _cat = string.Empty;
        private string _year = string.Empty;
        private string _month = string.Empty;
        private string _detailPage = Guid.Empty.ToString();
        private string _detailUrl = string.Empty;
        private string _filter = string.Empty;
        private string _sortExpression = string.Empty;

        #region Properties
        public int ItemsLimit
        {
            get { return this._itemLimit; }
            set { this._itemLimit = value; }
        }

        [WebEditor("Telerik.Cms.Web.UI.CmsHyperLinkUrlWebEditor, Telerik.Cms")]
        public string DetailPage
        {
            get { return this._detailPage; }
            set { this._detailPage = value; }
        }

        public string FilterExpression
        {
            get { return this._filter; }
            set { this._filter = value; }
        }

        public string SortExpression
        {
            get { return this._sortExpression; }
            set { this._sortExpression = value; }
        }

        [DefaultValue(""), WebEditor("Telerik.FileManager.UrlWebEditor, Telerik.FileManager"), Category("Appearance"), Themeable(false)]
        public override string LayoutTemplatePath
        {
            get
            {
                if (string.IsNullOrEmpty(base.LayoutTemplatePath))
                    return "~/Sitefinity/ControlTemplates/Brafton/BraftonFeed.ascx";
                return base.LayoutTemplatePath;
            }
            set
            {
                base.LayoutTemplatePath = value;
            }
        }

        protected Webality.DisplayModes DisplayMode
        {
            get { return this._displayMode; }
            set { this._displayMode = value; }
        }
        #endregion

        public BraftonFeed()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        #region Controls
        protected Repeater NewsFeed
        {
            get
            {
                if (this._newsFeed == null)
                    this._newsFeed = base.Container.GetControl<Repeater>("newsFeed", true, Telerik.Framework.Web.TraverseMethod.BreadthFirst);
                return this._newsFeed;
            }
        }

        protected Repeater CatPinn
        {
            get
            {
                if (this._catPinn == null)
                    this._catPinn = base.Container.GetControl<Repeater>("catPinn", true, Telerik.Framework.Web.TraverseMethod.BreadthFirst);
                return this._catPinn;
            }
        }

        protected Repeater CatOne
        {
            get
            {
                if (this._catOne == null)
                    this._catOne = base.Container.GetControl<Repeater>("catOne", true, Telerik.Framework.Web.TraverseMethod.BreadthFirst);
                return this._catOne;
            }
        }

        protected Repeater CatTwo
        {
            get
            {
                if (this._catTwo == null)
                    this._catTwo = base.Container.GetControl<Repeater>("catTwo", true, Telerik.Framework.Web.TraverseMethod.BreadthFirst);
                return this._catTwo;
            }
        }

        protected Repeater CatThree
        {
            get
            {
                if (this._catThree == null)
                    this._catThree = base.Container.GetControl<Repeater>("catThree", true, Telerik.Framework.Web.TraverseMethod.BreadthFirst);
                return this._catThree;
            }
        }

        protected Repeater CatFour
        {
            get
            {
                if (this._catFour == null)
                    this._catFour = base.Container.GetControl<Repeater>("catFour", true, Telerik.Framework.Web.TraverseMethod.BreadthFirst);
                return this._catFour;
            }
        }
        #endregion

        #region Overridden Methods
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (this.Page != null)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.Params["mode"]))
                {
                    this.DisplayMode = (Webality.DisplayModes)Enum.Parse(typeof(Webality.DisplayModes), this.Page.Request.Params["mode"].ToUpper());
                }
                if (!string.IsNullOrEmpty(this.Page.Request.Params["cat"]))
                    this._cat = this.Page.Request.Params["cat"];
                if (!string.IsNullOrEmpty(this.Page.Request.Params["year"]))
                {
                    this._year = this.Page.Request.Params["year"];
                    this._month = this.Page.Request.Params["month"];
                }
            }
        }

        protected override void InitializeControls(Control controlContainer)
        {
            base.InitializeControls(controlContainer);
            switch (this.DisplayMode)
            {
                case DisplayModes.DETAIL:
                    this.Controls.Clear();
                    this.Visible = false;
                    return;
                case DisplayModes.LIST:
                    this.NewsFeed.ItemDataBound += new RepeaterItemEventHandler(this.NewsFeed_ItemDataBound);
                    _objectScope = BraftonContext.GetNewObjectScope();
                    if (!string.IsNullOrEmpty(this._cat))
                    {
                        var result = from n in _objectScope.Extent<Category>()
                                     where n.Url == this._cat
                                     select n;
                        if (result.Count() > 0)
                        {
                            Category cat = result.First();
                            var items = cat.NewsItems.AsQueryable();
                            //if (!string.IsNullOrEmpty(this._sortExpression))
                                items = items.OrderByDescending(i => i.PublishDate);
                            this.NewsFeed.DataSource = items;
                            this.NewsFeed.DataBind();
                        }
                    }
                    else if (!string.IsNullOrEmpty(this._year)) //it's a date archive
                    {
                        var result = from n in _objectScope.Extent<NewsItem>()
                                     where n.PublishDate.Year.ToString() == this._year && n.PublishDate.Month.ToString() == this._month
                                     select n;
                        if (result.Count() > 0)
                        {
                            result = result.OrderByDescending(i => i.PublishDate);
                            this.NewsFeed.DataSource = result;
                            this.NewsFeed.DataBind();
                        }
                    }
                    else
                    {
                        var result = from n in _objectScope.Extent<NewsItem>()
                                     select n;
                        if (!string.IsNullOrEmpty(this._filter))
                            result = result.Where(this._filter);
                        result = result.OrderByDescending(i => i.PublishDate);
                        if (this._itemLimit > 0)
                            result = result.Take(3);

                        if (result.Count() > 0)
                        {
                            this.NewsFeed.DataSource = result;
                            this.NewsFeed.DataBind();
                        }

                        //Categories

                        //Pinnacle News Category
                        this.CatPinn.ItemDataBound += new RepeaterItemEventHandler(this.NewsFeed_ItemDataBound);
                        _objectScope = BraftonContext.GetNewObjectScope();
                        var result0 = from n in _objectScope.Extent<Category>()
                                      where n.Url == "pinnacle-news"
                                      select n;
                        if (result0.Count() > 0)
                        {
                            Category cat = result0.First();
                            var items = cat.NewsItems.AsQueryable();
                            //if (!string.IsNullOrEmpty(this._sortExpression))
                            items = items.OrderByDescending(i => i.PublishDate).Take(2);
                            this.CatPinn.Visible = true;
                            this.CatPinn.DataSource = items;
                            this.CatPinn.DataBind();
                        }


                        //IBM News Category
                        this.CatOne.ItemDataBound += new RepeaterItemEventHandler(this.NewsFeed_ItemDataBound);
                        _objectScope = BraftonContext.GetNewObjectScope();
                        var result1 = from n in _objectScope.Extent<Category>()
                                      where n.Url == "ibm-news"
                                      select n;
                        if (result1.Count() > 0)
                        {
                            Category cat = result1.First();
                            var items = cat.NewsItems.AsQueryable();
                            //if (!string.IsNullOrEmpty(this._sortExpression))
                            items = items.OrderByDescending(i => i.PublishDate).Take(2);
                            this.CatOne.Visible = true;
                            this.CatOne.DataSource = items;
                            this.CatOne.DataBind();
                        }

                        //EMC News Category
                        this.CatTwo.ItemDataBound += new RepeaterItemEventHandler(this.NewsFeed_ItemDataBound);
                        _objectScope = BraftonContext.GetNewObjectScope();
                        var result2 = from n in _objectScope.Extent<Category>()
                                      where n.Url == "emc-news"
                                      select n;
                        if (result2.Count() > 0)
                        {
                            Category cat = result2.First();
                            var items = cat.NewsItems.AsQueryable();
                            //if (!string.IsNullOrEmpty(this._sortExpression))
                            items = items.OrderByDescending(i => i.PublishDate).Take(2);
                            this.CatTwo.Visible = true;
                            this.CatTwo.DataSource = items;
                            this.CatTwo.DataBind();
                        }

                        //VM Ware News Category
                        this.CatThree.ItemDataBound += new RepeaterItemEventHandler(this.NewsFeed_ItemDataBound);
                        _objectScope = BraftonContext.GetNewObjectScope();
                        var result3 = from n in _objectScope.Extent<Category>()
                                      where n.Url == "vmware-news"
                                      select n;
                        if (result3.Count() > 0)
                        {
                            Category cat = result3.First();
                            var items = cat.NewsItems.AsQueryable();
                            //if (!string.IsNullOrEmpty(this._sortExpression))
                            items = items.OrderByDescending(i => i.PublishDate).Take(2);
                            this.CatThree.Visible = true;
                            this.CatThree.DataSource = items;
                            this.CatThree.DataBind();
                        }

                        //Cisco News Category
                        this.CatFour.ItemDataBound += new RepeaterItemEventHandler(this.NewsFeed_ItemDataBound);
                        _objectScope = BraftonContext.GetNewObjectScope();
                        var result4 = from n in _objectScope.Extent<Category>()
                                      where n.Url == "cisco-news"
                                      select n;
                        if (result4.Count() > 0)
                        {
                            Category cat = result4.First();
                            var items = cat.NewsItems.AsQueryable();
                            //if (!string.IsNullOrEmpty(this._sortExpression))
                            items = items.OrderByDescending(i => i.PublishDate).Take(2);
                            this.CatFour.Visible = true;
                            this.CatFour.DataSource = items;
                            this.CatFour.DataBind();
                        }
                    }
                    break;
                case DisplayModes.RSS:
                    OutputRSS();
                    break;
            }
        }

        #endregion

        #region Methods

        protected void NewsFeed_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                for (int i = 0; i < 4; i++)
                {
                    Control control = e.Item.FindControl("NavigateUrl" + (i + 1));
                    if (control != null && control is HyperLink)
                    {
                        NewsItem item = (NewsItem)e.Item.DataItem;
                        ((HyperLink)control).NavigateUrl = GetPageUrl() + item.Url + UrlHelper.PageExtension;
                    }
                }
            }

        }

        public string GetPageUrl()
        {
            if (string.IsNullOrEmpty(this._detailUrl))
            {
                if (!this._detailPage.Equals(Guid.Empty.ToString()))
                {
                    this._detailUrl = CmsHelper.GetPageUrl(new Guid(this._detailPage), HttpContext.Current);
                }
                else if (this._detailPage.Equals(Guid.Empty.ToString()) && SiteMap.CurrentNode != null)
                {
                    this._detailUrl = SiteMap.CurrentNode.Url;
                }

                if (!string.IsNullOrEmpty(this._detailUrl))
                {
                    string ext = UrlHelper.PageExtension;
                    if (this._detailUrl.EndsWith(ext))
                        this._detailUrl = this._detailUrl.Replace(ext, "/");
                    if (!this._detailUrl.EndsWith("/"))
                        this._detailUrl += "/";
                    if (!string.IsNullOrEmpty(this._cat))
                        this._detailUrl += "-in/" + this._cat + "/";/*((!this._cat.EndsWith("/")) ? this._cat + "/" : this._cat);*/
                    if (!string.IsNullOrEmpty(this._year))
                        this._detailUrl += "-on/" + this._year + "/" + this._month + "/";
                }
            }
            return _detailUrl;
        }

        protected void OutputRSS()
        {
            IQueryable<NewsItem> res = Enumerable.Empty<NewsItem>().AsQueryable();
            _objectScope = BraftonContext.GetNewObjectScope();
            if (!string.IsNullOrEmpty(this._cat))
            {
                var result = from n in _objectScope.Extent<Category>()
                             where n.Url == this._cat
                             select n;
                if (result.Count() > 0)
                {

                    Category cat = result.First();
                    var items = cat.NewsItems.AsQueryable();
                    if (!string.IsNullOrEmpty(this._sortExpression))
                        items.OrderBy(this._sortExpression);
                    if (items.Count() > 0)
                    {
                        res = items;
                    }
                }
            }
            else
            {
                res = from n in _objectScope.Extent<NewsItem>()
                            select n;
                if (!string.IsNullOrEmpty(this._filter))
                    res.Where(this._filter);
                if (!string.IsNullOrEmpty(this._sortExpression))
                    res.OrderBy(this._sortExpression);
            }

            if (res.Count() > 0)
            {
                HttpResponse response = HttpContext.Current.Response;
                HttpRequest request = HttpContext.Current.Request;
                string url = GetPageUrl();
                response.Clear();
                response.ContentType = "text/xml";
                XmlTextWriter feedWriter
                  = new XmlTextWriter(response.OutputStream, Encoding.UTF8);

                feedWriter.WriteStartDocument();

                // These are RSS Tags
                feedWriter.WriteStartElement("rss");
                feedWriter.WriteAttributeString("version", "2.0");

                feedWriter.WriteStartElement("channel");
                feedWriter.WriteElementString("title", "Pinnacle Business Systems, Inc.");
                string feedUrl = request.Url.ToString();
                feedUrl = feedUrl.Replace("?" + request.QueryString.ToString(), "");
                feedWriter.WriteElementString("link", feedUrl);
                feedWriter.WriteElementString("description", "Pinnacle Business Systems, Inc.");
                feedWriter.WriteElementString("copyright", "Copyright 2010 pbsnow.com. All rights reserved.");


                url = request.Url.Scheme + "://" + request.Url.Host + this.Page.ResolveUrl(url);

                // Write all Posts in the rss feed
                foreach (NewsItem item in res)
                {
                    feedWriter.WriteStartElement("item");
                    feedWriter.WriteElementString("title", item.Headline);
                    feedWriter.WriteElementString("description", item.Extract);
                    feedWriter.WriteElementString("link", url + item.Url + UrlHelper.PageExtension);
                    feedWriter.WriteElementString("pubDate", item.PublishDate.ToString());
                    feedWriter.WriteEndElement();
                }

                // Close all open tags tags
                feedWriter.WriteEndElement();
                feedWriter.WriteEndElement();
                feedWriter.WriteEndDocument();
                feedWriter.Flush();
                feedWriter.Close();

                response.End();
            }
        }
        #endregion
    }
}