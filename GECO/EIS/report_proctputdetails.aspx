<%@ Page Title="Report - Process Throughput Details" Language="VB" MasterPageFile="eismaster.master" AutoEventWireup="false" Inherits="GECO.EIS_report_proctputdetails" CodeBehind="report_proctputdetails.aspx.vb" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
    <div class="pageheader">
        Process Reporting Period Details
        <asp:Button ID="btnReportsHome" runat="server" Text="Reports Home" CausesValidation="False"
            CssClass="summarybutton" UseSubmitBehavior="False" />
        &nbsp;&nbsp;
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblFacilitySiteID_Process" class="styled" runat="server"
            Text="Facility ID"></asp:Label>
        <asp:TextBox ID="txtFacilitySiteID_Process" class="readonly" runat="server" Text=""
            ReadOnly="True" Width="300px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblFacilityName_Process" class="styled" runat="server"
            Text="Facility Name"></asp:Label>
        <asp:TextBox ID="txtFacilityName_Process" class="readonly" runat="server" Text=""
            ReadOnly="True" Width="300px"></asp:TextBox>
    </div>
    <div>
        <div class="fieldwrapper">
            <asp:Label ID="lblInventoryYear_Process" CssClass="styled" runat="server"
                Text="Inventory Year"></asp:Label>
            <asp:DropDownList ID="ddlInventoryYear_Process" runat="server" class=""
                AutoPostBack="True">
            </asp:DropDownList>
            &nbsp;
        <asp:Button ID="btnGo_Process" runat="server" Text="GO" />
            &nbsp;<asp:RequiredFieldValidator ID="reqvYear_Process" runat="server"
                ControlToValidate="ddlInventoryYear_Process" Display="Dynamic"
                ErrorMessage="* Select a year" InitialValue="-Select Year-"></asp:RequiredFieldValidator>
        </div>
        <br />
        <div style="text-align: center;">
            <p>
                <asp:Button ID="btnExportProcess" runat="server" Text="Download as Excel" CausesValidation="False"
                    CssClass="summarybutton" UseSubmitBehavior="False"
                    Visible="False" />
            </p>
            <asp:Label ID="lblEmptygvwReportDetails" runat="server" Visible="False"
                Font-Bold="True" Font-Size="Medium" ForeColor="#CC0000"></asp:Label>
        </div>
        <div class="gridview">
            <asp:GridView ID="gvwProcessDetails"
                runat="server"
                AutoGenerateColumns="False"
                DataKeyNames="FacilitySiteID"
                HorizontalAlign="Center"
                ForeColor="#333333"
                Caption="Process Reporting Period Details"
                CssClass="reportview">
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:BoundField DataField="EMISSIONSUNITID"
                        HeaderText="Emissions Unit ID"
                        SortExpression="EMISSIONSUNITID">
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:BoundField DataField="STRUNITDESCRIPTION"
                        HeaderText="Emissisons Unit Desc"
                        SortExpression="STRUNITDESCRIPTION">
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:BoundField DataField="PROCESSID"
                        HeaderText="Process ID"
                        SortExpression="PROCESSID">
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:BoundField DataField="STRPROCESSDESCRIPTION"
                        HeaderText="Process Desc"
                        SortExpression="STRPROCESSDESCRIPTION">
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:BoundField DataField="FLTCALCPARAMETERVALUE"
                        HeaderText="Calculation Parameter Value"
                        SortExpression="FLTCALCPARAMETERVALUE">
                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:BoundField DataField="STRCPUOMDESC"
                        HeaderText="Calculation Parameter Unit of Measure"
                        SortExpression="STRCPUOMDESC">
                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:BoundField DataField="STRCPTYPEDESC"
                        HeaderText="Calculation Parameter Type Desc"
                        SortExpression="STRCPTYPEDESC">
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:BoundField DataField="STRMATERIAL"
                        HeaderText="Material Desc"
                        SortExpression="STRMATERIAL">
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:BoundField DataField="STRREPORTINGPERIODCOMMENT"
                        HeaderText="Reporting Period Comment"
                        SortExpression="STRREPORTINGPERIODCOMMENT">
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:BoundField DataField="INTACTUALHOURSPERPERIOD"
                        HeaderText="Actual Hours Per Period"
                        SortExpression="INTACTUALHOURSPERPERIOD">
                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:BoundField DataField="NUMAVERAGEDAYSPERWEEK"
                        HeaderText="Average Days Per Week"
                        SortExpression="NUMAVERAGEDAYSPERWEEK">
                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:BoundField DataField="NUMAVERAGEHOURSPERDAY"
                        HeaderText="Average Hours per Day"
                        SortExpression="NUMAVERAGEHOURSPERDAY">
                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:BoundField DataField="NUMAVERAGEWEEKSPERPERIOD"
                        HeaderText="Average Weeks per Period"
                        SortExpression="NUMAVERAGEWEEKSPERPERIOD">
                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:BoundField DataField="NUMPERCENTWINTERACTIVITY"
                        HeaderText="Winter Activity %"
                        SortExpression="NUMPERCENTWINTERACTIVITY">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="20px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="NUMPERCENTSPRINGACTIVITY"
                        HeaderText="Spring Activity %"
                        SortExpression="NUMPERCENTSPRINGACTIVITY">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="20px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="NUMPERCENTSUMMERACTIVITY"
                        HeaderText="Summer Activity %"
                        SortExpression="NUMPERCENTSUMMERACTIVITY">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="20px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="NUMPERCENTFALLACTIVITY"
                        HeaderText="Fall Activity %"
                        SortExpression="NUMPERCENTFALLACTIVITY">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="20px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="HEATCONTENT"
                        HeaderText="Heat Content"
                        SortExpression="HEATCONTENT">
                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:BoundField DataField="HCNUMER"
                        HeaderText="Heat Content Numerator"
                        SortExpression="HCNUMER">
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:BoundField DataField="HCDENOM"
                        HeaderText="Heat Content Denominator"
                        SortExpression="HCDENOM">
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ASHCONTENT"
                        HeaderText="Ash Content"
                        SortExpression="ASHCONTENT">
                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:BoundField DataField="SULFURCONTENT"
                        HeaderText="Sullfur Content"
                        SortExpression="SULFURCONTENT">
                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" />
                    </asp:BoundField>
                </Columns>
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle HorizontalAlign="Left" BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>
        </div>
        <br />
    </div>
</asp:Content>
