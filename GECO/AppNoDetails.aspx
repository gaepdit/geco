<%@ Page Language="VB" MasterPageFile="~/MainMaster.master" AutoEventWireup="false" Inherits="GECO.AppNoDetails" Title="GA Air - Application Details" Codebehind="AppNoDetails.aspx.vb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <br />
    <b>Application No.:
    <asp:Label ID="lblAppNo" runat="server"></asp:Label>
        for AIRS Number:
    <asp:Label ID="lblAirs" runat="server"></asp:Label>
    </b>
    <br />

    <table style="width: 100%">
        <tr align="left">
            <td style="width: 22%">Engineer Name:
            </td>
            <td bgcolor="#CCCCCC" style="width: 40%">
                <asp:Label ID="lblEngName" runat="server"></asp:Label>
            </td>
            <td>&nbsp;</td>
            <td style="width: 23%">SSPP (APL) Unit:
            </td>
            <td bgcolor="#CCCCCC">
                <asp:Label ID="lblAplUnit" runat="server"></asp:Label>
            </td>
        </tr>
        <tr align="left">
            <td style="width: 22%">Engineer Phone:
            </td>
            <td bgcolor="#CCCCCC" style="width: 40%">
                <asp:Label ID="lblEngPhone" runat="server"></asp:Label>
            </td>
            <td>&nbsp;</td>
            <td style="width: 23%">Application Type:
            </td>
            <td bgcolor="#CCCCCC">
                <asp:Label ID="lblAplType" runat="server"></asp:Label>
            </td>
        </tr>
        <tr align="left">
            <td style="width: 22%">Engineer Email:
            </td>
            <td bgcolor="#CCCCCC" style="width: 40%">
                <asp:HyperLink ID="lblEngEmail" runat="server"></asp:HyperLink>
            </td>
            <td>&nbsp;</td>
            <td style="width: 23%">Action Type:</td>
            <td bgcolor="#CCCCCC">
                <asp:Label ID="lblActionType" runat="server"></asp:Label>
            </td>
        </tr>

        <tr align="left">
            <td style="width: 22%">Permit Number:</td>
            <td style="width: 22%" bgcolor="#CCCCCC">
                <asp:HyperLink ID="lblPermitNo" runat="server"></asp:HyperLink>
                <%-- <asp:Label ID="lblPermitNo" runat="server" backcolor="#CCCCCC"></asp:Label>--%></td>
            <td colspan="3">&nbsp;</td>
        </tr>
    </table>

    <br />
    <hr />
    <p>
        <span style="font-weight: 700">Facility Application Information: </span><span style="font-weight: normal">Facility
    name, location, and descriptions as submitted with the application</span>
    </p>
    <table style="width: 95%">
        <tr>
            <td style="width: 24%">Facility Name:
            </td>
            <td bgcolor="#CCCCCC" style="width: 45%">
                <asp:Label ID="lblFacName" runat="server"></asp:Label>
            </td>
            <td style="width: 1%">&nbsp;</td>
            <td style="width: 15%">SIC Code:
            </td>
            <td bgcolor="#CCCCCC">
                <asp:Label ID="txtSICCode" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="width: 24%">Facility Address:
            </td>
            <td bgcolor="#CCCCCC" style="width: 45%">
                <asp:Label ID="lblFacAddress" runat="server"></asp:Label>
            </td>
            <td style="width: 1%">&nbsp;</td>
            <td style="width: 15%">County:
            </td>
            <td bgcolor="#CCCCCC">
                <asp:Label ID="lblcounty" runat="server"></asp:Label></td>
        </tr>
        <tr>
            <td style="width: 24%">City, State, Zip:
            </td>
            <td bgcolor="#CCCCCC" style="width: 45%">
                <asp:Label ID="lblFacCityStateZip" runat="server"></asp:Label>
            </td>
            <td style="width: 1%">&nbsp;</td>
            <td style="width: 15%">District:
            </td>
            <td bgcolor="#CCCCCC">
                <asp:Label ID="lblDistrict" runat="server"></asp:Label></td>
        </tr>
    </table>
    <br />
    Plant Description (on Public Notice):<br />
    &nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Label ID="txtDesc" runat="server" BackColor="#CCCCCC"></asp:Label><br />
    <br />
    Reason for Application (on Public Notice):<br />
    &nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Label ID="txtReason" runat="server" BackColor="#CCCCCC"></asp:Label><br />

    <%--<br /> Commented out by Brian Gregory on 7-3-2008 as requested by SSPP.
<hr />
<p><span style="font-weight: 700">Header Data: </span> <span style="font-weight: normal">Facility regulatory details at
    the time of the application.</span></p>
  Operational Status: <asp:Label ID="txtOpStatus" runat="server" backcolor="#CCCCCC">
</asp:Label>
<br />
   Classification: <asp:Label ID="txtClass" runat="server" backcolor="#CCCCCC"></asp:Label>
<br /><br />
Non Attainment:<br />
<table style="width: 95%">
<tr>
<td style="width: 20%; text-align: right">1 hr Ozone:</td>
<td style="width: 14%" bgcolor="#CCCCCC"><asp:Label ID="txt1HourOzone" runat="server"></asp:Label></td>
<td style="width: 20%; text-align: right">8 hr Ozone:</td>
<td style="width: 13%" bgcolor="#CCCCCC"><asp:Label ID="txt8HROzone" runat="server"></asp:Label></td>
<td style="width: 20%; text-align: right">PM 2.5:</td>
<td style="width: 13%" bgcolor="#CCCCCC"><asp:Label ID="txtPM" runat="server"></asp:Label>
</td>
</tr>
</table>
<br />
    Air Program Code(s):<br />
                                    <asp:CheckBoxList ID="cblAirProgramCodes" runat="server"
                                        Enabled="False" Font-Bold="True" backcolor="#CCCCCC"
        ForeColor="WindowText" RepeatColumns="3">
                                        <asp:ListItem>0 - SIP</asp:ListItem>
                                        <asp:ListItem>6 - PSD</asp:ListItem>
                                        <asp:ListItem>7 - NSR</asp:ListItem>
                                        <asp:ListItem>8 - NESHAP (Part 61)</asp:ListItem>
                                        <asp:ListItem>9 - NSPS</asp:ListItem>
                                        <asp:ListItem>M - MACT (Part 63)</asp:ListItem>
                                        <asp:ListItem>V - Title V</asp:ListItem>
                                        <asp:ListItem>A - Acid Precipitation</asp:ListItem>
                                    </asp:CheckBoxList>
                                    <br />
                                    Other:<br />
                                    <table style="width: 310px">
                                    <tr>
                                    <td>
                                        <asp:CheckBox ID="chbNSRMajor" runat="server" Text="NSR/PSD Major"
                                            Enabled="false" Font-Bold="True" backcolor="#CCCCCC" style="text-align: left" />
                                    </td>
                                    <td><asp:CheckBox ID="chbHAPsMajor" runat="server" Text="HAPs Major"
                                            Enabled="false" Font-Bold="True" backcolor="#CCCCCC" style="text-align: left" />
                                    </td></tr></table>
    --%>
    <br />
    <hr />
    <p>
        <b>Application Processing Information:</b> <span style="font-weight: normal">Approximate application/permit processing timeline while at EPD.</span>
    </p>
    <table id="tblDates" runat="server" cellpadding="3" cellspacing="3">
        <tr>
            <td align="left" width="50%"><b>Action:</b></td>
            <td align="left" width="50%" bgcolor="#CCCCCC"><b>Date:</b></td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="LeftMenuContent" runat="Server">
</asp:Content>