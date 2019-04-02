<%@ Page Title="Emissions Unit Edit - GECO Facility Inventory" Language="VB" MaintainScrollPositionOnPostback="true" MasterPageFile="eismaster.master"
    AutoEventWireup="false" Inherits="GECO.eis_emissionunit_edit" Codebehind="emissionunit_edit.aspx.vb" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
    <script type="text/javascript">
        function Count(text) {
            var maxlength = 400;
            var object = document.getElementById(text.id)
            var div = document.getElementById("dvComment");
            div.innerHTML = object.value.length + '/' + maxlength + ' character(s).';
            if (object.value.length > maxlength) {
                object.focus();
                object.value = text.value.substring(0, maxlength);
                object.scrollTop = object.scrollHeight;
                return false;
            }
            return true;
        }
    </script>
    <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></act:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
        <ContentTemplate>
            <div class="pageheader">
                Edit Emission Unit Details
        <asp:TextBox ID="txtUnitStatusCodeOnLoad" runat="server" ReadOnly="True"
            Width="50px" Visible="false"></asp:TextBox>
                <asp:TextBox ID="txtUnitStatusCodeChanged" runat="server" ReadOnly="True"
                    Width="50px" Visible="false"></asp:TextBox>
                <asp:Button ID="btnSummary1" runat="server" Text="Summary" CausesValidation="False"
                    CssClass="summarybutton" UseSubmitBehavior="False" />
            </div>
            <asp:ValidationSummary ID="sumvEmissionUnit" runat="server" Font-Names="Arial" Font-Size="Small"
                HeaderText="The following items need to be corrected. See items marked with *" />
            <div class="fieldwrapperseparator">
                <asp:Label ID="LblSeparatorEmissionUnit" class="styledseparator" runat="server" Text="Emission Unit"></asp:Label>
                <div class="sepbuttons">
                    <asp:Button ID="btnSaveEmissionUnit1" runat="server" Text="Save" ToolTip=""
                        Font-Size="Small" />
                    &nbsp;<asp:Button ID="btnCancel1" runat="server" Text="Return to Details" Font-Size="Small"
                        CausesValidation="False" UseSubmitBehavior="False" />
                </div>
                <asp:Label ID="lblSaveMessage1" runat="server" Font-Bold="True" Font-Names="Arial"
                    Font-Size="Small" ForeColor="Red" CssClass="labelMessage"></asp:Label>
            </div>
            <div class="fieldwrapper">
                <asp:Label ID="lblEmissionUnitID" class="styled" runat="server" Text="Emission Unit ID:"></asp:Label>
                <asp:TextBox ID="txtEmissionsUnitID" class="readonly" runat="server" Text="" BorderColor="White"
                    ReadOnly="True" Width="100px"></asp:TextBox>
            </div>
            <div class="fieldwrapper">
                <asp:Label ID="lblUnitDescription" class="styled" runat="server" Text="Emission Unit Description:"></asp:Label>
                <asp:TextBox ID="txtUnitDescription" runat="server" class="editable" Text="" MaxLength="100"
                    Width="350px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="reqvUnitDesc" runat="server" ControlToValidate="txtUnitDescription"
                    ErrorMessage="Emission unit description is required." Display="Dynamic">*</asp:RequiredFieldValidator>
            </div>
            <div class="fieldwrapper">
                <asp:Label ID="lblUnitTypeCode" CssClass="styled" runat="server" Text="Unit Type:"></asp:Label>
                <asp:DropDownList ID="ddlUnitTypeCode" runat="server" class="" AutoPostBack="True">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="reqvUnitType" runat="server" ControlToValidate="ddlUnitTypeCode"
                    ErrorMessage="Unit type is required." InitialValue="--Select Unit Type--" Display="Dynamic">*</asp:RequiredFieldValidator>
            </div>
            <div class="fieldwrapper">
                <asp:Label ID="lblUnitStatusCode" CssClass="styled" runat="server" Text="Operating Status:"></asp:Label>
                <asp:DropDownList ID="ddlUnitStatusCode" runat="server" class=""
                    AutoPostBack="True"
                    ToolTip="If an Emission Unit is shutdown all underlying processes will be removed from the current reporting period and adding processes etc. will be disabled.">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="reqvOperatingStatus" runat="server" ControlToValidate="ddlUnitStatusCode"
                    ErrorMessage="Operating status required." InitialValue="--Select Unit Status--"
                    Display="Dynamic">*</asp:RequiredFieldValidator>
            </div>
            <div class="fieldwrapper">
                <asp:Label ID="lblUnitOperationDate" class="styled" runat="server" Text="Date Unit Placed In Operation:"></asp:Label>
                <asp:TextBox ID="txtUnitOperationDate" runat="server" class="editable" Text="" Width="150px"></asp:TextBox>
                <act:MaskedEditExtender ID="txtUnitOperationDate_MaskedEditExtender" runat="server"
                    CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat=""
                    CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                    CultureTimePlaceholder="" Enabled="True" Mask="99/99/9999" MaskType="Date" TargetControlID="txtUnitOperationDate"
                    UserDateFormat="MonthDayYear">
                </act:MaskedEditExtender>
                <act:CalendarExtender ID="txtUnitOperationDate_CalendarExtender" runat="server" Format="MM/dd/yyyy"
                    ClearTime="True" Enabled="True" TargetControlID="txtUnitOperationDate" PopupPosition="BottomRight">
                </act:CalendarExtender>
                <asp:RangeValidator ID="rngvDateInOperation" runat="server"
                    ControlToValidate="txtUnitOperationDate"
                    ErrorMessage="Date must be between 1/1/1900 and today." Type="Date" Display="Dynamic">* Date must be between 1/1/1900 and today.</asp:RangeValidator>
            </div>
            <asp:Panel ID="pnlFuelBurning" runat="server">
                <div class="fieldwrapper">
                    <asp:Label ID="lblUnitDesignCapacity" class="styled" runat="server" Text="Design Capacity:"></asp:Label>
                    <asp:TextBox ID="txtUnitDesignCapacity" runat="server" class="editable" Text="" Width="150px"></asp:TextBox>
                    <act:FilteredTextBoxExtender ID="txtUnitDesignCapacity_FilteredTextBoxExtender" runat="server"
                        Enabled="True" FilterType="Custom, Numbers" TargetControlID="txtUnitDesignCapacity"
                        ValidChars=".">
                    </act:FilteredTextBoxExtender>
                    <asp:RequiredFieldValidator ID="reqvDesigncapacity" runat="server" ControlToValidate="txtUnitDesignCapacity"
                        ErrorMessage="Design capacity is required." Display="Dynamic">*</asp:RequiredFieldValidator>
                    <asp:RangeValidator ID="rngvDesignCapacity" runat="server" ErrorMessage="Design capacity must be between 0.1 and 100,000,000."
                        ControlToValidate="txtUnitDesignCapacity" MaximumValue="100000000" MinimumValue="0.1"
                        Type="Double" Display="Dynamic">* Must be between 0.1 and 100,000,000</asp:RangeValidator>
                </div>
                <div class="fieldwrapper">
                    <asp:Label ID="lblUnitDesignCapacityUoMCode" class="styled" runat="server" Text="Design Capacity Unit:"></asp:Label>
                    <asp:DropDownList ID="ddlUnitDesignCapacityUOMCode" runat="server">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="reqvDesignCapUOM" runat="server" ControlToValidate="ddlUnitDesignCapacityUOMCode"
                        ErrorMessage="Select design capacity unit of measure." InitialValue="--Select Unit of Measure--"
                        Display="Dynamic">*</asp:RequiredFieldValidator>
                </div>
                <div class="fieldwrapper">
                    <asp:Label ID="lblElecGen" CssClass="styled" runat="server" Text="Electric generating unit?"></asp:Label>
                    <asp:DropDownList ID="ddlElecGen" runat="server" class="" AutoPostBack="True">
                        <asp:ListItem>--Select--</asp:ListItem>
                        <asp:ListItem>Yes</asp:ListItem>
                        <asp:ListItem>No</asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="reqvElecGen" runat="server" ControlToValidate="ddlElecGen"
                        ErrorMessage="Select Yes or No to indicate if electric generating unit." InitialValue="--Select--"
                        Display="Dynamic">*</asp:RequiredFieldValidator>
                </div>
                <asp:Panel ID="pnlElecGen" runat="server" Visible="false">
                    <div class="fieldwrapper">
                        <asp:Label ID="lblMaximumNameplateCapacity" class="styled" runat="server" Text="Maximum Nameplate Capacity (MW):"></asp:Label>
                        <asp:TextBox ID="txtMaximumNameplateCapacity" runat="server" class="editable" Text=""
                            Width="50px"></asp:TextBox>
                        <act:FilteredTextBoxExtender ID="txtMaximumNameplateCapacity_FilteredTextBoxExtender"
                            runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="txtMaximumNameplateCapacity"
                            ValidChars=".">
                        </act:FilteredTextBoxExtender>
                        <asp:RequiredFieldValidator ID="reqvMaxNameplateCap" runat="server" ControlToValidate="txtMaximumNameplateCapacity"
                            ErrorMessage="Maximum nameplate capacity required if electric generating unit."
                            Display="Dynamic">*</asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="rngvMaxnameplateCap" runat="server" ControlToValidate="txtMaximumNameplateCapacity"
                            ErrorMessage="Maximum nameplate capacity must be between 0.001 and 100,000 MW."
                            MaximumValue="100000" MinimumValue="0.001" Type="Double" Display="Dynamic">* Must be between 0.001 and 100,000</asp:RangeValidator>
                    </div>
                </asp:Panel>
            </asp:Panel>
            <div class="fieldwrapper">
                <asp:Label ID="lblUnitComment" runat="server" CssClass="styled" Text="Comment:"></asp:Label>
                <div style="display: inline-block">
                    <asp:TextBox ID="txtUnitComment" runat="server" class="editable" TextMode="MultiLine" onKeyUp="javascript:Count(this);"
                        Text="" Width="400px" MaxLength="400" Rows="4"></asp:TextBox>
                    <div id="dvComment" style="font: bold"></div>
                    <act:TextBoxWatermarkExtender ID="txtUnitComment_TextBoxWatermarkExtender" runat="server"
                        Enabled="True" TargetControlID="txtUnitComment" WatermarkText="OPTIONAL" WatermarkCssClass="watermarked">
                    </act:TextBoxWatermarkExtender>
                    <asp:RegularExpressionValidator ID="regexpProcessComment" runat="server"
                        ErrorMessage="Comment not to exceed 400 characters."
                        ControlToValidate="txtUnitComment"
                        ValidationExpression="^[\s\S]{0,400}$" />
                </div>
            </div>
            <div class="fieldwrapper">
                <asp:Label ID="lblLastEISSubmit" class="styled" runat="server" Text="Last Submitted to EPA:"></asp:Label>
                <asp:TextBox ID="txtLastEISSubmit" class="readonly" runat="server" Text="" ReadOnly="True"
                    Width="150px"></asp:TextBox>
            </div>
            <br />
            <div class="buttonwrapper">
                <asp:Button runat="server" ID="btnSaveEmissionUnit2" CssClass="buttondiv" Text="Save" />
                <asp:Button runat="server" ID="btnCancel2" CssClass="buttondiv" Text="Return to Details"
                    CausesValidation="False" ToolTip="Takes you back to the Emission Unit Details page"
                    UseSubmitBehavior="False" />
                <asp:Button runat="server" ID="btnSummary2" CssClass="buttondiv" Text="Summary" CausesValidation="False"
                    ToolTip="Takes you back to the Emission Unit Summary" UseSubmitBehavior="False" />
                <asp:Button ID="btnDeleteEmissionUnit" runat="server" CssClass="buttondiv" Text="Delete"
                    CausesValidation="False" />
                <act:ModalPopupExtender ID="mpeDeleteEmissionUnit" runat="server" DynamicServicePath=""
                    Enabled="True" TargetControlID="btnDeleteEmissionUnit" BackgroundCssClass="modalProgressGreyBackground"
                    PopupControlID="pnlConfirmDelete">
                </act:ModalPopupExtender>
                <%-- Following hidden field for modalpopup status warning --%>
                <asp:HiddenField ID="hidUnitStatusWarning" runat="server" />
                <act:ModalPopupExtender ID="mpeUnitStatusShutdown" runat="server" Enabled="True"
                    TargetControlID="hidUnitStatusWarning" PopupControlID="pnlUnitStatusWarning">
                </act:ModalPopupExtender>
            </div>
            <br />
            <asp:Panel ID="pnlConfirmDelete" runat="server" Width="450px" BackColor="White" BorderColor="Black"
                BorderStyle="Solid" Style="display: none;">
                <div class="confirmdelete">
                    <table align="center" width="450px">
                        <tr>
                            <td>&nbsp; &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="font-weight: bold; font-size: large; color: #D20000">Confirm Emission Unit Deletion
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="lblConfirmDelete1" runat="server" Text="Are you sure you want to delete this emission unit and all its underlying information?"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label ID="lblConfirmDelete2" runat="server" Text="Note: All emission unit control approach information as well as control
                        measures and associated pollutants will also be deleted."></asp:Label></td>
                        </tr>
                        <tr>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="buttonwrapper">
                                    <asp:Button ID="btnConfirmDelete" runat="server" CausesValidation="False" Text="Delete"
                                        CssClass="buttondiv" UseSubmitBehavior="False" />
                                    <asp:Button ID="btnNoDelete" runat="server" Text="No" CssClass="buttondiv" CausesValidation="False"
                                        UseSubmitBehavior="False" />
                                    <asp:Button ID="btnSummary" runat="server" Text="Summary" CssClass="buttondiv" CausesValidation="False"
                                        UseSubmitBehavior="False" />
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlUnitStatusWarning" runat="server" Width="450px" BackColor="White" BorderColor="Black"
                BorderStyle="Solid" Style="display: none;">
                <div class="confirmdelete" style="font-family: Verdana;">
                    <table align="center" width="450px">
                        <tr>
                            <td>&nbsp; &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="font-weight: bold; font-size: medium; font-family: Verdana; color: #D20000">Emission Unit Status Changed
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="font-size: small;">The emission unit status has been changed from an operating status to either a
                        temporarily or permanently shutdown status. This action will remove associated
                        processes and emissions for this emission unit from the current reporting period. Changes to
                        the processes for this emission unit will not be allowed.
                        <br />
                                <br />
                                Click Save to continue with the above action or Cancel to return to the form
                        without saving.</td>
                        </tr>
                        <tr>
                            <td>
                                <div class="buttonwrapper">
                                    <asp:Button ID="btnConfirmShutDownSave" runat="server" Text="Save"
                                        CssClass="buttondiv" ToolTip="" Font-Size="Small" />
                                    <asp:Button ID="btnCancelSave" runat="server" Text="Cancel" CssClass="buttondiv"
                                        CausesValidation="False" UseSubmitBehavior="False" />
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
