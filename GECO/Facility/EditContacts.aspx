<%@ Page Language="VB" MasterPageFile="~/Main.master" AutoEventWireup="false"
    Inherits="GECO.EditContacts" Title="GECO Edit Facility Contacts" CodeBehind="EditContacts.aspx.vb" %>

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
                    <a href="EditContacts.aspx" class="disabled no-visited selected" aria-disabled="true" disabled>Permit Fees</a>
                    <a href="EditContacts.aspx" class="no-visited">Permit Applications</a>
                    <a href="EditContacts.aspx" class="no-visited">Emissions Inventory</a>
                </td>
                <td>
                    <h2>Communication Preference for <em>Permit Fees</em></h2>

                    <asp:RadioButtonList ID="rbCommPref" runat="server">
                        <asp:ListItem Value="electronic">Receive electronic communications only. By selecting this options, you will no 
                                longer receive mailed communications.</asp:ListItem>
                        <asp:ListItem Value="mail">Continue to receive mailed communications only.</asp:ListItem>
                        <asp:ListItem Value="both" Selected="True">Receive both electronic and mailed communications.</asp:ListItem>
                    </asp:RadioButtonList>

                    <p class="message-highlight">Communication will continue to be sent by mail until email recipients have been verified.</p>
                    <p>
                        <asp:Button ID="btnSavePref" runat="server" Text="Save Communication Preferences" />
                    </p>

                    <h3>Primary Contact</h3>

                    <table class="table-simple table-list">
                        <tr>
                            <th>First Name</th>
                            <td>
                                <asp:TextBox ID="txtFName" runat="server" ValidationGroup="Contact"></asp:TextBox>
                                <i>required</i>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtFName"
                                    ErrorMessage="Type First Name" ValidationGroup="Contact"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <th>Last Name</th>
                            <td>
                                <asp:TextBox ID="txtLName" runat="server" ValidationGroup="Contact"></asp:TextBox>
                                <i>required</i>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtLName"
                                    ErrorMessage="Type Last Name" ValidationGroup="Contact"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <th>Title</th>
                            <td>
                                <asp:TextBox ID="txtTitle" runat="server" ValidationGroup="Contact"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>Company Name</th>
                            <td>
                                <asp:TextBox ID="txtCoName" runat="server" ValidationGroup="Contact"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>Street Address</th>
                            <td>
                                <asp:TextBox ID="txtAddress" runat="server" ValidationGroup="Contact"></asp:TextBox>
                                <i>required</i>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtAddress"
                                    ErrorMessage="Type Street Address" ValidationGroup="Contact"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <th>City</th>
                            <td>
                                <asp:TextBox ID="txtCity" runat="server" ValidationGroup="Contact"></asp:TextBox>
                                <i>required</i>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="txtCity"
                                    ErrorMessage="Type City Name" ValidationGroup="Contact"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <th>State</th>
                            <td>
                                <asp:TextBox ID="txtState" runat="server" MaxLength="2"
                                    ValidationGroup="Contact"></asp:TextBox>
                                <i>required</i>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator12"
                                    runat="server" ControlToValidate="txtState" ErrorMessage="State Abbreviation"
                                    ValidationGroup="Contact"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <th>Zip Code</th>
                            <td>
                                <asp:TextBox ID="txtZip" runat="server" MaxLength="5" ValidationGroup="Contact" />
                                <i>required</i>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="txtzip"
                                    ErrorMessage="Type 5-digit Zip Code" ValidationGroup="Contact"></asp:RequiredFieldValidator>
                                <act:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtZip"
                                    FilterType="Numbers" Enabled="True">
                                </act:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <th>Telephone Number</th>
                            <td>
                                <asp:TextBox ID="txtPhone" runat="server" MaxLength="30" ValidationGroup="Contact"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server"
                                    ControlToValidate="txtPhone" ErrorMessage="Phone Number is required"
                                    ValidationGroup="Contact" />
                            </td>
                        </tr>
                    </table>
                    <p>
                        <asp:Button ID="btnSaveContact" runat="server" Text="Save Contact" />
                    </p>

                    <h3>Email Contacts</h3>

                    <h4>Current recipients:</h4>
                    <ul class="flush">
                        <li>a.person@example.com (<a href="#">remove</a>)</li>
                        <li>b.person@example.com (<a href="#">remove</a>)</li>
                    </ul>

                    <h4>Unverified recipients:</h4>
                    <p>The following email addresses have been added but haven't yet been verified.</p>
                    <ul class="flush">
                        <li>c.person@example.com (<a href="#">resend verification email</a>) (<a href="#">remove</a>)</li>
                        <li>d.person@example.com (<a href="#">resend verification email</a>) (<a href="#">remove</a>)</li>
                    </ul>

                    <h4>Add a new email recipient:</h4>
                    <p>
                        <asp:TextBox ID="txtNewEmail" runat="server" />
                        <asp:Button ID="btnAddNewEmail" runat="server" Text="Add" />
                    </p>
                </td>
            </tr>
        </tbody>
    </table>
</asp:Content>
