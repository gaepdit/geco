<%@ Page Language="VB" MasterPageFile="~/Main.master" AutoEventWireup="false" Inherits="GECO.FacilitySummary" Title="GECO Facility Summary" CodeBehind="Summary.aspx.vb" %>

<%@ MasterType VirtualPath="~/Main.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
    <h1>Facility Summary</h1>

    <p>
        <b>
            <asp:Label ID="lblFacilityDisplay" runat="server"></asp:Label></b>
        <br />
        AIRS Number:
        <asp:Label ID="lblAIRS" runat="server"></asp:Label>
    </p>

    <ul class="menu-list-horizontal">
        <li>
            <asp:HyperLink ID="lnkFacilityHome" runat="server" NavigateUrl="~/Facility/">Menu</asp:HyperLink>
        </li>
        <li>
            <asp:HyperLink ID="lnkFacilityInfo" runat="server" NavigateUrl="~/Facility/Summary.aspx" Enabled="false" CssClass="selected-menu-item disabled">Facility Info</asp:HyperLink>
        </li>
        <li>
            <asp:HyperLink ID="lnkFacilityAdmin" runat="server" NavigateUrl="~/Facility/Admin.aspx">User Access</asp:HyperLink>
        </li>
    </ul>

    <h2>Facility Location</h2>

    <table class="table-simple table-list">
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
                                                <asp:HyperLink ID="hlDistrict" runat="server" Target="_blank" Text="District Responsible Source"
                                                    NavigateUrl="https://epd.georgia.gov/district-office-locations" />
                </td>
            </tr>
            <tr>
                <th scope="row">Office:</th>
                <td>
                    <asp:Label ID="lblOffice" runat="server"></asp:Label>
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

    <p>
        <a href="../FacilityMap.aspx?name=<% =lblFacilityDisplay.Text %>&address=<% =lblAddress.Text %>&city=<% =lblCityStateZip.Text %>&lat=<% =lblLatitude.Text %>&lon=<% =lblLongitude.Text %>" target="_blank">Open map in new window</a>
    </p>

    <h2>Facility Status</h2>

    <table class="table-simple table-list">
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

    <h2>State Contacts</h2>

    <table class="table-simple">
        <thead>
            <tr>
                <th scope="col">Program</th>
                <th scope="col">Contact Name</th>
                <th scope="col">Contact Phone</th>
                <th scope="col">Contact Email</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <th scope="row">Permitting</th>
                <td>
                    <asp:Label ID="lblPermitContactName" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblPermitContactPhone" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:HyperLink ID="hlPermitContactEmail" runat="server"></asp:HyperLink>
                </td>
            </tr>
            <tr>
                <th scope="row">Compliance</th>
                <td>
                    <asp:Label ID="lblComplianceContactName" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblComplianceContactPhone" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:HyperLink ID="hlComplianceContactEmail" runat="server"></asp:HyperLink>
                </td>
            </tr>
            <tr>
                <th scope="row">Monitoring</th>
                <td>
                    <asp:Label ID="lblMonitoringContactName" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblMonitoringContactPhone" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:HyperLink ID="hlMonitoringContactEmail" runat="server"></asp:HyperLink>
                </td>
            </tr>
        </tbody>
    </table>
</asp:Content>
