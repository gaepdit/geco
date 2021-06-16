<%@ Page Language="VB" MasterPageFile="~/Main.master" AutoEventWireup="false"
    Inherits="GECO.FacilityContacts" Title="GECO Facility Contacts" CodeBehind="Contacts.aspx.vb" %>

<%@ MasterType VirtualPath="~/Main.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">

    <% If Reconfirm %>
    <h1>Confirm Communication Preferences</h1>

    <p>
        Current communication preferences for this facility are shown below.
        Please review and confirm their accuracy.
    </p>

    <p>
        <asp:Button ID="btnLooksGood" runat="server" Text="Looks good" />
        &nbsp;
        <asp:Button ID="btnMakeChanges" runat="server" Text="Make changes" />
    </p>
    <% Else %>
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
    <% End If %>

    <table class="table-simple table-rowsections">
        <tbody>
            <%
                For Each category In GECO.GecoModels.Facility.CommunicationCategory.AllCategories
                    Dim info = CommunicationInfo(category)
            %>
            <tr>
                <td>
                    <h2><%= category.Name %></h2>
                    <% If Not Reconfirm AndAlso FacilityAccess.HasCommunicationPermission(category) Then %>
                    <a href="EditContacts.aspx" class="button button-small">Edit</a>
                    <% End If %>
                </td>
                <td>
                    <h2>Communication preference:</h2>
                    <p><%= info.Preference.CommunicationPreference.Display %></p>

                    <h3>Primary Contact:</h3>
                    <% If info.Mail Is Nothing %>
                    <p><em>Not set.</em></p>
                    <% Else %>
                    <p>
                        <%= info.Mail.Name %><br />
                        <% If info.Mail.Title IsNot Nothing %>
                        <%= info.Mail.Title %><br />
                        <% End If %>
                        <% If info.Mail.Organization IsNot Nothing %>
                        <%= info.Mail.Organization %><br />
                        <% End If %>
                        <%= info.Mail.Address1%><br />
                        <% If info.Mail.Address2 IsNot Nothing %>
                        <%= info.Mail.Address2%><br />
                        <% End If %>
                        <%= info.Mail.City %>, 
                        <%= info.Mail.State %>
                        <%= info.Mail.PostalCode %><br />
                        <% If info.Mail.Telephone IsNot Nothing %>
                        <br />
                        <%= info.Mail.Telephone %>
                        <% End If %>
                    </p>
                    <% End If %>

                    <h3>Email Contacts:</h3>
                    <% If info.Emails.Count = 0 %>
                    <p><em>None added.</em></p>
                    <% Else %>
                    <ul class="flush">
                        <% For Each email In CommunicationInfo(category).Emails %>
                        <li><%= email.Email %>
                            <% If Not email.Verified Then %>
                            <i>(not verified)</i>
                            <% End If %>
                        </li>
                        <% Next %>
                    </ul>
                    <% End If %>
                </td>
            </tr>
            <% Next %>
        </tbody>
    </table>
</asp:Content>
