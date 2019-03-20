<%@ Page Language="VB" ValidateRequest="false" EnableEventValidation="false" AutoEventWireup="false" Inherits="GECO.EIS_sccfinder" CodeBehind="sccfinder.aspx.vb" %>

<!DOCTYPE html>
<html lang="en-us" xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <title>GECO - SCC Finder</title>
    <link rel="apple-touch-icon" sizes="180x180" href="~/assets/epd-favicons/apple-touch-icon.png?v=69kRrvbXdL" />
    <link rel="icon" type="image/png" href="~/assets/epd-favicons/favicon-32x32.png?v=69kRrvbXdL" sizes="32x32" />
    <link rel="icon" type="image/png" href="~/assets/epd-favicons/favicon-16x16.png?v=69kRrvbXdL" sizes="16x16" />
    <link rel="mask-icon" href="~/assets/epd-favicons/safari-pinned-tab.svg?v=69kRrvbXdL" color="#5bbad5" />
    <link rel="shortcut icon" href="~/favicon.ico?v=69kRrvbXdL" />
    <meta name="theme-color" content="#e5f6fa" />
    <link href="~/assets/css/site.css?v=20190312" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function PassSCCCode() {
            window.opener.document.getElementById("ctl00_ContentPlaceHolder3_txtSourceClassCode").value = document.getElementById('<%=lblSCC.ClientID%>').innerText;
            window.close();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="content">
            <act:ToolkitScriptManager ID="ToolkitScriptManager1" EnablePartialRendering="true" runat="server" />
            <p>
                To find SCC for a specific type of Emission Source, select appropriate description
                of Emission Source from each level of drop down boxes. The SCC will be displayed
                below once you have completed all levels. Select the most appropriate Source Type.
            </p>
            <p>
                <asp:Label ID="lblLevel1" runat="server" AssociatedControlID="rcbLevel1">Level 1</asp:Label><br />
                <asp:DropDownList ID="rcbLevel1" runat="server" />
            </p>
            <p>
                <asp:Label ID="lblLevel2" runat="server" AssociatedControlID="rcbLevel2">Level 2</asp:Label><br />
                <asp:DropDownList ID="rcbLevel2" runat="server" />
            </p>
            <p>
                <asp:Label ID="lblLevel3" runat="server" AssociatedControlID="rcbLevel3">Level 3</asp:Label><br />
                <asp:DropDownList ID="rcbLevel3" runat="server" />
            </p>
            <p>
                <asp:Label ID="lblLevel4" runat="server" AssociatedControlID="rcbLevel4">Level 4</asp:Label><br />
                <asp:DropDownList ID="rcbLevel4" runat="server" />
            </p>

            <act:CascadingDropDown ID="cddLevel1" runat="server" ServiceMethod="GetLevel1" Category="Level1"
                LoadingText="[ Loading... ]" PromptText="- Select -" TargetControlID="rcbLevel1"
                ServicePath="SourceClassCode.asmx" />
            <act:CascadingDropDown ID="cddLevel2" runat="server" ServiceMethod="GetLevel2" Category="Level2"
                LoadingText="[ Loading... ]" PromptText="- Select -" TargetControlID="rcbLevel2" ParentControlID="rcbLevel1"
                ServicePath="SourceClassCode.asmx" />
            <act:CascadingDropDown ID="cddLevel3" runat="server" ServiceMethod="GetLevel3" Category="Level3"
                LoadingText="[ Loading... ]" PromptText="- Select -" TargetControlID="rcbLevel3" ParentControlID="rcbLevel2"
                ServicePath="SourceClassCode.asmx" />
            <act:CascadingDropDown ID="cddLevel4" runat="server" ServiceMethod="GetLevel4" Category="Level4"
                LoadingText="[ Loading... ]" PromptText="- Select -" TargetControlID="rcbLevel4" ParentControlID="rcbLevel3"
                ServicePath="SourceClassCode.asmx" />

            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <p>
                        <asp:Button ID="btnLookUp" runat="server" Text="Look up SCC" />
                    </p>

                    <div id="dSccDetails" runat="server" visible="false">
                        <h2 id="">
                            <asp:Label ID="lblSCC" runat="server" Text="" />
                            <asp:Button ID="btnUseSCC" runat="server" Text="Use this SCC code" OnClientClick="PassSCCCode();" />
                        </h2>

                        <table id="tblScc" runat="server" class="table-simple table-compact">
                            <tr>
                                <th>Category</th>
                                <td>
                                    <asp:Label ID="lCategory" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <th>Description</th>
                                <td>
                                    <asp:Label ID="lDesc" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <th>Short Name</th>
                                <td>
                                    <asp:Label ID="lShortName" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <th>Sector</th>
                                <td>
                                    <asp:Label ID="lSector" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <th>Usage Notes</th>
                                <td>
                                    <asp:Label ID="lUsage" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <th>Last Updated</th>
                                <td>
                                    <asp:Label ID="lUpdated" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <th>Tier 1</th>
                                <td>
                                    <asp:Label ID="lTier1" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <th>Tier 2</th>
                                <td>
                                    <asp:Label ID="lTier2" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <th>Tier 3</th>
                                <td>
                                    <asp:Label ID="lTier3" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>
