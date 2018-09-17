<%@ Page Title="Reset Data - GECO Emission Inventory" Language="VB" MasterPageFile="eismaster.master" AutoEventWireup="false" Inherits="GECO.EIS_rp_reset" Codebehind="rp_reset.aspx.vb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
    <div class="fieldwrapperseparator">
        <asp:Label ID="lblSeparator" class="styledseparator" runat="server" Text="Reset Emissions Inventory Data"
            Font-Bold="True"></asp:Label>
    </div>
    <div style="text-align: left; width: 600px; margin-left: 200px; font-size: small; color: #000000; font-weight: normal">
        Clicking the button below will reset the facility's Emissions Inventory data for
        <asp:Label ID="lblEIYear" runat="server" Text=""></asp:Label>. This allows one
        to start the Emissions Inventory process from the beginning. The reset does not
        affect the Facility Inventory data. Click the button below to reset the data.
        After the reset is done you will be taken back to the Emissions Inventory home
        page.
    </div>
    <br />
    <br />
    <div class="buttonwrapper">
        <asp:Label ID="lblMessage" runat="server" Font-Bold="True" Font-Names="Arial"
            Font-Size="Small" ForeColor="Red"></asp:Label><br />
        <br />
        <asp:Button ID="btnReset" runat="server" Text="Reset EI Data" />
        <asp:Button ID="btnConfReset" runat="server" Text="Click to Confirm Resetting Data"
            Font-Size="Large" ForeColor="#CC0000" Visible="False" />
        &nbsp;
    <asp:Button ID="btnCancel" runat="server" Text="Cancel" Visible="False"
        Height="33px" />
        <asp:Button ID="btnContinue" runat="server" Text="Continue" Visible="False" />
    </div>
</asp:Content>