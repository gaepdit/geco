<%@ Page Title="GECO - Request Facility Access" Language="VB" MasterPageFile="~/MainMaster.master" AutoEventWireup="false"
    Inherits="GECO.Home_FacilityRequest" CodeBehind="FacilityRequest.aspx.vb" %>

<%@ MasterType VirtualPath="~/MainMaster.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
    <h2>GECO Facility Access Request</h2>

    <p id="pUpdateRequired" runat="server" visible="false" class="message-highlight">
        Your profile is missing required information.
        <asp:HyperLink ID="lnkUpdateProfile" runat="server" NavigateUrl="~/Account/" CssClass="no-visited">Please update before continuing</asp:HyperLink>.
    </p>

    <asp:UpdatePanel ID="upRequestAccess" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <p>
                To request access for a facility, <b>find the facility</b> by AIRS number or name, 
                then click the <b>Send Request</b> button at the bottom of the page. Your request will be 
                forwarded to either the Administrator account of the facility or to the 
                Georgia Air Protection Branch if there is no current GECO Facility Administrator.
            </p>

            <table class="table-simple">
                <tr>
                    <th scope="col">
                        <asp:Label ID="lblAirsNo" AssociatedControlID="txtAirsNo" runat="server">By AIRS Number</asp:Label>
                    </th>
                    <th scope="col">
                        <asp:Label ID="lblFacility" AssociatedControlID="txtFacility" runat="server">By Facility Name</asp:Label>
                    </th>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox ID="txtAirsNo" runat="server" AutoPostBack="true" CssClass="input-small" />
                        <act:AutoCompleteExtender ID="aceAIRS" runat="server" CompletionSetCount="15" MinimumPrefixLength="2"
                            EnableCaching="true" CompletionInterval="10" TargetControlID="txtAirsNo" 
                            ServiceMethod="AutoCompleteAirs" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtFacility" runat="server" AutoPostBack="true" />
                        <act:AutoCompleteExtender ID="aceFacility" runat="server" CompletionSetCount="15"
                            MinimumPrefixLength="2" EnableCaching="true" CompletionInterval="10" TargetControlID="txtFacility"
                            ServiceMethod="AutoCompleteFacility" />
                    </td>
                </tr>
            </table>

            <p>
                <b><asp:Label ID="lblName" runat="server" AssociatedControlID="txtName">Name</asp:Label></b>
                <br />
                <asp:TextBox ID="txtName" runat="server" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server"
                    ControlToValidate="txtName" ErrorMessage="Name is required." />
            </p>
            <p>
                <b><asp:Label ID="lblEmail" runat="server" AssociatedControlID="txtEmail">Email</asp:Label></b>
                <br />
                <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator
                    ID="RequiredFieldValidator20" runat="server" ControlToValidate="txtEmail" ErrorMessage="Email is required" />
                <asp:RegularExpressionValidator ID="RegularExpressionValidator2"
                    ControlToValidate="txtEmail" runat="server" ErrorMessage="Valid Email is required"
                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
            </p>
            <p>
                <b><asp:Label ID="lblAccess" runat="server" AssociatedControlID="lstbAccess">Application Access</asp:Label></b>
                <br />
                <asp:CheckBoxList ID="lstbAccess" runat="server">
                    <asp:ListItem Text="Facility Access" Value="Facility" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="Permit Fees" Value="Fees"></asp:ListItem>
                    <asp:ListItem Text="Emission Inventory" Value="Inventory"></asp:ListItem>
                    <asp:ListItem Text="Emission Statement" Value="Statement"></asp:ListItem>
                </asp:CheckBoxList>
            </p>
            <p>
                <b><asp:Label ID="lblComments" runat="server" AssociatedControlID="txtComments">Additional Comments</asp:Label></b>
                <br />
                <asp:TextBox ID="txtComments" runat="server" TextMode="MultiLine" />
            </p>

            <blockquote id="bqMessage" runat="server" visible="false">
                <b>Message:</b>
                <asp:Literal ID="ltlMessage" runat="server" />
            </blockquote>

            <p>
                <asp:Button ID="btnSend" runat="server" Text="Send Request" OnClick="btnSend_Click" CssClass="button-large" />
                <asp:Label ID="lblSuccess" runat="server" Font-Bold="True" ForeColor="#007700" Text="Your message has been sent."
                    Visible="False"></asp:Label>
                <asp:Label ID="lblError" runat="server" Font-Bold="True" ForeColor="#770000" Text="Error."
                    Visible="False"></asp:Label>
            </p>

            <asp:UpdateProgress ID="UpdateProgress3" runat="server" DisplayAfter="50" AssociatedUpdatePanelID="upRequestAccess">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter"></div>
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
