<%@ Page Language="vb" MasterPageFile="~/Memo.master" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="GECO.InvoiceDefault" %>

<%@ MasterType VirtualPath="~/Memo.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:Panel ID="pnlVoidedInvoice" runat="server" Visible="false" CssClass="banner">
        <h1>Invoice Is Void</h1>
        <asp:Panel ID="pnlVoidedDate" runat="server" Visible="false">
            <p>
                <asp:Label ID="lblVoidedDate" runat="server"></asp:Label>
            </p>
        </asp:Panel>
    </asp:Panel>

    <div class="grid">
        <div class="grid__item">
            <h1>
                <asp:Label runat="server" ID="lblInvoiceId"></asp:Label>
            </h1>

            <table class="table-list">
                <tr>
                    <th scope="row">Invoice Date</th>
                    <td>
                        <asp:Label runat="server" ID="lblInvoiceDate"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th scope="row">Due Date</th>
                    <td>
                        <asp:Label runat="server" ID="lblDueDate"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th scope="row">For</th>
                    <td>
                        <asp:Label runat="server" ID="lblWhatFor"></asp:Label><br />
                        <asp:Label runat="server" ID="lblInvoiceType"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <div class="grid__item">
            <h2>Bill To:</h2>

            <table class="table-list">
                <tr>
                    <th scope="row">AIRS #</th>
                    <td>
                        <asp:Label runat="server" ID="lblAirsNo"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th scope="row">Facility</th>
                    <td>
                        <asp:Label runat="server" ID="lblCompany"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
    </div>

    <h2>Invoice Items</h2>
    <asp:GridView ID="grdInvoiceItems" runat="server" CssClass="table-simple table-full-width table-accounting"
        AutoGenerateColumns="False" ShowFooter="true" RowHeaderColumn="RateCategoryDisplay" UseAccessibleHeader="true">
        <Columns>
            <asp:BoundField DataField="RateCategoryDisplay" HeaderText="Item" />
            <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:c}" ItemStyle-CssClass="table-cell-alignright" />
        </Columns>
    </asp:GridView>

    <asp:Panel ID="pnlPayments" runat="server">
        <h2>Payments Applied</h2>
        <asp:GridView ID="grdPayments" runat="server" CssClass="table-simple table-full-width table-accounting"
            AutoGenerateColumns="False" ShowFooter="true" RowHeaderColumn="Description" UseAccessibleHeader="true">
            <Columns>
                <asp:BoundField DataField="Description" HeaderText="Payment" />
                <asp:BoundField DataField="AmountApplied" HeaderText="Amount Applied" DataFormatString="{0:c}" ItemStyle-CssClass="table-cell-alignright" />
            </Columns>
        </asp:GridView>

        <p class="balance">
            Balance Due:
            <asp:Label ID="lblBalance" runat="server"></asp:Label>
        </p>
    </asp:Panel>

    <div>
        <h2>Payment Terms</h2>
        <p>Check # ____________________</p>
        <p>Make checks payable to the order of:</p>
        <blockquote>Georgia Department of Natural Resources</blockquote>
        <p>Remit payment to:</p>
        <blockquote>
            Air Quality Fees<br />
            Post Office Box 101713<br />
            Atlanta, Georgia 30392
        </blockquote>
        <p>To pay by credit card, please contact Sakina Strozier at (404) 362-2749.</p>
    </div>

</asp:Content>
