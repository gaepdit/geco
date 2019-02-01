<%@ Page Language="VB" MasterPageFile="~/MainMaster.master" AutoEventWireup="false" Inherits="GECO.Fees_Deposits" Title="GECO Invoices" CodeBehind="Deposits.aspx.vb" %>

<%@ MasterType VirtualPath="~/MainMaster.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="FullContent" runat="server">
    <h1>Permit Fee Deposits & Refunds</h1>

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
        <li><a href="Invoices.aspx">Invoices</a></li>
        <li><span class="selected-menu-item">Deposits</span></li>
    </ul>

    <h2>Annual/Emissions Fees</h2>
        
    <p id="pAnnualTransactions" runat="server" visible="false">None.</p>

    <asp:GridView ID="grdAnnualTransactions" runat="server" CssClass="table-simple" Visible="true" AutoGenerateColumns="False">
        <Columns>
            <asp:BoundField DataField="Transaction ID" HeaderText="Transaction ID" />
            <asp:BoundField DataField="Payment Date" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}" ItemStyle-CssClass="table-cell-alignright" />
            <asp:BoundField DataField="Payment Type" HeaderText="Deposit/Refund" />
            <asp:BoundField DataField="Amount Paid" HeaderText="Amount" DataFormatString="{0:c}" ItemStyle-CssClass="table-cell-alignright" />
            <asp:BoundField DataField="Fee Year" HeaderText="Fee Year" />
            <asp:BoundField DataField="Check #" HeaderText="Check #" />
            <asp:BoundField DataField="Deposit #" HeaderText="Deposit #" />
            <asp:BoundField DataField="Batch #" HeaderText="Batch #" />
            <asp:BoundField DataField="Credit Card Conf" HeaderText="Credit Card Conf" />
        </Columns>
    </asp:GridView>

    <h2>Deposits for Permit Application Fees</h2>

    <p id="pApplicationDeposits" runat="server" visible="false">None.</p>

    <asp:GridView ID="grdApplicationDeposits" runat="server" CssClass="table-simple" Visible="true" AutoGenerateColumns="False">
        <Columns>
            <asp:BoundField DataField="Deposit ID" HeaderText="Deposit ID" />
            <asp:BoundField DataField="Date" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}" ItemStyle-CssClass="table-cell-alignright" />
            <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:c}" ItemStyle-CssClass="table-cell-alignright" />
            <asp:BoundField DataField="Check #" HeaderText="Check #" />
            <asp:BoundField DataField="Deposit #" HeaderText="Deposit #" />
            <asp:BoundField DataField="Batch #" HeaderText="Batch #" />
            <asp:BoundField DataField="Credit Card Conf" HeaderText="Credit Card Conf" />
        </Columns>
    </asp:GridView>

    <h2>Refunds for Permit Application Fees</h2>

    <p id="pApplicationRefunds" runat="server" visible="false">None.</p>

    <asp:GridView ID="grdApplicationRefunds" runat="server" CssClass="table-simple" Visible="true" AutoGenerateColumns="False">
        <Columns>
            <asp:BoundField DataField="Refund ID" HeaderText="Refund ID" />
            <asp:BoundField DataField="Date" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}" ItemStyle-CssClass="table-cell-alignright" />
            <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:c}" ItemStyle-CssClass="table-cell-alignright" />
        </Columns>
    </asp:GridView>

</asp:Content>
