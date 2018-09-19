<%@ Page Title="Edit Release Point Apportionment" Language="VB" MasterPageFile="eismaster.master"
    AutoEventWireup="false" Inherits="GECO.eis_rpapportionment_edit" Codebehind="rpapportionment_edit.aspx.vb" %>
<%@ Register src="../Controls/PreventRePost.ascx" tagname="PreventRePost" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
    <script type="text/javascript">
        function Count(text) {
            var maxlength = 400; //set your value here (or add a parm and pass it in)
            var object = document.getElementById(text.id)  //get your object
            var div = document.getElementById("dvComment");
            div.innerHTML = object.value.length + '/' + maxlength + ' character(s).';
            if (object.value.length > maxlength) {
                object.focus(); //set focus to prevent jumping
                object.value = text.value.substring(0, maxlength); //truncate the value
                object.scrollTop = object.scrollHeight; //scroll to the end to prevent jumping
                return false;
            }
            return true;
        }
    </script>
    <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></act:ToolkitScriptManager>
    <div class="pageheader">
        Edit Release Point Apportionment
    </div>
    <div class="fieldwrapperseparator">
        <asp:Label ID="lblSeparator" CssClass="styledseparator" runat="server" Text="Release Point Apportionment"></asp:Label>
        <div class="sepbuttons">
            &nbsp;<asp:Button runat="server" ID="btnProcessSummary" Text="Return to Process Details"
                ToolTip="" Font-Size="Small" CausesValidation="False" />
        </div>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblEmissionsUnitID" CssClass="styled" runat="server" Text="Emission Unit ID:"></asp:Label>
        <asp:TextBox ID="txtEmissionsUnitID" runat="server" CssClass="readonly" Text=""
            MaxLength="6"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblEmissionsUnitDesc" CssClass="styled" runat="server" Text="Emission Unit Description:"></asp:Label>
        <asp:TextBox ID="txtEmissionsUnitDesc" runat="server" CssClass="readonly"
            MaxLength="100"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblProcessID" CssClass="styled" runat="server" Text="Process ID:"></asp:Label>
        <asp:TextBox ID="txtProcessID" runat="server" CssClass="readonly" Text=""
            MaxLength="6"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblProcessDesc" CssClass="styled" runat="server" Text="Process Description:"></asp:Label>
        <asp:TextBox ID="txtProcessDesc" runat="server" CssClass="readonly" Text=""
            MaxLength="200"></asp:TextBox>
    </div>
    <br />
    <asp:Label ID="lblRPApportionmentDeleteWarning" runat="server" Font-Bold="False"
        ForeColor="#FFFFFF" BackColor="Red" Font-Size="Medium"></asp:Label><br />
    <br />
    <div class="gridview">
        <asp:GridView ID="gvwRPApportionment" runat="server" OnRowCommand="gvwRPApportionment_RowCommand"
            DataKeyNames="FacilitySiteID,EmissionsUnitID,ProcessID,ReleasePointID"
            CellPadding="4" Font-Names="Verdana" Font-Size="Small" ForeColor="#333333"
            GridLines="None" AutoGenerateColumns="False">
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#EFF3FB" />
            <Columns>
                <asp:CommandField ShowEditButton="True" ValidationGroup="vgApportionmentGVW">
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                </asp:CommandField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="GoToReleasePointID" runat="server" CausesValidation="False" CommandName="Delete"
                            OnClientClick="return confirm('Are you sure you want to delete this apportionment?');"
                            Text="Delete" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                </asp:TemplateField>
                <asp:BoundField DataField="FacilitySiteID" Visible="false" />
                <asp:BoundField DataField="EmissionsUnitID" Visible="false" />
                <asp:BoundField DataField="ProcessID" Visible="False" />
                <asp:BoundField HeaderText="Release Point ID" DataField="ReleasePointID"
                    ReadOnly="True">
                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Release Point Description"
                    DataField="strRPDescription" ReadOnly="True">
                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                </asp:BoundField>
                <asp:BoundField DataField="RPType" HeaderText="Release Point Type" ReadOnly="True">
                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="Average % Emissions">
                    <ItemTemplate>
                        <asp:Label ID="lblAvgPctEmissions" runat="server" Text='<%# Eval("AvgPctEmissions")%>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtAvgPctEmissions" runat="server" MaxLength="4" Text='<%# Eval("AvgPctEmissions")%>' ValidationGroup="vgRPApportionmentgvw"></asp:TextBox>
                        <act:FilteredTextBoxExtender ID="filtxtAvgPctEmissions" runat="server" Enabled="True"
                            TargetControlID="txtAvgPctEmissions" FilterType="Numbers">
                        </act:FilteredTextBoxExtender>
                        <asp:RequiredFieldValidator ID="reqvAvgPctEmissions" runat="server" ValidationGroup="vgApportionmentGVW"
                            ControlToValidate="txtAvgPctEmissions" Display="Dynamic" ErrorMessage="Average percent emissions required.">*
                        </asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="rngvAvgPctEmissions" runat="server" ValidationGroup="vgApportionmentGVW"
                            ControlToValidate="txtAvgPctEmissions" Display="Dynamic" MaximumValue="100" MinimumValue="1"
                            ErrorMessage="The averqage percent emissions must be between 1 and 100 percent." Type="Double">*</asp:RangeValidator>
                    </EditItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Apportionment Comment">
                    <ItemTemplate>
                        <asp:Label ID="lblRPApportionmentComment" runat="server" Text='<%# Eval("RPApportionmentComment")%>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtRPApportionmentComment" runat="server" MaxLength="100" Text='<%# Eval("RPApportionmentComment")%>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                </asp:TemplateField>
                <asp:BoundField DataField="LastEISSubmitDate" HeaderText="Last EPA Submittal"
                    ReadOnly="True" DataFormatString="{0:d}">
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                </asp:BoundField>
            </Columns>
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#BCD2EE" />
            <AlternatingRowStyle BackColor="White" />
        </asp:GridView>
        <br />
        <div id="RPApportionmentCheck">
            <asp:Label ID="lblRPApportionmentTotal" runat="server" Font-Bold="True" Font-Size="Medium"
                Text="Apportionment Total:  "></asp:Label>
            <asp:TextBox ID="txtRPApportionmentTotal" runat="server" ReadOnly="True" MaxLength="3"
                Width="55px" Font-Bold="True" BorderStyle="None"></asp:TextBox><br />
            <asp:Label ID="lblRPApportionmentWarning" runat="server" BackColor="#ffffff" Font-Names="Verdana"
                Font-Size="Medium" Font-Bold="True" ForeColor="#FF0000" Height="70px" Width="500px"></asp:Label>
        </div>
        <act:RoundedCornersExtender ID="lblRPApportionmentWarning_RoundedCornersExtender"
            runat="server" Enabled="True" TargetControlID="lblRPApportionmentWarning">
        </act:RoundedCornersExtender>
        <br />
    </div>
    <asp:ValidationSummary ID="vgApportionmentGVW" runat="server"
        HeaderText="The following items need to be corrected in the table above. See items marked with *"
        ValidationGroup="vgApportionmentGVW" />
    <asp:ValidationSummary ID="vgRPApportionmentaDD" runat="server"
        HeaderText="The following items need to be corrected below. See items marked with *"
        ValidationGroup="vgRPApportionmentAdd" />
    <br />
    <div class="fieldwrapper">
        <asp:Label ID="lblddlReleasePoint" CssClass="styled" runat="server" Text="Release Point ID: "></asp:Label>
        <asp:DropDownList ID="ddlReleasePointID" runat="server">
        </asp:DropDownList>
        &nbsp;
        <asp:RequiredFieldValidator ID="reqvReleasePointID" runat="server" ControlToValidate="ddlReleasePointID"
            Display="Dynamic" ErrorMessage="Select release point ID"
            InitialValue="--Select a Release Point --"
            ValidationGroup="vgRPApportionmentAdd">*</asp:RequiredFieldValidator>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblPctAverageEmissions" class="styled" runat="server" Text="Average Percent Emissions:"></asp:Label>
        <asp:TextBox ID="txtAvgPctEmissions" runat="server" class="editable" MaxLength="3"
            ToolTip="Each entry must be greater than or equal to 1, or less than or equal to 100. The total of all entries in the table above must be 100%."
            Width="150px"></asp:TextBox>
        <act:FilteredTextBoxExtender ID="ftbeAveragePercentEmissions" runat="server" TargetControlID="txtAvgPctEmissions"
            FilterType="Numbers">
        </act:FilteredTextBoxExtender>
        &nbsp;<asp:RequiredFieldValidator ID="reqvAvgPctEmissions" runat="server" ControlToValidate="txtAvgPctEmissions"
            Display="Dynamic" ErrorMessage="Average percent emissions required."
            ValidationGroup="vgRPApportionmentAdd">*</asp:RequiredFieldValidator>
        <asp:RangeValidator ID="rngvRPApportionment"
            runat="server" ControlToValidate="txtAvgPctEmissions"
            MaximumValue="100" MinimumValue="1" Display="Dynamic"
            ErrorMessage="Average percent emissions must be greater than or equal to 1, or less than or equal to 100."
            Type="Integer" ValidationGroup="vgRPApportionmentAdd">* Must be greater than or equal to 1, or less than or equal to 100.</asp:RangeValidator>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblRPApportionmentComment" CssClass="styled" runat="server" Text="Comment:"></asp:Label>
        <div style="display: inline-block">
            <asp:TextBox ID="txtRPApportionmentComment" Rows="4" onKeyUp="javascript:Count(this);" runat="server" CssClass="editable" TextMode="MultiLine"
                Text="" Width="400px"></asp:TextBox>
            <div id="dvComment" style="font: bold"></div>
            <asp:RegularExpressionValidator ID="regexpName" runat="server"
                ErrorMessage="Comment not to exceed 400 characters."
                ControlToValidate="txtRPApportionmentComment"
                ValidationExpression="^[\s\S]{0,400}$" ValidationGroup="vgRPApportionmentAdd" />
        </div>
    </div>
    <div class="buttonwrapper">
        <asp:Button runat="server" ID="btnAddRPApportionment" CssClass="buttondiv" Text="Add"
            Width="75px" ValidationGroup="vgRPApportionmentAdd" />
        <asp:Button runat="server" ID="btnClearRPApportionment" CssClass="buttondiv" Text="Clear"
            Width="75px" CausesValidation="False" />
        <asp:Button runat="server" ID="btnCancel2" CssClass="buttondiv"
            Text="Return to Process Details" CausesValidation="False"
            ToolTip="Takes you back to the Emission Unit Details page"
            UseSubmitBehavior="False" />
        <br />
    </div>
    <uc1:PreventRePost ID="PreventRePost1" runat="server" />
</asp:Content>
