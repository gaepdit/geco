<%@ Page Language="VB" MasterPageFile="~/MainMaster.master" AutoEventWireup="false" Inherits="GECO.FacilityMap" Title="GECO - Facility Map" Codebehind="FacilityMap.aspx.vb" %>

<%@ Register Assembly="Reimers.Google.Map" Namespace="Reimers.Google.Map" TagPrefix="Reimers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <Reimers:Map ID="GMap" Width="700px" Height="500px" runat="server"></Reimers:Map>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="LeftMenuContent" runat="Server">
</asp:Content>
