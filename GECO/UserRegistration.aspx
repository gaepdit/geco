<%@ Page Language="VB" MasterPageFile="~/MainMaster.master" AutoEventWireup="false" Inherits="GECO.UserRegistration" Title="GECO - User Registration" Codebehind="UserRegistration.aspx.vb" %>

<%@ Register Assembly="MSCaptcha" Namespace="MSCaptcha" TagPrefix="captcha" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <h2>GECO User Profile</h2>
            <br />
            <table cellpadding="2">
                <tr>
                    <td align="right">Email Address:</td>
                    <td>
                        <asp:TextBox ID="txtEmail" runat="server" Width="250"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtEmail"
                            Display="Dynamic" ErrorMessage="Email is required." Font-Size="Small">
                        </asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" Font-Size="small"
                            Display="Dynamic" ControlToValidate="txtEmail" ErrorMessage="Email address is invalid."
                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                        </asp:RegularExpressionValidator>
                        <asp:CustomValidator ID="cvEmailExists" runat="server" ControlToValidate="txtEmail"
                            Display="Dynamic" ErrorMessage="The email address is already registered."
                            Font-Size="Small"></asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td align="right">Salutation (optional):</td>
                    <td>
                        <asp:TextBox ID="txtSalutation" runat="server" MaxLength="5"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="right">First Name:</td>
                    <td>
                        <asp:TextBox ID="txtFName" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtFName"
                            ErrorMessage="First name is required." Font-Size="Small"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td align="right">Last Name:</td>
                    <td>
                        <asp:TextBox ID="txtLName" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtLName"
                            ErrorMessage="Last name is required." Font-Size="Small"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td align="right">Title:</td>
                    <td>
                        <asp:TextBox ID="txtTitle" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtTitle"
                            ErrorMessage="Title is required." Font-Size="Small"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td align="right">Company Name:</td>
                    <td>
                        <asp:TextBox ID="txtCoName" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtCoName"
                            ErrorMessage="Company Name is required." Font-Size="Small"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td align="right">Mailing Address:</td>
                    <td>
                        <asp:TextBox ID="txtAddress" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtAddress"
                            ErrorMessage="Street address is required." Font-Size="Small"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td align="right">City:</td>
                    <td>
                        <asp:TextBox ID="txtCity" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="txtCity"
                            ErrorMessage="City is required." Font-Size="Small"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td align="right">State:</td>
                    <td>
                        <asp:TextBox ID="txtState" runat="server" MaxLength="2"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="txtState"
                            ErrorMessage="State is required." Font-Size="Small"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td align="right">Zip Code:</td>
                    <td>
                        <asp:TextBox ID="txtZip" runat="server" MaxLength="5"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="txtzip"
                            ErrorMessage="Zip Code is required." Font-Size="Small"></asp:RequiredFieldValidator>
                        <act:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtZip"
                            FilterType="Numbers">
                        </act:FilteredTextBoxExtender>
                    </td>
                </tr>
                <tr>
                    <td align="right">Telephone Number:</td>
                    <td>
                        <asp:TextBox ID="txtPhone" runat="server" MaxLength="10"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtPhone"
                            ErrorMessage="10-digit Phone Number is required." Font-Size="Small"></asp:RequiredFieldValidator>
                        <act:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtPhone"
                            FilterType="Numbers">
                        </act:FilteredTextBoxExtender>
                    </td>
                </tr>
                <tr>
                    <td align="right">Telephone Extension (optional):</td>
                    <td>
                        <asp:TextBox ID="txtPhoneExt" runat="server" MaxLength="5" Width="64px"></asp:TextBox>
                        <act:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtPhoneExt"
                            FilterType="Numbers">
                        </act:FilteredTextBoxExtender>
                    </td>
                </tr>
                <tr>
                    <td align="right">Fax Number (optional):</td>
                    <td>
                        <asp:TextBox ID="txtFax" runat="server" MaxLength="10"></asp:TextBox>
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
                            ErrorMessage="User Type is required." Font-Size="Small" InitialValue="-- Select One --"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <p>Password must contain at least 8 characters with at least 1 uppercase letter, 1 lowercase letter and 1 number.</p>
                    </td>
                </tr>
                <tr>
                    <td align="right" valign="top">Password:</td>
                    <td>
                        <asp:TextBox ID="txtPwd" runat="server" TextMode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="txtPwd"
                            Display="Dynamic" ErrorMessage="Password is required." Font-Size="Small"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="Regex3" runat="server" ControlToValidate="txtPwd"
                            ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$"
                            ErrorMessage="Password does not meet complexity requirements."
                            ForeColor="Red" />
                    </td>
                </tr>
                <tr>
                    <td align="right" valign="top">Confirm Password:</td>
                    <td>
                        <asp:TextBox ID="txtPwdConfirm" runat="server" TextMode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="txtPwdConfirm"
                            Display="Dynamic" ErrorMessage="Password confirmation is required." Font-Size="Small"></asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txtPwd"
                            ControlToValidate="txtPwdConfirm" ErrorMessage="Passwords do not match"
                            Font-Size="Small" Display="Dynamic"></asp:CompareValidator>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td align="right" valign="top">Enter code as displayed in the image:</td>
                    <td>
                        <asp:TextBox ID="txtCaptcha" runat="server" AutoCompleteType="None"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" ControlToValidate="txtCaptcha"
                            ErrorMessage="Enter code as displayed in the image." Font-Size="Small"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="cvCaptcha" runat="server" ControlToValidate="txtCaptcha"
                            Display="Dynamic" ErrorMessage="Code was incorrect or expired." Font-Size="Small"></asp:CustomValidator>
                        <captcha:CaptchaControl ID="captchaControl" runat="server" CaptchaBackgroundNoise="high"
                            CaptchaLength="5" CaptchaHeight="50" CaptchaWidth="180" CaptchaLineNoise="None"
                            CaptchaMinTimeout="1" CaptchaMaxTimeout="240"
                            ErrorInputTooFast="Whoa!" ErrorInputTooSlow="Image has expired." />
                        <asp:LinkButton ID="lbtnRefreshCaptcha" runat="server" CausesValidation="false" Text="Refresh image"
                            ForeColor="DarkRed"></asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <asp:Button ID="btnRegister" runat="server" Text="Register" />
                    </td>
                </tr>
            </table>

            <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                <ProgressTemplate>
                    <div class="ProgressIndicator">
                        <img src="<%= Page.ResolveUrl("~/assets/images/progressbar_green.gif") %>" alt="Loading" />
                        Please Wait...
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="LeftMenuContent" runat="Server">
    <h2>Georgia Air Protection Branch<br />
        GECO Registration</h2>
    <p><strong>Notes:</strong></p>
    <ul>
        <li>Registration provides access to various facility-specific applications hosted by the Air Protection Branch.</li>
        <li>A valid email address is required for registration.</li>
    </ul>
</asp:Content>
