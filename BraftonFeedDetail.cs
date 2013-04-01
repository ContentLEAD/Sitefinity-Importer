using BraftonData;
using System;
using System.Data;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Telerik.Cms;
using Telerik.Cms.Engine.WebControls;
using Telerik.Cms.Web;
using Telerik.Cms.Web.UI;
using Telerik.OpenAccess;

namespace Webality
{

    /// <summary>
    /// Summary description for BraftonFeedDetail
    /// </summary>
    public class BraftonFeedDetail : SimpleControl
    {
        #region Private Variables
        private Webality.DisplayModes _displayMode = Webality.DisplayModes.LIST;
        private GenericContent _content;
        private IObjectScope _objectScope;
        private string _listPage = Guid.Empty.ToString();
        private string _providerName = "/";
        #endregion

        #region Properties

        [WebEditor("Telerik.Cms.Web.UI.CmsHyperLinkUrlWebEditor, Telerik.Cms")]
        public string DetailPage
        {
            get { return this._listPage; }
            set { this._listPage = value; }
        }

        [Category("Appearance"), WebEditor("Telerik.Cms.Engine.WebControls.ContentSelector, Telerik.Cms.Engine")]
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

        [DefaultValue(""), WebEditor("Telerik.FileManager.UrlWebEditor, Telerik.FileManager"), Category("Appearance"), Themeable(false)]
        public override string LayoutTemplatePath
        {
            get
            {
                if (string.IsNullOrEmpty(base.LayoutTemplatePath))
                    return "~/Sitefinity/ControlTemplates/Brafton/BraftonFeedDetail.ascx";
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
        #endregion

        public BraftonFeedDetail()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        #region Controls
        protected GenericContent Content
        {
            get
            {
                if (this._content == null)
                    this._content = base.Container.GetControl<GenericContent>("articleContent", true, Telerik.Framework.Web.TraverseMethod.BreadthFirst);
                return this._content;
            }
        }
        #endregion
        

        #region Overridden Methods
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (this.Page != null && !string.IsNullOrEmpty(this.Page.Request.Params["mode"]))
                this.DisplayMode = (Webality.DisplayModes)Enum.Parse(typeof(Webality.DisplayModes), this.Page.Request.Params["mode"].ToUpper());
        }

        protected override void InitializeControls(Control controlContainer)
        {
            base.InitializeControls(controlContainer);
            if (this.DisplayMode == DisplayModes.LIST)
            {
                this.Controls.Clear();
                this.Visible = false;
                return;
            }
            else
            {
                if (this.Page != null)
                {
                    string newsUrl = this.Page.Request.Params["article"];
                    if (!string.IsNullOrEmpty(newsUrl))
                    {
                        if (newsUrl.Contains(".aspx"))
                        {
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
                            this.Content.ContentID = this.ArticleContent;
                            string html = this.Content.SharedContent.Content.ToString();
                            this.Content.Content = ReplaceTokens((NewsItem)result.First(), html);
                        }
                    }
                }
            }
        }
        #endregion

        #region Methods
        protected string ReplaceTokens(NewsItem item, string content)
        {
            string tokenized = content;
            foreach (PropertyInfo prop in item.GetType().GetProperties())
            {
                object propValue = prop.GetValue(item, null);
                if ((propValue != null) && !string.IsNullOrEmpty(propValue.ToString()))
                {
                    tokenized = tokenized.Replace("%%" + prop.Name + "%%", propValue.ToString());
                }
            }
            return tokenized;
        }
        #endregion
    }
}