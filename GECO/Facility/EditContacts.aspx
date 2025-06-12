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

    <table class="table-simple" aria-label="Communication preferences">
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
                    <h2 id="pref">Edit Preferences for <em><%= CurrentCategory.Description %></em></h2>

                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="pnlFacilityCommunicationChoice" runat="server" Visible="false">
                                <h3 id="method">Preferred Method of Communication</h3>

                                <p>
                                    Select your preferred method of communication for <%= CurrentCategory.Description %>.
                                Please note that regardless of selection, a primary mail and email contact are still required.
                                </p>

                                <asp:RadioButtonList ID="rbCommPref" runat="server" ValidationGroup="Preference" CssClass="text-small">
                                    <asp:ListItem Value="Electronic">Prefer to receive electronic communications.</asp:ListItem>
                                    <asp:ListItem Value="Mail">Prefer to receive mailed communications.</asp:ListItem>
                                    <asp:ListItem Value="Both">Prefer to receive <strong>both</strong> electronic and mailed communications.</asp:ListItem>
                                </asp:RadioButtonList>

                                <p id="pPrefSaveError" runat="server" visible="false" class="message-warning anim-fade-in fast">
                                    There was an error while saving. Please try again.
                                </p>

                                <p id="pPrefSaveSuccess" runat="server" visible="false" class="message-success anim-fade-in fast">
                                    Communication preference saved.
                                </p>

                                <p>
                                    <asp:Button ID="btnSavePref" runat="server" Text="Save Communication Preferences" ValidationGroup="Preference" />
                                </p>
                            </asp:Panel>

                            <h3 id="contact">Primary Contact</h3>

                            <asp:Panel ID="pnlEditContact" runat="server" DefaultButton="btnSaveContact">
                                <table class="table-simple table-list text-small" aria-label="Communication preferences edit form">
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
                                            <asp:RegularExpressionValidator ID="regexFirstName" runat="server" ValidationExpression="[a-zA-Z].*" ValidationGroup="Contact"
                                                ControlToValidate="txtFirstName" ErrorMessage="Name must start with an alphabetic character." CssClass="text-error" Display="Dynamic" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>Last Name</th>
                                        <td>
                                            <asp:TextBox ID="txtLastName" runat="server" ValidationGroup="Contact" />
                                            <i>required</i>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="Contact"
                                                ControlToValidate="txtLastName" ErrorMessage="Last Name is required." CssClass="text-error" Display="Dynamic" />
                                            <asp:RegularExpressionValidator ID="regexLastName" runat="server" ValidationExpression="[a-zA-Z].*" ValidationGroup="Contact"
                                                ControlToValidate="txtLastName" ErrorMessage="Name must start with an alphabetic character." CssClass="text-error" Display="Dynamic" />
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
                                                ControlToValidate="txtAddress" ErrorMessage="Address is required." CssClass="text-error" Display="Dynamic" />
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
                                                ControlToValidate="txtCity" ErrorMessage="City is required." CssClass="text-error" Display="Dynamic" />
                                            <asp:RegularExpressionValidator ID="regexCity" runat="server" ValidationExpression="[a-zA-Z].*" ValidationGroup="Contact"
                                                ControlToValidate="txtCity" ErrorMessage="City must start with an alphabetic character." CssClass="text-error" Display="Dynamic" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>State</th>
                                        <td>
                                            <asp:TextBox ID="txtState" runat="server" MaxLength="2" ValidationGroup="Contact" />
                                            <i>required</i>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ValidationGroup="Contact"
                                                ControlToValidate="txtState" ErrorMessage="State abbreviation is required." CssClass="text-error" Display="Dynamic" />
                                            <asp:RegularExpressionValidator ID="regexState" runat="server" ValidationExpression="[a-zA-Z].*" ValidationGroup="Contact"
                                                ControlToValidate="txtState" ErrorMessage="State must start with an alphabetic character." CssClass="text-error" Display="Dynamic" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>Postal Code</th>
                                        <td>
                                            <asp:TextBox ID="txtPostalCode" runat="server" MaxLength="10" ValidationGroup="Contact" />
                                            <i>required</i>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ValidationGroup="Contact"
                                                ControlToValidate="txtPostalCode" ErrorMessage="Postal Code is required." CssClass="text-error" Display="Dynamic" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>Telephone Number</th>
                                        <td>
                                            <asp:TextBox ID="txtTelephone" runat="server" MaxLength="30" ValidationGroup="Contact" />
                                            <i>required</i>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ValidationGroup="Contact"
                                                ControlToValidate="txtTelephone" ErrorMessage="Phone number is required." CssClass="text-error" Display="Dynamic" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>Primary Contact Email</th>
                                        <td>
                                            <asp:TextBox ID="txtEmail" runat="server" MaxLength="100" ValidationGroup="Contact" />
                                            <i id="lPrimaryEmailRequired" runat="server">required</i>
                                            <asp:RequiredFieldValidator ID="reqPrimaryEmail" runat="server" ValidationGroup="Contact"
                                                ControlToValidate="txtEmail" ErrorMessage="Email address is required." CssClass="text-error" Display="Dynamic" />
                                            <asp:RegularExpressionValidator ID="regexEmail" runat="server" ValidationGroup="Contact"
                                                ControlToValidate="txtEmail" ErrorMessage="Email address not valid." CssClass="text-error" Display="Dynamic"
                                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                                        </td>
                                    </tr>
                                </table>

                                <p id="pContactSaveError" runat="server" visible="false" class="message-warning anim-fade-in fast">
                                    There was an error while saving. Please try again.
                                </p>

                                <p id="pContactSaveSuccess" runat="server" visible="false" class="message-success anim-fade-in fast">
                                    Primary contact saved.
                                </p>

                                <p>
                                    <asp:Button ID="btnSaveContact" runat="server" Text="Save Contact Info" ValidationGroup="Contact" />
                                </p>
                            </asp:Panel>

                            <asp:Panel ID="pnlElectronicCommunication" runat="server" Visible="false">
                                <h3 id="emails">Additional Email Recipients (optional)</h3>

                                <p id="pEmailRemovedSuccess" runat="server" visible="false" class="message-success anim-fade-in fast">
                                    The email was removed.
                                </p>
                                <p id="pEmailListError" runat="server" visible="false" class="message-warning anim-fade-in fast">
                                    There was an error. Please try again.
                                </p>
                                <p id="pEmailAddedSuccess" runat="server" visible="false" class="message-success anim-fade-in fast">
                                    The address has been added, and a notification email has been sent.
                                </p>
                                <p id="pEmailAlreadyRemoved" runat="server" visible="false" class="message-warning anim-fade-in fast">
                                    The email selected has already been removed.
                                </p>

                                <% If CurrentCommunicationInfo.Emails.Count = 0 Then %>
                                <p><em>None added.</em></p>
                                <% Else %>

                                <ul class="list-with-actions text-small">
                                    <asp:Repeater ID="rptEmails" runat="server">
                                        <ItemTemplate>
                                            <li>
                                                <%# Container.DataItem %>
                                                <details>
                                                    <summary class="button-link" role="button">Remove</summary>
                                                    <div class="anim-fade-in fast" role="dialog">
                                                        <p>Are you sure you want to remove this email address?</p>
                                                        <asp:Button ID="btnRemoveEmail" runat="server"
                                                            Text="Remove" OnClick="RemoveEmail" CommandArgument='<%# Container.DataItem %>' UseSubmitBehavior="False" />
                                                    </div>
                                                </details>
                                                <span id="a2" runat="server" visible="<%# Container.DataItem = AddedEmail %>" class="text-success"><i>Added</i></span>
                                            </li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ul>

                                <% End If %>

                                <h4>Add a new email recipient:</h4>
                                <asp:Panel ID="pnlAddEmail" runat="server" DefaultButton="btnAddNewEmail">
                                    <p id="pAddEmailError" runat="server" visible="false" class="message-warning anim-fade-in fast">
                                        There was an error while saving. Please try again.
                                    </p>
                                    <p id="pAddEmailInvalid" runat="server" visible="false" class="message-highlight anim-fade-in fast">
                                        The email entered is invalid. Please try again.
                                    </p>
                                    <p id="pAddEmailExists" runat="server" visible="false" class="message-highlight anim-fade-in fast">
                                        That email has already been added.
                                    </p>
                                    <p id="pAddEmailSuccess" runat="server" visible="false" class="message-success anim-fade-in fast">
                                        The email address has been added, and a notification email has been sent to the recipient.
                                    </p>
                                    <p>
                                        <asp:TextBox ID="txtNewEmail" runat="server" ValidationGroup="NewEmail" />
                                        <asp:Button ID="btnAddNewEmail" runat="server" ValidationGroup="NewEmail" Text="Add" />
                                        <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ValidationGroup="NewEmail"
                                            ControlToValidate="txtNewEmail" ErrorMessage="Email address is required." CssClass="text-error" Display="Dynamic" />
                                        <asp:RegularExpressionValidator ID="regexNewEmail" runat="server" ValidationGroup="Contact"
                                            ControlToValidate="txtNewEmail" ErrorMessage="Email address not valid." CssClass="text-error" Display="Dynamic"
                                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                                        <br />
                                        A notification email will be sent to the recipient.
                                    </p>
                                </asp:Panel>
                            </asp:Panel>

                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </tbody>
    </table>
    <script src="https://unpkg.com/details-element-polyfill@2.4.0/dist/details-element-polyfill.js"
        integrity="sha384-e9xku4VSRQ/IhD1GrrEGl4DR0H68G1fI1qUeiY9f6aSYKSAQJDNWwLiBq8uZB7aD" crossorigin="anonymous"></script>
</asp:Content>
