<%@ Page Title="GECO - Emissions Statement Confirmation" Language="VB" MasterPageFile="~/Main.master"
    AutoEventWireup="false" Inherits="GECO.es_confirm" CodeBehind="Confirm.aspx.vb" %>

<%@ MasterType VirtualPath="~/Main.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">

    <h1>Emissions Statement Confirmation</h1>

    <asp:Panel ID="pnlOptedOutYes" runat="server" CssClass="panel">
        <h2 class="message-update">Opted Out</h2>

        <p>
            Your facility has chosen to opt out of participating in the Emissions Statement process for calendar year
            <asp:Label ID="lblESYear1" runat="server"></asp:Label>
            by indicating that actual emissions of VOCs and NO<sub>x</sub> were each less than or equal to 25 tons for the year.
        </p>

        <table class="table-simple table-list">
            <tr>
                <th>Confirmation number:</th>
                <td>
                    <asp:Label ID="lblConfNum1" runat="server" CssClass="text-larger" />
                </td>
            </tr>
            <tr>
                <th>Facility:</th>
                <td>
                    <asp:Label ID="lblFacility1" runat="server" />
                </td>
            </tr>
            <tr>
                <th>AIRS Number:</th>
                <td>
                    <asp:Label ID="lblAirsNo1" runat="server" />
                </td>
            </tr>
            <tr>
                <th>Today's Date:</th>
                <td>
                    <asp:Label ID="lblDate1" runat="server" />
                </td>
            </tr>
        </table>
    </asp:Panel>

    <asp:Panel ID="pnlConfFinal" runat="server" CssClass="panel">
        <h2 class="message-update">Opted In</h2>

        <p>
            Your facility has opted in and submitted the Emissions Statement data for calendar year 
            <asp:Label ID="lblESYear3" runat="server"></asp:Label>
            by indicating that emissions of VOCs and/or NO<sub>x</sub> were greater than 25 tons for the year.
        </p>

        <table class="table-simple table-list">
            <tr>
                <th>Confirmation number:</th>
                <td>
                    <asp:Label ID="lblConfNumFinalize" runat="server" CssClass="text-larger" />
                </td>
            </tr>
            <tr>
                <th>Facility:</th>
                <td>
                    <asp:Label ID="lblFacility2" runat="server" />
                </td>
            </tr>
            <tr>
                <th>AIRS Number:</th>
                <td>
                    <asp:Label ID="lblAirsNo2" runat="server" />
                </td>
            </tr>
            <tr>
                <th>Today's Date:</th>
                <td>
                    <asp:Label ID="lblDate2" runat="server" />
                </td>
            </tr>
            <tr>
                <th>VOC Emissions:</th>
                <td>
                    <strong>
                        <asp:Label ID="lblVOCAmt2" runat="server" />
                    </strong>
                    tons/year
                </td>
            </tr>
            <tr>
                <th>NO<sub>x</sub> Emissions:</th>
                <td>
                    <strong>
                        <asp:Label ID="lblNOXAmt2" runat="server" />
                    </strong>
                    tons/year
                </td>
            </tr>
        </table>
    </asp:Panel>

    <p>
        <asp:Button ID="btnEsHome" runat="server" Text="Emissions Statement Home" CssClass="button-link" />
        <asp:Button ID="btnMakeChange" runat="server" Text="Make Changes" />
    </p>

    <p class="message-highlight">
        NOTE: You will get a new confirmation number if you make changes.
            However, your original submission date will be unchanged for compliance purposes.
    </p>

</asp:Content>
