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
    <div class="fieldwrapperseparator">
        <asp:Label ID="lblEmissionUnit" CssClass="styledseparator" runat="server" Text="Emission Unit"></asp:Label>
        <div class="sepbuttons">
            <asp:Button ID="btnAddEmissionUnit" runat="server" Text="Add" ToolTip="" Font-Size="Small"
                CausesValidation="False" Style="height: 21px" />
            <act:ModalPopupExtender ID="btnAddEmissionUnit_ModalPopupExtender" runat="server"
                BackgroundCssClass="modalProgressGreyBackground" CancelControlID="btnCancelEmissionUnit"
                DynamicServicePath="" Enabled="True" PopupControlID="pnlPopUpAddUnit" TargetControlID="btnAddEmissionUnit">
            </act:ModalPopupExtender>
            &nbsp;
            <asp:Button ID="btnDuplicate" runat="server" Text="Duplicate"
                ToolTip="Create a duplicate of this emission unit." Font-Size="Small"
                CausesValidation="False" Style="height: 21px" />
            <act:ModalPopupExtender ID="btnDuplicate_ModalPopupExtender" runat="server"
                DynamicServicePath="" Enabled="True" PopupControlID="pnlDuplicate" TargetControlID="btnDuplicate">
            </act:ModalPopupExtender>
            &nbsp;
            <asp:Button ID="btnEditEmissionUnit" runat="server" Text="Edit" ToolTip=""
                Font-Size="Small" Height="21px" CausesValidation="False" />
        </div>
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
        <div class="sepbuttons">
            <asp:Button ID="btnEditControlApproach" runat="server" Text="Edit"
                Font-Size="Small" CausesValidation="False" />
            <asp:Button ID="btnAddControlApproach" runat="server" Text="Add" Font-Size="Small" />
            <act:ModalPopupExtender ID="btnAddControlApproach_ModalPopupExtender" runat="server"
                BackgroundCssClass="modalProgressGreyBackground" CancelControlID="btnCancelUnitApproachControl"
                DynamicServicePath="" Enabled="True" PopupControlID="pnlAddUnitControlApproach"
                TargetControlID="btnAddControlApproach">
            </act:ModalPopupExtender>
        </div>
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
        <div class="sepbuttons">
            <asp:Button ID="btnAddProcess" runat="server" Text="Add" Font-Size="Small"
                Height="21px" />
            <act:ModalPopupExtender ID="btnAddprocess_ModalPopupExtender" runat="server"
                BackgroundCssClass="modalProgressGreyBackground" CancelControlID="btnCancelProcess"
                DynamicServicePath="" Enabled="True" PopupControlID="pnlAddProcess"
                TargetControlID="btnAddprocess">
            </act:ModalPopupExtender>
        </div>
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
    <asp:Panel ID="pnlPopUpAddUnit" BackColor="#ffffff" runat="server" BorderColor="#333399"
        BorderStyle="Ridge" ScrollBars="Auto" Width="600px" Style="display: none;">
        <table border="0" cellpadding="2" cellspacing="1" width="100%">
            <tr>
                <td align="center" colspan="2">
                    <strong><span style="color: #4169e1; font-size: 12pt; font-family: Verdana;">Add Emission
                        Unit</span></strong>
                </td>
            </tr>
            <tr>
                <td align="right" width="210px"></td>
                <td>&nbsp;
                </td>
            </tr>
            <tr>
                <td align="right" valign="top" width="210px">&nbsp;<asp:Label ID="lblNewEmissionsUnitID" runat="server" CssClass="label" Text="Emission Unit ID:"></asp:Label>
                </td>
                <td valign="top">
                    <asp:TextBox ID="txtNewEmissionsUnitID" runat="server" Font-Names="Arial" Font-Size="Small"
                        MaxLength="6" CssClass="editable" Width="150px"></asp:TextBox>
                    <act:FilteredTextBoxExtender ID="txtNewEmissionsUnitID_FilteredTextBoxExtender"
                        runat="server" Enabled="True" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters"
                        TargetControlID="txtNewEmissionsUnitID" ValidChars="-">
                    </act:FilteredTextBoxExtender>
                </td>
            </tr>
            <tr>
                <td align="right" valign="top" width="210px">&nbsp;
                </td>
                <td valign="top">
                    <asp:CustomValidator ID="cusvEmissionUnitID" runat="server" ControlToValidate="txtNewEmissionsUnitID"
                        OnServerValidate="EmissionsUnitIDCheck" ErrorMessage="* Unit ID already used. Enter another."
                        Font-Names="Arial" Font-Size="Small" CssClass="validator" Display="Dynamic" Width="100%"
                        ValidationGroup="AddEmissionsUnit"></asp:CustomValidator>
                    <asp:RequiredFieldValidator ID="reqvNewEmissionUnitID" runat="server" ControlToValidate="txtNewEmissionsUnitID"
                        CssClass="validator" ErrorMessage="* Emission unit ID is required." Width="100%"
                        ValidationGroup="AddEmissionsUnit"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="right" valign="top" width="210px">&nbsp;<asp:Label ID="lblNewEmissionUnitDesc" runat="server" CssClass="label" Text="Description:"></asp:Label>
                </td>
                <td valign="top">
                    <asp:TextBox ID="txtNewEmissionUnitDesc" runat="server" Font-Names="Arial" Font-Size="Small"
                        MaxLength="100" TextMode="MultiLine" Rows="4" Width="320px" CssClass="editable"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right" valign="top" width="210px">&nbsp;
                </td>
                <td valign="top">
                    <asp:RequiredFieldValidator ID="reqvNewEmissionUnitDesc" runat="server" ControlToValidate="txtNewEmissionUnitDesc"
                        CssClass="validator" ErrorMessage="* Description is required." Width="100%" ValidationGroup="AddEmissionsUnit"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">&nbsp;<asp:Label ID="lbl" runat="server" CssClass="label" Text="After saving you will enter more details on the form that appears."></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">&nbsp;&nbsp;
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <asp:Button ID="btnInsertEmissionUnit" runat="server" Text="Save" Width="90px" ValidationGroup="AddEmissionsUnit" />
                    &nbsp;&nbsp;
                    <asp:Button ID="btnCancelEmissionUnit" runat="server" Text="Cancel" CausesValidation="False" />
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">&nbsp;&nbsp;
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pnlDuplicate" BackColor="#ffffff" runat="server" BorderColor="#333399"
        BorderStyle="Ridge" ScrollBars="Auto" Width="600px" Style="display: none;">
        <table border="0" cellpadding="2" cellspacing="1" width="100%">
            <tr>
                <td align="center" colspan="2">
                    <strong><span style="color: #4169e1; font-size: 12pt; font-family: Verdana;">Duplicate Emission Unit</span></strong></td>
            </tr>
            <tr>
                <td align="right" width="210px"></td>
                <td>&nbsp;
                </td>
            </tr>
            <tr>
                <td align="right" valign="top" width="210px">&nbsp;<asp:Label ID="Label4" runat="server" CssClass="label" Text="Emission Unit ID:"></asp:Label>
                </td>
                <td valign="top">
                    <asp:TextBox ID="txtDupEmissionsUnitID" runat="server" Font-Names="Arial" Font-Size="Small"
                        MaxLength="6" CssClass="editable" Width="150px"></asp:TextBox>
                    <act:FilteredTextBoxExtender ID="txtDupEmissionsUnitID_FilteredTextBoxExtender"
                        runat="server" Enabled="True" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters"
                        TargetControlID="txtDupEmissionsUnitID" ValidChars="-">
                    </act:FilteredTextBoxExtender>
                </td>
            </tr>
            <tr>
                <td align="right" valign="top" width="210px">&nbsp;
                </td>
                <td valign="top">
                    <asp:CustomValidator ID="cusvDuplicate" runat="server" ControlToValidate="txtDupEmissionsUnitID"
                        OnServerValidate="EmissionsUnitDupIDCheck" ErrorMessage="* Unit ID already used. Enter another."
                        Font-Names="Arial" Font-Size="Small" CssClass="validator" Display="Dynamic" Width="100%"
                        ValidationGroup="DupEmissionsUnit"></asp:CustomValidator>
                    <asp:RequiredFieldValidator ID="reqvDupEUID" runat="server" ControlToValidate="txtDupEmissionsUnitID"
                        CssClass="validator" ErrorMessage="* Emission unit ID is required." Width="100%"
                        ValidationGroup="DupEmissionsUnit"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="right" valign="top" width="210px">&nbsp;<asp:Label ID="Label6" runat="server" CssClass="label" Text="Description:"></asp:Label>
                </td>
                <td valign="top">
                    <asp:TextBox ID="txtDupEUDescription" runat="server" Font-Names="Arial" Font-Size="Small"
                        MaxLength="100" TextMode="MultiLine" Rows="4" Width="320px" CssClass="editable"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right" valign="top" width="210px">&nbsp;
                </td>
                <td valign="top">
                    <asp:RequiredFieldValidator ID="reqvDupEmissionUnitDesc" runat="server" ControlToValidate="txtDupEUDescription"
                        CssClass="validator" ErrorMessage="* Description is required."
                        Width="100%" ValidationGroup="DupEmissionsUnit"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">&nbsp;<asp:Label ID="Label7" runat="server" CssClass="label" Text="After saving you will be taken to the form to edit any changes."></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">&nbsp;&nbsp;
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <asp:Button ID="btnDupInsertEmissionUnit" runat="server" Text="Save" Width="90px" ValidationGroup="DupEmissionsUnit" />
                    &nbsp;&nbsp;
                    <asp:Button ID="btnCancelDupEmissionUnit" runat="server" Text="Cancel" CausesValidation="False" />
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">&nbsp;&nbsp;
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pnlAddUnitControlApproach" BackColor="#ffffff" runat="server" BorderColor="#333399"
        BorderStyle="Ridge" ScrollBars="Auto" Width="650px"
        Style="display: none;">
        <div class="modalpopup">
            <table border="0" cellpadding="2" cellspacing="1" width="100%">
                <tr>
                    <td align="center" colspan="2">
                        <strong><span style="color: #4169e1; font-size: 12pt; font-family: Verdana;">Add Unit
                            Control Approach</span></strong>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="width: 287px;">&nbsp;
                    </td>
                    <td>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td align="right" style="width: 287px;">
                        <asp:Label ID="lblCtrlApprEUID" runat="server" CssClass="label" Text="Emission Unit ID:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtCtrlApprEUID" runat="server" BorderStyle="None" Font-Names="Arial"
                            Font-Size="Small" MaxLength="6" ReadOnly="True" Width="150px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="right" valign="top" style="width: 287px;">
                        <asp:Label ID="lblCtrlApproachDesc" runat="server" CssClass="label" Text="Control Approach Description:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtCtrlApproachDesc" runat="server" MaxLength="200"
                            Width="320px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:RequiredFieldValidator ID="reqvNewEmissionUnitID0" runat="server" ControlToValidate="txtCtrlApproachDesc"
                            CssClass="validator" Display="Dynamic" ErrorMessage="* Control approach description is required."
                            ValidationGroup="AddUnitControlApproach" Width="100%"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td align="right" valign="top" style="width: 287px;">
                        <asp:Label ID="lblCtrlApprCapEffic" runat="server" Text="Control Approach Capture Efficiency (%):"
                            CssClass="label"></asp:Label>
                    </td>
                    <td valign="top">
                        <asp:TextBox ID="txtCtrlApprCapEffic" runat="server" Font-Names="Arial" Font-Size="Small"
                            MaxLength="4" Width="100px"></asp:TextBox>
                        <act:FilteredTextBoxExtender ID="filtxtCtrlApprCapEffic" runat="server" Enabled="True"
                            TargetControlID="txtCtrlApprCapEffic" FilterType="Custom, Numbers" ValidChars=".">
                        </act:FilteredTextBoxExtender>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" valign="top">
                        <asp:RequiredFieldValidator ID="reqvCtrlApprEffic" runat="server" ControlToValidate="txtCtrlApprCapEffic"
                            CssClass="validator" Display="Dynamic" ErrorMessage="* Control approach efficiency is required."
                            ValidationGroup="AddUnitControlApproach"></asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="rngvCtrlApprEffic" runat="server" ControlToValidate="txtCtrlApprCapEffic"
                            CssClass="validator" Display="Dynamic" MaximumValue="100" MinimumValue="5"
                            ErrorMessage="* The control approach efficiency must be between 5 and 100 percent."
                            Type="Double"></asp:RangeValidator>
                    </td>
                </tr>
                <tr>
                    <td align="right" valign="top" style="width: 287px;">
                        <asp:Label ID="lblCtrlApprCapEffect" runat="server" CssClass="label" Text="Control Approach Effectiveness (%):"></asp:Label>
                    </td>
                    <td valign="top">
                        <asp:TextBox ID="txtCtrlApprEffect" runat="server" Font-Names="Arial" Font-Size="Small"
                            MaxLength="4" Width="100px"></asp:TextBox>
                        <act:FilteredTextBoxExtender ID="filtxCtrlApprEffect" runat="server" Enabled="true"
                            TargetControlID="txtCtrlApprEffect" FilterType="Numbers, Custom" ValidChars=".">
                        </act:FilteredTextBoxExtender>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" valign="top">
                        <asp:RequiredFieldValidator ID="reqvCtrlApprEffect" runat="server" ControlToValidate="txtCtrlApprEffect"
                            CssClass="validator" Display="Dynamic" ErrorMessage="* Control approach effectiveness is required."
                            ValidationGroup="AddUnitControlApproach" Width="100%"></asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="rngvCtrlApprEffect" runat="server" ControlToValidate="txtCtrlApprEffect"
                            CssClass="validator" Display="Dynamic" MaximumValue="100" MinimumValue="1"
                            ErrorMessage="* Control approach effectiveness must be between 1 and 100 percent."
                            Type="Double"></asp:RangeValidator>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <asp:Label ID="Label5" runat="server" CssClass="label" Text="After saving you will enter more details on the form that appears."></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2">&nbsp;&nbsp;
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <asp:Button ID="btnInsertUnitApproachControl" runat="server" Text="Save" Width="90px"
                            ValidationGroup="AddUnitControlApproach" />
                        &nbsp;&nbsp;
                        <asp:Button ID="btnCancelUnitApproachControl" runat="server" Text="Cancel" CausesValidation="False" />
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2">&nbsp;&nbsp;
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlAddProcess" BackColor="#ffffff" runat="server" BorderColor="#333399"
        BorderStyle="Ridge" Width="600px" Style="display: none;">
        <div class="modalpopup">
            <table border="0" cellpadding="2" cellspacing="1" width="100%">
                <tr>
                    <td align="center" colspan="2">
                        <strong><span style="color: #4169e1; font-size: 12pt; font-family: Verdana;">Add Process</span></strong>
                    </td>
                </tr>
                <tr>
                    <td align="right" width="205px">
                        <asp:Label ID="lblNewProcessIDEmissionsUnit" runat="server" CssClass="label" Text="Emission Unit ID:"></asp:Label>
                    </td>
                    <td width="165px">
                        <asp:TextBox ID="txtNewProcessIDEmissionsUnit" runat="server" BorderStyle="None"
                            Font-Names="Arial" Font-Size="Small" MaxLength="6" ReadOnly="True" Width="150px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="right" valign="top" width="205px">
                        <asp:Label ID="lblNewProcessID" runat="server" Text="Process ID:" CssClass="label"></asp:Label>
                    </td>
                    <td valign="top">
                        <asp:TextBox ID="txtNewProcessID" runat="server" Font-Names="Arial" Font-Size="Small"
                            MaxLength="6" Width="150px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="right" valign="top" width="205px"></td>
                    <td valign="top">
                        <asp:RequiredFieldValidator ID="reqvNewProcessID" runat="server" ControlToValidate="txtNewProcessID"
                            CssClass="validator" Display="Dynamic" ErrorMessage="* Process ID is required."
                            ValidationGroup="AddProcess" Width="100%"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="cusvNewProcessID" runat="server" ControlToValidate="txtNewProcessID"
                            OnServerValidate="ProcessIDCheck" ErrorMessage="* Process ID already used for this unit. Enter another."
                            Font-Names="Arial" Font-Size="Small" Display="Dynamic" ValidationGroup="AddProcess"></asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                    <td align="right" valign="top" width="205px">
                        <asp:Label ID="lblReleasePointID" runat="server" Text="Release Point ID:" CssClass="label"></asp:Label></td>
                    <td valign="top">
                        <asp:DropDownList ID="ddlReleasePointID" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td align="right" valign="top" width="205px"></td>
                    <td valign="top">
                        <asp:RequiredFieldValidator ID="reqvReleasePointID" runat="server"
                            ControlToValidate="ddlReleasePointID" CssClass="validator"
                            Display="Dynamic" ErrorMessage="* Release point ID is required."
                            InitialValue="--Select Release Point--" ValidationGroup="AddProcess"
                            Width="100%"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td align="right" valign="top" width="205px">&nbsp;<asp:Label ID="Label2" runat="server" Text="Process Description:"
                        CssClass="label"></asp:Label>
                    </td>
                    <td valign="top">
                        <asp:TextBox ID="txtNewProcessDesc" runat="server" Font-Names="Arial" Font-Size="Small"
                            MaxLength="199" TextMode="MultiLine" Rows="4" Width="320px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="right" width="205px" valign="top">&nbsp;
                    </td>
                    <td valign="top">
                        <asp:RequiredFieldValidator ID="reqvNewProcessDesc" runat="server" ControlToValidate="txtNewProcessDesc"
                            CssClass="validator" Display="Dynamic" ErrorMessage="* Process description is required."
                            ValidationGroup="AddProcess" Width="100%"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <asp:Label ID="lblEUPopUpComment0" runat="server" CssClass="label" Text="After saving you will enter more details on the form that appears."></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2">&nbsp;&nbsp;
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <asp:Button ID="btnInsertNewProcess" runat="server" Text="Save" ValidationGroup="AddProcess"
                            Width="90px" />
                        &nbsp;&nbsp;
                        <asp:Button ID="btnCancelProcess" runat="server" CausesValidation="False" Text="Cancel" />
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2">&nbsp;&nbsp;
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
    <uc1:PreventRePost ID="PreventRePost1" runat="server" />
</asp:Content>