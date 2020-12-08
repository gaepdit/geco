<%@ Page Language="VB" MasterPageFile="~/MainMaster.master" AutoEventWireup="false"
    Inherits="GECO.TN_Default" Title="GECO - Test Notifications" CodeBehind="Default.aspx.vb" %>

<%@ MasterType VirtualPath="~/MainMaster.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
    <acs:ModalUpdateProgress ID="ModalUpdateProgress1" runat="server" DisplayAfter="0"
        BackgroundCssClass="modalProgressGreyBackground">
        <ProgressTemplate>
            <div class="modalPopup">
                Please Wait...
                <img src="<%= Page.ResolveUrl("~/assets/images/progressbar_green.gif") %>" alt="" align="middle" />
            </div>
        </ProgressTemplate>
    </acs:ModalUpdateProgress>
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

            <asp:DataGrid ID="dgrTestNotify" runat="server" Width="100%" CellSpacing="1" CellPadding="3"
                AutoGenerateColumns="False" Font-Size="Small" AllowPaging="True"
                OnPageIndexChanged="dgrTestNotify_PageIndexChanged" OnItemCommand="RequestDetails"
                BackColor="White" Font-Names="Arial" BorderColor="#999999" BorderWidth="1px" AlternatingItemStyle-BackColor="White" CssClass="button-small">
                <FooterStyle Wrap="False" ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
                <SelectedItemStyle Font-Bold="True" Wrap="False" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
                <AlternatingItemStyle Wrap="False" BackColor="White"></AlternatingItemStyle>
                <ItemStyle Wrap="False" ForeColor="Black" BackColor="#EEEEEE"></ItemStyle>
                <HeaderStyle Font-Bold="True" Wrap="False" BackColor="#01009A" ForeColor="White"></HeaderStyle>
                <Columns>
                    <asp:ButtonColumn Text="strTestLogNumber" DataTextField="strTestLogNumber" HeaderText="Test Log No."
                        ButtonType="PushButton">
                        <ItemStyle Wrap="True" HorizontalAlign="Center" VerticalAlign="Top"></ItemStyle>
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:ButtonColumn>
                    <asp:BoundColumn DataField="strEmissionUnit" HeaderText="Emission Unit">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="strPollutants" HeaderText="Pollutants">
                        <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="datTestNotification" HeaderText="Notify Date" DataFormatString="{0:MM-dd-yyyy}">
                        <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="datProposedStartDate" ReadOnly="True" HeaderText="Start Date"
                        DataFormatString="{0:MM-dd-yyyy}">
                        <ItemStyle Wrap="False" HorizontalAlign="Center" VerticalAlign="Top"></ItemStyle>
                        <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="datProposedEndDate" ReadOnly="True" HeaderText="End Date"
                        DataFormatString="{0:MM-dd-yyyy}">
                        <ItemStyle Wrap="False" HorizontalAlign="Center" VerticalAlign="Top"></ItemStyle>
                        <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:BoundColumn>
                </Columns>
                <PagerStyle NextPageText="Next" PrevPageText="Prev" HorizontalAlign="Center" ForeColor="Black"
                    Position="TopAndBottom" BackColor="Gainsboro" Wrap="False" Mode="NumericPages"></PagerStyle>
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
                                <asp:Label ID="lblTestLogNumber" runat="server" ForeColor="Blue"></asp:Label>&nbsp;(refer to this number when calling EPD)<br />
                                Emission Unit(s):
                                <asp:Label ID="lblEmissionUnit" runat="server" ForeColor="Blue"></asp:Label><br />
                                <strong>Pollutant(s):</strong>
                                <asp:Label ID="lblPollutants" runat="server" ForeColor="Blue"></asp:Label><br />
                                Notification Date:
                                <asp:Label ID="lblNotificationDate" runat="server" ForeColor="Blue"></asp:Label><br />
                                <strong>Test Start Date:</strong>
                                <asp:Label ID="lblStartDate" runat="server" ForeColor="Blue"></asp:Label><br />
                                Test End Date:
                                <asp:Label ID="lblEndDate" runat="server" ForeColor="Blue"></asp:Label><br />
                                Comments:
                                <asp:Label ID="lblComment" runat="server" ForeColor="Blue"></asp:Label><br />
                                Confirmation No:
                                <asp:Label ID="lblConfNo" runat="server" ForeColor="Blue"></asp:Label>
                            </p>
                            <p>
                                <strong>Facility's Contact for Test</strong><br />
                                Name:
                                <asp:Label ID="lblContactName" runat="server" ForeColor="Blue"></asp:Label><br />
                                Telephone:
                                <asp:Label ID="lblTelephone" runat="server" ForeColor="Blue"></asp:Label>&nbsp;
                                Ext:
                                <asp:Label ID="lblExt" runat="server" ForeColor="Blue"></asp:Label><br />
                                Fax:
                                <asp:Label ID="lblFax" runat="server" ForeColor="Blue"></asp:Label><br />
                                Email:
                                <asp:Label ID="lblEmail" runat="server" ForeColor="Blue"></asp:Label>
                            </p>
                            <p>
                                <strong>EPD Contact for Test</strong><br />
                                Name:
                                <asp:Label ID="lblEPDContact" runat="server" ForeColor="Blue"></asp:Label><br />
                                Telephone:
                                <asp:Label ID="lblEPDTelephone" runat="server" ForeColor="Blue"></asp:Label><br />
                                Fax:
                                <asp:Label ID="lblEPDFax" runat="server" ForeColor="Blue"></asp:Label><br />
                                Email:
                                <asp:Label ID="lblEPDEmail" runat="server" ForeColor="Blue"></asp:Label>
                            </p>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
