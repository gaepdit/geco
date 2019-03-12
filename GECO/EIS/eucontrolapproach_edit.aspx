<%@ Page Title="Emissions Unit Control Approach Edit - GECO Facility Inventory" Language="VB"
    MaintainScrollPositionOnPostback="true" MasterPageFile="eismaster.master"
    AutoEventWireup="false" Inherits="GECO.eis_eucontrolapproach_edit" Codebehind="eucontrolapproach_edit.aspx.vb" %>

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
        Edit Emission Unit Control Approach<asp:Button ID="btnSummary" runat="server" Text="Summary"
            CausesValidation="False" CssClass="summarybutton" UseSubmitBehavior="False" PostBackUrl="~/eis/emissionunit_summary.aspx" />
    </div>
    <div class="fieldwrapperseparator">
        <asp:Label ID="lblSeparator" class="styledseparator" runat="server" Text="Emission Unit Control Approach"></asp:Label>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div class="sepbuttons">
                    <asp:Button runat="server" ID="btnSave" Text="Save" ToolTip="" Font-Size="Small"
                        ValidationGroup="vgUnitCPEdit" />&nbsp;
                    <asp:Button runat="server" ID="btnCancel" Text="Return to Details" ToolTip="" Font-Size="Small"
                        UseSubmitBehavior="False" />
                </div>
                <asp:Label ID="lblMessage" runat="server" CssClass="labelMessage" Font-Bold="True"
                    Font-Names="Arial" Font-Size="Small" ForeColor="Red"></asp:Label>
                <br />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:ValidationSummary ID="sumvUnitCPEdit" runat="server" HeaderText="You received the following errors:"
            ValidationGroup="vgUnitCPEdit" Style="font-size: small"></asp:ValidationSummary>
        <div class="fieldwrapper">
            <asp:Label ID="lblEmissionUnitID" class="styled" runat="server" Text="Emission Unit ID:"></asp:Label>
            <asp:TextBox ID="txtEmissionUnitID" class="readonly" runat="server" Text="" ReadOnly="True"
                Width="100px"></asp:TextBox>
        </div>
        <div class="fieldwrapper">
            <asp:Label ID="lblEmissionUnitDesc" class="styled" runat="server" Text="Emission Unit Description:"></asp:Label>
            <asp:TextBox ID="txtEmissionUnitDesc" class="readonly" runat="server" Text="" ReadOnly="True"
                Width="300px"></asp:TextBox>
        </div>
        <div class="fieldwrapper">
            <asp:Label ID="lblControlApproachDescription" class="styled" runat="server" Text="Control Approach Description:"></asp:Label>
            <asp:TextBox ID="txtControlApproachDescription" runat="server" class="editable" Text=""
                MaxLength="200" Width="300px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="reqvControlApproachDescription" runat="server" ControlToValidate="txtControlApproachDescription"
                ErrorMessage="The control approach description is required." ValidationGroup="vgUnitCPEdit">*</asp:RequiredFieldValidator>
        </div>
        <div class="fieldwrapper">
            <asp:Label ID="lblPctCtrlApproachCapEffic" class="styled" runat="server" Text="Control Approach Capture Efficiency (%):"></asp:Label>
            <asp:TextBox ID="txtPctCtrlApproachCapEffic" runat="server" class="editable" Text=""
                Width="100px" MaxLength="4"></asp:TextBox>
            <asp:RequiredFieldValidator ID="reqvPctCtrlApproachCapEffic" runat="server" ControlToValidate="txtPctCtrlApproachCapEffic"
                Display="Dynamic" ErrorMessage="The Control Approach Capture Efficiency is required."
                ValidationGroup="vgUnitCPEdit">*</asp:RequiredFieldValidator>
            <act:FilteredTextBoxExtender ID="filtxtPctCtrlApproachCapEffic" runat="server" Enabled="True"
                TargetControlID="txtPctCtrlApproachCapEffic" FilterType="Custom, Numbers" ValidChars=".">
            </act:FilteredTextBoxExtender>
            <asp:RangeValidator ID="rngvPctCtrlApproachCapEffic" runat="server" ControlToValidate="txtPctCtrlApproachCapEffic"
                ErrorMessage="The Capture Efficiency must be between 1 and 100 percent."
                MaximumValue="100" MinimumValue="1" ValidationGroup="vgUnitCPEdit"
                Type="Double" Style="font-size: small">Must be between 1 and 100</asp:RangeValidator>
        </div>
        <div class="fieldwrapper">
            <asp:Label ID="lblPctCtrlApproachEffect" class="styled" runat="server" Text="Control Approach Effectiveness (%):"></asp:Label>
            <asp:TextBox ID="txtPctCtrlApproachEffect" runat="server" class="editable" Text=""
                Width="100px" MaxLength="4"></asp:TextBox>
            <asp:RequiredFieldValidator ID="reqvPctCtrlApproachEffect" runat="server" ControlToValidate="txtPctCtrlApproachEffect"
                Display="Dynamic" ErrorMessage="The Control Approach Effectiveness is required."
                ValidationGroup="vgUnitCPEdit">*</asp:RequiredFieldValidator>
            <act:FilteredTextBoxExtender ID="filtxtPctCtrlApproachEffect" runat="server" Enabled="True"
                TargetControlID="txtPctCtrlApproachEffect" FilterType="Custom, Numbers" ValidChars=".">
            </act:FilteredTextBoxExtender>
            <asp:RangeValidator ID="rngvPctCtrlApproachEffect" runat="server" ControlToValidate="txtPctCtrlApproachEffect"
                MaximumValue="100" MinimumValue="1" ErrorMessage="The Control Approach Effectiveness must be between 1 and 100 percent."
                ValidationGroup="vgUnitCPEdit" Type="Double" Style="font-size: small">Must be between 1 and 100</asp:RangeValidator>
        </div>
        <div class="fieldwrapper">
            <asp:Label ID="lblControlApproachComment" class="styled" runat="server" Text="Control Approach Comment:"></asp:Label>
            <div style="display: inline-block">
                <asp:TextBox ID="txtControlApproachComment" runat="server" class="editable" TextMode="MultiLine" onKeyUp="javascript:Count(this);"
                    Rows="4" Text=""></asp:TextBox>
                <div id="dvComment" style="font: bold"></div>
                <asp:RegularExpressionValidator ID="regexpName" runat="server"
                    ErrorMessage="Comment not to exceed 400 characters."
                    ControlToValidate="txtControlApproachComment"
                    ValidationExpression="^[\s\S]{0,400}$" />
            </div>
        </div>
        <div class="fieldwrapperseparator">
            <asp:Label ID="Label3" class="styledseparator" runat="server" Text="Emission Unit Control Measure"></asp:Label>
            <asp:Label ID="lblEUControlMeasureWarning" runat="server" Font-Size="Small" ForeColor="#CA0000"
                CssClass="labelwarningleft"></asp:Label>
        </div>
        <asp:ValidationSummary ID="sumvControlMeasure" ValidationGroup="vgControlMeasure"
            runat="server" Style="font-size: small" />
        <div class="gridview">
            <asp:GridView ID="gvwEUControlMeasure" runat="server" CellPadding="4" Font-Names="Verdana"
                Font-Size="Small" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False"
                DataKeyNames="FacilitySiteID,EmissionsUnitID,MeasureCode">
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:LinkButton ID="DeleteMeasure" runat="server" CausesValidation="False" CommandName="Delete"
                                OnClientClick="return confirm('Are you sure you want to delete this record?');"
                                Text="Delete" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="FacilitySiteID" Visible="false" />
                    <asp:BoundField DataField="EmissionsUnitID" Visible="false" />
                    <asp:BoundField DataField="MeasureCode" Visible="false" />
                    <asp:BoundField DataField="CDType" HeaderText="Control Device">
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:BoundField DataField="LastEISSubmitDate" HeaderText="Last EPA Submittal" DataFormatString="{0:d}">
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
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
            <asp:DropDownList ID="ddlControlMeasure" runat="server" class="">
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="reqvControlMeasure" ControlToValidate="ddlControlMeasure"
                InitialValue="--Select a Control Measure --" runat="server" ValidationGroup="vgControlMeasure"
                ErrorMessage="A control measure is required." Display="Dynamic"
                Style="font-size: small">*</asp:RequiredFieldValidator>
            &nbsp;
            <asp:Button ID="btnAddControlMeasure" runat="server" Text="Add" ToolTip="" Font-Size="Small"
                ValidationGroup="vgControlMeasure" UseSubmitBehavior="False" />
        </div>
        <div class="fieldwrapperseparator">
            <asp:Label ID="Label1" class="styledseparator" runat="server" Text="Emission Unit Control Pollutants"></asp:Label>
            <asp:Label ID="lblEUControlPollutantWarning" runat="server" Font-Size="Small" ForeColor="#CA0000"
                CssClass="labelwarningleft"></asp:Label>
        </div>
        <asp:ValidationSummary ID="sumvCMReductionEff" runat="server" ValidationGroup="vgCMReductionEff"
            HeaderText="You received the following errors:" Style="font-size: small" />
        <asp:ValidationSummary ID="sumvPollutantDGV" runat="server" ValidationGroup="vgPollutantDGV"
            HeaderText="You received the following errors:" Style="font-size: small" />
        <div class="gridview">
            <asp:GridView ID="gvwEUControlPollutant" runat="server" CellPadding="4" Font-Names="Verdana"
                Font-Size="Small" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False"
                CssClass="gridview" DataKeyNames="FacilitySiteID,EmissionsUnitID,PollutantCode">
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:CommandField ShowEditButton="True" DeleteText="" ValidationGroup="vgPollutantDGV">
                        <ItemStyle VerticalAlign="Middle" />
                    </asp:CommandField>
                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:LinkButton ID="DeletePollutant" runat="server" CausesValidation="False" CommandName="Delete"
                                OnClientClick="return confirm('Are you sure you want to delete this record?');"
                                Text="Delete" />
                        </ItemTemplate>
                        <ItemStyle VerticalAlign="Middle" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="FacilitySiteID" Visible="false" />
                    <asp:BoundField DataField="EmissionsUnitID" Visible="false" />
                    <asp:BoundField DataField="PollutantCode" Visible="False" />
                    <asp:BoundField DataField="PollutantType" HeaderText="Pollutant" SortExpression="strPollutantDesc"
                        ReadOnly="true">
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Reduction Efficiency (%)">
                        <ItemTemplate>
                            <asp:Label ID="lblMeasureEfficiency" runat="server" Text='<%# Eval("MeasureEfficiency")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtMeasureEfficiency" runat="server" MaxLength="7" Text='<%# Eval("MeasureEfficiency")%>'
                                ValidationGroup="vgPollutantDGV"></asp:TextBox>
                            <asp:Label ID="lblError" Text="" Visible="false" Style="color: red" runat="server"></asp:Label>
                            <act:FilteredTextBoxExtender ID="filtxtMeasureEfficiency" runat="server" Enabled="True"
                                TargetControlID="txtMeasureEfficiency" FilterType="Numbers,Custom" ValidChars=".">
                            </act:FilteredTextBoxExtender>
                            <asp:RegularExpressionValidator ID="RegexCMReductionEff" runat="server" ControlToValidate="txtMeasureEfficiency"
                                ErrorMessage="Efficiency can have at most three decimal places. " ValidationExpression="\d*(\.\d{0,3})?"
                                ValidationGroup="vgPollutantDGV">At most three decimal places allowed.</asp:RegularExpressionValidator><%-- Regex: https://regexr.com/4a34g --%>
                            <asp:RangeValidator ID="rngvMeasureEfficiency" runat="server" ValidationGroup="vgPollutantDGV"
                                ControlToValidate="txtMeasureEfficiency" Display="Dynamic" MaximumValue="99.999"
                                MinimumValue="5" ErrorMessage="The reduction efficiency must be between 5.0 and 99.999 percent."
                                Type="Double"></asp:RangeValidator>
                            <asp:RequiredFieldValidator ID="rqvTxtMeasureEfficiency" runat="server" ControlToValidate="txtMeasureEfficiency"
                                Display="Dynamic" Font-Size="small" ErrorMessage="Reduction efficiency is required" 
                                ValidationGroup="vgPollutantDGV"></asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="LastEISSubmitDate" ReadOnly="true" HeaderText="Last EPA Submittal">
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
        <br />
        <div class="fieldwrapper">
            <asp:Label ID="lblControlpollutants" CssClass="styled" runat="server" Text="Control Pollutants: "></asp:Label>
            <asp:DropDownList ID="ddlControlPollutants" runat="server" class="">
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="reqvddlControlPollutants" ControlToValidate="ddlControlPollutants"
                InitialValue="--Select a Pollutant --" runat="server" ValidationGroup="vgCMReductionEff"
                ErrorMessage="A pollutant is required." Display="Dynamic"
                Style="font-size: small">*</asp:RequiredFieldValidator>
        </div>
        <div class="fieldwrapper">
            <asp:Label ID="lblCMReductionEff" class="styled" runat="server" Text="Control Measure Reduction Efficiency(%):"></asp:Label>
            <asp:TextBox ID="txtCMReductionEff" runat="server" class="editable" Text="" Width="75px"
                MaxLength="7"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rqvCMReductionEff" runat="server" ControlToValidate="txtCMReductionEff"
                Display="Dynamic" ErrorMessage="The pollutant reduction efficiency is required."
                ValidationGroup="vgCMReductionEff">*</asp:RequiredFieldValidator>
            <act:FilteredTextBoxExtender ID="filtxtCMReductionEff" runat="server" Enabled="True"
                TargetControlID="txtCMReductionEff" FilterType="Numbers,Custom" ValidChars=".">
            </act:FilteredTextBoxExtender>
            <asp:RegularExpressionValidator ID="RegexCMReductionEff" runat="server" ControlToValidate="txtCMReductionEff"
                ErrorMessage="Control Measure Reduction Efficiency can have at most three decimal places. " ValidationExpression="\d*(\.\d{0,3})?"
                ValidationGroup="vgCMReductionEff">At most three decimal places allowed.</asp:RegularExpressionValidator>
            <asp:RangeValidator ID="rngvCMReductionEff" runat="server" ControlToValidate="txtCMReductionEff"
                ValidationGroup="vgCMReductionEff" ErrorMessage="The reduction efficiency must be between 5.0 and 99.999 percent."
                Display="Dynamic" MaximumValue="99.999" MinimumValue="5" Type="Double"
                Style="font-size: small">Must be between 5.0 and 99.999 percent.</asp:RangeValidator>
            &nbsp;
            <asp:Button ID="btnAddControlPollutant" runat="server" Text="Add" ToolTip="" Font-Size="Small"
                ValidationGroup="vgCMReductionEff" UseSubmitBehavior="False" />
        </div>
        <div class="fieldwrapperseparator">
            <asp:Label ID="lblSeparatorOnly" class="styledseparator" runat="server" Text="_"
                Font-Size="Small" BackColor="#ffffff" ForeColor="#ffffff"></asp:Label>
        </div>
        <div class="buttonwrapper">
            <asp:Button ID="btnCancel2" runat="server" Text="Return to Details" ToolTip="" Font-Size="Small"
                CausesValidation="False" Width="120px" UseSubmitBehavior="False" />&nbsp;
            <asp:Button ID="btnReturnSummary2" runat="server" Text="Summary" ToolTip="" Font-Size="Small"
                CausesValidation="False" PostBackUrl="~/eis/emissionunit_summary.aspx" />&nbsp;
            <asp:Button ID="btnDelete" runat="server" Text="Delete" ToolTip="" Font-Size="Small" />
            <act:ModalPopupExtender ID="mpeDelete" runat="server" BackgroundCssClass="modalProgressGreyBackground"
                DynamicServicePath="" Enabled="True" TargetControlID="btnDelete" PopupControlID="pnlDeleteUnitCA">
            </act:ModalPopupExtender>
            <asp:Panel ID="pnlDeleteUnitCA" BackColor="#ffffff" runat="server" BorderColor="#333399"
                BorderStyle="Ridge" ScrollBars="Auto" Width="450px" Style="display: none;">
                <table border="0" cellpadding="2" cellspacing="1" width="450px">
                    <tr>
                        <td align="center">
                            <strong><span style="color: #4169e1; font-size: 12pt; font-family: Verdana;">Delete
                                Control Approach</span></strong>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblDeleteUnitCA" runat="server" Text="Are you sure you want to delete the Emissions Unit Control Approach,
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
                            <asp:Button ID="btnDeleteDetails" runat="server" Text="Return to Details" Width="120px" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">&nbsp;&nbsp;
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </div>
    </div>
</asp:Content>
