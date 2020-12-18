<%@ Page MasterPageFile="~/EIS/EIS.master" Language="VB" AutoEventWireup="false"
    Title="GECO EIS: Edit Facility Details"
    Inherits="GECO.EIS_Facility_EditPage" CodeBehind="Edit.aspx.vb" %>

<%@ MasterType VirtualPath="~/EIS/EIS.master" %>
<%@ Register Assembly="Reimers.Google.Map" Namespace="Reimers.Google.Map" TagPrefix="Reimers" %>
<asp:Content ID="Content3" ContentPlaceHolderID="Content" runat="Server">
    <h2>Edit Facility Information</h2>

    <asp:Panel ID="pnlFacilityEdit" runat="server">
        <asp:ValidationSummary ID="ValidationSummary" runat="server" HeaderText="Please correct the following errors:"></asp:ValidationSummary>

        <p><a href="<%= Page.ResolveUrl("~/EIS/Facility/") %>" class="button button-cancel">Cancel</a></p>

        <h3>Name and Address</h3>
        <p>If the facility name or address are incorrect, please contact the Air Protection Branch.</p>
        <table class="table-simple table-list">
            <tbody>
                <tr>
                    <th>Name</th>
                    <td>
                        <asp:Label ID="lblFacilityName" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th>Site Address</th>
                    <td>
                        <asp:Label ID="lblSiteAddress" runat="server" />
                    </td>
                </tr>
            </tbody>
        </table>

        <h3>Mailing Address</h3>
        <table class="table-simple table-list">
            <tbody>
                <tr>
                    <th>
                        <asp:Label ID="lblMailingAddressText" runat="server"
                            AssociatedControlID="txtMailingAddressText">Mailing Address</asp:Label>
                    </th>
                    <td>
                        <asp:TextBox ID="txtMailingAddressText" runat="server" />
                        <asp:RequiredFieldValidator ID="reqvMailingAddressText" runat="server" ControlToValidate="txtMailingAddressText"
                            ErrorMessage="The facility mailing address is required.">*</asp:RequiredFieldValidator>
                        <br />
                        <asp:TextBox ID="txtSupplementalAddressText" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th>
                        <asp:Label ID="lblMailingAddressCityName" runat="server"
                            AssociatedControlID="txtMailingAddressCityName">City</asp:Label>
                    </th>
                    <td>
                        <asp:TextBox ID="txtMailingAddressCityName" runat="server" />
                        <asp:RequiredFieldValidator ID="reqvMailingAddressCityName" runat="server" ControlToValidate="txtMailingAddressCityName"
                            ErrorMessage="The facility mailing address city is required.">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <th>
                        <asp:Label ID="lblMailingAddressStateCode" runat="server"
                            AssociatedControlID="ddlContact_MailState">State</asp:Label>
                    </th>
                    <td>
                        <asp:DropDownList ID="ddlContact_MailState" runat="server" />
                        <asp:RequiredFieldValidator ID="reqvMailingAddressStateCode" ControlToValidate="ddlContact_MailState"
                            InitialValue="--Select a State--" runat="server" ValidationGroup="vgStack" ErrorMessage="The facility mailing address state is required."
                            Display="Dynamic">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <th>
                        <asp:Label ID="lblMailingAddressPostalCode" runat="server"
                            AssociatedControlID="txtMailingAddressPostalCode">Postal Code</asp:Label>
                    </th>
                    <td>
                        <asp:TextBox ID="txtMailingAddressPostalCode" runat="server" MaxLength="10" />
                        <asp:RequiredFieldValidator ID="reqvMailingAddressPostalCode" runat="server" ControlToValidate="txtMailingAddressPostalCode"
                            ErrorMessage="The facility mailing address postal code is required.">*</asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="rgxvMailingAddressPostalCode" runat="server"
                            ControlToValidate="txtMailingAddressPostalCode" ErrorMessage="Please check the facility mailing address postal code format."
                            ValidationExpression="^(\d{5})(-\d{4})?$" Display="Dynamic">Format must be either 99999 or 99999-9999</asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <th>
                        <asp:Label ID="lblAddressComment" runat="server"
                            AssociatedControlID="txtMailingAddressComment">Address Comment</asp:Label>
                    </th>
                    <td>
                        <asp:TextBox ID="txtMailingAddressComment" runat="server" TextMode="MultiLine" Rows="4" />
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                            ErrorMessage="Comment must not exceed 400 characters."
                            ControlToValidate="txtMailingAddressComment"
                            ValidationExpression="^[\s\S]{0,400}$" />
                    </td>
                </tr>
            </tbody>
        </table>

        <h3>Facility Description</h3>
        <table class="table-simple table-list">
            <tbody>
                <tr>
                    <th>
                        <asp:Label ID="lblFacilitySiteDescription" runat="server"
                            AssociatedControlID="txtFacilitySiteDescription">Description</asp:Label>
                    </th>
                    <td>
                        <asp:TextBox ID="txtFacilitySiteDescription" runat="server" MaxLength="100" />
                        <asp:RequiredFieldValidator ID="reqvFacilitySiteDescription" runat="server" ControlToValidate="txtFacilitySiteDescription"
                            ErrorMessage="The facility description is required.">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <th>
                        <asp:Label ID="lblNAICSCode" runat="server"
                            AssociatedControlID="txtNAICSCode">NAICS Code</asp:Label>
                    </th>
                    <td>
                        <asp:TextBox ID="txtNAICSCode" runat="server" MaxLength="6" />
                        <asp:Button ID="btnNAICSLookup" runat="server" Text="NAICS Lookup" CausesValidation="False" />
                        <asp:RequiredFieldValidator ID="reqvNAICSCode" runat="server" ControlToValidate="txtNAICSCode"
                            ErrorMessage="The facility NAICS Code is required">*</asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="rgxvNAICSCode" runat="server" ControlToValidate="txtNAICSCode"
                            Display="Dynamic" ErrorMessage="NAICS Code must be 6 digits" ValidationExpression="\d{6}">*</asp:RegularExpressionValidator>
                        <asp:CustomValidator ID="CustomValidator1" runat="server" ControlToValidate="txtNAICSCode"
                            OnServerValidate="NAICSCheck"
                            ErrorMessage="NAICS Code not valid. Enter another or use the search button.">*</asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                    <th>
                        <asp:Label ID="lblFacilitySiteComment" runat="server"
                            AssociatedControlID="txtFacilitySiteComment">Description Comment</asp:Label>
                    </th>
                    <td>
                        <asp:TextBox ID="txtFacilitySiteComment" runat="server" TextMode="MultiLine" Rows="4" />
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server"
                            ErrorMessage="Comment must not exceed 400 characters."
                            ControlToValidate="txtFacilitySiteComment"
                            ValidationExpression="^[\s\S]{0,400}$" />
                    </td>
                </tr>
            </tbody>
        </table>

        <h3>Geographic Coordinates</h3>
        <p class="label">Facility latitude/longitude must be located at the center of the production area.</p>
        <div>
            <asp:HyperLink ID="lnkGoogleMap" runat="server" Target="_blank" CssClass="no-visited">
                <asp:Image ID="imgGoogleStaticMap" runat="server" BorderStyle="Solid" BorderWidth="2px" /><br />
                Open map in new window
            </asp:HyperLink>
        </div>
        <p class="label" id="pLatLonLocked" runat="server" visible="false"><i>Coordinates are locked for this facility.</i></p>
        <p class="label" id="pGeoInfo" runat="server">
            <em>Geographic information updates must be reviewed by APB staff.</em>
            If the existing values are incorrect, enter your corrections below and include a comment in the 
                    comment box explaining your changes. Facility data will not be modified in the Emissions Inventory system until 
                    approved by APB staff. Please contact us if you have questions.
        </p>
        <table class="table-simple table-list">
            <tbody>
                <tr>
                    <th>
                        <asp:Label ID="lblLatitudeMeasure" runat="server"
                            AssociatedControlID="txtLatitudeMeasure">Latitude</asp:Label>
                    </th>
                    <td>
                        <asp:TextBox ID="txtLatitudeMeasure" runat="server" MaxLength="8" />
                        <asp:Button ID="btnGetLatLon" runat="server" CausesValidation="false" Text="Pick Latitude/Longitude" />
                        <asp:RequiredFieldValidator ID="reqvLatitudeMeasure" ControlToValidate="txtLatitudeMeasure"
                            runat="server" ErrorMessage="The facility latitude is required." Display="Dynamic">*</asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="rngvLatitudeMeasure" runat="server" ControlToValidate="txtLatitudeMeasure"
                            MaximumValue="35.00028" MinimumValue="30.35944" Type="Double"
                            ErrorMessage="The facility latitiude must be between 30.35944° and 35.200028°."
                            Display="Dynamic">Must be between 30.35944° and 35.200028°</asp:RangeValidator>
                    </td>
                </tr>
                <tr>
                    <th>
                        <asp:Label ID="lblLongitudeMeasure" runat="server"
                            AssociatedControlID="txtLongitudeMeasure">Longitude</asp:Label>
                    </th>
                    <td>
                        <asp:TextBox ID="txtLongitudeMeasure" runat="server" MaxLength="9" />
                        <asp:RequiredFieldValidator ID="reqvLongitudeMeasure" ControlToValidate="txtLongitudeMeasure"
                            runat="server" ErrorMessage="The facility longitude is required.">*</asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="rngvLongitudeMeasure" runat="server" ControlToValidate="txtLongitudeMeasure"
                            MinimumValue="-85.60889" MaximumValue="-80.84417" Type="Double"
                            ErrorMessage="The facility longitude must be between -85.60889° and -80.84417°.">
                                Must be between -85.60889° and -80.84417°</asp:RangeValidator>
                    </td>
                </tr>
                <tr>
                    <th>
                        <asp:Label ID="lblHorCollectionMetCode" runat="server"
                            AssociatedControlID="ddlHorCollectionMetCode">Horizontal Collection Method</asp:Label>
                    </th>
                    <td>
                        <asp:DropDownList ID="ddlHorCollectionMetCode" runat="server" />
                        <asp:RequiredFieldValidator ID="reqvHorCollectionMetCode" ControlToValidate="ddlHorCollectionMetCode"
                            runat="server" ErrorMessage="The facility horizontal collection method is required."
                            InitialValue="--Select Horizontal Collection Method--" Display="Dynamic">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <th>
                        <asp:Label ID="lblHorizontalAccuracyMeasure" runat="server"
                            AssociatedControlID="txtHorizontalAccuracyMeasure">Accuracy Measure (m)</asp:Label>
                    </th>
                    <td>
                        <asp:TextBox ID="txtHorizontalAccuracyMeasure" runat="server" />
                        <asp:RequiredFieldValidator ID="reqvHorizontalAccuracyMeasure" ControlToValidate="txtHorizontalAccuracyMeasure"
                            runat="server" ErrorMessage="The facility accuracy measure is required." Display="Dynamic">*</asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="rngvHorizontalAccuracyMeasure" runat="server" ControlToValidate="txtHorizontalAccuracyMeasure"
                            ErrorMessage="The facility accuracy measure must be between 1 and 2000 meters."
                            MaximumValue="2000" MinimumValue="1" Display="Dynamic" Type="Integer">Must be between 1 and 2000</asp:RangeValidator>
                    </td>
                </tr>
                <tr>
                    <th>
                        <asp:Label ID="lblHorReferenceDatCode" runat="server"
                            AssociatedControlID="ddlHorReferenceDatCode">Horizontal Reference Datum</asp:Label>
                    </th>
                    <td>
                        <asp:DropDownList ID="ddlHorReferenceDatCode" runat="server" />
                        <asp:RequiredFieldValidator ID="reqvHorReferenceDatCode" ControlToValidate="ddlHorReferenceDatCode"
                            runat="server" ErrorMessage="The facility horizontal reference datum is required."
                            InitialValue="--Select Horizontal Reference Datum--" Display="Dynamic">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <th>
                        <asp:Label ID="lblGeographicComment" runat="server"
                            AssociatedControlID="txtGeographicComment">Location Comment</asp:Label>
                    </th>
                    <td>
                        <asp:TextBox ID="txtGeographicComment" runat="server" TextMode="MultiLine" Rows="4" />
                        <asp:RegularExpressionValidator ID="regexpName" runat="server"
                            ErrorMessage="Comment must not exceed 400 characters."
                            ControlToValidate="txtGeographicComment"
                            ValidationExpression="^[\s\S]{0,400}$" />
                    </td>
                </tr>
            </tbody>
        </table>

        <asp:HiddenField ID="hidLatitude" runat="server" Visible="false" />
        <asp:HiddenField ID="hidLongitude" runat="server" Visible="false" />
        <asp:HiddenField ID="hidHorCollectionMetCode" runat="server" Visible="false" />
        <asp:HiddenField ID="hidHorCollectionMetDesc" runat="server" Visible="false" />
        <asp:HiddenField ID="hidHorizontalAccuracyMeasure" runat="server" Visible="false" />
        <asp:HiddenField ID="hidHorReferenceDatCode" runat="server" Visible="false" />
        <asp:HiddenField ID="hidHorReferenceDatDesc" runat="server" Visible="false" />
        <asp:HiddenField ID="hidGeographicComment" runat="server" Visible="false" />

        <h3>Emissions Inventory Contact</h3>
        <p>The Emissions Inventory contact will receive notices regarding annual Emissions Inventory submittals for the facility.</p>
        <table class="table-simple table-list">
            <tbody>
                <tr>
                    <th>
                        <asp:Label ID="lblPrefix" runat="server"
                            AssociatedControlID="txtPrefix">Honorific</asp:Label>
                    </th>
                    <td>
                        <asp:TextBox ID="txtPrefix" runat="server" MaxLength="15" />
                    </td>
                </tr>
                <tr>
                    <th>
                        <asp:Label ID="lblFirstName" runat="server"
                            AssociatedControlID="txtFirstName">First Name</asp:Label>
                    </th>
                    <td>
                        <asp:TextBox ID="txtFirstName" runat="server" />
                        <asp:RequiredFieldValidator ID="reqvFirstName" runat="server" ControlToValidate="txtFirstName"
                            ErrorMessage="The Emissions Inventory contact first name is required.">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <th>
                        <asp:Label ID="lblLastName" runat="server"
                            AssociatedControlID="txtLastName">Last Name</asp:Label>
                    </th>
                    <td>
                        <asp:TextBox ID="txtLastName" runat="server" />
                        <asp:RequiredFieldValidator ID="reqvLastName" runat="server" ControlToValidate="txtLastName"
                            ErrorMessage="The Emissions Inventory contact last name is required.">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <th>
                        <asp:Label ID="lblIndividualTitleText" runat="server"
                            AssociatedControlID="txtIndividualTitleText">Title</asp:Label>
                    </th>
                    <td>
                        <asp:TextBox ID="txtIndividualTitleText" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th>
                        <asp:Label ID="lblMailingAddressText_Contact" runat="server"
                            AssociatedControlID="txtMailingAddressText_Contact">Mailing Address</asp:Label>
                    </th>
                    <td>
                        <asp:TextBox ID="txtMailingAddressText_Contact" runat="server" />
                        <asp:RequiredFieldValidator ID="reqvMailingAddressText_Contact" runat="server" ControlToValidate="txtMailingAddressText_Contact"
                            ErrorMessage="The Emissions Inventory contact mailing address is required.">*</asp:RequiredFieldValidator>
                        <br />
                        <asp:TextBox ID="txtSupplementalAddressText_Contact" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th>
                        <asp:Label ID="lblMailingAddressCityName_Contact" runat="server"
                            AssociatedControlID="txtMailingAddressCityName_Contact">City</asp:Label>
                    </th>
                    <td>
                        <asp:TextBox ID="txtMailingAddressCityName_Contact" runat="server" />
                        <asp:RequiredFieldValidator ID="reqvMailingAddressCityName_Contact" runat="server"
                            ControlToValidate="txtMailingAddressCityName_Contact" ErrorMessage="The Emissions Inventory contact city is required.">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <th>
                        <asp:Label ID="lblMailingAddressStateCode_Contact" runat="server"
                            AssociatedControlID="ddlFacility_StateMail">State</asp:Label>
                    </th>
                    <td>
                        <asp:DropDownList ID="ddlFacility_StateMail" runat="server" />
                        <asp:RequiredFieldValidator ID="reqvMailingAddressStateCode_Contact" ControlToValidate="ddlFacility_StateMail"
                            InitialValue="--Select a State--" runat="server" ValidationGroup="vgStack" ErrorMessage="The Emissions Inventory contact state is required."
                            Display="Dynamic">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <th>
                        <asp:Label ID="lblMailingAddressPostalCode_Contact" runat="server"
                            AssociatedControlID="txtMailingAddressPostalCode_Contact">Postal Code</asp:Label>
                    </th>
                    <td>
                        <asp:TextBox ID="txtMailingAddressPostalCode_Contact" runat="server" />
                        <asp:RequiredFieldValidator ID="reqvMailingAddressPostalCode_Contact" runat="server"
                            ControlToValidate="txtMailingAddressPostalCode_Contact" ErrorMessage="The Emissions Inventory contact postal code is required.">*</asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="rgxvMailingAddressPostalCode_Contact" runat="server"
                            ControlToValidate="txtMailingAddressPostalCode_Contact" ErrorMessage="Please check the contact mailing address postal code format."
                            ValidationExpression="^(\d{5})(-\d{4})?$" Display="Dynamic">Format must be either 99999 or 99999-9999</asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <th>
                        <asp:Label ID="lblTelephoneNumberText" runat="server"
                            AssociatedControlID="txtTelephoneNumberText">Phone Number</asp:Label>
                    </th>
                    <td>
                        <asp:TextBox ID="txtTelephoneNumberText" runat="server" MaxLength="30" />
                        <asp:RequiredFieldValidator ID="reqvTelephoneNumberText" runat="server" ControlToValidate="txtTelephoneNumberText"
                            ErrorMessage="The Emissions Inventory contact phone number is required.">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <th>
                        <asp:Label ID="lblTelephoneNumberText_mobile" runat="server"
                            AssociatedControlID="txtTelephoneNumber_Mobile">Mobile</asp:Label>
                    </th>
                    <td>
                        <asp:TextBox ID="txtTelephoneNumber_Mobile" runat="server" MaxLength="15" />
                    </td>
                </tr>
                <tr>
                    <th>
                        <asp:Label ID="lblTelephoneNumberText_fax" runat="server"
                            AssociatedControlID="txtTelephoneNumber_Fax">Fax</asp:Label>
                    </th>
                    <td>
                        <asp:TextBox ID="txtTelephoneNumber_Fax" runat="server" MaxLength="15" />
                    </td>
                </tr>
                <tr>
                    <th>
                        <asp:Label ID="lblElectronicAddress" runat="server"
                            AssociatedControlID="txtElectronicAddressText">Email</asp:Label>
                    </th>
                    <td>
                        <asp:TextBox ID="txtElectronicAddressText" runat="server" />
                        <asp:RequiredFieldValidator ID="reqvElectronicAddressText" runat="server" ControlToValidate="txtElectronicAddressText"
                            ErrorMessage="The Emissions Inventory contact email address is required.">*</asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="rgxvElectronicAddress" runat="server"
                            ControlToValidate="txtElectronicAddressText"
                            ErrorMessage="Email address not valid."
                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <th>
                        <asp:Label ID="lblContactAddressComment_Contact" runat="server"
                            AssociatedControlID="txtAddressComment_Contact">Contact Comment</asp:Label>
                    </th>
                    <td>
                        <asp:TextBox ID="txtAddressComment_Contact" runat="server" TextMode="MultiLine" Rows="4" />
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                            ErrorMessage="Comment must not exceed 400 characters."
                            ControlToValidate="txtAddressComment_Contact"
                            ValidationExpression="^[\s\S]{0,400}$" />
                    </td>
                </tr>
            </tbody>
        </table>

        <p>
            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button-large" />
            <a href="<%= Page.ResolveUrl("~/EIS/Facility/") %>" class="button button-large button-cancel">Cancel</a>
        </p>
    </asp:Panel>

    <asp:Panel ID="pnlNAICSCodeLookup" runat="server" CssClass="panel">
        <h3>NAICS Code Lookup</h3>

        <table class="table-simple table-list">
            <tr>
                <th>
                    <asp:Label ID="lbllookupNAICSCode" runat="server"
                        AssociatedControlID="txtLookupNAICSCode">NAICS Code</asp:Label>
                </th>
                <td>
                    <asp:TextBox ID="txtLookupNAICSCode" runat="server" MaxLength="10"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>
                    <asp:Label ID="lbllookupNAICSCodeDesc" runat="server"
                        AssociatedControlID="txtLookupNAICSDesc">NAICS Code Desc</asp:Label>
                </th>
                <td>
                    <asp:TextBox ID="txtLookupNAICSDesc" runat="server" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
        </table>

        <p>
            <asp:Button ID="btnSearchNAICS" runat="server" Text="Search" CausesValidation="False" CssClass="button" />
            <asp:Button ID="btnCancelNAICS" runat="server" Text="Cancel" CausesValidation="False" CssClass="button button-cancel" />
        </p>

        <p id="pNaicsSelected" runat="server" visible="false" class="table-simple table-list table-compact label-highlight">
            Selected NAICS Code:
            <asp:Label ID="lblSelectedNaicsCode" runat="server" />
            <asp:Button ID="btnUseNAICSCode" runat="server" Text="Use NAICS Code" ValidationGroup="vgNAICS" />
            (NAICS code will be prefilled with the selection.)
        </p>

        <p>
            <asp:Label ID="lblRowCount" runat="server" Font-Bold="True"></asp:Label>
        </p>

        <asp:GridView ID="gvwNAICS" runat="server" CellPadding="4" DataKeyNames="NAICSCode"
            GridLines="None" AutoGenerateColumns="False"
            AllowPaging="True" PageSize="50">
            <PagerSettings PageButtonCount="20" Position="TopAndBottom" />
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
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
    </asp:Panel>

    <asp:Panel ID="pnlMap" runat="server" CssClass="panel">
        <h3>Latitude/Longitude Lookup</h3>

        <asp:Button ID="btnPopupDisplay" runat="server" Style="display: none;" />
        <Reimers:Map ID="GMap" Width="700" Height="400" runat="server"></Reimers:Map>
        <table class="table-simple table-list">
            <tr>
                <th>Latitude</th>
                <td>
                    <asp:TextBox ID="txtMapLat" runat="server" ></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>Longitude</th>
                <td>
                    <asp:TextBox ID="txtMapLon" runat="server"></asp:TextBox>
                </td>
            </tr>
        </table>

        <p>
            <asp:Button ID="btnUseLatLon" runat="server" Text="Use these values" ValidationGroup="LatLon" CssClass="button-large" />
            <asp:Button ID="btnCloseMap" runat="server" Text="Cancel" CausesValidation="false" CssClass="button-large button-cancel" />
        </p>
    </asp:Panel>

</asp:Content>
