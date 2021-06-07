<%@ Page Language="VB" MasterPageFile="~/Main.master" AutoEventWireup="false" Inherits="GECO.FacilityHome" Title="GECO Facility Home" CodeBehind="Default.aspx.vb" %>

<%@ MasterType VirtualPath="~/Main.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
    <h1>Facility Home</h1>

    <p>
        <b>
            <asp:Label ID="lblFacilityDisplay" runat="server" />
        </b>
        <br />
        AIRS Number:
        <asp:Label ID="lblAIRS" runat="server" />
    </p>

    <ul class="menu-list-horizontal">
        <li>
            <asp:HyperLink ID="lnkFacilityHome" runat="server" NavigateUrl="~/Facility/" Enabled="false" CssClass="selected-menu-item disabled">Menu</asp:HyperLink>
        </li>
        <li>
            <asp:HyperLink ID="lnkFacilityInfo" runat="server" NavigateUrl="~/Facility/Summary.aspx">Facility Info</asp:HyperLink>
        </li>
        <li>
            <asp:HyperLink ID="lnkFacilityAdmin" runat="server" NavigateUrl="~/Facility/Admin.aspx">User Access</asp:HyperLink>
        </li>
    </ul>

    <h2>Application Menu</h2>

    <asp:Table ID="AppTable" runat="server" CssClass="table-simple table-menu table-bordered">
        <asp:TableHeaderRow ID="AppsHeader" runat="server" BackColor="#F0F0F6" CssClass="table-head">
            <asp:TableHeaderCell Text="GECO Applications" runat="server" />
            <asp:TableHeaderCell Text="Current Status" runat="server" />
        </asp:TableHeaderRow>

        <asp:TableRow ID="AppsPermits" runat="server">
            <asp:TableHeaderCell runat="server" CssClass="table-cell-link">
                <asp:HyperLink ID="PALink" runat="server" NavigateUrl="~/Permits/">Permits & Application Fees</asp:HyperLink>
            </asp:TableHeaderCell>
            <asp:TableCell runat="server">
                <asp:Literal ID="litPermits" runat="server" />
            </asp:TableCell>
        </asp:TableRow>

        <asp:TableRow ID="AppsEmissionFees" runat="server">
            <asp:TableHeaderCell runat="server" CssClass="table-cell-link">
                <asp:HyperLink ID="EFLink" runat="server" NavigateUrl="~/AnnualFees/">Annual/Emissions Fees</asp:HyperLink>
            </asp:TableHeaderCell>
            <asp:TableCell runat="server">
                <asp:Literal ID="litEmissionsFees" runat="server" />
            </asp:TableCell>
        </asp:TableRow>

        <asp:TableRow ID="AppsFeesSummary" runat="server">
            <asp:TableHeaderCell runat="server" CssClass="table-cell-link">
                <asp:HyperLink ID="PFLink" runat="server" NavigateUrl="~/Fees/">Fees Summary</asp:HyperLink>
            </asp:TableHeaderCell>
            <asp:TableCell runat="server" />
        </asp:TableRow>

        <asp:TableRow ID="AppsEmissionInventory" runat="server">
            <asp:TableHeaderCell runat="server" CssClass="table-cell-link">
                <asp:HyperLink ID="EisLink" runat="server" Text="Emissions Inventory" NavigateUrl="~/EIS/" />
            </asp:TableHeaderCell>
            <asp:TableCell runat="server">
                <asp:Literal ID="litEmissionsInventory" runat="server" />
            </asp:TableCell>
        </asp:TableRow>

        <asp:TableRow ID="AppsEmissionsStatement" runat="server">
            <asp:TableHeaderCell runat="server" CssClass="table-cell-link">
                <asp:HyperLink ID="ESLink" runat="server" Text="Emissions Statement" NavigateUrl="~/ES/" />
            </asp:TableHeaderCell>
            <asp:TableCell runat="server">
                <asp:Literal ID="litEmissionsStatement" runat="server" />
            </asp:TableCell>
        </asp:TableRow>

        <asp:TableRow ID="AppsTestNotifications" runat="server">
            <asp:TableHeaderCell runat="server" CssClass="table-cell-link">
                <asp:HyperLink ID="TNLink" runat="server" Text="Test Notifications" NavigateUrl="~/TN/" />
            </asp:TableHeaderCell>
            <asp:TableCell runat="server">
                <asp:Literal ID="litTestNotifications" runat="server" />
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
</asp:Content>
