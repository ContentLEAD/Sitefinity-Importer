<%@ Control Language="C#" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Cms.Engine.WebControls" Assembly="Telerik.Cms.Engine" %>
<script runat="server">
    private string _cat = string.Empty;
    private string _listingUrl = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.Page != null && SiteMap.CurrentNode != null)
        {
            if (!string.IsNullOrEmpty(this.Page.Request.Params["cat"]))
                this._cat = this.Page.Request.Params["cat"];
            
            this._listingUrl = SiteMap.CurrentNode.Url;
            string ext = Telerik.Cms.Web.UrlHelper.PageExtension;
            if (this._listingUrl.EndsWith(ext))
                this._listingUrl = this._listingUrl.Replace(ext, "");
            
            if (!string.IsNullOrEmpty(this._cat))
            {
                if (!string.IsNullOrEmpty(this._cat))
                    this._listingUrl += (this._listingUrl.EndsWith("/") ? "" : "/") + "-in/" + ((!this._cat.EndsWith("/")) ? this._cat.TrimEnd('/') : this._cat);
            }
            if (this._listingUrl.EndsWith("/"))
                this._listingUrl.TrimEnd('/') ;
            this._listingUrl += ext;
            
            this.returnLink.NavigateUrl = this._listingUrl;
        }
    }
</script>
<div class="articleContent">
    <telerik:GenericContent ID="articleContent" runat="server"></telerik:GenericContent>
</div>
<asp:HyperLink ID="returnLink" runat="server" Text="Back To Listings" CssClass="returnLink"></asp:HyperLink>
