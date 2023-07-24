<%@ Page Title="Georgia EPD Upcoming Events" Language="VB" MasterPageFile="~/Main.master"
    AutoEventWireup="false" Inherits="GECO.EventRegistration_Default" CodeBehind="Default.aspx.vb" %>

<%@ MasterType VirtualPath="~/Main.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
    <h1 id="upcoming-classes">Upcoming classes and workshops offered by Georgia EPD</h1>

    <p id="pLoginWarning" runat="server" class="message-highlight">
        <strong>A GECO account is required to register for any event.</strong>
        <asp:HyperLink ID="lnkLogin" runat="server" NavigateUrl="~/Login.aspx" CssClass="no-visited">Sign in</asp:HyperLink>
        or
        <asp:HyperLink ID="lnkRegister" runat="server" NavigateUrl="~/Register.aspx" CssClass="no-visited">create an account</asp:HyperLink>.
    </p>

    <p id="pUpdateRequired" runat="server" visible="false" class="message-highlight">
        Your profile is missing required information. 
        <asp:HyperLink ID="lnkUpdateProfile" runat="server" NavigateUrl="~/Account/" CssClass="no-visited">Please update</asp:HyperLink>
        before registering.
    </p>

    <asp:GridView ID="gvwEventList" runat="server" AutoGenerateColumns="False" DataKeyNames="numres_eventid"
        ShowHeader="false" ShowFooter="false" EmptyDataText="There are no classes or workshops scheduled at this time."
        BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px"
        CellPadding="3" GridLines="Vertical" Width="100%">
        <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <table cellpadding="5" width="100%" aria-labelledby="upcoming-classes">
                        <tr>
                            <td align="left" valign="top">
                                <h2>
                                    <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl='<%# Eval("numres_eventid", "~/EventRegistration/Details.aspx?eventid={0}") %>' Text='<%# Eval("StrTitle") %>' />
                                </h2>
                                <%#IIf(Container.DataItem("strDescription") Is DBNull.Value, "", Container.DataItem("strDescription"))%>
                                <p>
                                    <b>Date & Time:</b>
                                    <br />
                                    <%#DataBinder.Eval(Container.DataItem, "datStartDate", "{0: MM/dd/yyyy}")%> at
                                    <%#Container.DataItem("strEventStartTime")%>
                                    <br />
                                    to <%#IIf(Container.DataItem("datEndDate") Is DBNull.Value, "", DataBinder.Eval(Container.DataItem, "datEndDate", "{0: MM/dd/yyyy}") + " at ")%>
                                    <%#Container.DataItem("strEventEndTime")%>
                                </p>
                                <p>
                                    <b>Location:</b><br />
                                    <%#Container.DataItem("strVenue")%>
                                    <br />
                                    <a title="Click to open Google Map" href="https://maps.google.com/?q=<%#Container.DataItem("strAddress")%> <%#Container.DataItem("strCity")%> <%#Container.DataItem("strState")%> <%#Container.DataItem("NumZipCode")%>"
                                         rel="noopener" target="_blank">
                                        <%#Container.DataItem("strAddress")%><br />
                                        <%#Container.DataItem("strCity")%>, <%#Container.DataItem("strState")%> <%#Container.DataItem("NumZipCode")%></a>
                                </p>
                            </td>
                            <td align="right" valign="top">
                                <h2>
                                    <%#DataBinder.Eval(Container.DataItem, "datStartDate", "{0:MM/dd/yyyy}")%></h2>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
        <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
        <AlternatingRowStyle BackColor="#DCDCDC" />
    </asp:GridView>
</asp:Content>
