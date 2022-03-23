<%@ Page Title="GECO - Emissions Statement" Language="VB" MasterPageFile="~/Main.master"
    AutoEventWireup="false" Inherits="GECO.es_default" CodeBehind="Default.aspx.vb" %>

<%@ MasterType VirtualPath="~/Main.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">

    <h1>Emissions Statement</h1>

    <p>
        <b>Select an Emissions Statement year to view or work on:</b>
        <asp:DropDownList ID="cboESYear" runat="server" AutoPostBack="True" BackColor="#FFFF66"></asp:DropDownList>
    </p>

    <asp:Panel ID="pnlCurrentES" runat="server">
        <p>
            Welcome to the current Emissions Statement data collection process. We are
            collecting data for calendar year
            <asp:Label ID="lblCurrentYear" runat="server" />.
        </p>

        <asp:Panel ID="pnlCurrentESStatus" runat="server" Width="400" CssClass="panel panel-inprogress text-centered">
            <h2>Status for
                <asp:Label ID="lblCurrentYear2" runat="server" />
                <br />
                Emissions Statement</h2>
            <p>
                <asp:Label ID="lblCurrentStatus" runat="server" />
            </p>
            <p>
                <asp:Button ID="btnCurrentES" runat="server" CssClass="button-large button-proceed" />
            </p>
        </asp:Panel>
    </asp:Panel>

    <asp:Panel ID="pnlPastES" runat="server">
        <p>
            The following is the Emissions Statement information for your facility for the calendar year
            <asp:Label ID="lblPastYear1" runat="server" />.
        </p>
        <div class="panel panel-noaction text-centered">
            <table class="table-simple table-list">
                <tr>
                    <th>Facility Name:</th>
                    <td>
                        <asp:Label ID="lblFacilityName" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th>AIRS No:</th>
                    <td>
                        <asp:Label ID="lblAIRSNo" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th>Confirmation No:</th>
                    <td>
                        <asp:Label ID="lblConfNo" runat="server" />
                    </td>
                </tr>
                <asp:Panel ID="pnlOptedIn" runat="server" Visible="False">
                    <tr>
                        <th>VOC Emissions:</th>
                        <td>
                            <asp:Label ID="lblVOC" runat="server" />
                            tpy
                        </td>
                    </tr>
                    <tr>
                        <th>NO<sub>x</sub> Emissions:</th>
                        <td>
                            <asp:Label ID="lblNOx" runat="server" />
                            tpy
                        </td>
                    </tr>
                </asp:Panel>
            </table>

            <asp:Panel ID="pnlOptedOut" runat="server" Visible="False">
                <p>
                    The facility opted out of the Emissions Statement process in
                    <asp:Label ID="lblPastYear2" runat="server" />.
                </p>
            </asp:Panel>
        </div>
    </asp:Panel>

    <p>
        Per Georgia Rule for Air Quality Control 391-3-1-.02(6)(a)4, the Emissions Statement is 
        required to be completed by facilities located in the state of Georgia that meet the following criteria:
    </p>
    <ul>
        <li>
            <p>
                Located in any of the following counties: Barrow, Bartow, Carroll, Cherokee, Clayton,
                Cobb, Coweta, DeKalb, Douglas, Fayette, Forsyth, Fulton, Gwinnett, Hall, Henry,
                Newton, Paulding, Rockdale, Spalding, or Walton.
            </p>
        </li>
        <li>
            <p>Actual annual emissions of VOCs and/or NO<sub>x</sub> are greater than 25 tons per year.</p>
        </li>
    </ul>
    <p>
        If a facility received a notice to submit the Emissions
        Statement, but their emissions of both VOCs and NO<sub>x</sub> fall below the thresholds, the
        facility should complete the Emissions Statement and choose the option in the emissions
        area of the form to indicate that their emissions are below the thresholds. In doing
        so, the facility will be opting out of the Emissions Statement process for the calendar
        year. The opt out option appears after providing facility and contact information.
    </p>

</asp:Content>
