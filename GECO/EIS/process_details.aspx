<%@ Page Title="Process Details - GECO Facility Inventory" Language="VB" MasterPageFile="eismaster.master"
    AutoEventWireup="false" Inherits="GECO.eis_process_details" Codebehind="process_details.aspx.vb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style type="text/css">
        .style2 {
            width: 211px;
        }

        .style4 {
            width: 233px;
        }

        .style5 {
            width: 283px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
    <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></act:ToolkitScriptManager>
    <div class="pageheader">
        Process Details
        <asp:TextBox ID="txtEmissionUnitID" runat="server" Text="" class="readonly" ReadOnly="True"
            Visible="false" Width="10px"></asp:TextBox>
        <asp:Button ID="btnReturnToSummary" runat="server" Text="Summary" CausesValidation="False"
            CssClass="summarybutton" PostBackUrl="~/eis/process_summary.aspx" UseSubmitBehavior="False" />
    </div>
    <div class="fieldwrapperseparator">
        <asp:Label ID="lblSeparatorNoText1" class="styledseparator" runat="server" Text="Process"></asp:Label>
        <div class="sepbuttons">
            <asp:Button ID="btnAddProcess" runat="server" Text="Add" ToolTip="" Font-Size="Small"
                CausesValidation="False" />
            <act:ModalPopupExtender ID="btnAddProcess_ModalPopupExtender" runat="server" BackgroundCssClass="modalProgressGreyBackground"
                CancelControlID="btnCancel" DynamicServicePath="" Enabled="True" PopupControlID="pnlPopUpAddProcess"
                TargetControlID="btnAddProcess">
            </act:ModalPopupExtender>
            &nbsp;<asp:Button ID="btnEdit" runat="server" Text="Edit" ToolTip="" Font-Size="Small"
                CausesValidation="False" UseSubmitBehavior="False" />
        </div>
        <asp:Label ID="lblEmissionUnitStatusWarning" runat="server" Font-Bold="True"
            Font-Size="Small" ForeColor="#CC0000"></asp:Label>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblEmissionUnitID" class="styled" runat="server" Text="Emission Unit ID:"></asp:Label>
        <asp:HyperLink ID="hlinkEmissionUnitID" runat="server" ToolTip="Click to go to Emission Unit Details"></asp:HyperLink>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblEmissionUnitStatus" class="styled" runat="server" Text="Emission Unit Status:"></asp:Label>
        <asp:TextBox ID="txtEmissionUnitStatus" runat="server" Text="" class="readonly" ReadOnly="True"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblEmissionUnitDesc" class="styled" runat="server" Text="Emission Unit Description:"></asp:Label>
        <asp:TextBox ID="txtEmissionUnitDesc" runat="server" Text="" class="readonly" ReadOnly="True"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblProcessID" class="styled" runat="server" Text="Process ID:"></asp:Label>
        <asp:TextBox ID="txtProcessID" runat="server" Text="" class="readonly" ReadOnly="True"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblProcessDescription" class="styled" runat="server" Text="Process Description:"></asp:Label>
        <asp:TextBox ID="txtProcessDescription" runat="server" Text="" class="readonly" ReadOnly="True"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblSourceClassCode" class="styled" runat="server" Text="Source Classification Code:"></asp:Label>
        <asp:TextBox ID="txtSourceClassCode" runat="server" Text="" class="readonly" ReadOnly="True"
            Width="100px"></asp:TextBox>
        <asp:Label ID="lblSccDetails" runat="server" />
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblProcessComment" class="styled" runat="server" Text="Process Comment:"></asp:Label>
        <asp:TextBox ID="txtProcessComment" runat="server" Text="" class="readonly" TextMode="MultiLine" Rows="4" 
            runat="server" Width="400px" ReadOnly="True"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblLastEISSubmit" class="styled" runat="server" Text="Last Submitted to EPA:"></asp:Label>
        <asp:TextBox ID="txtLastEISSubmit" class="readonly" runat="server" Text="" ReadOnly="True"
            Width="200px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblLastUpdated" class="styled" runat="server" Text="Last Updated On:"></asp:Label>
        <asp:TextBox ID="txtLastUpdate" class="readonly" runat="server" Text="" ReadOnly="True"></asp:TextBox>
    </div>
    <div class="fieldwrapperseparator">
        <asp:Label ID="lblSeparator" class="styledseparator" runat="server" Text="Release Point Apportionment Information"></asp:Label>
        <div class="sepbuttons">
            <asp:Button ID="btnEditRPApportion" runat="server" Text="Edit" ToolTip="" Font-Size="Small"
                CausesValidation="False" UseSubmitBehavior="False" />
        </div>
        <asp:Label ID="lblRPApportionInfoWarning" runat="server" Font-Size="Small" ForeColor="#CA0000"
            CssClass="labelwarningleft"></asp:Label>
    </div>
    <div class="gridview">
        <asp:GridView ID="gvwRPApportionment" runat="server" DataSourceID="sqldsRPApportionment"
            CellPadding="4" Font-Names="Verdana" Font-Size="Small" ForeColor="#333333" GridLines="None"
            AutoGenerateColumns="False">
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#EFF3FB" />
            <Columns>
                <asp:BoundField DataField="RELEASEPOINTID" HeaderText="Release Point ID" SortExpression="RELEASEPOINTID">
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                </asp:BoundField>
                <asp:BoundField DataField="STRRPDESCRIPTION" HeaderText="Release Point Description">
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                </asp:BoundField>
                <asp:BoundField DataField="RPType" HeaderText="Release Point Type">
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                </asp:BoundField>
                <asp:BoundField DataField="Apportionment" HeaderText="Apportionment %">
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                </asp:BoundField>
                <%--<asp:BoundField DataField="STRRPAPPORTIONMENTCOMMENT" HeaderText="Release Point Apportionment Comment">
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                </asp:BoundField>--%>
                <asp:BoundField DataField="LASTEISSUBMITDATE" HeaderText="Last Submission to EPA">
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                </asp:BoundField>
            </Columns>
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#2461BF" />
            <AlternatingRowStyle BackColor="White" />
        </asp:GridView>
        <asp:SqlDataSource ID="sqldsRPApportionment" runat="server"></asp:SqlDataSource>
    </div>
    <div class="fieldwrapperseparator">
        <asp:Label ID="lblProcessControlApproach" class="styledseparator" runat="server"
            Text="Process Control Approach"></asp:Label>
        <asp:Label ID="lblProcessControlApproachWarning" runat="server" Font-Size="Small"
            ForeColor="#CA0000" CssClass="labelwarningleft"></asp:Label>
        <div class="sepbuttons">
            <asp:Button ID="btnAddControlApproach" runat="server" Text="Add" Font-Size="Small"
                CausesValidation="False" />
            <act:ModalPopupExtender ID="btnAddControlApproach_ModalPopupExtender" runat="server"
                BackgroundCssClass="modalProgressGreyBackground" CancelControlID="btnCancelNewProcessControlApproach"
                DynamicServicePath="" Enabled="True" PopupControlID="pnlPopUpAddProcessControlApproach"
                TargetControlID="btnAddControlApproach">
            </act:ModalPopupExtender>
            <asp:Button ID="btnEditControlApproach" runat="server" Text="Edit" Font-Size="Small"
                CausesValidation="False" UseSubmitBehavior="False" />
        </div>
    </div>
    <asp:Panel ID="pnlProcessControlApproach" runat="server">
        <div class="fieldwrapper">
            <asp:Label ID="lblControlApproachDescription" class="styled" runat="server" Text="Control Approach Description:"></asp:Label>
            <asp:TextBox ID="txtControlApproachDescription" class="readonly" runat="server" Text=""
                ReadOnly="True"></asp:TextBox>
        </div>
        <div class="fieldwrapper">
            <asp:Label ID="lblPctCtrlApproachCapEffic" class="styled" runat="server" Text="Percent Control Approach Capture Efficiency:"></asp:Label>
            <asp:TextBox ID="txtPctCtrlApproachCapEffic" class="readonly" runat="server" Text=""
                ReadOnly="True"></asp:TextBox>
        </div>
        <div class="fieldwrapper">
            <asp:Label ID="lblPctCtrlApproachEffect" class="styled" runat="server" Text="Percent Control Approach Effectiveness:"></asp:Label>
            <asp:TextBox ID="txtPctCtrlApproachEffect" class="readonly" runat="server" Text=""
                ReadOnly="True"></asp:TextBox>
        </div>
        <div class="fieldwrapper">
            <asp:Label ID="lblFirstInventoryYear" class="styled" runat="server" Text="First Inventory Year:"></asp:Label>
            <asp:TextBox ID="txtFirstInventoryYear" class="readonly" runat="server" Text="" ReadOnly="True"></asp:TextBox>
        </div>
        <div class="fieldwrapper">
            <asp:Label ID="lblLastInventoryYear" class="styled" runat="server" Text="Last Inventory Year:"></asp:Label>
            <asp:TextBox ID="txtLastInventoryYear" class="readonly" runat="server" Text="" ReadOnly="True"></asp:TextBox>
        </div>
        <div class="fieldwrapper">
            <asp:Label ID="lblControlApproachComment" class="styled" runat="server" Text="Control Approach Comment:"></asp:Label>
            <asp:TextBox ID="txtControlApproachComment" class="readonly" runat="server" Text="" Rows="4" 
                TextMode="MultiLine" Width="400px"></asp:TextBox>
        </div>
        <div class="fieldwrapper">
            <asp:Label ID="lbllastsubmitEPA_CP" class="styled" runat="server" Text="Last Submitted to EPA:"></asp:Label>
            <asp:TextBox ID="txtLastSubmitEPA_CP" class="readonly" runat="server" Text="" ReadOnly="True"></asp:TextBox>
        </div>
        <div class="fieldwrapper">
            <asp:Label ID="lbllastupdate_CP" class="styled" runat="server" Text="Last Updated On:"></asp:Label>
            <asp:TextBox ID="txtLastUpdate_CP" class="readonly" runat="server" Text="" ReadOnly="True"></asp:TextBox>
        </div>
        <div class="fieldwrapperseparator">
            <asp:Label ID="Label3" class="styledseparator" runat="server" Text="Process Control Measures"></asp:Label>
            <asp:Label ID="lblProcessControlMeasureWarning" runat="server" Font-Size="Small"
                ForeColor="#CA0000" CssClass="labelwarningleft"></asp:Label>
        </div>
        <div class="gridview">
            <asp:GridView ID="gvwProcessControlMeasure" runat="server" DataSourceID="SqlDataSourceID1"
                CellPadding="4" Font-Names="Verdana" Font-Size="Small" ForeColor="#333333" GridLines="None"
                AutoGenerateColumns="False">
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:BoundField DataField="CDType" HeaderText="Control Device" SortExpression="CDType">
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:BoundField DataField="LastEISSubmitDate" HeaderText="Last Submission to EPA">
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:BoundField>
                </Columns>
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>
            <asp:SqlDataSource ID="SqlDataSourceID1" runat="server"></asp:SqlDataSource>
        </div>
        <div class="fieldwrapperseparator">
            <asp:Label ID="Label1" class="styledseparator" runat="server" Text="Process Control Pollutants"></asp:Label>
            <asp:Label ID="lblProcessControlPollutantWarning" runat="server" Font-Size="Small"
                ForeColor="#CA0000" CssClass="labelwarningleft"></asp:Label>
        </div>
        <div class="gridview">
            <asp:GridView ID="gvwProcessControlPollutant" runat="server" DataSourceID="SqlDataSourceID2"
                CellPadding="4" Font-Names="Verdana" Font-Size="Small" ForeColor="#333333" GridLines="None"
                AutoGenerateColumns="False">
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:BoundField DataField="PollutantType" HeaderText="Pollutant">
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:BoundField DataField="CtrlEfficiency" HeaderText="Reduction Efficiency (%)">
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:BoundField DataField="LASTEISSUBMITDATE" HeaderText="Last Submission to EPA">
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
            <asp:SqlDataSource ID="SqlDataSourceID2" runat="server"></asp:SqlDataSource>
        </div>
    </asp:Panel>
    <br />
    <br />
    <div class="fieldwrapperseparator">
        <asp:Label ID="lblRPEmissions" class="styledseparator" runat="server" Text="Annual Reporting Period Emissions"></asp:Label>
        <asp:Label ID="lblGVWReportingPeriodEmpty" runat="server" Font-Size="Small"
            ForeColor="#0033CC" CssClass="labelwarningleft"></asp:Label>
    </div>
    <div class="gridview">
        <asp:GridView ID="gvwReportingPeriods" runat="server" DataSourceID="sqldsReportingPeriod"
            CellPadding="4" Font-Names="Verdana" Font-Size="Small" ForeColor="#333333" GridLines="None"
            AutoGenerateColumns="False">
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#EFF3FB" />
            <Columns>
                <asp:BoundField DataField="strPollutantDescription" HeaderText="Pollutant"
                    SortExpression="strPollutantDescription">
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Font-Size="Small" />
                </asp:BoundField>
                <asp:BoundField DataField="RptPeriodType" HeaderText="Reporting Period Type"
                    SortExpression="RptPeriodType">
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="curr_flttotalemissions" HeaderText="Year1"
                    NullDisplayText="No Data">
                    <ItemStyle Font-Size="Small" />
                </asp:BoundField>
                <asp:BoundField DataField="prev1_flttotalemissions" HeaderText="Year2"
                    NullDisplayText="No Data">
                    <ItemStyle Font-Size="Small" />
                </asp:BoundField>
                <asp:BoundField DataField="prev2_flttotalemissions" HeaderText="Year3"
                    NullDisplayText="No Data">
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top"
                        Font-Size="Small" />
                </asp:BoundField>
                <asp:BoundField DataField="prev3_flttotalemissions" HeaderText="Year4"
                    NullDisplayText="No Data">
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top"
                        Font-Size="Small" />
                </asp:BoundField>
                <asp:BoundField DataField="prev4_flttotalemissions" HeaderText="Year5"
                    NullDisplayText="No Data">
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top"
                        Font-Size="Small" />
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                </asp:BoundField>
            </Columns>
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#2461BF" />
            <AlternatingRowStyle BackColor="White" />
        </asp:GridView>
        <asp:SqlDataSource ID="sqldsReportingPeriod" runat="server"></asp:SqlDataSource>
    </div>
    <div class="fieldwrapperseparator">
        <asp:Label ID="lblSeparatorOnly" class="styledseparator" runat="server" Text="_"
            Font-Size="Small" BackColor="#ffffff" ForeColor="#ffffff"></asp:Label>
    </div>
    <div class="buttonwrapper">
        <asp:Button runat="server" ID="btnSummary2" CssClass="buttondiv" Text="Summary" CausesValidation="False"
            PostBackUrl="process_summary.aspx" UseSubmitBehavior="False" />
    </div>
    <asp:Panel ID="pnlPopUpAddProcess" BackColor="#ffffff" runat="server" BorderColor="#333399"
        BorderStyle="Ridge" ScrollBars="Auto" Width="450px" Style="display: none;">
        <table border="0" cellpadding="2" cellspacing="1" width="100%">
            <tr>
                <td align="center" colspan="2">
                    <strong><span style="color: #4169e1; font-size: 12pt; font-family: Verdana;">Add New
                        Process</span></strong>
                </td>
            </tr>
            <tr>
                <td align="center" width="30%" colspan="2">
                    <asp:ValidationSummary ID="sumvAddProcessModalpopup" runat="server" HeaderText="You have received the following errors:"
                        ValidationGroup="AddProcess" />
                </td>
            </tr>
            <tr>
                <td align="right" width="30%"></td>
                <td>&nbsp;
                </td>
            </tr>
            <tr>
                <td align="right" width="30%" valign="top">&nbsp;<asp:Label ID="lblexistEmissionUnitID" runat="server" Text="Emission Unit ID:"
                    Font-Names="Verdana" Font-Size="Small"></asp:Label>
                </td>
                <td valign="top">
                    <asp:TextBox ID="txtExistEmissionUnitID" runat="server" Font-Names="Arial" Font-Size="Small"
                        MaxLength="6" ReadOnly="True" Height="22px" BorderStyle="None"></asp:TextBox>
                    <act:FilteredTextBoxExtender ID="txtExistEmissionUnitID_FilteredTextBoxExtender"
                        runat="server" Enabled="True" TargetControlID="txtExistEmissionUnitID" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters"
                        ValidChars="-">
                    </act:FilteredTextBoxExtender>
                </td>
            </tr>
            <tr>
                <td align="right" width="30%" valign="top">&nbsp;<asp:Label ID="lblNewProcessID" runat="server" CssClass="label" Text="Process ID:"></asp:Label>
                </td>
                <td valign="top">
                    <asp:TextBox ID="txtNewProcessID" runat="server" Font-Names="Arial" Font-Size="Small"
                        MaxLength="6"></asp:TextBox>
                    <act:FilteredTextBoxExtender ID="txtNewProcessID_FilteredTextBoxExtender" runat="server"
                        Enabled="True" TargetControlID="txtNewProcessID" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters"
                        ValidChars="-">
                    </act:FilteredTextBoxExtender>
                    <asp:RequiredFieldValidator ID="reqvNewProcessID" runat="server" ControlToValidate="txtNewProcessID"
                        Display="Dynamic" ErrorMessage="Process ID is required." ValidationGroup="AddProcess">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="right" valign="top" width="30%">&nbsp;
                </td>
                <td valign="top">
                    <asp:CustomValidator ID="cusvProcessID" runat="server" ControlToValidate="txtNewProcessID"
                        OnServerValidate="ProcessIDCheck" ErrorMessage="* Process ID already used. Enter another."
                        Font-Names="Arial" Font-Size="Small" Display="Dynamic" ValidationGroup="AddProcess"></asp:CustomValidator>
                </td>
            </tr>
            <tr>
                <td align="right" width="30%" valign="top">&nbsp;<asp:Label ID="lblexistReleasePointID" runat="server" CssClass="label" Text="Release Point ID:"
                    BorderStyle="None" Font-Names="Verdana" Font-Size="Small"></asp:Label>
                </td>
                <td valign="top">
                    <asp:DropDownList ID="ddlexistReleasePointID" runat="server" Font-Names="Verdana"
                        Font-Size="Small">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="reqvexistReleasePointID" ControlToValidate="ddlexistReleasePointID"
                        InitialValue="--Select Release Point ID--" runat="server" ErrorMessage="The Release Point ID is required."
                        Display="Dynamic" ValidationGroup="AddProcess">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="right" valign="top" width="30%">&nbsp;
                </td>
            </tr>
            <tr>
                <td align="right" width="30%" valign="top">&nbsp;<asp:Label ID="lblNewProcessDesc" runat="server" CssClass="label" Text="Process Description:"></asp:Label>
                </td>
                <td valign="top">
                    <asp:TextBox ID="txtNewProcessDesc" runat="server" Font-Names="Arial" Font-Size="Small" Rows="4" 
                        MaxLength="100" TextMode="MultiLine"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="reqvNewProcessID0" runat="server" ControlToValidate="txtNewProcessDesc"
                        Display="Dynamic" ErrorMessage="Process Description is required." ValidationGroup="AddProcess">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2" class="style4">&nbsp;<asp:Label ID="lbl" runat="server" CssClass="label" Text="After saving you will enter more details on the form that appears."></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">&nbsp;&nbsp;
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <asp:Button ID="btnInsertProcess" runat="server" Text="Save" Width="70px" ValidationGroup="AddProcess" />
                    &nbsp;&nbsp;
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="False"
                        Width="70px" />
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">&nbsp;&nbsp;
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pnlPopUpAddProcessControlApproach" BackColor="#ffffff" runat="server"
        BorderColor="#333399" BorderStyle="Ridge" ScrollBars="Auto" Width="750px" Style="display: none;">
        <table border="0" cellpadding="2" cellspacing="1" width="100%">
            <tr>
                <td align="center" colspan="2">
                    <strong><span style="color: #4169e1; font-size: 12pt; font-family: Verdana;">Add Process
                        Control Approach</span></strong>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <asp:ValidationSummary ID="sumvAddProcessConAppModalPopup" runat="server" HeaderText="You have received the following errors:"
                        ValidationGroup="vgAddProcessControlApproach" />
                </td>
            </tr>
            <tr>
                <td align="right" width="30%"></td>
                <td>&nbsp;
                </td>
            </tr>
            <tr>
                <td align="right" width="30%" valign="top">&nbsp;<asp:Label ID="lblexistemissionUnitID2" runat="server" CssClass="label" Text="Emission Unit ID:"></asp:Label>
                </td>
                <td valign="top">
                    <asp:TextBox ID="txtExistEmissionUnitID2" runat="server" Font-Names="Arial" Font-Size="Small"
                        MaxLength="6" ReadOnly="True"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right" valign="top" width="30%">
                    <asp:Label ID="lblexistProcessID2" runat="server" CssClass="label" Text="Process ID:"></asp:Label>
                </td>
                <td valign="top">
                    <asp:TextBox ID="txtExistProcessID" runat="server" Font-Names="Arial" Font-Size="Small"
                        MaxLength="6" ReadOnly="True"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right" valign="top" width="30%">
                    <asp:Label ID="lblNewProcessControlApproachDesc" runat="server" CssClass="label"
                        Text="Control Approach Description:"></asp:Label>
                </td>
                <td valign="top">
                    <asp:TextBox ID="txtNewProcessControlApproachDesc" runat="server" Font-Names="Arial"
                        Font-Size="Small" MaxLength="100" Width="300px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="reqvNewProcessControlApproachDesc" runat="server"
                        ControlToValidate="txtNewProcessControlApproachDesc" Display="Dynamic" ErrorMessage="Control Approach description is required."
                        ValidationGroup="vgAddProcessControlApproach">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="right" valign="top" class="style5">
                    <asp:Label ID="lblProcessCACaptureEffic" runat="server" CssClass="label" Text="Control Approach Capture Efficiency (%):"></asp:Label>
                </td>
                <td valign="top">
                    <asp:TextBox ID="txtProcessCACaptureEffic" runat="server" Font-Names="Arial" Font-Size="Small"
                        MaxLength="4" Width="100px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rqvProcessCACaptureEffic" runat="server" ControlToValidate="txtProcessCACaptureEffic"
                        ErrorMessage="The Process Control Approach Capture Efficiency is required." ValidationGroup="vgAddProcessControlApproach">*</asp:RequiredFieldValidator>
                    <asp:RangeValidator ID="rngvProcessCACaptureEffic" runat="server" ValidationGroup="vgAddProcessControlApproach"
                        ErrorMessage="The Process Control Approach Capture Efficiency must be between 1 and 100."
                        ControlToValidate="txtProcessCACaptureEffic" MaximumValue="100" MinimumValue="1"
                        Type="Double">Must be between 1 and 100</asp:RangeValidator>
                </td>
            </tr>
            <tr>
                <td align="right" valign="top" class="style5">
                    <asp:Label ID="lblProcessCAControlEffect" runat="server" CssClass="label" Text="Control Approach Effectiveness (%):"></asp:Label>
                </td>
                <td valign="top">
                    <asp:TextBox ID="txtProcessCAControlEffect" runat="server" Font-Names="Arial" Font-Size="Small"
                        MaxLength="4" Width="100px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rqvProcessCAControlEffect" runat="server" ErrorMessage="The Process Control Approach Control Effectiveness is required."
                        ControlToValidate="txtProcessCAControlEffect" ValidationGroup="vgAddProcessControlApproach">*</asp:RequiredFieldValidator>
                    <asp:RangeValidator ID="rngvProcessCAControlEffect" runat="server" ControlToValidate="txtProcessCAControlEffect"
                        MaximumValue="100" MinimumValue="1" ValidationGroup="vgAddProcessControlApproach"
                        ErrorMessage="The Process Control Approach Control Effectiveness must be between 1 and 100."
                        Type="Double">Must be between 1 and 100</asp:RangeValidator>
                </td>
            </tr>
            <tr>
                <td align="right" valign="top" class="style2" colspan="2"></td>
            </tr>
            <tr>
                <td align="center" colspan="2" class="style4">
                    <asp:Label ID="lblAddProcessControlAppMsg" runat="server" CssClass="label" Text="After saving you will enter more details on the form that appears."
                        Width="700px"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">&nbsp;&nbsp;
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <asp:Button ID="btnInsertProcessControlApproach" runat="server" Text="Save" Width="70px"
                        ValidationGroup="vgAddProcessControlApproach" />
                    &nbsp;&nbsp;
                    <asp:Button ID="btnCancelNewProcessControlApproach" runat="server" Text="Cancel"
                        CausesValidation="False" Width="70px" />
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">&nbsp;&nbsp;
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>