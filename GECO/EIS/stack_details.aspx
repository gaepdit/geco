<%@ Page Title="Stack Release Point Details - GECO Facility Inventory" Language="VB"
    MasterPageFile="eismaster.master" AutoEventWireup="false"
    Inherits="GECO.eis_stack_details" Codebehind="stack_details.aspx.vb" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
    <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></act:ToolkitScriptManager>
    <div class="pageheader">
        Stack Release Point Details
        <asp:Button ID="btnReturnToSummary" runat="server" Text="Summary" CausesValidation="False"
            CssClass="summarybutton" PostBackUrl="~/eis/releasepoint_summary.aspx" />
    </div>
    <div class="fieldwrapperseparator">
        <asp:Label ID="lblStackDetails" class="styledseparator" runat="server" Text="Stack Information"></asp:Label>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblReleasePointID" class="styled" runat="server" Text="Stack ID:"></asp:Label>
        <asp:TextBox ID="txtReleasePointID" runat="server" Text="" class="readonly"
            ReadOnly="True" Width="100px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblRPDescription" class="styled" runat="server" Text="Stack Description:"></asp:Label>
        <asp:TextBox ID="txtRPDescription" runat="server" Text="" class="readonly" ReadOnly="True"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblRPTypeCode" class="styled" runat="server" Text="Stack Type:"></asp:Label>
        <asp:TextBox ID="txtRPTypeCode" runat="server" Text="" class="readonly"
            ReadOnly="True" Width="200px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblRPStatusCode" class="styled" runat="server" Text="Stack Operating Status:"></asp:Label>
        <asp:TextBox ID="txtRPStatusCode" runat="server" Text="" class="readonly"
            ReadOnly="True" Width="250px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblRPStackHeightMeasure" class="styled" runat="server" Text="Stack Height (ft):"></asp:Label>
        <asp:TextBox ID="txtRPStackHeightMeasure" runat="server" Text="" class="readonly"
            ReadOnly="True" Width="100px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblRPStackDiameterMeasure" class="styled" runat="server" Text="Stack Diameter (ft):"></asp:Label>
        <asp:TextBox ID="txtRPStackDiameterMeasure" runat="server" Text="" class="readonly"
            ReadOnly="True" Width="100px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblRPExitGasVelocityMeasure" class="styled" runat="server" Text="Exit Gas Velocity (fps):"></asp:Label>
        <asp:TextBox ID="txtRPExitGasVelocityMeasure" runat="server" Text="" class="readonly"
            ReadOnly="True" Width="100px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblRPExitGasFlowRateMeasure" class="styled" runat="server" Text="Exit Gas Flow Rate (acfs):"></asp:Label>
        <asp:TextBox ID="txtRPExitGasFlowRateMeasure" runat="server" Text="" class="readonly"
            ReadOnly="True" Width="150px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblRPExitGasTemperatureMeasure" class="styled" runat="server" Text="Exit Gas Temperature (°F):"></asp:Label>
        <asp:TextBox ID="txtRPExitGasTemperatureMeasure" runat="server" Text="" class="readonly"
            ReadOnly="True" Width="100px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblRPFenceLineDistanceMeasure" class="styled" runat="server" Text="Fence Line Distance (ft):"></asp:Label>
        <asp:TextBox ID="txtRPFenceLineDistanceMeasure" runat="server" Text="" class="readonly"
            ReadOnly="True" Width="100px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblRPComment" class="styled" runat="server" Text="Comment:"></asp:Label>
        <asp:TextBox ID="txtRPComment" runat="server" Text="" class="readonly" TextMode="MultiLine" Rows="4" 
            Width="400px"></asp:TextBox>
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
        <asp:Label ID="lblSeparator2" class="styledseparator" runat="server" Text="Stack Geographic Coordinate Information"></asp:Label>
        <asp:Label ID="lblNoRPGeoCoordInfo" runat="server" Text=""></asp:Label>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="LblLatitudeMeasure" class="styled" runat="server" Text="Latitude:"></asp:Label>
        <asp:TextBox ID="TxtLatitudeMeasure" runat="server" Text="" class="readonly" ReadOnly="True"
            Width="250px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="LblLongitudeMeasure" class="styled" runat="server" Text="Longitude:"></asp:Label>
        <asp:TextBox ID="TxtLongitudeMeasure" runat="server" Text="" class="readonly" ReadOnly="True"
            Width="250px"></asp:TextBox>
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
        <asp:TextBox ID="TxtHorCollectionMetCode" runat="server" Text="" class="readonly" Rows="4" 
            ReadOnly="True" Width="400px" Font-Names="Verdana" Font-Size="Small" TextMode="MultiLine"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="LblHorizontalAccuracyMeasure" class="styled" runat="server" Text="Accuracy Measure (m):"></asp:Label>
        <asp:TextBox ID="TxtHorizontalAccuracyMeasure" runat="server" Text="" class="readonly"
            ReadOnly="True" Width="250px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="LblHorReferenceDatCode" class="styled" runat="server" Text="Horizontal Reference Datum:"></asp:Label>
        <asp:TextBox ID="TxtHorReferenceDatCode" runat="server" Text="" class="readonly"
            ReadOnly="True" Width="250px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="LblGeographicComment" class="styled" runat="server" Text="Comment:"></asp:Label>
        <asp:TextBox ID="TxtGeographicComment" runat="server" Text="" class="readonly" TextMode="MultiLine" Rows="4" 
            Width="400px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblLastEISSubmit_SGC" class="styled" runat="server" Text="Last Submitted to EPA:"></asp:Label>
        <asp:TextBox ID="txtlastEISSubmit_SGC" class="readonly" runat="server" Text="" ReadOnly="True"
            Width="200px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblLastUpdate_SGC" class="styled" runat="server" Text="Last Updated On:"></asp:Label>
        <asp:TextBox ID="txtLastUpdate_SGC" class="readonly" runat="server" Text="" ReadOnly="True"
            Width="300px"></asp:TextBox>
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