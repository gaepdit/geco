<%@ Page Language="VB" MasterPageFile="~/Main.master" AutoEventWireup="false"
    Inherits="GECO.TN_Default" Title="GECO - Test Notifications" CodeBehind="Default.aspx.vb" %>

<%@ MasterType VirtualPath="~/Main.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
    <asp:UpdateProgress ID="ModalUpdateProgress1" runat="server" DisplayAfter="200" class="progressIndicator">
        <ProgressTemplate>
            <div class="progressIndicator-inner">
                Please Wait...<br />
                <img src="<%= Page.ResolveUrl("~/assets/images/progressbar_green.gif") %>" alt="" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <h1>Performance Test Notifications</h1>

            <p>
                The list below shows all test notifications for your facility. Click the Test Log Number for more details.
            </p>
            <p>
                When calling EPD about test notifications please have the <strong>Test Log Number</strong>
                available. This will enable us to help you more efficiently.
            </p>

            <asp:DataGrid ID="dgrTestNotify" runat="server"
                AutoGenerateColumns="False" AllowPaging="True"
                OnPageIndexChanged="dgrTestNotify_PageIndexChanged" OnItemCommand="RequestDetails"
                CssClass="button-small table-simple table-full-width">
                <HeaderStyle CssClass="table-head" />
                <PagerStyle HorizontalAlign="Center" Mode="NumericPages" CssClass="table-head datagrid-pager" />
                <Columns>
                    <asp:ButtonColumn DataTextField="strTestLogNumber" HeaderText="Test Log No."
                        ButtonType="PushButton">
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:ButtonColumn>
                    <asp:BoundColumn DataField="strEmissionUnit" HeaderText="Emission Unit" />
                    <asp:BoundColumn DataField="strPollutants" HeaderText="Pollutants" />
                    <asp:BoundColumn DataField="datTestNotification" HeaderText="Notify Date"
                        DataFormatString="{0:MM-dd-yyyy}" />
                    <asp:BoundColumn DataField="datProposedStartDate" ReadOnly="True" HeaderText="Start Date"
                        DataFormatString="{0:MM-dd-yyyy}" />
                    <asp:BoundColumn DataField="datProposedEndDate" ReadOnly="True" HeaderText="End Date"
                        DataFormatString="{0:MM-dd-yyyy}" />
                </Columns>
            </asp:DataGrid>

            <asp:Panel ID="pnlDetails" runat="server" Width="100%" Visible="False">
                <h2>Details for Selected Test Notification:</h2>
                <table style="width: 100%">
                    <tr>
                        <td>
                            <asp:Label ID="lblTestPlan" runat="server" Font-Bold="True" Font-Names="Verdana"
                                Font-Size="Medium" ForeColor="Red" Text="Remember to submit Test Plan if not yet done."></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <p>
                                Test Log No:
                                <asp:Label ID="lblTestLogNumber" runat="server"></asp:Label>&nbsp;(refer to this number when calling EPD)<br />
                                Emission Unit(s):
                                <asp:Label ID="lblEmissionUnit" runat="server"></asp:Label><br />
                                <strong>Pollutant(s):</strong>
                                <asp:Label ID="lblPollutants" runat="server"></asp:Label><br />
                                Notification Date:
                                <asp:Label ID="lblNotificationDate" runat="server"></asp:Label><br />
                                <strong>Test Start Date:</strong>
                                <asp:Label ID="lblStartDate" runat="server"></asp:Label><br />
                                Test End Date:
                                <asp:Label ID="lblEndDate" runat="server"></asp:Label><br />
                                Comments:
                                <asp:Label ID="lblComment" runat="server"></asp:Label><br />
                                Confirmation No:
                                <asp:Label ID="lblConfNo" runat="server"></asp:Label>
                            </p>
                            <p>
                                <strong>Facility's Contact for Test</strong><br />
                                Name:
                                <asp:Label ID="lblContactName" runat="server"></asp:Label><br />
                                Telephone:
                                <asp:Label ID="lblTelephone" runat="server"></asp:Label>&nbsp;
                                Ext:
                                <asp:Label ID="lblExt" runat="server"></asp:Label><br />
                                Fax:
                                <asp:Label ID="lblFax" runat="server"></asp:Label><br />
                                Email:
                                <asp:Label ID="lblEmail" runat="server"></asp:Label>
                            </p>
                            <p>
                                <strong>EPD Contact for Test</strong><br />
                                Name:
                                <asp:Label ID="lblEPDContact" runat="server"></asp:Label><br />
                                Telephone:
                                <asp:Label ID="lblEPDTelephone" runat="server"></asp:Label><br />
                                Fax:
                                <asp:Label ID="lblEPDFax" runat="server"></asp:Label><br />
                                Email:
                                <asp:Label ID="lblEPDEmail" runat="server"></asp:Label>
                            </p>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
