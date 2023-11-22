<%@ Page MasterPageFile="~/EIS/EIS.master" Language="VB" AutoEventWireup="false"
    Title="GECO Emissions Inventory"
    Inherits="GECO.EIS_Default" CodeBehind="Default.aspx.vb" %>

<%@ MasterType VirtualPath="~/EIS/EIS.master" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="Server">

    <p>
        For more information on how to submit your Emissions Inventory, visit 
        <a href="https://epd.georgia.gov/forms-permits/air-protection-branch-forms-permits/point-source-emissions-inventory"
            rel="noopener" target="_blank">https://epd.georgia.gov/forms-permits/air-protection-branch-forms-permits/point-source-emissions-inventory</a>.
    </p>

    <p>
        <asp:Label ID="lblEnrollmentStatus" runat="server" Visible="false" CssClass="message-highlight" Font-Bold="True" />
    </p>

    <h2 id="facility-info">1. Facility Information</h2>

    <p>
        Review the facility information below and if there is any mistake, please email: 
        <a href="mailto:emissions.inventory@dnr.ga.gov">emissions.inventory@dnr.ga.gov</a>.
    </p>

    <table class="table-simple table-list" aria-labelledby="facility-info">
        <tbody>
            <tr>
                <th>Description</th>
                <td>
                    <asp:Label ID="lblDescription" runat="server" />
                </td>
            </tr>
            <tr>
                <th>Operating Status</th>
                <td>
                    <asp:Label ID="lblOperatingStatus" runat="server" /><br />
                    <small>* Operating status pertains only to the last Emissions Inventory reported</small>
                </td>
            </tr>
            <tr>
                <th>NAICS Code</th>
                <td>
                    <asp:Label ID="lblNAICS" runat="server" />
                </td>
            </tr>
            <tr>
                <th>Site Address</th>
                <td>
                    <asp:Label ID="lblSiteAddress" runat="server" />
                </td>
            </tr>
            <tr>
                <th>Geocoordinates</th>
                <td>
                    <asp:Label ID="lblLatitude" runat="server" />,
                        <asp:Label ID="lblLongitude" runat="server" />
                </td>
            </tr>
        </tbody>
    </table>

    <div>
        <asp:HyperLink ID="lnkGoogleMap" runat="server" rel="noopener" Target="_blank" CssClass="no-visited">
            <asp:Image ID="imgGoogleStaticMap" runat="server" BorderStyle="Solid" BorderWidth="2px" /><br />
            Open map in new window
        </asp:HyperLink>
    </div>

    <h2 id="caers-users">2. CAERS Users</h2>

    <p>
        <asp:HyperLink ID="lnkOtherContactInfo" runat="server" NavigateUrl="~/Facility/Contacts.aspx" Target="_blank">
            Please ensure all contact information is correct in communication preferences before proceeding 
            to update CAERS contact information.</asp:HyperLink>
    </p>
    <p>
        Next add and update CAERS users below. Use of CAERS requires one certifier and one or more 
        preparers. If a single person serves both roles, they must be added as both.
    </p>

    <asp:UpdatePanel ID="updAddNew" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hidCertifiersCount" runat="server" />
            <asp:HiddenField ID="hidPreparersCount" runat="server" />

            <h3>Current CAERS Users</h3>
            <p id="pNoUsersNotice" runat="server" visible="false"><em>No CAERS User have been added.</em></p>

            <% If hidCertifiersCount.Value = 0 Then %>
            <p class="message-highlight">A certifier has not been added. Use of CAERS requires a certifier.</p>
            <% ElseIf hidCertifiersCount.Value > 1 Then %>
            <p class="message-warning">Only one certifier is allowed.</p>
            <% End If %>

            <% If hidPreparersCount.Value = 0 Then %>
            <p class="message-highlight">A preparer has not been added. Use of CAERS requires at least one preparer.</p>
            <% End If %>

            <asp:Panel ID="pnlAddNew" runat="server" CssClass="panel" Visible="false" DefaultButton="btnSaveNew">
                <h3>Add New User</h3>
                <asp:ValidationSummary ID="ValidationSummaryNew" runat="server" HeaderText="Please correct the following errors:"></asp:ValidationSummary>

                <table class="table-simple table-list" aria-label="CAERS new user entry form">
                    <tr>
                        <th>
                            <asp:Label ID="lblRoleNew" runat="server" AssociatedControlID="rRoleNew">CAERS Role</asp:Label>
                        </th>
                        <td>
                            <asp:RadioButtonList ID="rRoleNew" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal">
                                <asp:ListItem>Preparer</asp:ListItem>
                                <asp:ListItem>Certifier</asp:ListItem>
                                <asp:ListItem>Both</asp:ListItem>
                            </asp:RadioButtonList>
                            <asp:RequiredFieldValidator ID="reqvRoleNew" runat="server" ControlToValidate="rRoleNew" Display="Dynamic"
                                ErrorMessage="The CAERS Role is required.">*</asp:RequiredFieldValidator>

                            <asp:RadioButtonList ID="rRolePreparer" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal"
                                Enabled="false" Visible="false">
                                <asp:ListItem Selected="True">Preparer</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            <asp:Label ID="lblPrefixNew" runat="server"
                                AssociatedControlID="txtPrefixNew">Honorific</asp:Label>
                        </th>
                        <td>
                            <asp:TextBox ID="txtPrefixNew" runat="server" MaxLength="15" />
                        </td>
                    </tr>
                    <tr>
                        <th>
                            <asp:Label ID="lblFirstNameNew" runat="server"
                                AssociatedControlID="txtFirstNameNew">First Name</asp:Label>
                        </th>
                        <td>
                            <asp:TextBox ID="txtFirstNameNew" runat="server" />
                            <asp:RequiredFieldValidator ID="reqvFirstNameNew" runat="server" ControlToValidate="txtFirstNameNew" Display="Dynamic"
                                ErrorMessage="The first name is required.">*</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="regexFirstName" runat="server" ValidationExpression="^[a-zA-Z]" Display="Dynamic"
                                ControlToValidate="txtFirstNameNew" ErrorMessage="Name must start with an alphabetic character." CssClass="text-error" />
                        </td>
                    </tr>
                    <tr>
                        <th>
                            <asp:Label ID="lblLastNameNew" runat="server"
                                AssociatedControlID="txtLastNameNew">Last Name</asp:Label>
                        </th>
                        <td>
                            <asp:TextBox ID="txtLastNameNew" runat="server" />
                            <asp:RequiredFieldValidator ID="reqvLastNameNew" runat="server" ControlToValidate="txtLastNameNew" Display="Dynamic"
                                ErrorMessage="The last name is required.">*</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="regexLastNameNew" runat="server" ValidationExpression="^[a-zA-Z]" Display="Dynamic"
                                ControlToValidate="txtLastNameNew" ErrorMessage="Name must start with an alphabetic character." CssClass="text-error" />
                        </td>
                    </tr>
                    <tr>
                        <th>
                            <asp:Label ID="lblTitleNew" runat="server"
                                AssociatedControlID="txtTitleNew">Title</asp:Label>
                        </th>
                        <td>
                            <asp:TextBox ID="txtTitleNew" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <th>
                            <asp:Label ID="lblCompanyNew" runat="server"
                                AssociatedControlID="txtCompanyNew">Company</asp:Label>
                        </th>
                        <td>
                            <asp:TextBox ID="txtCompanyNew" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <th>
                            <asp:Label ID="lblStreetNew" runat="server"
                                AssociatedControlID="txtStreetNew">Mailing Address</asp:Label>
                        </th>
                        <td>
                            <asp:TextBox ID="txtStreetNew" runat="server" />
                            <asp:RequiredFieldValidator ID="reqvStreetNew" runat="server" ControlToValidate="txtStreetNew" Display="Static"
                                ErrorMessage="The mailing address is required.">*</asp:RequiredFieldValidator>
                            <br />
                            <asp:TextBox ID="txtStreet2New" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <th>
                            <asp:Label ID="lblCityNew" runat="server"
                                AssociatedControlID="txtCityNew">City</asp:Label>
                        </th>
                        <td>
                            <asp:TextBox ID="txtCityNew" runat="server" />
                            <asp:RequiredFieldValidator ID="reqvCityNew" runat="server" Display="Dynamic"
                                ControlToValidate="txtCityNew" ErrorMessage="The city is required.">*</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="regexCityNew" runat="server" ValidationExpression="^[a-zA-Z]" Display="Dynamic"
                                ControlToValidate="txtCityNew" ErrorMessage="City must start with an alphabetic character." CssClass="text-error" />
                        </td>
                    </tr>
                    <tr>
                        <th>
                            <asp:Label ID="lblStateNew" runat="server"
                                AssociatedControlID="ddlStateNew">State</asp:Label>
                        </th>
                        <td>
                            <asp:DropDownList ID="ddlStateNew" runat="server" />
                            <asp:RequiredFieldValidator ID="reqvStateNew" ControlToValidate="ddlStateNew" Display="Dynamic"
                                InitialValue="--Select a State--" runat="server" ErrorMessage="The state is required.">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            <asp:Label ID="lblPostalCodeNew" runat="server"
                                AssociatedControlID="txtPostalCodeNew">Postal Code</asp:Label>
                        </th>
                        <td>
                            <asp:TextBox ID="txtPostalCodeNew" runat="server" />
                            <asp:RequiredFieldValidator ID="reqvPostalCodeNew" runat="server" Display="Dynamic"
                                ControlToValidate="txtPostalCodeNew" ErrorMessage="The postal code is required.">*</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="rgxvPostalCodeNew" runat="server" Display="Dynamic"
                                ControlToValidate="txtPostalCodeNew" ErrorMessage="Please check the contact mailing address postal code format." CssClass="text-error"
                                ValidationExpression="^(\d{5})(-\d{4})?$">Format must be either 99999 or 99999-9999</asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            <asp:Label ID="lblTelephoneNew" runat="server"
                                AssociatedControlID="txtTelephoneNew">Phone Number</asp:Label>
                        </th>
                        <td>
                            <asp:TextBox ID="txtTelephoneNew" runat="server" MaxLength="30" />
                        </td>
                    </tr>
                    <tr>
                        <th>
                            <asp:Label ID="lblEmailNew" runat="server"
                                AssociatedControlID="txtEmailNew">Email</asp:Label>
                        </th>
                        <td>
                            <asp:TextBox ID="txtEmailNew" runat="server" />
                            <asp:RequiredFieldValidator ID="reqvEmailNew" runat="server" ControlToValidate="txtEmailNew" Display="Dynamic"
                                ErrorMessage="The email address is required.">*</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="rgxvEmailNew" runat="server" Display="Dynamic"
                                ControlToValidate="txtEmailNew" ErrorMessage="Email address not valid." CssClass="text-error"
                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                            <asp:CustomValidator ID="custEmailNew" runat="server" ControlToValidate="txtEmailNew" Display="Dynamic"
                                ErrorMessage="A Preparer with that email already exists for this facility.">*</asp:CustomValidator>
                        </td>
                    </tr>
                </table>

                <p>
                    <asp:Button ID="btnSaveNew" runat="server" Text="Save" />
                    <asp:Button ID="btnCancelNew" runat="server" CausesValidation="false"
                        Text="Cancel" CssClass="button-cancel" />
                </p>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="updCurrentUsers" runat="server">
        <ContentTemplate>
            <asp:GridView ID="grdCaersUsers" runat="server" DataKeyNames="Id" AutoGenerateColumns="false" CssClass="table-simple">
                <Columns>
                    <asp:TemplateField HeaderText="Role">
                        <ItemTemplate><%# Eval("CaerRole") %></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="User">
                        <ItemTemplate>
                            <%# Eval("Honorific") %> <%# Eval("FirstName") %> <%# Eval("LastName") %><br />
                            <%# If(String.IsNullOrWhiteSpace(Eval("Title").ToString()), "", $"{Eval("Title")}<br />") %>
                            <%# Eval("Company") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Address">
                        <ItemTemplate>
                            <%# Eval("Street") %><br />
                            <%# If(String.IsNullOrWhiteSpace(Eval("Street2").ToString()), "", $"{Eval("Street2")}<br />") %>
                            <%# Eval("City")%>, <%# Eval("State") %> <%# Eval("PostalCode") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Email">
                        <ItemTemplate><%# Eval("Email") %></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Phone">
                        <ItemTemplate><%# Eval("PhoneNumber") %></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Controls">
                        <ItemTemplate>
                            <asp:LinkButton ID="EditButton" runat="server"
                                CommandName="Edit" Text="Edit" CausesValidation="False" />
                            <asp:LinkButton ID="DeleteButton" runat="server"
                                CommandName="Delete" Text="Delete" CausesValidation="False"
                                OnClientClick="return confirm('Are you sure you want to delete this record?');" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

            <asp:Panel ID="pnlEditUser" runat="server" CssClass="panel" Visible="false" DefaultButton="btnSaveEdit">
                <h3>Edit User</h3>
                <asp:ValidationSummary ID="ValidationSummaryEdit" runat="server" HeaderText="Please correct the following errors" />

                <table class="table-simple table-list" aria-label="CAERS user edit form">
                    <tr>
                        <th>
                            <asp:Label ID="lblRoleEdit" runat="server"
                                AssociatedControlID="ddlRoleEdit">CAERS Role</asp:Label>
                        </th>
                        <td>
                            <asp:DropDownList ID="ddlRoleEdit" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <th>
                            <asp:Label ID="lblPrefixEdit" runat="server"
                                AssociatedControlID="txtPrefixEdit">Honorific</asp:Label>
                        </th>
                        <td>
                            <asp:TextBox ID="txtPrefixEdit" runat="server" MaxLength="15" />
                        </td>
                    </tr>
                    <tr>
                        <th>
                            <asp:Label ID="lblFirstNameEdit" runat="server"
                                AssociatedControlID="txtFirstNameEdit">First Name</asp:Label>
                        </th>
                        <td>
                            <asp:TextBox ID="txtFirstNameEdit" runat="server" />
                            <asp:RequiredFieldValidator ID="reqvFirstNameEdit" runat="server" ControlToValidate="txtFirstNameEdit" Display="Dynamic"
                                ErrorMessage="The first name is required.">*</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="regexFirstNameEdit" runat="server" ValidationExpression="^[a-zA-Z]" Display="Dynamic"
                                ControlToValidate="txtFirstNameEdit" ErrorMessage="Name must start with an alphabetic character." CssClass="text-error" />
                        </td>
                    </tr>
                    <tr>
                        <th>
                            <asp:Label ID="lblLastNameEdit" runat="server"
                                AssociatedControlID="txtLastNameEdit">Last Name</asp:Label>
                        </th>
                        <td>
                            <asp:TextBox ID="txtLastNameEdit" runat="server" />
                            <asp:RequiredFieldValidator ID="reqvLastNameEdit" runat="server" ControlToValidate="txtLastNameEdit" Display="Dynamic"
                                ErrorMessage="The last name is required.">*</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="regexLastNameEdit" runat="server" ValidationExpression="^[a-zA-Z]" Display="Dynamic"
                                ControlToValidate="txtLastNameEdit" ErrorMessage="Name must start with an alphabetic character." CssClass="text-error" />
                        </td>
                    </tr>
                    <tr>
                        <th>
                            <asp:Label ID="lblTitleEdit" runat="server"
                                AssociatedControlID="txtTitleEdit">Title</asp:Label>
                        </th>
                        <td>
                            <asp:TextBox ID="txtTitleEdit" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <th>
                            <asp:Label ID="lblCompanyEdit" runat="server"
                                AssociatedControlID="txtCompanyEdit">Company</asp:Label>
                        </th>
                        <td>
                            <asp:TextBox ID="txtCompanyEdit" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <th>
                            <asp:Label ID="lblStreetEdit" runat="server"
                                AssociatedControlID="txtStreetEdit">Mailing Address</asp:Label>
                        </th>
                        <td>
                            <asp:TextBox ID="txtStreetEdit" runat="server" />
                            <asp:RequiredFieldValidator ID="reqvStreetEdit" runat="server" ControlToValidate="txtStreetEdit" Display="Dynamic"
                                ErrorMessage="The mailing address is required.">*</asp:RequiredFieldValidator>
                            <br />
                            <asp:TextBox ID="txtStreet2Edit" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <th>
                            <asp:Label ID="lblCityEdit" runat="server"
                                AssociatedControlID="txtCityEdit">City</asp:Label>
                        </th>
                        <td>
                            <asp:TextBox ID="txtCityEdit" runat="server" />
                            <asp:RequiredFieldValidator ID="reqvCityEdit" runat="server" Display="Dynamic"
                                ControlToValidate="txtCityEdit" ErrorMessage="The city is required.">*</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="regexCityEdit" runat="server" ValidationExpression="^[a-zA-Z]" Display="Dynamic"
                                ControlToValidate="txtCityEdit" ErrorMessage="City must start with an alphabetic character." CssClass="text-error" />
                        </td>
                    </tr>
                    <tr>
                        <th>
                            <asp:Label ID="lblStateEdit" runat="server"
                                AssociatedControlID="ddlStateEdit">State</asp:Label>
                        </th>
                        <td>
                            <asp:DropDownList ID="ddlStateEdit" runat="server" />
                            <asp:RequiredFieldValidator ID="reqvStateEdit" ControlToValidate="ddlStateEdit" Display="Dynamic"
                                InitialValue="--Select a State--" runat="server" ErrorMessage="The state is required.">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            <asp:Label ID="lblPostalCodeEdit" runat="server"
                                AssociatedControlID="txtPostalCodeEdit">Postal Code</asp:Label>
                        </th>
                        <td>
                            <asp:TextBox ID="txtPostalCodeEdit" runat="server" />
                            <asp:RequiredFieldValidator ID="reqvPostalCodeEdit" runat="server" Display="Dynamic"
                                ControlToValidate="txtPostalCodeEdit" ErrorMessage="The postal code is required.">*</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="rgxvPostalCodeEdit" runat="server" Display="Dynamic" CssClass="text-error"
                                ControlToValidate="txtPostalCodeEdit" ErrorMessage="Please check the contact mailing address postal code format."
                                ValidationExpression="^(\d{5})(-\d{4})?$">Format must be either 99999 or 99999-9999</asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            <asp:Label ID="lblTelephoneEdit" runat="server"
                                AssociatedControlID="txtTelephoneEdit">Phone Number</asp:Label>
                        </th>
                        <td>
                            <asp:TextBox ID="txtTelephoneEdit" runat="server" MaxLength="30" />
                        </td>
                    </tr>
                    <tr>
                        <th>
                            <asp:Label ID="lblEmailEdit" runat="server"
                                AssociatedControlID="txtEmailEdit">Email</asp:Label>
                        </th>
                        <td>
                            <asp:TextBox ID="txtEmailEdit" runat="server" />
                            <asp:RequiredFieldValidator ID="reqvEmailEdit" runat="server" ControlToValidate="txtEmailEdit" Display="Dynamic"
                                ErrorMessage="The email address is required.">*</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="rgxvEmailEdit" runat="server" Display="Dynamic"
                                ControlToValidate="txtEmailEdit" ErrorMessage="Email address not valid." CssClass="text-error"
                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                            <asp:CustomValidator ID="custEmailEdit" runat="server" ControlToValidate="txtEmailEdit" Display="Dynamic"
                                ErrorMessage="A Preparer with that email already exists for this facility.">*</asp:CustomValidator>
                        </td>
                    </tr>
                </table>

                <p>
                    <asp:HiddenField ID="hidEditId" runat="server" />
                    <asp:Button ID="btnSaveEdit" runat="server" Text="Save" />
                    <asp:Button ID="btnCancelEdit" runat="server" CausesValidation="false"
                        Text="Cancel" CssClass="button-cancel" />
                </p>
            </asp:Panel>

            <p id="pAddNew" runat="server">
                <asp:Button ID="btnAddNew" runat="server" Text="Add New CAERS User" />
            </p>

        </ContentTemplate>
    </asp:UpdatePanel>

    <div id="submitEiSection" runat="server">
        <h2 id="submit-ei">3. Submit EI</h2>

        <p>
            If your facility qualifies to opt out, please download the 
            <a href="2023-opt-out-form_triennial.xlsx" download="2023-opt-out-form_triennial.xlsx">Opt-out form</a>
            to start your EI reporting. Upload the completed form to CAERS.
        </p>
        <p>
            If new to CDX/CAERS, the preparer(s) and certifier you have specified should follow this procedure:
        </p>
        <ol>
            <li>
                <p>Register in CDX using the link to EPA's CDX above and set up CAERS in CDX.</p>
            </li>
            <li>
                <p>Await email approval from CDX that their CAERS account is linked to the correct facilities.</p>
            </li>
            <li>
                <p>Once approved, select facility, then click on the “Create New Report” button for the Report.</p>
            </li>
        </ol>

        <p class="centered">
            <asp:HyperLink ID="CdxLink" runat="server" Visible="false" Text="Link to EPA CDX" Target="_blank" CssClass="button button-large button-proceed" />
            <asp:Label ID="lblCdxAlt" runat="server" Visible="false" CssClass="message-highlight" Font-Bold="True" Text="EI not applicable." />
        </p>
    </div>
</asp:Content>
