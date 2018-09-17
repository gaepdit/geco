<%@ Page Title="Stack Release Point Details - GECO Facility Inventory" Language="VB"
    MasterPageFile="eismaster.master" AutoEventWireup="false"
    Inherits="GECO.eis_stack_details" Codebehind="stack_details.aspx.vb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
    <asp:ScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="pageheader">
        Stack Release Point Details
        <asp:Button ID="btnReturnToSummary" runat="server" Text="Summary" CausesValidation="False"
            CssClass="summarybutton" PostBackUrl="~/eis/releasepoint_summary.aspx" />
    </div>
    <div class="fieldwrapperseparator">
        <asp:Label ID="lblStackDetails" class="styledseparator" runat="server" Text="Stack Information"></asp:Label>
        <div class="sepbuttons">
            <asp:Button ID="btnAddStack" runat="server" Text="Add" ToolTip="" Font-Size="Small"
                CausesValidation="False" />
            <act:ModalPopupExtender ID="btnAddStack_ModalPopupExtender" runat="server" BackgroundCssClass="modalProgressGreyBackground"
                CancelControlID="btnCancelStack" DynamicServicePath="" Enabled="True" PopupControlID="pnlPopUpAddStack"
                TargetControlID="btnAddStack">
            </act:ModalPopupExtender>
            &nbsp;
            <asp:Button ID="btnDuplicate" runat="server" Text="Duplicate"
                ToolTip="Create a duplicate of this stack." Font-Size="Small"
                CausesValidation="False" Style="height: 21px" />
            <act:ModalPopupExtender ID="btnDuplicate_ModalPopupExtender" runat="server"
                DynamicServicePath="" Enabled="True" PopupControlID="pnlDuplicate" TargetControlID="btnDuplicate">
            </act:ModalPopupExtender>
            &nbsp;
            &nbsp;<asp:Button ID="btnEdit" runat="server" Text="Edit" Font-Size="Small" CausesValidation="False" />
        </div>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblReleasePointID" class="styled" runat="server" Text="Stack ID:"></asp:Label>
        <asp:TextBox ID="txtReleasePointID" runat="server" Text="" class="readonly"
            ReadOnly="True" Width="100px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblRPDescription" class="styled" runat="server" Text="Stack Description:"></asp:Label>
        <asp:TextBox ID="txtRPDescription" runat="server" Text="" class="readonly" ReadOnly="True"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblRPTypeCode" class="styled" runat="server" Text="Stack Type:"></asp:Label>
        <asp:TextBox ID="txtRPTypeCode" runat="server" Text="" class="readonly"
            ReadOnly="True" Width="200px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblRPStatusCode" class="styled" runat="server" Text="Stack Operating Status:"></asp:Label>
        <asp:TextBox ID="txtRPStatusCode" runat="server" Text="" class="readonly"
            ReadOnly="True" Width="250px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblRPStackHeightMeasure" class="styled" runat="server" Text="Stack Height (ft):"></asp:Label>
        <asp:TextBox ID="txtRPStackHeightMeasure" runat="server" Text="" class="readonly"
            ReadOnly="True" Width="100px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblRPStackDiameterMeasure" class="styled" runat="server" Text="Stack Diameter (ft):"></asp:Label>
        <asp:TextBox ID="txtRPStackDiameterMeasure" runat="server" Text="" class="readonly"
            ReadOnly="True" Width="100px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblRPExitGasVelocityMeasure" class="styled" runat="server" Text="Exit Gas Velocity (fps):"></asp:Label>
        <asp:TextBox ID="txtRPExitGasVelocityMeasure" runat="server" Text="" class="readonly"
            ReadOnly="True" Width="100px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblRPExitGasFlowRateMeasure" class="styled" runat="server" Text="Exit Gas Flow Rate (acfs):"></asp:Label>
        <asp:TextBox ID="txtRPExitGasFlowRateMeasure" runat="server" Text="" class="readonly"
            ReadOnly="True" Width="150px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblRPExitGasTemperatureMeasure" class="styled" runat="server" Text="Exit Gas Temperature (°F):"></asp:Label>
        <asp:TextBox ID="txtRPExitGasTemperatureMeasure" runat="server" Text="" class="readonly"
            ReadOnly="True" Width="100px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblRPFenceLineDistanceMeasure" class="styled" runat="server" Text="Fence Line Distance (ft):"></asp:Label>
        <asp:TextBox ID="txtRPFenceLineDistanceMeasure" runat="server" Text="" class="readonly"
            ReadOnly="True" Width="100px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblRPComment" class="styled" runat="server" Text="Comment:"></asp:Label>
        <asp:TextBox ID="txtRPComment" runat="server" Text="" class="readonly" TextMode="MultiLine" Rows="4" 
            Width="400px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblLastEISSubmit" class="styled" runat="server" Text="Last Submitted to EPA:"></asp:Label>
        <asp:TextBox ID="txtLastEISSubmit" class="readonly" runat="server" Text="" ReadOnly="True"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblLastUpdated" class="styled" runat="server" Text="Last Updated On:"></asp:Label>
        <asp:TextBox ID="txtLastUpdate" class="readonly" runat="server" Text="" ReadOnly="True"></asp:TextBox>
    </div>
    <br />
    <div class="fieldwrapperseparator">
        <asp:Label ID="lblSeparator2" class="styledseparator" runat="server" Text="Stack Geographic Coordinate Information"></asp:Label>
        <asp:Label ID="lblNoRPGeoCoordInfo" runat="server" Text=""></asp:Label>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="LblLatitudeMeasure" class="styled" runat="server" Text="Latitude:"></asp:Label>
        <asp:TextBox ID="TxtLatitudeMeasure" runat="server" Text="" class="readonly" ReadOnly="True"
            Width="250px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="LblLongitudeMeasure" class="styled" runat="server" Text="Longitude:"></asp:Label>
        <asp:TextBox ID="TxtLongitudeMeasure" runat="server" Text="" class="readonly" ReadOnly="True"
            Width="250px"></asp:TextBox>
    </div>
    <div style="text-align: center;">
        <asp:Panel ID="pnlLocationMap" runat="server" Width="610px" HorizontalAlign="Left">
            The release point&#39;s location is centered in the map below. If the location is
            incorrect go to the Edit page and make the correction.
            <asp:Image ID="imgGoogleStaticMap" runat="server" ImageUrl="" />
        </asp:Panel>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="LblHorCollectionMetCode" class="styled" runat="server" Text="Horizontal Collection Method:"></asp:Label>
        <asp:TextBox ID="TxtHorCollectionMetCode" runat="server" Text="" class="readonly" Rows="4" 
            ReadOnly="True" Width="400px" Font-Names="Verdana" Font-Size="Small" TextMode="MultiLine"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="LblHorizontalAccuracyMeasure" class="styled" runat="server" Text="Accuracy Measure (meters):"></asp:Label>
        <asp:TextBox ID="TxtHorizontalAccuracyMeasure" runat="server" Text="" class="readonly"
            ReadOnly="True" Width="250px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="LblHorReferenceDatCode" class="styled" runat="server" Text="Horizontal Reference Datum:"></asp:Label>
        <asp:TextBox ID="TxtHorReferenceDatCode" runat="server" Text="" class="readonly"
            ReadOnly="True" Width="250px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="LblGeographicComment" class="styled" runat="server" Text="Comment:"></asp:Label>
        <asp:TextBox ID="TxtGeographicComment" runat="server" Text="" class="readonly" TextMode="MultiLine" Rows="4" 
            Width="400px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblLastEISSubmit_SGC" class="styled" runat="server" Text="Last Submitted to EPA:"></asp:Label>
        <asp:TextBox ID="txtlastEISSubmit_SGC" class="readonly" runat="server" Text="" ReadOnly="True"
            Width="200px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblLastUpdate_SGC" class="styled" runat="server" Text="Last Updated On:"></asp:Label>
        <asp:TextBox ID="txtLastUpdate_SGC" class="readonly" runat="server" Text="" ReadOnly="True"
            Width="300px"></asp:TextBox>
    </div>
    <div class="fieldwrapperseparator">
        <asp:Label ID="lblSeparator" class="styledseparator" runat="server" Text="Release Point Apportionments"></asp:Label>
        <asp:Label ID="lblReleasePointAppMessage" runat="server" Font-Size="Small" ForeColor="#CA0000"
            CssClass="labelwarningleft"></asp:Label><br />
        <asp:Label ID="lblRPShutdownMessage" runat="server" Font-Size="Small" ForeColor="#CA0000"
            CssClass="labelwarningleft"></asp:Label>
    </div>
    <div class="gridview">
        <asp:GridView ID="gvwRPApportionment" runat="server" DataSourceID="SqlDataSourceRPApp"
            CellPadding="4" Font-Names="Arial" Font-Size="Small" ForeColor="#333333" GridLines="None"
            AutoGenerateColumns="False">
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#EFF3FB" />
            <Columns>
                <asp:BoundField DataField="EmissionsUnitID" HeaderText="Emission Unit ID" />
                <asp:HyperLinkField DataNavigateUrlFields="ProcessID,EmissionsUnitID" DataNavigateUrlFormatString="~/eis/rpapportionment_edit.aspx?ep={0}&amp;eu={1}"
                    DataTextField="ProcessID" HeaderText="Process ID" NavigateUrl="~/eis/rpapportionment_edit.aspx" />
                <asp:HyperLinkField DataNavigateUrlFields="ProcessID,EmissionsUnitID" DataNavigateUrlFormatString="~/eis/rpapportionment_edit.aspx?ep={0}&amp;eu={1}"
                    DataTextField="strprocessdescription" HeaderText="Process Description" NavigateUrl="~/eis/rpapportionment_edit.aspx">
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                </asp:HyperLinkField>
                <asp:BoundField DataField="ReleasePointID" HeaderText="Release Point ID" />
                <asp:BoundField DataField="intaveragepercentemissions" HeaderText="Apportionment %" />
            </Columns>
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#2461BF" />
            <AlternatingRowStyle BackColor="White" />
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSourceRPApp" runat="server"></asp:SqlDataSource>
    </div>
    <div class="fieldwrapperseparator">
        <asp:Label ID="lblSeparatorOnly" class="styledseparator" runat="server" Text="_"
            Font-Size="Small" BackColor="#ffffff" ForeColor="#ffffff"></asp:Label>
    </div>
    <div class="buttonwrapper">
        <asp:Button runat="server" ID="btnSummary2" CssClass="buttondiv" Text="Summary" CausesValidation="False"
            PostBackUrl="~/eis/releasepoint_summary.aspx" />
    </div>
    <asp:Panel ID="pnlPopUpAddStack" BackColor="#ffffff" runat="server" BorderColor="#333399"
        BorderStyle="Ridge" ScrollBars="Auto" Width="450px" Style="display: none;">
        <table border="0" cellpadding="2" cellspacing="1" width="100%">
            <tr>
                <td align="center" colspan="2">
                    <strong><span style="color: #4169e1; font-size: 12pt; font-family: Verdana;">Add Stack</span></strong>
                </td>
            </tr>
            <tr>
                <td align="right" width="30%"></td>
                <td>&nbsp;
                </td>
            </tr>
            <tr>
                <td align="right" width="30%" valign="top">Stack ID:
                </td>
                <td valign="top">
                    <asp:TextBox ID="txtNewStackID" runat="server" Font-Names="Arial" Font-Size="Small"
                        MaxLength="6" Width="100px"></asp:TextBox>
                    <act:FilteredTextBoxExtender ID="filtxtNewStackID" runat="server" Enabled="True"
                        FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" TargetControlID="txtNewStackID">
                    </act:FilteredTextBoxExtender>
                </td>
            </tr>
            <tr>
                <td align="right" valign="top" width="30%">&nbsp;
                </td>
                <td valign="top">
                    <asp:RequiredFieldValidator ID="reqvNewStackID" runat="server"
                        ControlToValidate="txtNewStackID" ErrorMessage="*Stack ID required"
                        ValidationGroup="vgStackSave"></asp:RequiredFieldValidator>
                    <asp:CustomValidator ID="cusvStackID" runat="server" ControlToValidate="txtNewStackID"
                        OnServerValidate="StackIDCheck" ErrorMessage="*Stack ID already in use. Enter another."
                        Font-Names="Arial" Font-Size="Small" CssClass="validator"
                        Display="Dynamic" Width="100%"
                        ValidationGroup="vgStackSave"></asp:CustomValidator>
                </td>
            </tr>
            <tr>
                <td align="right" valign="top" width="30%">Stack Type:</td>
                <td valign="top">
                    <asp:DropDownList ID="ddlRPTypeCode" runat="server" class="">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="right" valign="top" width="30%">&nbsp;</td>
                <td valign="top">
                    <asp:RequiredFieldValidator ID="reqvRPtypeCode" runat="server" ControlToValidate="ddlRPTypeCode"
                        Display="Dynamic" ErrorMessage="*Choose stack type"
                        InitialValue="--Select Stack Type--" ValidationGroup="vgStackSave"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="right" width="30%" valign="top">Description:
                </td>
                <td valign="top">
                    <asp:TextBox ID="txtNewStackDesc" runat="server" Font-Names="Arial" Font-Size="Small"
                        MaxLength="100" Width="250px" ValidationGroup="vgStackSave"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right" valign="top" width="30%">&nbsp;</td>
                <td valign="top">
                    <asp:RequiredFieldValidator ID="reqvNewStackDesc" runat="server"
                        ControlToValidate="txtNewStackDesc" ErrorMessage="* Description required"
                        ValidationGroup="vgStackSave"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2" class="style3">After saving you will enter more details on the form that appears.
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">&nbsp;&nbsp;
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <asp:Button ID="btnInsertStack" runat="server" Text="Save" Width="90px" ValidationGroup="vgStackSave" />
                    &nbsp;&nbsp;
                    <asp:Button ID="btnCancelStack" runat="server" Text="Cancel" CausesValidation="False" />
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">&nbsp;&nbsp;
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pnlDuplicate" BackColor="#ffffff" runat="server" BorderColor="#333399"
        BorderStyle="Ridge" ScrollBars="Auto" Width="600px" Style="display: none;">
        <table border="0" cellpadding="2" cellspacing="1" width="100%">
            <tr>
                <td align="center" colspan="2">
                    <strong><span style="color: #4169e1; font-size: 12pt; font-family: Verdana;">Duplicate Stack</span></strong></td>
            </tr>
            <tr>
                <td align="center" colspan="2" style="font-family: Verdana; font-size: small;">All stack data will be duplicated except for geographic coordinate data and comments.<br />
                    After being taken to the Stack Edit page the geographic coordinate data
                    must be entered and saved.</td>
            </tr>
            <tr>
                <td align="right" width="210px"></td>
                <td>&nbsp;
                </td>
            </tr>
            <tr>
                <td align="right" valign="top" width="210px">&nbsp;<asp:Label ID="lblStackID" runat="server" CssClass="label"
                    Text="Stack ID:"></asp:Label>
                </td>
                <td valign="top">
                    <asp:TextBox ID="txtDupStackID" runat="server" Font-Names="Arial" Font-Size="Small"
                        MaxLength="6" CssClass="editable" Width="100px"></asp:TextBox>
                    <act:FilteredTextBoxExtender ID="txtDupStackID_FilteredTextBoxExtender"
                        runat="server" Enabled="True" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters"
                        TargetControlID="txtDupStackID" ValidChars="-">
                    </act:FilteredTextBoxExtender>
                </td>
            </tr>
            <tr>
                <td align="right" valign="top" width="210px">&nbsp;
                </td>
                <td valign="top">
                    <asp:CustomValidator ID="cusvDuplicate" runat="server" ControlToValidate="txtDupStackID"
                        OnServerValidate="StackDupIDCheck" ErrorMessage="* Stack ID already used. Enter another."
                        Font-Names="Arial" Font-Size="Small" CssClass="validator" Display="Dynamic" Width="100%"
                        ValidationGroup="DupStack"></asp:CustomValidator>
                    <asp:RequiredFieldValidator ID="reqvDupStackID" runat="server" ControlToValidate="txtDupStackID"
                        CssClass="validator" ErrorMessage="* Stack ID is required." Width="100%"
                        ValidationGroup="DupStack"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="right" valign="top" width="210px">&nbsp;<asp:Label ID="lblStackDesc" runat="server" CssClass="label" Text="Description:"></asp:Label>
                </td>
                <td valign="top">
                    <asp:TextBox ID="txtDupStackDescription" runat="server" Font-Names="Arial" Font-Size="Small"
                        MaxLength="100" Width="400px" CssClass="editable"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right" valign="top" width="210px">&nbsp;
                </td>
                <td valign="top">
                    <asp:RequiredFieldValidator ID="reqvDupStackDesc" runat="server" ControlToValidate="txtDupStackDescription"
                        CssClass="validator" ErrorMessage="* Description is required."
                        Width="100%" ValidationGroup="DupStack"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">&nbsp;<asp:Label ID="Label7" runat="server" CssClass="label" Text="After saving you will be taken to the form to edit any changes."></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">&nbsp;&nbsp;
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <asp:Button ID="btnDupInsertStack" runat="server" Text="Save" Width="90px" ValidationGroup="DupStack" />
                    &nbsp;&nbsp;
                    <asp:Button ID="btnCancelDupStack" runat="server" Text="Cancel" CausesValidation="False" />
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">&nbsp;&nbsp;
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>