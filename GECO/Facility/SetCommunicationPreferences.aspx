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

        <asp:UpdatePanel ID="settings" runat="server">
            <ContentTemplate>

                <p id="pNotSelected" runat="server" visible="false" class="message-warning">
                    Please make a selection.
                </p>

                <asp:RadioButtonList ID="rbCommPref" runat="server">
                    <asp:ListItem Value="Electronic">Prefer to receive electronic communications.</asp:ListItem>
                    <asp:ListItem Value="Mail">Prefer to receive mailed communications.</asp:ListItem>
                    <asp:ListItem Value="Both">Prefer to receive <strong>both</strong> electronic and mailed communications.</asp:ListItem>
                </asp:RadioButtonList>

                <p>
                    Note: You will be required to confirm recipient addresses on the next page.
                </p>

                <p id="pPrefSaveError" runat="server" visible="false" class="message-warning">
                    There was an error while saving. Please try again.
                </p>

                <p>
                    <asp:Button ID="btnSavePref" runat="server" CssClass="button-large" Text="Submit" />
                    <asp:HiddenField ID="hidAirs" runat="server" />
                </p>

            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
