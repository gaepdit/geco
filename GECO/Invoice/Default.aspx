<%@ Page Language="vb" MasterPageFile="~/Memo.master" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="GECO.InvoiceDefault" %>

<%@ MasterType VirtualPath="~/Memo.Master" %>
<%@ Import Namespace="GECO.GecoModels" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:Panel ID="pnlNotFound" runat="server" Visible="false">
        <h1>No items found.</h1>
    </asp:Panel>

    <asp:Repeater ID="invoicePages" runat="server">
        <ItemTemplate>
            <div class="wrapper">
                <header class="grid">
                    <div class="grid__item">
                        <asp:Image runat="server" CssClass="logo" ImageUrl="~/assets/images/epd-logo.png" />
                    </div>
                    <div class="grid__item grid__item_right">
                        <address class="inside-address">
                            <div class="director"><%= Master.EpdDirector %></div>
                            <div class="branch">
                                Air Protection Branch
                            </div>
                            <div>
                                4244 International Parkway, Suite 120<br />
                                Atlanta, Georgia 30354<br />
                                (404) 363-7000
                            </div>
                        </address>
                    </div>
                </header>

                <asp:Panel ID="pnlVoidedInvoice" runat="server" Visible="<%# DirectCast(Container.DataItem, Invoice).Voided %>" CssClass="banner">
                    <h1>Invoice Is Void</h1>
                    <asp:Panel ID="pnlVoidedDate" runat="server" Visible="<%# DirectCast(Container.DataItem, Invoice).VoidedDate.HasValue %>">
                        <p><%# DisplayNullableDate(Eval("VoidedDate"), "Voided on {0}") %></p>
                    </asp:Panel>
                </asp:Panel>

                <div class="grid">
                    <div class="grid__item">
                        <h1><%# DisplayObject(Eval("InvoiceID"), "Invoice #{0}") %></h1>

                        <table class="table-list">
                            <tr>
                                <th scope="row">Invoice Date</th>
                                <td><%# DisplayDate(Eval("InvoiceDate")) %></td>
                            </tr>
                            <tr>
                                <th scope="row">Due Date</th>
                                <td><%# DisplayDate(Eval("DueDate")) %></td>
                            </tr>
                            <tr>
                                <th scope="row">For</th>
                                <td>
                                    <%# DisplayWhatFor(Container.DataItem) %><br />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="grid__item">
                        <h2>Bill To:</h2>

                        <table class="table-list">
                            <tr>
                                <th scope="row">AIRS #</th>
                                <td><%# DirectCast(Container.DataItem, Invoice).FacilityID.FormattedString() %></td>
                            </tr>
                            <tr>
                                <th scope="row">Facility</th>
                                <td><%# DirectCast(Container.DataItem, Invoice).FacilityName %></td>
                            </tr>
                        </table>
                    </div>
                </div>

                <h2>Invoice Items</h2>
                <asp:GridView ID="grdInvoiceItems" runat="server" CssClass="table-simple table-full-width table-accounting"
                    AutoGenerateColumns="False" ShowFooter="true" RowHeaderColumn="InvoiceItemDescription" UseAccessibleHeader="true"
                    OnDataBound="gridView_DataBound" EmptyDataText="<%# DirectCast(Container.DataItem, Invoice).TotalAmountDue %>">
                    <Columns>
                        <asp:BoundField DataField="InvoiceItemDescription" HeaderText="Item" />
                        <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:c}" ItemStyle-CssClass="table-cell-alignright" />
                    </Columns>
                </asp:GridView>

                <asp:Panel ID="pnlPayments" runat="server"
                    Visible="<%# DirectCast(Container.DataItem, Invoice).DepositsApplied.Count > 0 %>">
                    <h2>Payments Applied</h2>
                    <asp:GridView ID="grdPayments" runat="server" CssClass="table-simple table-full-width table-accounting"
                        AutoGenerateColumns="False" ShowFooter="true" RowHeaderColumn="Description" UseAccessibleHeader="true"
                        OnDataBound="gridView_DataBound" EmptyDataText="<%# DirectCast(Container.DataItem, Invoice).DepositsApplied.Sum(Function(i) i.AmountApplied) %>">
                        <Columns>
                            <asp:BoundField DataField="Description" HeaderText="Payment" />
                            <asp:BoundField DataField="AmountApplied" HeaderText="Amount Applied" DataFormatString="{0:c}" ItemStyle-CssClass="table-cell-alignright" />
                        </Columns>
                    </asp:GridView>

                    <p class="balance" runat="server"
                        visible="<%# DirectCast(Container.DataItem, Invoice).InvoiceCategory = InvoiceCategory.PermitApplicationFees %>">
                        Balance Due: <%# DirectCast(Container.DataItem, Invoice).CurrentBalance.ToString("c") %>
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
                    <p>To pay by credit card, please contact Susie Kocsis at (470) 628-8378.</p>
                </div>
            </div>
        </ItemTemplate>

    </asp:Repeater>
</asp:Content>
