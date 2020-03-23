<%@ Page Title="Facility Thresholds - GECO Emissions Inventory" Language="VB" MaintainScrollPositionOnPostback="true" MasterPageFile="eismaster.master"
    AutoEventWireup="false" Inherits="GECO.EIS_rp_threshold" CodeBehind="rp_facilitythreshold.aspx.vb" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
    <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></act:ToolkitScriptManager>
    <div class="pageheader">
        Facility Emissions Thresholds (Potential Emissions)
    </div>
    <div style="font-size: small" align="left">
        <p>
            According to the facility's AIRS Number, the facility is
            <em>
                <asp:Label ID="lblLocation" runat="server" Text=""></asp:Label></em>
        </p>
        <p>
            The thresholds in the questions below pertain to the facility's location. Participation
            in the Emissions Inventory depends on the responses to the threshold questions below.
        </p>
        <p>
            Remember that the numbers pertain to <b>potential emissions,</b> except for lead.
            <b>The threshold for lead (Pb) is now based on actual emissions.</b>
        </p>
        <p align="center">Select a response for all pollutants.</p>
    </div>
    <div style="font-size: small" align="center">
        <asp:Table ID="tblThreshold" runat="server" HorizontalAlign="Center" BorderWidth="1px"
            CellPadding="4" CellSpacing="0" Width="450px" Font-Names="Arial" Font-Size="Medium">
            <asp:TableHeaderRow runat="server">
                <asp:TableHeaderCell ID="TableCell1" runat="server" BorderWidth="1px">Pollutant</asp:TableHeaderCell>
                <asp:TableHeaderCell ID="TableCell2" runat="server" BorderWidth="1px" HorizontalAlign="Center">Threshold (tpy)</asp:TableHeaderCell>
                <asp:TableHeaderCell ID="TableCell3" runat="server" BorderWidth="1px" HorizontalAlign="Center">Below Threshold?</asp:TableHeaderCell>
            </asp:TableHeaderRow>
            <asp:TableRow ID="TableRow1" runat="server" BackColor="#9BD7FF">
                <asp:TableCell ID="TableCell4" runat="server" BorderWidth="1px">
                    <asp:Label ID="lblSOx" runat="server">SO<sub>x</sub></asp:Label>
                </asp:TableCell>
                <asp:TableCell ID="TableCell5" runat="server" BorderWidth="1px" HorizontalAlign="Center">
                    <asp:Label ID="lblSOxThreshold" runat="server"></asp:Label>
                </asp:TableCell>
                <asp:TableCell ID="TableCell6" runat="server" BorderWidth="1px" HorizontalAlign="Center">
                    <asp:RadioButtonList ID="rblSOx" runat="server" RepeatDirection="Horizontal" Font-Size="Medium"
                        RepeatLayout="Flow">
                        <asp:ListItem Value="Yes">Yes</asp:ListItem>
                        <asp:ListItem Value="No">No</asp:ListItem>
                    </asp:RadioButtonList>
                    <asp:RequiredFieldValidator ID="reqvSOx" runat="server" ValidationGroup="vgThreshold"
                        ControlToValidate="rblSOx"></asp:RequiredFieldValidator>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow2" runat="server">
                <asp:TableCell ID="TableCell7" runat="server" BorderWidth="1px">
                    <asp:Label ID="lblVOC" runat="server" Text="VOC"></asp:Label>
                </asp:TableCell>
                <asp:TableCell ID="TableCell8" runat="server" BorderWidth="1px" HorizontalAlign="Center">
                    <asp:Label ID="lblVOCThreshold" runat="server"></asp:Label>
                </asp:TableCell>
                <asp:TableCell ID="TableCell9" runat="server" BorderWidth="1px" HorizontalAlign="Center">
                    <asp:RadioButtonList ID="rblVOC" runat="server" RepeatDirection="Horizontal" Font-Size="Medium"
                        RepeatLayout="Flow">
                        <asp:ListItem Value="Yes">Yes</asp:ListItem>
                        <asp:ListItem Value="No">No</asp:ListItem>
                    </asp:RadioButtonList>
                    <asp:RequiredFieldValidator ID="reqvVOC" runat="server" ValidationGroup="vgThreshold"
                        ControlToValidate="rblVOC"></asp:RequiredFieldValidator>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow3" runat="server" BackColor="#9BD7FF">
                <asp:TableCell ID="TableCell10" runat="server" BorderWidth="1px">
                    <asp:Label ID="lblNOx" runat="server">NO<sub>x</sub></asp:Label>
                </asp:TableCell>
                <asp:TableCell ID="TableCell11" runat="server" BorderWidth="1px" HorizontalAlign="Center">
                    <asp:Label ID="lblNOxThreshold" runat="server"></asp:Label>
                </asp:TableCell>
                <asp:TableCell ID="TableCell12" runat="server" BorderWidth="1px" HorizontalAlign="Center">
                    <asp:RadioButtonList ID="rblNOx" runat="server" RepeatDirection="Horizontal" Font-Size="Medium"
                        RepeatLayout="Flow">
                        <asp:ListItem Value="Yes">Yes</asp:ListItem>
                        <asp:ListItem Value="No">No</asp:ListItem>
                    </asp:RadioButtonList>
                    <asp:RequiredFieldValidator ID="reqvNOx" runat="server" ValidationGroup="vgThreshold"
                        ControlToValidate="rblNOx"></asp:RequiredFieldValidator>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow4" runat="server">
                <asp:TableCell ID="TableCell13" runat="server" BorderWidth="1px">
                    <asp:Label ID="lblCO" runat="server" Text="CO"></asp:Label>
                </asp:TableCell>
                <asp:TableCell ID="TableCell14" runat="server" BorderWidth="1px" HorizontalAlign="Center">
                    <asp:Label ID="lblCOThreshold" runat="server"></asp:Label>
                </asp:TableCell>
                <asp:TableCell ID="TableCell15" runat="server" BorderWidth="1px" HorizontalAlign="Center">
                    <asp:RadioButtonList ID="rblCO" runat="server" RepeatDirection="Horizontal" Font-Size="Medium"
                        RepeatLayout="Flow">
                        <asp:ListItem Value="Yes">Yes</asp:ListItem>
                        <asp:ListItem Value="No">No</asp:ListItem>
                    </asp:RadioButtonList>
                    <asp:RequiredFieldValidator ID="reqvCO" runat="server" ValidationGroup="vgThreshold"
                        ControlToValidate="rblCO"></asp:RequiredFieldValidator>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow5" runat="server" BackColor="#9BD7FF">
                <asp:TableCell ID="TableCell16" runat="server" BorderWidth="1px">
                    <asp:Label ID="lblPM10" runat="server">PM<sub>10</sub></asp:Label>
                </asp:TableCell>
                <asp:TableCell ID="TableCell17" runat="server" BorderWidth="1px" HorizontalAlign="Center">
                    <asp:Label ID="lblPM10Threshold" runat="server"></asp:Label>
                </asp:TableCell>
                <asp:TableCell ID="TableCell18" runat="server" BorderWidth="1px" HorizontalAlign="Center">
                    <asp:RadioButtonList ID="rblPM10" runat="server" RepeatDirection="Horizontal" Font-Size="Medium"
                        RepeatLayout="Flow">
                        <asp:ListItem Value="Yes">Yes</asp:ListItem>
                        <asp:ListItem Value="No">No</asp:ListItem>
                    </asp:RadioButtonList>
                    <asp:RequiredFieldValidator ID="reqvPM10" runat="server" ValidationGroup="vgThreshold"
                        ControlToValidate="rblPM10"></asp:RequiredFieldValidator>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow6" runat="server">
                <asp:TableCell ID="TableCell19" runat="server" BorderWidth="1px">
                    <asp:Label ID="lblPM25" runat="server">PM<sub>2.5</sub></asp:Label>
                </asp:TableCell>
                <asp:TableCell ID="TableCell20" runat="server" BorderWidth="1px" HorizontalAlign="Center">
                    <asp:Label ID="lblPM25Threshold" runat="server"></asp:Label>
                </asp:TableCell>
                <asp:TableCell ID="TableCell21" runat="server" BorderWidth="1px" HorizontalAlign="Center">
                    <asp:RadioButtonList ID="rblPM25" runat="server" RepeatDirection="Horizontal" Font-Size="Medium"
                        RepeatLayout="Flow">
                        <asp:ListItem Value="Yes">Yes</asp:ListItem>
                        <asp:ListItem Value="No">No</asp:ListItem>
                    </asp:RadioButtonList>
                    <asp:RequiredFieldValidator ID="reqvPM25" runat="server" ValidationGroup="vgThreshold"
                        ControlToValidate="rblPM25"></asp:RequiredFieldValidator>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow7" runat="server" BackColor="#9BD7FF">
                <asp:TableCell ID="TableCell22" runat="server" BorderWidth="1px">
                    <asp:Label ID="lblNH3" runat="server">NH<sub>3</sub></asp:Label>
                </asp:TableCell>
                <asp:TableCell ID="TableCell23" runat="server" BorderWidth="1px" HorizontalAlign="Center">
                    <asp:Label ID="lblNH3Threshold" runat="server"></asp:Label>
                </asp:TableCell>
                <asp:TableCell ID="TableCell24" runat="server" BorderWidth="1px" HorizontalAlign="Center">
                    <asp:RadioButtonList ID="rblNH3" runat="server" RepeatDirection="Horizontal" Font-Size="Medium"
                        RepeatLayout="Flow">
                        <asp:ListItem Value="Yes">Yes</asp:ListItem>
                        <asp:ListItem Value="No">No</asp:ListItem>
                    </asp:RadioButtonList>
                    <asp:RequiredFieldValidator ID="reqvNH3" runat="server" ValidationGroup="vgThreshold"
                        ControlToValidate="rblNH3"></asp:RequiredFieldValidator>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow8" runat="server">
                <asp:TableCell ID="TableCell25" runat="server" BorderWidth="1px">
                    <asp:Label ID="lblPb" runat="server" Text="Pb"></asp:Label>
                </asp:TableCell>
                <asp:TableCell ID="TableCell26" runat="server" BorderWidth="1px" HorizontalAlign="Center">
                    <asp:Label ID="lblPbThreshold" runat="server"></asp:Label>
                </asp:TableCell>
                <asp:TableCell ID="TableCell27" runat="server" BorderWidth="1px" HorizontalAlign="Center">
                    <asp:RadioButtonList ID="rblPb" runat="server" RepeatDirection="Horizontal" Font-Size="Medium"
                        RepeatLayout="Flow">
                        <asp:ListItem Value="Yes">Yes</asp:ListItem>
                        <asp:ListItem Value="No">No</asp:ListItem>
                    </asp:RadioButtonList>
                    <asp:RequiredFieldValidator ID="reqvPb" runat="server" ValidationGroup="vgThreshold"
                        ControlToValidate="rblPb"></asp:RequiredFieldValidator>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <br />
        <div>
            <asp:TextBox ID="txtComment" runat="server" class="editable" TextMode="MultiLine" Rows="4"
                Text="" Width="400px" MaxLength="400" Height="50px"></asp:TextBox>
            <act:TextBoxWatermarkExtender ID="txtComment_TextBoxWatermarkExtender" runat="server"
                Enabled="True" TargetControlID="txtComment" WatermarkCssClass="watermarked" WatermarkText="Type optional comments here">
            </act:TextBoxWatermarkExtender>
        </div>
    </div>
    <p>
        Clicking "<asp:Label ID="lblNextButton" runat="server">Continue</asp:Label>" below and submitting
        this form is certification that the facility's potential emissions are as described
        by the selections in the table above.
    </p>
    <asp:ValidationSummary ID="vgThresholds" runat="server" Style="text-align: center"
        ValidationGroup="vgThreshold" HeaderText="One or more selections were not made. All are required." />
    <div id="dOptOut" runat="server" visible="false" style="text-align: center;">
        <asp:Label ID="lblOptOutStatus1" runat="server" ForeColor="#000099" Font-Bold="True" Font-Size="Medium" /><br />

        <asp:Panel ID="pnlColocate" runat="server" Visible="false">
            <p style="font-size: medium;">
                Is your facility colocated with another facility?
            </p>
            <asp:RadioButtonList ID="rblIsColocated" runat="server" RepeatDirection="Horizontal"
                Font-Size="Medium" AutoPostBack="True" RepeatLayout="Flow">
                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                <asp:ListItem Value="No">No</asp:ListItem>
            </asp:RadioButtonList><br />
            <asp:RequiredFieldValidator ID="reqIsColocated" runat="server"
                ControlToValidate="rblIsColocated" ValidationGroup="vgOptOut"
                ErrorMessage="Select Yes or No to continue" Font-Bold="False" />
            <asp:Panel ID="pnlColocation" runat="server" Visible="false">
                <div style="text-align: center; font-size: medium;">
                    What facility are you colocated with? Please provide name and AIRS # of colocated facility.
                    Your facility and/or the colocated facility will be contacted by the Air Protection 
                    Branch about possible EI submittal.
                    <br />
                    <asp:TextBox ID="txtColocatedWith" runat="server" BackColor="White"
                        TextMode="MultiLine" Width="350px" MaxLength="4000" Rows="4"></asp:TextBox>
                    <br />
                </div>
            </asp:Panel>
        </asp:Panel>

        <p>Click Submit to continue or Cancel to make changes above.</p>
    </div>

    <div class="buttonwrapper" style="text-align: center;">
        <asp:Button runat="server" ID="btnContinue" CssClass="button button-large" Text="Continue" ValidationGroup="vgThreshold" />
        <asp:Button runat="server" ID="btnSubmit" CssClass="button button-large" Text="Submit" ValidationGroup="vgOptOut" Visible="False" />
        <asp:Button runat="server" ID="btnCancel" CssClass="button button-large button-cancel" Text="Cancel" CausesValidation="False" Visible="False" />
    </div>
    <br />
    <br />
</asp:Content>
