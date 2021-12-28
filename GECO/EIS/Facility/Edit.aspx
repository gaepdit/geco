<%@ Page MasterPageFile="~/EIS/EIS.master" Language="VB" AutoEventWireup="false"
    Title="GECO EI: Edit Facility Information"
    Inherits="GECO.EIS_Facility_EditPage" CodeBehind="Edit.aspx.vb" %>

<%@ MasterType VirtualPath="~/EIS/EIS.master" %>
<%@ Register Assembly="Reimers.Google.Map" Namespace="Reimers.Google.Map" TagPrefix="Reimers" %>
<asp:Content ID="Content3" ContentPlaceHolderID="Content" runat="Server">
    <h1>Edit EI Facility Information</h1>

    <asp:Panel ID="pnlFacilityEdit" runat="server">
        <p><a href="<%= Page.ResolveUrl("~/EIS/") %>" class="button button-cancel">Cancel</a></p>

        <asp:ValidationSummary ID="ValidationSummary" runat="server" HeaderText="Please correct the following errors:"></asp:ValidationSummary>

        <h3>Name and Address</h3>
        <p>If the facility name or address are incorrect, please email <a href="mailto:emissions.inventory@dnr.ga.gov">emissions.inventory@dnr.ga.gov</a>.</p>
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

        <p>
            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button-large" />
            <a href="<%= Page.ResolveUrl("~/EIS/") %>" class="button button-large button-cancel">Cancel</a>
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
                    <asp:TextBox ID="txtMapLat" runat="server"></asp:TextBox>
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
