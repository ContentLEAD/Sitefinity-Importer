<%@ Control Language="C#" %>

<asp:Repeater ID="dateFeed" runat="server">
    <ItemTemplate>
        <p><asp:HyperLink ID="NavigateUrl1" runat="server" Text='<%# string.Format("{0} {1} ({2})", DataBinder.Eval(Container.DataItem, "MonthName"), DataBinder.Eval(Container.DataItem, "Year"), DataBinder.Eval(Container.DataItem, "NewsArticles.Count")) %>'></asp:HyperLink></p>
    </ItemTemplate>    
</asp:Repeater>
