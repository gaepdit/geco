<%@ Page Language="VB" MasterPageFile="~/MainMaster.master" AutoEventWireup="false" Inherits="GECO.FacilityMap" Title="GECO - Facility Map" CodeBehind="FacilityMap.aspx.vb" %>

<%@ Register Assembly="Reimers.Google.Map" Namespace="Reimers.Google.Map" TagPrefix="Reimers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="FullContent" runat="Server">
    <Reimers:Map ID="GMap" Width="700px" Height="500px" runat="server"></Reimers:Map>
</asp:Content>
