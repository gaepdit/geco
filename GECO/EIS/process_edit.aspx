<%@ Page Title="Process Edit - GECO Facility Inventory" Language="VB" MasterPageFile="eismaster.master"
    AutoEventWireup="false" MaintainScrollPositionOnPostback="true"
    Inherits="GECO.eis_process_edit" CodeBehind="process_edit.aspx.vb" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
    <script type="text/javascript">
        function Count(text) {
            var maxlength = 400;
            var object = document.getElementById(text.id)
            var div = document.getElementById("dvComment");
            div.innerHTML = object.value.length + '/' + maxlength + ' character(s).';
            if (object.value.length > maxlength) {
                object.focus();
                object.value = text.value.substring(0, maxlength);
                object.scrollTop = object.scrollHeight;
                return false;
            }
            return true;
        }
    </script>
    <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></act:ToolkitScriptManager>
    <acs:ModalUpdateProgress ID="ModalUpdateProgress1" runat="server" DisplayAfter="0"
        BackgroundCssClass="modalProgressGreyBackground">
        <ProgressTemplate>
            <div class="modalPopup">
                &nbsp;<img src="<%= Page.ResolveUrl("~/assets/images/progressbar_green.gif") %>" alt="" align="middle" /><br />
                <span style="font-family: Verdana; font-size: small; font-weight: normal">Please Wait...</span>
            </div>
        </ProgressTemplate>
    </acs:ModalUpdateProgress>

    <asp:Panel ID="pnlProcess" runat="server">
        <div class="pageheader">
            Edit Process Details
            <asp:Button ID="btnReturnToSummary" runat="server" Text="Summary" CausesValidation="False"
                CssClass="summarybutton" UseSubmitBehavior="False"
                PostBackUrl="~/eis/process_summary.aspx" />
        </div>
        <div class="fieldwrapperseparator">
            <asp:Label ID="lblSeparatorNoText" class="styledseparator" runat="server" Text="Process"
                BackColor="#ffffff" ForeColor="#ffffff"></asp:Label>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="sepbuttons">
                        <asp:Label ID="lblMessageTop" runat="server" Font-Bold="True" Font-Names="Arial"
                            Font-Size="Small" ForeColor="Red"></asp:Label>
                        &nbsp;
                <asp:Button ID="btnSave2" runat="server" Text="Save" ToolTip="" Font-Size="Small"
                    ValidationGroup="vgProcessEdit" UseSubmitBehavior="False" />&nbsp;
                <asp:Button ID="btnCancel2" runat="server" Text="Return to Details" ToolTip="" Font-Size="Small"
                    CausesValidation="False" UseSubmitBehavior="False" />
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <asp:ValidationSummary ID="sumvProcessEdit" runat="server" HeaderText="You received the following errors:"
            ValidationGroup="vgProcessEdit"></asp:ValidationSummary>
        <div class="fieldwrapper">
            <asp:Label ID="lblEmissionUnitID" class="styled" runat="server" Text="Emission Unit ID:"></asp:Label>
            <asp:TextBox ID="txtEmissionUnitID" runat="server" class="editable" Text="" Width="100px"
                ReadOnly="True" BorderStyle="None"></asp:TextBox>
        </div>
        <div class="fieldwrapper">
            <asp:Label ID="lblEmissionUnitDesc" class="styled" runat="server" Text="Emission Unit Description:"></asp:Label>
            <asp:TextBox ID="txtEmissionUnitDesc" runat="server" Text="" class="readonly" ReadOnly="True"></asp:TextBox>
        </div>
        <div class="fieldwrapper">
            <asp:Label ID="lblProcessID" class="styled" runat="server" Text="Process ID:"></asp:Label>
            <asp:TextBox ID="txtProcessID" runat="server" class="editable" Text="" Width="100px"
                ReadOnly="True" BorderStyle="None"></asp:TextBox>
        </div>
        <div class="fieldwrapper">
            <asp:Label ID="lblProcessDescription" class="styled" runat="server" Text="Process Description:"></asp:Label>
            <asp:TextBox ID="txtProcessDescription" runat="server" MaxLength="200" class="editable" Text=""></asp:TextBox>
            <asp:RequiredFieldValidator ID="reqvProcessDesc" runat="server" ControlToValidate="txtProcessDescription"
                Display="Dynamic" ErrorMessage="The Process Description is required." ValidationGroup="vgProcessEdit">*</asp:RequiredFieldValidator>
        </div>

        <div class="fieldwrapper">
            <asp:Label ID="lblSourceClassCode" class="styled" runat="server" Text="Source Classification Code:"></asp:Label>
            <asp:TextBox ID="txtSourceClassCode" runat="server" class="editable" Text="" Width="100px"
                MaxLength="10"></asp:TextBox>
            <act:FilteredTextBoxExtender ID="filtxtSourceClassCode" runat="server" Enabled="True"
                FilterType="Numbers" TargetControlID="txtSourceClassCode">
            </act:FilteredTextBoxExtender>
            <asp:RequiredFieldValidator ID="reqvSCC" runat="server" ControlToValidate="txtSourceClassCode"
                Display="Dynamic" ErrorMessage="The Source Classification Code is required."
                ValidationGroup="vgProcessEdit" CssClass="validator">*</asp:RequiredFieldValidator>
            <asp:CustomValidator ID="cusvSCCcode" runat="server" ControlToValidate="txtSourceClassCode"
                OnServerValidate="SCCCheck" ErrorMessage="The Source Classification Code is not valid. Enter a valid SCC code."
                Font-Names="Arial" Font-Size="Small" Display="Dynamic" ValidationGroup="vgProcessEdit"
                CssClass="validator">*</asp:CustomValidator>
            &nbsp;<asp:Button ID="btnSCCLoopup" runat="server" Text="SCC Lookup" ToolTip="" Font-Size="Small"
                CausesValidation="False" OnClientClick="openSCCLookup();" />
            <asp:Label ID="lblSccDetails" runat="server" />
        </div>
        <div class="fieldwrapper">
            <asp:Label ID="lblProcessComment" class="styled" runat="server" Text="Process Comment:"></asp:Label>
            <div style="display: inline-block">
                <asp:TextBox ID="txtProcessComment" runat="server" Rows="4" onKeyUp="javascript:Count(this);" class="editable" TextMode="MultiLine"
                    Text="" Width="400px"></asp:TextBox>
                <asp:RegularExpressionValidator ID="regexpProcessComment" runat="server"
                    ErrorMessage="Comment not to exceed 400 characters."
                    ControlToValidate="txtProcessComment"
                    ValidationExpression="^[\s\S]{0,400}$" ValidationGroup="vgProcessEdit" />
                <div id="dvComment" style="font: bold"></div>
            </div>
        </div>
        <div class="buttonwrapper">
            <asp:Button runat="server" ID="btnSave" CssClass="buttondiv" Text="Save" ValidationGroup="vgProcessEdit" />
            <asp:Button runat="server" ID="btnCancel" CssClass="buttondiv"
                Text="Return to Details" UseSubmitBehavior="False" Width="120px" />
            <asp:Button runat="server" ID="btnReturnSummary" CssClass="buttondiv" Text="Summary"
                CausesValidation="False" PostBackUrl="~/eis/process_summary.aspx" />
            <asp:Button runat="server" ID="btnDeleteProcess" CssClass="buttondiv" Text="Delete" />
            <act:ModalPopupExtender ID="mpeDeleteProcess" runat="server" DynamicServicePath=""
                Enabled="True" TargetControlID="btnDeleteProcess" BackgroundCssClass="modalProgressGreyBackground"
                PopupControlID="pnlConfirmDelete">
            </act:ModalPopupExtender>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlConfirmDelete" runat="server" Width="450px" BackColor="White" BorderColor="Black"
        BorderStyle="Solid" Style="display: none;">
        <div class="confirmdelete">
            <table align="center">
                <tr>
                    <td>&nbsp; &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="style3">Confirm Process Deletion
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <strong>
                            <asp:Label ID="lblConfirmDelete1" runat="server" Text="Are you sure you want to delete this Process?"></asp:Label></strong>
                        <br />
                        <br />
                        <asp:Label ID="lblConfirmDelete2" runat="server" Text="Note: All process control approach information as well as control measures and associated pollutants will also be deleted."></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;&nbsp;
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button ID="btnDeleteProcessOK" runat="server" Text="Delete" Width="60px" />
                        <asp:Button ID="btnNoDelete" runat="server" Text="No" Width="60px" />
                        <asp:Button ID="btnDeleteSummary" runat="server" Text="Summary" Width="80px" PostBackUrl="~/eis/process_summary.aspx" />
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
</asp:Content>
