<%@ Page Language="VB" MasterPageFile="~/MainMaster.master" AutoEventWireup="false" Inherits="GECO.Fees_Default" Title="GECO Permit Fees" CodeBehind="Default.aspx.vb" %>

<%@ MasterType VirtualPath="~/MainMaster.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="FullContent" runat="server">
    <h1>Permit Fees</h1>

    <p>
        <b>
            <asp:Label ID="lblFacilityDisplay" runat="server"></asp:Label>
            <br />
            AIRS No:
            <asp:Label ID="lblAIRS" runat="server"></asp:Label>
        </b>
    </p>

    <ul class="menu-list-horizontal">
        <li><span class="selected-menu-item">Summary</span></li>
        <li><a href="Invoices.aspx">Invoices</a></li>
        <li><a href="Deposits.aspx">Deposits</a></li>
    </ul>

    <h2>Out of Balance Reports</h2>
    <h3>Annual/Emissions Fees</h3>
    <p><em>Note: Only includes data for annual fees starting from calendar year 2010.</em></p>

    <p id="pAnnualFees" runat="server" visible="false">None.</p>

    <asp:GridView ID="grdAnnualFees" runat="server" CssClass="table-simple" Visible="true" AutoGenerateColumns="False">
        <Columns>
            <asp:BoundField DataField="Fee Year" HeaderText="Fee Year" />
            <asp:BoundField DataField="Invoiced Amount" HeaderText="Invoiced Amount" DataFormatString="{0:c}" ItemStyle-CssClass="table-cell-alignright" />
            <asp:BoundField DataField="Payments Applied" HeaderText="Payments Applied" DataFormatString="{0:c}" ItemStyle-CssClass="table-cell-alignright" />
            <asp:BoundField DataField="Balance" HeaderText="Balance" DataFormatString="{0:c}" ItemStyle-CssClass="table-cell-alignright" />
        </Columns>
    </asp:GridView>

    <h3>Application Fees</h3>
    <p><em>Note: Only includes data for application fees starting from March 1, 2019.</em></p>

    <p id="pApplicationDeposits" runat="server" visible="false">None.</p>

    <asp:GridView ID="grdApplicationDeposits" runat="server" CssClass="table-simple" Visible="true" AutoGenerateColumns="False">
        <Columns>
            <asp:BoundField DataField="Deposit ID" HeaderText="Deposit ID" />
            <asp:BoundField DataField="Date" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}" ItemStyle-CssClass="table-cell-alignright" />
            <asp:BoundField DataField="Deposit Amount" HeaderText="Deposit Amount" DataFormatString="{0:c}" ItemStyle-CssClass="table-cell-alignright" />
            <asp:BoundField DataField="Applied to Invoices" HeaderText="Applied to Invoices" DataFormatString="{0:c}" ItemStyle-CssClass="table-cell-alignright" />
            <asp:BoundField DataField="Refunded" HeaderText="Refunded" DataFormatString="{0:c}" ItemStyle-CssClass="table-cell-alignright" />
            <asp:BoundField DataField="Balance" HeaderText="Balance" DataFormatString="{0:c}" ItemStyle-CssClass="table-cell-alignright" />
        </Columns>
    </asp:GridView>

</asp:Content>
