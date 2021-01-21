<%@ Page MasterPageFile="~/EIS/EIS.master" Language="VB" AutoEventWireup="false"
    Title="GECO EIS: Facility Status"
    Inherits="GECO.EIS_Process_Default" CodeBehind="Default.aspx.vb" %>

<%@ MasterType VirtualPath="~/EIS/EIS.master" %>
<asp:Content ID="Content3" ContentPlaceHolderID="Content" runat="Server">
    <ul class="form-progress">
        <li class="done"><a href="<%= Page.ResolveUrl("~/EIS/Facility/Edit.aspx") %>">Facility Information</a></li>
        <li class="current">Facility Status</li>
        <li>CAERS Users</li>
        <li>Submit</li>
    </ul>

    <asp:UpdatePanel ID="UpdatePanel" runat="server">
        <ContentTemplate>

            <h2>Facility Operational Status</h2>

            <p><b>Did the facility operate at any time during calendar year <%=EiStatus.MaxYear %>?</b></p>

            <asp:RadioButtonList ID="rOperate" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal" AutoPostBack="true">
                <asp:ListItem>Yes</asp:ListItem>
                <asp:ListItem>No</asp:ListItem>
            </asp:RadioButtonList>
            <asp:RequiredFieldValidator ID="reqvOperateYesNo" runat="server" ControlToValidate="rOperate"
                ErrorMessage="Select Yes or No to continue." />

            <p>
                Comments (optional):<br />
                <asp:TextBox ID="txtComment" runat="server" TextMode="MultiLine" Rows="4" />
            </p>

            <asp:Panel ID="pnlEmissions" runat="server" Visible="false">
                <h2>Facility Emissions Thresholds</h2>

                <p>
                    Note that the thresholds below pertain to <em>potential emissions,</em> except for lead.
                    The threshold for lead (Pb) is based on actual emissions.
                </p>

                <asp:GridView ID="gThresholds" runat="server" AutoGenerateColumns="false" CssClass="table-simple">
                    <Columns>
                        <asp:TemplateField HeaderText="Pollutant">
                            <ItemTemplate><%# Eval("Pollutant") %></ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Threshold (tpy)" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate><%# Eval("Threshold", "{0:0.##}") %></ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>

                <p>
                    <b>Is the facility below <strong>ALL</strong> of the thresholds listed?</b><br />
                </p>

                <asp:RadioButtonList ID="rThresholds" runat="server" RepeatLayout="Flow"
                    RepeatDirection="Horizontal" AutoPostBack="True">
                    <asp:ListItem>Yes</asp:ListItem>
                    <asp:ListItem>No</asp:ListItem>
                </asp:RadioButtonList>
                <asp:RequiredFieldValidator ID="reqThresholds" runat="server" ControlToValidate="rThresholds"
                    ErrorMessage="Select Yes or No to continue." />
            </asp:Panel>

            <asp:Panel ID="pnlColocate" runat="server" Visible="false">
                <h2>Facility Colocation</h2>

                <p><b>Is the facility colocated with another facility?</b></p>

                <asp:RadioButtonList ID="rColocated" runat="server" RepeatLayout="Flow"
                    RepeatDirection="Horizontal" AutoPostBack="True">
                    <asp:ListItem>Yes</asp:ListItem>
                    <asp:ListItem>No</asp:ListItem>
                </asp:RadioButtonList>
                <asp:RequiredFieldValidator ID="reqIsColocated" runat="server" ControlToValidate="rColocated"
                    ErrorMessage="Select Yes or No to continue." />

                <asp:Panel ID="pnlColocatedWith" runat="server" Visible="false">
                    <p>
                        <b>What facility are you colocated with?</b><br />
                        Please provide the name and AIRS number of the 
                        colocated facility. Your facility and/or the colocated facility will be contacted by 
                        the Air Protection Branch about possible EI submittal.
                    </p>
                    <asp:TextBox ID="txtColocatedWith" runat="server" TextMode="MultiLine" Rows="4" MaxLength="4000" />
                </asp:Panel>
            </asp:Panel>

            <asp:Panel ID="pnlContinue" runat="server" Visible="false">
                <p>
                    <asp:Button ID="btnContinue" runat="server" Text="Continue →" CssClass="button-large button-proceed" />
                </p>
            </asp:Panel>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
