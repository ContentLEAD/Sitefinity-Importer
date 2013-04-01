using BraftonData;
using System;
using System.Data;
using System.Collections.Generic;
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

namespace Webality
{
    /// <summary>
    /// Summary description for BraftonDateArchive
    /// </summary>
    public class BraftonDateArchive : SimpleControl
    {
        private Webality.DisplayModes _displayMode = Webality.DisplayModes.LIST;
        private IObjectScope _objectScope;
        private int _itemLimit = -1;
        private Repeater _dateFeed;
        private string _year = string.Empty;
        private string _month = string.Empty;
        private string _detailPage = Guid.Empty.ToString();
        private string _detailUrl = string.Empty;
        private string _sortExpression = string.Empty;
        private List<ArtDate> _dates = new List<ArtDate>();

        #region Properties
        /*public int ItemsLimit
        {
            get { return this._itemLimit; }
            set { this._itemLimit = value; }
        }*/

        [WebEditor("Telerik.Cms.Web.UI.CmsHyperLinkUrlWebEditor, Telerik.Cms")]
        public string DetailPage
        {
            get { return this._detailPage; }
            set { this._detailPage = value; }
        }

        /*public string SortExpression
        {
            get { return this._sortExpression; }
            set { this._sortExpression = value; }
        }*/

        [DefaultValue(""), WebEditor("Telerik.FileManager.UrlWebEditor, Telerik.FileManager"), Category("Appearance"), Themeable(false)]
        public override string LayoutTemplatePath
        {
            get
            {
                if (string.IsNullOrEmpty(base.LayoutTemplatePath))
                    return "~/Sitefinity/ControlTemplates/Brafton/BraftonDateArchive.ascx";
                return base.LayoutTemplatePath;
            }
            set
            {
                base.LayoutTemplatePath = value;
            }
        }

        //contains the month, year and a List of posts published this month; to be pushed into a List, one for each month at least one article was published
        //contains code to compare two together (to determine whether a new month needs to be pushed in)
        public class ArtDate
        {
            private int testMonth;

            public ArtDate(int m, int y)
            {
                this.Month = m;
                this.Year = y;
                this.NewsArticles = new List<NewsItem>();
                this.MonthToString();
            }

            public int Year { get; set; }
            public int Month {
                get
                {
                    return testMonth;
                }
                set
                {
                    if (value == 0)
                        throw new ArgumentException();
                    testMonth = value;
                } 
            }
            public string MonthName { get; set; }
            public List<NewsItem> NewsArticles { get; set; }

            //returns name of the month as a String
            public void MonthToString()
            {
                switch (Month)
                {
                    case 1: MonthName = "January"; break;
                    case 2: MonthName = "February"; break;
                    case 3: MonthName = "March"; break;
                    case 4: MonthName = "April"; break;
                    case 5: MonthName = "May"; break;
                    case 6: MonthName = "June"; break;
                    case 7: MonthName = "July"; break;
                    case 8: MonthName = "August"; break;
                    case 9: MonthName = "September"; break;
                    case 10: MonthName = "October"; break;
                    case 11: MonthName = "November"; break;
                    case 12: MonthName = "December"; break;
                    default: throw new ArgumentException(Month.ToString());//MonthName = "Beyond Time and Space"; break;
                }
            }

            public void Iterate(NewsItem n)
            {
                if(n.PublishDate.Month > 12 || n.PublishDate.Month < 1)
                    throw new ArgumentException();
                NewsArticles.Add(n);
            }
        }

        #endregion

        public BraftonDateArchive()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        #region Controls
        protected Repeater DateFeed
        {
            get
            {
                if (this._dateFeed == null)
                    this._dateFeed = base.Container.GetControl<Repeater>("dateFeed", true, Telerik.Framework.Web.TraverseMethod.BreadthFirst);
                return this._dateFeed;
            }
        }
        #endregion

        #region Overridden Methods
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (this.Page != null && !string.IsNullOrEmpty(this.Page.Request.Params["year"]))
            {
                this._year = this.Page.Request.Params["year"];
                this._month = this.Page.Request.Params["month"];
            }
        }

        protected override void InitializeControls(Control controlContainer)
        {
            base.InitializeControls(controlContainer);
            this.DateFeed.ItemDataBound += new RepeaterItemEventHandler(this.DateFeed_ItemDataBound);

            //establish connection to database containing Brafton info...consider using GetObjectScope() (no "new")
            _objectScope = BraftonContext.GetNewObjectScope(); 
            
            //This following chunk, declaring "result", simply pulls all NewsItem objects (read: Articles) from "BraftonContext" (db holding Brafton articles?)
            var result = from n in _objectScope.Extent<NewsItem>()
                         //where n.NewsItems.Count() > 0 
                         select n;


            //logic for determining number of months (with respective years), as well as number of posts per month
            //Concept: Iterate through articles, pushing months to a List as they come
            foreach (var item in result)
            {
                bool added = false;

                //if _dates is empty, add first month without checking
                if (_dates.Count < 1)
                {
                    var aDate = new ArtDate(item.PublishDate.Month, item.PublishDate.Year);
                    aDate.Iterate(item);
                    _dates.Add(aDate);
                    added = true;
                    continue;
                }

                //iterate the _dates list, comparing the month/year in each ArtDate in the list to item
                for (int i = 0; i < _dates.Count; i++)
                {
                    if (_dates[i].Month == item.PublishDate.Month && _dates[i].Year == item.PublishDate.Year)
                    {
                        _dates[i].Iterate(item);
                        added = true;
                        
                        break;
                    }
                }
                if (added == false) //no ArtDate objects with article's Month/Year, add a new one!
                {
                    var aDate = new ArtDate(item.PublishDate.Month, item.PublishDate.Year);
                    aDate.Iterate(item);
                    _dates.Add(aDate);
                }
            }

            //check that at least one date has been added to _dates list, bind to DateFeed repeater if so
            if (_dates.Count > 0)
            {
                this.DateFeed.DataSource = _dates;
                this.DateFeed.DataBind();
            }
        }

        #endregion

        #region Methods

        //Event handler for once NewsItem objects (articles) have been pulled and bound to repeater...
        //EDIT: Now the datasource is a List of dates, each date containing a list of NewsItems published in that month
        protected void DateFeed_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                for (int i = 0; i < 4; i++)
                {
                    Control control = e.Item.FindControl("NavigateUrl" + (i + 1));
                    if (control != null && control is HyperLink)
                    {
                        ArtDate item = (ArtDate)e.Item.DataItem;
                        //build out url in format ../news-item/-on/yyyy/mm.aspx
                        ((HyperLink)control).NavigateUrl = GetPageUrl() + "-on/" + item.Year.ToString() + "/" + item.Month.ToString() + UrlHelper.PageExtension;
                        if (!string.IsNullOrEmpty(this._year) && this._year == item.Year.ToString() && this._month == item.Month.ToString())
                        {
                            ((WebControl)control).CssClass = "selected";
                        }
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
                        this._detailUrl = this._detailUrl.Replace(ext, "");
                    if (!this._detailUrl.EndsWith("/"))
                        this._detailUrl += "/";
                }
            }
            return _detailUrl;
        }
        #endregion
    }
}