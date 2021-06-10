<%@ Page MasterPageFile="~/EIS/EIS.master" Language="VB" AutoEventWireup="false"
    Title="GECO EIS: Facility Details"
    Inherits="GECO.EIS_Facility_Default" CodeBehind="Default.aspx.vb" %>

<%@ MasterType VirtualPath="~/EIS/EIS.master" %>
<asp:Content ID="Content3" ContentPlaceHolderID="Content" runat="Server">
    <p id="updateMessage" runat="server" visible="false" class="message-highlight">Facility details were successfully saved.</p>

    <h2>Facility Information</h2>

    <p>
        <asp:HyperLink ID="btnEdit" runat="server" NavigateUrl="~/EIS/Facility/Edit.aspx" CssClass="button">
            Edit Facility Details
        </asp:HyperLink>
    </p>

    <h3>Facility Description</h3>

    <table class="table-simple table-list">
        <tbody>
            <tr>
                <th>Name</th>
                <td>
                    <asp:Label ID="lblFacilityName" runat="server" />
                </td>
            </tr>
            <tr>
                <th>Description</th>
                <td>
                    <asp:Label ID="lblDescription" runat="server" />
                </td>
            </tr>
            <tr>
                <th>Operating Status</th>
                <td>
                    <asp:Label ID="lblOperatingStatus" runat="server" /><br />
                    <small>* Operating status pertains only to Emissions Inventory</small>
                </td>
            </tr>
            <tr>
                <th>NAICS Code</th>
                <td>
                    <asp:Label ID="lblNAICS" runat="server" />
                </td>
            </tr>
            <tr>
                <th>Description Comment</th>
                <td>
                    <asp:Label ID="lblDescriptionComment" runat="server" />
                </td>
            </tr>
            <tr>
                <th>Last Updated</th>
                <td>
                    <asp:Label ID="lblDescriptionUpdated" runat="server" />
                </td>
            </tr>
        </tbody>
    </table>

    <h3>Location</h3>

    <table class="table-simple table-list">
        <tbody>
            <tr>
                <th>Site Address</th>
                <td>
                    <asp:Label ID="lblSiteAddress" runat="server" />
                </td>
            </tr>
            <tr>
                <th>Latitude</th>
                <td>
                    <asp:Label ID="lblLatitude" runat="server" />
                </td>
            </tr>
            <tr>
                <th>Longitude</th>
                <td>
                    <asp:Label ID="lblLongitude" runat="server" />
                </td>
            </tr>
            <tr>
                <th>Horizontal Collection Method</th>
                <td>
                    <asp:Label ID="lblHorizontalCollectionMethod" runat="server" />
                </td>
            </tr>
            <tr>
                <th>Accuracy Measure (m)</th>
                <td>
                    <asp:Label ID="lblAccuracyMeasure" runat="server" />
                </td>
            </tr>
            <tr>
                <th>Horizontal Reference Datum</th>
                <td>
                    <asp:Label ID="lblHorizontalReferenceDatum" runat="server" />
                </td>
            </tr>
            <tr>
                <th>Location Comment</th>
                <td>
                    <asp:Label ID="lblLocationComment" runat="server" />
                </td>
            </tr>
            <tr>
                <th>Last Updated</th>
                <td>
                    <asp:Label ID="lblLocationUpdated" runat="server" />
                </td>
            </tr>
        </tbody>
    </table>

    <div>
        <asp:HyperLink ID="lnkGoogleMap" runat="server" Target="_blank" CssClass="no-visited">
            <asp:Image ID="imgGoogleStaticMap" runat="server" BorderStyle="Solid" BorderWidth="2px" /><br />
            Open map in new window
        </asp:HyperLink>
    </div>
</asp:Content>
