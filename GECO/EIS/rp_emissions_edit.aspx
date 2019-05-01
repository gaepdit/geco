<%@ Page Title="Reporting Period Emissions - GECO Emissions Inventory" Language="VB"
    MasterPageFile="eismaster.master" AutoEventWireup="false"
    Inherits="GECO.EIS_rp_emissions_edit" Codebehind="rp_emissions_edit.aspx.vb" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
    <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></act:ToolkitScriptManager>
    <div class="fieldwrapperseparator">
        <asp:Label ID="LblSeparatorEmissionUnit" class="styledseparator" runat="server" Text="Edit Emissions Details"
            Font-Names="Verdana" Font-Size="X-Large"></asp:Label>
        <div class="sepbuttons">
            <asp:Button ID="btnDetails1" runat="server" Text="Return to Details" Font-Size="Small"
                CausesValidation="False" UseSubmitBehavior="False" />
        </div>
        <asp:Label ID="lblSaveMessage1" runat="server" Font-Bold="True" Font-Names="Arial"
            Font-Size="Small" ForeColor="Red" CssClass="labelMessage"></asp:Label>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblEmissionUnitID" class="styled" runat="server" Text="Emission Unit ID:"></asp:Label>
        <asp:TextBox ID="txtEmissionUnitID" class="readonly" runat="server" Text="" ReadOnly="True"
            Width="300px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblEmissionUnitDescription" class="styled" runat="server" Text="Emission Unit Description:"></asp:Label>
        <asp:TextBox ID="txtEmissionUnitDescription" class="readonly" runat="server" Text=""
            ReadOnly="True" Width="300px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblProcessID" class="styled" runat="server" Text="Process ID:"></asp:Label>
        <asp:TextBox ID="txtProcessID" class="readonly" runat="server" Text="" ReadOnly="True"
            Width="300px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblProcessDescription" class="styled" runat="server" Text="Process Description:"></asp:Label>
        <asp:TextBox ID="txtProcessDescription" class="readonly" runat="server" Text="" ReadOnly="True"
            Width="300px"></asp:TextBox>
    </div>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <h3>Pollutants</h3>
            <div class="gridview">
                <asp:GridView ID="gvwPollutants" runat="server"
                    DataKeyNames="EmissionsUnitID,ProcessID,PollutantCode" CellPadding="4" Font-Names="Arial"
                    Font-Size="Small" ForeColor="#333333" GridLines="None"
                    AutoGenerateColumns="False" EnableModelValidation="True">
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#EFF3FB" />
                    <Columns>
                        <asp:ButtonField DataTextField="STRPOLLUTANTDESCRIPTION" HeaderText="Pollutant"
                            Text="Pollutant" ItemStyle-ForeColor="#2222aa">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:ButtonField>
                        <asp:BoundField DataField="PollutantCode" HeaderText="Code" />
                        <asp:BoundField DataField="RPTPeriodType" HeaderText="Emissions Period" />
                        <asp:TemplateField HeaderText="Emissions">
                            <ItemTemplate>
                                <%# If(Eval("CURR_FLTTOTALEMISSIONS").ToString = "", "", Eval("CURR_FLTTOTALEMISSIONS") & " " & Eval("PollutantUnit")) %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                Most Recent<br />
                                Previous Year
                            </HeaderTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate><%# Eval("PREV_INTINVENTORYYEAR") %></ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                Previous<br />
                                Emissions
                            </HeaderTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <%# If(Eval("PREV_FLTTOTALEMISSIONS").ToString = "", "", Eval("PREV_FLTTOTALEMISSIONS") & " " & Eval("PollutantUnit")) %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>>20% Change</HeaderTemplate>
                            <ItemTemplate>
                                <%# If(Eval("EmissionsChangeGreaterThan20Percent"), "<span class='highlight-normal'>Yes**</span>", "No") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#2461BF" />
                    <AlternatingRowStyle BackColor="White" />
                </asp:GridView>
            </div>
            <br />
            <asp:Label ID="lblSummerDayNote" runat="server" Font-Size="Small">
                * Summer Day emissions = emissions on an average summer day May 1 through Sep 30. Units are in tons per day (TPD).<br /><br />
            </asp:Label>
            <asp:Label ID="lbl20PercentNote" runat="server" Font-Size="Small" CssClass="highlight-normal">
                ** If total emissions have increased or decreased by more than 20 percent over previously reported emissions, 
                then supporting documentation must be submitted by email to 
                <a href="mailto:GeorgiaAirProtectionBranch@dnr.ga.gov">GeorgiaAirProtectionBranch@dnr.ga.gov</a>.
            </asp:Label><br />
            <br />
            <asp:Label ID="lblNoPollutantData" runat="server" BackColor="White" Font-Names="Verdana"
                Font-Size="Small" Font-Bold="True" ForeColor="Red" Width="560px">
                Note: The only remaining pollutant for this process cannot be deleted.
                Go to the Operating Details Edit page to delete the operating 
                details. Deleting that information will delete the remaining 
                pollutant that appears on this page.<br /><br />
            </asp:Label>
            <asp:ValidationSummary ID="sumvEmissions" runat="server" Font-Names="Arial" Font-Size="Small"
                HeaderText="The following items need to be corrected. See items marked with *"
                ValidationGroup="vgEmissions" />
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="fieldwrapper">
                        <asp:Label ID="lblPollutant" CssClass="styled" runat="server" Text="Pollutant:"></asp:Label>
                        <asp:DropDownList ID="ddlPollutant" runat="server" class="" AutoPostBack="True">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="reqvPollutant" runat="server" ControlToValidate="ddlPollutant"
                            Display="Dynamic" ErrorMessage="Select a pollutant" InitialValue="--Select Pollutant--"
                            ValidationGroup="vgEmissions">*</asp:RequiredFieldValidator>
                    </div>
                    <div class="fieldwrapper">
                        <asp:Label ID="lblID1" class="styled" runat="server" Text="Emission Calculation Method:"></asp:Label>
                        <asp:DropDownList ID="ddlEMCalcMethod" runat="server" class="" AutoPostBack="True"></asp:DropDownList>
                        <asp:RequiredFieldValidator ID="reqvPollutant0" runat="server" ControlToValidate="ddlEMCalcMethod"
                            Display="Dynamic" ErrorMessage="Select emission calculation method" InitialValue="0"
                            ValidationGroup="vgEmissions">*</asp:RequiredFieldValidator>
                    </div>
                    <div class="fieldwrapper">
                        <asp:Label ID="lblEmissionFactor" class="styled" runat="server" Text="Emission Factor:"></asp:Label>
                        <asp:TextBox ID="txtEmissionFactor" runat="server" class="editable" Text="" Width="100px"
                            MaxLength="5" ToolTip="Minimum value is .0001 and maximum in 10000"></asp:TextBox>
                        <act:FilteredTextBoxExtender ID="txtEmissionFactor_FilteredTextBoxExtender" runat="server"
                            Enabled="True" FilterType="Custom, Numbers" TargetControlID="txtEmissionFactor"
                            ValidChars=".">
                        </act:FilteredTextBoxExtender>
                        <asp:RequiredFieldValidator ID="reqvEmissionFactor" runat="server" ControlToValidate="txtEmissionFactor"
                            ErrorMessage="Emission factor value is required" ValidationGroup="vgEmissions"
                            Display="Dynamic">*</asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="rngvEmissionFactor" runat="server" ControlToValidate="txtEmissionFactor"
                            Display="Dynamic" ErrorMessage="Emission factor must be from 0.0001 to 9999."
                            MinimumValue="0.0001" Type="Double" MaximumValue="9999" ValidationGroup="vgEmissions">*</asp:RangeValidator>
                    </div>
                    <div class="fieldwrapper">
                        <asp:Label ID="lblEFNumUoM" class="styled" runat="server" Text="Emission Factor Numerator:"></asp:Label>
                        <asp:DropDownList ID="ddlEFNumUoM" runat="server" class="">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="reqvEFNumUoM" runat="server" ControlToValidate="ddlEFNumUoM"
                            Display="Dynamic" ErrorMessage="Select emission factor numerator" InitialValue="--Select Numerator--"
                            ValidationGroup="vgEmissions">*</asp:RequiredFieldValidator>
                    </div>
                    <div class="fieldwrapper">
                        <asp:Label ID="lblEFDenUoM" class="styled" runat="server" Text="Emission Factor Denominator:"></asp:Label>
                        <asp:DropDownList ID="ddlEFDenUoM" runat="server" class="">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="reqvEFDenUoM" runat="server" ControlToValidate="ddlEFDenUoM"
                            Display="Dynamic" ErrorMessage="Select emission factor denominator" InitialValue="--Select Denominator--"
                            ValidationGroup="vgEmissions">*</asp:RequiredFieldValidator>
                    </div>
                    <div class="fieldwrapper">
                        <asp:Label ID="lblEFExplanation" class="styled" runat="server" Text="Emission Factor Explanation:"></asp:Label>
                        <asp:TextBox ID="txtEFExplanation" runat="server" class="editable" Text="" Width="500px"
                            MaxLength="100" ToolTip="100 character limit"></asp:TextBox>
                        <act:TextBoxWatermarkExtender ID="txtEFExplanation_TextBoxWatermarkExtender" runat="server"
                            Enabled="True" TargetControlID="txtEFExplanation" WatermarkCssClass="watermarked"
                            WatermarkText="OPTIONAL">
                        </act:TextBoxWatermarkExtender>
                    </div>
                    <div class="fieldwrapper">
                        <asp:Label ID="lblTotalEmissions" class="styled" runat="server" Text="Actual Emissions (tons/year):"></asp:Label>
                        <asp:TextBox ID="txtTotalEmissions" runat="server" class="editable" Text="" Width="100px"
                            MaxLength="6"></asp:TextBox>
                        <act:FilteredTextBoxExtender ID="txtTotalEmissions_FilteredTextBoxExtender" runat="server"
                            Enabled="True" FilterType="Custom, Numbers" TargetControlID="txtTotalEmissions"
                            ValidChars=".">
                        </act:FilteredTextBoxExtender>
                        <asp:RequiredFieldValidator ID="reqvTotalEmission" runat="server" ControlToValidate="txtTotalEmissions"
                            ErrorMessage="Annual emissions value is required" ValidationGroup="vgEmissions"
                            Display="Dynamic">*</asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="rngvTotalEmissions" runat="server" ControlToValidate="txtTotalEmissions"
                            Display="Dynamic" ErrorMessage="The total emissions must be from 0 up to 100000 tons."
                            MaximumValue="100000" MinimumValue="0" Type="Double" ValidationGroup="vgEmissions">*</asp:RangeValidator>
                    </div>
                    <asp:Panel ID="pnlSummerDay" runat="server">
                        <div class="fieldwrapper">
                            <asp:Label ID="lblSummmerDay" class="styled" runat="server" Text="Process operated May&nbsp;1 - Sep&nbsp;30?:"
                                ToolTip="Did the process operate any time between May&nbsp;1 and Sep&nbsp;30?"></asp:Label>
                            <asp:DropDownList ID="ddlSummerDay" runat="server" class="" AutoPostBack="True">
                                <asp:ListItem>--Select Yes or No--</asp:ListItem>
                                <asp:ListItem>Yes</asp:ListItem>
                                <asp:ListItem>No</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="reqvSummerDayddl" runat="server" ControlToValidate="ddlSummerDay"
                                Display="Dynamic" ErrorMessage="Select Yes or No indicating if process operated any time between May&nbsp;1 and Sep&nbsp;30."
                                InitialValue="--Select Yes or No--" ValidationGroup="vgEmissions">*</asp:RequiredFieldValidator>
                            <asp:Label ID="Label1" runat="server" Font-Size="Small"
                                ForeColor="Black"
                                Text="* Select &quot;No&quot; if Actual ANNUAL Emissions less than 0.3 TPY"></asp:Label>
                        </div>
                        <asp:Panel ID="pnlSummerDayPollutant" runat="server">
                            <div class="fieldwrapper">
                                <asp:Label ID="lblSummerDayPollutant" class="styled" runat="server"></asp:Label>
                                <asp:TextBox ID="txtSummerDayPollutant" runat="server" class="editable" Text="" Width="100px"
                                    MaxLength="5"></asp:TextBox>
                                <act:FilteredTextBoxExtender ID="ftbeSummerDay" runat="server" Enabled="True" FilterType="Custom, Numbers"
                                    TargetControlID="txtSummerDayPollutant" ValidChars=".">
                                </act:FilteredTextBoxExtender>
                                <asp:RequiredFieldValidator ID="reqvSummerDayPollutant" runat="server" ControlToValidate="txtSummerDayPollutant"
                                    ErrorMessage="Emission quantity required for summer day pollutant" ValidationGroup="vgEmissions"
                                    Display="Dynamic">*</asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="cusvSummerDayPollutant" runat="server"
                                    ControlToValidate="txtSummerDayPollutant" Text="*" Display="Dynamic" ErrorMessage="*"
                                    ValidationGroup="vgEmissions"></asp:CustomValidator>
                                <asp:RegularExpressionValidator ID="regxvSummerDay" runat="server"
                                    ControlToValidate="txtSummerDayPollutant"
                                    ErrorMessage="No more than 3 digits after the decimal is allowed."
                                    ValidationExpression="^\d*(\.\d{1,3})?$"
                                    ValidationGroup="vgEmissions">*</asp:RegularExpressionValidator>
                                &nbsp;<asp:Label ID="lblSummerDayValue" runat="server" Font-Bold="True"
                                    Font-Size="Small" ForeColor="Red"
                                    Text="Warning: Summer Day value larger than expected, but has been saved. Please verify the amount."
                                    Visible="False"></asp:Label>
                            </div>
                        </asp:Panel>
                    </asp:Panel>
                    <div class="fieldwrapper">
                        <asp:Label ID="lblEmissionsComment" runat="server" CssClass="styled" Text="Comment:"></asp:Label>
                        <div style="display: inline-block">
                            <asp:Label ID="lblExplanationRequired" runat="server" CssClass="highlight-normal asBlock">
                                The Emission Calculation Method chosen requires additional explanation.
                            </asp:Label>
                            <asp:TextBox ID="txtEmissionsComment" runat="server" class="editable" TextMode="MultiLine" Rows="5"
                                Text="" Width="400px" MaxLength="7500" ToolTip="7500 character limit. Any excess will be truncated."></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rqvEmissionsComment" runat="server" ControlToValidate="txtEmissionsComment"
                                ErrorMessage="Explanation of Emission Calculation Method is required" ValidationGroup="vgEmissions"
                                Display="Dynamic">*</asp:RequiredFieldValidator>
                            <act:TextBoxWatermarkExtender ID="txtEmissionsComment_TextBoxWatermarkExtender" runat="server"
                                Enabled="True" TargetControlID="txtEmissionsComment" WatermarkCssClass="watermarked"
                                WatermarkText="OPTIONAL">
                            </act:TextBoxWatermarkExtender>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="buttonwrapper">
                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="buttondiv" ValidationGroup="vgEmissions"
                    Width="42px" />
                <asp:Button runat="server" ID="btnDetails2" CssClass="buttondiv" Text="Return to Details"
                    CausesValidation="False" />
                <asp:Button runat="server" ID="btnClear" CssClass="buttondiv" Text="Clear Form" CausesValidation="False" />
                <asp:Button runat="server" ID="btnDelete" CssClass="buttondiv" Text="Delete" />
                <act:ModalPopupExtender ID="mpeDeletePollutant" runat="server" DynamicServicePath=""
                    Enabled="True" TargetControlID="btnDelete" BackgroundCssClass="modalProgressGreyBackground"
                    CancelControlID="btnCancel" PopupControlID="pnlConfirmDelete">
                </act:ModalPopupExtender>
                <br />
                <asp:Label ID="lblSaveMessage2" runat="server" Font-Bold="True" Font-Names="Arial"
                    Font-Size="Small" ForeColor="Red"></asp:Label>
            </div>
            <div class="fieldwrapperseparator">
                <asp:Label ID="lblSummary" class="styledseparator" runat="server" Text="lblSeparatorwithSaveandNoText"
                    BackColor="#ffffff" ForeColor="#ffffff"></asp:Label>
            </div>
            <asp:Panel ID="pnlConfirmDelete" runat="server" Width="500px" BackColor="White" BorderColor="Black"
                BorderStyle="Solid" Style="display: none;">
                <div class="confirmdelete">
                    <table align="center" width="500px">
                        <tr>
                            <td>&nbsp; &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="font-family: Verdana; font-weight: bold; font-size: medium; color: #D20000">Confirm Pollutant Deletion
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:RequiredFieldValidator ID="reqvPollutanttoDelete" runat="server"
                                    ControlToValidate="ddlPollutant">* An existing pollutant must be selected on the page.</asp:RequiredFieldValidator>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblPollutantToDelete" runat="server" Text="Pollutant to be deleted:"
                                    Font-Names="Verdana" Font-Size="Small"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtPollutantToDelete" runat="server" BorderStyle="None" Font-Bold="True"
                                    ReadOnly="True" Font-Size="Small"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <table align="center" width="500px">
                        <tr>
                            <td align="left">
                                <b>NOTE</b>: If the facility is in the ozone non-attainment area and VOC or NOx
                                    is being deleted then both the annual and the summer day emissions will be
                                    deleted. If VOC or NOx emissions exist without summer day emissionss, then those
                                    emissions must be re-entered without the summer day component.</td>
                        </tr>
                        <tr>
                            <td>&nbsp;&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblConfirmDelete1" runat="server" Font-Names="Verdana" Font-Size="Small"
                                    Text="Are you sure you want to delete this pollutant?"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="buttonwrapper">
                                    <asp:Button ID="btnConfirmDelete" runat="server" CausesValidation="False" CssClass="buttondiv"
                                        Text="Delete" UseSubmitBehavior="False" />
                                    <asp:Button ID="btnCancel" runat="server" CausesValidation="False" CssClass="buttondiv"
                                        Text="Cancel" UseSubmitBehavior="False" />
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
