<%@ Page Language="VB" MasterPageFile="~/Main.master" AutoEventWireup="false"
    Inherits="GECO.FacilityHome" Title="GECO Facility Home" CodeBehind="Default.aspx.vb" %>

<%@ MasterType VirtualPath="~/Main.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">

    <ul class="menu-list-horizontal">
        <li>
            <asp:HyperLink ID="lnkFacilityHome" runat="server" NavigateUrl="~/Facility/"
                Enabled="false" CssClass="selected-menu-item disabled">
                Home
            </asp:HyperLink>
        </li>
        <li>
            <asp:HyperLink ID="lnkFacilityInfo" runat="server" NavigateUrl="~/Facility/Summary.aspx">Facility Info</asp:HyperLink>
        </li>
        <li>
            <asp:HyperLink ID="lnkFacilityAdmin" runat="server" NavigateUrl="~/Facility/Admin.aspx">User Access</asp:HyperLink>
        </li>
        <li>
            <asp:HyperLink ID="lnkFacilityContacts" runat="server" NavigateUrl="~/Facility/Contacts.aspx">Communication Preferences</asp:HyperLink>
        </li>
    </ul>

    <h1>Facility Home</h1>

    <asp:Table ID="AppTable" runat="server" CssClass="table-simple table-menu table-bordered" aria-label="Available GECO modules">
        <asp:TableHeaderRow ID="AppsHeader" runat="server" BackColor="#F0F0F6" CssClass="table-head">
            <asp:TableHeaderCell Text="GECO Applications" runat="server" />
            <asp:TableHeaderCell Text="Description" runat="server" />
            <asp:TableHeaderCell Text="Current Status" runat="server" />
        </asp:TableHeaderRow>

        <asp:TableRow ID="AppsPermits" runat="server">
            <asp:TableHeaderCell runat="server" CssClass="table-cell-link">
                <asp:HyperLink ID="PALink" runat="server" NavigateUrl="~/Permits/">Permits & Permit Application Fees</asp:HyperLink>
            </asp:TableHeaderCell>
            <asp:TableCell runat="server">
                View current and historical air quality permits and permit applications, as well as application fee invoices.
            </asp:TableCell>
            <asp:TableCell runat="server">
                <asp:Literal ID="litPermits" runat="server" />
            </asp:TableCell>
        </asp:TableRow>

        <asp:TableRow ID="AppsEmissionFees" runat="server">
            <asp:TableHeaderCell runat="server" CssClass="table-cell-link">
                <asp:HyperLink ID="EFLink" runat="server" NavigateUrl="~/AnnualFees/">Annual Emissions Fees</asp:HyperLink>
            </asp:TableHeaderCell>
            <asp:TableCell runat="server">
                Report annual emissions and calculate annual emissions fees.
            </asp:TableCell>
            <asp:TableCell runat="server">
                <asp:Literal ID="litEmissionsFees" runat="server" />
            </asp:TableCell>
        </asp:TableRow>

        <asp:TableRow ID="AppsFeesSummary" runat="server">
            <asp:TableHeaderCell runat="server" CssClass="table-cell-link">
                <asp:HyperLink ID="PFLink" runat="server" NavigateUrl="~/Fees/">Fees Summary</asp:HyperLink>
            </asp:TableHeaderCell>
            <asp:TableCell runat="server">
                View a summary of invoices and deposits for permit application fees and annual emissions fees.
            </asp:TableCell>
            <asp:TableCell></asp:TableCell>
        </asp:TableRow>

        <asp:TableRow ID="AppsEmissionInventory" runat="server">
            <asp:TableHeaderCell runat="server" CssClass="table-cell-link">
                <asp:HyperLink ID="EisLink" runat="server" Text="Emissions Inventory" NavigateUrl="~/EIS/" />
            </asp:TableHeaderCell>
            <asp:TableCell runat="server">
                Access the Emissions Inventory reporting process and historical data.
            </asp:TableCell>
            <asp:TableCell runat="server">
                <asp:Literal ID="litEmissionsInventory" runat="server" />
            </asp:TableCell>
        </asp:TableRow>

        <asp:TableRow ID="AppsEmissionsStatement" runat="server">
            <asp:TableHeaderCell runat="server" CssClass="table-cell-link">
                <asp:HyperLink ID="ESLink" runat="server" Text="Emissions Statement" Enabled="False" />
            </asp:TableHeaderCell>
            <asp:TableCell runat="server">
                <span class="text-error">
                    Effective June 19, 2023, facilities are no longer required to submit Emissions Statement data.
                </span><br />
                Please see the
                <a href="https://epd.georgia.gov/forms-permits/air-protection-branch-forms-permits/point-source-emissions-inventory#toc-other-updates-2"
                   target="_blank" rel="noopener">
                    APB website
                </a>
                for more information.
            </asp:TableCell>
            <asp:TableCell></asp:TableCell>
        </asp:TableRow>

        <asp:TableRow ID="AppsTestNotifications" runat="server">
            <asp:TableHeaderCell runat="server" CssClass="table-cell-link">
                <asp:HyperLink ID="TNLink" runat="server" Text="Test Notifications" NavigateUrl="~/TN/" />
            </asp:TableHeaderCell>
            <asp:TableCell>
                View all performance test notifications for your facility.
            </asp:TableCell>
            <asp:TableCell runat="server">
                <asp:Literal ID="litTestNotifications" runat="server" />
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>

</asp:Content>
