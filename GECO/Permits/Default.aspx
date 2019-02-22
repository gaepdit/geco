<%@ Page Language="vb" MasterPageFile="~/MainMaster.master" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="GECO.PermitDefault" Title="Air Quality Permits" %>

<%@ MasterType VirtualPath="~/MainMaster.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="FullContent" runat="Server">
    <h1>Air Quality Permits and Permit Applications</h1>

    <p>
        <b>
            <asp:Label ID="lblFacilityDisplay" runat="server"></asp:Label>
            <br />
            AIRS No:
            <asp:Label ID="lblAIRS" runat="server"></asp:Label>
        </b>
    </p>

    <h2>Open Permit Applications</h2>
    <p id="pNoOpenApps" runat="server">None.</p>
    <asp:GridView ID="grdOpenApps" runat="server" CssClass="table-simple table-menu" Visible="false" AutoGenerateColumns="False">
        <Columns>
            <asp:HyperLinkField DataTextField="Application #" HeaderText="Application #" ItemStyle-CssClass="table-cell-alignright"
                DataNavigateUrlFields="Application #" DataNavigateUrlFormatString="~/Permits/Application.aspx?id={0}" />
            <asp:BoundField HeaderText="Date Received" DataField="Date Received" DataFormatString="{0:dd-MMM-yyyy}" />
            <asp:BoundField HeaderText="Application Type" DataField="Type" />
            <asp:BoundField HeaderText="Action Type" DataField="Action Type" />
            <asp:BoundField HeaderText="Current Status" DataField="Status" />
            <asp:BoundField HeaderText="Status Date" DataField="Status Date" DataFormatString="{0:dd-MMM-yyyy}" />
        </Columns>
    </asp:GridView>

    <h2>Air Quality Permits</h2>
    <p id="pNoCurrentPermits" runat="server">None.</p>
    <p id="pYesCurrentPermits" runat="server">
        Issued permits can be downloaded at the
        <asp:HyperLink ID="hlPermitSearch" runat="server" Target="_blank">Permit Search Engine</asp:HyperLink>.
    </p>
    <asp:GridView ID="grdCurrentPermits" runat="server" CssClass="table-simple" Visible="false" AutoGenerateColumns="False">
        <Columns>
            <asp:BoundField HeaderText="Permit #" DataField="Permit #" />
            <asp:BoundField HeaderText="Date Issued" DataField="Date Issued" DataFormatString="{0:dd-MMM-yyyy}" />
            <asp:BoundField HeaderText="Active" DataField="Active" ItemStyle-CssClass="table-cell-aligncenter" />
            <asp:BoundField HeaderText="Date Revoked" DataField="Date Revoked" DataFormatString="{0:dd-MMM-yyyy}" />
        </Columns>
    </asp:GridView>

    <h2>Closed Permit Applications</h2>
    <p id="pNoClosedApps" runat="server">None.</p>
    <asp:GridView ID="grdClosedApps" runat="server" CssClass="table-simple table-menu" Visible="false" AutoGenerateColumns="False">
        <Columns>
            <asp:HyperLinkField DataTextField="Application #" HeaderText="Application #" ItemStyle-CssClass="table-cell-alignright"
                DataNavigateUrlFields="Application #" DataNavigateUrlFormatString="~/Permits/Application.aspx?id={0}" />
            <asp:BoundField HeaderText="Date Received" DataField="Date Received" DataFormatString="{0:dd-MMM-yyyy}" />
            <asp:BoundField HeaderText="Application Type" DataField="Type" />
            <asp:BoundField HeaderText="Action Type" DataField="Action Type" />
            <asp:BoundField HeaderText="Date Closed Out" DataField="Status Date" DataFormatString="{0:dd-MMM-yyyy}" />
        </Columns>
    </asp:GridView>

</asp:Content>
