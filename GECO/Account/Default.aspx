<%@ Page Title="GECO Account" Language="VB" MasterPageFile="~/MainMaster.master" AutoEventWireup="false" Inherits="GECO.Account_Default" CodeBehind="Default.aspx.vb" %>

<%@ MasterType VirtualPath="~/MainMaster.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
    <h1>Account</h1>

    <p>
        Logged in as
        <asp:Label runat="server" ID="lblDisplayName"></asp:Label>.
    </p>

    <p>
        <asp:Button ID="hlSignOut" runat="server" Text="Sign Out" PostBackUrl="~/Default.aspx?do=SignOut" CssClass="button-large"></asp:Button>
    </p>

    <ul class="menu-list-horizontal">
        <li>
            <asp:HyperLink ID="lnkEditProfile" runat="server" NavigateUrl="~/Account/" Enabled="false" CssClass="selected-menu-item disabled">Profile</asp:HyperLink>
        </li>
        <li>
            <asp:HyperLink ID="lnkEditEmail" runat="server" NavigateUrl="~/Account/Email.aspx">Email</asp:HyperLink>
        </li>
        <li>
            <asp:HyperLink ID="lnkEditPassword" runat="server" NavigateUrl="~/Account/Password.aspx">Password</asp:HyperLink>
        </li>
    </ul>

    <h2>Profile</h2>

    <asp:UpdatePanel ID="UpdatePanel_profile" runat="server">
        <ContentTemplate>
            <p id="pUpdateRequired" runat="server" visible="false" class="message-highlight">
                Your profile is missing required information. Please update before continuing.
            </p>

            <p>
                Please ensure your profile information is accurate as we use this to improve our communication with you. 
                Also, this information is necessary when signing up for events hosted by EPD.
            </p>

            <asp:Panel ID="subForm" runat="server" DefaultButton="btnUpdateProfile">
                <p>
                    <asp:Label ID="lblProfileMessage" runat="server" CssClass="message-update"></asp:Label>
                </p>

                <table class="table-simple table-list">
                    <tr>
                        <th>First Name</th>
                        <td>
                            <asp:TextBox ID="txtFName" runat="server" MaxLength="50"></asp:TextBox>
                            <i>required</i>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                                ControlToValidate="txtFName" ErrorMessage="First Name is required."
                                Font-Size="Small" ValidationGroup="Profile"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <th>Last Name</th>
                        <td>
                            <asp:TextBox ID="txtLName" runat="server" MaxLength="50"></asp:TextBox>
                            <i>required</i>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                ControlToValidate="txtLName" ErrorMessage="Last Name is required."
                                Font-Size="Small" ValidationGroup="Profile"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <th>Title</th>
                        <td>
                            <asp:TextBox ID="txtTitle" runat="server" MaxLength="100"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>Company</th>
                        <td>
                            <asp:TextBox ID="txtCoName" runat="server" MaxLength="200"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>Street Address</th>
                        <td>
                            <asp:TextBox ID="txtAddress" runat="server" MaxLength="250"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>City</th>
                        <td>
                            <asp:TextBox ID="txtCity" runat="server" MaxLength="50"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>State</th>
                        <td>
                            <asp:TextBox ID="txtState" runat="server" MaxLength="30"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>Zip Code</th>
                        <td>
                            <asp:TextBox ID="txtZip" runat="server" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>Telephone Number</th>
                        <td>
                            <asp:TextBox ID="txtPhone" runat="server" MaxLength="30"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>User Type</th>
                        <td>
                            <asp:DropDownList ID="ddlUserType" runat="server">
                                <asp:ListItem>-- Select --</asp:ListItem>
                                <asp:ListItem>Environmental Consultant</asp:ListItem>
                                <asp:ListItem>Government Agency</asp:ListItem>
                                <asp:ListItem>Public</asp:ListItem>
                                <asp:ListItem>Work for a facility</asp:ListItem>
                                <asp:ListItem>Work for Environmental Group</asp:ListItem>
                            </asp:DropDownList>
                            <i>required</i>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="ddlUserType"
                                ErrorMessage="Select One" ValidationGroup="Profile" InitialValue="-- Select --"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                </table>

                <p>
                    <asp:Button ID="btnUpdateProfile" runat="server" Text="Update Profile" ValidationGroup="Profile" CausesValidation="True" CssClass="button-large" />
                </p>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdateProgress ID="ProfileUpdateProgress" runat="server" DisplayAfter="0">
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
