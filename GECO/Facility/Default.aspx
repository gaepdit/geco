<%@ Page Language="VB" MasterPageFile="~/MainMaster.master" AutoEventWireup="false" Inherits="GECO.FacilityHome" Title="GECO Facility Home" CodeBehind="Default.aspx.vb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="FullContent" runat="Server">
    <h1>Facility Home</h1>

    <p>
        <b>
            <asp:Label ID="lblFacilityDisplay" runat="server"></asp:Label>
            <br />
            AIRS No:
            <asp:Label ID="lblAIRS" runat="server"></asp:Label>
        </b>
    </p>

    <ul class="menu-list-horizontal">
        <li><a href="Summary.aspx">Facility Info</a></li>
        <li><a href="Admin.aspx">User Access</a></li>
    </ul>

    <h2>Application Menu</h2>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <asp:Table ID="AppTable" runat="server" CssClass="table-simple table-menu table-bordered">
                <asp:TableHeaderRow ID="AppsHeader" runat="server" BackColor="#F0F0F6" CssClass="table-head">
                    <asp:TableHeaderCell Text="GECO Applications" runat="server"></asp:TableHeaderCell>
                    <asp:TableHeaderCell Text="Current Status" runat="server"></asp:TableHeaderCell>
                    <asp:TableHeaderCell Text="Deadline" runat="server"></asp:TableHeaderCell>
                    <asp:TableHeaderCell Text="Current Contact<br><span class='table-cell-subhead'>(click to edit)</span>" runat="server"></asp:TableHeaderCell>
                </asp:TableHeaderRow>

                <asp:TableRow ID="AppsPermitApps" runat="server">
                    <asp:TableHeaderCell runat="server" CssClass="table-cell-link">
                        <asp:HyperLink ID="PALink" runat="server" Text="Air Quality Permits" NavigateUrl="~/Permits/"></asp:HyperLink>
                    </asp:TableHeaderCell>
                    <asp:TableCell runat="server">                        
                        <asp:Label ID="PAText" runat="server"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell runat="server">
                        N/A
                    </asp:TableCell>
                    <asp:TableCell runat="server" CssClass="table-cell-link">
                        <asp:LinkButton ID="PAContact" runat="server" CausesValidation="False"></asp:LinkButton>
                    </asp:TableCell>
                </asp:TableRow>

                <asp:TableRow ID="AppsEmissionFees" runat="server">
                    <asp:TableHeaderCell runat="server" CssClass="table-cell-link">
                        <asp:HyperLink ID="EFLink" runat="server" Text="Emissions Fees" NavigateUrl="~/Fees/Default.aspx"></asp:HyperLink>
                    </asp:TableHeaderCell>
                    <asp:TableCell runat="server">
                        <asp:Literal ID="litEFText" runat="server"></asp:Literal>
                    </asp:TableCell>
                    <asp:TableCell runat="server">
                        <asp:Label ID="lblEFDate" runat="server"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell runat="server" CssClass="table-cell-link">
                        <asp:LinkButton ID="lbtnEFContact" runat="server" CausesValidation="False"></asp:LinkButton>
                    </asp:TableCell>
                </asp:TableRow>

                <asp:TableRow ID="AppsEmissionInventory" runat="server">
                    <asp:TableHeaderCell runat="server" CssClass="table-cell-link">
                        <asp:HyperLink ID="EisLink" runat="server" Text="Emissions Inventory" NavigateUrl="~/EIS/Default.aspx"></asp:HyperLink>
                    </asp:TableHeaderCell>
                    <asp:TableCell runat="server">
                        <asp:Label ID="lblEIText" runat="server"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell runat="server">
                        <asp:Label ID="lblEIDate" runat="server"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell runat="server" CssClass="table-cell-link">
                        <asp:LinkButton ID="lbtnEIContact" runat="server" CausesValidation="False"></asp:LinkButton>
                    </asp:TableCell>
                </asp:TableRow>

                <asp:TableRow ID="AppsEmissionsStatement" runat="server">
                    <asp:TableHeaderCell runat="server" CssClass="table-cell-link">
                        <asp:HyperLink ID="ESLink" runat="server" Text="Emissions Statement" NavigateUrl="~/ES/Default.aspx"></asp:HyperLink>
                    </asp:TableHeaderCell>
                    <asp:TableCell runat="server">
                        <asp:Label ID="lblESText" runat="server"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell runat="server">
                        <asp:Label ID="lblESDate" runat="server"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell runat="server" CssClass="table-cell-link">
                        <asp:LinkButton ID="lbtnESContact" runat="server" CausesValidation="False"></asp:LinkButton>
                    </asp:TableCell>
                </asp:TableRow>

                <asp:TableRow ID="AppsTestNotifications" runat="server">
                    <asp:TableHeaderCell runat="server" CssClass="table-cell-link">
                        <asp:HyperLink ID="TNLink" runat="server" Text="Test Notifications" NavigateUrl="~/TN/Default.aspx"></asp:HyperLink>
                    </asp:TableHeaderCell>
                    <asp:TableCell runat="server">
                        <asp:Label ID="TNText" runat="server"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell runat="server">
                        <asp:Label ID="TNDate" runat="server" Text="N/A"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell runat="server" CssClass="table-cell-link">
                        <asp:LinkButton ID="TNContact" runat="server" CausesValidation="False"></asp:LinkButton>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>

            <asp:Panel ID="pnlContact" runat="server" Visible="False">
                <asp:Label ID="lblContactHeader" runat="server" Font-Bold="True" Font-Size="Large"></asp:Label>
                <asp:HiddenField ID="hidContactKey" runat="server" Visible="False" />
                <asp:RadioButtonList ID="rblContact" runat="server" AutoPostBack="True">
                    <asp:ListItem Value="1" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="2"></asp:ListItem>
                </asp:RadioButtonList>

                <table>
                    <tr>
                        <td width="25%">First Name:
                        </td>
                        <td>
                            <asp:TextBox ID="txtFName" runat="server" CssClass="unwatermarked" ValidationGroup="Contact"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtFName"
                                ErrorMessage="Type First Name" Font-Size="Small" ValidationGroup="Contact"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>Last Name:
                        </td>
                        <td>
                            <asp:TextBox ID="txtLName" runat="server" CssClass="unwatermarked" ValidationGroup="Contact"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtLName"
                                ErrorMessage="Type Last Name" Font-Size="Small" ValidationGroup="Contact"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>Title:
                        </td>
                        <td>
                            <asp:TextBox ID="txtTitle" runat="server" CssClass="unwatermarked" ValidationGroup="Contact"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtTitle"
                                ErrorMessage="Type Title" Font-Size="Small" ValidationGroup="Contact"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>Company Name:
                        </td>
                        <td>
                            <asp:TextBox ID="txtCoName" runat="server" CssClass="unwatermarked" ValidationGroup="Contact"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtCoName"
                                ErrorMessage="Type Company Name" Font-Size="Small" ValidationGroup="Contact"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>Street Address:
                        </td>
                        <td>
                            <asp:TextBox ID="txtAddress" runat="server" CssClass="unwatermarked" ValidationGroup="Contact"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtAddress"
                                ErrorMessage="Type Street Address" Font-Size="Small" ValidationGroup="Contact"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>City:
                        </td>
                        <td>
                            <asp:TextBox ID="txtCity" runat="server" CssClass="unwatermarked" ValidationGroup="Contact"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="txtCity"
                                ErrorMessage="Type City Name" Font-Size="Small" ValidationGroup="Contact"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>State:
                        </td>
                        <td>
                            <asp:TextBox ID="txtState" runat="server" CssClass="unwatermarked" MaxLength="2"
                                ValidationGroup="Contact"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator12"
                                    runat="server" ControlToValidate="txtState" ErrorMessage="State Abbreviation"
                                    Font-Size="Small" ValidationGroup="Contact"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>Zip Code:
                        </td>
                        <td>
                            <asp:TextBox ID="txtZip" runat="server" CssClass="unwatermarked" MaxLength="5" ValidationGroup="Contact" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="txtzip"
                                ErrorMessage="Type 5-digit Zip Code" Font-Size="Small" ValidationGroup="Contact"></asp:RequiredFieldValidator>
                            <act:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtZip"
                                FilterType="Numbers" Enabled="True">
                            </act:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>Telephone Number:
                        </td>
                        <td>
                            <asp:TextBox ID="txtPhone" runat="server" CssClass="unwatermarked" MaxLength="10"
                                ValidationGroup="Contact"></asp:TextBox>
                            &nbsp; Ext:
                                        <asp:TextBox ID="txtPhoneExt" runat="server" CssClass="unwatermarked" MaxLength="5"
                                            Width="64px" ValidationGroup="Contact"></asp:TextBox><asp:RequiredFieldValidator
                                                ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtPhone" ErrorMessage="10-digit Phone Number"
                                                Font-Size="Small" ValidationGroup="Contact"></asp:RequiredFieldValidator>
                            <act:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtPhone"
                                FilterType="Numbers" Enabled="True">
                            </act:FilteredTextBoxExtender>
                            <act:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtPhoneExt"
                                FilterType="Numbers" Enabled="True">
                            </act:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>Fax Number:
                        </td>
                        <td>
                            <asp:TextBox ID="txtFax" runat="server" CssClass="unwatermarked" MaxLength="10" ValidationGroup="Contact"></asp:TextBox>
                            <act:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtFax"
                                FilterType="Numbers" Enabled="True">
                            </act:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>Email Address:
                        </td>
                        <td>
                            <asp:TextBox ID="txtEmailContact" runat="server" CssClass="unwatermarked" ValidationGroup="Contact"></asp:TextBox><asp:RequiredFieldValidator
                                ID="RequiredFieldValidator13" runat="server" ControlToValidate="txtEmailContact"
                                ErrorMessage="Type Email Address" Font-Size="Small" ValidationGroup="Contact"></asp:RequiredFieldValidator><asp:RegularExpressionValidator
                                    ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEmailContact"
                                    ErrorMessage="Type a valid Email address" Font-Size="Small" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                    Display="Dynamic" ValidationGroup="Contact"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                </table>

                <asp:Button ID="btnUpdateContact" runat="server" Text="Save Contact" ValidationGroup="Contact" />
                &nbsp; &nbsp;
                            <asp:Button ID="btnCloseContact" runat="server" Text="Close" ValidationGroup="Contact" />
                <asp:Label ID="lblContactMsg" runat="server" ForeColor="#C000C0" Visible="False"></asp:Label>
            </asp:Panel>

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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
