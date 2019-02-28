<%@ Page Language="vb" MasterPageFile="~/MainMaster.master" AutoEventWireup="false" CodeBehind="Application.aspx.vb" Inherits="GECO.Permit_Application" %>

<%@ MasterType VirtualPath="~/MainMaster.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="FullContent" runat="Server">
    <h1>Permit Application #<asp:Label ID="lblAppNum" runat="server" Text=""></asp:Label></h1>

    <h2>Application Details</h2>
    <asp:Table ID="tApplicationDetails" runat="server" CssClass="table-simple table-list"></asp:Table>

    <h2>Application Fees</h2>
    <p id="pFeesNotApplicable" runat="server" visible="False">No permit application fees applicable.</p>
    <p id="pFeesNotDetermined" runat="server" visible="False">Permit application fees have not yet been reviewed by APB staff.</p>

    <p id="pFeesNotified" runat="server" visible="false">
        Facility notified of pending fees on
        <asp:Label ID="lblNotifiedDate" runat="server"></asp:Label>.
    </p>
    <asp:Table ID="tblFeesSummary" runat="server" CssClass="table-simple table-bordered table-accounting" Visible="false" UseAccessibleHeader="true">
        <asp:TableHeaderRow TableSection="TableHeader">
            <asp:TableHeaderCell Scope="Column">Fee Type</asp:TableHeaderCell>
            <asp:TableHeaderCell Scope="Column">Fee Amount</asp:TableHeaderCell>
        </asp:TableHeaderRow>
    </asp:Table>

    <asp:UpdatePanel ID="pnlGenerateInvoice" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hAppNumber" runat="server" />

            <p id="pGenerateInvoice" runat="server" visible="false" class="message-highlight">
                <asp:Button ID="btnGenerateInvoice" runat="server" Text="Generate New Invoice" />
                <asp:Label ID="lblCredits" runat="server" Visible="false" Font-Italic="True"></asp:Label>
            </p>

            <p id="pGenerateNoAccess" runat="server" visible="false" class="message-highlight">
                You must be logged into GECO and have Fee Access to this facility in order to generate an invoice.
            </p>

            <p id="pGenerateDbError" runat="server" class="message-highlight" visible="false">An error occurred while trying to generate the invoice. Please contact the Air Protection Branch for assistance.</p>
            <p id="pGenerateExists" runat="server" class="message-highlight" visible="false">Error: An active invoice already exists for this permit application.</p>
            <p id="pGenerateSuccess" runat="server" class="message-highlight" visible="false">
                Invoice
                <asp:Label ID="lblNewInvoiceNumber" runat="server"></asp:Label>
                has been generated. Please print and return it with your payment.
            </p>

            <asp:Panel ID="pnlInvoices" runat="server" Visible="false">
                <h3>Invoices</h3>
                <asp:GridView ID="grdInvoices" runat="server" CssClass="table-simple table-menu"
                    AutoGenerateColumns="False" RowHeaderColumn="Invoice #" UseAccessibleHeader="true">
                    <Columns>
                        <asp:HyperLinkField DataTextField="Invoice #" HeaderText="Invoice #" ItemStyle-CssClass="table-cell-alignright"
                            DataNavigateUrlFields="InvoiceGuid" DataNavigateUrlFormatString="~/Invoice/?id={0}" />
                        <asp:BoundField DataField="Invoice Date" HeaderText="Invoice Date" DataFormatString="{0:dd-MMM-yyyy}" ItemStyle-CssClass="table-cell-alignright" />
                        <asp:BoundField DataField="Due Date" HeaderText="Due Date" DataFormatString="{0:dd-MMM-yyyy}" ItemStyle-CssClass="table-cell-alignright" />
                        <asp:BoundField DataField="Amount" HeaderText="Total Amount" DataFormatString="{0:c}" ItemStyle-CssClass="table-cell-alignright" />
                        <asp:BoundField DataField="Balance" HeaderText="Balance" DataFormatString="{0:c}" ItemStyle-CssClass="table-cell-alignright" />
                        <asp:BoundField DataField="Status" HeaderText="Status" />
                    </Columns>
                </asp:GridView>
            </asp:Panel>

            <asp:Panel ID="pnlPayments" runat="server" Visible="false">
                <h3>Payments</h3>
                <asp:GridView ID="grdPayments" runat="server" CssClass="table-simple table-menu"
                    AutoGenerateColumns="False" RowHeaderColumn="Date" UseAccessibleHeader="true">
                    <Columns>
                        <asp:BoundField DataField="Date" HeaderText="Deposit Date" DataFormatString="{0:dd-MMM-yyyy}" ItemStyle-CssClass="table-cell-alignright" />
                        <asp:BoundField DataField="Amount applied" HeaderText="Amount Applied" DataFormatString="{0:c}" ItemStyle-CssClass="table-cell-alignright" />
                        <asp:BoundField DataField="Invoice #" HeaderText="Invoice #" />
                    </Columns>
                </asp:GridView>
            </asp:Panel>

        </ContentTemplate>
    </asp:UpdatePanel>

    <h2>Facility Information</h2>
    <p><em>Facility information as submitted with the application.</em></p>
    <asp:Table ID="tFacilityInfo" runat="server" CssClass="table-simple table-list"></asp:Table>

    <h2>Air Protection Branch Contact</h2>
    <p id="pNoContact" runat="server" visible="false">No Air Protection Branch staff assigned to application.</p>
    <asp:Table ID="tContact" runat="server" CssClass="table-simple table-list"></asp:Table>

    <h2>History</h2>
    <p><em>Approximate application processing timeline.</em></p>
    <asp:Table ID="tTracking" runat="server" CssClass="table-simple table-list"></asp:Table>

</asp:Content>
