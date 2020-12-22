﻿<%@ Page MasterPageFile="~/EIS/EIS.master" Language="VB" AutoEventWireup="false"
    Title="GECO EIS: Facility Status"
    Inherits="GECO.EIS_Process_Default" CodeBehind="Default.aspx.vb" %>

<%@ MasterType VirtualPath="~/EIS/EIS.master" %>
<asp:Content ID="Content3" ContentPlaceHolderID="Content" runat="Server">
    <ul class="form-progress">
        <li class="done">Facility Information</li>
        <li class="done">CAERS Users</li>
        <li class="current">Facility Status</li>
    </ul>

    <asp:UpdatePanel ID="UpdatePanel" runat="server">
        <ContentTemplate>

            <h2>Facility Operational Status</h2>

            <p><b>Did the facility operate at any time during calendar year <%=EiStatus.MaxYear %>?</b></p>

            <asp:RadioButtonList ID="rOperate" runat="server" RepeatLayout="Flow"
                RepeatDirection="Horizontal" AutoPostBack="true">
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
                    The facility is <em><% If Not SummerDayRequired Then %> not <% End If %> 
                    located in the ozone nonattainment area.</em>
                    The thresholds in the table below pertain to the facility's location. 
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
                    Remember that the thresholds pertain to <em>potential emissions,</em> except for lead.
                    The threshold for lead (Pb) is based on actual emissions.
                </p>

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

            <asp:Panel ID="pnlSubmit" runat="server" Visible="false">
                <h2>Submittal</h2>

                <p class="message-highlight">
                    <% If Participating = 1 Then %>
                    The facility <em>will not</em> participate in the <%=EiStatus.MaxYear %> Emissions Inventory because it did not operate.
                    <% ElseIf Participating = 2 Then %>
                    The facility <em>will not</em> participate in the <%=EiStatus.MaxYear %> Emissions Inventory because all emissions were below the thresholds.
                    <% ElseIf Participating = 3 Then %>
                    The facility <em>will</em> participate in the <%=EiStatus.MaxYear %> Emissions Inventory.
                    <% End If %>
                </p>

                <p>Click "Submit" to submit your selections, or click Cancel to make changes.</p>
                <p>
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="button-large button-proceed" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="button-large button-cancel" />
                </p>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
