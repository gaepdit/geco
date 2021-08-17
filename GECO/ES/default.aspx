<%@ Page Language="VB" MaintainScrollPositionOnPostback="true" MasterPageFile="es.master"
    AutoEventWireup="false" Inherits="GECO.es_default" Title="GECO - Emissions Statement" CodeBehind="default.aspx.vb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h1>Emissions Statement</h1>

    <p>
        <b>Select an Emissions Statement year to view or work on:</b>
        <asp:DropDownList ID="cboESYear" runat="server" AutoPostBack="True" BackColor="#FFFF66"></asp:DropDownList>
    </p>

    <asp:Panel ID="pnlInitial" runat="server">
        <table cellpadding="2" style="width: 90%">
            <tr>
                <td align="left" valign="top">Per Georgia Rule for Air Quality Control 391-3-1-.02(6)(a)4, the Emissions Statement is 
                    required to be completed by facilities located in the state of Georgia that meet the following criteria:<br />
                    <ul>
                        <li>Located in any of the following counties: Barrow, Bartow, Carroll, Cherokee, Clayton,
                            Cobb, Coweta, DeKalb, Douglas, Fayette, Forsyth, Fulton, Gwinnett, Hall, Henry,
                            Newton, Paulding, Rockdale, Spalding, or Walton.<br />
                            <br />
                        </li>
                        <li>Actual annual emissions of VOCs and/or NO<sub>x</sub> are greater than 25 tons per year.</li>
                    </ul>
                </td>
            </tr>
            <tr>
                <td align="left" valign="top">If a facility received a notice to submit the Emissions
                                Statement, but their emissions of both VOCs and NO<sub>x</sub> fall below the thresholds, the
                                facility should complete the Emissions Statement and choose the option in the emissions
                                area of the form to indicate that their emissions are below the thresholds. In doing
                                so, the facility will be opting out of the Emissions Statement process for the calendar
                                year. The opt out option appears after providing facility and contact information.
                </td>
            </tr>
            <tr>
                <td align="left" valign="top">&nbsp;&nbsp;</td>
            </tr>
        </table>
    </asp:Panel>

    <asp:Panel ID="pnlCurrentES" runat="server">
        <table cellspacing="1" style="width: 100%">
            <tr>
                <td>
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <p>
                                    Welcome to the current Emissions Statement data collection process. We are
                                    collecting data for calendar year                               
                                    <asp:Label ID="lblCurrentYear" runat="server"></asp:Label>.
                                </p>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Panel ID="pnlCurrentESStatus" runat="server" Width="400" CssClass="panel panel-inprogress text-centered">
                                    <br />
                                    <strong>Status for
                                        <asp:Label ID="lblCurrentYear2" runat="server"></asp:Label>
                                        <br />
                                        Emissions Statement</strong>
                                    <br />
                                    <br />
                                    <asp:Label ID="lblCurrentStatus" runat="server"></asp:Label>
                                    <br />
                                    <br />
                                    <asp:Button ID="btnCurrentES" runat="server" CssClass="button-large button-proceed" />
                                    <br />
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </asp:Panel>

    <asp:Panel ID="pnlPastES" runat="server">
        <table cellspacing="1" style="width: 100%">
            <tr>
                <td>
                    <table cellpadding="0" cellspacing="0" style="width: 500px">
                        <tr>
                            <td>
                                <br />
                                The following is the Emissions Statement information for your facility for the calendar year                               
                                <asp:Label ID="lblPastYear1" runat="server"></asp:Label>.
                                <br />
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td valign="middle">
                                <table>
                                    <tr>
                                        <td align="right" style="width: 150px">
                                            <b>Facility Name:</b></td>
                                        <td align="left">
                                            <asp:Label ID="lblFacilityName" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <b>AIRS No:</b></td>
                                        <td align="left">
                                            <asp:Label ID="lblAIRSNo" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <b>Confirmation No:</b></td>
                                        <td align="left">
                                            <asp:Label ID="lblConfNo" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td valign="middle">
                                <asp:Panel ID="pnlOptedIn" runat="server" Visible="False">
                                    <table>
                                        <tr>
                                            <td align="right" style="width: 150px">
                                                <b>VOC Emissions:</b></td>
                                            <td align="left">
                                                <asp:Label ID="lblVOC" runat="server"></asp:Label>
                                                &nbsp;tpy</td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <b>NO<sub>x</sub> Emissions:</b></td>
                                            <td align="left">
                                                <asp:Label ID="lblNOx" runat="server"></asp:Label>
                                                &nbsp;tpy</td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td valign="middle">
                                <asp:Panel ID="pnlOptedOut" runat="server" Visible="False">
                                    <table>
                                        <tr>
                                            <td>The facility opted out of the Emissions Statement process in
                                                <asp:Label ID="lblPastYear2" runat="server"></asp:Label>.<br />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
