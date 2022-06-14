<%@ Page Title="GECO - Emissions Statement Entry" Language="VB" MasterPageFile="~/Main.master"
    AutoEventWireup="false" Inherits="GECO.es_form" CodeBehind="Form.aspx.vb" %>

<%@ MasterType VirtualPath="~/Main.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">

    <h1>Emissions Statement</h1>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <asp:Panel ID="pnlFacility" runat="server">
                <asp:MultiView ID="mltiViewESFacility" runat="server" ActiveViewIndex="0">

                    <asp:View ID="ViewESFacilityLocation" runat="server">

                        <h2>Facility Location</h2>

                        <div class="label-highlight-mild">
                            <p>
                                Facility Location cannot be changed. The address shown is for the facility's physical street address.
                                If errors exist for the facility name or location, contact the Air Protection Branch Permitting Program.
                            </p>
                        </div>

                        <table class="table-simple table-list">
                            <tr>
                                <th>Facility Name:</th>
                                <td>
                                    <asp:Label ID="txtFacilityName" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <th>Street Address:</th>
                                <td>
                                    <asp:Label ID="txtLocationAddress" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <th>City:</th>
                                <td>
                                    <asp:Label ID="txtCity" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <th>State:</th>
                                <td>
                                    <asp:Label ID="txtState" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <th>Zip:</th>
                                <td>
                                    <asp:Label ID="txtZipCode" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <th>County:</th>
                                <td>
                                    <asp:Label ID="txtCounty" runat="server" />
                                </td>
                            </tr>
                        </table>

                        <h2>Geocoordinates</h2>

                        <table class="table-simple table-list">
                            <tr>
                                <th></th>
                                <td>
                                    <asp:Button ID="btnLatLongConvert" runat="server" CausesValidation="False" CssClass="button-tool"
                                        Text="Convert from deg-min-sec" />
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    <asp:Label AssociatedControlID="txtYCoordinate" runat="server">Latitude:</asp:Label>
                                </th>
                                <td>
                                    <asp:TextBox ID="txtYCoordinate" runat="server" MaxLength="9" CssClass="input-small" />
                                    degrees North
                                </td>
                            </tr>
                            <tr>
                                <th></th>
                                <td>
                                    <act:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server"
                                        EnableViewState="False" FilterType="Custom, Numbers" TargetControlID="txtYCoordinate"
                                        ValidChars="." />
                                    <asp:RequiredFieldValidator ID="reqValYCoordinate" runat="server" ControlToValidate="txtYCoordinate"
                                        CssClass="validator" Display="Dynamic" Width="100%" ErrorMessage="Latitude is required." />
                                    <asp:RangeValidator ID="rngValYCoordinate" runat="server" ControlToValidate="txtYCoordinate"
                                        CssClass="validator" Width="100%" ErrorMessage="Latitude is outside county limits."
                                        Type="Double" Display="Dynamic" />
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    <asp:Label AssociatedControlID="txtXCoordinate" runat="server">Longitude:</asp:Label>
                                </th>
                                <td>
                                    <asp:TextBox ID="txtXCoordinate" runat="server" MaxLength="9" CssClass="input-small" />
                                    degrees West
                                </td>
                            </tr>
                            <tr>
                                <th></th>
                                <td>
                                    <act:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server"
                                        FilterType="Custom, Numbers" TargetControlID="txtXCoordinate" ValidChars="." />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtXCoordinate"
                                        CssClass="validator" Display="Dynamic" Width="100%" ErrorMessage="Longitude is required." />
                                    <asp:RangeValidator ID="rngValXCoordinate" runat="server" ControlToValidate="txtXCoordinate"
                                        CssClass="validator" Width="100%" ErrorMessage="Longitude is outside county limits."
                                        Type="Double" Display="Dynamic" />
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    <asp:Label AssociatedControlID="cboHorizontalCollectionCode" runat="server">Horizontal Collection Method:</asp:Label>
                                </th>
                                <td>
                                    <asp:DropDownList ID="cboHorizontalCollectionCode" runat="server" Width="90%" />
                                </td>
                            </tr>
                            <tr>
                                <th></th>
                                <td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="cboHorizontalCollectionCode"
                                        Display="Dynamic" Width="100%" ErrorMessage="Horizontal collection method is required."
                                        InitialValue=" --Select a Method-- " CssClass="validator" />
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    <asp:Label AssociatedControlID="txtHorizontalAccuracyMeasure" runat="server">Horizontal Accuracy Measure:</asp:Label>
                                </th>
                                <td>
                                    <asp:TextBox ID="txtHorizontalAccuracyMeasure" runat="server" MaxLength="6" CssClass="input-small" />
                                    meters
                                </td>
                            </tr>
                            <tr>
                                <th></th>
                                <td>
                                    <act:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server"
                                        EnableViewState="False" FilterType="Custom, Numbers" TargetControlID="txtHorizontalAccuracyMeasure"
                                        ValidChars="." />
                                    <asp:RangeValidator ID="RangeValidator7" runat="server" ControlToValidate="txtHorizontalAccuracyMeasure"
                                        Width="100%" ErrorMessage="Horizontal accuracy measure must be between 0.01 and 1000."
                                        MaximumValue="1000" MinimumValue="0.01" Type="Double" CssClass="validator" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="txtHorizontalAccuracyMeasure"
                                        CssClass="validator" Display="Dynamic" Width="100%" ErrorMessage="Horizontal accuracy measure is required." />
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    <asp:Label AssociatedControlID="cboHorizontalReferenceCode" runat="server">Horizontal Datum Reference Code:</asp:Label>
                                </th>
                                <td>
                                    <asp:DropDownList ID="cboHorizontalReferenceCode" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <th></th>
                                <td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="cboHorizontalReferenceCode"
                                        Display="Dynamic" Width="100%" ErrorMessage="Horizontal datum reference is required."
                                        InitialValue=" --Select a Code-- " CssClass="validator" />
                                </td>
                            </tr>
                        </table>

                        <p>
                            <asp:Button ID="btnContinueToContact" runat="server" Text="Continue to Contact Information" CssClass="button-large" />
                            <asp:Button ID="btnCancelLocation" runat="server" Text="Cancel" CausesValidation="False" CssClass="button-cancel" />
                        </p>
                    </asp:View>

                    <asp:View ID="ViewESFacilityContact" runat="server">
                        <h2>Contact Information</h2>
                        <p class="label-highlight-mild">Verify the information for the person to contact regarding the Emissions Statement.</p>

                        <table class="table-simple table-list">
                            <tr>
                                <th>
                                    <asp:Label AssociatedControlID="txtContactPrefix" runat="server">Prefix:</asp:Label>
                                </th>
                                <td>
                                    <asp:TextBox ID="txtContactPrefix" runat="server" MaxLength="15" />
                                    (optional)
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    <asp:Label AssociatedControlID="txtContactFirstName" runat="server">First Name:</asp:Label>
                                </th>
                                <td>
                                    <asp:TextBox ID="txtContactFirstName" runat="server" MaxLength="50" />
                                </td>
                            </tr>
                            <tr>
                                <th></th>
                                <td>
                                    <asp:RequiredFieldValidator ID="reqValFirstName" runat="server" ControlToValidate="txtContactFirstName"
                                        Display="Dynamic" CssClass="validator" ErrorMessage="First name is required." />
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    <asp:Label AssociatedControlID="txtContactLastName" runat="server">Last Name:</asp:Label>
                                </th>
                                <td>
                                    <asp:TextBox ID="txtContactLastName" runat="server" MaxLength="50" />
                                </td>
                            </tr>
                            <tr>
                                <th></th>
                                <td>
                                    <asp:RequiredFieldValidator ID="reqValLastName" runat="server" ControlToValidate="txtContactLastName"
                                        Display="Dynamic" Width="100%" CssClass="validator" ErrorMessage="Last name is required." />
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    <asp:Label AssociatedControlID="txtContactTitle" runat="server">Title/Position:</asp:Label>
                                </th>
                                <td>
                                    <asp:TextBox ID="txtContactTitle" runat="server" MaxLength="100" />
                                </td>
                            </tr>
                            <tr>
                                <th></th>
                                <td>
                                    <asp:RequiredFieldValidator ID="reqValTitle" runat="server" ControlToValidate="txtContactTitle"
                                        Display="Dynamic" Width="100%" CssClass="validator" ErrorMessage="Contact title/position is required." />
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    <asp:Label AssociatedControlID="txtContactEmail" runat="server">Email Address:</asp:Label>
                                </th>
                                <td>
                                    <asp:TextBox ID="txtContactEmail" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <th></th>
                                <td>
                                    <asp:RequiredFieldValidator ID="reqvalEmail1" runat="server" ControlToValidate="txtContactEmail"
                                        Display="Dynamic" CssClass="validator" ErrorMessage="Email address is required." Width="100%" />
                                    <asp:RegularExpressionValidator ID="regExpValEmail1" runat="server" ControlToValidate="txtContactEmail"
                                        ErrorMessage="Email address format is incorrect."
                                        ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" Display="Dynamic"
                                        Width="100%" CssClass="validator" />
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    <asp:Label AssociatedControlID="txtOfficePhoneNbr" runat="server">Phone:</asp:Label>
                                </th>
                                <td>
                                    <asp:TextBox ID="txtOfficePhoneNbr" runat="server" MaxLength="30" />
                                </td>
                            </tr>
                            <tr>
                                <th></th>
                                <td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtOfficePhoneNbr"
                                        Display="Dynamic" CssClass="validator" ErrorMessage="Phone number is required." Width="100%" />
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    <asp:Label AssociatedControlID="txtFaxNbr" runat="server">Fax:</asp:Label>
                                </th>
                                <td>
                                    <asp:TextBox ID="txtFaxNbr" runat="server" MaxLength="15" />
                                    (optional)
                                </td>
                            </tr>
                            <tr>
                                <th></th>
                                <td>
                                    <act:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" runat="server"
                                        FilterType="Numbers" TargetControlID="txtFaxNbr" />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtFaxNbr"
                                        Display="Dynamic" CssClass="validator" ValidationExpression="\d{10}"
                                        ErrorMessage="Fax number must be 10 digits; no dashes, parentheses or spaces allowed." />
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    <asp:Label AssociatedControlID="txtContactCompanyName" runat="server">Company Name:</asp:Label>
                                </th>
                                <td>
                                    <asp:TextBox ID="txtContactCompanyName" runat="server" MaxLength="100" />
                                </td>
                            </tr>
                            <tr>
                                <th></th>
                                <td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server"
                                        ControlToValidate="txtContactCompanyName" CssClass="validator"
                                        Display="Dynamic" ErrorMessage="Contact company name is required." />
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    <asp:Label AssociatedControlID="txtContactAddress1" runat="server">Mailing Address:</asp:Label>
                                </th>
                                <td>
                                    <asp:TextBox ID="txtContactAddress1" runat="server" MaxLength="100" />
                                </td>
                            </tr>
                            <tr>
                                <th></th>
                                <td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server"
                                        ControlToValidate="txtContactAddress1" CssClass="validator"
                                        Display="Dynamic" ErrorMessage="Mailing address is required." />
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    <asp:Label AssociatedControlID="txtContactCity" runat="server">City:</asp:Label>
                                </th>
                                <td>
                                    <asp:TextBox ID="txtContactCity" runat="server" MaxLength="50" />
                                </td>
                            </tr>
                            <tr>
                                <th></th>
                                <td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server"
                                        ControlToValidate="txtContactCity" CssClass="validator"
                                        Display="Dynamic" ErrorMessage="City is required." />
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    <asp:Label AssociatedControlID="cboContactState" runat="server">State:</asp:Label>
                                </th>
                                <td>
                                    <asp:DropDownList ID="cboContactState" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <th></th>
                                <td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server"
                                        ControlToValidate="cboContactState" CssClass="validator" Display="Dynamic"
                                        ErrorMessage="Select state for contact mailing address." InitialValue=" -- " />
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    <asp:Label AssociatedControlID="txtContactZipCode" runat="server">Zip:</asp:Label>
                                </th>
                                <td>
                                    <asp:TextBox ID="txtContactZipCode" runat="server" MaxLength="5" CssClass="input-small" />
                                    -
                                    <asp:TextBox ID="txtContactZipPlus4" runat="server" MaxLength="4" CssClass="input-small" />
                                    (plus4 is optional)
                                </td>
                            </tr>
                            <tr>
                                <th></th>
                                <td>
                                    <act:FilteredTextBoxExtender ID="FilteredTextBoxExtender17" runat="server"
                                        FilterType="Numbers" TargetControlID="txtContactZipCode" />
                                    <act:FilteredTextBoxExtender ID="FilteredTextBoxExtender18" runat="server"
                                        FilterType="Numbers" TargetControlID="txtContactZipPlus4" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server"
                                        ControlToValidate="txtContactZipCode" CssClass="validator"
                                        Display="Dynamic" ErrorMessage="Zip code is required." Width="100%" />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server"
                                        ControlToValidate="txtContactZipCode" CssClass="validator"
                                        Display="Dynamic" ErrorMessage="Zip code must be 5 digits."
                                        ValidationExpression="\d{5}" Width="100%" />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server"
                                        ControlToValidate="txtContactZipPlus4" CssClass="validator"
                                        Display="Dynamic" ErrorMessage="Zip Plus4 must be 4 digits."
                                        ValidationExpression="\d{4}" Width="100%" />
                                </td>
                            </tr>
                        </table>

                        <p>
                            <asp:Button ID="btnContinueToEmissions" runat="server" CssClass="button-large"
                                Text="Continue to Emissions Information" />
                            <asp:Button ID="btnbackToLocation" runat="server" Text="Back" CausesValidation="False" />
                            <asp:Button ID="btnCancelContact" runat="server" Text="Cancel" CausesValidation="False" CssClass="button-cancel" />
                        </p>
                    </asp:View>

                    <asp:View ID="ViewESFacilityDescription" runat="server">
                        <h2>Facility-wide VOC and NO<sub>x</sub> Emissions Information</h2>

                        <p>
                            <asp:Label AssociatedControlID="cboYesNo" runat="server">
                                Were the facility's actual annual emissions of VOC and NO<sub>x</sub> both 
                                <em>less than or equal to</em> 25&nbsp;tons&nbsp;per&nbsp;year in <% = Now.Year - 1 %>?
                            </asp:Label>
                        </p>

                        <div>
                            <asp:DropDownList ID="cboYesNo" runat="server" AutoPostBack="True">
                                <asp:ListItem Selected="True">--</asp:ListItem>
                                <asp:ListItem>NO</asp:ListItem>
                                <asp:ListItem>YES</asp:ListItem>
                            </asp:DropDownList>

                            <asp:RequiredFieldValidator ID="reqValYesNo" runat="server" ControlToValidate="cboYesNo"
                                Display="Dynamic" ErrorMessage="Select Yes or No." CssClass="validator"
                                InitialValue="--" />
                        </div>

                        <p class="label-highlight-mild" id="pEmissionsHelpYes" runat="server">
                            A response of "YES" in the drop-down box indicates that your facility's actual emissions of both 
                            NO<sub>x</sub> and VOC are 25&nbsp;tons&nbsp;per&nbsp;year or less and will cause your facility to opt out 
                            of the Emissions Statement process.
                        </p>

                        <p class="label-highlight-mild" id="pEmissionsHelpNo" runat="server">
                            Choosing "NO" opts the facility into the Emissions Statement process. You must provide the actual 
                            emission quantities for both NO<sub>x</sub> and VOC.
                        </p>

                        <asp:Panel ID="pnlEmissions" runat="server" Visible="False">
                            <table class="table-simple table-list">
                                <tr>
                                    <th>Actual annual facility-wide VOC emissions:</th>
                                    <td>
                                        <asp:TextBox ID="txtVOC" runat="server" MaxLength="7" CssClass="input-small">0</asp:TextBox>
                                        tons/year
                                    </td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <td>
                                        <act:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Numbers, Custom"
                                            ValidChars="." TargetControlID="txtVOC" />
                                        <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtVOC"
                                            CssClass="validator" Display="Dynamic" ErrorMessage="Maximum VOC emissions is 99,999 tons."
                                            MaximumValue="99999" MinimumValue="0" Type="Double" Width="100%" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtVOC"
                                            Display="Dynamic" ErrorMessage="VOC quantity is required." CssClass="validator" Width="100%" />
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <th>Actual annual facility-wide NO<sub>x</sub> emissions:</th>
                                    <td>
                                        <asp:TextBox ID="txtNOx" runat="server" MaxLength="7" CssClass="input-small">0</asp:TextBox>
                                        tons/year
                                    </td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <td>
                                        <act:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterType="Numbers, Custom"
                                            ValidChars="." TargetControlID="txtNOx" />
                                        <asp:RangeValidator ID="RangeValidator9" runat="server" ControlToValidate="txtNOx"
                                            CssClass="validator" Display="Dynamic" ErrorMessage="Maximum NOx emissions is 99,999 tons."
                                            MaximumValue="99999" MinimumValue="0" Type="Double" Width="100%" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtNOx"
                                            Display="Dynamic" ErrorMessage="NOx quantity is required." Width="100%" CssClass="validator" />
                                    </td>
                                </tr>
                            </table>

                            <asp:Label ID="lblVOCNOXZero" runat="server" CssClass="validator" Visible="false">
                                Either VOC or NO<sub>x</sub> must be greater than 25 tons/year.
                            </asp:Label>
                        </asp:Panel>

                        <p>
                            <asp:Button ID="btnSave" runat="server" CssClass="button-large button-proceed"
                                Text="Submit Emissions Statement" />
                            <asp:Button ID="btnBackToContactInfo" runat="server" Text="Back" CausesValidation="False" />
                            <asp:Button ID="btnCancelEmission" runat="server" Text="Cancel" CausesValidation="False" CssClass="button-cancel" />
                        </p>

                        <asp:Button ID="btnContinue" runat="server" Text="Continue to Confirmation Page"
                            CausesValidation="False" Visible="False" />
                    </asp:View>

                </asp:MultiView>
            </asp:Panel>

            <asp:Panel ID="pnlLatLongConvert" runat="server">
                <h2>Longitude/Latitude Converter</h2>
                <p>This tool converts longitude and latitude from "degrees-minutes-seconds" format to decimal format.</p>

                <table class="table-simple table-list">
                    <tr>
                        <th>Longitude:</th>
                        <td>
                            <asp:TextBox ID="txtLonDeg" runat="server" MaxLength="2" CssClass="input-small" />deg &nbsp;
                    <asp:TextBox ID="txtLonMin" runat="server" MaxLength="2" CssClass="input-small" />min &nbsp;
                    <asp:TextBox ID="txtLonSec" runat="server" MaxLength="2" CssClass="input-small" />sec
                        </td>
                    </tr>
                    <tr>
                        <th>Latitude:</th>
                        <td>
                            <asp:TextBox ID="txtLatDeg" runat="server" MaxLength="2" CssClass="input-small" />deg &nbsp;
                    <asp:TextBox ID="txtLatMin" runat="server" MaxLength="2" CssClass="input-small" />min &nbsp;
                    <asp:TextBox ID="txtLatSec" runat="server" MaxLength="2" CssClass="input-small" />sec
                        </td>
                    </tr>
                </table>

                <div>
                    <act:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server"
                        TargetControlID="txtLonDeg" FilterType="Numbers" />
                    <act:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server"
                        TargetControlID="txtLonMin" FilterType="Numbers" />
                    <act:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server"
                        TargetControlID="txtLonSec" FilterType="Numbers" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="txtLonDeg"
                        CssClass="validator" Display="Dynamic" Width="100%" ErrorMessage="Longitude degrees is required." />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="txtLonMin"
                        CssClass="validator" Display="Dynamic" Width="100%" ErrorMessage="Longitude minutes is required." />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="txtLonSec"
                        CssClass="validator" Display="Dynamic" Width="100%" ErrorMessage="Longitude seconds is required." />
                    <asp:RangeValidator ID="RangeValidator2" runat="server" ControlToValidate="txtLonDeg"
                        CssClass="validator" Display="Dynamic" Width="100%" MaximumValue="85" MinimumValue="80" Type="Integer"
                        ErrorMessage="Longitude degrees must be an integer between 80 and 85 for Georgia." />
                    <asp:RangeValidator ID="RangeValidator3" runat="server" ControlToValidate="txtLonMin"
                        CssClass="validator" Display="Dynamic" Width="100%" MaximumValue="59" MinimumValue="0" Type="Integer"
                        ErrorMessage="Longitude minutes must be an integer between 0 and 59" />
                    <asp:RangeValidator ID="RangeValidator4" runat="server" ControlToValidate="txtLonSec"
                        CssClass="validator" Display="Dynamic" Width="100%" MaximumValue="59" MinimumValue="0" Type="Integer"
                        ErrorMessage="Longitude seconds must be an integer between 0 and 59." />
                </div>

                <div>
                    <act:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server"
                        TargetControlID="txtLatDeg" FilterType="Numbers" />
                    <act:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server"
                        TargetControlID="txtLatMin" FilterType="Numbers" />
                    <act:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server"
                        TargetControlID="txtLatSec" FilterType="Numbers" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="txtLatDeg"
                        CssClass="validator" Display="Dynamic" Width="100%" ErrorMessage="Latitude degrees is required." />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ControlToValidate="txtLatMin"
                        CssClass="validator" Display="Dynamic" Width="100%" ErrorMessage="Latitude minutes is required." />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ControlToValidate="txtLatSec"
                        CssClass="validator" Display="Dynamic" Width="100%" ErrorMessage="Latitude seconds is required." />
                    <asp:RangeValidator ID="RangeValidator5" runat="server" ControlToValidate="txtLatDeg" CssClass="validator"
                        Display="Dynamic" Width="100%" MaximumValue="35" MinimumValue="30" Type="Integer"
                        ErrorMessage="Latitude degrees must be an integer between 30 and 35 for Georgia." />
                    <asp:RangeValidator ID="RangeValidator6" runat="server" ControlToValidate="txtLatMin"
                        CssClass="validator" Display="Dynamic" Width="100%" MaximumValue="59" MinimumValue="0" Type="Integer"
                        ErrorMessage="Latitude minutes must be an integer between 0 and 59." />
                    <asp:RangeValidator ID="RangeValidator8" runat="server" ControlToValidate="txtLatSec"
                        CssClass="validator" Display="Dynamic" Width="100%" MaximumValue="59" MinimumValue="0" Type="Integer"
                        ErrorMessage="Latitude seconds must be an integer between 0 and 59." />
                </div>

                <p>
                    <asp:Button ID="btnConvert" runat="server" Text="Convert" CssClass="button-tool" />
                </p>

                <table class="table-simple table-list">
                    <tr>
                        <th>Longitude:</th>
                        <td>
                            <asp:TextBox ID="txtLongDec" runat="server" CssClass="input-small" BackColor="Wheat"
                                ReadOnly="True" />
                            degrees West
                        </td>
                    </tr>
                    <tr>
                        <th>Latitude:</th>
                        <td>
                            <asp:TextBox ID="txtLatDec" runat="server" CssClass="input-small" BackColor="Wheat"
                                ReadOnly="True" />
                            degrees North
                        </td>
                    </tr>
                </table>
                <asp:RangeValidator ID="rngValLongDec" runat="server" ControlToValidate="txtLongDec" CssClass="validator"
                    Type="Double" Display="Dynamic" Width="100%" ErrorMessage="Longitude is outside county limits." />
                <asp:RangeValidator ID="rngValLatDec" runat="server" ControlToValidate="txtLatDec" CssClass="validator"
                    Type="Double" Display="Dynamic" Width="100%" ErrorMessage="Latitude is outside county limits." />

                <p>
                    <asp:Label ID="lblDecLatLongEmpty" runat="server" CssClass="validator" />
                </p>
                <p>
                    <asp:Button ID="btnUseLatLong" runat="server" Text="Use These Values" CausesValidation="False" CssClass="button-large" />
                    <asp:Button ID="btnCancelLatLong" runat="server" Text="Cancel" CausesValidation="False" CssClass="button-cancel" />
                </p>
            </asp:Panel>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
