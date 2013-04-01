<%@ Control Language="C#" %>

<telerik:CssFileLink ID="CssFileLink1" FileName="~/Sitefinity/ControlTemplates/News/newsCommonLayout.css" Media="screen" runat="server" />
<telerik:CssFileLink runat="server" FileName="~/CSS/jquery.marquee.css" />

<asp:Repeater ID="newsFeed" runat="server">
    <HeaderTemplate>
        <ul class="sf_newsList" id="marquee" class="marquee">
    </HeaderTemplate>
    <ItemTemplate>
        <li>
            <h2 class="sf_newsTitle"><asp:HyperLink ID="NavigateUrl1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Headline") %>'></asp:HyperLink><span class="sf_newsTS"><asp:Literal ID="newsTS" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PublishDate") %>'></asp:Literal></span></h2>
            <p class="sf_newsReadMore"><asp:HyperLink ID="NavigateUrl2" runat="server" Text="Read More"></asp:HyperLink></p>
		</li>
    </ItemTemplate>
    <FooterTemplate>
        </ul>
    </FooterTemplate>
</asp:Repeater>

 <div style="width:45%; float:left; display:none;">
    <h1 class="pageTitle">Pinnacle News</h1>
    	<asp:Repeater ID="catPinn" runat="server" Visible="false">
		<HeaderTemplate>
			<ol class="sf_newsList">
		</HeaderTemplate>
		<ItemTemplate>
		<li>
			<h2 class="sf_newsTitle"><asp:HyperLink ID="NavigateUrl1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Headline") %>'></asp:HyperLink></h2>
            <p><%# DataBinder.Eval(Container.DataItem, "Extract") %></p>
			<p class="sf_newsReadMore"><asp:HyperLink ID="NavigateUrl2" runat="server" Text="Read More"></asp:HyperLink></p>
		</li>
		</ItemTemplate>
		<FooterTemplate>
			</ol>
		</FooterTemplate>	
	</asp:Repeater>
    <h1 class="pageTitle">IBM News</h1>
    	<asp:Repeater ID="catOne" runat="server" Visible="false">
		<HeaderTemplate>
			<ol class="sf_newsList">
		</HeaderTemplate>
		<ItemTemplate>
		<li>
			<h2 class="sf_newsTitle"><asp:HyperLink ID="NavigateUrl1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Headline") %>'></asp:HyperLink></h2>
            <p><%# DataBinder.Eval(Container.DataItem, "Extract") %></p>
			<p class="sf_newsReadMore"><asp:HyperLink ID="NavigateUrl2" runat="server" Text="Read More"></asp:HyperLink></p>
		</li>
		</ItemTemplate>
		<FooterTemplate>
			</ol>
		</FooterTemplate>	
	</asp:Repeater>
        <h1 class="pageTitle">EMC News</h1>
    	<asp:Repeater ID="catTwo" runat="server" Visible="false">
		<HeaderTemplate>
			<ol class="sf_newsList">
		</HeaderTemplate>
		<ItemTemplate>
		<li>
			<h2 class="sf_newsTitle"><asp:HyperLink ID="NavigateUrl1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Headline") %>'></asp:HyperLink></h2>
            <p><%# DataBinder.Eval(Container.DataItem, "Extract") %></p>
			<p class="sf_newsReadMore"><asp:HyperLink ID="NavigateUrl2" runat="server" Text="Read More"></asp:HyperLink></p>
		</li>
		</ItemTemplate>
		<FooterTemplate>
			</ol>
		</FooterTemplate>	
	</asp:Repeater>
    </div>

    <div style="width:45%; float:right; display:none;">
    <h1 class="pageTitle">VMware News</h1>
    	<asp:Repeater ID="catThree" runat="server" Visible="false">
		<HeaderTemplate>
			<ol class="sf_newsList">
		</HeaderTemplate>
		<ItemTemplate>
		<li>
			<h2 class="sf_newsTitle"><asp:HyperLink ID="NavigateUrl1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Headline") %>'></asp:HyperLink></h2>
            <p><%# DataBinder.Eval(Container.DataItem, "Extract") %></p>
			<p class="sf_newsReadMore"><asp:HyperLink ID="NavigateUrl2" runat="server" Text="Read More"></asp:HyperLink></p>
		</li>
		</ItemTemplate>
		<FooterTemplate>
			</ol>
		</FooterTemplate>	
	</asp:Repeater>
        <h1 class="pageTitle">Cisco News</h1>
    	<asp:Repeater ID="catFour" runat="server" Visible="false">
		<HeaderTemplate>
			<ol class="sf_newsList">
		</HeaderTemplate>
		<ItemTemplate>
		<li>
			<h2 class="sf_newsTitle"><asp:HyperLink ID="NavigateUrl1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Headline") %>'></asp:HyperLink></h2>
            <p><%# DataBinder.Eval(Container.DataItem, "Extract") %></p>
			<p class="sf_newsReadMore"><asp:HyperLink ID="NavigateUrl2" runat="server" Text="Read More"></asp:HyperLink></p>
		</li>
		</ItemTemplate>
		<FooterTemplate>
			</ol>
		</FooterTemplate>	
	</asp:Repeater>
    </div>
<%-- 
NewsID
CreateDate
Extract
Headline
LastModifiedDate
PublishDate
State
Txt
Url
//--%>