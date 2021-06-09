<%@ Page Language="VB" MasterPageFile="~/Main.master" AutoEventWireup="false"
    Inherits="GECO.SetCommunicationPreferences" Title="GECO Communication Preferences" CodeBehind="SetCommunicationPreferences.aspx.vb" %>

<%@ MasterType VirtualPath="~/Main.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
    <div class="panel panel-centered">
        <h1>Set Communication Preferences</h1>

        <p>
            The Georgia Environmental Protection Division will begin offering electronic 
            communications (e-notices) for information related to fees. Let us know your 
            preference for receiving communications below:
        </p>

        <asp:RadioButtonList ID="rbCommPref" runat="server">
            <asp:ListItem Value="electronic">Prefer to receive electronic communications <strong>only.</strong></asp:ListItem>
            <asp:ListItem Value="mail">Continue to receive mailed communications <strong>only.</strong></asp:ListItem>
            <asp:ListItem Value="both">Prefer to receive <strong>both</strong> electronic and mailed communications.</asp:ListItem>
        </asp:RadioButtonList>

        <p>
            Note: Communication will continue to be sent by mail until an email recipient has been verified.
            Recipients can be added on the next page.
        </p>

        <p>
            <asp:Button ID="btnSavePref" runat="server" CssClass="button-large" Text="Submit" />
        </p>
    </div>
</asp:Content>
