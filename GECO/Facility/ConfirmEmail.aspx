<%@ Page Language="VB" MasterPageFile="~/Main.master" AutoEventWireup="false" Inherits="GECO.ConfirmEmail" Title="GECO Email Confirmation" CodeBehind="ConfirmEmail.aspx.vb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <asp:MultiView ID="MultiView1" runat="server">

        <asp:View ID="ConfirmSuccess" runat="server">
            <h1 class="text-success">Email Confirmed</h1>
            <p>You have successfully confirmed your email address to receive communication for the following facility.</p>
            <table class="table-simple table-list">
                <tbody>
                    <tr>
                        <th>AIRS Number</th>
                        <td>
                            <asp:Label ID="lblAirs" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <th>Facility</th>
                        <td>
                            <asp:Label ID="lblFacility" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <th>Category</th>
                        <td>
                            <asp:Label ID="lblCategory" runat="server" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </asp:View>

        <asp:View ID="ConfirmFailed" runat="server">
            <h1 class="text-error">Email Confirmation Failed</h1>
            <p>Either the account does not exist or the confirmation link has expired.</p>
        </asp:View>

    </asp:MultiView>
</asp:Content>
