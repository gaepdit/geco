<%@ Page Title="Reporting Period Operating Details Edit - GECO Emissions Inventory "
    Language="VB" MasterPageFile="eismaster.master" AutoEventWireup="false" Inherits="GECO.EIS_rp_operscp_edit" Codebehind="rp_operscp_edit.aspx.vb" %>

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
    <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></act:ToolkitScriptManager>
    <div class="pageheader">
        Edit Process Operating Details
    </div>
    <div class="fieldwrapperseparator">
        <asp:Label ID="lblSeparatorNoText" class="styledseparator" runat="server" Text="Process"
            BackColor="#ffffff" ForeColor="#ffffff"></asp:Label>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div class="sepbuttons">
                    <asp:Label ID="lblMessageTop" runat="server" Font-Bold="True" Font-Names="Arial"
                        Font-Size="Small" ForeColor="Red"></asp:Label>
                    <asp:Button ID="btnSave1" runat="server" Text="Save" ToolTip="" Font-Size="Small"
                        ValidationGroup="vgRPDetailsEdit" UseSubmitBehavior="False" />&nbsp;&nbsp;<asp:Button
                            ID="btnSummary3" runat="server" Text="Summary" CausesValidation="False"
                            UseSubmitBehavior="False" PostBackUrl="~/EIS/rp_summary.aspx"
                            Font-Size="Small" />
                    &nbsp;
                    <asp:Button ID="btnCancel1" runat="server" Text="Return to Details" ToolTip="" Font-Size="Small"
                        CausesValidation="False" UseSubmitBehavior="False" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <asp:ValidationSummary ID="sumvFugitive" runat="server" ForeColor="Red" ValidationGroup="vgRPDetailsEdit"
        HeaderText="You received the following errors:"></asp:ValidationSummary>
    <div class="fieldwrapper">
        <asp:Label ID="lblEISYear" class="styled" runat="server" Text="Reporting Period:"></asp:Label>
        <asp:TextBox ID="txtEISYear" class="readonly" runat="server" Text="" ReadOnly="true"
            Width="100px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblEmissionUnitID" class="styled" runat="server" Text="Emission Unit ID:"></asp:Label>
        <asp:TextBox ID="txtEmissionsUnitID" class="readonly" runat="server" Text="" ReadOnly="true"
            Width="100px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblEmissionUnitDescription" class="styled" runat="server" Text="Emission Unit Description:"></asp:Label>
        <asp:TextBox ID="txtUnitDescription" class="readonly" runat="server" Text="" ReadOnly="true"
            Width="300px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblProcessID" class="styled" runat="server" Text="Process ID:"></asp:Label>
        <asp:TextBox ID="txtProcessID" class="readonly" runat="server" Text="" ReadOnly="true"
            Width="100px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblProcessDescription" class="styled" runat="server" Text="Process Description:"></asp:Label>
        <asp:TextBox ID="txtProcessDescription" class="readonly" runat="server" Text="" ReadOnly="True"
            Width="300px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblSourceClassCode" class="styled" runat="server" Text="Source Classification Code:"></asp:Label>
        <asp:TextBox ID="txtSourceClassCode" runat="server" Text="" class="readonly" ReadOnly="True"
            Width="100px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblSccDescLabel" class="styled" runat="server" Text="SCC Description:"></asp:Label>
        <asp:Label ID="lblSccDesc" runat="server" />
    </div>
    <div class="fieldwrapperseparator">
        <asp:Label ID="lblProcessOperatingDetails" CssClass="styledseparator" runat="server"
            Text="Process Operating Details"></asp:Label>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblCalcParamType" CssClass="styled" runat="server" Text="Calculation Parameter Type:"></asp:Label>
        <asp:DropDownList ID="ddlCalcParamType" runat="server" class="">
        </asp:DropDownList>
        <asp:RequiredFieldValidator ID="reqvCalcParamType" ControlToValidate="ddlCalcParamType"
            runat="server" InitialValue="-- Select Type --" ErrorMessage="The Calculation Parameter Type is required."
            Display="Dynamic" ValidationGroup="vgRPDetailsEdit">*</asp:RequiredFieldValidator>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblCalcParamValue" class="styled" runat="server" Text="Actual Annual Throughput/Activity:"></asp:Label>
        <asp:TextBox ID="txtCalcParamValue" runat="server" class="editable" Text=""
            Width="100px" MaxLength="11"></asp:TextBox>
        <act:FilteredTextBoxExtender ID="filtxtCalcParamValue" runat="server" Enabled="True"
            FilterType="Custom, Numbers" TargetControlID="txtCalcParamValue" ValidChars=".">
        </act:FilteredTextBoxExtender>
        <asp:RequiredFieldValidator ID="reqvCalcParamValue" runat="server" ControlToValidate="txtCalcParamValue"
            ValidationGroup="vgRPDetailsEdit" ErrorMessage="The Actual Annual Throughput is required.">*</asp:RequiredFieldValidator>
        <asp:RangeValidator ID="rngvCalcParamValue" runat="server" ControlToValidate="txtCalcParamValue"
            MinimumValue="0" MaximumValue="9999999999" Type="Double" ErrorMessage="The Actual Annual Throughput is outside the expected range of 0 to 9,999,999,999"
            ValidationGroup="vgRPDetailsEdit">Must be between 0 and 9,999,999,999</asp:RangeValidator>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblCalcParamUoM" CssClass="styled" runat="server" Text="Annual Throughput/Activity Units:"></asp:Label>
        <asp:DropDownList ID="ddlCalcParamUoM" runat="server" class="">
        </asp:DropDownList>
        <asp:RequiredFieldValidator ID="reqvCalcParamUoM" ControlToValidate="ddlCalcParamUoM"
            runat="server" InitialValue="-- Select Units --" Display="Dynamic" ErrorMessage="The Annual Throughput/Activity Units are required."
            ValidationGroup="vgRPDetailsEdit">*</asp:RequiredFieldValidator>
        <asp:CompareValidator ID="cmpCalcParamUom" ControlToValidate="ddlCalcParamUoM" runat="server" 
            Operator="Equal" Type="String" ValidationGroup="vgRPDetailsEdit">* Invalid units selected</asp:CompareValidator>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblMaterialCode" CssClass="styled" runat="server" Text="Material Processed or Fuel Used:"></asp:Label>
        <asp:DropDownList ID="ddlMaterialCode" runat="server" class="">
        </asp:DropDownList>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlMaterialCode"
            InitialValue="-- Select Material --" ErrorMessage="The Material Processed or Fuel Used is required."
            ValidationGroup="vgRPDetailsEdit">*</asp:RequiredFieldValidator>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblRPComment" class="styled" runat="server" Text="Comments:"></asp:Label>
        <div style="display: inline-block">
            <asp:TextBox ID="txtRPComment" runat="server" class="editable" TextMode="MultiLine" Rows="4" onKeyUp="javascript:Count(this);"
                Text="" Width="400px"></asp:TextBox>
            <act:TextBoxWatermarkExtender ID="txtRPComment_TextBoxWatermarkExtender" runat="server"
                Enabled="True" TargetControlID="txtRPComment" WatermarkText="OPTIONAL" WatermarkCssClass="watermarked">
            </act:TextBoxWatermarkExtender>
            <div id="dvComment" style="font: bold"></div>
            <asp:RegularExpressionValidator ID="regexpName" runat="server"
                ErrorMessage="Comment not to exceed 400 characters."
                ControlToValidate="txtRPComment"
                ValidationExpression="^[\s\S]{0,400}$" ValidationGroup="vgStack" />
        </div>
    </div>
    <div class="fieldwrapperseparator">
        <asp:Label ID="lblSeparator" class="styledseparator" runat="server" Text="Daily, Weekly &amp; Annual Information"></asp:Label>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblAvgHoursPerDay" class="styled" runat="server" Text="Average Hours Per Day:"></asp:Label>
        <asp:TextBox ID="txtAvgHoursPerDay" runat="server" class="editable" Text="" Width="100px"
            MaxLength="4" ReadOnly="false"></asp:TextBox>
        <asp:RequiredFieldValidator ID="reqvAvgHoursPerDay" runat="server" ControlToValidate="txtAvgHoursPerDay"
            ErrorMessage="The Average Hours Per Day is required." ValidationGroup="vgRPDetailsEdit">*</asp:RequiredFieldValidator>
        <act:FilteredTextBoxExtender ID="filtxtAvgHoursPerDay" runat="server" Enabled="True"
            FilterType="Custom, Numbers" TargetControlID="txtAvgHoursPerDay" ValidChars=".">
        </act:FilteredTextBoxExtender>
        <asp:RangeValidator ID="rngvAvgHoursPerDay" runat="server" ControlToValidate="txtAvgHoursPerDay"
            MinimumValue="0.1" MaximumValue="24" Type="Double" ErrorMessage="The average hours per day must be between 0.1 and 24 hours."
            ValidationGroup="vgRPDetailsEdit">Must be between 0.1 and 24</asp:RangeValidator>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblAvgDaysPerWeek" class="styled" runat="server" Text="Average Days Per Week:"></asp:Label>
        <asp:TextBox ID="txtAvgDaysPerWeek" runat="server" class="editable" Text="" Width="100px"
            ReadOnly="false" MaxLength="3"></asp:TextBox>
        <act:FilteredTextBoxExtender ID="filtxtAvgDaysPerWeek" runat="server" Enabled="True"
            FilterType="Custom, Numbers" TargetControlID="txtAvgDaysPerWeek" ValidChars=".">
        </act:FilteredTextBoxExtender>
        <asp:RequiredFieldValidator ID="reqvAvgDaysPerWeek" runat="server" ControlToValidate="txtAvgDaysPerWeek"
            ErrorMessage="The Average Days per Week is required." ValidationGroup="vgRPDetailsEdit">*</asp:RequiredFieldValidator>
        <asp:RangeValidator ID="rngvAvgDaysPerWeek" runat="server" ControlToValidate="txtAvgDaysPerWeek"
            MinimumValue="0.1" MaximumValue="7" Type="Double" ErrorMessage="The average days per week must be between 0.1 and 7 days."
            ValidationGroup="vgRPDetailsEdit">Must be between 0.1 and 7</asp:RangeValidator>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblAvgWeeksPerYear" class="styled" runat="server" Text="Average Weeks Per Year:"></asp:Label>
        <asp:TextBox ID="txtAvgWeeksPerYear" runat="server" class="editable" Text="" Width="100px"
            ReadOnly="false" MaxLength="2"></asp:TextBox>
        <act:FilteredTextBoxExtender ID="filtxtAvgWeeksPerYear" runat="server" Enabled="True"
            FilterType="Numbers" TargetControlID="txtAvgWeeksPerYear">
        </act:FilteredTextBoxExtender>
        <asp:RequiredFieldValidator ID="reqvAvgWeeksPerYear" runat="server" ControlToValidate="txtAvgWeeksPerYear"
            ErrorMessage="The Aveage Weeks per Year is required." ValidationGroup="vgRPDetailsEdit">*</asp:RequiredFieldValidator>
        <asp:RangeValidator ID="rngvAvgWeeksPerYear" runat="server" ControlToValidate="txtAvgWeeksPerYear"
            MinimumValue="1" MaximumValue="52" Type="Integer" ErrorMessage="The average weeks per year must be between 1 and 52 weeks."
            ValidationGroup="vgRPDetailsEdit">Must be between 1 and 52</asp:RangeValidator>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblActualHoursPerYear" class="styled" runat="server" Text="Actual Hours Per Year:"></asp:Label>
        <asp:TextBox ID="txtActualHoursPerYear" runat="server" class="editable" Text="" Width="100px"
            ReadOnly="false" MaxLength="4"></asp:TextBox>
        <act:FilteredTextBoxExtender ID="filxtxActualHoursPerYear" runat="server" Enabled="True"
            FilterType="Numbers" TargetControlID="txtActualHoursPerYear">
        </act:FilteredTextBoxExtender>
        <asp:RequiredFieldValidator ID="reqvActualHoursPerYear" runat="server" ControlToValidate="txtActualHoursPerYear"
            ErrorMessage="The Actual Hours per Year are required." ValidationGroup="vgRPDetailsEdit">*</asp:RequiredFieldValidator>
        <asp:RangeValidator ID="rngvActualHoursPerYear" runat="server" ControlToValidate="txtActualHoursPerYear"
            MinimumValue="1" MaximumValue="8784" Type="Integer" ErrorMessage="The actual hours per year must be between 1 and 8,784 hours"
            ValidationGroup="vgRPDetailsEdit">Must be between 1 and 8,784</asp:RangeValidator>
    </div>
    <div class="fieldwrapperseparator">
        <asp:Label ID="lblSeasonalInfo" class="styledseparator" runat="server" Text="Seasonal Operation Percentages"></asp:Label>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblWinterPct" class="styled" runat="server" Text="Winter Percent (%):"></asp:Label>
        <asp:TextBox ID="txtWinterPct" runat="server" class="editable" Text="" Width="75px"
            MaxLength="4" ReadOnly="false"
            ToolTip="Months: January, February ... December"></asp:TextBox>
        <act:FilteredTextBoxExtender ID="filtxtWinterPct" runat="server" Enabled="True" TargetControlID="txtWinterPct"
            FilterType="Custom, Numbers" ValidChars=".">
        </act:FilteredTextBoxExtender>
        <asp:RequiredFieldValidator ID="reqvWinterPct" runat="server" ControlToValidate="txtWinterPct"
            ErrorMessage="The Winter Percent is required." ValidationGroup="vgRPDetailsEdit">*</asp:RequiredFieldValidator>
        <asp:RangeValidator ID="rngvWinterPct" runat="server" ControlToValidate="txtWinterPct"
            MinimumValue="0" MaximumValue="100" Type="Double" ErrorMessage="The Winter Percent must be between 0% and 100%."
            ValidationGroup="vgRPDetailsEdit">Must be between 0 and 100</asp:RangeValidator>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblSpringPct" class="styled" runat="server" Text="Spring Percent (%):"></asp:Label>
        <asp:TextBox ID="txtSpringPct" runat="server" class="editable" Text="" Width="75px"
            MaxLength="4" ReadOnly="false" ToolTip="Months: March, April, and May"></asp:TextBox>
        <act:FilteredTextBoxExtender ID="filtxtSpringPct" runat="server" Enabled="True" TargetControlID="txtSpringPct"
            FilterType="Custom, Numbers" ValidChars=".">
        </act:FilteredTextBoxExtender>
        <asp:RequiredFieldValidator ID="reqvSpringPct" runat="server" ControlToValidate="txtSpringPct"
            ErrorMessage="The Spring Percent is required." ValidationGroup="vgRPDetailsEdit">*</asp:RequiredFieldValidator>
        <asp:RangeValidator ID="rngvSpringPct" runat="server" ControlToValidate="txtSpringPct"
            MinimumValue="0" MaximumValue="100" Type="Double" ErrorMessage="The Spring Percent must be between 0% and 100%."
            ValidationGroup="vgRPDetailsEdit">Must be between 0 and 100</asp:RangeValidator>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblSummerPct" class="styled" runat="server" Text="Summer Percent (%):"></asp:Label>
        <asp:TextBox ID="txtSummerPct" runat="server" class="editable" Text="" Width="75px"
            MaxLength="4" ReadOnly="false" ToolTip="Months: June, July, and August"></asp:TextBox>
        <act:FilteredTextBoxExtender ID="filtxtSummerPct" runat="server" Enabled="True" TargetControlID="txtSummerPct"
            FilterType="Custom, Numbers" ValidChars=".">
        </act:FilteredTextBoxExtender>
        <asp:RequiredFieldValidator ID="reqvSummerPct" runat="server" ControlToValidate="txtSummerPct"
            ErrorMessage="The Summer Percent is required." ValidationGroup="vgRPDetailsEdit">*</asp:RequiredFieldValidator>
        <asp:RangeValidator ID="rngvSummerPct" runat="server" ControlToValidate="txtSummerPct"
            MinimumValue="0" MaximumValue="100" Type="Double" ErrorMessage="The Summer Percent must be between 0% and 100%."
            ValidationGroup="vgRPDetailsEdit">Must be between 0 and 100</asp:RangeValidator>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblFallPct" class="styled" runat="server" Text="Fall Percent (%):"></asp:Label>
        <asp:TextBox ID="txtFallPct" runat="server" class="editable" Text="" Width="75px"
            MaxLength="4" ReadOnly="false" ToolTip="Months: September, Ocotber, and November"></asp:TextBox>
        <act:FilteredTextBoxExtender ID="filtxtFallPct" runat="server" Enabled="True" TargetControlID="txtFallPct"
            FilterType="Custom, Numbers" ValidChars=".">
        </act:FilteredTextBoxExtender>
        <asp:RequiredFieldValidator ID="reqvFallPct" runat="server" ControlToValidate="txtFallPct"
            ErrorMessage="The Fall Percent is required." ValidationGroup="vgRPDetailsEdit">*</asp:RequiredFieldValidator>
        <asp:RangeValidator ID="rngvFallPct" runat="server" ControlToValidate="txtFallPct"
            MinimumValue="0" MaximumValue="100" Type="Double" ErrorMessage="The Fall Percent must be between 0% and 100%."
            ValidationGroup="vgRPDetailsEdit">Must be between 0 and 100</asp:RangeValidator>
    </div>
    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
        <ContentTemplate>
            <div class="fieldwrapper">
                <asp:Label ID="lblTotalPct" class="styled" runat="server" Text="Total Seasonal Percent (%):"></asp:Label>
                <asp:TextBox ID="txtTotalPct" runat="server" class="editable" Text="" Width="75px"
                    MaxLength="4" ReadOnly="True" Font-Bold="True"></asp:TextBox>&nbsp
                <asp:Button ID="btnSumSeasonalPct" runat="server" Text="Calculate"
                    CausesValidation="False" />&nbsp
                <asp:Label ID="lblTotalSeasonValidate" runat="server" ForeColor="Red" Text="Must total 100%"></asp:Label>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="fieldwrapperseparator">
        <asp:Label ID="lblFuelBurning" class="styledseparator" runat="server" Text="Fuel Burning Information"></asp:Label>
    </div>
    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
        <ContentTemplate>
            <div class="fieldwrapper">
                <asp:Label ID="lblFuelUsage" CssClass="styled" runat="server" Text="Is this process fuel burning?"></asp:Label>
                <asp:DropDownList ID="ddlFuelBurning" runat="server" class="" AutoPostBack="true">
                </asp:DropDownList>
            </div>
            <asp:Panel ID="pnlFuelBurning" runat="server" Width="100%">
                <div class="fieldwrapper">
                    <asp:Label ID="lblHeatContent" class="styled" runat="server" Text="Heat Content:"></asp:Label>
                    <asp:TextBox ID="txtHeatContent" class="editable" runat="server" ReadOnly="false"
                        Width="100px" ValidationGroup="vgRPDetailsEdit" MaxLength="9"></asp:TextBox>
                    <act:FilteredTextBoxExtender ID="filtxtHeatContent"
                        runat="server" Enabled="True" TargetControlID="txtHeatContent" FilterType="Custom, Numbers" ValidChars=".">
                    </act:FilteredTextBoxExtender>
                    <asp:RequiredFieldValidator ID="reqvHeatContent" runat="server" ControlToValidate="txtHeatContent"
                        ErrorMessage="The Heat Content is required." ValidationGroup="vgRPDetailsEdit">*</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="rgxvHeatContent" runat="server" ControlToValidate="txtHeatContent"
                        ErrorMessage="Heat content must be between 0.01 and 99999.99 with max 2 digits after the decimal."
                        ValidationExpression="^[+]?[0-9]\d{0,4}(\.\d{1,2})?%?$"
                        ValidationGroup="vgRPDetailsEdit">*</asp:RegularExpressionValidator>
                    <asp:RangeValidator ID="rngvHeatContent" runat="server" Type="Double" ControlToValidate="txtHeatContent"
                        ErrorMessage="The Heat Content must be between 0.00 and 99,999.99" ValidationGroup="vgRPDetailsEdit"
                        MaximumValue="99999.99" MinimumValue="0.01">Must be between 0.01 and 99,999.99</asp:RangeValidator>
                </div>
                <div class="fieldwrapper">
                    <asp:Label ID="lblHeatContentNumUoM" class="styled" runat="server" Text="Heat Content Units:"></asp:Label>
                    Million BTUs / 
                    <asp:DropDownList ID="ddlHeatContentDenUoM" runat="server" class=""
                        ValidationGroup="vgRPDetailsEdit"
                        ToolTip="E6FT3S indicates million standard cubic feet">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="reqvHeatContentDenUoM" runat="server" ControlToValidate="ddlHeatContentDenUoM"
                        ErrorMessage="The Heat Content Units are required." InitialValue="-Select a Value-"
                        ValidationGroup="vgRPDetailsEdit">*</asp:RequiredFieldValidator>
                </div>
                <div style="text-align: center;">
                </div>
                <div class="fieldwrapper">
                    <asp:Label ID="lblSulfurPct" class="styled" runat="server" Text="Sulfur Content (%):"></asp:Label>
                    <asp:TextBox ID="txtSulfurPct" class="editable" runat="server" ReadOnly="false" Width="75px"
                        MaxLength="4" ValidationGroup="vgRPDetailsEdit"></asp:TextBox>
                    <act:FilteredTextBoxExtender ID="filtxtSulfurPct" runat="server" Enabled="True" FilterType="Custom, Numbers"
                        TargetControlID="txtSulfurPct" ValidChars=".">
                    </act:FilteredTextBoxExtender>
                    &nbsp;<asp:CheckBox ID="cbxSulfurNegligible" runat="server" AutoPostBack="true"
                        Checked="false" Text="Negligible (&lt; 0.01%)" />
                    &nbsp;<asp:RequiredFieldValidator ID="reqvSulfurPct" runat="server" ControlToValidate="txtSulfurPct"
                        ErrorMessage="The Sulfur Content is required." ValidationGroup="vgRPDetailsEdit">*</asp:RequiredFieldValidator>
                    <asp:RangeValidator ID="rngvSulfurPct" runat="server" Type="Double" ControlToValidate="txtSulfurPct"
                        ErrorMessage="Must be from 0.01% to 10%." MaximumValue="10"
                        MinimumValue="0.01" ValidationGroup="vgRPDetailsEdit">Must be from 0.01% to 10%</asp:RangeValidator>
                </div>
                <div class="fieldwrapper">
                    <asp:Label ID="lblAshPct" class="styled" runat="server" Text="Ash Content (%):"></asp:Label>
                    <asp:TextBox ID="txtAshPct" class="editable" runat="server" ReadOnly="false" Width="75px"
                        MaxLength="4" ValidationGroup="vgRPDetailsEdit"></asp:TextBox>
                    <act:FilteredTextBoxExtender ID="filtxtAshPct" runat="server" Enabled="True" FilterType="Custom, Numbers"
                        TargetControlID="txtAshPct" ValidChars=".">
                    </act:FilteredTextBoxExtender>
                    &nbsp;<asp:CheckBox ID="cbxAshNegligible" runat="server" AutoPostBack="true" Checked="false"
                        Text="Negligible (&lt; 0.01%)" />
                    &nbsp;<asp:RequiredFieldValidator ID="reqvAshPct" runat="server" ControlToValidate="txtAshPct"
                        ErrorMessage="The Ash Content is required." ValidationGroup="vgRPDetailsEdit">*</asp:RequiredFieldValidator>
                    <asp:RangeValidator ID="rngvAshPct" runat="server" Type="Double" ControlToValidate="txtAshPct"
                        ErrorMessage="Ash content must be from 0.01% to 20%." MaximumValue="20"
                        MinimumValue="0.01" ValidationGroup="vgRPDetailsEdit">Must be from 0.01% and 20%</asp:RangeValidator>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="fieldwrapperseparator">
        <asp:Label ID="lblSummary" class="styledseparator" runat="server" Text="lblSeparatorwithSaveandNoText"
            BackColor="#ffffff" ForeColor="#ffffff"></asp:Label>
    </div>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <div class="buttonwrapper">
                <asp:Button ID="btnSave2" runat="server" Text="Save" CausesValidation="True" ValidationGroup="vgRPDetailsEdit" />&nbsp;
                <asp:Button ID="btnCancel2" runat="server" Text="Return to Details" ToolTip="" Width="120px"
                    CausesValidation="False" />&nbsp;
                <asp:Button ID="btnSummary" runat="server" Text="Summary" ToolTip="" Font-Size="Small"
                    CausesValidation="False" UseSubmitBehavior="False" PostBackUrl="~/EIS/rp_summary.aspx" />&nbsp;
                <asp:Button ID="btnDelete" runat="server" Text="Remove"
                    CausesValidation="False" ToolTip="Remove from reporting period" />
                <act:ModalPopupExtender ID="mpeDelete" runat="server" DynamicServicePath="" Enabled="True"
                    TargetControlID="btnDelete" BackgroundCssClass="modalProgressGreyBackground"
                    PopupControlID="pnlConfirmDelete">
                </act:ModalPopupExtender>
                <br />
                <asp:Label ID="lblMessageBottom" runat="server" Font-Bold="True" Font-Names="Arial"
                    Font-Size="Small" ForeColor="Red"></asp:Label><br />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Panel ID="pnlConfirmDelete" runat="server" Width="650px" BackColor="White" BorderColor="Black"
        BorderStyle="Solid" Style="display: none;">
        <div class="confirmdelete">
            <table align="center" width="600px">
                <tr>
                    <td>&nbsp; &nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="font-family: Verdana; font-weight: bold; font-size: medium; color: #D20000">Remove Process From Process Reporting Period
                    </td>
                </tr>
                <tr>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblDeleteConfirm1" runat="server" Text="" CssClass="WideTextBox" BorderStyle="None"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblDeleteConfirm2" runat="server" Text="Note: This will remove the process from the current
                        reporting period. All reporting period information, operating period details, and pollutant information for
                        this emissions inventory year for the selected process will be deleted."></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="buttonwrapper">
                            <asp:Button ID="btnConfirmDelete" runat="server" CausesValidation="False" Text="Remove"
                                CssClass="buttondiv" UseSubmitBehavior="False" />
                            <asp:Button ID="btnCancelDelete" runat="server" Text="No" CssClass="buttondiv" CausesValidation="False"
                                UseSubmitBehavior="False" />
                            <asp:Button ID="btnSummary2" runat="server" Text="Summary" CssClass="buttondiv" CausesValidation="False"
                                UseSubmitBehavior="False" PostBackUrl="~/EIS/rp_summary.aspx" />
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
</asp:Content>
