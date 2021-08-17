<%@ Page Language="VB" MaintainScrollPositionOnPostback="true" MasterPageFile="es.master" AutoEventWireup="false"
    Inherits="GECO.es_esform" Title="GECO - Emissions Statement" CodeBehind="esform.aspx.vb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlFacility" runat="server">
                <br />
                <br />
                <asp:MultiView ID="mltiViewESFacility" runat="server" ActiveViewIndex="0">
                    <asp:View ID="ViewESFacilityLocation" runat="server">
                        <table>
                            <tr>
                                <td colspan="2">
                                    <strong><span style="color: #4169e1; font-size: 12pt;">Facility Location</span></strong>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2"></td>
                            </tr>
                            <tr>
                                <td align="right" valign="middle">Facility Name:
                                </td>
                                <td valign="middle">
                                    <asp:TextBox ID="txtFacilityName" runat="server" BorderColor="White" BorderStyle="Solid"
                                        ReadOnly="True" BorderWidth="1px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="middle">Street Address:
                                </td>
                                <td valign="middle">
                                    <asp:TextBox ID="txtLocationAddress" runat="server" BorderColor="White" BorderStyle="Solid"
                                        ReadOnly="True" BorderWidth="1px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="middle" style="width: 35%; height: 26px;">City:
                                </td>
                                <td valign="middle">
                                    <asp:TextBox ID="txtCity" runat="server" BorderColor="White" BorderStyle="Solid"
                                        ReadOnly="True" BorderWidth="1px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="middle">State:
                                </td>
                                <td valign="middle" style="height: 24px;">
                                    <asp:TextBox ID="txtState" runat="server" BorderColor="White" BorderStyle="Solid"
                                        ReadOnly="True" BorderWidth="1px">GA</asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="middle">Zip:
                                </td>
                                <td valign="middle">
                                    <asp:TextBox ID="txtZipCode" runat="server" BorderColor="White" BorderStyle="Solid"
                                        ReadOnly="True" BorderWidth="1px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="middle">County:
                                </td>
                                <td valign="middle">
                                    <asp:TextBox ID="txtCounty" runat="server" BorderColor="White" BorderStyle="Solid"
                                        ReadOnly="True" BorderWidth="1px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="top"></td>
                                <td valign="top">&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <strong><span style="color: #4169e1; font-size: 12pt;">Geocoordinates</span></strong>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="middle">Latitude:
                                </td>
                                <td valign="middle">
                                    <asp:TextBox ID="txtYCoordinate" runat="server" BorderColor="CornflowerBlue" BorderStyle="Solid"
                                        BorderWidth="1px" MaxLength="9"></asp:TextBox>
                                    degrees North
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="middle"></td>
                                <td valign="middle">
                                    <act:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server"
                                        EnableViewState="False" FilterType="Custom, Numbers" TargetControlID="txtYCoordinate"
                                        ValidChars=".">
                                    </act:FilteredTextBoxExtender>
                                    <asp:RequiredFieldValidator ID="reqValYCoordinate" runat="server" ControlToValidate="txtYCoordinate"
                                        CssClass="validator" Display="Dynamic" Width="100%" ErrorMessage="Latitude is required."></asp:RequiredFieldValidator>
                                    <asp:RangeValidator ID="rngValYCoordinate" runat="server" ControlToValidate="txtYCoordinate"
                                        CssClass="validator" Width="100%" ErrorMessage="Latitude outside county limits."
                                        Type="Double" Display="Dynamic"></asp:RangeValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="middle">Longitude:
                                </td>
                                <td valign="middle">
                                    <asp:TextBox ID="txtXCoordinate" runat="server" BorderColor="CornflowerBlue" BorderStyle="Solid"
                                        BorderWidth="1px" MaxLength="9"></asp:TextBox>
                                    degrees West
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="middle"></td>
                                <td valign="middle">
                                    <act:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server"
                                        FilterType="Custom, Numbers" TargetControlID="txtXCoordinate" ValidChars=".">
                                    </act:FilteredTextBoxExtender>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtXCoordinate"
                                        CssClass="validator" Display="Dynamic" Width="100%" ErrorMessage="Longitude is required."></asp:RequiredFieldValidator>
                                    <asp:RangeValidator ID="rngValXCoordinate" runat="server" ControlToValidate="txtXCoordinate"
                                        CssClass="validator" Width="100%" ErrorMessage="Longitude outside county limits."
                                        Type="Double" Display="Dynamic"></asp:RangeValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="middle"></td>
                                <td valign="middle">
                                    <asp:Button ID="btnLatLongConvert" runat="server" CausesValidation="False"
                                        Text="Latitude-Longitude Converter" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="middle">Horizontal Collection Method:
                                </td>
                                <td valign="middle"></td>
                            </tr>
                            <tr>
                                <td colspan="2" valign="middle">
                                    <asp:DropDownList ID="cboHorizontalCollectionCode" runat="server" Width="90%"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="middle"></td>
                                <td valign="middle">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="cboHorizontalCollectionCode"
                                        Display="Dynamic" Width="100%" ErrorMessage="Select horizontal collection method."
                                        InitialValue=" --Select a Method-- "></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="middle">Horizontal Accuracy Measure:
                                </td>
                                <td valign="middle">
                                    <asp:TextBox ID="txtHorizontalAccuracyMeasure" runat="server" BorderColor="CornflowerBlue"
                                        BorderStyle="Solid" BorderWidth="1px" MaxLength="6"></asp:TextBox>
                                    meters
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="middle"></td>
                                <td valign="middle">
                                    <act:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server"
                                        EnableViewState="False" FilterType="Custom, Numbers" TargetControlID="txtHorizontalAccuracyMeasure"
                                        ValidChars=".">
                                    </act:FilteredTextBoxExtender>
                                    <asp:RangeValidator ID="RangeValidator7" runat="server" ControlToValidate="txtHorizontalAccuracyMeasure"
                                        Width="100%" ErrorMessage="Horizontal accuracy measure cannot be zero."
                                        MaximumValue="1000" MinimumValue="0.01" Type="Double"></asp:RangeValidator>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="txtHorizontalAccuracyMeasure"
                                        CssClass="validator" Display="Dynamic" Width="100%" ErrorMessage="Horizontal accuracy measure is required."></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="middle">Horizontal Datum Reference Code:
                                </td>
                                <td valign="middle">
                                    <asp:DropDownList ID="cboHorizontalReferenceCode" runat="server"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="middle"></td>
                                <td valign="middle">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="cboHorizontalReferenceCode"
                                        Display="Dynamic" Width="100%" ErrorMessage="Select horizontal datum reference."
                                        InitialValue=" --Select a Code-- "></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="middle"></td>
                                <td valign="middle">&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" valign="top">
                                    <asp:Button ID="btnContinueToContact" runat="server" CssClass="button-large" 
                                        Text="Continue to Contact Information" />
                                    &nbsp;
                                    <asp:Button ID="btnCancelLocation" runat="server" Text="Cancel" CausesValidation="False" CssClass="button-cancel" />
                                </td>
                            </tr>
                        </table>
                    </asp:View>

                    <asp:View ID="ViewESFacilityContact" runat="server">
                        <table border="0" cellpadding="2" cellspacing="1" align="center">
                            <tr>
                                <td colspan="2">
                                    <strong><span style="color: #4169e1; font-size: 12pt;">Contact Information</span></strong>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <span style="font-size: 10pt;">(Contact for the Emissions Statement)</span>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="middle">Prefix:
                                </td>
                                <td valign="middle">
                                    <asp:TextBox ID="txtContactPrefix" runat="server" BorderColor="CornflowerBlue"
                                        BorderStyle="Solid" BorderWidth="1px" MaxLength="15"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="middle">First Name:
                                </td>
                                <td valign="middle">
                                    <asp:TextBox ID="txtContactFirstName" runat="server" BorderColor="CornflowerBlue"
                                        BorderStyle="Solid" BorderWidth="1px" MaxLength="35"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="middle"></td>
                                <td valign="middle">
                                    <asp:RequiredFieldValidator ID="reqValFirstName" runat="server" ControlToValidate="txtContactFirstName"
                                        Display="Dynamic" ErrorMessage="First name is required."></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="middle">Last Name:
                                </td>
                                <td valign="middle">
                                    <asp:TextBox ID="txtContactLastName" runat="server" BorderColor="CornflowerBlue"
                                        BorderStyle="Solid" BorderWidth="1px" MaxLength="35"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="middle"></td>
                                <td valign="middle">
                                    <asp:RequiredFieldValidator ID="reqValLastName" runat="server" ControlToValidate="txtContactLastName"
                                        Display="Dynamic" Width="100%" ErrorMessage="Last name is required."></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="middle">Title/Position:
                                </td>
                                <td valign="middle">
                                    <asp:TextBox ID="txtContactTitle" runat="server" BorderColor="CornflowerBlue" BorderStyle="Solid"
                                        BorderWidth="1px" MaxLength="100"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="middle"></td>
                                <td valign="middle">
                                    <asp:RequiredFieldValidator ID="reqValTitle" runat="server" ControlToValidate="txtContactTitle"
                                        Display="Dynamic" Width="100%" ErrorMessage="Contact title/position is required."></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="middle">Email Address:
                                </td>
                                <td valign="middle">
                                    <asp:TextBox ID="txtContactEmail" runat="server" BorderColor="CornflowerBlue" BorderStyle="Solid"
                                        BorderWidth="1px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="middle"></td>
                                <td valign="middle">
                                    <asp:RequiredFieldValidator ID="reqvalEmail1" runat="server" ControlToValidate="txtContactEmail"
                                        Display="Dynamic" ErrorMessage="Email address is required." Width="100%"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="regExpValEmail1" runat="server" ControlToValidate="txtContactEmail"
                                        ErrorMessage="Email address format is incorrect."
                                        ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" Display="Dynamic"
                                        Width="100%"></asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="middle">Phone:
                                </td>
                                <td valign="middle">
                                    <asp:TextBox ID="txtOfficePhoneNbr" runat="server" BorderColor="CornflowerBlue" BorderStyle="Solid"
                                        BorderWidth="1px" MaxLength="30"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="middle"></td>
                                <td valign="middle">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtOfficePhoneNbr"
                                        Display="Dynamic" ErrorMessage="Phone number is required." Width="100%"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="middle">Fax:
                                </td>
                                <td valign="middle">
                                    <asp:TextBox ID="txtFaxNbr" runat="server" BorderColor="CornflowerBlue" BorderStyle="Solid"
                                        BorderWidth="1px" MaxLength="10"></asp:TextBox>
                                    (optional)
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="middle"></td>
                                <td valign="middle">
                                    <act:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" runat="server"
                                        FilterType="Numbers" TargetControlID="txtFaxNbr">
                                    </act:FilteredTextBoxExtender>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtFaxNbr"
                                        Display="Dynamic" ErrorMessage="Fax number must be 10 digits; no dashes, parentheses or spaces allowed."
                                        ValidationExpression="\d{10}"></asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="middle">Company Name:
                                </td>
                                <td valign="middle">
                                    <asp:TextBox ID="txtContactCompanyName" runat="server" BorderColor="CornflowerBlue"
                                        BorderStyle="Solid" BorderWidth="1px" MaxLength="100"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="middle"></td>
                                <td valign="middle">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtContactCompanyName"
                                        Display="Dynamic" ErrorMessage="Contact company name is required."></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="middle">Mailing Address:
                                </td>
                                <td valign="middle">
                                    <asp:TextBox ID="txtContactAddress1" runat="server" BorderColor="CornflowerBlue"
                                        BorderStyle="Solid" BorderWidth="1px" MaxLength="100"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="middle"></td>
                                <td valign="middle">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtContactAddress1"
                                        Display="Dynamic" ErrorMessage="Mailing address is required."></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="middle">City:
                                </td>
                                <td valign="middle">
                                    <asp:TextBox ID="txtContactCity" runat="server" BorderColor="CornflowerBlue" BorderStyle="Solid"
                                        BorderWidth="1px" MaxLength="50"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="middle"></td>
                                <td valign="middle">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtContactCity"
                                        Display="Dynamic" ErrorMessage="City is required."></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="middle">State:
                                </td>
                                <td valign="middle">
                                    <asp:DropDownList ID="cboContactState" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="middle"></td>
                                <td valign="middle">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="cboContactState"
                                        Display="Dynamic" ErrorMessage="Select state for contact mailing address." InitialValue=" -- "></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="middle">Zip:
                                </td>
                                <td valign="middle">
                                    <asp:TextBox ID="txtContactZipCode" runat="server" BorderColor="CornflowerBlue" BorderStyle="Solid"
                                        BorderWidth="1px" MaxLength="5" CssClass="input-small"></asp:TextBox>
                                    -
                                    <asp:TextBox ID="txtContactZipPlus4" runat="server" BorderColor="CornflowerBlue" BorderStyle="Solid"
                                        BorderWidth="1px" MaxLength="4" CssClass="input-small"></asp:TextBox>&nbsp;
                                    plus4 is optional
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="middle"></td>
                                <td valign="middle">
                                    <act:FilteredTextBoxExtender ID="FilteredTextBoxExtender17" runat="server"
                                        FilterType="Numbers" TargetControlID="txtContactZipCode">
                                    </act:FilteredTextBoxExtender>
                                    <act:FilteredTextBoxExtender ID="FilteredTextBoxExtender18" runat="server"
                                        FilterType="Numbers" TargetControlID="txtContactZipPlus4">
                                    </act:FilteredTextBoxExtender>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="txtContactZipCode"
                                        Display="Dynamic" ErrorMessage="Zip code is required." Width="100%"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="txtContactZipCode"
                                        Display="Dynamic" ErrorMessage="Zip code must be 5 digits."
                                        ValidationExpression="\d{5}" Width="100%"></asp:RegularExpressionValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ControlToValidate="txtContactZipPlus4"
                                        Display="Dynamic" ErrorMessage="Zip Plus4 must be 4 digits."
                                        ValidationExpression="\d{4}" Width="100%"></asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="middle">&nbsp;
                                </td>
                                <td valign="middle">&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" valign="top">
                                    <asp:Button ID="btnContinueToEmissions" runat="server" CssClass="button-large"
                                        Text="Continue to Emissions Information" />
                                    &nbsp;
                                    <asp:Button ID="btnbackToLocation" runat="server" Text="Back" CausesValidation="False" />
                                    &nbsp;
                                    <asp:Button ID="btnCancelContact" runat="server" Text="Cancel" CausesValidation="False" CssClass="button-cancel" />
                                </td>
                            </tr>
                        </table>
                    </asp:View>

                    <asp:View ID="ViewESFacilityDescription" runat="server">
                        <table border="0" cellpadding="2" cellspacing="1">
                            <tr>
                                <td style="width: 682px">
                                    <strong><span style="color: #4169e1; font-size: 12pt;">Emissions Information</span></strong>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 682px">
                                    <strong>Facility-wide VOC and NO<sub>x</sub> emissions</strong>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 682px">
                                    <table>
                                        <tr>
                                            <td style="width: 59%" valign="top">Were the facility's actual annual emissions of
                                                VOC and NO<sub>x</sub> both less than or equal to 25 tons per year in <% = Now.Year - 1 %>?
                                            </td>
                                            <td align="left" style="width: 50%" valign="top">
                                                &nbsp;
                                                <asp:DropDownList ID="cboYesNo" runat="server" AutoPostBack="True">
                                                    <asp:ListItem Selected="True">--</asp:ListItem>
                                                    <asp:ListItem>NO</asp:ListItem>
                                                    <asp:ListItem>YES</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td align="left">
                                                <asp:RequiredFieldValidator ID="reqValYesNo" runat="server" ControlToValidate="cboYesNo"
                                                    Display="Dynamic" ErrorMessage="Select Yes or No." Font-Bold="False"
                                                    InitialValue="--"></asp:RequiredFieldValidator>
                                                <br />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <asp:Panel ID="pnlEmissions" runat="server" Visible="False">
                            <table>
                                <tr>
                                    <td></td>
                                    <td align="left">
                                        <asp:Label ID="lblVOCNOXZero" runat="server" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" valign="middle">Actual annual facility-wide <br />
                                        VOC emissions:</td>
                                    <td align="left" valign="middle">
                                        &nbsp;
                                        <asp:TextBox ID="txtVOC" runat="server" MaxLength="7">0</asp:TextBox>
                                        &nbsp;tons/year
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td align="left">
                                        <act:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Numbers, Custom"
                                            ValidChars="." TargetControlID="txtVOC">
                                        </act:FilteredTextBoxExtender>
                                        <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtVOC"
                                            CssClass="validator" Display="Dynamic" ErrorMessage="Maximum VOC emissions is 99,999 tons."
                                            MaximumValue="99999" MinimumValue="0" Type="Double" Width="100%"></asp:RangeValidator>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtVOC"
                                            Display="Dynamic" ErrorMessage="VOC quantity required." Font-Bold="False"
                                            Width="100%"></asp:RequiredFieldValidator>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" valign="middle">Actual annual facility-wide <br />
                                        NO<sub>x</sub> emissions:</td>
                                    <td align="left" valign="middle">
                                        &nbsp;
                                        <asp:TextBox ID="txtNOx" runat="server" MaxLength="7">0</asp:TextBox>
                                        &nbsp;tons/year
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td align="left" valign="middle">
                                        <act:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterType="Numbers, Custom"
                                            ValidChars="." TargetControlID="txtNOx">
                                        </act:FilteredTextBoxExtender>
                                        <asp:RangeValidator ID="RangeValidator9" runat="server" ControlToValidate="txtNOx"
                                            CssClass="validator" Display="Dynamic" ErrorMessage="Maximum NOx emissions is 99,999 tons."
                                            MaximumValue="99999" MinimumValue="0" Type="Double" Width="100%"></asp:RangeValidator>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtNOx"
                                            Display="Dynamic" ErrorMessage="NOx quantity required." Font-Bold="False" Width="100%"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>

                        <table>
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="lblActionDone" runat="server" Font-Bold="False" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">&nbsp;<asp:Button ID="btnSave" runat="server" CssClass="button-large button-proceed" 
                                    Text="Submit Emissions Statement" />
                                    &nbsp;
                                    <asp:Button ID="btnBackToContactInfo" runat="server" Text="Back" CausesValidation="False" />
                                    &nbsp;                                   
                                    <asp:Button ID="btnCancelEmission" runat="server" Text="Cancel" CausesValidation="False" CssClass="button-cancel" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    &nbsp;
                                    <asp:Button ID="btnContinue" runat="server" Text="Continue to Confirmation Page"
                                        CausesValidation="False" Visible="False" />
                                </td>
                            </tr>
                        </table>
                    </asp:View>
                </asp:MultiView>
            </asp:Panel>

            <asp:Panel ID="pnlLatLongConvert" runat="server">
                <div>
                    <table align="center" width="70%">
                        <tr>
                            <td align="center">
                                <strong><span style="font-size: 16pt; color: #4169e1;">Longitude
                                    - Latitude Converter</span></strong>
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div style="text-align: center">
                                    <table style="width: 80%">
                                        <tr>
                                            <td align="left">This tool allows you to convert Longitude and Latitude in "Degrees-Minutes-Seconds"
                                                format to "Decimal Degree" format.<br />
                                                <br />
                                                Enter the longitude and latitude and click "Convert" to show the decimal values
                                                below. Then click "Use These Values" to return to the previous form. If the values
                                                are outside the county in which the facility is located you will not be allowed
                                                to use them. If acceptable, the values will be entered in the appropriate fields
                                                on the Facility Information form on return. Click "Cancel" to go back.
                                                <br />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table style="width: 100%" align="center">
                                    <tr>
                                        <td align="right" valign="middle" style="width: 214px">Longitude:</td>
                                        <td align="left" valign="middle">
                                            <asp:TextBox ID="txtLonDeg" runat="server" BorderColor="CornflowerBlue" BorderStyle="Solid"
                                                BorderWidth="1px" MaxLength="2" Width="50px"></asp:TextBox>
                                            deg &nbsp;<asp:TextBox ID="txtLonMin" runat="server" BorderColor="CornflowerBlue"
                                                BorderStyle="Solid" BorderWidth="1px" MaxLength="2" Width="50px"></asp:TextBox>
                                            min&nbsp;
                                            &nbsp;<asp:TextBox ID="txtLonSec" runat="server" BorderColor="CornflowerBlue" BorderStyle="Solid"
                                                BorderWidth="1px" MaxLength="2" Width="50px"></asp:TextBox>
                                            sec
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" valign="middle" style="width: 214px"></td>
                                        <td align="left" valign="middle">
                                            <act:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server"
                                                TargetControlID="txtLonDeg" FilterType="Numbers">
                                            </act:FilteredTextBoxExtender>
                                            <act:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server"
                                                TargetControlID="txtLonMin" FilterType="Numbers">
                                            </act:FilteredTextBoxExtender>
                                            <act:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server"
                                                TargetControlID="txtLonSec" FilterType="Numbers">
                                            </act:FilteredTextBoxExtender>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="txtLonDeg"
                                                CssClass="validator" Display="Dynamic" Width="100%" ErrorMessage="Longitude degrees required."></asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="txtLonMin"
                                                CssClass="validator" Display="Dynamic" Width="100%" ErrorMessage="Longitude minutes required."></asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="txtLonSec"
                                                CssClass="validator" Display="Dynamic" Width="100%" ErrorMessage="Longitude seconds required."></asp:RequiredFieldValidator><asp:RangeValidator
                                                    ID="RangeValidator2" runat="server" ControlToValidate="txtLonDeg" CssClass="validator"
                                                    Display="Dynamic" Width="100%" ErrorMessage="Longitude degrees must be an integer between 80 and 85 for Georgia."
                                                    MaximumValue="85" MinimumValue="80" Type="Integer"></asp:RangeValidator>
                                            <asp:RangeValidator ID="RangeValidator3" runat="server" ControlToValidate="txtLonMin"
                                                CssClass="validator" Display="Dynamic" Width="100%" ErrorMessage="Longitude minutes must be an integer between 0 and 59"
                                                MaximumValue="59" MinimumValue="0" Type="Integer"></asp:RangeValidator>
                                            <asp:RangeValidator ID="RangeValidator4" runat="server" ControlToValidate="txtLonSec"
                                                CssClass="validator" Display="Dynamic" Width="100%" ErrorMessage="Longitude seconds must be an integer between 0 and 59."
                                                MaximumValue="59" MinimumValue="0" Type="Integer"></asp:RangeValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" valign="middle" style="width: 214px">Latitude:</td>
                                        <td style="height: 20px" align="left" valign="middle">
                                            <asp:TextBox ID="txtLatDeg" runat="server" BorderColor="CornflowerBlue" BorderStyle="Solid"
                                                BorderWidth="1px" MaxLength="2" Width="50px"></asp:TextBox>
                                            deg&nbsp;
                                            <asp:TextBox ID="txtLatMin" runat="server" BorderColor="CornflowerBlue" BorderStyle="Solid"
                                                BorderWidth="1px" MaxLength="2" Width="50px"></asp:TextBox>
                                            min&nbsp;
                                            <asp:TextBox ID="txtLatSec" runat="server" BorderColor="CornflowerBlue" BorderStyle="Solid"
                                                BorderWidth="1px" MaxLength="2" Width="50px"></asp:TextBox>
                                            sec
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" valign="middle" style="width: 214px"></td>
                                        <td align="left" valign="middle">
                                            <act:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server"
                                                TargetControlID="txtLatDeg" FilterType="Numbers">
                                            </act:FilteredTextBoxExtender>
                                            <act:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server"
                                                TargetControlID="txtLatMin" FilterType="Numbers">
                                            </act:FilteredTextBoxExtender>
                                            <act:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server"
                                                TargetControlID="txtLatSec" FilterType="Numbers">
                                            </act:FilteredTextBoxExtender>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="txtLatDeg"
                                                CssClass="validator" Display="Dynamic" Width="100%" ErrorMessage="Latitude degrees required."></asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ControlToValidate="txtLatMin"
                                                CssClass="validator" Display="Dynamic" Width="100%" ErrorMessage="Latitude minutes required."></asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ControlToValidate="txtLatSec"
                                                CssClass="validator" Display="Dynamic" Width="100%" ErrorMessage="Latitude seconds required."></asp:RequiredFieldValidator><asp:RangeValidator
                                                    ID="RangeValidator5" runat="server" ControlToValidate="txtLatDeg" CssClass="validator"
                                                    Display="Dynamic" Width="100%" ErrorMessage="Latitude degrees must be an integer between 30 and 35 for Georgia."
                                                    MaximumValue="35" MinimumValue="30" Type="Integer"></asp:RangeValidator>
                                            <asp:RangeValidator ID="RangeValidator6" runat="server" ControlToValidate="txtLatMin"
                                                CssClass="validator" Display="Dynamic" Width="100%" ErrorMessage="Latitude minutes must be an integer between 0 and 59."
                                                MaximumValue="59" MinimumValue="0" Type="Integer"></asp:RangeValidator>
                                            <asp:RangeValidator ID="RangeValidator8" runat="server" ControlToValidate="txtLatSec"
                                                CssClass="validator" Display="Dynamic" Width="100%" ErrorMessage="Latitude seconds must be an integer between 0 and 59."
                                                MaximumValue="59" MinimumValue="0" Type="Integer"></asp:RangeValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2"></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Button ID="btnConvert" runat="server" Text="Convert" />
                                            &nbsp;&nbsp;                                           
                                            <asp:Button ID="btnCancelLatLong" runat="server" Text="Cancel" CausesValidation="False" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2"></td>
                                    </tr>
                                    <tr>
                                        <td align="right" valign="middle" style="width: 214px">Longitude:
                                        </td>
                                        <td align="left" valign="middle">
                                            <asp:TextBox ID="txtLongDec" runat="server" BorderColor="White" BorderStyle="None"
                                                ReadOnly="True"></asp:TextBox>
                                            degrees West
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" valign="middle" style="width: 214px">Latitude:
                                        </td>
                                        <td align="left" valign="middle">
                                            <asp:TextBox ID="txtLatDec" runat="server" BorderColor="White" BorderStyle="None"
                                                ReadOnly="True"></asp:TextBox>
                                            degrees North
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" valign="middle" style="width: 214px"></td>
                                        <td align="left" valign="middle">
                                            <asp:Label ID="lblDecLatLongEmpty" runat="server" ForeColor="Red"></asp:Label>
                                            <asp:RangeValidator ID="rngValLongDec" runat="server"
                                                ControlToValidate="txtLongDec" CssClass="validator" Type="Double" Display="Dynamic"
                                                Width="100%" ErrorMessage="Longitude is outside county limits."></asp:RangeValidator><asp:RangeValidator
                                                    ID="rngValLatDec" runat="server" ControlToValidate="txtLatDec" CssClass="validator"
                                                    Type="Double" Display="Dynamic" Width="100%" ErrorMessage="Latitude is outside county limits."></asp:RangeValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2"></td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="2">
                                            <asp:Button ID="btnUseLatLong" runat="server" Text="Use These Values" CausesValidation="False" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
