<%@ Page Language="VB" MasterPageFile="~/Main.master" AutoEventWireup="false"
    Inherits="GECO.FacilityContacts" Title="GECO Facility Contacts" CodeBehind="Contacts.aspx.vb" %>

<%@ MasterType VirtualPath="~/Main.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">

    <ul class="menu-list-horizontal">
        <li>
            <asp:HyperLink ID="lnkFacilityHome" runat="server" NavigateUrl="~/Facility/">Home</asp:HyperLink>
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

    <h1>Communication Preferences</h1>

    <p>
        Current communication preferences for this facility are shown below.
            Preferences can be set separately for each type of communication by selecting the "Edit" button for each type.
    </p>

    <table class="table-simple table-rowsections">
        <tbody>
            <tr>
                <td>
                    <h2>Permit Fees</h2>
                    <a href="EditContacts.aspx" class="button button-small">Edit</a>
                </td>
                <td>
                    <h3>Communication preference:</h3>
                    <asp:Literal ID="litPermitFeeCommPref" runat="server" Text="By mail only" />

                    <h3>Primary Contact:</h3>
                    <p>
                        <asp:Literal ID="litPermitFeeContact" runat="server" Text="A. Person<br/>Plant Manager<br/>123 Main St.<br/>Atlanta, GA 30303" />
                    </p>

                    <h3>Email Contacts:</h3>
                    <p>None.</p>
                </td>
            </tr>
            <tr>
                <td>
                    <h2>Permit Applications</h2>
                    <a href="EditContacts.aspx" class="button button-small">Edit</a>
                </td>
                <td>
                    <h3>Communication preference:</h3>
                    <asp:Literal ID="litPermitAppsCommPref" runat="server" Text="Both electronic and mail" />

                    <h3>Primary Contact:</h3>
                    <p>
                        <asp:Literal ID="litPermitAppsContact" runat="server" Text="A. Person<br/>Plant Manager<br/>123 Main St.<br/>Atlanta, GA 30303" />
                    </p>

                    <h3>Email Contacts:</h3>
                    <ul class="flush">
                        <li>a.person@example.com</li>
                        <li>b.person@example.com <i>(not verified)</i></li>
                    </ul>
                </td>
            </tr>
            <tr>
                <td>
                    <h2>Emissions Inventory</h2>
                    <a href="EditContacts.aspx" class="button button-small">Edit</a>
                </td>
                <td>
                    <h3>Communication preference:</h3>
                    <asp:Literal ID="litEICommPref" runat="server" Text="Electronic communication only" />

                    <p class="message-warning">
                        <b>Warning:</b> No email recipients have been verified.
                            Communication will continue to be sent by mail.
                    </p>

                    <h3>Primary Contact:</h3>
                    <p>
                        <asp:Literal ID="litEIContact" runat="server" Text="A. Person<br/>Plant Manager<br/>123 Main St.<br/>Atlanta, GA 30303" />
                    </p>

                    <h3>Email Contacts:</h3>
                    <ul class="flush">
                        <li>b.person@example.com <i>(not verified)</i></li>
                        <li>c.person@example.com <i>(not verified)</i></li>
                    </ul>
                </td>
            </tr>
        </tbody>
    </table>
</asp:Content>
