<%@ Page Language="VB" MasterPageFile="~/MainMaster.master" AutoEventWireup="false" Inherits="GECO.FacilityAdmin" Title="GECO Facility Admin" CodeBehind="Admin.aspx.vb" %>

<%@ MasterType VirtualPath="~/MainMaster.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
    <h1>Facility Admin</h1>

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
            <asp:HyperLink ID="lnkFacilityHome" runat="server" NavigateUrl="~/Facility/">Facility Home</asp:HyperLink>
        </li>
        <li>
            <asp:HyperLink ID="lnkFacilityInfo" runat="server" NavigateUrl="~/Facility/Summary.aspx">Facility Info</asp:HyperLink>
        </li>
        <li>
            <asp:HyperLink ID="lnkFacilityAdmin" runat="server" NavigateUrl="~/Facility/Admin.aspx" Enabled="false" CssClass="selected-menu-item">User Access</asp:HyperLink>
        </li>
    </ul>

    <asp:UpdatePanel ID="FacilityAccessUpdatePanel" runat="server">
        <ContentTemplate>

            <h2>Users with access rights to this facility in GECO:</h2>

            <asp:GridView ID="grdUsers" DataKeyNames="NUMUSERID,STRAIRSNUMBER" CssClass="table-simple"
                AutoGenerateColumns="false" AutoGenerateEditButton="true" AutoGenerateDeleteButton="false"
                runat="Server" CellPadding="5">
                <Columns>
                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:LinkButton ID="DeleteButton" runat="server" CausesValidation="False" CommandName="Delete"
                                OnClientClick="return confirm('Are you sure you want to delete this record?');"
                                Text="Delete" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="NUMUSERID" Visible="False" />
                    <asp:BoundField DataField="STRAIRSNUMBER" Visible="False" />
                    <asp:BoundField DataField="strUserEmail" HeaderText="User ID" ReadOnly="True" />
                    <asp:CheckBoxField HeaderText="Admin Rights" DataField="intAdminAccess">
                        <ItemStyle HorizontalAlign="Center" CssClass="table-cell-checkbox" />
                    </asp:CheckBoxField>
                    <asp:CheckBoxField HeaderText="Permit Fees" DataField="intFeeAccess">
                        <ItemStyle HorizontalAlign="Center" CssClass="table-cell-checkbox" />
                    </asp:CheckBoxField>
                    <asp:CheckBoxField HeaderText="Emissions Inventory" DataField="intEIAccess">
                        <ItemStyle HorizontalAlign="Center" CssClass="table-cell-checkbox" />
                    </asp:CheckBoxField>
                    <asp:CheckBoxField HeaderText="Emissions Statement" DataField="intESAccess">
                        <ItemStyle HorizontalAlign="Center" CssClass="table-cell-checkbox" />
                    </asp:CheckBoxField>
                </Columns>
                <EditRowStyle BackColor="#C0FFC0" BorderColor="Green" BorderStyle="Ridge" />
                <RowStyle BackColor="#ffffff" ForeColor="#333333" />
                <SelectedRowStyle BackColor="#008A8C" BorderColor="Red" Font-Bold="True" ForeColor="White" />
                <HeaderStyle CssClass="table-head" />
                <AlternatingRowStyle BackColor="#f3f3f7" />
            </asp:GridView>
            <br />
            <asp:Panel runat="server" ID="pnlAddNewUser">
                User Email:
                            <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
                <asp:Button ID="btnAddUser" runat="server" BackColor="Control" Text="Add New User" />
                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                    ErrorMessage="Valid email address is required." Font-Size="Small"></asp:RequiredFieldValidator>
                <br />
                <asp:Label ID="lblMessage" runat="server" ForeColor="#C00000" Visible="False">
                                The user you are trying to add does not have a GECO account.
                </asp:Label>
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
