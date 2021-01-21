<%@ Page MasterPageFile="~/EIS/EIS.master" Language="VB" AutoEventWireup="false"
    Title="GECO EIS: Submit Process"
    Inherits="GECO.EIS_Process_Submit" CodeBehind="Submit.aspx.vb" %>

<%@ MasterType VirtualPath="~/EIS/EIS.master" %>
<asp:Content ID="Content3" ContentPlaceHolderID="Content" runat="Server">
    <ul class="form-progress">
        <li class="done"><a href="<%= Page.ResolveUrl("~/EIS/Facility/Edit.aspx") %>">Facility Information</a></li>
        <li class="done"><a href="<%= Page.ResolveUrl("~/EIS/Process/") %>">Facility Status</a></li>
        <% If Participating = 3 Then %>
        <li class="done"><a href="<%= Page.ResolveUrl("~/EIS/Users/") %>">CAERS Users</a></li>
        <% Else %>
        <li class="skipped">CAERS Users</li>
        <% End If %>
        <li class="current">Submit</li>
    </ul>

    <h2>Submittal</h2>

    <p class="message-highlight">
        Status:
        <% If Participating = 1 Then %>
        The facility <em>will not</em> participate in the <%=InventoryYear %> Emissions Inventory because it did not operate.
        <% ElseIf Participating = 2 Then %>
        The facility <em>will not</em> participate in the <%=InventoryYear %> Emissions Inventory because all emissions were below the thresholds.
        <% ElseIf Participating = 3 Then %>
        The facility <em>will</em> participate in the <%=InventoryYear %> Emissions Inventory.
        <% End If %>
    </p>

    <p>Click "Submit" to submit your Emissions Inventory status, or use the tabs above to edit your information.</p>
    <p>
        <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="button-large button-proceed" />
    </p>
</asp:Content>
