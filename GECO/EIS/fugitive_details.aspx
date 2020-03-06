<%@ Page Title="Fugitive Details - GECO Facility Inventory" Language="VB" MasterPageFile="eismaster.master" AutoEventWireup="false" Inherits="GECO.eis_fugitive_details" Codebehind="fugitive_details.aspx.vb" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
    <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></act:ToolkitScriptManager>
    <div class="pageheader">
        Fugitive Release Point Details
        <asp:Button ID="btnReturntoSummary" runat="server" Text="Summary" CausesValidation="False"
            CssClass="summarybutton" PostBackUrl="~/eis/releasepoint_summary.aspx" />
    </div>
    <div class="fieldwrapperseparator">
        <asp:Label ID="lblFugitiveStacks" class="styledseparator" runat="server" Text="Fugitive Release Point"></asp:Label>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblReleasePointID" class="styled" runat="server" Text="Fugitive ID:"></asp:Label>
        <asp:TextBox ID="txtReleasePointID" class="readonly" runat="server" Text="" ReadOnly="True"
            Width="100px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblRPDescription" class="styled" runat="server" Text="Fugitive Description:"></asp:Label>
        <asp:TextBox ID="txtRPDescription" class="readonly" runat="server" Text="" ReadOnly="True"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblRPStatusCode" class="styled" runat="server" Text="Fugitive Operating Status:"></asp:Label>
        <asp:TextBox ID="txtRPStatusCode" class="readonly" runat="server" Text="" ReadOnly="True"
            Width="250px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblRPFenceLineDistanceMeasure" class="styled" runat="server" Text="Fence Line Distance (ft):"></asp:Label>
        <asp:TextBox ID="txtRPFenceLineDistanceMeasure" class="readonly" runat="server" Text=""
            ReadOnly="True" Width="100px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblRPFugitiveHeightMeasure" class="styled" runat="server" Text="Fugitive Height (ft):"></asp:Label>
        <asp:TextBox ID="txtRPFugitiveHeightMeasure" class="readonly" runat="server" Text=""
            ReadOnly="True" Width="100px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblRPFugitiveWidthMeasure" class="styled" runat="server" Text="Fugitive Width (ft):"></asp:Label>
        <asp:TextBox ID="txtRPFugitiveWidthMeasure" class="readonly" runat="server" Text=""
            ReadOnly="True" Width="100px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblRPFugitiveLengthMeasure" class="styled" runat="server" Text="Fugitive Length (ft):"></asp:Label>
        <asp:TextBox ID="txtRPFugitiveLengthMeasure" class="readonly" runat="server" Text=""
            ReadOnly="True" Width="100px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblRPFugitiveAngleMeasure" class="styled" runat="server" Text="Fugitive Angle (°):"></asp:Label>
        <asp:TextBox ID="txtRPFugitiveAngleMeasure" class="readonly" runat="server" Text=""
            ReadOnly="True" Width="100px" MaxLength="2"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblRPComment" class="styled" runat="server" Text="Comment:"></asp:Label>
        <asp:TextBox ID="txtRPComment" class="readonly" runat="server" ReadOnly="True" TextMode="MultiLine" Rows="4" 
            Text="" Width="400px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblLastEISSubmit" class="styled" runat="server" Text="Last Submitted to EPA:"></asp:Label>
        <asp:TextBox ID="txtLastEISSubmit" class="readonly" runat="server" Text="" ReadOnly="True"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblLastUpdated" class="styled" runat="server" Text="Last Updated On:"></asp:Label>
        <asp:TextBox ID="txtLastUpdate" class="readonly" runat="server" Text="" ReadOnly="True"></asp:TextBox>
    </div>
    <br />
    <div class="fieldwrapperseparator">
        <asp:Label ID="Label3" class="styledseparator" runat="server" Text="Fugitive Geographic Coordinate Information:"></asp:Label>
        <asp:Label ID="lblNoRPGeoCoordInfo" runat="server" Text="" ForeColor="Red" Font-Bold="True"></asp:Label>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="LblLatitudeMeasure" class="styled" runat="server" Text="Latitude:"></asp:Label>
        <asp:TextBox ID="TxtLatitudeMeasure" class="readonly" runat="server" Text="" ReadOnly="True"
            Width="150px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="LblLongitudeMeasure" class="styled" runat="server" Text="Longitude:"></asp:Label>
        <asp:TextBox ID="TxtLongitudeMeasure" class="readonly" runat="server" Text="" ReadOnly="True"
            Width="150px"></asp:TextBox>
    </div>
    <div style="text-align: center;">
        <asp:Panel ID="pnlLocationMap" runat="server" Width="610px" HorizontalAlign="Left">
            The release point&#39;s location is centered in the map below. If the location is
            incorrect go to the Edit page and make the correction.
            <asp:Image ID="imgGoogleStaticMap" runat="server" ImageUrl="" />
        </asp:Panel>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="LblHorCollectionMetCode" class="styled" runat="server" Text="Horizontal Collection Method:"></asp:Label>
        <asp:TextBox ID="TxtHorCollectionMetCode" class="readonly" runat="server" Text="" Rows="4" 
            ReadOnly="True" Font-Names="Verdana" Font-Size="Small" TextMode="MultiLine" Width="400px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="LblHorizontalAccuracyMeasure" class="styled" runat="server" Text="Accuracy Measure (m):"></asp:Label>
        <asp:TextBox ID="TxtHorizontalAccuracyMeasure" class="readonly" runat="server" Text=""
            ReadOnly="True"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="LblHorReferenceDatCode" class="styled" runat="server" Text="Horizontal Reference Datum:"></asp:Label>
        <asp:TextBox ID="TxtHorReferenceDatCode" class="readonly" runat="server" Text=""
            ReadOnly="True"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="LblGeographicComment" class="styled" runat="server" Text="Comment:"></asp:Label>
        <asp:TextBox ID="TxtGeographicComment" class="readonly" runat="server" ReadOnly="True" Rows="4" 
            TextMode="MultiLine" Text="" Width="400px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblLastEISSubmit_FGC" class="styled" runat="server" Text="Last Submitted to EPA:"></asp:Label>
        <asp:TextBox ID="txtLastEISSubmit_FGC" class="readonly" runat="server" Text="" ReadOnly="True"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblLastUpdate_FGC" class="styled" runat="server" Text="Last Updated On:"></asp:Label>
        <asp:TextBox ID="txtLastUpdate_FGC" class="readonly" runat="server" Text="" ReadOnly="True"></asp:TextBox>
    </div>
    <div class="fieldwrapperseparator">
        <asp:Label ID="lblSeparator" class="styledseparator" runat="server" Text="Release Point Apportionments"></asp:Label>
        <asp:Label ID="lblReleasePointAppMessage" runat="server" Font-Size="Small" ForeColor="#CA0000"
            CssClass="labelwarningleft"></asp:Label><br />
        <asp:Label ID="lblRPShutdownMessage" runat="server" Font-Size="Small" ForeColor="#CA0000"
            CssClass="labelwarningleft"></asp:Label>
    </div>
    <div class="gridview">
        <asp:GridView ID="gvwRPApportionment" runat="server" DataSourceID="SqlDataSourceRPApp"
            CellPadding="4" Font-Names="Arial" Font-Size="Small" ForeColor="#333333" GridLines="None"
            AutoGenerateColumns="False">
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#EFF3FB" />
            <Columns>
                <asp:BoundField DataField="EmissionsUnitID" HeaderText="Emission Unit ID" />
                <asp:HyperLinkField DataNavigateUrlFields="ProcessID,EmissionsUnitID" DataNavigateUrlFormatString="~/eis/rpapportionment_view.aspx?ep={0}&amp;eu={1}"
                    DataTextField="ProcessID" HeaderText="Process ID" NavigateUrl="~/eis/rpapportionment_view.aspx" />
                <asp:HyperLinkField DataNavigateUrlFields="ProcessID,EmissionsUnitID" DataNavigateUrlFormatString="~/eis/rpapportionment_view.aspx?ep={0}&amp;eu={1}"
                    DataTextField="strprocessdescription" HeaderText="Process Description" NavigateUrl="~/eis/rpapportionment_view.aspx">
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                </asp:HyperLinkField>
                <asp:BoundField DataField="ReleasePointID" HeaderText="Release Point ID" />
                <asp:BoundField DataField="intaveragepercentemissions" HeaderText="Apportionment %" />
            </Columns>
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#2461BF" />
            <AlternatingRowStyle BackColor="White" />
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSourceRPApp" runat="server"></asp:SqlDataSource>
    </div>
    <div class="fieldwrapperseparator">
        <asp:Label ID="lblSeparatorOnly" class="styledseparator" runat="server" Text="_"
            Font-Size="Small" BackColor="#ffffff" ForeColor="#ffffff"></asp:Label>
    </div>
    <div class="buttonwrapper">
        <asp:Button runat="server" ID="btnSummary2" CssClass="buttondiv" Text="Summary" CausesValidation="False"
            PostBackUrl="~/eis/releasepoint_summary.aspx" />
    </div>
</asp:Content>