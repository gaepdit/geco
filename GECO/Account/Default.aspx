<%@ Page Title="" Language="VB" MasterPageFile="~/MainMaster.master" AutoEventWireup="false" Inherits="GECO.Account_Default" CodeBehind="Default.aspx.vb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="FullContent" runat="Server">
    <h1>Account</h1>

    <p>
        Logged in as
        <asp:Label runat="server" ID="lblDisplayName"></asp:Label>.
    </p>

    <p>
        <asp:Button ID="hlSignOut" runat="server" Text="Sign Out" PostBackUrl="~/Default.aspx?do=SignOut"></asp:Button>
    </p>

    <h2>Email</h2>

    <asp:UpdatePanel ID="UpdatePanel_email" runat="server">
        <ContentTemplate>
            <p>
                <asp:Label ID="lblEmailMessage" runat="server" CssClass="message-update"></asp:Label>
            </p>
            <table>
                <tr>
                    <td align="right">Email address:</td>
                    <td>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="unwatermarked"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtEmail"
                            ErrorMessage="Email address is required." Font-Size="Small" ValidationGroup="Email"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtEmail"
                            ErrorMessage="A valid email address is required." Font-Size="Small"
                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                            Display="Dynamic" ValidationGroup="Email"></asp:RegularExpressionValidator>
                    </td>
                </tr>
            </table>

            <p>
                <asp:Button ID="btnSaveEmail" runat="server" Text="Update Email" CausesValidation="true" ValidationGroup="Email" />
            </p>
        </ContentTemplate>
    </asp:UpdatePanel>

    <p>NOTE: A change in email will not take effect until after the new email address has been confirmed.</p>

    <h2>Password</h2>

    <asp:UpdatePanel ID="UpdatePanel_password" runat="server">
        <ContentTemplate>
            <p>
                <asp:Label ID="lblPasswordMessage" runat="server" CssClass="message-update"></asp:Label>
            </p>

            <table>
                <tr>
                    <td align="right">Old Password:</td>
                    <td>
                        <asp:TextBox ID="txtOldPassword" runat="server" TextMode="Password" CssClass="unwatermarked"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtOldPassword" Display="Dynamic"
                            ErrorMessage="Old Password is required." Font-Size="Small" ValidationGroup="Password"></asp:RequiredFieldValidator>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td align="right">New Password:</td>
                    <td>
                        <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password" CssClass="unwatermarked"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="txtNewPassword" Display="Dynamic"
                            ErrorMessage="New Password is required." Font-Size="Small" ValidationGroup="Password"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="Regex3" runat="server" ControlToValidate="txtNewPassword"
                            ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$" ValidationGroup="Password"
                            ErrorMessage="Password does not meet complexity requirements."
                            ForeColor="Red" />

                    </td>
                </tr>
                <tr>
                    <td align="right">Confirm New Password:</td>
                    <td>
                        <asp:TextBox ID="txtPwdConfirm" runat="server" TextMode="Password" CssClass="unwatermarked"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="txtPwdConfirm" Display="Dynamic"
                            ErrorMessage="Password Confirmation is required." Font-Size="Small" ValidationGroup="Password"></asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txtNewPassword" ControlToValidate="txtPwdConfirm"
                            ErrorMessage="Password fields must match." Font-Size="Small" Display="Dynamic" ValidationGroup="Password"></asp:CompareValidator>&nbsp;
                    </td>
                </tr>
            </table>

            <p>
                <asp:Button ID="btnPwdUpdate" runat="server" Text="Update Password" CausesValidation="true" ValidationGroup="Password" />
            </p>
            <p>NOTE: New password must contain at least 8 characters with at least 1 uppercase letter, 1 lowercase letter and 1 number.</p>
        </ContentTemplate>
    </asp:UpdatePanel>

    <h2>Profile</h2>

    <asp:UpdatePanel ID="UpdatePanel_profile" runat="server">
        <ContentTemplate>
            <p>
                <asp:Label ID="lblProfileMessage" runat="server" CssClass="message-update"></asp:Label>
            </p>

            <table>
                <tr>
                    <td align="right">Salutation:</td>
                    <td>
                        <asp:TextBox ID="txtSalutation" runat="server" CssClass="unwatermarked" MaxLength="5">
                        </asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="right">First Name:</td>
                    <td>
                        <asp:TextBox ID="txtFName" runat="server" CssClass="unwatermarked">
                        </asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                            ControlToValidate="txtFName" ErrorMessage="First Name is required." Font-Size="Small"
                            ValidationGroup="Profile"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td align="right">Last Name:</td>
                    <td>
                        <asp:TextBox ID="txtLName" runat="server" CssClass="unwatermarked"></asp:TextBox><asp:RequiredFieldValidator
                            ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtLName" ErrorMessage="Last Name is required."
                            Font-Size="Small" ValidationGroup="Profile"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td align="right">Title:</td>
                    <td>
                        <asp:TextBox ID="txtTitle" runat="server" CssClass="unwatermarked"></asp:TextBox><asp:RequiredFieldValidator
                            ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtTitle" ErrorMessage="Title is required."
                            Font-Size="Small" ValidationGroup="Profile"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td align="right">Company Name:</td>
                    <td>
                        <asp:TextBox ID="txtCoName" runat="server" CssClass="unwatermarked"></asp:TextBox><asp:RequiredFieldValidator
                            ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtCoName" ErrorMessage="Company Name is required."
                            Font-Size="Small" ValidationGroup="Profile"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td align="right">Street Address:</td>
                    <td>
                        <asp:TextBox ID="txtAddress" runat="server" CssClass="unwatermarked"></asp:TextBox><asp:RequiredFieldValidator
                            ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtAddress" ErrorMessage="Street Address is required."
                            Font-Size="Small" ValidationGroup="Profile"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td align="right">City:</td>
                    <td>
                        <asp:TextBox ID="txtCity" runat="server" CssClass="unwatermarked"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="txtCity"
                            ErrorMessage="City Name is required." Font-Size="Small" ValidationGroup="Profile"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td align="right">State:</td>
                    <td>
                        <asp:TextBox ID="txtState" runat="server" CssClass="unwatermarked" MaxLength="2"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="txtState"
                            ErrorMessage="State Abbreviation is required." Font-Size="Small" ValidationGroup="Profile"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td align="right">Zip Code:</td>
                    <td>
                        <asp:TextBox ID="txtZip" runat="server" CssClass="unwatermarked" MaxLength="5"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="txtZip"
                            ErrorMessage="Zip Code is required." Font-Size="Small" ValidationGroup="Profile"></asp:RequiredFieldValidator>&nbsp;
                        <act:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtZip"
                            FilterType="Numbers">
                        </act:FilteredTextBoxExtender>
                    </td>
                </tr>
                <tr>
                    <td align="right">Telephone Number:</td>
                    <td>
                        <asp:TextBox ID="txtPhone" runat="server" CssClass="unwatermarked" MaxLength="10"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5"
                            runat="server" ControlToValidate="txtPhone" ErrorMessage="Phone Number is required."
                            Font-Size="Small" ValidationGroup="Profile"></asp:RequiredFieldValidator>
                        <act:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtPhone"
                            FilterType="Numbers">
                        </act:FilteredTextBoxExtender>
                    </td>
                </tr>
                <tr>
                    <td align="right">Telephone Ext:</td>
                    <td>
                        <asp:TextBox ID="txtPhoneExt" runat="server" CssClass="unwatermarked" MaxLength="5"
                            Width="64px"></asp:TextBox>
                        <act:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtPhoneExt"
                            FilterType="Numbers">
                        </act:FilteredTextBoxExtender>
                    </td>
                </tr>
                <tr>
                    <td align="right">Fax Number:</td>
                    <td>
                        <asp:TextBox ID="txtFax" runat="server" CssClass="unwatermarked" MaxLength="10"></asp:TextBox>
                        <act:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtFax"
                            FilterType="Numbers">
                        </act:FilteredTextBoxExtender>
                    </td>
                </tr>
                <tr>
                    <td align="right">User Type:</td>
                    <td>
                        <asp:DropDownList ID="ddlUserType" runat="server">
                            <asp:ListItem>-- Select One --</asp:ListItem>
                            <asp:ListItem>Environmental Consultant</asp:ListItem>
                            <asp:ListItem>Government Agency</asp:ListItem>
                            <asp:ListItem>Public</asp:ListItem>
                            <asp:ListItem>Work for a facility</asp:ListItem>
                            <asp:ListItem>Work for Environmental Group</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="ddlUserType"
                            ErrorMessage="Select One" Font-Size="Small" ValidationGroup="Profile" InitialValue="-- Select One --"></asp:RequiredFieldValidator>
                    </td>
                </tr>
            </table>

            <p>
                <asp:Button ID="btnUpdateProfile" runat="server" Text="Update Profile" ValidationGroup="Profile" CausesValidation="True" />
            </p>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="0">
        <ProgressTemplate>
            <div id="progressBackgroundFilter">
            </div>
            <div id="progressMessage">
                Please Wait...
                        <br />
                <img alt="Loading" src="<%= Page.ResolveUrl("~/assets/images/progressbar_green.gif") %>" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
