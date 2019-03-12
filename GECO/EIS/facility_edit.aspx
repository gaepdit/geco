<%@ Page Title="Facility Details Edit - GECO Facility Inventory" Language="VB" MaintainScrollPositionOnPostback="true" MasterPageFile="eismaster.master"
    AutoEventWireup="false" Inherits="GECO.eis_facility_edit" Codebehind="facility_edit.aspx.vb" %>

<%@ Register Assembly="Reimers.Google.Map" Namespace="Reimers.Google.Map" TagPrefix="Reimers" %>
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
    <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></act:ToolkitScriptManager>
    <acs:ModalUpdateProgress ID="ModalUpdateProgress1" runat="server" DisplayAfter="0"
        BackgroundCssClass="modalProgressGreyBackground">
        <ProgressTemplate>
            <div class="modalPopup">
                <img src="<%= Page.ResolveUrl("~/assets/images/progressbar_green.gif") %>" alt="" align="middle" /><br />
                <span style="font-family: Verdana; font-size: small; font-weight: normal">Please Wait...</span>
            </div>
        </ProgressTemplate>
    </acs:ModalUpdateProgress>
    <asp:Panel ID="pnlFacilityEdit" runat="server">
        <div class="pageheader">
            Edit Facility Details
            <asp:Button ID="btnReturnToDetails" runat="server" Text="Return to Details" CausesValidation="False"
                CssClass="summarybutton" UseSubmitBehavior="False" Style="margin-left: 5px;"
                PostBackUrl="~/eis/facility_details.aspx" />
            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="summarybutton" UseSubmitBehavior="true"
                Width="75px" />
            <asp:Label ID="lblFacilityMessage" runat="server" Font-Bold="True" Font-Names="Arial"
                Font-Size="Small" ForeColor="Red" CssClass="summarybutton" Style="padding-right: 5px;"></asp:Label>
        </div>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" HeaderText="You received the following errors:"></asp:ValidationSummary>
        <div class="fieldwrapperseparator">
            <p class="styledseparator">Facility Name and Address</p>
            <p class="label"><em>If the facility name or address are incorrect, please contact us using the &quot;Contact Us&quot; menu item above.</em></p>
        </div>
        <div class="fieldwrapper">
            <asp:Label ID="lblFacilitySiteName" class="styled" runat="server" Text="Facility Site Name:"></asp:Label>
            <asp:TextBox ID="txtFacilitySiteName" class="readonly" runat="server" ReadOnly="True"
                Width="300px"></asp:TextBox>
        </div>
        <div class="fieldwrapper">
            <asp:Label ID="LblLocationAddressText" class="styled" runat="server" Text="Facility Site Address:"></asp:Label>
            <asp:TextBox ID="TxtLocationAddressText" class="readonly" runat="server" Text=""
                ReadOnly="True" Width="300px"></asp:TextBox>
        </div>
        <div class="fieldwrapper">
            <asp:Label ID="LblSupplementalLocationText2" class="styled_noline" runat="server"
                Text=""></asp:Label>
            <asp:TextBox ID="TxtSupplementalLocationText2" class="readonly" runat="server" Text=""
                ReadOnly="True" Width="300px"></asp:TextBox>
        </div>
        <div class="fieldwrapper">
            <asp:Label ID="lblLocalityName" class="styled" runat="server" Text="Facility Site City:"></asp:Label>
            <asp:TextBox ID="TxtLocalityName" class="readonly" runat="server" Text="" ReadOnly="True"
                Width="200px"></asp:TextBox>
        </div>
        <div class="fieldwrapper">
            <asp:Label ID="LblLocationAddressStateCode" class="styled" runat="server" Text="Physical Location State:"></asp:Label>
            <asp:TextBox ID="TxtLocationAddressStateCode" class="readonly" runat="server" Text="GA"
                ReadOnly="True" Width="100px"></asp:TextBox>
        </div>
        <div class="fieldwrapper">
            <asp:Label ID="lblLocationAddressPostalCode" class="styled" runat="server" Text="Facility Site Zip Code:"></asp:Label>
            <asp:TextBox ID="TxtLocationAddressPostalCode" class="readonly" runat="server" Text=""
                ReadOnly="True" Width="100px"></asp:TextBox>
        </div>
        <br />
        <div class="fieldwrapperseparator">
            <asp:Label ID="lblSeparator2" class="styledseparator" runat="server" Text="Facility Mailing Address"></asp:Label>
        </div>
        <div class="fieldwrapper">
            <asp:Label ID="lblMailingAddressText" class="styled" runat="server" Text="Facility Mailing Address:"></asp:Label>
            <asp:TextBox ID="txtMailingAddressText" runat="server" class="editable" Text="" Width="350px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="reqvMailingAddressText" runat="server" ControlToValidate="txtMailingAddressText"
                ErrorMessage="The facility mailing address is required.">*</asp:RequiredFieldValidator>
        </div>
        <div class="fieldwrapper">
            <asp:Label ID="lblSupplementalAddressText" class="styled_noline" runat="server" Text=""></asp:Label>
            <asp:TextBox ID="TxtSupplementalAddressText" runat="server" class="editable" Text=""
                Width="350px"></asp:TextBox>
            <act:TextBoxWatermarkExtender ID="tbwSupplementalAddressText" runat="server" Enabled="True"
                TargetControlID="TxtSupplementalAddressText" WatermarkText="OPTIONAL" WatermarkCssClass="watermarked">
            </act:TextBoxWatermarkExtender>
        </div>
        <div class="fieldwrapper">
            <asp:Label ID="lblMailingAddressCityName" class="styled" runat="server" Text="Facility Mailing Address City:"></asp:Label>
            <asp:TextBox ID="txtMailingAddressCityName" runat="server" class="editable" Text=""
                Width="200px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="reqvMailingAddressCityName" runat="server" ControlToValidate="txtMailingAddressCityName"
                ErrorMessage="The facility mailing address city is required.">*</asp:RequiredFieldValidator>
        </div>
        <div class="fieldwrapper">
            <asp:Label ID="lblMailingAddressStateCode" class="styled" runat="server" Text="Facility Mailing Address State:"></asp:Label>
            <asp:DropDownList ID="ddlContact_MailState" runat="server">
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="reqvMailingAddressStateCode" ControlToValidate="ddlContact_MailState"
                InitialValue="--Select a State--" runat="server" ValidationGroup="vgStack" ErrorMessage="The facility mailing address state is required."
                Display="Dynamic">*</asp:RequiredFieldValidator>
        </div>
        <div class="fieldwrapper">
            <asp:Label ID="lblMailingAddressPostalCode" class="styled" runat="server" Text="Facility Mailing Address Zip Code:"></asp:Label>
            <asp:TextBox ID="txtMailingAddressPostalCode" runat="server" class="editable" Text=""
                Width="100px" MaxLength="10"></asp:TextBox>
            <asp:RequiredFieldValidator ID="reqvMailingAddressPostalCode" runat="server" ControlToValidate="txtMailingAddressPostalCode"
                ErrorMessage="The facility mailing address zip code is required.">*</asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="rgxvMailingAddressPostalCode" runat="server"
                ControlToValidate="txtMailingAddressPostalCode" ErrorMessage="Please check the facility mailing address zip code format."
                ValidationExpression="^(\d{5})(-\d{4})?$" Display="Dynamic">Format must be either 99999 or 99999-9999</asp:RegularExpressionValidator>
            <act:FilteredTextBoxExtender ID="filttxtMailingAddressPostalCode" runat="server"
                Enabled="True" TargetControlID="txtMailingAddressPostalCode" FilterType="Numbers, Custom"
                ValidChars="-">
            </act:FilteredTextBoxExtender>
        </div>
        <div class="fieldwrapper">
            <asp:Label ID="LblMailingAddressComment" runat="server" CssClass="styled" Text="Comment:"></asp:Label>
            <asp:TextBox ID="TxtMailingAddressComment" runat="server" class="editable" TextMode="MultiLine" Rows="4"
                Text="" Width="400px"></asp:TextBox>
        </div>
        <br />
        <div class="fieldwrapperseparator">
            <asp:Label ID="lblSeparator3" class="styledseparator" runat="server" Text="Facility Description"></asp:Label>
        </div>
        <div class="fieldwrapper">
            <asp:Label ID="lblFacilitySiteDescription" class="styled" runat="server" Text="Description:"></asp:Label>
            <asp:TextBox ID="TxtFacilitySiteDescription" runat="server" class="editable" Text=""
                Width="350px" MaxLength="100" ValidationGroup="vgFacilityDesc"></asp:TextBox>
            <asp:RequiredFieldValidator ID="reqvFacilitySiteDescription" runat="server" ControlToValidate="txtFacilitySiteDescription"
                ErrorMessage="The facility description is required.">*</asp:RequiredFieldValidator>
        </div>
        <div class="fieldwrapper">
            <asp:Label ID="lblFacilitySiteStatusCode" class="styled" runat="server" Text="Facility Operating Status:"></asp:Label>
            <asp:TextBox ID="txtFacilityStatusCode" runat="server" class="readonly" Text=""
                Width="250px" MaxLength="100" ReadOnly="true"></asp:TextBox>
            <asp:Label ID="lblStatusCodeNote" runat="server" Font-Size="Smaller"
                ForeColor="Blue" Text="* Current Status. Pertains to Emissions Inventory"></asp:Label>
        </div>
        <div class="fieldwrapper">
            <asp:Label ID="lblNAICSCode" class="styled" runat="server" Text="NAICS code:"></asp:Label>
            <asp:TextBox ID="txtNAICSCode" runat="server" class="editable" Text="" Width="100px"
                MaxLength="6"></asp:TextBox>
            <act:FilteredTextBoxExtender ID="mpeNAICSCode" runat="server" Enabled="True" TargetControlID="txtNAICSCode"
                FilterType="Numbers">
            </act:FilteredTextBoxExtender>
            <asp:RequiredFieldValidator ID="reqvNAICSCode" runat="server" ControlToValidate="txtNAICSCode"
                ErrorMessage="The facility NAICS Code is required">*</asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="rgxvNAICSCode" runat="server" ControlToValidate="txtNAICSCode"
                Display="Dynamic" ErrorMessage="NAICS Code must be 6 digits" ValidationExpression="\d{6}">*</asp:RegularExpressionValidator>
            <asp:CustomValidator ID="CustomValidator1" runat="server" ControlToValidate="txtNAICSCode"
                OnServerValidate="NAICSCheck" ErrorMessage="NAICS Code not valid. Enter another or use the search button.">*</asp:CustomValidator>
            &nbsp;<asp:Button ID="btnNAICSLoopup" runat="server" Text="NAICS Lookup" ToolTip=""
                Font-Size="Small" CausesValidation="False" />
        </div>
        <div class="fieldwrapper">
            <asp:Label ID="lblFacilitySiteComment" runat="server" CssClass="styled" Text="Comment:"></asp:Label>
            <asp:TextBox ID="txtFacilitySiteComment" runat="server" class="editable" TextMode="MultiLine" Rows="4"
                Text="" Width="400px"></asp:TextBox>
        </div>
        <br />
        <asp:UpdatePanel ID="upLatLon" runat="server">
            <Triggers>
                <asp:PostBackTrigger ControlID="lbtnGetLatLon" />
            </Triggers>
            <ContentTemplate>
                <div class="fieldwrapperseparator">
                    <p class="styledseparator">Facility Geographic Coordinate Information:</p>
                    <p class="label">Facility latitude/longitude must be located at the center of the production area.</p>
                    <p class="label">
                        <em>Geographic information updates must be reviewed by APB staff.</em>
                        If the existing values are incorrect, enter your corrections below and include a comment in the 
                        Comment box explaining your changes. Facility data will not be modified in the EIS system until 
                        approved by APB staff. Please use the &quot;Contact Us&quot; menu item above if you have questions.
                    </p>
                </div>
                <div class="fieldwrapper">
                    <asp:Label ID="LblLatitudeMeasure" class="styled" runat="server" Text="Latitude:"></asp:Label>
                    <asp:TextBox ID="TxtLatitudeMeasure" runat="server" class="editable" Text="" Width="100px"
                        MaxLength="8"></asp:TextBox>
                    <act:FilteredTextBoxExtender ID="filtxtLatitudeMeasure" runat="server" Enabled="True"
                        FilterType="Custom, Numbers" TargetControlID="TxtLatitudeMeasure" ValidChars=".">
                    </act:FilteredTextBoxExtender>
                    <asp:RequiredFieldValidator ID="reqvLatitudeMeasure" ControlToValidate="TxtLatitudeMeasure"
                        runat="server" ErrorMessage="The facility latitude is required." Display="Dynamic">*</asp:RequiredFieldValidator>
                    <asp:RangeValidator ID="rngvLatitudeMeasure" runat="server" ControlToValidate="TxtLatitudeMeasure"
                        MaximumValue="35.00028" MinimumValue="30.35944" Type="Double"
                        ErrorMessage="The facility latitiude must be between 30.35944° and 35.200028°."
                        Display="Dynamic">Must be between 30.35944° and 35.200028°</asp:RangeValidator>
                </div>
                <div class="fieldwrapper">
                    <asp:Label ID="LblLongitudeMeasure" class="styled" runat="server" Text="Longitude:"></asp:Label>
                    <asp:TextBox ID="TxtLongitudeMeasure" runat="server" class="editable" Text="" Width="100px"></asp:TextBox>
                    <act:FilteredTextBoxExtender ID="filtxtLongitudeMeasure" runat="server" Enabled="True"
                        FilterType="Custom, Numbers" TargetControlID="TxtLongitudeMeasure" ValidChars=".-">
                    </act:FilteredTextBoxExtender>
                    <asp:RequiredFieldValidator ID="reqvLongitudeMeasure" ControlToValidate="TxtLongitudeMeasure"
                        runat="server" ErrorMessage="The facility longitude is required.">*</asp:RequiredFieldValidator>
                    <asp:RangeValidator ID="rngvLongitudeMeasure" runat="server" ControlToValidate="TxtLongitudeMeasure"
                        MinimumValue="-85.60889" MaximumValue="-80.84417" type="Double"
                        ErrorMessage="The facility longitude must be between -85.60889° and -80.84417°.">
                        Must be between -85.60889° and -80.84417°</asp:RangeValidator>
                </div>
                <div class="fieldwrapper">
                    <asp:Label class="styled" runat="server" Text="Map:"></asp:Label>
                    <div style="display: inline-block; width: 610px">
                        <asp:HyperLink ID="lnkGoogleMap" runat="server" Text="View in Google Maps" Target="_blank">
                            <asp:Image ID="imgGoogleStaticMap" runat="server" />
                        </asp:HyperLink><br />
                        <asp:Panel ID="pnlLocationMap" runat="server" Width="610px"></asp:Panel>
                        <p>
                            The facility latitude/longitude is centered in the map above. If the location is incorrect, 
                        use the &quot;Pick Latitude/Longitude&quot; button to select the correct location.
                        </p>
                        <asp:Button ID="lbtnGetLatLon" runat="server" CausesValidation="false" Text="Pick Latitude/Longitude" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="fieldwrapper">
            <asp:Label ID="LblHorCollectionMetCode" CssClass="styled" runat="server" Text="Horizontal Collection Method:"></asp:Label>
            <asp:DropDownList ID="ddlHorCollectionMetCode" runat="server" Width="600px"></asp:DropDownList>
            <asp:RequiredFieldValidator ID="reqvHorCollectionMetCode" ControlToValidate="ddlHorCollectionMetCode"
                runat="server" ErrorMessage="The facility horizontal collection method is required."
                InitialValue="--Select Horizontal Collection Method--" Display="Dynamic">*</asp:RequiredFieldValidator>
        </div>
        <div class="fieldwrapper">
            <asp:Label ID="LblHorizontalAccuracyMeasure" class="styled" runat="server" Text="Accuracy Measure (m):"></asp:Label>
            <asp:TextBox ID="TxtHorizontalAccuracyMeasure" runat="server" class="editable" Text="" Width="100px"></asp:TextBox>
            <act:FilteredTextBoxExtender ID="filtxtHorizontalAccuracyMeasure" runat="server"
                Enabled="True" FilterType="Numbers" TargetControlID="TxtHorizontalAccuracyMeasure">
            </act:FilteredTextBoxExtender>
            <asp:RequiredFieldValidator ID="reqvHorizontalAccuracyMeasure" ControlToValidate="TxtHorizontalAccuracyMeasure"
                runat="server" ErrorMessage="The facility accuracy measure is required." Display="Dynamic">*</asp:RequiredFieldValidator>
            <asp:RangeValidator ID="rngvHorizontalAccuracyMeasure" runat="server" ControlToValidate="TxtHorizontalAccuracyMeasure"
                ErrorMessage="The facility accuracy measure must be between 1 and 2000 meters."
                MaximumValue="2000" MinimumValue="1" Display="Dynamic" Type="Integer">Must be between 1 and 2000</asp:RangeValidator>
        </div>
        <div class="fieldwrapper">
            <asp:Label ID="lblHorReferenceDatCode" CssClass="styled" runat="server" Text="Horizontal Reference Datum:"></asp:Label>
            <asp:DropDownList ID="ddlHorReferenceDatCode" runat="server"></asp:DropDownList>
            <asp:RequiredFieldValidator ID="reqvHorReferenceDatCode" ControlToValidate="ddlHorReferenceDatCode"
                runat="server" ErrorMessage="The facility horizontal reference datum is required."
                InitialValue="--Select Horizontal Reference Datum--" Display="Dynamic">*</asp:RequiredFieldValidator>
        </div>
        <div class="fieldwrapper">
            <asp:Label ID="lblGeographicComment" runat="server" CssClass="styled" Text="Comment:"></asp:Label>
            <div style="display: inline-block">
                <asp:TextBox ID="txtGeographicComment" runat="server" class="editable" TextMode="MultiLine" Rows="4" onKeyUp="javascript:Count(this);"
                    Text="" Width="400px"></asp:TextBox>
                <div id="dvComment" style="font: bold"></div>
                <asp:RegularExpressionValidator ID="regexpName" runat="server"
                    ErrorMessage="Comment must not exceed 400 characters."
                    ControlToValidate="txtGeographicComment"
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
        <br />
        <div class="fieldwrapperseparator">
            <asp:Label ID="lblSeparator5" class="styledseparator" runat="server" Text="Emission Inventory Contact Information"></asp:Label>
            <asp:Label ID="Label1" CssClass="label" runat="server" Text="The Emission Inventory contact
            will receive notices regarding annual Emission Inventory submittals for the facility."></asp:Label>
        </div>
        <div class="fieldwrapper">
            <asp:Label ID="lblNamePrefix" class="styled" runat="server" Text="Honorific:"></asp:Label>
            <asp:TextBox ID="txtPrefix" runat="server" class="editable" Text="" Width="74px" MaxLength="15"></asp:TextBox>
            <act:TextBoxWatermarkExtender ID="tbwPrefix" runat="server" Enabled="True" TargetControlID="txtPrefix" WatermarkCssClass="watermarked" WatermarkText="OPTIONAL">
            </act:TextBoxWatermarkExtender>
        </div>
        <div class="fieldwrapper">
            <asp:Label ID="lblfirstname" class="styled" runat="server" Text="First Name:"></asp:Label>
            <asp:TextBox ID="txtFirstName" runat="server" class="editable" Text="" Width="200px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="reqvFirstName" runat="server" ControlToValidate="txtFirstName"
                ErrorMessage="The Emission Inventory contact first name is required.">*</asp:RequiredFieldValidator>
        </div>
        <div class="fieldwrapper">
            <asp:Label ID="lblLastName" class="styled" runat="server" Text="Last Name:"></asp:Label>
            <asp:TextBox ID="txtLastName" runat="server" class="editable" Text="" Width="200px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="reqvLastName" runat="server" ControlToValidate="txtLastName"
                ErrorMessage="The Emission Inventory contact last name is required.">*</asp:RequiredFieldValidator>
        </div>
        <div class="fieldwrapper">
            <asp:Label ID="lblIndividualTitleText" class="styled" runat="server" Text="Individual Title: "></asp:Label>
            <asp:TextBox ID="txtIndividualTitleText" runat="server" class="editable" Text=""
                Width="200px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="reqvIndividualTitleText" runat="server" ControlToValidate="txtIndividualTitleText"
                ErrorMessage="The Emission Inventory contact title is required.">*</asp:RequiredFieldValidator>
        </div>
        <div class="fieldwrapper">
            <asp:Label ID="lblMailingAddressText_Contact" class="styled" runat="server" Text="Contact Mailing Address:"></asp:Label>
            <asp:TextBox ID="txtMailingAddressText_Contact" runat="server" class="editable" Text=""></asp:TextBox>
            <asp:RequiredFieldValidator ID="reqvMailingAddressText_Contact" runat="server" ControlToValidate="txtMailingAddressText_Contact"
                ErrorMessage="The Emission Inventory contact mailing address is required.">*</asp:RequiredFieldValidator>
        </div>
        <div class="fieldwrapper">
            <asp:Label ID="lblSupplementalAddressText_Contact" class="styled_noline" runat="server"
                Text=""></asp:Label>
            <asp:TextBox ID="txtSupplementalAddressText_Contact" runat="server" class="editable"
                Text=""></asp:TextBox>
            <act:TextBoxWatermarkExtender ID="tbwSupplementalAddressText_Contact" runat="server"
                Enabled="True" TargetControlID="txtSupplementalAddressText_Contact" WatermarkText="OPTIONAL"
                WatermarkCssClass="watermarked">
            </act:TextBoxWatermarkExtender>
        </div>
        <div class="fieldwrapper">
            <asp:Label ID="lblMailingAddressCityName_Contact" class="styled" runat="server" Text="City:"></asp:Label>
            <asp:TextBox ID="txtMailingAddressCityName_Contact" runat="server" class="editable"
                Text="" Width="200px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="reqvMailingAddressCityName_Contact" runat="server"
                ControlToValidate="txtMailingAddressCityName_Contact" ErrorMessage="The Emission Inventory contact city is required.">*</asp:RequiredFieldValidator>
        </div>
        <div class="fieldwrapper">
            <asp:Label ID="lblMailingAddressStateCode_Contact" class="styled" runat="server"
                Text="State:"></asp:Label>
            <asp:DropDownList ID="ddlFacility_StateMail" runat="server">
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="reqvMailingAddressStateCode_Contact" ControlToValidate="ddlFacility_StateMail"
                InitialValue="--Select a State--" runat="server" ValidationGroup="vgStack" ErrorMessage="The Emission Inventory contact state is required."
                Display="Dynamic">*</asp:RequiredFieldValidator>
        </div>
        <div class="fieldwrapper">
            <asp:Label ID="lblMailingAddressPostalCode_Contact" class="styled" runat="server"
                Text="Zip Code:"></asp:Label>
            <asp:TextBox ID="txtMailingAddressPostalCode_Contact" runat="server" class="editable"
                Text="" Width="150px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="reqvMailingAddressPostalCode_Contact" runat="server"
                ControlToValidate="txtMailingAddressPostalCode_Contact" ErrorMessage="The Emission Inventory contact zip code is required.">*</asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="rgxvMailingAddressPostalCode_Contact" runat="server"
                ControlToValidate="txtMailingAddressPostalCode_Contact" ErrorMessage="Please check the contact mailing address zip code format."
                ValidationExpression="^(\d{5})(-\d{4})?$" Display="Dynamic">Format must be either 99999 or 99999-9999</asp:RegularExpressionValidator>
            <act:FilteredTextBoxExtender ID="filtxtMailingAddressPostalCode_Contact" runat="server"
                Enabled="True" TargetControlID="txtMailingAddressPostalCode_Contact" FilterType="Numbers, Custom"
                ValidChars="-">
            </act:FilteredTextBoxExtender>
        </div>
        <div class="fieldwrapper">
            <asp:Label ID="lblTelephoneNumberText" class="styled" runat="server" Text="Contact Phone Number: "></asp:Label>
            <asp:TextBox ID="txtTelephoneNumberText" runat="server" class="editable" Text=""
                Width="150px" MaxLength="10"></asp:TextBox>
            <act:FilteredTextBoxExtender ID="txtTelephoneNumberText_FilteredTextBoxExtender"
                runat="server" Enabled="True" TargetControlID="txtTelephoneNumberText"
                FilterType="Numbers">
            </act:FilteredTextBoxExtender>

            <asp:RequiredFieldValidator ID="reqvTelephoneNumberText" runat="server" ControlToValidate="txtTelephoneNumberText"
                ErrorMessage="The Emission Inventory contact phone number is required.">*</asp:RequiredFieldValidator>
        </div>
        <div class="fieldwrapper">
            <asp:Label ID="lblTelephoneExtensionNumberText" class="styled" runat="server" Text="Extension: "></asp:Label>
            <asp:TextBox ID="txtTelephoneExtensionNumberText" runat="server" class="editable"
                Text="" Width="150px" MaxLength="10"></asp:TextBox>
            <act:TextBoxWatermarkExtender ID="tbwTelephoneExtensionNumberText" runat="server"
                Enabled="True" TargetControlID="txtTelephoneExtensionNumberText" WatermarkText="OPTIONAL"
                WatermarkCssClass="watermarked">
            </act:TextBoxWatermarkExtender>
        </div>
        <div class="fieldwrapper">
            <asp:Label ID="lblTelephoneNumberText_mobile" class="styled" runat="server" Text="Contact Mobile Number: "></asp:Label>
            <asp:TextBox ID="txtTelephoneNumber_Mobile" runat="server" class="editable" Text=""
                Width="150px" MaxLength="10"></asp:TextBox>
            <act:FilteredTextBoxExtender ID="txtTelephoneNumber_Mobile_FilteredTextBoxExtender"
                runat="server" Enabled="True" FilterType="Numbers"
                TargetControlID="txtTelephoneNumber_Mobile">
            </act:FilteredTextBoxExtender>
        </div>
        <div class="fieldwrapper">
            <asp:Label ID="lblTelephoneNumberText_fax" class="styled" runat="server" Text="Contact Fax Number: "></asp:Label>
            <asp:TextBox ID="txtTelephoneNumber_Fax" runat="server" class="editable" Text=""
                Width="150px" MaxLength="10"></asp:TextBox>
            <act:FilteredTextBoxExtender ID="txtTelephoneNumber_Fax_FilteredTextBoxExtender"
                runat="server" Enabled="True" FilterType="Numbers"
                TargetControlID="txtTelephoneNumber_Fax">
            </act:FilteredTextBoxExtender>
        </div>
        <div class="fieldwrapper">
            <asp:Label ID="lblElectronicAddress" class="styled" runat="server" Text="Email: "></asp:Label>
            <asp:TextBox ID="txtElectronicAddressText" runat="server" class="editable" Text=""></asp:TextBox>
            <asp:RequiredFieldValidator ID="reqvElectronicAddressText" runat="server" ControlToValidate="txtElectronicAddressText"
                ErrorMessage="The Emission Inventory contact email address is required.">*</asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="rgxvElectronicAddress" runat="server"
                ControlToValidate="txtElectronicAddressText"
                ErrorMessage="Email address not valid."
                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
        </div>
        <div class="fieldwrapper">
            <asp:Label ID="lblContactAddressComment_Contact" runat="server" CssClass="styled"
                Text="Comment:"></asp:Label>
            <asp:TextBox ID="txtAddressComment_Contact" runat="server" class="editable" TextMode="MultiLine" Rows="4"
                Text="" Width="400px"></asp:TextBox>
        </div>
        <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>--%>
        <div class="buttonwrapper">
            <asp:Button runat="server" ID="btnSave2" CssClass="buttondiv" Text="Save" Width="75px" />
            <asp:Button runat="server" ID="btnCancel" CssClass="buttondiv" Text="Return to Details"
                CausesValidation="False" PostBackUrl="~/eis/facility_details.aspx" /><br />
            <asp:Label ID="lblFacilityMessage2" runat="server" Font-Bold="True" Font-Names="Arial"
                Font-Size="Small" ForeColor="Red"></asp:Label>
        </div>
        <%--</ContentTemplate>
        </asp:UpdatePanel>--%>
    </asp:Panel>
    <asp:Panel ID="pnlNAICSCodeLookup" BackColor="#ffffff" runat="server" BorderColor="#333399"
        BorderStyle="Ridge" ScrollBars="Vertical" Width="100%">
        <table border="0" cellpadding="2" cellspacing="1" width="100%">
            <tr>
                <td align="center" colspan="2">
                    <strong><span style="color: #4169e1; font-size: 12pt; font-family: Verdana;">NAICS Code
                        Lookup</span></strong>
                </td>
            </tr>
            <tr>
                <td align="right" width="30%"></td>
                <td>&nbsp;
                </td>
            </tr>
            <tr>
                <td align="right" width="30%" valign="top">&nbsp;<asp:Label ID="lbllookupNAICSCode" runat="server" CssClass="label" Text="NAICS Code:"></asp:Label>
                </td>
                <td valign="top">
                    <asp:TextBox ID="txtLookupNAICSCode" runat="server" Font-Names="Arial" Font-Size="Small"
                        MaxLength="10"></asp:TextBox>
                    <act:FilteredTextBoxExtender ID="txtLookupNAICSCode_FilteredTextBoxExtender" runat="server"
                        Enabled="True" FilterType="Numbers" TargetControlID="txtLookupNAICSCode">
                    </act:FilteredTextBoxExtender>
                </td>
            </tr>
            <tr>
                <td align="right" valign="top" width="30%">&nbsp;
                </td>
            </tr>
            <tr>
                <td align="right" width="30%" valign="top">&nbsp;<asp:Label ID="lbllookupNAICSCodeDesc" runat="server" CssClass="label" Text="NAICS Code Desc:"></asp:Label>
                </td>
                <td valign="top">
                    <asp:TextBox ID="txtLookupNAICSDesc" runat="server" Font-Names="Arial" Font-Size="Small"
                        MaxLength="50" Width="200px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <asp:Button ID="btnSearchNAICS" runat="server" Text="Search" CausesValidation="False" />
                    &nbsp; &nbsp;
                    <asp:Button ID="btnCancelNAICS" runat="server" Text="Cancel" CausesValidation="False" />
                </td>
            </tr>
            <tr>
                <td align="right" valign="top" width="30%">&nbsp;
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <asp:Button ID="btnUseNAICSCode" runat="server" Text="Use NAICS Code"
                        Width="135px" ValidationGroup="vgNAICS" />
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2" class="style4">&nbsp;<asp:Label ID="lblNAICSMessage" runat="server" CssClass="label" Text="After clicking this button, the NAICS code will be prefilled with the selection."></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right" width="30%" valign="top">&nbsp;<asp:Label ID="lblSelectedNAICSCode" runat="server" CssClass="label" Text="Selected NAICS Code:"></asp:Label>
                </td>
                <td valign="top">
                    <asp:TextBox ID="txtSelectedNAICSCode" runat="server" Font-Names="Arial" Font-Size="Small"
                        MaxLength="10" ReadOnly="True" ValidationGroup="vgNAICS"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="reqvNAICS" runat="server"
                        ControlToValidate="txtSelectedNAICSCode"
                        ErrorMessage="*Search and select a code or select Cancel to enter code manually."
                        ValidationGroup="vgNAICS"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">&nbsp;&nbsp;&nbsp;
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <asp:Label ID="lblRowCount" runat="server" Font-Bold="True"></asp:Label>
                </td>
            </tr>
        </table>
        <div class="gridview">
            <asp:GridView ID="gvwNAICS" runat="server" CellPadding="4" DataKeyNames="NAICSCode"
                GridLines="None" AutoGenerateColumns="False"
                AllowPaging="True" PageSize="50" Width="90%">
                <PagerSettings PageButtonCount="20" Position="TopAndBottom" />
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <EditRowStyle BackColor="#999999" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                <Columns>
                    <asp:CommandField ShowSelectButton="True"></asp:CommandField>
                    <asp:BoundField DataField="NAICSCode" HeaderText="NAICS Code" SortExpression="NAICSCode">
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="strDesc" HeaderText="Description" SortExpression="strDesc">
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                </Columns>
            </asp:GridView>
        </div>
    </asp:Panel>
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
