<%@ Page Language="VB" MasterPageFile="~/Main.master" AutoEventWireup="false"
    Inherits="GECO.FacilityContacts" Title="GECO Facility Contacts" CodeBehind="Contacts.aspx.vb" %>

<%@ MasterType VirtualPath="~/Main.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
    <h1>Facility Contacts</h1>

    <p>
        <b>
            <asp:Label ID="lblFacilityDisplay" runat="server"></asp:Label></b>
        <br />
        AIRS Number:
        <asp:Label ID="lblAIRS" runat="server"></asp:Label>
    </p>

    <ul class="menu-list-horizontal">
        <li>
            <asp:HyperLink ID="lnkFacilityHome" runat="server" NavigateUrl="~/Facility/">Menu</asp:HyperLink>
        </li>
        <li>
            <asp:HyperLink ID="lnkFacilityInfo" runat="server" NavigateUrl="~/Facility/Summary.aspx">Facility Info</asp:HyperLink>
        </li>
        <li>
            <asp:HyperLink ID="lnkFacilityAdmin" runat="server" NavigateUrl="~/Facility/Admin.aspx">User Access</asp:HyperLink>
        </li>
        <li>
            <asp:HyperLink ID="lnkFacilityContacts" runat="server" NavigateUrl="~/Facility/Contacts.aspx"
                Enabled="false" CssClass="selected-menu-item disabled">Communication Preferences</asp:HyperLink>
        </li>
    </ul>

    <table class="table-simple table-rowsections">
        <tbody>
            <tr>
                <td>
                    <h4>Permit Fees</h4>
                    <a href="Contacts.aspx" class="button button-small">Edit</a>
                </td>
                <td>
                    <h4>Communication preference:</h4>
                    <asp:Literal ID="litPermitFeeCommPref" runat="server" Text="By mail only" />

                    <h4>Primary Contact:</h4>
                    <p>
                        <asp:Literal ID="litPermitFeeContact" runat="server" Text="A. Person<br/>Plant Manager<br/>123 Main St.<br/>Atlanta, GA 30303" />
                    </p>

                    <h4>Email Contacts:</h4>
                    <p>None.</p>
                </td>
            </tr>
            <tr>
                <td>
                    <h4>Permit Applications</h4>
                    <a href="Contacts.aspx" class="button button-small">Edit</a>
                </td>
                <td>
                    <h4>Communication preference:</h4>
                    <asp:Literal ID="litPermitAppsCommPref" runat="server" Text="Both electronic and mail" />

                    <h4>Primary Contact:</h4>
                    <p>
                        <asp:Literal ID="litPermitAppsContact" runat="server" Text="A. Person<br/>Plant Manager<br/>123 Main St.<br/>Atlanta, GA 30303" />
                    </p>

                    <h4>Email Contacts:</h4>
                    <ul>
                        <li>a.person@example.com</li>
                        <li>b.person@example.com <i>(not verified)</i></li>
                    </ul>
                </td>
            </tr>
            <tr>
                <td>
    <h4>Emissions Inventory</h4>
        <a href="Contacts.aspx" class="button button-small">Edit</a>
                </td>
                <td>
        <h4>Communication preference:</h4>
        <asp:Literal ID="litEICommPref" runat="server" Text="Electronic communication only" />

    <p class="message-warning">
        <b>Warning:</b> No email recipients have been verified.<br />
        Communication will continue to be sent by mail.
    </p>

    <h4>Primary Contact:</h4>
    <p>
        <asp:Literal ID="litEIContact" runat="server" Text="A. Person<br/>Plant Manager<br/>123 Main St.<br/>Atlanta, GA 30303" />
    </p>

    <h4>Email Contacts:</h4>
    <ul>
        <li>b.person@example.com <i>(not verified)</i></li>
        <li>c.person@example.com <i>(not verified)</i></li>
    </ul>
                </td>
            </tr>
        </tbody>
    </table>

</asp:Content>
