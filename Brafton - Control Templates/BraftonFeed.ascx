<%@ Control Language="C#" %>
<%@ Register TagName="DateArchive" TagPrefix="ba" Src="~/Sitefinity/ControlTemplates/Brafton/BraftonDateArchive.ascx" %>
<%@ Import Namespace="BraftonData" %>
<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="Telerik.OpenAccess" %>
<script runat="server">
    private string _cat = string.Empty;
    private string _year = string.Empty;
    private string _month = string.Empty;
    private Webality.DisplayModes _displayMode = Webality.DisplayModes.LIST;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.Page != null)
        {
            if (!string.IsNullOrEmpty(this.Page.Request.Params["mode"]))
                this._displayMode = (Webality.DisplayModes)Enum.Parse(typeof(Webality.DisplayModes), this.Page.Request.Params["mode"].ToUpper());
            
            if (!string.IsNullOrEmpty(this.Page.Request.Params["cat"]))
                this._cat = this.Page.Request.Params["cat"];
            if (!string.IsNullOrEmpty(this.Page.Request.Params["year"]))
            {
                this._year = this.Page.Request.Params["year"];
                this._month = this.Page.Request.Params["month"];
            }
            if (!string.IsNullOrEmpty(this._cat))
            {
                Telerik.OpenAccess.IObjectScope scope = BraftonContext.GetNewObjectScope();
                var category = from n in scope.Extent<Category>()
                               where n.Url == this._cat
                               select n;
                if (category.Count() > 0)
                    pageTitle.Text = "<h1 class=\"pageTitle\">" + category.First().CategoryName + "</h1>";
                this.returnLink.Visible = true;
                this.returnLink.NavigateUrl = SiteMap.CurrentNode.Url;
            }
            else if (!string.IsNullOrEmpty(this._year))
            {
                string monthName = string.Empty;
                switch (this._month)
                {
                    case "1": monthName = "January"; break;
                    case "2": monthName = "February"; break;
                    case "3": monthName = "March"; break;
                    case "4": monthName = "April"; break;
                    case "5": monthName = "May"; break;
                    case "6": monthName = "June"; break;
                    case "7": monthName = "July"; break;
                    case "8": monthName = "August"; break;
                    case "9": monthName = "September"; break;
                    case "10": monthName = "October"; break;
                    case "11": monthName = "November"; break;
                    case "12": monthName = "December"; break;
                    default: monthName = "Beyond Time and Space"; break;
                }
                pageTitle.Text = "<h1 class=\"pageTitle\">Archives for " + monthName + " " + this._year + "</h1>";
                this.returnLink.Visible = true;
                this.returnLink.NavigateUrl = SiteMap.CurrentNode.Url;
            }
			else
			{
				pageTitle.Text = "<h1 class=\"pageTitle\">Latest News</h1>";
                catOneTitle.Visible = true;
                catTwoTitle.Visible = true;
                catThreeTitle.Visible = true;
                catFourTitle.Visible = true;
			}
        }
    }
