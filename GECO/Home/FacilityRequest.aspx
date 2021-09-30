<%@ Page Title="GECO - Request Facility Access" Language="VB" MasterPageFile="~/Main.master" AutoEventWireup="false"
    Inherits="GECO.Home_FacilityRequest" CodeBehind="FacilityRequest.aspx.vb" %>

<%@ MasterType VirtualPath="~/Main.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
    <h2>GECO Facility Access Request</h2>

    <p id="pUpdateRequired" runat="server" visible="false" class="message-highlight">
        Your profile is missing required information.
        <asp:HyperLink ID="lnkUpdateProfile" runat="server" NavigateUrl="~/Account/" CssClass="no-visited">Please update before continuing</asp:HyperLink>.
    </p>

    <asp:Panel ID="pnlRequestAccess" runat="server">
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
                            <act:AutoCompleteExtender ID="aceAIRS" runat="server" CompletionSetCount="15" MinimumPrefixLength="3"
                                EnableCaching="true" CompletionInterval="10" TargetControlID="txtAirsNo"
                                ServiceMethod="AutoCompleteAirs" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtFacility" runat="server" AutoPostBack="true" />
                            <act:AutoCompleteExtender ID="aceFacility" runat="server" CompletionSetCount="15" MinimumPrefixLength="3"
                                EnableCaching="true" CompletionInterval="10" TargetControlID="txtFacility"
                                ServiceMethod="AutoCompleteFacility" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <b>
                                <asp:Label ID="lblAccess" runat="server" AssociatedControlID="lstbAccess">Application Access Requested</asp:Label></b>
                            <br />
                            <asp:CheckBoxList ID="lstbAccess" runat="server" AutoPostBack="true">
                                <asp:ListItem Text="Facility Access" Selected="True" Enabled="false" />
                                <asp:ListItem Text="Permit Fees" />
                                <asp:ListItem Text="Emissions Inventory" />
                                <asp:ListItem Text="Emissions Statement" />
                            </asp:CheckBoxList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <b>
                                <asp:Label ID="lblComments" runat="server" AssociatedControlID="txtComments">Additional Comments</asp:Label></b>
                            <br />
                            <asp:TextBox ID="txtComments" runat="server" TextMode="MultiLine" />
                        </td>
                    </tr>
                </table>

                <blockquote id="bqMessage" runat="server" visible="false">
                    <b>
                        <asp:Label ID="lblMessageLabel" runat="server" />
                    </b>
                    <hr />
                    <asp:Literal ID="ltlMessage" runat="server" />
                </blockquote>

                <p>
                    <asp:Button ID="btnSend" runat="server" Text="Send Request" Enabled="false" CssClass="button-large" />
                    <asp:Label ID="lblSuccess" runat="server" Font-Bold="True" CssClass="text-success"
                        Text="Your message has been sent." Visible="False"></asp:Label>
                    <asp:Label ID="lblError" runat="server" Font-Bold="True" CssClass="text-error"
                        Text="Error." Visible="False"></asp:Label>
                </p>

                <asp:UpdateProgress ID="ModalUpdateProgress1" runat="server" DisplayAfter="200" class="progressIndicator">
                    <ProgressTemplate>
                        <div class="progressIndicator-inner">
                            Please Wait...<br />
                            <img src="<%= Page.ResolveUrl("~/assets/images/progressbar_green.gif") %>" alt="" />
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Content>
