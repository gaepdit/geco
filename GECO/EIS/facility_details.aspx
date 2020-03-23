<%@ Page Language="VB" MasterPageFile="eismaster.master" AutoEventWireup="false" Inherits="GECO.eis_facility_details" Title="Facility Details - GECO Facility Inventory" Codebehind="facility_details.aspx.vb" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
    <div class="pageheader">
        <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></act:ToolkitScriptManager>
        Facility Details
    <asp:Button ID="btnEditAllInfo"
        runat="server" Text="Edit All Info" CausesValidation="False" CssClass="summarybutton" />
    </div>

    <div class="fieldwrapperseparator">
        <asp:Label ID="lblSeparator1" class="styledseparator" runat="server" Text="Facility Name and Address"></asp:Label>
        <div class="sepbuttons">
            <asp:Button ID="btnRequest" runat="server"
                Text="Correct Facility Name or Address" ToolTip="" Font-Size="Small"
                Visible="False" />
        </div>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblFacilitySiteName" class="styled" runat="server" Text="Facility Site Name:"></asp:Label>
        <asp:TextBox ID="txtFacilitySiteName" class="readonly" runat="server"
            ReadOnly="True" Width="300px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="LblLocationAddressText" class="styled" runat="server" Text="Facility Site Address:"></asp:Label>
        <asp:TextBox ID="TxtLocationAddressText" class="readonly" runat="server"
            Text="" ReadOnly="True" Width="300px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="LblSupplementalLocationText2" class="styled_noline" runat="server"></asp:Label>
        <asp:TextBox ID="TxtSupplementalLocationText2" class="readonly" runat="server"
            Text="" ReadOnly="True" Width="200px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblLocalityName" class="styled" runat="server" Text="Facility Site City:"></asp:Label>
        <asp:TextBox ID="TxtLocalityName" class="readonly" runat="server" Text=""
            ReadOnly="True" Width="200px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="LblLocationAddressStateCode" class="styled" runat="server" Text="Physical Location State:"></asp:Label>
        <asp:TextBox ID="TxtLocationAddressStateCode" class="readonly" runat="server"
            Text="" ReadOnly="True" Width="200px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblLocationAddressPostalCode" class="styled" runat="server" Text="Facility Site Zip Code:"></asp:Label>
        <asp:TextBox ID="TxtLocationAddressPostalCode" class="readonly" runat="server"
            Text="" ReadOnly="True" Width="100px"></asp:TextBox>
    </div>
    <br />
    <div class="fieldwrapperseparator">
        <asp:Label ID="lblSeparator2" class="styledseparator" runat="server" Text="Facility Mailing Address"></asp:Label>
        <div class="sepbuttons">
            <asp:Button ID="Button1" runat="server" Text="Edit Mailing Address"
                ToolTip="" Font-Size="Small" Visible="False" />
        </div>
    </div>

    <div class="fieldwrapper">
        <asp:Label ID="lblMailingAddressText" class="styled" runat="server" Text="Facility Mailing Address:"></asp:Label>
        <asp:TextBox ID="txtmailingAddressText" class="readonly" runat="server" Text=""
            ReadOnly="True" Width="300px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblSupplementalAddressText" class="styled_noline" runat="server" Text=""></asp:Label>
        <asp:TextBox ID="TxtSupplementalAddressText" class="readonly" runat="server"
            Text="" ReadOnly="True" Width="300px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblMailingAddressCityName" class="styled" runat="server" Text="Facility Mailing Address City:"></asp:Label>
        <asp:TextBox ID="txtMailingAddressCityName" class="readonly" runat="server"
            Text="" ReadOnly="True" Width="100px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblMailingAddressStateCode" class="styled" runat="server" Text="Facility Mailing Address State:"></asp:Label>
        <asp:TextBox ID="txtMailingAddressStateCode" class="readonly" runat="server"
            Text="" ReadOnly="True" Width="200px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblMailingAddressPostalCode" class="styled" runat="server" Text="Facility Mailing Address Zip Code:"></asp:Label>
        <asp:TextBox ID="TxtMailingAddressPostalCode" class="readonly" runat="server"
            Text="" ReadOnly="True" Width="100px"></asp:TextBox>
    </div>

    <div class="fieldwrapper">
        <asp:Label ID="LblMailingAddressComment" runat="server" CssClass="styled" Text="Comment:"></asp:Label>
        <asp:TextBox ID="TxtMailingAddressComment" runat="server" ReadOnly="True" class="readonly" TextMode="MultiLine" Rows="4"
            Text="" Width="400px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblLastEISSubmit" class="styled" runat="server" Text="Last Submitted to EPA:"></asp:Label>
        <asp:TextBox ID="txtLastEISSubmit" class="readonly" runat="server" Text=""
            ReadOnly="True" Width="100px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblLastUpdated" class="styled" runat="server" Text="Last Updated On:"></asp:Label>
        <asp:TextBox ID="txtLastUpdate" class="readonly" runat="server" Text=""
            ReadOnly="True" Width="300px"></asp:TextBox>
    </div>
    <div class="fieldwrapperseparator">
        <asp:Label ID="lblSeparator3" class="styledseparator" runat="server" Text="Facility Description"></asp:Label>
        <div class="sepbuttons">
            <asp:Button ID="btnEditFacilityDescription" runat="server" Text="Edit Facility Description"
                ToolTip="" Font-Size="Small" Visible="False" />
        </div>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblFacilitySiteDescription" class="styled" runat="server" Text="Description:"></asp:Label>
        <asp:TextBox ID="TxtFacilitySiteDescription" class="readonly" runat="server"
            Text="" ReadOnly="True"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblFacilitySiteStatusCode" class="styled" runat="server" Text="Facility Operating Status:"></asp:Label>
        <asp:TextBox ID="txtFacilitySiteStatusCode" class="readonly" runat="server"
            ReadOnly="True" Width="300px"></asp:TextBox>
        <asp:Label ID="lblStatusCodeNote" runat="server" Font-Size="Smaller" ForeColor="Blue" Text="*Status pertains only to Emissions Inventory"></asp:Label>
    </div>

    <div class="fieldwrapper">
        <asp:Label ID="lblNAICSCode" class="styled" runat="server" Text="NAICS code:"></asp:Label>
        <asp:TextBox ID="TxtNAICSCode" class="readonly" runat="server" Text=""
            ReadOnly="True"></asp:TextBox>
    </div>

    <div class="fieldwrapper">
        <asp:Label ID="LblFacilitySiteComment" class="styled" runat="server" Text="Description Comment:"></asp:Label>
        <asp:TextBox ID="TxtFacilitySiteComment" class="readonly" runat="server"
            TextMode="MultiLine" Rows="4" Text="" Width="400px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblLastEISSubmit_M" class="styled" runat="server" Text="Last Submitted to EPA:"></asp:Label>
        <asp:TextBox ID="txtLastEISSubmit_M" class="readonly" runat="server" Text=""
            ReadOnly="True" Width="100px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblLastUpdate_M" class="styled" runat="server" Text="Last Updated On:"></asp:Label>
        <asp:TextBox ID="txtlastUpdate_M" class="readonly" runat="server" Text=""
            ReadOnly="True" Width="300px"></asp:TextBox>
    </div>
    <div class="fieldwrapperseparator">
        <asp:Label ID="lblSeparator4" class="styledseparator" runat="server" Text="Facility Geographic Coordinate Information"></asp:Label>
        <div class="sepbuttons">
            <asp:Button ID="btnEditGeoInfo" runat="server"
                Text="Edit Geographic Coordinate Info" ToolTip="" Font-Size="Small"
                Visible="False" />
        </div>
        <br />
        <span style="font-size: small; color: Maroon;">Facility&#39;s latitude/longitude must be located at
                    the center of the production area.</span>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="LblLatitudeMeasure" class="styled" runat="server" Text="Latitude:"></asp:Label>
        <asp:TextBox ID="TxtLatitudeMeasure" class="readonly" runat="server" Text="" ReadOnly="True"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="LblLongitudeMeasure" class="styled" runat="server" Text="Longitude:"></asp:Label>
        <asp:TextBox ID="TxtLongitudeMeasure" class="readonly" runat="server" Text=""
            ReadOnly="True"></asp:TextBox>
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
                go to the Edit page and make the correction.
            </p>
        </div>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="LblHorCollectionMetCode" class="styled" runat="server" Text="Horizontal Collection Method:"></asp:Label>
        <asp:TextBox ID="TxtHorCollectionMetCode" runat="server" TextMode="MultiLine" Rows="4"
            Text="" ReadOnly="True" Width="400px" CssClass="readonly"
            Font-Names="Verdana" Font-Size="Small"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="LblHorizontalAccuracyMeasure" class="styled" runat="server" Text="Accuracy Measure (m):"></asp:Label>
        <asp:TextBox ID="TxtHorizontalAccuracyMeasure" class="readonly" runat="server"
            Text="" ReadOnly="True"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="LblHorReferenceDatCode" class="styled" runat="server" Text="Horizontal Reference Datum:"></asp:Label>
        <asp:TextBox ID="TxtHorReferenceDatCode" class="readonly" runat="server"
            Text="" ReadOnly="True"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="LblGeographicComment" class="styled" runat="server" Text="Comment:"></asp:Label>
        <asp:TextBox ID="TxtGeographicComment" class="readonly" runat="server" TextMode="MultiLine" Rows="4" 
            Text="" Width="400px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblLastSubmitEPA_GC" class="styled" runat="server" Text="Last Submitted to EPA:"></asp:Label>
        <asp:TextBox ID="txtLastSubmitEPA_GC" class="readonly" runat="server" Text="" ReadOnly="True"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblLastUpdate_GC" class="styled" runat="server" Text="Last Updated On:"></asp:Label>
        <asp:TextBox ID="txtLastUpdate_GC" class="readonly" runat="server" Text=""
            ReadOnly="True"></asp:TextBox>
    </div>
    <div class="fieldwrapperseparator">
        <asp:Label ID="lblSeparator5" class="styledseparator" runat="server" Text="Emissions Inventory Contact Information"></asp:Label>
        <div class="sepbuttons">
            <asp:Button ID="btnEditContact" runat="server" Text="Edit Contact Info"
                ToolTip="" Font-Size="Small" Visible="False" />
        </div>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblNamePrefix" class="styled" runat="server" Text="Prefix:"></asp:Label>
        <asp:TextBox ID="txtNamePrefix" class="readonly" runat="server" Text=""
            ReadOnly="True" Width="100px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblfirstname" class="styled" runat="server" Text="First Name:"></asp:Label>
        <asp:TextBox ID="txtfirstname" class="readonly" runat="server" Text=""
            ReadOnly="True" Width="100px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblLastName" class="styled" runat="server" Text="Last Name:"></asp:Label>
        <asp:TextBox ID="txtLastName" class="readonly" runat="server" Text=""
            ReadOnly="True" Width="100px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblIndividualTitleText" class="styled" runat="server" Text="Individual Title: "></asp:Label>
        <asp:TextBox ID="txtIndividualTitleText" class="readonly" runat="server"
            Text="" ReadOnly="True" Width="300px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblMailingAddressText_contact" class="styled" runat="server" Text="Contact Mailing Address:"></asp:Label>
        <asp:TextBox ID="txtMailingAddressText_contact" class="readonly" runat="server"
            Text="" ReadOnly="True"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblSupplementalAddressText_contact" class="styled_noline" runat="server"
            Text=""></asp:Label>
        <asp:TextBox ID="txtSupplementalAddressText_contact" class="readonly" runat="server"
            Text="" ReadOnly="True"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblMailingAddressCityName_contact" class="styled" runat="server" Text="City:"></asp:Label>
        <asp:TextBox ID="txtMailingAddressCityName_contact" class="readonly" runat="server"
            Text="" ReadOnly="True" Width="200px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblMailingAddressStateCode_contact" class="styled" runat="server"
            Text="State:"></asp:Label>
        <asp:TextBox ID="txtMailingAddressStateCode_contact" class="readonly" runat="server"
            Text="" ReadOnly="True" Width="100px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblMailingAddressPostalCode_Contact" class="styled" runat="server" Text="Zip Code:"></asp:Label>
        <asp:TextBox ID="txtMailingAddressPostalCode_Contact" class="readonly" runat="server"
            Text="" ReadOnly="True" Width="100px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblTelephoneNumberText" class="styled" runat="server" Text="Contact Phone Number: "></asp:Label>
        <asp:TextBox ID="txtTelephoneNumberText" class="readonly" runat="server"
            Text="" ReadOnly="True" Width="150px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblTelephoneNumberText_mobile" class="styled" runat="server" Text="Contact Mobile Number: "></asp:Label>
        <asp:TextBox ID="txtTelephoneNumber_Mobile" class="readonly" runat="server"
            Text="" ReadOnly="True" Width="100px"></asp:TextBox>
        <act:MaskedEditExtender ID="meeTelephoneNumber_Mobile"
            runat="server" TargetControlID="txtTelephoneNumber_Mobile"
            Mask="999-999-9999" ClearMaskOnLostFocus="False">
        </act:MaskedEditExtender>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblTelephoneNumberText_fax" class="styled" runat="server" Text="Contact Fax Number: "></asp:Label>
        <asp:TextBox ID="txtTelephoneNumber_Fax" class="readonly" runat="server"
            Text="" ReadOnly="True" Width="100px"></asp:TextBox>
        <act:MaskedEditExtender ID="meeTelephoneNumber_Fax"
            runat="server" TargetControlID="txtTelephoneNumber_Fax"
            Mask="999-999-9999" ClearMaskOnLostFocus="False">
        </act:MaskedEditExtender>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblElectronicAddressText" class="styled" runat="server" Text="Contact Email Address:"></asp:Label>
        <asp:TextBox ID="txtElectronicAddressText" class="readonly" runat="server"
            Text="" ReadOnly="True"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblAddressComment_contact" class="styled" runat="server" Text="Comment: "></asp:Label>
        <asp:TextBox ID="txtAddressComment_contact" class="readonly" runat="server"
            TextMode="MultiLine" Rows="4" Text="" Width="400px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblLastEISSubmitEPA_C" class="styled" runat="server" Text="Last Submitted to EPA:"></asp:Label>
        <asp:TextBox ID="txtLastEISSubmitEPA_C" class="readonly" runat="server" Text=""
            ReadOnly="True" Width="100px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblLastUpdate_C" class="styled" runat="server" Text="Last Updated On:"></asp:Label>
        <asp:TextBox ID="txtLastUpdate_C" class="readonly" runat="server" Text=""
            ReadOnly="True" Width="300px"></asp:TextBox>
    </div>
</asp:Content>
