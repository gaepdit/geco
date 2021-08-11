<%@ Page Language="VB" MasterPageFile="../Main.master" AutoEventWireup="false"
    Inherits="GECO.EditContacts" Title="GECO Edit Facility Contacts" CodeBehind="EditContacts.aspx.vb" %>

<%@ MasterType VirtualPath="~/Main.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">

    <ul class="menu-list-horizontal">
        <li>
            <asp:HyperLink ID="lnkFacilityHome" runat="server" NavigateUrl="~/Facility/"><b>Home</b></asp:HyperLink>
        </li>
        <li>
            <asp:HyperLink ID="lnkFacilityInfo" runat="server" NavigateUrl="~/Facility/Summary.aspx">Facility Info</asp:HyperLink>
        </li>
        <li>
            <asp:HyperLink ID="lnkFacilityAdmin" runat="server" NavigateUrl="~/Facility/Admin.aspx">User Access</asp:HyperLink>
        </li>
        <li>
            <asp:HyperLink ID="lnkFacilityContacts" runat="server" NavigateUrl="~/Facility/Contacts.aspx"
                CssClass="selected-menu-item">Communication Preferences</asp:HyperLink>
        </li>
    </ul>

    <h1>
        <a href="<%= Page.ResolveUrl("~/Facility/Contacts.aspx") %>" class="no-visited">Communication Preferences</a>
        / Edit</h1>
    <p>
        Set your preferences for receiving communications from the Georgia Environmental Protection Division. 
        Preferences can be set separately for each type of communication.
    </p>

    <table class="table-simple">
        <tbody>
            <tr>
                <td class="table-cell-link">
                    <% For Each category In GECO.GecoModels.Facility.CommunicationCategory.AllCategories
                            If FacilityAccess.HasCommunicationPermission(category) Then
                                If category.Name = CurrentCategory.Name Then
                    %>
                    <a href="EditContacts.aspx?category=<%= category.Name %>" class="no-visited disabled selected" aria-disabled="true" disabled><%= category.Description %></a>
                    <%          Else
                    %>
                    <a href="EditContacts.aspx?category=<%= category.Name %>" class="no-visited"><%= category.Description %></a>
                    <%          End If
                            End If
                        Next
                    %>
                </td>
                <td>
                    <h2 id="pref">Communication Preference for <em><%= CurrentCategory.Description %></em></h2>

                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <% If CurrentCategory.ElectronicCommunicationAllowed Then %>
                            <p>Note: Some communication may still be required to be sent by mail.</p>

                            <asp:RadioButtonList ID="rbCommPref" runat="server" ValidationGroup="Preference" CssClass="text-small">
                                <asp:ListItem Value="Electronic">Prefer to receive electronic communications <strong>only.</strong></asp:ListItem>
                                <asp:ListItem Value="Mail">Prefer to receive mailed communications <strong>only.</strong></asp:ListItem>
                                <asp:ListItem Value="Both">Prefer to receive <strong>both</strong> electronic and mailed communications.</asp:ListItem>
                            </asp:RadioButtonList>

                            <% If CurrentCommunicationInfo.Preference.CommunicationPreference.IncludesElectronic AndAlso Not CurrentCommunicationInfo.AnyVerifiedEmails Then %>
                            <p id="pEmailPrefWarning" runat="server" class="message-highlight">
                                Communication will continue to be sent by mail until an email recipient has been verified.
                            </p>
                            <% End If %>

                            <p id="pPrefSaveError" runat="server" visible="false" class="message-warning">
                                There was an error while saving. Please try again.
                            </p>

                            <p id="pPrefSaveSuccess" runat="server" visible="false" class="message-success">
                                Communication preference saved.
                            </p>

                            <p>
                                <asp:Button ID="btnSavePref" runat="server" Text="Save Communication Preferences" ValidationGroup="Preference" />
                            </p>
                            <% End If %>

                            <h3 id="contact">Primary Contact</h3>

                            <asp:Panel ID="pnlEditContact" runat="server" DefaultButton="btnSaveContact">
                                <table class="table-simple table-list text-small">
                                    <tr>
                                        <th>Salutation</th>
                                        <td>
                                            <asp:TextBox ID="txtPrefix" runat="server" ValidationGroup="Contact" />
                                            <small>("Ms.", "Dr.", etc.)</small>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>First Name</th>
                                        <td>
                                            <asp:TextBox ID="txtFirstName" runat="server" ValidationGroup="Contact" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>Last Name</th>
                                        <td>
                                            <asp:TextBox ID="txtLastName" runat="server" ValidationGroup="Contact" />
                                            <i>required</i>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="Contact"
                                                ControlToValidate="txtLastName" ErrorMessage="Last Name is required." />
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>Title</th>
                                        <td>
                                            <asp:TextBox ID="txtTitle" runat="server" ValidationGroup="Contact" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>Organization</th>
                                        <td>
                                            <asp:TextBox ID="txtOrganization" runat="server" ValidationGroup="Contact" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>Street Address</th>
                                        <td>
                                            <asp:TextBox ID="txtAddress" runat="server" ValidationGroup="Contact" />
                                            <i>required</i>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ValidationGroup="Contact"
                                                ControlToValidate="txtAddress" ErrorMessage="Address is required." />
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>Apt / Suite / Other</th>
                                        <td>
                                            <asp:TextBox ID="txtAddress2" runat="server" ValidationGroup="Contact" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>City</th>
                                        <td>
                                            <asp:TextBox ID="txtCity" runat="server" ValidationGroup="Contact" />
                                            <i>required</i>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ValidationGroup="Contact"
                                                ControlToValidate="txtCity" ErrorMessage="City is required." />
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>State</th>
                                        <td>
                                            <asp:TextBox ID="txtState" runat="server" MaxLength="2" ValidationGroup="Contact" />
                                            <i>required</i>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ValidationGroup="Contact"
                                                ControlToValidate="txtState" ErrorMessage="State abbreviation is required." />
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>Postal Code</th>
                                        <td>
                                            <asp:TextBox ID="txtPostalCode" runat="server" MaxLength="10" ValidationGroup="Contact" />
                                            <i>required</i>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ValidationGroup="Contact"
                                                ControlToValidate="txtPostalCode" ErrorMessage="Postal Code is required." />
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>Telephone Number</th>
                                        <td>
                                            <asp:TextBox ID="txtTelephone" runat="server" MaxLength="30" ValidationGroup="Contact" />
                                            <i>required</i>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ValidationGroup="Contact"
                                                ControlToValidate="txtTelephone" ErrorMessage="Phone number is required." />
                                        </td>
                                    </tr>
                                </table>

                                <p id="pMailSaveError" runat="server" visible="false" class="message-warning">
                                    There was an error while saving. Please try again.
                                </p>

                                <p id="pMailSaveSuccess" runat="server" visible="false" class="message-success">
                                    Mail contact saved.
                                </p>

                                <p>
                                    <asp:Button ID="btnSaveContact" runat="server" Text="Save Contact" ValidationGroup="Contact" />
                                </p>
                            </asp:Panel>

                            <% If CurrentCategory.ElectronicCommunicationAllowed Then %>
                            <asp:Panel ID="pnlElectronicCommunication" runat="server">
                                <h3 id="emails">Email Contacts</h3>

                                <% If CurrentCommunicationInfo.Emails.Count = 0 Then %>
                                <p><em>None added.</em></p>
                                <% Else %>

                                <% If CurrentCommunicationInfo.AnyVerifiedEmails Then %>
                                <h4>Current recipients:</h4>
                                <p id="pVerifiedEmailRemovedSuccess" runat="server" visible="false" class="message-success">
                                    The email was removed.
                                </p>
                                <p id="pVerifiedEmailListError" runat="server" visible="false" class="message-warning">
                                    There was an error. Please try again.
                                </p>
                                <ul class="list-with-actions text-small">
                                    <asp:Repeater ID="rptVerifiedEmails" runat="server">
                                        <ItemTemplate>
                                            <li>
                                                <%# Container.DataItem %>
                                                <details>
                                                    <summary class="button-link" role="button">Remove</summary>
                                                    <div class="anim-fade-in fast" role="dialog">
                                                        <p>Are you sure you want to remove this email address?</p>
                                                        <asp:Button ID="btnRemoveVerifiedEmail" runat="server" Text="Remove" CommandArgument='<%# Container.DataItem %>' OnClick="RemoveEmail" UseSubmitBehavior="False" />
                                                    </div>
                                                </details>
                                            </li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ul>
                                <% End If %>

                                <% If CurrentCommunicationInfo.AnyUnverifiedEmails Then %>
                                <h4>Unverified recipients:</h4>
                                <p id="pUnverifiedEmailRemovedSuccess" runat="server" visible="false" class="message-success">
                                    The email was removed.
                                </p>
                                <p id="pUnverifiedEmailListError" runat="server" visible="false" class="message-warning">
                                    There was an error. Please try again.
                                </p>
                                <p id="pEmailVerificationSuccess" runat="server" visible="false" class="message-success">
                                    A confirmation email has been sent.
                                </p>
                                <p id="pEmailAlreadyRemoved" runat="server" visible="false" class="message-warning">
                                    The email selected was previously removed.
                                </p>
                                <p>The following email addresses have been added but have not yet been verified.</p>
                                <ul class="list-with-actions text-small">
                                    <asp:Repeater ID="rptUnverifiedEmails" runat="server">
                                        <ItemTemplate>
                                            <li>
                                                <%# Container.DataItem %>
                                                <details>
                                                    <summary class="button-link" role="button">Remove</summary>
                                                    <div class="anim-fade-in fast" role="dialog">
                                                        <p>Are you sure you want to remove this email address?</p>
                                                        <asp:Button ID="btnRemoveUnverifiedEmail" runat="server"
                                                            Text="Remove" OnClick="RemoveEmail" CommandArgument='<%# Container.DataItem %>' UseSubmitBehavior="False" />
                                                    </div>
                                                </details>
                                                <br />

                                                <span id="a1" runat="server" visible="<%# Container.DataItem = ResentVerificationEmail %>" class="text-success"><i>Sent</i></span>
                                                <span id="a2" runat="server" visible="<%# Container.DataItem = AddedEmail %>" class="text-success"><i>Added</i></span>
                                                <asp:Button ID="btnResendVerification" runat="server" CssClass="button-link"
                                                    Visible="<%# Container.DataItem <> AddedEmail AndAlso Container.DataItem <> ResentVerificationEmail %>"
                                                    Text="Resend verification email" OnClick="ResendVerificationEmail"
                                                    CommandArgument='<%# Container.DataItem %>' UseSubmitBehavior="False" />
                                            </li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ul>
                                <% End If %>

                                <% End If %>

                                <h4>Add a new email recipient:</h4>
                                <asp:Panel ID="pnlAddEmail" runat="server" DefaultButton="btnAddNewEmail">
                                    <p id="pAddEmailError" runat="server" visible="false" class="message-warning">
                                        There was an error while saving. Please try again.
                                    </p>
                                    <p id="pAddEmailInvalid" runat="server" visible="false" class="message-highlight">
                                        The email entered is invalid. Please try again.
                                    </p>
                                    <p id="pAddEmailExists" runat="server" visible="false" class="message-highlight">
                                        The email has already been added.
                                    </p>
                                    <p id="pAddEmailSuccess" runat="server" visible="false" class="message-success">
                                        The email address has been added, and a confirmation email has been sent to the recipient.
                                    </p>
                                    <p>
                                        <asp:TextBox ID="txtNewEmail" runat="server" ValidationGroup="NewEmail" />
                                        <asp:Button ID="btnAddNewEmail" runat="server" ValidationGroup="NewEmail" Text="Add" />
                                        <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ValidationGroup="NewEmail"
                                            ControlToValidate="txtNewEmail" ErrorMessage="Email address is required." />
                                    </p>
                                </asp:Panel>
                            </asp:Panel>
                            <% End If %>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </tbody>
    </table>
    <script src="https://unpkg.com/details-element-polyfill@2.4.0/dist/details-element-polyfill.js"></script>
</asp:Content>
