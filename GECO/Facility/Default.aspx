<%@ Page Language="VB" MasterPageFile="~/MainMaster.master" AutoEventWireup="false" Inherits="GECO.FacilityHome" Title="GECO Facility Home" CodeBehind="Default.aspx.vb" %>

<%@ MasterType VirtualPath="~/MainMaster.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
    <h1>Facility Home</h1>

    <p>
        Current Facility: 
        <b>
            <asp:Label ID="lblFacilityDisplay" runat="server"></asp:Label>
        </b>
        <br />
        AIRS No:        
        <b>
            <asp:Label ID="lblAIRS" runat="server"></asp:Label>
        </b>
    </p>

    <ul class="menu-list-horizontal">
        <li>
            <asp:HyperLink ID="lnkFacilityHome" runat="server" NavigateUrl="~/Facility/" Enabled="false" CssClass="selected-menu-item">Facility Home</asp:HyperLink>
        </li>
        <li>
            <asp:HyperLink ID="lnkFacilityInfo" runat="server" NavigateUrl="~/Facility/Summary.aspx">Facility Info</asp:HyperLink>
        </li>
        <li>
            <asp:HyperLink ID="lnkFacilityAdmin" runat="server" NavigateUrl="~/Facility/Admin.aspx">User Access</asp:HyperLink>
        </li>
    </ul>

    <h2>Application Menu</h2>

    <asp:UpdatePanel ID="FacilityContactsUpdatePanel" runat="server">
        <ContentTemplate>

            <asp:Table ID="AppTable" runat="server" CssClass="table-simple table-menu table-bordered">
                <asp:TableHeaderRow ID="AppsHeader" runat="server" BackColor="#F0F0F6" CssClass="table-head">
                    <asp:TableHeaderCell Text="GECO Applications" runat="server"></asp:TableHeaderCell>
                    <asp:TableHeaderCell Text="Current Status" runat="server"></asp:TableHeaderCell>
                    <asp:TableHeaderCell Text="Deadline" runat="server"></asp:TableHeaderCell>
                    <asp:TableHeaderCell Text="Current Contact<br><span class='table-cell-subhead'>(click to edit)</span>" runat="server"></asp:TableHeaderCell>
                </asp:TableHeaderRow>

                <asp:TableRow ID="AppsPermits" runat="server">
                    <asp:TableHeaderCell runat="server" CssClass="table-cell-link">
                        <asp:HyperLink ID="PALink" runat="server" NavigateUrl="~/Permits/">Permits & Application Fees</asp:HyperLink>
                    </asp:TableHeaderCell>
                    <asp:TableCell runat="server">
                        <asp:Label ID="PAText" runat="server"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell runat="server"></asp:TableCell>
                    <asp:TableCell runat="server" CssClass="table-cell-link">
                        <asp:LinkButton ID="PAContact" runat="server" CausesValidation="False"></asp:LinkButton>
                    </asp:TableCell>
                </asp:TableRow>

                <asp:TableRow ID="AppsEmissionFees" runat="server">
                    <asp:TableHeaderCell runat="server" CssClass="table-cell-link">
                        <asp:HyperLink ID="EFLink" runat="server" NavigateUrl="~/AnnualFees/">Annual/Emissions Fees</asp:HyperLink>
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

                <asp:TableRow ID="AppsFeesSummary" runat="server">
                    <asp:TableHeaderCell runat="server" CssClass="table-cell-link">
                        <asp:HyperLink ID="PFLink" runat="server" NavigateUrl="~/Fees/">Fees Summary</asp:HyperLink>
                    </asp:TableHeaderCell>
                    <asp:TableCell runat="server"></asp:TableCell>
                    <asp:TableCell runat="server"></asp:TableCell>
                    <asp:TableCell runat="server" CssClass="table-cell-link">
                        <asp:LinkButton ID="lbtnPFContact" runat="server" CausesValidation="False"></asp:LinkButton>
                    </asp:TableCell>
                </asp:TableRow>

                <asp:TableRow ID="AppsEmissionInventory" runat="server">
                    <asp:TableHeaderCell runat="server" CssClass="table-cell-link">
                        <asp:HyperLink ID="EisLink" runat="server" Text="Emissions Inventory" NavigateUrl="~/EIS/"></asp:HyperLink>
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
                        <asp:HyperLink ID="ESLink" runat="server" Text="Emissions Statement" NavigateUrl="~/ES/"></asp:HyperLink>
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
                        <asp:HyperLink ID="TNLink" runat="server" Text="Test Notifications" NavigateUrl="~/TN/"></asp:HyperLink>
                    </asp:TableHeaderCell>
                    <asp:TableCell runat="server">
                        <asp:Label ID="TNText" runat="server"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell runat="server"></asp:TableCell>
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

                <table class="table-simple table-list">
                    <tr>
                        <th>First Name</th>
                        <td>
                            <asp:TextBox ID="txtFName" runat="server" ValidationGroup="Contact"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtFName"
                                ErrorMessage="Type First Name" Font-Size="Small" ValidationGroup="Contact"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <th>Last Name</th>
                        <td>
                            <asp:TextBox ID="txtLName" runat="server" ValidationGroup="Contact"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtLName"
                                ErrorMessage="Type Last Name" Font-Size="Small" ValidationGroup="Contact"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <th>Title</th>
                        <td>
                            <asp:TextBox ID="txtTitle" runat="server" ValidationGroup="Contact"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtTitle"
                                ErrorMessage="Type Title" Font-Size="Small" ValidationGroup="Contact"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <th>Company Name</th>
                        <td>
                            <asp:TextBox ID="txtCoName" runat="server" ValidationGroup="Contact"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtCoName"
                                ErrorMessage="Type Company Name" Font-Size="Small" ValidationGroup="Contact"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <th>Street Address</th>
                        <td>
                            <asp:TextBox ID="txtAddress" runat="server" ValidationGroup="Contact"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtAddress"
                                ErrorMessage="Type Street Address" Font-Size="Small" ValidationGroup="Contact"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <th>City</th>
                        <td>
                            <asp:TextBox ID="txtCity" runat="server" ValidationGroup="Contact"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="txtCity"
                                ErrorMessage="Type City Name" Font-Size="Small" ValidationGroup="Contact"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <th>State</th>
                        <td>
                            <asp:TextBox ID="txtState" runat="server" MaxLength="2"
                                ValidationGroup="Contact"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator12"
                                    runat="server" ControlToValidate="txtState" ErrorMessage="State Abbreviation"
                                    Font-Size="Small" ValidationGroup="Contact"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <th>Zip Code</th>
                        <td>
                            <asp:TextBox ID="txtZip" runat="server" MaxLength="5" ValidationGroup="Contact" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="txtzip"
                                ErrorMessage="Type 5-digit Zip Code" Font-Size="Small" ValidationGroup="Contact"></asp:RequiredFieldValidator>
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
                                Font-Size="Small" ValidationGroup="Contact" />
                        </td>
                    </tr>
                    <tr>
                        <th>Fax Number</th>
                        <td>
                            <asp:TextBox ID="txtFax" runat="server" MaxLength="10" ValidationGroup="Contact"></asp:TextBox> (numbers only)
                            <act:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtFax"
                                FilterType="Numbers" Enabled="True">
                            </act:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <th>Email Address</th>
                        <td>
                            <asp:TextBox ID="txtEmailContact" runat="server" ValidationGroup="Contact"></asp:TextBox><asp:RequiredFieldValidator
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
