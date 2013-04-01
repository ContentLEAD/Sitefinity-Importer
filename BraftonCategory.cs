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

namespace Webality
{

    /// <summary>
    /// Summary description for BraftonFeed
    /// </summary>
    public class BraftonCategory : SimpleControl
    {
        private Webality.DisplayModes _displayMode = Webality.DisplayModes.LIST;
        private IObjectScope _objectScope;
        private int _itemLimit = -1;
        private Repeater _categoryFeed;
        private string _cat = string.Empty;
        private string _detailPage = Guid.Empty.ToString();
        private string _detailUrl = string.Empty;
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
                    return "~/Sitefinity/ControlTemplates/Brafton/BraftonCategory.ascx";
                return base.LayoutTemplatePath;
            }
            set
            {
                base.LayoutTemplatePath = value;
            }
        }
        #endregion

        public BraftonCategory()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        #region Controls
        protected Repeater CategoryFeed
        {
            get
            {
                if (this._categoryFeed == null)
                    this._categoryFeed = base.Container.GetControl<Repeater>("categoryFeed", true, Telerik.Framework.Web.TraverseMethod.BreadthFirst);
                return this._categoryFeed;
            }
        }
        #endregion

        #region Overridden Methods
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (this.Page != null && !string.IsNullOrEmpty(this.Page.Request.Params["cat"]))
            {
                this._cat = this.Page.Request.Params["cat"];
            }
        }

        protected override void InitializeControls(Control controlContainer)
        {
            base.InitializeControls(controlContainer);
            this.CategoryFeed.ItemDataBound += new RepeaterItemEventHandler(this.CategoryFeed_ItemDataBound);
            _objectScope = BraftonContext.GetNewObjectScope();
            var result = from n in _objectScope.Extent<Category>()
                         where n.NewsItems.Count() > 0
                         select n;
            if (result.Count() > 0)
            {
                this.CategoryFeed.DataSource = result;
                this.CategoryFeed.DataBind();
            }
        }

        #endregion

        #region Methods

        protected void CategoryFeed_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                for (int i = 0; i < 4; i++)
                {
                    Control control = e.Item.FindControl("NavigateUrl" + (i + 1));
                    if (control != null && control is HyperLink)
                    {
                        Category item = (Category)e.Item.DataItem;
                        ((HyperLink)control).NavigateUrl = GetPageUrl() + "-in/" + item.Url + UrlHelper.PageExtension;
                        if (!string.IsNullOrEmpty(this._cat) && this._cat == item.Url)
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