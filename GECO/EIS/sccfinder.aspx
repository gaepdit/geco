<%@ Page Language="VB" ValidateRequest="false" EnableEventValidation="false" AutoEventWireup="false" Inherits="GECO.EIS_sccfinder" Codebehind="sccfinder.aspx.vb" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html lang="en-us" xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>GA Air - SCC Finder</title>
    <link rel="apple-touch-icon" sizes="180x180" href="~/assets/epd-favicons/apple-touch-icon.png?v=69kRrvbXdL" />
    <link rel="icon" type="image/png" href="~/assets/epd-favicons/favicon-32x32.png?v=69kRrvbXdL" sizes="32x32" />
    <link rel="icon" type="image/png" href="~/assets/epd-favicons/favicon-16x16.png?v=69kRrvbXdL" sizes="16x16" />
    <link rel="manifest" href="~/assets/epd-favicons/manifest.json?v=69kRrvbXdL" />
    <link rel="mask-icon" href="~/assets/epd-favicons/safari-pinned-tab.svg?v=69kRrvbXdL" color="#5bbad5" />
    <link rel="shortcut icon" href="~/favicon.ico?v=69kRrvbXdL" />
    <meta name="theme-color" content="#e5f6fa" />
    <style>
        body {
            background: #ffffff;
            margin: 10px;
            padding: 10px;
            font-family: 'Verdana', 'Arial', 'Helvetica';
            font-size: 75%;
            _font-size: 100%; /* using underscore hack for MSIE here */
        }
    </style>
    <script type="text/javascript">
        function PassSCCCode() {
            //window.returnValue = document.getElementById('<%=txtSCC.ClientID%>').value;
            window.opener.document.getElementById("ctl00_ContentPlaceHolder3_txtSourceClassCode").value = document.getElementById('<%=txtSCC.ClientID%>').value;
            window.close();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <act:ToolkitScriptManager ID="ToolkitScriptManager1" EnablePartialRendering="true" runat="server"></act:ToolkitScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div>
                    <p>
                        To find SCC for a specific type of Emission Source, select appropriate description
                    of Emission Source from each level of drop down boxes. The SCC will be displayed
                    below once you have completed all levels.
                    </p>
                    <p>
                        Select most appropriate Source Type
                    </p>
                    <asp:Label ID="lblLevel1" runat="server" AssociatedControlID="rcbLevel1">Level 1</asp:Label>&nbsp;
                <asp:DropDownList ID="rcbLevel1" runat="server" />
                    <br />
                    <asp:Label ID="lblLevel2" runat="server" AssociatedControlID="rcbLevel2">Level 2</asp:Label>&nbsp;
                <asp:DropDownList ID="rcbLevel2" runat="server" />
                    <br />
                    <asp:Label ID="lblLevel3" runat="server" AssociatedControlID="rcbLevel3">Level 3</asp:Label>&nbsp;
                <asp:DropDownList ID="rcbLevel3" runat="server" />
                    <br />
                    <asp:Label ID="lblLevel4" runat="server" AssociatedControlID="rcbLevel4">Level 4</asp:Label>&nbsp;
                <asp:DropDownList ID="rcbLevel4" runat="server" AutoPostBack="true" />
                    <br />
                    <act:CascadingDropDown ID="cddLevel1" runat="server" ServiceMethod="GetLevel1" Category="Level1"
                        LoadingText="[ Loading... ]" PromptText="- Select a Source Type -" TargetControlID="rcbLevel1" ServicePath="SourceClassCode.asmx">
                    </act:CascadingDropDown>
                    <act:CascadingDropDown ID="cddLevel2" runat="server" ServiceMethod="GetLevel2" Category="Level2"
                        LoadingText="[ Loading... ]" PromptText=" " TargetControlID="rcbLevel2" ParentControlID="rcbLevel1" ServicePath="SourceClassCode.asmx">
                    </act:CascadingDropDown>
                    <act:CascadingDropDown ID="cddLevel3" runat="server" ServiceMethod="GetLevel3" Category="Level3"
                        LoadingText="[ Loading... ]" PromptText=" " TargetControlID="rcbLevel3" ParentControlID="rcbLevel2" ServicePath="SourceClassCode.asmx">
                    </act:CascadingDropDown>
                    <act:CascadingDropDown ID="cddLevel4" runat="server" ServiceMethod="GetLevel4" Category="Level4"
                        LoadingText="[ Loading... ]" PromptText=" " TargetControlID="rcbLevel4" ParentControlID="rcbLevel3" ServicePath="SourceClassCode.asmx">
                    </act:CascadingDropDown>
                </div>
                <p>
                </p>
                <asp:TextBox ID="txtSCC" runat="server" Enabled="false" Width="100" Visible="false"></asp:TextBox>
                <asp:Button ID="btnUseSCC" runat="server" Text="Use this SCC code" Visible="false" OnClientClick="PassSCCCode();" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>