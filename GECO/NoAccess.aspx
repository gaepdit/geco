<%@ Page Language="VB" MasterPageFile="~/Main.master" AutoEventWireup="false" Inherits="GECO.NoAccess" Title="Access Denied" Codebehind="NoAccess.aspx.vb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
    You do not have sufficient rights to work on the page that you were trying to access.
    You may be getting this message either if you have not logged in, or if your session
    has expired or if you just don't have sufficient rights.<br />
    <br />
    You may Sign In and try to access the page again. If you believe that this is a
    mistake, please contact either the Administrator for that particular facility or
    contact us with a detailed message.<br />
    <br />
    You may continue to navigate through the applications by clicking on any of the
    quick links on the right hand side.
</asp:Content>