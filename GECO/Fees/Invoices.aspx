<%@ Page Language="VB" MasterPageFile="~/MainMaster.master" AutoEventWireup="false" Inherits="GECO.Fees_Invoices" Title="GECO Invoices" CodeBehind="Invoices.aspx.vb" %>

<%@ MasterType VirtualPath="~/MainMaster.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="FullContent" runat="server">
    <h1>Permit Fees Invoices</h1>

    <p>
        <b>
            <asp:Label ID="lblFacilityDisplay" runat="server"></asp:Label>
            <br />
            AIRS No:
            <asp:Label ID="lblAIRS" runat="server"></asp:Label>
        </b>
    </p>

    <ul class="menu-list-horizontal">
        <li><a href="Default.aspx">Permit Fees Summary</a></li>
        <li><span class="selected-menu-item">Invoices</span></li>
        <li><a href="Deposits.aspx">Deposits</a></li>
    </ul>

    <h2>Annual/Emissions Fees</h2>

    <p id="pAnnualInvoices" runat="server" visible="false">None.</p>

    <asp:GridView ID="grdAnnualInvoices" runat="server" CssClass="table-simple" Visible="true" AutoGenerateColumns="False">
        <Columns>
            <asp:HyperLinkField DataTextField="Fee Year" HeaderText="Fee Year" ItemStyle-CssClass="table-cell-link"
                DataNavigateUrlFields="Fee Year" DataNavigateUrlFormatString="~/AnnualFees/Invoice.aspx?FeeYear={0}" />
            <asp:BoundField DataField="Invoice Date" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}" ItemStyle-CssClass="table-cell-alignright" />
            <asp:BoundField DataField="Invoice Amount" HeaderText="Amount" DataFormatString="{0:c}" ItemStyle-CssClass="table-cell-alignright" />
            <asp:BoundField DataField="Invoice ID" HeaderText="Invoice ID" />
            <asp:BoundField DataField="Invoice Type" HeaderText="Schedule" />
            <asp:BoundField DataField="Status" HeaderText="Status" />
        </Columns>
    </asp:GridView>

    <h2>Permit Application Fees</h2>

    <p id="pApplicationInvoices" runat="server" visible="false">None.</p>

    <asp:GridView ID="grdApplicationInvoices" runat="server" CssClass="table-simple" Visible="true" AutoGenerateColumns="False">
        <Columns>
            <asp:HyperLinkField DataTextField="Invoice ID" HeaderText="Invoice #" ItemStyle-CssClass="table-cell-alignright table-cell-link"
                DataNavigateUrlFields="InvoiceGuid" DataNavigateUrlFormatString="~/Invoice/?id={0}" />
            <asp:BoundField DataField="Date" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}" ItemStyle-CssClass="table-cell-alignright" />
            <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:c}" ItemStyle-CssClass="table-cell-alignright" />
            <asp:HyperLinkField DataTextField="Permit Application" HeaderText="Permit Application" ItemStyle-CssClass="table-cell-link"
                DataNavigateUrlFields="Permit Application" DataNavigateUrlFormatString="~/Permits/Application.aspx?id={0}" />
            <asp:BoundField DataField="Schedule" HeaderText="Schedule" />
            <asp:BoundField DataField="Status" HeaderText="Status" />
            <asp:BoundField DataField="Balance" HeaderText="Balance" DataFormatString="{0:c}" ItemStyle-CssClass="table-cell-alignright" />
        </Columns>
    </asp:GridView>

</asp:Content>
