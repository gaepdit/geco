<%@ Page Title="Fugitive Edit - GECO Facility Inventory" Language="VB" MasterPageFile="eismaster.master"
    AutoEventWireup="false" Inherits="GECO.eis_fugitive_edit"
    MaintainScrollPositionOnPostback="true" Codebehind="fugitive_edit.aspx.vb" %>

<%@ Register Assembly="Reimers.Google.Map" Namespace="Reimers.Google.Map" TagPrefix="Reimers" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
    <script type="text/javascript">
        function Count(text, maxlength, displayEl) {
            var object = document.getElementById(text.id)  //get your object
            var div = document.getElementById(displayEl);
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
    <acs:ModalUpdateProgress ID="ModalUpdateProgress1" runat="server" DisplayAfter="0"
        BackgroundCssClass="modalProgressGreyBackground">
        <ProgressTemplate>
            <div class="modalPopup">
                &nbsp;<img src="<%= Page.ResolveUrl("~/assets/images/progressbar_green.gif") %>" alt="" align="middle" /><br />
                <span style="font-family: Verdana; font-size: small; font-weight: normal">Please Wait...</span>
            </div>
        </ProgressTemplate>
    </acs:ModalUpdateProgress>
    <div class="pageheader">
        Edit Fugitive Release Point Details
        <asp:TextBox ID="txtFugitiveStatusCodeOnLoad" runat="server" ReadOnly="True" Width="50px"
            Visible="False"></asp:TextBox>
        <asp:TextBox ID="txtFugitiveStatusCodeChanged" runat="server" ReadOnly="True" Width="50px"
            Visible="False"></asp:TextBox>
        <asp:Button ID="btnReturnToSummary" runat="server" Text="Summary" CausesValidation="False"
            CssClass="summarybutton" UseSubmitBehavior="False" />
    </div>
    <div class="fieldwrapperseparator">
        <asp:Label ID="lblSeparator1" class="styledseparator" runat="server" Text="Fugitive Release Point"></asp:Label>
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                <div class="sepbuttons">
                    <asp:Label ID="lblFugitiveMessage" runat="server" Font-Bold="True" Font-Names="Arial"
                        Font-Size="Small" ForeColor="Red"></asp:Label>&nbsp;
                    <asp:Button ID="btnSaveFugitive1" runat="server" Text="Save" ToolTip="" ValidationGroup="vgFugitive"
                        Font-Size="Small" UseSubmitBehavior="False" />&nbsp;
                    <asp:Button ID="btnCancel" runat="server" Text="Return to Details" ToolTip="" Font-Size="Small"
                        CausesValidation="False" UseSubmitBehavior="False" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <asp:ValidationSummary ID="sumvFugitive" runat="server" ValidationGroup="vgFugitive"
        HeaderText="You received the following errors:"></asp:ValidationSummary>
    <div class="fieldwrapper">
        <asp:Label ID="lblReleasePointID" class="styled" runat="server" Text="Fugitive ID:"></asp:Label>
        <asp:TextBox ID="txtReleasePointID" runat="server" ReadOnly="true" class="readonly" MaxLength="6"
            Text="" Width="100px"></asp:TextBox>
    </div>
    <%--Rquired Fugitive Fields--%>
    <div class="fieldwrapper">
        <asp:Label ID="lblRPDescription" class="styled" runat="server" Text="Fugitive Description:"></asp:Label>
        <asp:TextBox ID="txtRPDescription" runat="server" class="editable" Text=""
            Width="350px" MaxLength="100"></asp:TextBox>
        <asp:RequiredFieldValidator ID="reqvRPDescription" ControlToValidate="txtRPDescription"
            runat="server" ValidationGroup="vgFugitive" ErrorMessage="The fugitive description is required.">*</asp:RequiredFieldValidator>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblddlFugitiveStatusCode" CssClass="styled" runat="server" Text="Fugitive Operating Status:"></asp:Label>
        <asp:DropDownList ID="ddlFugitiveStatusCode" runat="server" class="" AutoPostBack="True">
        </asp:DropDownList>
        <asp:RequiredFieldValidator ID="reqvFugitiveStatusCode" ControlToValidate="ddlFugitiveStatusCode"
            InitialValue="--Select Operating Status--" runat="server" ValidationGroup="vgFugitive"
            ErrorMessage="The operating status is required.">*</asp:RequiredFieldValidator>&nbsp;
        <asp:Label ID="lblRPShutdownMessage" runat="server" Font-Size="Small" ForeColor="#CA0000"></asp:Label>
    </div>
    <%--Begin Optional Fugitive Fields--%>
    <div class="fieldwrapper">
        <asp:Label ID="lblRPFenceLineDistanceMeasure" class="styled" runat="server" Text="Fence Line Distance (ft):"></asp:Label>
        <asp:TextBox ID="txtRPFenceLineDistanceMeasure" runat="server" class="editable" Text=""
            Width="100px" ToolTip="The measure of the horizontal distance to the nearest fence line of a property within which the release point is located."
            MaxLength="5"></asp:TextBox>
        <act:FilteredTextBoxExtender ID="filtxtRPFenceLineDistanceMeasure" runat="server"
            Enabled="True" TargetControlID="txtRPFenceLineDistanceMeasure" FilterType="Numbers">
        </act:FilteredTextBoxExtender>
        <act:TextBoxWatermarkExtender ID="tbwRPFenceLineDistanceMeasure" runat="server" Enabled="True"
            TargetControlID="txtRPFenceLineDistanceMeasure" WatermarkCssClass="watermarked"
            WatermarkText="OPTIONAL">
        </act:TextBoxWatermarkExtender>
        <asp:RangeValidator ID="rngvRPFenceLineDistanceMeasure" runat="server" ControlToValidate="txtRPFenceLineDistanceMeasure"
            MinimumValue="0" MaximumValue="99999" Type="Integer" ValidationGroup="vgFugitive"
            ErrorMessage="The fence line distance is outside the expected range of 0 to 99,999 feet.">Must be between 0 and 99,999 ft </asp:RangeValidator>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblRPFugitiveHeightMeasure" class="styled" runat="server" Text="Fugitive Height Measure (ft):"></asp:Label>
        <asp:TextBox ID="txtRPFugitiveHeightMeasure" runat="server" class="editable" Text=""
            Width="100px" ToolTip="The release height above the terrain of the fugitive emissions."
            MaxLength="3"></asp:TextBox>
        <act:FilteredTextBoxExtender ID="filtxtRPFugitiveHeightMeasure" runat="server" Enabled="True"
            TargetControlID="txtRPFugitiveHeightMeasure" FilterType="Numbers">
        </act:FilteredTextBoxExtender>
        <act:TextBoxWatermarkExtender ID="tbwRPFugitiveHeightMeasure" runat="server" Enabled="True"
            TargetControlID="txtRPFugitiveHeightMeasure" WatermarkCssClass="watermarked"
            WatermarkText="OPTIONAL">
        </act:TextBoxWatermarkExtender>
        <asp:RangeValidator ID="rngvRPFugitiveHeightMeasure" runat="server" ControlToValidate="txtRPFugitiveHeightMeasure"
            MinimumValue="0" MaximumValue="500" Type="Integer" ValidationGroup="vgFugitive"
            ErrorMessage="The release height of the fugitive emissions must be between 0 and 500 feet.">Must be between 0 and 500 ft</asp:RangeValidator>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblRPFugitiveWidthMeasure" class="styled" runat="server" Text="Fugitive Width Measure (ft):"></asp:Label>
        <asp:TextBox ID="txtRPFugitiveWidthMeasure" runat="server" class="editable" Text=""
            Width="100px" ToolTip="The width of the fugitive release in the North-South direction as if the angle is zero degrees, also known as SigmaY."
            MaxLength="5"></asp:TextBox>
        <act:FilteredTextBoxExtender ID="filtxtRPFugitiveWidthMeasure" runat="server" Enabled="True"
            TargetControlID="txtRPFugitiveWidthMeasure" FilterType="Numbers">
        </act:FilteredTextBoxExtender>
        <act:TextBoxWatermarkExtender ID="tbwRPFugitiveWidthMeasure" runat="server" Enabled="True"
            TargetControlID="txtRPFugitiveWidthMeasure" WatermarkCssClass="watermarked" WatermarkText="OPTIONAL">
        </act:TextBoxWatermarkExtender>
        <asp:RangeValidator ID="rngvRPFugitiveWidthMeasure" runat="server" ControlToValidate="txtRPFugitiveWidthMeasure"
            MinimumValue="1" MaximumValue="10000" Type="Integer" ValidationGroup="vgFugitive"
            ErrorMessage="The release width of the fugitive emissions must be between 1 and 10,000 feet.">Must be between 1 and 10,000 ft</asp:RangeValidator>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblRPFugitiveLengthMeasure" class="styled" runat="server" Text="Fugitive Length Measure (ft):"></asp:Label>
        <asp:TextBox ID="txtRPFugitiveLengthMeasure" runat="server" class="editable" Text=""
            Width="100px" ToolTip="The length of the fugitive release in the East-West direction as if the angle is zero degrees, also known as SigmaX."
            MaxLength="5"></asp:TextBox>
        <act:FilteredTextBoxExtender ID="filtxtRPFugitiveLengthMeasure" runat="server" Enabled="True"
            TargetControlID="txtRPFugitiveLengthMeasure" FilterType="Numbers">
        </act:FilteredTextBoxExtender>
        <act:TextBoxWatermarkExtender ID="tbwRPFugitiveLengthMeasure" runat="server" Enabled="True"
            TargetControlID="txtRPFugitiveLengthMeasure" WatermarkCssClass="watermarked"
            WatermarkText="OPTIONAL">
        </act:TextBoxWatermarkExtender>
        <asp:RangeValidator ID="rngvRPFugitiveLengthMeasure" runat="server" ControlToValidate="txtRPFugitiveLengthMeasure"
            MinimumValue="1" MaximumValue="10000" Type="Integer" ValidationGroup="vgFugitive"
            ErrorMessage="The release length of the fugitive emissions must be between 1 and 10,000 feet.">Must be between 1 and 10,000 ft</asp:RangeValidator>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblRPFugitiveAngleMeasure" class="styled" runat="server"
            Text="Fugitive Angle Measure (°):"></asp:Label>
        <asp:TextBox ID="txtRPFugitiveAngleMeasure" runat="server" class="editable" Text=""
            Width="100px" ToolTip="The orientation of the y-dimension (North-South) of the area in degrees from true North, measured positive in the clockwise direction."
            MaxLength="3"></asp:TextBox>
        <act:FilteredTextBoxExtender ID="filtxtRPFugitiveAngleMeasure" runat="server" Enabled="True"
            TargetControlID="txtRPFugitiveAngleMeasure" FilterType="Numbers">
        </act:FilteredTextBoxExtender>
        <act:TextBoxWatermarkExtender ID="tbwRPFugitiveAngleMeasure" runat="server" Enabled="True"
            TargetControlID="txtRPFugitiveAngleMeasure" WatermarkCssClass="watermarked" WatermarkText="OPTIONAL">
        </act:TextBoxWatermarkExtender>
        <asp:RangeValidator ID="rngvRPFugitiveAngleMeasure" runat="server" ControlToValidate="txtRPFugitiveAngleMeasure"
            MinimumValue="0" MaximumValue="89" Type="Integer" ValidationGroup="vgFugitive"
            ErrorMessage="The fugitive angle of measure must be between 0° and 89° inclusive.">Must be between 0° and 89°</asp:RangeValidator>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblRPComment" class="styled" runat="server" Text="Comment:"></asp:Label>
        <div style="display: inline-block">
            <asp:TextBox ID="txtRPComment" runat="server" class="editable" TextMode="MultiLine" Rows="4" onKeyUp="javascript:Count(this,400,'dvComment');"
                Text="" Width="400px"></asp:TextBox>
            <div id="dvComment" style="font: bold"></div>
            <asp:RegularExpressionValidator ID="regexpName" runat="server"
                ErrorMessage="Comment not to exceed 400 characters."
                ControlToValidate="txtRPComment"
                ValidationExpression="^[\s\S]{0,400}$" ValidationGroup="vgFugitive" />
        </div>
    </div>
    <div class="fieldwrapperseparator">
        <asp:Label ID="lblSeparator" class="styledseparator" runat="server" Text="Release Point Apportionments"></asp:Label>
        <asp:Label ID="lblReleasePointAppMessage" runat="server" Font-Size="Small" ForeColor="#CA0000"
            CssClass="labelwarningleft"></asp:Label><br />
    </div>
    <div class="gridview">
        <asp:GridView ID="gvwRPApportionment" runat="server" DataSourceID="SqlDataSourceRPApp"
            CellPadding="4" Font-Names="Arial" Font-Size="Small" ForeColor="#333333" GridLines="None"
            AutoGenerateColumns="False">
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#EFF3FB" />
            <Columns>
                <asp:BoundField DataField="EmissionsUnitID" HeaderText="Emission Unit ID" />
                <asp:HyperLinkField DataNavigateUrlFields="ProcessID,EmissionsUnitID" DataNavigateUrlFormatString="~/eis/rpapportionment_view.aspx?ep={0}&amp;eu={1}"
                    DataTextField="ProcessID" HeaderText="Process ID" NavigateUrl="~/eis/rpapportionment_view.aspx" />
                <asp:HyperLinkField DataNavigateUrlFields="ProcessID,EmissionsUnitID" DataNavigateUrlFormatString="~/eis/rpapportionment_view.aspx?ep={0}&amp;eu={1}"
                    DataTextField="strprocessdescription" HeaderText="Process Description" NavigateUrl="~/eis/rpapportionment_view.aspx">
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
    <%-- Stack Geographic Coordinate Information --%>
    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
        <ContentTemplate>
            <div class="fieldwrapperseparator">
                <asp:Label ID="lblSeparator2" class="styledseparator" runat="server" Text="Stack Geographic Coordinate Information:"></asp:Label>
                <asp:Label ID="lblFugitiveGCDataMissing" runat="server" Text="" ForeColor="Red" Font-Bold="True"></asp:Label>
            </div>
            <asp:UpdatePanel ID="upLatLon" runat="server">
                <ContentTemplate>
                    <div class="fieldwrapper">
                        <asp:Label ID="LblLatitudeMeasure" class="styled" runat="server" Text="Latitude:"></asp:Label>
                        <asp:TextBox ID="TxtLatitudeMeasure" runat="server" class="editable" Text="" Width="150px"
                            MaxLength="8"></asp:TextBox>
                        <act:FilteredTextBoxExtender ID="filtxtLatitudeMeasure" runat="server" Enabled="True"
                            FilterType="Custom, Numbers" TargetControlID="TxtLatitudeMeasure" ValidChars=".">
                        </act:FilteredTextBoxExtender>
                        <asp:RequiredFieldValidator ID="reqvLatitudeMeasure" ControlToValidate="TxtLatitudeMeasure"
                            runat="server" ValidationGroup="vgFugitive" ErrorMessage="The release point latitude is required."
                            Display="Dynamic">*</asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="rngvLatitudeMeasure" runat="server" ControlToValidate="TxtLatitudeMeasure"
                            ValidationGroup="vgFugitive" MaximumValue="35.00028" MinimumValue="30.35944" Type="Double"
                            ErrorMessage="Latitude must be between 35.200028° and 30.35944°"
                            Display="Dynamic">Must be between 35.200028° and 30.35944°</asp:RangeValidator>
                    </div>
                    <div class="fieldwrapper">
                        <asp:Label ID="LblLongitudeMeasure" class="styled" runat="server" Text="Longitude:"></asp:Label>
                        <asp:TextBox ID="TxtLongitudeMeasure" runat="server" class="editable" Text="" Width="150px"
                            MaxLength="9"></asp:TextBox>
                        <act:FilteredTextBoxExtender ID="filtxtLongitudeMeasure" runat="server" Enabled="True"
                            FilterType="Custom, Numbers" TargetControlID="TxtLongitudeMeasure" ValidChars=".-">
                        </act:FilteredTextBoxExtender>
                        <asp:RequiredFieldValidator ID="reqvLongitudeMeasure" ControlToValidate="TxtLongitudeMeasure"
                            runat="server" ValidationGroup="vgFugitive" ErrorMessage="The release point longitude is required."
                            Display="Dynamic">*</asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="rngvLongitudeMeasure" runat="server" ControlToValidate="TxtLongitudeMeasure"
                            ValidationGroup="vgFugitive" MinimumValue="-85.60889" MaximumValue="-80.84417" Type="Double"
                            ErrorMessage="Longitude must be between -85.60889° and -80.84417°."
                            Display="Dynamic">Must be between -85.60889° and -80.84417°</asp:RangeValidator>
                    </div>
                    <div class="fieldwrapper">
                        <asp:Label class="styled" runat="server" Text="Map:"></asp:Label>
                        <div style="display: inline-block; width: 610px">
                            <asp:HyperLink ID="lnkGoogleMap" runat="server" Text="View in Google Maps" Target="_blank">
                                <asp:Image ID="imgGoogleStaticMap" runat="server" />
                            </asp:HyperLink><br />
                            <asp:Panel ID="pnlLocationMap" runat="server" Width="610px"></asp:Panel>
                            <p>
                                The release point latitude/longitude is centered in the map above. If the location is incorrect, 
                                use the &quot;Pick Latitude/Longitude&quot; button to select the correct location.
                            </p>
                            <asp:Button ID="lbtnGetLatLon" runat="server" CausesValidation="false" Text="Pick Latitude/Longitude" />
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="lbtnGetLatLon" />
                </Triggers>
            </asp:UpdatePanel>
            <div class="fieldwrapper">
                <asp:Label ID="LblHorCollectionMetCode" CssClass="styled" runat="server" Text="Horizontal Collection Method:"></asp:Label>
                <asp:DropDownList ID="ddlHorCollectionMetCode" runat="server" Width="600px"></asp:DropDownList>
                <asp:RequiredFieldValidator ID="reqvHorCollectionMetCode" ControlToValidate="ddlHorCollectionMetCode"
                    runat="server" ValidationGroup="vgFugitive" ErrorMessage="The Horizontal Collection Method Code is required."
                    InitialValue="--Select Horizontal Collection Method--" Display="Dynamic">*</asp:RequiredFieldValidator>
            </div>
            <div class="fieldwrapper">
                <asp:Label ID="LblHorizontalAccuracyMeasure" class="styled" runat="server" Text="Accuracy Measure (m):"></asp:Label>
                <asp:TextBox ID="TxtHorizontalAccuracyMeasure" runat="server" class="editable" Text=""
                    Width="100px" ToolTip="The horizontal measure of the relative accuracy of teh latitude and longitude coordinates."
                    MaxLength="4"></asp:TextBox>
                <act:FilteredTextBoxExtender ID="filtxtHorizontalAccuracyMeasure" runat="server"
                    Enabled="True" FilterType="Numbers" TargetControlID="TxtHorizontalAccuracyMeasure">
                </act:FilteredTextBoxExtender>
                <asp:RequiredFieldValidator ID="reqvHorizontalAccuracyMeasure" ControlToValidate="TxtHorizontalAccuracyMeasure"
                    runat="server" ValidationGroup="vgFugitive" ErrorMessage="The Accuracy Measure is required."
                    Display="Dynamic">*</asp:RequiredFieldValidator>
                <asp:RangeValidator ID="rngvHorizontalAccuracyMeasure" runat="server" ControlToValidate="TxtHorizontalAccuracyMeasure"
                    ErrorMessage="The Accuracy Measure must be between 1 and 2000 meters." MaximumValue="2000"
                    MinimumValue="1" ValidationGroup="vgFugitive" Display="Dynamic"
                    Type="Integer">Must be between 1 and 2000</asp:RangeValidator>
            </div>
            <div class="fieldwrapper">
                <asp:Label ID="lblHorReferenceDatCode" CssClass="styled" runat="server" Text="Horizontal Reference Datum:"></asp:Label>
                <asp:DropDownList ID="ddlHorReferenceDatCode" runat="server" ToolTip="The reference datum used in determining the latitude and longitude.  The most common is the &quot;North American datum of 1983&quot;.">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="reqvHorReferenceDatCode" ControlToValidate="ddlHorReferenceDatCode"
                    runat="server" ValidationGroup="vgFugitive" ErrorMessage="The Horizontal Reference Datum Code is required."
                    InitialValue="--Select Horizontal Reference Datum--" Display="Dynamic">*</asp:RequiredFieldValidator>
            </div>
            <div class="fieldwrapper">
                <asp:Label ID="LblGeographicComment" class="styled" runat="server" Text="Comment:"></asp:Label>
                <div style="display: inline-block">
                    <asp:TextBox ID="TxtGeographicComment" runat="server" class="editable" TextMode="MultiLine" Rows="4" onKeyUp="javascript:Count(this,400,'dvComment2');"
                        Text="" Width="400px"></asp:TextBox>
                    <div id="dvComment2" style="font: bold"></div>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                        ErrorMessage="Comment must not exceed 400 characters."
                        ControlToValidate="TxtGeographicComment"
                        ValidationExpression="^[\s\S]{0,400}$" />
                </div>
            </div>
            <asp:HiddenField ID="hidLatitude" runat="server" Visible="false" />
            <asp:HiddenField ID="hidLongitude" runat="server" Visible="false" />
            <asp:HiddenField ID="hidHorCollectionMetCode" runat="server" Visible="false" />
            <asp:HiddenField ID="hidHorCollectionMetDesc" runat="server" Visible="false" />
            <asp:HiddenField ID="hidHorizontalAccuracyMeasure" runat="server" Visible="false" />
            <asp:HiddenField ID="hidHorReferenceDatCode" runat="server" Visible="false" />
            <asp:HiddenField ID="hidHorReferenceDatDesc" runat="server" Visible="false" />
            <asp:HiddenField ID="hidGeographicComment" runat="server" Visible="false" />

            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="buttonwrapper">
                        <asp:Button ID="btnCancel3" runat="server" Text="Return to Details" ToolTip="" Font-Size="Small"
                            CausesValidation="False" UseSubmitBehavior="False" Width="120px" />&nbsp;
                        <asp:Button ID="btnReturnSummary2" runat="server" Text="Summary" ToolTip="" Font-Size="Small"
                            CausesValidation="False" UseSubmitBehavior="False" />&nbsp;
                        <asp:Button ID="btnSaveFugitive2" runat="server" Text="Save" Width="60px" ValidationGroup="vgFugitive" />&nbsp;
                        <asp:Button ID="btnDelete" runat="server" Text="Delete" ToolTip="" Font-Size="Small"
                            Width="60px" />
                        <act:ModalPopupExtender ID="mpeDelete" runat="server" BackgroundCssClass="modalProgressGreyBackground"
                            Enabled="True" TargetControlID="btnDelete" PopupControlID="pnlDeleteFugitive">
                        </act:ModalPopupExtender>
                        &nbsp;<asp:Label ID="lblFugitiveMessage2" runat="server" Font-Bold="True" Font-Names="Arial"
                            Font-Size="Small" ForeColor="Red"></asp:Label>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:Panel ID="pnlDeleteFugitive" BackColor="#ffffff" runat="server" BorderColor="#333399"
                BorderStyle="Ridge" ScrollBars="Auto" Width="450px" Style="display: none;">
                <table border="0" cellpadding="2" cellspacing="1" width="450px">
                    <tr>
                        <td align="center">
                            <strong><span style="color: #4169e1; font-size: 12pt; font-family: Verdana;">Fugitive
                        Deleted</span></strong>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblDeleteFugitive" runat="server" Text="Are you sure you want to delete the fugitive release point?"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnDeleteOK" runat="server" Text="Delete" Width="60px" />
                            <asp:Button ID="btnDeleteCancel" runat="server" Text="No" Width="60px" />
                            <asp:Button ID="btnDeleteSummary" runat="server" Text="Summary" Width="80px" PostBackUrl="~/eis/releasepoint_summary.aspx" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">&nbsp;&nbsp;
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Panel ID="pnlMap" runat="server" Style="display: inherit" BackColor="#507CD1">
        <asp:Button ID="btnPopupDisplay" runat="server" Style="display: none;" />
        <Reimers:Map ID="GMap" Width="700" Height="400" runat="server"></Reimers:Map>
        <br />
        <table cellpadding="3" cellspacing="3">
            <tr>
                <td>
                    <asp:Label ID="lblMapLat" runat="server" ForeColor="White" Text="Latitude: "></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtMapLat" runat="server" ValidationGroup="LatLon"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvMapLat" runat="server" ErrorMessage="*" ControlToValidate="txtMapLat"
                        ValidationGroup="LatLon"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblMapLon" runat="server" ForeColor="White" Text="Longitude: "></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtMapLon" runat="server" ValidationGroup="LatLon"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvMapLon" runat="server" ErrorMessage="*" ControlToValidate="txtMapLon"
                        ValidationGroup="LatLon"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btnCloseMap" runat="server" Text="Close" CausesValidation="false" />
                </td>
                <td>
                    <asp:Button ID="btnUseLatLon" runat="server" Text="Use these values" ValidationGroup="LatLon" />
                </td>
            </tr>
        </table>
        <act:ModalPopupExtender ID="lbtnGetLatLon_ModalPopupExtender" runat="server" TargetControlID="btnPopupDisplay"
            BackgroundCssClass="modalProgressGreyBackground" PopupControlID="pnlMap" CancelControlID="btnCloseMap"
            RepositionMode="RepositionOnWindowResizeAndScroll" Y="20">
        </act:ModalPopupExtender>
    </asp:Panel>
</asp:Content>
