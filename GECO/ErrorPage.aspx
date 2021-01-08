<%@ Page Language="VB" MasterPageFile="~/Main.master" AutoEventWireup="false"
    Inherits="GECO.ErrorPage" Title="GECO Error Page" CodeBehind="ErrorPage.aspx.vb" %>

<%@ MasterType VirtualPath="~/Main.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
    <div class="content" id="main-content" style="margin: 0; padding: 100px 25px; width: auto; max-width: 600px;">
        <h1>An Error Has Occurred</h1>
        <p>
            An unexpected error has occurred on our website. If the problem persists, please contact the Air Protection Branch 
            at 404-363-7000 or email <a href="mailto:epd_it@dnr.ga.gov">epd_it@dnr.ga.gov</a>.
        </p>
        <ul>
            <li><a href="<%= Page.ResolveUrl("~") %>">Return to the homepage</a></li>
        </ul>
    </div>
</asp:Content>
