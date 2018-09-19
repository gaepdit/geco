<%@ Page Title="Edit Release Point - GECO Facility Inventory" Language="VB" MasterPageFile="eismaster.master"
    AutoEventWireup="false" MaintainScrollPositionOnPostback="true"
    Inherits="GECO.eis_stack_edit" Codebehind="stack_edit.aspx.vb" %>

<%@ Register Assembly="Reimers.Google.Map" Namespace="Reimers.Google.Map" TagPrefix="Reimers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
    <script type="text/javascript">
        function Count(text, maxlength, displayEl) {
            var object = document.getElementById(text.id)  //get your object
            var div = document.getElementById(displayEl);
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
    <acs:ModalUpdateProgress ID="ModalUpdateProgress1" runat="server" DisplayAfter="0"
        BackgroundCssClass="modalProgressGreyBackground">
        <ProgressTemplate>
            <div class="modalPopup">
                &nbsp;<img src="<%= Page.ResolveUrl("~/assets/images/progressbar_green.gif") %>" alt="" align="middle" /><br />
                <span style="font-family: Verdana; font-size: small; font-weight: normal">Please Wait...</span>
            </div>
        </ProgressTemplate>
    </acs:ModalUpdateProgress>
    <div class="pageheader">
        Edit Stack Release Point Details
        <asp:TextBox ID="txtStackStatusCodeOnLoad" runat="server" ReadOnly="True" Width="10px"
            Visible="False"></asp:TextBox>
        <asp:TextBox ID="txtStackStatusCodeChanged" runat="server" ReadOnly="True" Width="10px"
            Visible="False"></asp:TextBox>
        <asp:Button ID="btnReturnToSummary" runat="server" Text="Summary" CausesValidation="False"
            CssClass="summarybutton" UseSubmitBehavior="False" PostBackUrl="~/eis/releasepoint_summary.aspx" />
    </div>
    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
        <ContentTemplate>
            <div class="fieldwrapperseparator">
                <asp:Label ID="lblSeparator1" class="styledseparator" runat="server" Text="Stack Information"></asp:Label>
                <div class="sepbuttons">
                    <asp:Button ID="btnSaveStack1" runat="server" Text="Save" ToolTip="" Font-Size="Small"
                        ValidationGroup="vgStack" UseSubmitBehavior="False" />
                    &nbsp;<asp:Button ID="btnCancel" runat="server" Text="Return to Details" ToolTip=""
                        Font-Size="Small" CausesValidation="False" UseSubmitBehavior="False" />
                </div>
                <asp:Label ID="lblStackMessage" runat="server" CssClass="labelMessage" Font-Bold="True"
                    Font-Names="Arial" Font-Size="Small" ForeColor="Red"></asp:Label>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:ValidationSummary ID="sumvStack" runat="server" HeaderText="The following errors occurred on the page:"
        ValidationGroup="vgStack" ForeColor="Red"></asp:ValidationSummary>
    <div class="fieldwrapper">
        <asp:Label ID="lblReleasePointID" class="styled" runat="server" Text="Stack ID:"></asp:Label>
        <asp:TextBox ID="txtReleasePointID" runat="server" ReadOnly="true" class="readonly" MaxLength="6"
            Text="" Width="100px"></asp:TextBox>
    </div>
    <%--Required Stack fields--%>
    <div class="fieldwrapper">
        <asp:Label ID="lblRPDescription" class="styled" runat="server" Text="Stack Description:"></asp:Label>
        <asp:TextBox ID="txtRPDescription" runat="server" class="editable" Text=""
            ToolTip="Description of the Release Point." MaxLength="100"></asp:TextBox>
        <asp:RequiredFieldValidator ID="reqvRPDescription" ControlToValidate="txtRPDescription"
            runat="server" ErrorMessage="The stack description is required." ValidationGroup="vgStack"
            Display="Dynamic">*</asp:RequiredFieldValidator>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblddlddlRPtypeCode" CssClass="styled" runat="server" Text="Stack Type:"></asp:Label>
        <asp:DropDownList ID="ddlRPtypeCode" runat="server" class="">
        </asp:DropDownList>
        <asp:RequiredFieldValidator ID="reqvRPtypeCode" ControlToValidate="ddlRPtypeCode"
            InitialValue="--Select Stack Type--" runat="server" ErrorMessage="The stack type is required."
            ValidationGroup="vgStack" Display="Dynamic">*</asp:RequiredFieldValidator>
    </div>
    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
        <ContentTemplate>
            <div class="fieldwrapper">
                <asp:Label ID="lblddlstackStatusCode" CssClass="styled" runat="server" Text="Stack Operating Status:"></asp:Label>
                <asp:DropDownList ID="ddlStackStatusCode" runat="server" class="" AutoPostBack="True">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="reqvStackStatusCode" ControlToValidate="ddlstackStatusCode"
                    InitialValue="--Select Operating Status--" runat="server" ValidationGroup="vgStack"
                    ErrorMessage="The operating status is required." Display="Dynamic">*</asp:RequiredFieldValidator>
            </div>
            <div class="fieldwrapper">
                <asp:Label ID="lblRPStackHeightMeasure" class="styled" runat="server" Text="Stack Height (ft):"></asp:Label>
                <asp:TextBox ID="txtRPStackHeightMeasure" runat="server" class="editable" Text=""
                    Width="100px" MaxLength="6"></asp:TextBox>
                <act:FilteredTextBoxExtender ID="filttxtRPStackHeightMeasure" runat="server" Enabled="True"
                    FilterType="Custom, Numbers" TargetControlID="txtRPStackHeightMeasure" ValidChars=".">
                </act:FilteredTextBoxExtender>
                <asp:RequiredFieldValidator ID="reqvRPStackHeightMeasure" ControlToValidate="txtRPStackHeightMeasure"
                    runat="server" ErrorMessage="The stack height is required." ValidationGroup="vgStack"
                    Display="Dynamic">*</asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="rgxvStackHeight" runat="server" ControlToValidate="txtRPStackHeightMeasure"
                    ErrorMessage="Stack height can have at most one decimal place. " ValidationExpression="(\d{0,4})?([\.]{1})?(\d{0,1})"
                    ValidationGroup="vgStack" Display="Dynamic">One decimal place allowed. </asp:RegularExpressionValidator>
                <asp:RangeValidator ID="rngvRPStackHeightMeasure" runat="server" ControlToValidate="txtRPStackHeightMeasure"
                    MinimumValue="1.0" MaximumValue="1300" Type="Double" ErrorMessage="The stack height must be between 1.0 and 1300 feet."
                    ValidationGroup="vgStack">Must be 1.0 to 1300.</asp:RangeValidator>
            </div>
            <div class="fieldwrapper">
                <asp:Label ID="lblRPStackDiameterMeasure" class="styled" runat="server" Text="Stack Diameter (ft):"></asp:Label>
                <asp:TextBox ID="txtRPStackDiameterMeasure" runat="server" class="editable" Text=""
                    Width="100px" MaxLength="4"></asp:TextBox>
                <act:FilteredTextBoxExtender ID="filtxtRPStackDiameterMeasure" runat="server" Enabled="True"
                    FilterType="Custom, Numbers" TargetControlID="txtRPStackDiameterMeasure" ValidChars=".">
                </act:FilteredTextBoxExtender>
                <asp:RequiredFieldValidator ID="reqvRPStackDiameterMeasure" ControlToValidate="txtRPStackDiameterMeasure"
                    runat="server" ErrorMessage="The stack diameter is required." ValidationGroup="vgStack"
                    Display="Dynamic">*</asp:RequiredFieldValidator>
                <asp:RangeValidator ID="rngvRPStackHeightMeasure0" runat="server" ControlToValidate="txtRPStackDiameterMeasure"
                    ErrorMessage="The stack diameter must be between 0.1 and 100 feet." MaximumValue="100"
                    MinimumValue="0.1" Type="Double" ValidationGroup="vgStack">0.1 to 100. </asp:RangeValidator>
                <asp:RegularExpressionValidator ID="rgxvStackDiameter" runat="server" ControlToValidate="txtRPStackDiameterMeasure"
                    ErrorMessage="Stack diameter can have at most one decimal place. " ValidationExpression="\d*\.?\d?"
                    ValidationGroup="vgStack">At most one decimal place allowed. </asp:RegularExpressionValidator>
                <asp:CompareValidator ID="cmpvStackHeightDiameter" runat="server" ControlToCompare="txtRPStackHeightMeasure"
                    ControlToValidate="txtRPStackDiameterMeasure" ErrorMessage="Stack diameter must be less than stack height"
                    Operator="LessThan" Type="Double" ValidationGroup="vgStack">Diameter must be less than height. </asp:CompareValidator>
            </div>
            <div class="fieldwrapper">
                <asp:Label ID="lblRPExitGasVelocityMeasure" class="styled" runat="server" Text="Exit Gas Velocity (fps):"></asp:Label>
                <asp:TextBox ID="txtRPExitGasVelocityMeasure" runat="server" class="editable" Text=""
                    Width="100px" MaxLength="5"></asp:TextBox>
                <act:FilteredTextBoxExtender ID="filtxtRPExitGasVelocityMeasure" runat="server" Enabled="True"
                    FilterType="Custom, Numbers" TargetControlID="txtRPExitGasVelocityMeasure" ValidChars=".">
                </act:FilteredTextBoxExtender>
                <%-- <asp:RequiredFieldValidator ID="reqvRPExitGasVelocityMeasure" ControlToValidate="txtRPExitGasVelocityMeasure"
                    runat="server" ErrorMessage="The Exit Gas Velocity is required." ValidationGroup="vgStack"
                    Display="Dynamic">*</asp:RequiredFieldValidator>--%>
                <asp:RegularExpressionValidator ID="rgxvExitGasVelocity" runat="server" ControlToValidate="txtRPExitGasVelocityMeasure"
                    ErrorMessage="Exit gas velocity can have at most one decimal place." ValidationExpression="\d*\.?\d?"
                    ValidationGroup="vgStack">At most one decimal place allowed. </asp:RegularExpressionValidator>
                <asp:RangeValidator ID="rngvRPExitGasVelocityMeasure" runat="server" ControlToValidate="txtRPExitGasVelocityMeasure"
                    MinimumValue="0.1" MaximumValue="600" Type="Double" ErrorMessage="The exit gas velocity is outside the expected range of 0.1 to 600 FPS."
                    ValidationGroup="vgStack">Must be 0.1 to 600. </asp:RangeValidator>
                <asp:CustomValidator ID="custRPExitGASVelocityMeasure" ValidateEmptyText="true" ControlToValidate="txtRPExitGasVelocityMeasure"
                    runat="server" OnServerValidate="FlowRateRangeAndGasVelocityCheck" ValidationGroup="vgStack"
                    Display="Dynamic"></asp:CustomValidator>
            </div>
            <div class="fieldwrapper">
                <asp:Label ID="lblRPExitGasFlowRateMeasure" class="styled" runat="server" Text="Exit Gas Flow Rate (acfs):"></asp:Label>
                <asp:TextBox ID="txtRPExitGasFlowRateMeasure" runat="server" class="editable" Text=""
                    Width="100px" MaxLength="10"></asp:TextBox>
                <act:FilteredTextBoxExtender ID="filtxtRPExitGasFlowRateMeasure" runat="server" Enabled="True"
                    FilterType="Custom, Numbers" TargetControlID="txtRPExitGasFlowRateMeasure" ValidChars=".">
                </act:FilteredTextBoxExtender>
                <asp:RegularExpressionValidator ID="rgxvExitGasFlowRate" runat="server" ControlToValidate="txtRPExitGasFlowRateMeasure"
                    ErrorMessage="Exit gas flow rate can have at most one decimal place. " ValidationExpression="\d*\.?\d?"
                    ValidationGroup="vgStack">At most one decimal place allowed. </asp:RegularExpressionValidator>
                <asp:CustomValidator ID="cusvRPExitGasFlowRateMeasure" ControlToValidate="txtRPExitGasFlowRateMeasure"
                    runat="server" OnServerValidate="FlowRateRangeCheck" ValidationGroup="vgStack"
                    Display="Dynamic"></asp:CustomValidator>
            </div>
            <div class="fieldwrapper">
                <asp:Label ID="lblRPExitGasTemperatureMeasure" class="styled" runat="server" Text="Exit Gas Temperature (°F):"></asp:Label>
                <asp:TextBox ID="txtRPExitGasTemperatureMeasure" runat="server" class="editable"
                    Text="" Width="100px" MaxLength="4"></asp:TextBox>
                <act:FilteredTextBoxExtender ID="filtxtRPExitGasTemperatureMeasure" runat="server"
                    Enabled="True" FilterType="Custom, Numbers" TargetControlID="txtRPExitGasTemperatureMeasure"
                    ValidChars="-">
                </act:FilteredTextBoxExtender>
                <asp:RequiredFieldValidator ID="reqvRPExitGasTemperatureMeasure" ControlToValidate="txtRPExitGasTemperatureMeasure"
                    runat="server" ErrorMessage="The Exit Gas Temperature is required." ValidationGroup="vgStack"
                    Display="Dynamic">*</asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="rgxvExitgasTemp" runat="server" ControlToValidate="txtRPExitGasTemperatureMeasure"
                    ErrorMessage="Exit gas temperature must be an integer (no decimal places). "
                    ValidationExpression="^-{0,1}\d+$" ValidationGroup="vgStack">Must be an integer. </asp:RegularExpressionValidator>
                <asp:RangeValidator ID="rngvRPExitGasTemperatureMeasure" runat="server" ControlToValidate="txtRPExitGasTemperatureMeasure"
                    MinimumValue="-30" MaximumValue="3500" Type="Integer" ErrorMessage="The exit gas temperature is outside the expected range of -30 to 3500 °F."
                    ValidationGroup="vgStack">Must be -30 to 3500. </asp:RangeValidator>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <%--Optional Stack Fields--%>
    <div class="fieldwrapper">
        <asp:Label ID="lblRPFenceLineDistanceMeasure" class="styled" runat="server" Text="Fence Line Distance (ft):"></asp:Label>
        <asp:TextBox ID="txtRPFenceLineDistanceMeasure" runat="server" class="editable" Text=""
            Width="100px" MaxLength="5"></asp:TextBox>
        <act:FilteredTextBoxExtender ID="filtxtRPFenceLineDistanceMeasure" runat="server"
            Enabled="True" FilterType="Numbers" TargetControlID="txtRPFenceLineDistanceMeasure">
        </act:FilteredTextBoxExtender>
        <act:TextBoxWatermarkExtender ID="tbwRPFenceLineDistanceMeasure" runat="server" WatermarkText="OPTIONAL"
            WatermarkCssClass="watermarked" Enabled="True" TargetControlID="txtRPFenceLineDistanceMeasure">
        </act:TextBoxWatermarkExtender>
        <asp:RangeValidator ID="rngvRPFenceLineDistanceMeasure" runat="server" ControlToValidate="txtRPFenceLineDistanceMeasure"
            MinimumValue="0" MaximumValue="99999" Type="Integer" ErrorMessage="The fence line distance is outside the expected range of 0 99,999 feet."
            ValidationGroup="vgStack">*</asp:RangeValidator>
        <asp:RegularExpressionValidator ID="rgxvStackHeight0" runat="server" ControlToValidate="txtRPFenceLineDistanceMeasure"
            ErrorMessage="Fence Line Distance must be an integer (no decimal places).  "
            ValidationExpression="(\d{0,5})?([\.]{1})?(\d{0,0})" ValidationGroup="vgStack">Must be an integer. </asp:RegularExpressionValidator>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblRPComment" class="styled" runat="server" Text="Comment:"></asp:Label>
        <div style="display: inline-block">
            <asp:TextBox ID="txtRPComment" runat="server" Rows="4" onKeyUp="javascript:Count(this,400,'dvComment');" class="editable" TextMode="MultiLine"
                Text="" Width="400px"></asp:TextBox>
            <div id="dvComment" style="font: bold"></div>
            <asp:RegularExpressionValidator ID="regexpName" runat="server"
                ErrorMessage="Comment not to exceed 400 characters."
                ControlToValidate="txtRPComment"
                ValidationExpression="^[\s\S]{0,400}$" ValidationGroup="vgStack" />
        </div>
    </div>
    <div class="fieldwrapperseparator">
        <asp:Label ID="lblSeparator" class="styledseparator" runat="server" Text="Release Point Apportionments"></asp:Label>
        <asp:Label ID="lblReleasePointAppMessage" runat="server" Font-Size="Small" ForeColor="#CA0000"
            CssClass="labelwarningleft"></asp:Label><br />
    </div>
    <div class="gridview">
        <asp:GridView ID="gvwRPApportionment" runat="server" DataSourceID="SqlDataSourceRPApp"
            CellPadding="4" Font-Names="Arial" Font-Size="Small" ForeColor="#333333" GridLines="None"
            AutoGenerateColumns="False">
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#EFF3FB" />
            <Columns>
                <asp:BoundField DataField="EmissionsUnitID" HeaderText="Emission Unit ID" />
                <asp:HyperLinkField DataNavigateUrlFields="ProcessID,EmissionsUnitID" DataNavigateUrlFormatString="~/eis/rpapportionment_edit.aspx?ep={0}&amp;eu={1}"
                    DataTextField="ProcessID" HeaderText="Process ID" NavigateUrl="~/eis/rpapportionment_edit.aspx" />
                <asp:HyperLinkField DataNavigateUrlFields="ProcessID,EmissionsUnitID" DataNavigateUrlFormatString="~/eis/rpapportionment_edit.aspx?ep={0}&amp;eu={1}"
                    DataTextField="strprocessdescription" HeaderText="Process Description" NavigateUrl="~/eis/rpapportionment_edit.aspx">
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                </asp:HyperLinkField>
                <asp:BoundField DataField="ReleasePointID" HeaderText="Release Point ID" />
                <asp:BoundField DataField="intaveragepercentemissions" HeaderText="Apportionment %" />
            </Columns>
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#2461BF" />
            <AlternatingRowStyle BackColor="White" />
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSourceRPApp" runat="server"></asp:SqlDataSource>
    </div>
    <%-- Stack Geographic Coordinate Information --%>
    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
        <ContentTemplate>
            <div class="fieldwrapperseparator">
                <asp:Label ID="lblSeparator2" class="styledseparator" runat="server" Text="Stack Geographic Coordinate Information:"></asp:Label>
                <asp:Label ID="lblStackGCDataMissing" runat="server" Text="" ForeColor="Red" Font-Bold="True"></asp:Label>
            </div>
            <asp:UpdatePanel ID="upLatLon" runat="server">
                <ContentTemplate>
                    <div class="fieldwrapper">
                        <asp:Label ID="LblLatitudeMeasure" class="styled" runat="server" Text="Latitude:"></asp:Label>
                        <asp:TextBox ID="TxtLatitudeMeasure" runat="server" class="editable" Text="" Width="150px"
                            MaxLength="8"></asp:TextBox>
                        <act:FilteredTextBoxExtender ID="filtxtLatitudeMeasure" runat="server" Enabled="True"
                            FilterType="Custom, Numbers" TargetControlID="TxtLatitudeMeasure" ValidChars=".">
                        </act:FilteredTextBoxExtender>
                        <asp:RequiredFieldValidator ID="reqvLatitudeMeasure" ControlToValidate="TxtLatitudeMeasure"
                            runat="server" ValidationGroup="vgStack" ErrorMessage="The release point latitude is required."
                            Display="Dynamic">*</asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="rngvLatitudeMeasure" runat="server" ControlToValidate="TxtLatitudeMeasure"
                            ValidationGroup="vgStack" MaximumValue="35.00028" MinimumValue="30.35944" Type="Double"
                            ErrorMessage="Latitiude must be between 30.35944 and 35.200028 degrees"
                            Display="Dynamic">Must be between 30.35944 and 35.200028</asp:RangeValidator>
                    </div>
                    <div class="fieldwrapper">
                        <asp:Label ID="LblLongitudeMeasure" class="styled" runat="server" Text="Longitude:"></asp:Label>
                        <asp:TextBox ID="TxtLongitudeMeasure" runat="server" class="editable" Text="" Width="150px"
                            MaxLength="9"></asp:TextBox>
                        <act:FilteredTextBoxExtender ID="filtxtLongitudeMeasure" runat="server" Enabled="True"
                            FilterType="Custom, Numbers" TargetControlID="TxtLongitudeMeasure" ValidChars=".-">
                        </act:FilteredTextBoxExtender>
                        <asp:RequiredFieldValidator ID="reqvLongitudeMeasure" ControlToValidate="TxtLongitudeMeasure"
                            runat="server" ValidationGroup="vgStack" ErrorMessage="The release point longitude is required."
                            Display="Dynamic">*</asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="rngvLongitudeMeasure" runat="server" ControlToValidate="TxtLongitudeMeasure"
                            ValidationGroup="vgStack" MinimumValue="-85.60889" MaximumValue="-80.84417" Type="Double"
                            ErrorMessage="Longitude must be between -85.60889 and -80.84417 degrees."
                            Display="Dynamic">Must be between -85.60889 and -80.84417</asp:RangeValidator>
                    </div>
                    <div class="fieldwrapper">
                        <asp:Label class="styled" runat="server" Text="Map:"></asp:Label>
                        <div style="display: inline-block; width: 610px">
                            <asp:HyperLink ID="lnkGoogleMap" runat="server" Text="View in Google Maps" Target="_blank">
                                <asp:Image ID="imgGoogleStaticMap" runat="server" />
                            </asp:HyperLink><br />
                            <asp:Panel ID="pnlLocationMap" runat="server" Width="610px"></asp:Panel>
                            <p>
                                The release point latitude/longitude is centered in the map above. If the location is incorrect, 
                                use the &quot;Pick Latitude/Longitude&quot; button to select the correct location.
                            </p>
                            <asp:Button ID="lbtnGetLatLon" runat="server" CausesValidation="false" Text="Pick Latitude/Longitude" />
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="lbtnGetLatLon" />
                </Triggers>
            </asp:UpdatePanel>
            <div class="fieldwrapper">
                <asp:Label ID="LblHorCollectionMetCode" CssClass="styled" runat="server" Text="Horizontal Collection Method:"></asp:Label>
                <asp:DropDownList ID="ddlHorCollectionMetCode" runat="server" Width="600px"></asp:DropDownList>
                <asp:RequiredFieldValidator ID="reqvHorCollectionMetCode" ControlToValidate="ddlHorCollectionMetCode"
                    runat="server" ValidationGroup="vgStack" ErrorMessage="The Horizontal Collection Method Code is required."
                    InitialValue="--Select Horizontal Collection Method--" Display="Dynamic">*</asp:RequiredFieldValidator>
            </div>
            <div class="fieldwrapper">
                <asp:Label ID="LblHorizontalAccuracyMeasure" class="styled" runat="server" Text="Accuracy Measure (meters):"></asp:Label>
                <asp:TextBox ID="TxtHorizontalAccuracyMeasure" runat="server" class="editable" Text=""
                    Width="100px" ToolTip="The horizontal measure of the relative accuracy of the latitude and longitude coordinates."
                    MaxLength="4"></asp:TextBox>
                <act:FilteredTextBoxExtender ID="filtxtHorizontalAccuracyMeasure" runat="server"
                    Enabled="True" FilterType="Numbers" TargetControlID="TxtHorizontalAccuracyMeasure">
                </act:FilteredTextBoxExtender>
                <asp:RequiredFieldValidator ID="reqvHorizontalAccuracyMeasure" ControlToValidate="TxtHorizontalAccuracyMeasure"
                    runat="server" ValidationGroup="vgStack" ErrorMessage="The Accuracy Measure is required."
                    Display="Dynamic">*</asp:RequiredFieldValidator>
                <asp:RangeValidator ID="rngvHorizontalAccuracyMeasure" runat="server" ControlToValidate="TxtHorizontalAccuracyMeasure"
                    ErrorMessage="The Accuracy Measure must be between 1 and 2000 meters." MaximumValue="2000"
                    MinimumValue="1" ValidationGroup="vgStack" Display="Dynamic" Type="Integer">Must be between 1 and 2000</asp:RangeValidator>
            </div>
            <div class="fieldwrapper">
                <asp:Label ID="lblHorReferenceDatCode" CssClass="styled" runat="server" Text="Horizontal Reference Datum:"></asp:Label>
                <asp:DropDownList ID="ddlHorReferenceDatCode" runat="server" ToolTip="The reference datum used in determining the latitude and longitude.  The most common is the &quot;North American datum of 1983&quot;.">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="reqvHorReferenceDatCode" ControlToValidate="ddlHorReferenceDatCode"
                    runat="server" ValidationGroup="vgStack" ErrorMessage="The Horizontal Reference Datum Code is required."
                    InitialValue="--Select Horizontal Reference Datum--" Display="Dynamic">*</asp:RequiredFieldValidator>
            </div>
            <div class="fieldwrapper">
                <asp:Label ID="LblGeographicComment" class="styled" runat="server" Text="Comment:"></asp:Label>
                <div style="display: inline-block">
                    <asp:TextBox ID="TxtGeographicComment" runat="server" class="editable" TextMode="MultiLine" Rows="4" onKeyUp="javascript:Count(this,400,'dvComment2');"
                        Text="" Width="400px"></asp:TextBox>
                    <div id="dvComment2" style="font: bold"></div>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                        ErrorMessage="Comment not to exceed 400 characters."
                        ControlToValidate="TxtGeographicComment"
                        ValidationExpression="^[\s\S]{0,400}$" />
                </div>
            </div>
            <asp:HiddenField ID="hidLatitude" runat="server" Visible="false" />
            <asp:HiddenField ID="hidLongitude" runat="server" Visible="false" />
            <asp:HiddenField ID="hidHorCollectionMetCode" runat="server" Visible="false" />
            <asp:HiddenField ID="hidHorCollectionMetDesc" runat="server" Visible="false" />
            <asp:HiddenField ID="hidHorizontalAccuracyMeasure" runat="server" Visible="false" />
            <asp:HiddenField ID="hidHorReferenceDatCode" runat="server" Visible="false" />
            <asp:HiddenField ID="hidHorReferenceDatDesc" runat="server" Visible="false" />
            <asp:HiddenField ID="hidGeographicComment" runat="server" Visible="false" />

            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="buttonwrapper">
                        <asp:Button runat="server" ID="btnCancel3" Text="Return to Details" ToolTip="" Font-Size="Small"
                            CausesValidation="False" UseSubmitBehavior="False" Width="120px" />&nbsp;
                        <asp:Button ID="btnReturnToSummary2" runat="server" Text="Summary" ToolTip="" Font-Size="Small"
                            UseSubmitBehavior="False" CausesValidation="False" PostBackUrl="~/eis/releasepoint_summary.aspx" />&nbsp;
                        <asp:Button ID="btnSaveStack2" runat="server" Text="Save" ValidationGroup="vgStack"
                            Width="60px" />&nbsp;
                        <asp:Button ID="btnDelete" runat="server" Text="Delete" ToolTip="" Font-Size="Small"
                            CausesValidation="False" Width="60px" />
                        <act:ModalPopupExtender ID="mpeDelete" runat="server" BackgroundCssClass="modalProgressGreyBackground"
                            DynamicServicePath="" Enabled="True" TargetControlID="btnDelete" PopupControlID="pnlDeleteStack">
                        </act:ModalPopupExtender>
                        &nbsp;<asp:Label ID="lblStackMessage0" runat="server" CssClass="labelMessage" Font-Bold="True"
                            Font-Names="Arial" Font-Size="Small" ForeColor="Red"></asp:Label>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:Panel ID="pnlDeleteStack" BackColor="#ffffff" runat="server" BorderColor="#333399"
                BorderStyle="Ridge" ScrollBars="Auto" Width="450px" Style="display: none;">
                <table border="0" cellpadding="2" cellspacing="1" width="450px">
                    <tr>
                        <td align="center">
                            <strong><span style="color: #4169e1; font-size: 12pt; font-family: Verdana;">Stack Deleted</span></strong>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblDeleteStack" runat="server" Text="Are you sure you want to delete the stack?"></asp:Label>
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
                            <asp:Button ID="btnDeleteSummary" runat="server" Text="Summary" Width="80px" PostBackUrl="~/eis/releasepoint_summary.aspx" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">&nbsp;&nbsp;
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Panel ID="pnlMap" runat="server" Style="display: inherit" BackColor="#507CD1">
        <asp:Button ID="btnPopupDisplay" runat="server" Style="display: none;" />
        <Reimers:Map ID="GMap" Width="700" Height="400" runat="server"></Reimers:Map>
        <br />
        <table cellpadding="3" cellspacing="3">
            <tr>
                <td>
                    <asp:Label ID="lblMapLat" runat="server" ForeColor="White" Text="Latitude: "></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtMapLat" runat="server" ValidationGroup="LatLon"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvMapLat" runat="server" ErrorMessage="*" ControlToValidate="txtMapLat"
                        ValidationGroup="LatLon"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblMapLon" runat="server" ForeColor="White" Text="Longitude: "></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtMapLon" runat="server" ValidationGroup="LatLon"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvMapLon" runat="server" ErrorMessage="*" ControlToValidate="txtMapLon"
                        ValidationGroup="LatLon"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btnCloseMap" runat="server" Text="Close" CausesValidation="false" />
                </td>
                <td>
                    <asp:Button ID="btnUseLatLon" runat="server" Text="Use these values" ValidationGroup="LatLon" />
                </td>
            </tr>
        </table>
        <act:ModalPopupExtender ID="lbtnGetLatLon_ModalPopupExtender" runat="server" TargetControlID="btnPopupDisplay"
            BackgroundCssClass="modalProgressGreyBackground" PopupControlID="pnlMap" CancelControlID="btnCloseMap"
            RepositionMode="RepositionOnWindowResizeAndScroll" Y="20">
        </act:ModalPopupExtender>
    </asp:Panel>
</asp:Content>
