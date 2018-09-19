<%@ Page Title="Fugitive Details - GECO Facility Inventory" Language="VB" MasterPageFile="eismaster.master" AutoEventWireup="false" Inherits="GECO.eis_fugitive_details" Codebehind="fugitive_details.aspx.vb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
    <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></act:ToolkitScriptManager>
    <div class="pageheader">
        Fugitive Release Point Details
        <asp:Button ID="btnReturntoSummary" runat="server" Text="Summary" CausesValidation="False"
            CssClass="summarybutton" PostBackUrl="~/eis/releasepoint_summary.aspx" />
    </div>
    <div class="fieldwrapperseparator">
        <asp:Label ID="lblFugitiveStacks" class="styledseparator" runat="server" Text="Fugitive Release Point"></asp:Label>
        <div class="sepbuttons">
            <asp:Button ID="btnAddFugitiveRP" runat="server" Text="Add" ToolTip="" Font-Size="Small"
                CausesValidation="False" />
            <act:ModalPopupExtender ID="btnAddFugitiveRP_ModalPopupExtender" runat="server" BackgroundCssClass="modalProgressGreyBackground"
                CancelControlID="btnCancelFugitiveRP" DynamicServicePath="" Enabled="True" PopupControlID="pnlPopUpAddFugitiveRP"
                TargetControlID="btnAddFugitiveRP">
            </act:ModalPopupExtender>
            &nbsp;
            <asp:Button ID="btnDuplicate" runat="server" Text="Duplicate"
                ToolTip="Create a duplicate of this fugitive release point." Font-Size="Small"
                CausesValidation="False" Style="height: 21px" />
            <act:ModalPopupExtender ID="btnDuplicate_ModalPopupExtender" runat="server"
                DynamicServicePath="" Enabled="True" PopupControlID="pnlDuplicate" TargetControlID="btnDuplicate">
            </act:ModalPopupExtender>
            &nbsp;
            <asp:Button ID="btnEdit" runat="server" Text="Edit" ToolTip="" Font-Size="Small"
                CausesValidation="False" />
        </div>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblReleasePointID" class="styled" runat="server" Text="Fugitive ID:"></asp:Label>
        <asp:TextBox ID="txtReleasePointID" class="readonly" runat="server" Text="" ReadOnly="True"
            Width="100px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblRPDescription" class="styled" runat="server" Text="Fugitive Description:"></asp:Label>
        <asp:TextBox ID="txtRPDescription" class="readonly" runat="server" Text="" ReadOnly="True"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblRPStatusCode" class="styled" runat="server" Text="Fugitive Operating Status:"></asp:Label>
        <asp:TextBox ID="txtRPStatusCode" class="readonly" runat="server" Text="" ReadOnly="True"
            Width="250px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblRPFenceLineDistanceMeasure" class="styled" runat="server" Text="Fence Line Distance (ft):"></asp:Label>
        <asp:TextBox ID="txtRPFenceLineDistanceMeasure" class="readonly" runat="server" Text=""
            ReadOnly="True" Width="100px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblRPFugitiveHeightMeasure" class="styled" runat="server" Text="Fugitive Height (ft):"></asp:Label>
        <asp:TextBox ID="txtRPFugitiveHeightMeasure" class="readonly" runat="server" Text=""
            ReadOnly="True" Width="100px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblRPFugitiveWidthMeasure" class="styled" runat="server" Text="Fugitive Width (ft):"></asp:Label>
        <asp:TextBox ID="txtRPFugitiveWidthMeasure" class="readonly" runat="server" Text=""
            ReadOnly="True" Width="100px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblRPFugitiveLengthMeasure" class="styled" runat="server" Text="Fugitive Length (ft):"></asp:Label>
        <asp:TextBox ID="txtRPFugitiveLengthMeasure" class="readonly" runat="server" Text=""
            ReadOnly="True" Width="100px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblRPFugitiveAngleMeasure" class="styled" runat="server" Text="Fugitive Angle (0 - 179):"></asp:Label>
        <asp:TextBox ID="txtRPFugitiveAngleMeasure" class="readonly" runat="server" Text=""
            ReadOnly="True" Width="100px" MaxLength="2"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblRPComment" class="styled" runat="server" Text="Comment:"></asp:Label>
        <asp:TextBox ID="txtRPComment" class="readonly" runat="server" ReadOnly="True" TextMode="MultiLine" Rows="4" 
            Text="" Width="400px"></asp:TextBox>
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
        <asp:Label ID="Label3" class="styledseparator" runat="server" Text="Fugitive Geographic Coordinate Information:"></asp:Label>
        <asp:Label ID="lblNoRPGeoCoordInfo" runat="server" Text="" ForeColor="Red" Font-Bold="True"></asp:Label>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="LblLatitudeMeasure" class="styled" runat="server" Text="Latitude:"></asp:Label>
        <asp:TextBox ID="TxtLatitudeMeasure" class="readonly" runat="server" Text="" ReadOnly="True"
            Width="150px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="LblLongitudeMeasure" class="styled" runat="server" Text="Longitude:"></asp:Label>
        <asp:TextBox ID="TxtLongitudeMeasure" class="readonly" runat="server" Text="" ReadOnly="True"
            Width="150px"></asp:TextBox>
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
        <asp:TextBox ID="TxtHorCollectionMetCode" class="readonly" runat="server" Text="" Rows="4" 
            ReadOnly="True" Font-Names="Verdana" Font-Size="Small" TextMode="MultiLine" Width="400px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="LblHorizontalAccuracyMeasure" class="styled" runat="server" Text="Accuracy Measure (meters):"></asp:Label>
        <asp:TextBox ID="TxtHorizontalAccuracyMeasure" class="readonly" runat="server" Text=""
            ReadOnly="True"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="LblHorReferenceDatCode" class="styled" runat="server" Text="Horizontal Reference Datum:"></asp:Label>
        <asp:TextBox ID="TxtHorReferenceDatCode" class="readonly" runat="server" Text=""
            ReadOnly="True"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="LblGeographicComment" class="styled" runat="server" Text="Comment:"></asp:Label>
        <asp:TextBox ID="TxtGeographicComment" class="readonly" runat="server" ReadOnly="True" Rows="4" 
            TextMode="MultiLine" Text="" Width="400px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblLastEISSubmit_FGC" class="styled" runat="server" Text="Last Submitted to EPA:"></asp:Label>
        <asp:TextBox ID="txtLastEISSubmit_FGC" class="readonly" runat="server" Text="" ReadOnly="True"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblLastUpdate_FGC" class="styled" runat="server" Text="Last Updated On:"></asp:Label>
        <asp:TextBox ID="txtLastUpdate_FGC" class="readonly" runat="server" Text="" ReadOnly="True"></asp:TextBox>
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
    <asp:Panel ID="pnlPopUpAddFugitiveRP" BackColor="#ffffff" runat="server" BorderColor="#333399"
        BorderStyle="Ridge" ScrollBars="Auto" Width="450px" Style="display: none;">
        <table border="0" cellpadding="2" cellspacing="1" width="100%">
            <tr>
                <td align="center" colspan="2">
                    <strong><span style="color: #4169e1; font-size: 12pt; font-family: Verdana;">Add Fugitive
                        Release Point </span></strong>
                </td>
            </tr>
            <tr>
                <td align="right" width="30%"></td>
                <td>&nbsp;
                </td>
            </tr>
            <tr>
                <td align="right" width="30%" valign="top">Fugitive ID:
                </td>
                <td valign="top">
                    <asp:TextBox ID="txtNewFugitiveRP" runat="server" Font-Names="Arial" Font-Size="Small"
                        MaxLength="6" ValidationGroup="vgFugitiveSave" Width="100px"></asp:TextBox>
                    <act:FilteredTextBoxExtender ID="filtxtNewFugitiveRP" runat="server" Enabled="True"
                        FilterType="Numbers, UppercaseLetters, LowercaseLetters" TargetControlID="txtNewFugitiveRP">
                    </act:FilteredTextBoxExtender>
                </td>
            </tr>
            <tr>
                <td align="right" valign="top" width="30%">&nbsp;
                </td>
                <td valign="top">
                    <asp:CustomValidator ID="cusvFugitiveID" runat="server" ControlToValidate="txtNewFugitiveRP"
                        OnServerValidate="FugitiveRPIDCheck" ErrorMessage="* Release Point ID already in use. Enter another. *"
                        Font-Names="Arial" Font-Size="Small" CssClass="validator" Display="Dynamic" Width="100%"
                        ValidationGroup="vgFugitiveSave"></asp:CustomValidator>
                    <asp:RequiredFieldValidator ID="reqvNewFugitiveRP" ControlToValidate="txtNewFugitiveRP"
                        runat="server" CssClass="validator" ErrorMessage="* Release Point ID is required."
                        Width="100%" ValidationGroup="vgFugitiveSave">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="right" width="30%" valign="top">Description:
                </td>
                <td valign="top">
                    <asp:TextBox ID="txtNewFugitiveRPDesc" runat="server" Font-Names="Arial" Font-Size="Small"
                        MaxLength="100" Width="400px" ValidationGroup="vgFugitiveSave"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="reqvNewFugitiveRPDesc" ControlToValidate="txtNewFugitiveRPDesc"
                        runat="server" ErrorMessage="" ValidationGroup="vgFugitiveSave">*</asp:RequiredFieldValidator>
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
                    <asp:Button ID="btnInsertFugitiveID" runat="server" Text="Save" Width="90px" ValidationGroup="vgFugitiveSave" />
                    &nbsp;&nbsp;
                    <asp:Button ID="btnCancelFugitiveRP" runat="server" Text="Cancel" CausesValidation="False" />
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
                    <strong><span style="color: #4169e1; font-size: 12pt; font-family: Verdana;">Duplicate Fugitive Release Point</span></strong></td>
            </tr>
            <tr>
                <td align="center" colspan="2" style="font-family: Verdana; font-size: small;">All fugitive release point data will be duplicated except for geographic coordinate data and comments. After being taken to the Fugitive Edit page the geographic coordinates data
                    must be entered and saved.</td>
            </tr>
            <tr>
                <td align="right" width="210px"></td>
                <td>&nbsp;
                </td>
            </tr>
            <tr>
                <td align="right" valign="top" width="210px">&nbsp;<asp:Label ID="Label4" runat="server" CssClass="label" Text="Fugitive ID:"></asp:Label>
                </td>
                <td valign="top">
                    <asp:TextBox ID="txtDupFugitiveID" runat="server" Font-Names="Arial" Font-Size="Small"
                        MaxLength="6" CssClass="editable" Width="100px"></asp:TextBox>
                    <act:FilteredTextBoxExtender ID="txtDupFugitiveID_FilteredTextBoxExtender"
                        runat="server" Enabled="True" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters"
                        TargetControlID="txtDupFugitiveID" ValidChars="-">
                    </act:FilteredTextBoxExtender>
                </td>
            </tr>
            <tr>
                <td align="right" valign="top" width="210px">&nbsp;
                </td>
                <td valign="top">
                    <asp:CustomValidator ID="cusvDuplicate" runat="server" ControlToValidate="txtDupFugitiveID"
                        OnServerValidate="FugitiveDupIDCheck" ErrorMessage="* Release Point ID already used. Enter another."
                        Font-Names="Arial" Font-Size="Small" CssClass="validator" Display="Dynamic" Width="100%"
                        ValidationGroup="DupFugitiveRP"></asp:CustomValidator>
                    <asp:RequiredFieldValidator ID="reqvDupEUID" runat="server" ControlToValidate="txtDupFugitiveID"
                        CssClass="validator" ErrorMessage="* Release Point ID is required." Width="100%"
                        ValidationGroup="DupFugitiveRP"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="right" valign="top" width="210px">&nbsp;<asp:Label ID="Label6" runat="server" CssClass="label" Text="Description:"></asp:Label>
                </td>
                <td valign="top">
                    <asp:TextBox ID="txtDupFugitiveDescription" runat="server" Font-Names="Arial" Font-Size="Small"
                        MaxLength="400" Width="400px" CssClass="editable"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right" valign="top" width="210px">&nbsp;
                </td>
                <td valign="top">
                    <asp:RequiredFieldValidator ID="reqvDupFugitiveDesc" runat="server" ControlToValidate="txtDupFugitiveDescription"
                        CssClass="validator" ErrorMessage="* Description is required."
                        Width="100%" ValidationGroup="DupFugitiveRP"></asp:RequiredFieldValidator>
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
                    <asp:Button ID="btnDupInsertFugitiveRP" runat="server" Text="Save" Width="90px" ValidationGroup="DupFugitiveRP" />
                    &nbsp;&nbsp;
                    <asp:Button ID="btnCancelDupFugitiveRP" runat="server" Text="Cancel" CausesValidation="False" />
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">&nbsp;&nbsp;
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:HiddenField ID="hiddenFacilityLatitude" runat="server" Visible="false" />
    <asp:HiddenField ID="hiddenFacilityLongitude" runat="server" Visible="false" />
</asp:Content>