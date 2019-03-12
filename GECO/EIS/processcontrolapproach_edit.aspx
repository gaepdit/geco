<%@ Page Title="Process Control Approach - GECO Facility Inventory" Language="VB"
    MasterPageFile="eismaster.master" AutoEventWireup="false"
    MaintainScrollPositionOnPostback="true" Inherits="GECO.eis_processcontrolapproach_edit" Codebehind="processcontrolapproach_edit.aspx.vb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
    <script type="text/javascript">
        function Count(text) {
            var maxlength = 400; //set your value here (or add a parm and pass it in)
            var object = document.getElementById(text.id)  //get your object
            var div = document.getElementById("dvComment");
            div.innerHTML = object.value.length + '/' + maxlength + ' character(s).';
            if (object.value.length > maxlength) {
                object.focus(); //set focus to prevent jumping
                object.value = text.value.substring(0, maxlength); //truncate the value
                object.scrollTop = object.scrollHeight; //scroll to the end to prevent jumping
                return false;
            }
            return true;
        }
    </script>
    <div class="pageheader">
        <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></act:ToolkitScriptManager>
        Edit Process Control Approach
    </div>
    <div class="fieldwrapperseparator">
        <asp:Label ID="lblSeparator" class="styledseparator" runat="server" Text="Process Control Approach"></asp:Label>
        <div class="sepbuttons">
            <asp:Label ID="lblMessageTop" runat="server" Font-Bold="True" Font-Names="Arial"
                Font-Size="Small" ForeColor="Red"></asp:Label>&nbsp;
            <asp:Button runat="server" ID="btnSave2" Text="Save" ToolTip=""
                Font-Size="Small" ValidationGroup="vgProcessControlAppEdit" />&nbsp;
            <asp:Button runat="server" ID="btnCancel" Text="Return to Details" ToolTip=""
                Font-Size="Small" UseSubmitBehavior="False" />
        </div>
    </div>
    <asp:ValidationSummary ID="sumvProcessCAEdit" runat="server" HeaderText="You received the following errors:"
        ValidationGroup="vgProcessControlAppEdit"></asp:ValidationSummary>
    <div class="fieldwrapper">
        <asp:Label ID="lblEmissionUnitID" class="styled" runat="server" Text="Emission Unit ID:"></asp:Label>
        <asp:TextBox ID="txtEmissionUnitID" runat="server" class="readonly" Text=""
            Width="100px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblProcessId" class="styled" runat="server" Text="Process ID:"></asp:Label>
        <asp:TextBox ID="txtProcessID" runat="server" class="readonly" Text=""
            Width="100px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblControlApproachDescription" class="styled" runat="server" Text="Control Approach Description:"></asp:Label>
        <asp:TextBox ID="txtControlApproachDescription" runat="server" MaxLength="200" class="editable" Text=""></asp:TextBox>
        <asp:RequiredFieldValidator ID="reqvControlApproachDesc" runat="server" ControlToValidate="txtControlApproachDescription"
            Display="Dynamic" ErrorMessage="The Control Approach Description is required."
            ValidationGroup="vgProcessControlAppEdit">*</asp:RequiredFieldValidator>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblPctCtrlApproachCapEffic" class="styled" runat="server" Text="Percent Control Approach Capture Efficiency:"></asp:Label>
        <asp:TextBox ID="txtPctCtrlApproachCapEffic" runat="server" class="editable" Text=""
            Width="100px" MaxLength="5"></asp:TextBox>
        <asp:RequiredFieldValidator ID="reqvPctCtrlApproachCapEffic" runat="server" ControlToValidate="txtPctCtrlApproachCapEffic"
            Display="Dynamic" ErrorMessage="The Control Approach Capture Efficiency is required."
            ValidationGroup="vgProcessControlAppEdit">*</asp:RequiredFieldValidator>
        <act:FilteredTextBoxExtender ID="filtxtPctCtrlApproachCapEffic"
            runat="server" Enabled="True" TargetControlID="txtPctCtrlApproachCapEffic" FilterType="Numbers, Custom" ValidChars=".">
        </act:FilteredTextBoxExtender>
        <asp:RegularExpressionValidator ID="rgxvCtrlApproachCapEffic" runat="server" ControlToValidate="txtPctCtrlApproachCapEffic"
            ErrorMessage="Control Approach Capture Efficiency can have at most one decimal place. " ValidationExpression="\d*\.?\d?"
            ValidationGroup="vgProcessControlAppEdit">At most one decimal place allowed. </asp:RegularExpressionValidator>
        <asp:RangeValidator ID="rngvPctCtrlApproachCapEffic" runat="server" ControlToValidate="txtPctCtrlApproachCapEffic"
            MinimumValue="1.0" MaximumValue="100.0" Type="Double" ValidationGroup="vgProcessControlAppEdit"
            ErrorMessage="The Percent Control Approach Capture Efficiency is outside the expected range of 1.0 to 100"
            Display="Dynamic">Must be between 1.0 and 100 percent.</asp:RangeValidator>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblPctCtrlApproachEffect" class="styled" runat="server" Text="Percent Control Approach Effectiveness:"></asp:Label>
        <asp:TextBox ID="txtPctCtrlApproachEffect" runat="server" class="editable" Text=""
            Width="100px" MaxLength="5"></asp:TextBox>
        <asp:RequiredFieldValidator ID="reqvPctCtrlApproachEffect" runat="server" ControlToValidate="txtPctCtrlApproachEffect"
            Display="Dynamic" ErrorMessage="The Control Approach Effectiveness is required."
            ValidationGroup="vgProcessControlAppEdit">*</asp:RequiredFieldValidator>
        <act:FilteredTextBoxExtender ID="filtxtPctCtrlApproachEffect"
            runat="server" Enabled="True" TargetControlID="txtPctCtrlApproachEffect" FilterType="Numbers, Custom" ValidChars=".">
        </act:FilteredTextBoxExtender>
        <asp:RegularExpressionValidator ID="RegExCtrlApproachEffect" runat="server" ControlToValidate="txtPctCtrlApproachEffect"
            ErrorMessage="Control Approach Effectiveness can have at most one decimal place. " ValidationExpression="\d*\.?\d?"
            ValidationGroup="vgProcessControlAppEdit">At most one decimal place allowed. </asp:RegularExpressionValidator>
        <asp:RangeValidator ID="rngvPctCtrlApproachEffect" runat="server" ControlToValidate="txtPctCtrlApproachEffect"
            MinimumValue="1.0" MaximumValue="100.0" Type="Double" ValidationGroup="vgProcessControlAppEdit"
            ErrorMessage="The Percent Control Approach Effectiveness is outside the expected range of 1.0 to 100."
            Display="Dynamic">Must be between 1.0 and 100 percent.</asp:RangeValidator>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblControlApproachComment" class="styled" runat="server" Text="Control Approach Comment:"></asp:Label>
        <div style="display: inline-block">
            <asp:TextBox ID="txtControlApproachComment" runat="server" Rows="4" onKeyUp="javascript:Count(this);" class="editable" TextMode="MultiLine"
                Text="" Width="400px"></asp:TextBox>
            <asp:RegularExpressionValidator ID="regexpControlApproachComment" runat="server"
                ErrorMessage="Comment not to exceed 400 characters."
                ControlToValidate="txtControlApproachComment"
                ValidationExpression="^[\s\S]{0,400}$" ValidationGroup="vgProcessControlAppEdit" />
            <div id="dvComment" style="font: bold"></div>
        </div>
    </div>
    <div class="fieldwrapperseparator">
        <asp:Label ID="Label3" class="styledseparator" runat="server" Text="Process Control Measure"></asp:Label>
        <asp:Label ID="lblProcessControlMeasureWarning" runat="server" Font-Size="Small"
            ForeColor="#CA0000" CssClass="labelwarningleft"></asp:Label>
    </div>
    <asp:ValidationSummary ID="sumvControlMeasure" ValidationGroup="vgControlMeasure" runat="server"
        HeaderText="You received the following errors:" />
    <div class="gridview">
        <asp:GridView ID="gvwProcessControlMeasure" runat="server" CellPadding="4" Font-Names="Verdana"
            Font-Size="Small" ForeColor="#333333" GridLines="None"
            AutoGenerateColumns="False"
            DataKeyNames="FacilitySiteID,EmissionsUnitID,ProcessID,MeasureCode">
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#EFF3FB" />
            <Columns>
                <asp:TemplateField ShowHeader="False">
                    <ItemTemplate>
                        <asp:LinkButton ID="DeleteMeasure" runat="server" CausesValidation="False" CommandName="Delete"
                            OnClientClick="return confirm('Are you sure you want to delete Control Measure?');"
                            Text="Delete" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="FacilitySiteID" Visible="false" />
                <asp:BoundField DataField="EmissionsUnitID" Visible="false" />
                <asp:BoundField DataField="ProcessID" Visible="false" />
                <asp:BoundField DataField="MeasureCode" Visible="False" />
                <asp:BoundField DataField="CDType" HeaderText="Control Device">
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                </asp:BoundField>
                <asp:BoundField DataField="LastEISSubmitDate" HeaderText="Last EPA Submittal"
                    DataFormatString="{0:d}">
                    <ItemStyle HorizontalAlign="center" VerticalAlign="Top" />
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
    <div class="fieldwrapper">
        <asp:Label ID="lblddlcontrolmeasure" CssClass="styled" runat="server" Text="Control Measure: "></asp:Label>
        <asp:DropDownList ID="ddlControlMeasure" runat="server" class=""
            ValidationGroup="vgControlMeasure">
        </asp:DropDownList>
        <asp:RequiredFieldValidator ID="reqvControlMeasure" ControlToValidate="ddlControlMeasure"
            InitialValue="--Select a Control Measure --" runat="server" ValidationGroup="vgControlMeasure"
            ErrorMessage="A control measure is required." Display="Dynamic">*</asp:RequiredFieldValidator>
        <asp:Button ID="btnAddControlMeasure" runat="server" Text="Add" ToolTip="" Font-Size="Small"
            ValidationGroup="vgControlMeasure" />
    </div>
    <div class="fieldwrapperseparator">
        <asp:Label ID="Label1" class="styledseparator" runat="server" Text="Process Control Pollutant"></asp:Label>
        <asp:Label ID="lblProcessControlPollutantWarning" runat="server" Font-Size="Small"
            ForeColor="#CA0000" CssClass="labelwarningleft"></asp:Label>
    </div>
    <asp:ValidationSummary ID="sumvPollutantGVW" runat="server" ValidationGroup="vgPollutantGVW"
        HeaderText="You received the following errors:" />
    <br />
    <asp:ValidationSummary ID="sumvPollutantDetails" runat="server" ValidationGroup="vgPollutantDetails"
        HeaderText="You received the following errors:" />

    <div class="gridview">
        <asp:GridView ID="gvwProcessCtrlPollutant" runat="server" CellPadding="4" Font-Names="Verdana"
            Font-Size="Small" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False"
            CssClass="gridview" DataKeyNames="FacilitySiteID,EmissionsUnitID,ProcessID,PollutantCode">
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#EFF3FB" />
            <Columns>
                <asp:CommandField ShowEditButton="True" ValidationGroup="vgPollutantGVW" />
                <asp:TemplateField ShowHeader="False">
                    <ItemTemplate>
                        <asp:LinkButton ID="DeletePollutant" runat="server" CausesValidation="False" CommandName="Delete"
                            OnClientClick="return confirm('Are you sure you want to delete this Pollutant?');"
                            Text="Delete" />
                    </ItemTemplate>
                    <ItemStyle VerticalAlign="Middle" />
                </asp:TemplateField>
                <asp:BoundField DataField="FacilitySiteID" Visible="false" />
                <asp:BoundField DataField="EmissionsUnitID" Visible="false" />
                <asp:BoundField DataField="ProcessID" Visible="false" />
                <asp:BoundField DataField="PollutantCode" Visible="False" />
                <asp:BoundField DataField="PollutantType" HeaderText="Pollutant"
                    ReadOnly="true">
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="Reduction Efficiency (%)">
                    <ItemTemplate>
                        <asp:Label ID="lblReductionEfficiency" runat="server" Text='<%# Eval("ReductionEfficiency")%>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtReductionEfficiency" MaxLength="7" runat="server"
                            Text='<%# Eval("ReductionEfficiency")%>'></asp:TextBox>
                        <asp:Label ID="lblError" Text="" Visible="false" Style="color: red" runat="server"></asp:Label>
                        <asp:RequiredFieldValidator
                            ID="reqvReductionEfficiency" runat="server" ValidationGroup="vgPollutantGVW"
                            ControlToValidate="txtReductionEfficiency" Display="Dynamic"
                            ErrorMessage="The reduction efficiency is required."></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegexReductionEfficiency" runat="server" ControlToValidate="txtReductionEfficiency"
                            ErrorMessage="Control Approach Reduction Efficiency can have at most three decimal places. " ValidationExpression="\d*(\.\d{0,3})?"
                            ValidationGroup="vgCMReductionEff"></asp:RegularExpressionValidator><%-- Regex: https://regexr.com/4a34g --%>
                        <asp:RangeValidator ID="rngvReductionEfficiency" runat="server" ValidationGroup="vgPollutantGVW" Type="Double"
                            ControlToValidate="txtReductionEfficiency" Display="Dynamic" MaximumValue="99.999" MinimumValue="5.0"
                            ErrorMessage="The reduction efficiency must be between 5.0 and 99.999 percent."></asp:RangeValidator>
                    </EditItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:TemplateField>
                <asp:BoundField DataField="LastEISSubmitDate" ReadOnly="true"
                    HeaderText="Last EPA Submittal" DataFormatString="{0:d}">
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="CalculatedReduction" HeaderText="Calculated Overall<br> Pollutant Reduction (%)" ReadOnly="True" SortExpression="CalculatedReduction" HtmlEncode="false" />
            </Columns>
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#BCD2EE" />
            <AlternatingRowStyle BackColor="White" />
        </asp:GridView>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblControlpollutants" CssClass="styled" runat="server" Text="Control Pollutants: "></asp:Label>
        <asp:DropDownList ID="ddlControlPollutants" runat="server" class=""
            ValidationGroup="vgCMReductionEff">
        </asp:DropDownList>
        <asp:RequiredFieldValidator ID="reqvddlControlPollutants" ControlToValidate="ddlControlPollutants"
            InitialValue="--Select a Pollutant --" runat="server" ValidationGroup="vgPollutantDetails"
            ErrorMessage="A pollutant is required." Display="Dynamic">*</asp:RequiredFieldValidator>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblCMReductionEff" class="styled" runat="server" Text="Control Approach Reduction Efficiency(%):"></asp:Label>
        <asp:TextBox ID="txtCMReductionEff" runat="server" class="editable" Text=""
            Width="100px" MaxLength="7"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rqvCMReductionEff" runat="server" ControlToValidate="txtCMReductionEff"
            Display="Dynamic" ErrorMessage="The pollutant reduction efficiency is required."
            ValidationGroup="vgPollutantDetails"></asp:RequiredFieldValidator>
        <act:FilteredTextBoxExtender ID="filtxtCMReductionEff" runat="server" Enabled="True"
            TargetControlID="txtCMReductionEff" FilterType="Numbers, Custom" ValidChars=".">
        </act:FilteredTextBoxExtender>
        <asp:RegularExpressionValidator ID="RegexCMReductionEff" runat="server" ControlToValidate="txtCMReductionEff"
            ErrorMessage="Control Approach Reduction Efficiency can have at most three decimal places. " ValidationExpression="\d*(\.\d{0,3})?"
            ValidationGroup="vgCMReductionEff">At most three decimal places allowed.</asp:RegularExpressionValidator>
        <asp:RangeValidator ID="rngvCMReductionEff" runat="server" ControlToValidate="txtCMReductionEff"
            ValidationGroup="vgPollutantDetails" Display="Dynamic"
            MaximumValue="99.999" MinimumValue="5.0"
            ErrorMessage="The reduction efficiency must be between 5.0 and 99.999 percent"
            Type="Double">Must be between 5.0 and 99.999 percent</asp:RangeValidator>
        <asp:Button ID="btnAddControlPollutant" runat="server" Text="Add" ToolTip=""
            Font-Size="Small" ValidationGroup="vgPollutantDetails" />
    </div>
    <div class="fieldwrapperseparator">
        <asp:Label ID="lblSeparatorOnly" class="styledseparator" runat="server" Text="_"
            Font-Size="Small" BackColor="#ffffff" ForeColor="#ffffff"></asp:Label>
    </div>
    <div class="buttonwrapper">
        <asp:Button ID="btnCancel2" runat="server" Text="Return to Details" ToolTip="" Font-Size="Small"
            CausesValidation="False" Width="120px" UseSubmitBehavior="False" />&nbsp;
        <asp:Button ID="btnReturnSummary2" runat="server" Text="Summary" ToolTip="" Font-Size="Small"
            CausesValidation="False" PostBackUrl="~/eis/process_summary.aspx"
            UseSubmitBehavior="False" />&nbsp;
        <asp:Button ID="btnDelete" runat="server" Text="Delete" ToolTip="" Font-Size="Small" />
        <act:ModalPopupExtender ID="mpeDelete" runat="server"
            BackgroundCssClass="modalProgressGreyBackground" DynamicServicePath="" Enabled="True"
            TargetControlID="btnDelete" PopupControlID="pnlDeleteProcessCA">
        </act:ModalPopupExtender>
        <asp:Panel ID="pnlDeleteProcessCA" BackColor="#ffffff" runat="server" BorderColor="#333399"
            BorderStyle="Ridge" ScrollBars="Auto" Width="450px" Style="display: none;">
            <table border="0" cellpadding="2" cellspacing="1" width="450px">
                <tr>
                    <td align="center">
                        <strong><span style="color: #4169e1; font-size: 12pt; font-family: Verdana;">Delete Control Approach</span></strong></td>
                </tr>
                <tr>
                    <td>&nbsp
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblDeleteProcessCA" runat="server"
                            Text="Are you sure you want to delete the Process Control Approach,
                            all control measures, and all control pollutants?"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button ID="btnDeleteOK" runat="server" Text="Delete" Width="60px" />
                        <asp:Button ID="btnDeleteCancel" runat="server" Text="No" Width="60px" />
                        <asp:Button ID="btnDeleteDetails" runat="server" Text="Return to Details"
                            Width="120px" />
                    </td>
                </tr>
                <tr>
                    <td align="center">&nbsp;&nbsp;
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
</asp:Content>