</script>
<telerik:CssFileLink ID="CssFileLink1" FileName="~/Sitefinity/ControlTemplates/News/newsCommonLayout.css" Media="screen" runat="server" />
<ba:DateArchive ID="BraftonArchiveTest" runat="server" />
<div class="articleFeed">
	<asp:Literal ID="pageTitle" runat="server"></asp:Literal>
	<asp:HyperLink ID="returnLink" runat="server" Text="Back To Latest News" CssClass="returnLink" Visible="false"></asp:HyperLink>
	<asp:Repeater ID="newsFeed" runat="server">
		<HeaderTemplate>
			<ol class="sf_newsList">
		</HeaderTemplate>
		<ItemTemplate>
		<li>
			<h2 class="sf_newsTitle"><asp:HyperLink ID="NavigateUrl1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Headline") %>'></asp:HyperLink></h2>
            <p>Posted on <%# DataBinder.Eval(Container.DataItem, "PublishDate", "{0:MMMM d, yyyy}")%></p>
            <p><%# DataBinder.Eval(Container.DataItem, "Extract") %><span class="sf_newsReadMore"><asp:HyperLink ID="NavigateUrl2" runat="server" Text=" Read More"></asp:HyperLink></p>
		</li>
		</ItemTemplate>
		<FooterTemplate>
			</ol>
		</FooterTemplate>	
	</asp:Repeater>
    <div style="width:45%; float:left;">
    <h1 id="pinnacleCatTitle" runat="server" visible="false" class="pageTitle">Pinnacle News</h1>
    	<asp:Repeater ID="catPinn" runat="server" Visible="false">
		<HeaderTemplate>
			<ol class="sf_newsList">
		</HeaderTemplate>
		<ItemTemplate>
		<li>
			<h2 class="sf_newsTitle"><asp:HyperLink ID="NavigateUrl1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Headline") %>'></asp:HyperLink></h2>
            <p>Posted on <%# DataBinder.Eval(Container.DataItem, "PublishDate", "{0:MMMM d, yyyy}")%></p>
            <p><%# DataBinder.Eval(Container.DataItem, "Extract") %><span class="sf_newsReadMore"><asp:HyperLink ID="NavigateUrl2" runat="server" Text=" Read More"></asp:HyperLink></p>
		</li>
		</ItemTemplate>
		<FooterTemplate>
			</ol>
		</FooterTemplate>	
	</asp:Repeater>
    <h1 id="catOneTitle" runat="server" visible="false" class="pageTitle">IBM News</h1>
    	<asp:Repeater ID="catOne" runat="server" Visible="false">
		<HeaderTemplate>
			<ol class="sf_newsList">
		</HeaderTemplate>
		<ItemTemplate>
		<li>
			<h2 class="sf_newsTitle"><asp:HyperLink ID="NavigateUrl1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Headline") %>'></asp:HyperLink></h2>
            <p>Posted on <%# DataBinder.Eval(Container.DataItem, "PublishDate", "{0:MMMM d, yyyy}")%></p>
            <p><%# DataBinder.Eval(Container.DataItem, "Extract") %><span class="sf_newsReadMore"><asp:HyperLink ID="NavigateUrl2" runat="server" Text=" Read More"></asp:HyperLink></p>
		</li>
		</ItemTemplate>
		<FooterTemplate>
			</ol>
		</FooterTemplate>	
	</asp:Repeater>
        <h1 id="catTwoTitle" runat="server" visible="false" class="pageTitle">EMC News</h1>
    	<asp:Repeater ID="catTwo" runat="server" Visible="false">
		<HeaderTemplate>
			<ol class="sf_newsList">
		</HeaderTemplate>
		<ItemTemplate>
		<li>
			<h2 class="sf_newsTitle"><asp:HyperLink ID="NavigateUrl1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Headline") %>'></asp:HyperLink></h2>
            <p>Posted on <%# DataBinder.Eval(Container.DataItem, "PublishDate", "{0:MMMM d, yyyy}")%></p>
            <p><%# DataBinder.Eval(Container.DataItem, "Extract") %><span class="sf_newsReadMore"><asp:HyperLink ID="NavigateUrl2" runat="server" Text=" Read More"></asp:HyperLink></p>
		</li>
		</ItemTemplate>
		<FooterTemplate>
			</ol>
		</FooterTemplate>	
	</asp:Repeater>
    </div>

    <div style="width:45%; float:right;">
    <h1 id="catThreeTitle" runat="server" visible="false" class="pageTitle">VMware News</h1>
    	<asp:Repeater ID="catThree" runat="server" Visible="false">
		<HeaderTemplate>
			<ol class="sf_newsList">
		</HeaderTemplate>
		<ItemTemplate>
		<li>
			<h2 class="sf_newsTitle"><asp:HyperLink ID="NavigateUrl1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Headline") %>'></asp:HyperLink></h2>
            <p>Posted on <%# DataBinder.Eval(Container.DataItem, "PublishDate", "{0:MMMM d, yyyy}")%></p>
            <p><%# DataBinder.Eval(Container.DataItem, "Extract") %><span class="sf_newsReadMore"><asp:HyperLink ID="NavigateUrl2" runat="server" Text=" Read More"></asp:HyperLink></p>
		</li>
		</ItemTemplate>
		<FooterTemplate>
			</ol>
		</FooterTemplate>	
	</asp:Repeater>
        <h1 id="catFourTitle" runat="server" visible="false" class="pageTitle">Cisco News</h1>
    	<asp:Repeater ID="catFour" runat="server" Visible="false">
		<HeaderTemplate>
			<ol class="sf_newsList">
		</HeaderTemplate>
		<ItemTemplate>
		<li>
			<h2 class="sf_newsTitle"><asp:HyperLink ID="NavigateUrl1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Headline") %>'></asp:HyperLink></h2>
            <p>Posted on <%# DataBinder.Eval(Container.DataItem, "PublishDate", "{0:MMMM d, yyyy}")%></p>
            <p><%# DataBinder.Eval(Container.DataItem, "Extract") %><span class="sf_newsReadMore"><asp:HyperLink ID="NavigateUrl2" runat="server" Text=" Read More"></asp:HyperLink></p>
		</li>
		</ItemTemplate>
		<FooterTemplate>
			</ol>
		</FooterTemplate>	
	</asp:Repeater>
    </div>
</div>
