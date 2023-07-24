<%@ Page Language="VB" MasterPageFile="~/Main.master" AutoEventWireup="false" Inherits="GECO.FacilitySummary" Title="GECO Facility Summary" CodeBehind="Summary.aspx.vb" %>

<%@ MasterType VirtualPath="~/Main.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">

    <ul class="menu-list-horizontal">
        <li>
            <asp:HyperLink ID="lnkFacilityHome" runat="server" NavigateUrl="~/Facility/">Home</asp:HyperLink>
        </li>
        <li>
            <asp:HyperLink ID="lnkFacilityInfo" runat="server" NavigateUrl="~/Facility/Summary.aspx"
                Enabled="false" CssClass="selected-menu-item disabled">Facility Info</asp:HyperLink>
        </li>
        <li>
            <asp:HyperLink ID="lnkFacilityAdmin" runat="server" NavigateUrl="~/Facility/Admin.aspx">User Access</asp:HyperLink>
        </li>
        <li>
            <asp:HyperLink ID="lnkFacilityContacts" runat="server" NavigateUrl="~/Facility/Contacts.aspx">Communication Preferences</asp:HyperLink>
        </li>
    </ul>

    <h1>Facility Info</h1>

    <h2>Location</h2>

    <table class="table-simple table-list" aria-label="Facility location details">
        <tbody>
            <tr>
                <th scope="row">Address:</th>
                <td>
                    <asp:Label ID="lblAddress" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <th></th>
                <td>
                    <asp:Label ID="lblCityStateZip" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <th scope="row">County:</th>
                <td>
                    <asp:Label ID="lblCounty" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <th scope="row">District:</th>
                <td>
                    <asp:Label ID="lblDistrict" runat="server"></asp:Label>
                    &nbsp; &nbsp;
                                                <asp:HyperLink ID="hlDistrict" runat="server" rel="noopener" Target="_blank" Text="District Responsible Source"
                                                    NavigateUrl="https://epd.georgia.gov/district-office-locations" />
                </td>
            </tr>
            <tr>
                <th scope="row">Longitude:</th>
                <td>
                    <asp:Label ID="lblLongitude" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <th scope="row">Latitude:</th>
                <td>
                    <asp:Label ID="lblLatitude" runat="server"></asp:Label>
                </td>
            </tr>
        </tbody>
    </table>

    <div>
        <asp:HyperLink ID="lnkGoogleMap" runat="server" rel="noopener" Target="_blank" CssClass="no-visited">
            <asp:Image ID="imgGoogleStaticMap" runat="server" BorderStyle="Solid" BorderWidth="2px" /><br />
            Open map in new window
        </asp:HyperLink>
    </div>

    <h2>Facility Status</h2>

    <table class="table-simple table-list" aria-label="Facility status details">
        <tbody>
            <tr>
                <th scope="row">Classification:</th>
                <td>
                    <asp:Label ID="lblClassification" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <th scope="row">Operating Status:</th>
                <td>
                    <asp:Label ID="lblOpStatus" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <th scope="row">SIC Code:</th>
                <td>
                    <asp:Label ID="lblSICCode" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <th scope="row">Startup Date:</th>
                <td>
                    <asp:Label ID="lblStartUp" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <th scope="row">Date Closed:</th>
                <td>
                    <asp:Label ID="lblClosed" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <th scope="row">CMS Status:</th>
                <td>
                    <asp:Label ID="lblCMSStatus" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <th scope="row">Air Program Codes:</th>
                <td>
                    <asp:Label ID="lblAirProgramCodes" runat="server"></asp:Label>
                </td>
            </tr>
        </tbody>
    </table>

    <h2>Most Recent State Contacts</h2>
    <p>This table lists Air Protection Branch staff who have most recently worked on this facility.</p>
    <asp:GridView ID="gvStateContacts" runat="server" AutoGenerateColumns="false" CssClass="table-simple">
        <Columns>
            <asp:BoundField DataField="Program" HeaderText="Program" />
            <asp:TemplateField HeaderText="Assigned staff">
                <ItemTemplate>
                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("Assigned staff") %>' /><br />
                    <asp:Label ID="Lable3" runat="server" Text='<%# Eval("Staff phone number") %>' /><br />
                    <asp:Label ID="Label2" runat="server" Text='<%# Eval("Staff email") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Assignment date" HeaderText="Assignment date" DataFormatString="{0:d}" />
            <asp:BoundField DataField="Work item" HeaderText="Work item" />
        </Columns>
    </asp:GridView>
</asp:Content>
