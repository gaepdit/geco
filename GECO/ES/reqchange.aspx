<%@ Page Language="VB" MasterPageFile="es.master" MaintainScrollPositionOnPostback="true" AutoEventWireup="false" Inherits="GECO.ei_reqchange" Title="GECO - Request Name/Address Change" Codebehind="reqchange.aspx.vb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="text-align: center">
        <div style="text-align: center">
            <table style="width: 100%">
                <tr>
                    <td>
                        <span style="font-size: 16pt; color: #4169e1; font-family: Arial">Facility Name and Address Change</span></td>
                </tr>
            </table>
        </div>
        &nbsp;<br />
        <asp:Panel ID="pnlTop" runat="server" Width="100%" Height="500px">
            <table border="0" cellpadding="2" cellspacing="2" style="width: 100%">
                <tr>
                    <td align="left">Request a change in the facility's name or address by filling in the "New Information"
                    column on the right. It is not necessary to enter information for items that do
                    not need to be corrected.</td>
                </tr>
                <tr>
                    <td align="center">&nbsp;</td>
                </tr>
                <tr>
                    <td align="center">
                        <div style="text-align: center">
                            <table>
                                <tr>
                                    <td></td>
                                    <td align="left">
                                        <strong>Current Information</strong></td>
                                    <td align="left">
                                        <strong>New Information</strong></td>
                                </tr>
                                <tr>
                                    <td style="width: 135px" align="right">Facility Name:</td>
                                    <td align="left">
                                        <asp:TextBox ID="txtFacilityName" runat="server" BorderColor="White" BorderStyle="None"
                                            Font-Names="Arial" Font-Size="Small" ReadOnly="True" Width="100%"></asp:TextBox></td>
                                    <td align="left">
                                        <asp:TextBox ID="txtFacilityNameNew" runat="server" Font-Names="Arial" Font-Size="Small" Width="98%" BorderColor="CornflowerBlue" BorderStyle="Solid" BorderWidth="1px" MaxLength="80" TabIndex="1"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width: 135px; height: 22px;" align="right">Street Address:</td>
                                    <td align="left" style="height: 22px">
                                        <asp:TextBox ID="txtStreetAddress" runat="server" BorderColor="White" BorderStyle="None"
                                            Font-Names="Arial" Font-Size="Small" ReadOnly="True" Width="100%"></asp:TextBox></td>
                                    <td align="left">
                                        <asp:TextBox ID="txtStreetAddressNew" runat="server" Font-Names="Arial" Font-Size="Small" Width="98%" BorderColor="CornflowerBlue" BorderStyle="Solid" BorderWidth="1px" MaxLength="50" TabIndex="2"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width: 135px" align="right">City:</td>
                                    <td align="left">
                                        <asp:TextBox ID="txtCity" runat="server" BorderColor="White" BorderStyle="None"
                                            Font-Names="Arial" Font-Size="Small" ReadOnly="True" Width="100%"></asp:TextBox></td>
                                    <td align="left">
                                        <asp:TextBox ID="txtCityNew" runat="server" Font-Names="Arial" Font-Size="Small" BorderColor="CornflowerBlue" BorderStyle="Solid" BorderWidth="1px" MaxLength="60" Width="98%" TabIndex="3"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width: 135px" align="right">Zip Code:</td>
                                    <td align="left">
                                        <asp:TextBox ID="txtZipCode" runat="server" BorderColor="White" BorderStyle="None"
                                            Font-Names="Arial" Font-Size="Small" ReadOnly="True" Width="100%"></asp:TextBox></td>
                                    <td align="left">
                                        <asp:TextBox ID="txtZipCodeNew" runat="server" Font-Names="Arial" Font-Size="Small" BorderColor="CornflowerBlue" BorderStyle="Solid" BorderWidth="1px" MaxLength="5" Width="70px" TabIndex="4"></asp:TextBox>
                                        <act:FilteredTextBoxExtender ID="txtZipCodeNew_FilteredTextBoxExtender"
                                            runat="server" Enabled="True" FilterType="Custom, Numbers"
                                            TargetControlID="txtZipCodeNew">
                                        </act:FilteredTextBoxExtender>
                                        -<asp:TextBox
                                            ID="txtZipPlus4New" runat="server" BorderColor="CornflowerBlue" BorderStyle="Solid"
                                            BorderWidth="1px" Font-Names="Arial" Font-Size="Small" MaxLength="4" TabIndex="4"
                                            Width="50px"></asp:TextBox>
                                        <act:FilteredTextBoxExtender ID="txtZipPlus4New_FilteredTextBoxExtender"
                                            runat="server" Enabled="True" FilterType="Custom, Numbers"
                                            TargetControlID="txtZipPlus4New">
                                        </act:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" style="width: 135px">County:</td>
                                    <td align="left">
                                        <asp:TextBox ID="txtCounty" runat="server" BorderColor="White" BorderStyle="None"
                                            Font-Names="Arial" Font-Size="Small" ReadOnly="True" Width="100%"></asp:TextBox></td>
                                    <td align="left">
                                        <asp:DropDownList ID="cboCountyNew" runat="server" TabIndex="5">
                                        </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td align="right" style="width: 135px" valign="top">
                                        <span style="color: #ff0000"><span style="color: black;">Comments:</span><br />
                                            <span style="font-size: 7pt">400-char limit</span></td>
                                    <td align="left" colspan="2" valign="top" style="font-size: 8pt; color: #ff0000">
                                        <asp:TextBox ID="txtComments" runat="server" Font-Names="Arial" Font-Size="Small"
                                            Height="60px" MaxLength="400" TextMode="MultiLine" Width="447px" TabIndex="6"></asp:TextBox></td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr style="font-size: 8pt; color: #ff0000">
                    <td align="center">
                        <span><span style="color: red">After submitting this request, the new information
                        will not be immediately<br />
                        </span><span style="color: red">
                            <span>reflected &nbsp;on the Facility Information form. It may take a few days before the<br />
                            </span><span>update takes effect. You may also be contacted by the Air Protection Branch.</span></span></span></td>
                </tr>
                <tr style="color: #000000">
                    <td align="center">&nbsp;&nbsp;</td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit Request" TabIndex="7" />
                        &nbsp; &nbsp;
                    <asp:Button ID="btnCancel" runat="server" CausesValidation="False" Text="Cancel" TabIndex="8" />
                        <asp:Label ID="lblBlankWarning" runat="server" Font-Bold="True" Font-Names="Arial"
                            Font-Size="Small" ForeColor="Red" Visible="False"></asp:Label></td>
                </tr>
                <tr>
                    <td align="center">&nbsp;</td>
                </tr>
            </table>
        </asp:Panel>
    </div>
    <asp:Panel ID="pnlConfirm" runat="server" Width="100%" Height="500px">
        <div style="text-align: center">
            <table style="width: 100%">
                <tr>
                    <td style="width: 100px"></td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <span style="font-size: 12pt"><strong>Confirmation of Changes</strong></span></td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <table width="70%">
                            <tr>
                                <td></td>
                                <td align="left">
                                    <strong>Change Requested</strong></td>
                            </tr>
                            <tr>
                                <td style="width: 135px" align="right">Facility Name:</td>
                                <td align="left">
                                    <asp:TextBox ID="txtFacilityNameConf" runat="server" BorderColor="DarkGray" BorderStyle="Solid"
                                        Font-Names="Arial" Font-Size="Small" ReadOnly="True" Width="95%" BorderWidth="1px" ForeColor="#0000C0"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td style="width: 135px; height: 22px;" align="right">Street Address:</td>
                                <td align="left" style="height: 22px">
                                    <asp:TextBox ID="txtStreetAddressConf" runat="server" BorderColor="DarkGray" BorderStyle="Solid"
                                        Font-Names="Arial" Font-Size="Small" ReadOnly="True" Width="95%" BorderWidth="1px" ForeColor="#0000C0"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td style="width: 135px" align="right">City:</td>
                                <td align="left">
                                    <asp:TextBox ID="txtCityConf" runat="server" BorderColor="DarkGray" BorderStyle="Solid"
                                        Font-Names="Arial" Font-Size="Small" ReadOnly="True" Width="95%" BorderWidth="1px" ForeColor="#0000C0"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td style="width: 135px" align="right">Zip Code:</td>
                                <td align="left">
                                    <asp:TextBox ID="txtZipCodeConf" runat="server" BorderColor="DarkGray" BorderStyle="Solid"
                                        Font-Names="Arial" Font-Size="Small" ReadOnly="True" Width="70px" BorderWidth="1px" ForeColor="#0000C0"></asp:TextBox>-<asp:TextBox
                                            ID="txtZipPlus4Conf" runat="server" BorderColor="DarkGray" BorderStyle="Solid"
                                            BorderWidth="1px" Font-Names="Arial" Font-Size="Small" ForeColor="#0000C0" MaxLength="4"
                                            ReadOnly="True" TabIndex="4" Width="21%"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td align="right" style="width: 135px">County:</td>
                                <td align="left">
                                    <asp:TextBox ID="txtCountyConf" runat="server" BorderColor="DarkGray" BorderStyle="Solid"
                                        Font-Names="Arial" Font-Size="Small" ReadOnly="True" Width="95%" BorderWidth="1px" ForeColor="#0000C0"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td align="right" style="width: 135px; height: 62px;" valign="top">Comments:<br />
                                    <span style="font-size: 7pt; color: red">400 char limit</span></td>
                                <td align="left" colspan="1" valign="top" style="font-size: 8pt; color: #ff0000">
                                    <asp:TextBox ID="txtCommentsConf" runat="server" BorderColor="DarkGray" BorderStyle="Solid"
                                        Font-Names="Arial" Font-Size="Small" Height="120px" MaxLength="400" ReadOnly="True"
                                        TextMode="MultiLine" Width="95%" BorderWidth="1px" ForeColor="#0000C0"></asp:TextBox></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>Confirm that the above are the changes being requested before continuing.</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td style="height: 26px">
                        <asp:Button ID="btnSubmitRequest" runat="server" Text="Submit Change Request" Width="170px" TabIndex="7" />
                        &nbsp; &nbsp;
                        <asp:Button ID="btnCancelConf" runat="server" CausesValidation="False" Text="Go back and make corrections" TabIndex="8" Width="200px" /></td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnReturn" runat="server" Text="Return to Emission Statement" Width="206px" TabIndex="7" />
                        <asp:Label ID="lblConfirmSubmit" runat="server" Font-Bold="True" Font-Names="Arial"
                            Font-Size="Small" ForeColor="Red" Visible="False"></asp:Label></td>
                </tr>
            </table>
        </div>
    </asp:Panel>
</asp:Content>