<%@ Page Language="VB" MasterPageFile="~/Main.master" AutoEventWireup="false" Inherits="GECO.FacilityAdmin"
    Title="GECO User Access Admin" CodeBehind="Admin.aspx.vb" %>

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
            <asp:HyperLink ID="lnkFacilityAdmin" runat="server" NavigateUrl="~/Facility/Admin.aspx"
                Enabled="false" CssClass="selected-menu-item disabled">User Access</asp:HyperLink>
        </li>
        <li>
            <asp:HyperLink ID="lnkFacilityContacts" runat="server" NavigateUrl="~/Facility/Contacts.aspx">Communication Preferences</asp:HyperLink>
        </li>
    </ul>

    <h1>GECO User Access Admin</h1>

    <asp:UpdatePanel ID="FacilityAccessUpdatePanel" runat="server">
        <ContentTemplate>

            <p>
                The following GECO users have access rights to this facility.
                <% If FacilityAccess.AdminAccess Then %>
                To change which section a user can access, use the "Edit" button next to their name. 
                To remove all access for a user, use the "Delete" button.
                <% End If %>
            </p>

            <p>
                <em>Note: This page is only for showing access rights in GECO.</em>
                To edit facility contact information, use the
                <asp:HyperLink runat="server" NavigateUrl="~/Facility/Contacts.aspx">Communication Preferences</asp:HyperLink>
                tab.
                <% If FacilityAccess.AdminAccess Then %>
                To edit CAERS Users, go to the
                <asp:HyperLink runat="server" NavigateUrl="~/EIS/Default.aspx">Emissions Inventory</asp:HyperLink>
                page.
                <% End If %>
            </p>

            <% If ReviewRequested Then %>
            <p class="message-highlight">
                Please review the user access table below for accuracy and make changes as needed.
            </p>
            <% End If %>

            <asp:GridView ID="grdUsers" runat="Server" DataKeyNames="NUMUSERID" CssClass="table-simple table-checkbox-menu"
                AutoGenerateColumns="false">
                <Columns>
                    <asp:CommandField ShowCancelButton="true" ShowEditButton="true" ShowDeleteButton="true" ButtonType="Button" />
                    <asp:BoundField DataField="NUMUSERID" Visible="False" />
                    <asp:TemplateField HeaderText="User">
                        <ItemTemplate>
                            <%#Eval("FirstName") %> <%#Eval("LastName") %><br />
                            <%#Eval("Email") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:CheckBoxField HeaderText="Admin Rights" DataField="intAdminAccess" Text="&nbsp;">
                        <ItemStyle HorizontalAlign="Center" CssClass="table-cell-checkbox" />
                    </asp:CheckBoxField>
                    <asp:CheckBoxField HeaderText="Permit Fees" DataField="intFeeAccess" Text="&nbsp;">
                        <ItemStyle HorizontalAlign="Center" CssClass="table-cell-checkbox" />
                    </asp:CheckBoxField>
                    <asp:CheckBoxField HeaderText="Emissions Inventory" DataField="intEIAccess" Text="&nbsp;">
                        <ItemStyle HorizontalAlign="Center" CssClass="table-cell-checkbox" />
                    </asp:CheckBoxField>
                    <asp:CheckBoxField HeaderText="Emissions Statement" DataField="intESAccess" Text="&nbsp;">
                        <ItemStyle HorizontalAlign="Center" CssClass="table-cell-checkbox" />
                    </asp:CheckBoxField>
                </Columns>
                <EditRowStyle BackColor="#ffffc0" BorderColor="#eeeec0" BorderStyle="Solid" />
                <RowStyle BackColor="#ffffff" ForeColor="#333333" />
                <SelectedRowStyle BackColor="#008A8C" BorderColor="Red" Font-Bold="True" ForeColor="White" />
                <HeaderStyle CssClass="table-head" />
                <AlternatingRowStyle BackColor="#f3f3f7" />
            </asp:GridView>
            <br />

            <asp:Panel runat="server" ID="pnlAddNewUser" Visible="false">
                <h3>Add New User</h3>
                <p>
                    To grant another user access to this facility, enter their email below. 
                    Note: They must already have a GECO account before they can be added here.

                </p>
                <p>
                    User Email:
                    <asp:TextBox ID="txtEmail" runat="server" ValidationGroup="NewUser" />
                    <asp:Button ID="btnAddUser" runat="server" Text="Add New User" ValidationGroup="NewUser" />
                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                        ErrorMessage="Valid email address is required." ValidationGroup="NewUser" />
                </p>
                <asp:Label ID="lblMessage" runat="server" ForeColor="#C00000" Visible="False" />
            </asp:Panel>

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
</asp:Content>
