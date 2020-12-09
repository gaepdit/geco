<%@ Page Language="VB" MasterPageFile="~/MainMaster.master" AutoEventWireup="false"
    Inherits="GECO.Http404Page" Title="File or Page Not Found" CodeBehind="404.aspx.vb" %>

<%@ MasterType VirtualPath="~/MainMaster.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
    <div class="content" id="main-content" style="margin: 0; padding: 100px 25px; width: auto; max-width: 600px;">
        <h1>File or Page Not Found</h1>
        <p>
            The page you have requested could not be found. If you need assistance
            please contact the           
            <asp:HyperLink ID="lnkContact" runat="server">Air Protection Branch</asp:HyperLink>.       
        </p>
        <ul>
            <li><a href="<%= Page.ResolveUrl("~/Home/") %>">Return to the homepage</a></li>
        </ul>
    </div>
</asp:Content>
