<%@ Page Language="VB" MasterPageFile="~/Main.master" AutoEventWireup="false"
    Inherits="GECO.FacilityContacts" Title="GECO Facility Contacts" CodeBehind="Contacts.aspx.vb" %>

<%@ Import Namespace="GECO.GecoModels.Facility" %>
<%@ MasterType VirtualPath="~/Main.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">

    <% If CommunicationUpdate.ResponseRequired Then %>

    <% If CommunicationUpdate.UpdateRequired Then %>
    <h1>Update Communication Preferences</h1>

    <p>
        The contact information for this facility needs to be updated.
        Please review the issues below and make corrections as needed.
    </p>
    <% Else %>
    <h1>Confirm Communication Preferences</h1>

    <p>
        Current communication preferences and contacts for this facility are shown below.
        Please review and confirm their accuracy or click the "Edit" button to make changes.
    </p>

    <p>
        <asp:Button ID="btnLooksGood" runat="server" Text="Looks good" />
    </p>
    <% End If %>

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
        Current communication preferences and contacts for this facility are shown below.
        Preferences can be set separately for each type of communication by selecting the "Edit" button for each type.
    </p>

    <p>
        <em>Note: This page is for editing facility contact information.</em>
        To edit GECO user access, use the
        <asp:HyperLink runat="server" NavigateUrl="~/Facility/Admin.aspx">User Access</asp:HyperLink>
        tab.
        <% If FacilityAccess.EisAccess Then %>
        To edit CAERS Users, go to the
        <asp:HyperLink runat="server" NavigateUrl="~/EIS/Default.aspx">Emissions Inventory</asp:HyperLink>
        page.
    <% End If %>
    </p>

    <% End If %>

    <% If Today < New Date(2026,7,1) %>
    <div class="announcement announcement-wide">
        <h3>Notice</h3>
        <p>
            Beginning in 2026, notices for Permit Fees will be sent electronically only. 
            In anticipation of this change, we kindly request that you verify the contact information currently on file with us.
        </p>
    </div>
    <% End If %>

    <table class="table-simple table-rowsections" aria-label="Communication preferences">
        <tbody>
            <%
                For Each category In CommunicationCategory.AllCategories

                    If CommunicationUpdate.ResponseRequired AndAlso Not FacilityAccess.HasCommunicationPermission(category) Then
                        ' Display all categories on normal viewing. If a confirmation or update is required, then only show 
                        ' editable categories.
                        Continue For
                    End If

                    Dim info = CommunicationInfo(category)
            %>
            <tr>
                <td>
                    <h2><%= category.Description %></h2>
                    <% If FacilityAccess.HasCommunicationPermission(category) Then %>
                    <a href="EditContacts.aspx?category=<%= category.Name %>" class="button button-small">Edit</a>
                    <% End If %>
                </td>
                <td>
                    <% If category.CommunicationOption = CommunicationOptionType.FacilityChoice Then %>
                    <h3>Communication preference:</h3>
                    <p><%= info.Preference.CommunicationPreference.Description %></p>
                    <% End If %>

                    <% If category.CommunicationOption = CommunicationOptionType.Electronic Then %>
                    <h3>Notice:</h3>
                    <p><%= category.Description %> communication is electronic only. Please ensure email addresses have been provided and are accurate.</p>
                    <% End If %>

                    <% If CommunicationUpdate.UpdateRequired AndAlso CommunicationUpdate.CategoryUpdates.ContainsKey(category) Then %>
                    <p class="message-warning">The contact information is incomplete.</p>
                    <% End If %>

                    <h3>Primary Contact:</h3>
                    <% If info.Mail Is Nothing Then %>
                    <p><em>Not set.</em></p>
                    <% Else %>
                    <p>
                        <%= info.Mail.Name %><br />
                        <% If Not String.IsNullOrEmpty(info.Mail.Title) Then %>
                        <%= info.Mail.Title %><br />
                        <% End If %>
                        <% If Not String.IsNullOrEmpty(info.Mail.Organization) Then %>
                        <%= info.Mail.Organization %><br />
                        <% End If %>
                        <%= info.Mail.Address1%><br />
                        <% If Not String.IsNullOrEmpty(info.Mail.Address2) Then %>
                        <%= info.Mail.Address2%><br />
                        <% End If %>
                        <%= info.Mail.City %>, 
                        <%= info.Mail.State %>
                        <%= info.Mail.PostalCode %><br />
                        <% If Not String.IsNullOrEmpty(info.Mail.Telephone) Then %>
                        <br />
                        <%= info.Mail.Telephone %>
                        <% If Not String.IsNullOrEmpty(info.Mail.Email) Then %>
                        <br />
                        <%= info.Mail.Email%><br />
                        <% End If %>
                        <% End If %>
                    </p>
                    <% End If %>

                    <% If category.CommunicationOption = CommunicationOptionType.Electronic OrElse
                (category.CommunicationOption = CommunicationOptionType.FacilityChoice AndAlso
                info.Preference.CommunicationPreference.IncludesElectronic) Then %>
                    <h3>Additional Email Recipients:</h3>
                    <% If info.Emails.Count = 0 Then %>
                    <p><em>None added.</em></p>
                    <% Else %>
                    <ul class="flush">
                        <% For Each email In info.Emails %>
                        <li><%= email.Email %></li>
                        <% Next %>
                    </ul>
                    <% End If %>
                    <% End If %>
                </td>
            </tr>
            <% 
                Next
            %>
        </tbody>
    </table>
</asp:Content>
