<%@ Page Title="Reporting Period Details - GECO Emissions Inventory" Language="VB"
    MasterPageFile="eismaster.master" AutoEventWireup="false"
    Inherits="GECO.EIS_rp_details" Codebehind="rp_details.aspx.vb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
    <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></act:ToolkitScriptManager>
    <div class="pageheader">
        Emission Inventory Details
        <asp:Button ID="btnSummary1" runat="server" Text="Summary" CssClass="summarybutton"
            PostBackUrl="~/EIS/rp_summary.aspx" />
        <asp:Button ID="btnProcess1" runat="server" Text="Return to Process Details" CssClass="summarybutton"
            Width="175px" />
    </div>
    <div class="fieldwrapperseparator">
        <asp:Label ID="lblSeparatorOnly" class="styledseparator" runat="server" Text="_"
            Font-Size="Small" BackColor="#ffffff" ForeColor="#ffffff"></asp:Label>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblEIYear" class="styled" runat="server" Text="Reporting Period:"></asp:Label>
        <asp:TextBox ID="txtEISYear" class="readonly" runat="server" Text="" ReadOnly="True"
            Width="100px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblEmissionUnitID" class="styled" runat="server" Text="Emission Unit ID:"></asp:Label>
        <asp:TextBox ID="txtEmissionsUnitID" class="readonly" runat="server" Text="" ReadOnly="True"
            Width="100px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblEmissionUnitDescription" class="styled" runat="server" Text="Emission Unit Description:"></asp:Label>
        <asp:TextBox ID="txtUnitDescription" class="readonly" runat="server" Text="" ReadOnly="True"
            Width="300px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblProcessID" class="styled" runat="server" Text="Process ID:"></asp:Label>
        <asp:TextBox ID="txtProcessID" class="readonly" runat="server" Text="" ReadOnly="True"
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
    <div class="fieldwrapperseparator">
        <asp:Label ID="lblEmissionDetails" class="styledseparator" runat="server" Text="Emission Details"></asp:Label>
        <div class="sepbuttons">
            <asp:Button ID="btnAddPollutant" runat="server" Text="Add" ToolTip="" Font-Size="Small" />
            <act:ModalPopupExtender ID="btnAddPollutant_ModalPopupExtender" runat="server" BackgroundCssClass="modalProgressGreyBackground"
                CancelControlID="btnCancel" DynamicServicePath="" Enabled="True" OkControlID="btnCancel"
                PopupControlID="pnlAddPollutant" TargetControlID="btnAddPollutant">
            </act:ModalPopupExtender>
            <asp:Button ID="btnEditRPEmissions" runat="server" Text="Edit" ToolTip="" Font-Size="Small" />
        </div>
        <asp:Label ID="lblRPEmissions" runat="server" Font-Size="Small" ForeColor="#CA0000"
            CssClass="labelwarningleft"></asp:Label>
    </div>
    <div class="gridview">
        <asp:GridView ID="gvwPollutants" runat="server"
            CellPadding="4" Font-Names="Arial" Font-Size="Small" ForeColor="#333333" GridLines="None"
            AutoGenerateColumns="False" EnableModelValidation="True">
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#EFF3FB" />
            <Columns>
                <asp:BoundField DataField="EMISSIONSUNITID" HeaderText="EmissionsUnitID" Visible="False" />
                <asp:BoundField DataField="PROCESSID" HeaderText="ProcessID" Visible="False" />
                <asp:BoundField DataField="PollutantCode" HeaderText="PollutantCode" Visible="False" />
                <asp:HyperLinkField DataTextField="strPollutant" DataNavigateUrlFields="EmissionsUnitID,ProcessID,PollutantCode"
                    DataNavigateUrlFormatString="~/EIS/rp_emissions_edit.aspx?eu={0}&amp;ep={1}&amp;em={2}"
                    HeaderText="Pollutant" NavigateUrl="~/EIS/rp_emissions_edit.aspx">
                    <ItemStyle HorizontalAlign="Left" />
                </asp:HyperLinkField>
                <asp:BoundField DataField="strPollutant" HeaderText="Pollutant" Visible="False">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="RPTPeriodType" HeaderText="Pollutant Period">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Emissions">
                    <ItemStyle HorizontalAlign="Center" />
                    <ItemTemplate>
                        <%# Eval("FLTTOTALEMISSIONS") %> <%# Eval("PollutantUnit") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="lasteissubmitdate" HeaderText="Last EPA Submittal" SortExpression="lasteissubmitdate"
                    DataFormatString="{0:d}" NullDisplayText="Not Submitted">
                    <HeaderStyle HorizontalAlign="Center" />
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
    <asp:Label ID="lblSummerDayNote" runat="server" Font-Size="Small"></asp:Label>
    <br />
    <br />
    <div class="fieldwrapperseparator">
        <asp:Label ID="Label1" class="styledseparator" runat="server" Text="_" Font-Size="Small"
            BackColor="#ffffff" ForeColor="#ffffff"></asp:Label>
    </div>
    <div class="pageheader">
        Process Operating Details<asp:Button ID="btnEditRPDetails" runat="server" Text="Edit"
            CausesValidation="False" CssClass="summarybutton" Font-Size="Small" />
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblLastEISSubmit" CssClass="styled" runat="server" Text="Last Submitted to EPA on:"></asp:Label>
        <asp:TextBox ID="txtLastEISSubmit" CssClass="readonly" runat="server" Text="" ReadOnly="True"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblLastUpdated" CssClass="styled" runat="server" Text="Last Updated on:"></asp:Label>
        <asp:TextBox ID="txtLastUpdated" CssClass="readonly" runat="server" Text="" ReadOnly="True"></asp:TextBox>
    </div>
    <div class="fieldwrapperseparator">
        <asp:Label ID="lblProcessOperatingDetails" CssClass="styledseparator" runat="server"
            Text="Process Details"></asp:Label>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblCalcParamTypeCode" CssClass="styled" runat="server" Text="Calculation Parameter Type:"></asp:Label>
        <asp:TextBox ID="txtCalcParamType" runat="server" class="readonly" Text="" Width="100px"
            ReadOnly="True"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblCalcParamValue" class="styled" runat="server" Text="Actual Annual Throughput/Activity:"></asp:Label>
        <asp:TextBox ID="txtCalcParamValue" runat="server" class="readonly" Text="" Width="100px"
            ReadOnly="True"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblCalcParamUoM" CssClass="styled" runat="server" Text="Annual Throughput/Activity Units:"></asp:Label>
        <asp:TextBox ID="txtCalcParamUoM" runat="server" class="readonly" Text="" Width="250px"
            ReadOnly="True"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblMaterialCode" CssClass="styled" runat="server" Text="Material Processed or Fuel Used:"></asp:Label>
        <asp:TextBox ID="txtCalcMaterial" runat="server" class="readonly" Text="" Width="250px"
            ReadOnly="True"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblRPComment" CssClass="styled" runat="server" Text="Comment:"></asp:Label>
        <asp:TextBox ID="txtRPComment" runat="server" class="readonly" TextMode="MultiLine" Rows="4"
            Width="400px" ReadOnly="True"></asp:TextBox>
    </div>
    <div class="fieldwrapperseparator">
        <asp:Label ID="lblSeparator" class="styledseparator" runat="server" Text="Daily, Weekly &amp; Annual Information"></asp:Label>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblAvgHoursPerDay" class="styled" runat="server" Text="Average Hours Per Day:"></asp:Label>
        <asp:TextBox ID="txtAvgHoursPerDay" runat="server" class="readonly" Text="" Width="100px"
            MaxLength="3" ReadOnly="True"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblAvgDaysPerWeek" class="styled" runat="server" Text="Average Days Per Week:"></asp:Label>
        <asp:TextBox ID="txtAvgDaysPerWeek" runat="server" class="readonly" Text="" Width="100px"
            ReadOnly="True"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblAvgWeeksPerYear" class="styled" runat="server" Text="Average Weeks Per Year:"></asp:Label>
        <asp:TextBox ID="txtAvgWeeksPerYear" runat="server" class="readonly" Text="" Width="100px"
            ReadOnly="True"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblActualHoursPerYear" class="styled" runat="server" Text="Actual Hours Per Year:"></asp:Label>
        <asp:TextBox ID="txtActualHoursPerYear" runat="server" class="readonly" Text="" Width="100px"
            ReadOnly="True"></asp:TextBox>
    </div>
    <div class="fieldwrapperseparator">
        <asp:Label ID="lblSeasonalInfo" class="styledseparator" runat="server" Text="Seasonal Operation Percentages"></asp:Label>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblWinterPct" class="styled" runat="server" Text="Winter Percent (%):"></asp:Label>
        <asp:TextBox ID="txtWinterPct" runat="server" class="readonly" Text="" Width="75px"
            MaxLength="4" ReadOnly="True"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblSpringPct" class="styled" runat="server" Text="Spring Percent (%):"></asp:Label>
        <asp:TextBox ID="txtSpringPct" runat="server" class="readonly" Text="" Width="75px"
            MaxLength="4" ReadOnly="True"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblSummerPct" class="styled" runat="server" Text="Summer Percent (%):"></asp:Label>
        <asp:TextBox ID="txtSummerPct" runat="server" class="readonly" Text="" Width="75px"
            MaxLength="4" ReadOnly="True"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblFallPct" class="styled" runat="server" Text="Fall percent (%):"></asp:Label>
        <asp:TextBox ID="txtFallPct" runat="server" class="readonly" Text="" Width="75px"
            MaxLength="4" ReadOnly="True"></asp:TextBox>
    </div>
    <div class="fieldwrapperseparator">
        <asp:Label ID="lblFuelBurning" class="styledseparator" runat="server" Text="Fuel Burning Information"></asp:Label>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblFuelUsage" CssClass="styled" runat="server" Text="Is this process fuel burning?"></asp:Label>
        <asp:TextBox ID="txtFuelUsage" runat="server" class="readonly" Text="" Width="75px"
            ReadOnly="True"></asp:TextBox>
    </div>
    <asp:Panel ID="pnlFuelBurning" runat="server" Width="100%">
        <div class="fieldwrapper">
            <asp:Label ID="lblHeatContent" class="styled" runat="server" Text="Heat Content:"></asp:Label>
            <asp:TextBox ID="txtHeatContent" class="readonly" runat="server" ReadOnly="True"
                Width="100px"></asp:TextBox>
        </div>
        <div class="fieldwrapper">
            <asp:Label ID="lblHeatContentNumUoM" class="styled" runat="server" Text="Heat Content Units:"></asp:Label>
            <asp:Label ID="txtHeatContentNumUoM" runat="server" Text=""></asp:Label>
            <asp:Label ID="lblPer" runat="server" Text="/"></asp:Label>
            <asp:Label ID="txtHeatContentDenUoM" runat="server" Text=""></asp:Label>
        </div>
        <div class="fieldwrapper">
            <asp:Label ID="lblSulfurPct" class="styled" runat="server" Text="Sulfur Content (%):"></asp:Label>
            <asp:TextBox ID="txtSulfurPct" class="readonly" runat="server" ReadOnly="True"></asp:TextBox>
        </div>
        <div class="fieldwrapper">
            <asp:Label ID="lblAshPct" class="styled" runat="server" Text="Ash Content (%):"></asp:Label>
            <asp:TextBox ID="txtAshPct" class="readonly" runat="server" ReadOnly="True"></asp:TextBox>
        </div>
    </asp:Panel>
    <div class="fieldwrapperseparator">
        <asp:Label ID="lblSummary" class="styledseparator" runat="server" Text="lblSeparatorwithSaveandNoText"
            BackColor="#ffffff" ForeColor="#ffffff"></asp:Label>
    </div>
    <div class="buttonwrapper">
        <asp:Button ID="btnSummary2" runat="server" Text="Summary" ToolTip="" PostBackUrl="~/EIS/rp_summary.aspx" />
        <asp:Button ID="btnProcess2" runat="server" Text="Return to Process Details" CssClass="summarybutton"
            Width="175px" />
    </div>
    <asp:Panel ID="pnlAddPollutant" BackColor="#ffffff" runat="server" BorderColor="#333399"
        BorderStyle="Ridge" ScrollBars="Auto" Width="450px" Style="display: none;">
        <table border="0" cellpadding="2" cellspacing="1" width="100%">
            <tr>
                <td align="center" colspan="2">
                    <strong><span style="color: #4169e1; font-size: 12pt; font-family: Verdana;">Add Emission</span></strong>
                </td>
            </tr>
            <tr>
                <td align="center" width="30%" colspan="2">&nbsp;
                </td>
            </tr>
            <tr>
                <td align="right" width="30%" valign="top">&nbsp;<asp:Label ID="Label2" runat="server" CssClass="label" Text="Emission Unit ID:"></asp:Label>
                </td>
                <td valign="top">
                    <asp:TextBox ID="txtEmissionsUnitID_Add" runat="server" class="readonly" ReadOnly="true"
                        Width="100px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right" width="30%" valign="top">&nbsp;<asp:Label ID="lblNewProcessID" runat="server" CssClass="label" Text="Process ID:"></asp:Label>
                </td>
                <td valign="top">
                    <asp:TextBox ID="txtProcessID_Add" runat="server" class="readonly" ReadOnly="true"
                        Width="100px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right" valign="top" width="30%">&nbsp;
                </td>
                <td valign="top">&nbsp;
                </td>
            </tr>
            <tr>
                <td align="right" width="30%" valign="top">&nbsp;<asp:Label ID="lblPollutant" runat="server" CssClass="label" Text="Pollutant:"
                    BorderStyle="None"></asp:Label>
                </td>
                <td valign="top">
                    <asp:DropDownList ID="ddlPollutant" runat="server">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="reqvPollutant" ControlToValidate="ddlPollutant" InitialValue="--Select a Pollutant--"
                        runat="server" ErrorMessage="* A pollutant is required." Display="Dynamic" ValidationGroup="vgAddPollutant"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="right" valign="top" width="30%">&nbsp;
                </td>
            </tr>
            <tr>
                <td align="center" class="style4" colspan="2">&nbsp;<asp:Label ID="lbl" runat="server" CssClass="label" Text="After saving you will enter more details on the form that appears."></asp:Label>
                </td>
                <tr>
                    <td align="center" colspan="2">&nbsp;&nbsp;
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <asp:Button ID="btnSavePollutant" runat="server" Text="Save" ValidationGroup="vgAddPollutant"
                            Width="75px" />
                        &nbsp;&nbsp;
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" Width="75px" />
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2">&nbsp;&nbsp;
                    </td>
                </tr>
        </table>
    </asp:Panel>
</asp:Content>
