<%@ Page Title="Emission Unit Details - GECO Facility Inventory" Language="VB" MaintainScrollPositionOnPostback="true" MasterPageFile="eismaster.master"
    AutoEventWireup="false" Inherits="GECO.eis_emissionunit_details" Codebehind="emissionunit_details.aspx.vb" %>
<%@ Register src="../Controls/PreventRePost.ascx" tagname="PreventRePost" tagprefix="uc1" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="pageheader">
        Emission Unit Details<asp:Button ID="btnSummary1"
            runat="server" Text="Summary" CausesValidation="False"
            CssClass="summarybutton" PostBackUrl="~/eis/emissionunit_summary.aspx" />
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblEmissionUnitID" class="styled" runat="server" Text="Emission Unit ID:"></asp:Label>
        <asp:TextBox ID="txtEmissionUnitID" CssClass="readonly" runat="server" Text="" MaxLength="6"
            ReadOnly="True" Width="100px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblUnitDescription" CssClass="styled" runat="server" Text="Emission Unit Description:"></asp:Label>
        <asp:TextBox ID="txtUnitDescription" MaxLength="100" CssClass="readonly" runat="server" Text="" ReadOnly="True"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblUnitTypeCode" CssClass="styled" runat="server"
            Text="Unit Type:"></asp:Label>
        <asp:TextBox ID="txtUnitTypeCode" CssClass="readonly" runat="server" Text=""
            ReadOnly="True" Width="325px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblUnitStatusCode" CssClass="styled" runat="server" Text="Operating Status:"></asp:Label>
        <asp:TextBox ID="txtUnitStatusDesc" CssClass="readonly" runat="server" Text=""
            ReadOnly="True" Width="200px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblUnitOperationDate" CssClass="styled" runat="server" Text="Unit Placed In Operation:"></asp:Label>
        <asp:TextBox ID="txtUnitOperationDate" CssClass="readonly" runat="server" Text=""
            ReadOnly="True" Width="150px"></asp:TextBox>
    </div>
    <asp:Panel ID="pnlFuelBurning" runat="server">
        <div class="fieldwrapper">
            <asp:Label ID="lblUnitDesignCapacity" CssClass="styled" runat="server" Text="Design Capacity:"></asp:Label>
            <asp:TextBox ID="txtUnitDesignCapacity" CssClass="readonly" runat="server" Text=""
                ReadOnly="True"></asp:TextBox>
        </div>
    </asp:Panel>
    <div style="text-align: center;">
        <asp:Label ID="lblElecGenerating1" runat="server" Font-Bold="True" Font-Size="Small"
            ForeColor="#990033"></asp:Label><br />
        <asp:Label ID="lblElecGenerating2" runat="server" Font-Bold="True" Font-Size="Small"
            ForeColor="#990033"></asp:Label>
    </div>
    <asp:Panel ID="pnlElecGenerating" runat="server">
        <div class="fieldwrapper">
            <asp:Label ID="lblMaxNameplatecapacity" CssClass="styled" runat="server" Text="Maximum Nameplate Capacity:"></asp:Label>
            <asp:TextBox ID="txtMaxNameplateCapacity" CssClass="readonly" runat="server" Text=""
                ReadOnly="True"></asp:TextBox>
        </div>
    </asp:Panel>
    <div class="fieldwrapper">
        <asp:Label ID="lblUnitComment" runat="server" CssClass="styled" Text="Comment:"></asp:Label>
        <asp:TextBox ID="txtUnitComment" runat="server" ReadOnly="True"
            TextMode="MultiLine" Rows="4" Text="" CssClass="readonly"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblLastEISSubmit" CssClass="styled" runat="server" Text="Last Submitted to EPA:"></asp:Label>
        <asp:TextBox ID="txtLastEISSubmit" CssClass="readonly" runat="server" Text=""
            ReadOnly="True" Width="200px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblEULastUpdated" CssClass="styled" runat="server"
            Text="Last Updated on:"></asp:Label>
        <asp:TextBox ID="txtEULastUpdated" CssClass="readonly" runat="server" Text=""
            ReadOnly="True"></asp:TextBox>
    </div>
    <br />
    <div class="fieldwrapperseparator">
        <asp:Label ID="lblSeparator3" CssClass="styledseparator" runat="server" Text="Emission Unit Control Approach"></asp:Label>
        <asp:Label ID="lblUnitCtrlApprWarning" runat="server" Font-Size="Small"
            ForeColor="#CA0000" CssClass="labelwarningleft"></asp:Label>
    </div>
    <asp:Panel ID="pnlUnitControlApproach" runat="server">
        <div class="fieldwrapper">
            <asp:Label ID="lblControlApproachDescription" CssClass="styled" runat="server" Text="Control Approach Description:"></asp:Label>
            <asp:TextBox ID="txtControlApproachDescription" CssClass="readonly" runat="server" Text=""
                ReadOnly="True"></asp:TextBox>
        </div>
        <div class="fieldwrapper">
            <asp:Label ID="lblPctCtrlApproachCapEffic" CssClass="styled" runat="server" Text="Percent Control Approach Capture Efficiency:"></asp:Label>
            <asp:TextBox ID="txtPctCtrlApproachCapEffic" CssClass="readonly" runat="server" Text=""
                ReadOnly="True" Width="100px"></asp:TextBox>
        </div>
        <div class="fieldwrapper">
            <asp:Label ID="lblPctCtrlApproachEffect" CssClass="styled" runat="server" Text="Percent Control Approach Effectiveness:"></asp:Label>
            <asp:TextBox ID="txtPctCtrlApproachEffect" CssClass="readonly" runat="server" Text=""
                ReadOnly="True" Width="100px"></asp:TextBox>
        </div>
        <div class="fieldwrapper">
            <asp:Label ID="lblFirstInventoryYear" CssClass="styled" runat="server" Text="First Inventory Year:"></asp:Label>
            <asp:TextBox ID="txtFirstInventoryYear" CssClass="readonly" runat="server" Text=""
                ReadOnly="True" Width="100px"></asp:TextBox>
        </div>
        <div class="fieldwrapper">
            <asp:Label ID="lblLastInventoryYear" CssClass="styled" runat="server" Text="Last Inventory Year:"></asp:Label>
            <asp:TextBox ID="txtLastInventoryYear" CssClass="readonly" runat="server" Text=""
                ReadOnly="True" Width="100px"></asp:TextBox>
        </div>
        <div class="fieldwrapper">
            <asp:Label ID="lblControlApproachComment" CssClass="styled" runat="server" Text="Control Approach Comment:"></asp:Label>
            <asp:TextBox ID="txtControlApproachComment" CssClass="readonly" runat="server" Text="" TextMode="MultiLine" Rows="4"
                ReadOnly="True"></asp:TextBox>
        </div>
        <div class="fieldwrapper">
            <asp:Label ID="lblEUCtrlApprLastUpdated" CssClass="styled" runat="server"
                Text="Last Updated on:"></asp:Label>
            <asp:TextBox ID="txtEUCtrlApprLastUpdated" CssClass="readonly" runat="server" Text=""
                ReadOnly="True"></asp:TextBox>
        </div>
        <div class="fieldwrapperseparator">
            <asp:Label ID="Label3" class="styledseparator" runat="server" Text="Emissions Unit Control Measures"></asp:Label>
            <asp:Label ID="lblUnitControlMeasureWarning" runat="server" Font-Size="Small"
                ForeColor="#CA0000" CssClass="labelwarningleft"></asp:Label>
        </div>

        <div class="gridview">
            <asp:GridView ID="gvwUnitControlMeasure" runat="server" 
                CellPadding="4" Font-Names="Verdana" Font-Size="Small" ForeColor="#333333"
                GridLines="None" AutoGenerateColumns="False">
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:BoundField DataField="CDType" HeaderText="Control Device" SortExpression="CDType">
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:BoundField DataField="LastEISSubmitDate" HeaderText="Last EPA Submittal"
                        DataFormatString="{0:d}">
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:BoundField>
                </Columns>
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>
        </div>
        <div class="fieldwrapperseparator">
            <asp:Label ID="Label1" class="styledseparator" runat="server" Text="Emissions Unit Control Pollutants"></asp:Label>
            <asp:Label ID="lblUnitControlPollutantWarning" runat="server" Font-Size="Small"
                ForeColor="#CA0000" CssClass="labelwarningleft"></asp:Label>
        </div>
        <div class="gridview">
            <asp:GridView ID="gvwUnitControlPollutant" runat="server" 
                CellPadding="4" Font-Names="Verdana" Font-Size="Small" ForeColor="#333333"
                GridLines="None" AutoGenerateColumns="False">
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:BoundField DataField="PollutantType" HeaderText="Pollutant">
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:BoundField DataField="MeasureEfficiency" HeaderText="Reduction Efficiency (%)">
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:BoundField DataField="LastEISSubmitDate" HeaderText="Last EPA Submittal"
                        DataFormatString="{0:d}">
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:BoundField DataField="CalculatedReduction" HeaderText="Calculated Overall<br> Pollutant Reduction (%)" ReadOnly="True" SortExpression="CalculatedReduction" HtmlEncode="false" />
                </Columns>
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>
        </div>
    </asp:Panel>
    <div class="fieldwrapperseparator">
        <asp:Label ID="lblProcesses" class="styledseparator" runat="server" Text="Processes"></asp:Label>
        <asp:Label ID="lblProcessWarning" runat="server" Font-Size="Small"
            ForeColor="#CA0000" CssClass="labelwarningleft"></asp:Label>
    </div>
    <div class="gridview">
        <asp:GridView ID="gvwProcesses" runat="server" 
            CellPadding="4" Font-Names="Arial" Font-Size="Small" ForeColor="#333333" GridLines="None"
            AutoGenerateColumns="False">
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#EFF3FB" />
            <Columns>
                <asp:HyperLinkField DataNavigateUrlFields="ProcessID,EmissionsUnitID" NavigateUrl="~/eis/process_details.aspx"
                    DataNavigateUrlFormatString="process_details.aspx?ep={0}&amp;eu={1}" DataTextField="PROCESSID"
                    HeaderText="Process ID" SortExpression="PROCESSID">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                </asp:HyperLinkField>
                <asp:HyperLinkField DataNavigateUrlFields="ProcessID,EmissionsUnitID"
                    DataNavigateUrlFormatString="process_details.aspx?ep={0}&amp;eu={1}"
                    DataTextField="STRPROCESSDESCRIPTION" HeaderText="Process Description"
                    NavigateUrl="~/eis/process_details.aspx"
                    SortExpression="STRPROCESSDESCRIPTION">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:HyperLinkField>
                <asp:BoundField DataField="SOURCECLASSCODE" HeaderText="SCC">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                </asp:BoundField>
                <asp:BoundField DataField="LastEISSubmitDate" HeaderText="Last EPA Submittal"
                    NullDisplayText="Not Submitted" DataFormatString="{0:d}">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Control Approach" DataField="ControlApproach">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
            </Columns>
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#2461BF" />
            <AlternatingRowStyle BackColor="White" />
        </asp:GridView>
    </div>
    <br />
    <div class="fieldwrapperseparator">
        <asp:Label ID="lblSeparatorOnly" class="styledseparator" runat="server" Text="_"
            Font-Size="Small" BackColor="#ffffff" ForeColor="#ffffff"></asp:Label>
    </div>
    <div class="buttonwrapper">
        <asp:Button runat="server" ID="btnSummary2" CssClass="buttondiv" Text="Summary" CausesValidation="False"
            PostBackUrl="emissionunit_summary.aspx" />
    </div>
    <uc1:PreventRePost ID="PreventRePost1" runat="server" />
</asp:Content>