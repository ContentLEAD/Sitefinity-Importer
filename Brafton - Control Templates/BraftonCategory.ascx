<%@ Control Language="C#" %>

<asp:Repeater ID="categoryFeed" runat="server">
    <ItemTemplate>
        <p><asp:HyperLink ID="NavigateUrl1" runat="server" Text='<%# string.Format("{0} ({1})", DataBinder.Eval(Container.DataItem, "CategoryName"), DataBinder.Eval(Container.DataItem, "NewsItems.Count")) %>'></asp:HyperLink></p>
    </ItemTemplate>    
</asp:Repeater>
