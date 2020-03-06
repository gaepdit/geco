Imports System.Data.SqlClient
Imports GECO.MapHelper

Partial Class eis_fugitive_details
    Inherits Page

    Private RPStatusCode As String

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        Dim FugitiveID As String = Request.QueryString("fug")
        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim EISAccessCode As String = GetCookie(EisCookie.EISAccess)

        FIAccessCheck(EISAccessCode)

        If Not ReleasePointExists(FacilitySiteID, FugitiveID) Then
            Throw New HttpException(404, "Not found")
        End If

        If Not IsPostBack Then
            LoadFugitiveDetails(FugitiveID)
            LoadRPApportionment(FacilitySiteID, FugitiveID)
        End If

        HideTextBoxBorders(Me)
    End Sub

    Private Sub LoadFugitiveDetails(RPid As String)

        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim UpdateUser As String
        Dim UpdateDateTime As String
        Dim HCCcode As String
        Dim HCCdesc As String
        Dim HRCcode As String
        Dim HRCdesc As String
        Dim RPFencelineDistanceMeasure As Decimal
        Dim RPFugitiveHeightMeasure As Decimal
        Dim RPFugitiveWidthMeasure As Decimal
        Dim RPFugitiveLengthMeasure As Decimal
        Dim RPFugitiveAngleMeasure As Decimal
        Dim HORACCURACYMEASURE As Decimal
        Dim FacilityLongitude As String
        Dim FacilityLatitude As String
        Dim RPStatusCodeDesc As String
        Dim RPStatusCodeyear As String

        Try
            Dim query As String = "select RELEASEPOINTID, " &
                "strRPDescription, " &
                        "strRPStatusCode, " &
                        "strRPTypeCode , " &
                        "NumRPStatusCodeYear, " &
                        "NUMRPFENCELINEDISTMEASURE, " &
                        "numRPFugitiveHeightMeasure , " &
                        "numRPFugitiveWidthMeasure, " &
                        "numRPFugitiveLengthMeasure, " &
                        "numRPFugitiveAngleMeasure, " &
                        "convert(char, UpdateDateTime, 20) As UpdateDateTime,  " &
                        "convert(char, LASTEISSUBMITDATE, 101) As LastEISSubmitDate, " &
                        "UpdateUser, " &
                        "strRPComment " &
                        "FROM EIS_ReleasePoint where EIS_ReleasePoint.FACILITYSITEID = @FacilitySiteID and " &
                        "RELEASEPOINTID = @RPid and ACTIVE = '1' order by RELEASEPOINTID"

            Dim params As SqlParameter() = {
                New SqlParameter("@FacilitySiteID", FacilitySiteID),
                New SqlParameter("@RPid", RPid)
            }

            Dim dr As DataRow = DB.GetDataRow(query, params)

            If dr IsNot Nothing Then

                'Facility Name and Location
                If IsDBNull(dr("RELEASEPOINTID")) Then
                    txtReleasePointID.Text = ""
                Else
                    txtReleasePointID.Text = dr.Item("RELEASEPOINTID")
                End If

                If IsDBNull(dr("strRPDescription")) Then
                    txtRPDescription.Text = ""
                Else
                    txtRPDescription.Text = dr.Item("strRPDescription")
                End If

                If IsDBNull(dr("strRPStatusCode")) Then
                    txtRPStatusCode.Text = 0
                    RPStatusCode = ""
                    RPStatusCodeDesc = ""
                Else
                    RPStatusCode = dr.Item("strRPStatusCode")
                    RPStatusCodeDesc = GetStackStatusCodeDesc(RPStatusCode)
                End If

                If IsDBNull(dr("NumRPStatusCodeYear")) Then
                    RPStatusCodeyear = ""
                Else
                    RPStatusCodeyear = dr.Item("NumRPStatusCodeYear")
                End If

                txtRPStatusCode.Text = RPStatusCodeDesc & " as reported in " & RPStatusCodeyear

                If IsDBNull(dr("NUMRPFENCELINEDISTMEASURE")) Then
                    txtRPFenceLineDistanceMeasure.Text = ""
                Else
                    RPFencelineDistanceMeasure = dr.Item("NUMRPFENCELINEDISTMEASURE")
                    If RPFencelineDistanceMeasure = -1 Then
                        txtRPFenceLineDistanceMeasure.Text = ""
                    Else
                        txtRPFenceLineDistanceMeasure.Text = RPFencelineDistanceMeasure
                    End If

                End If
                If IsDBNull(dr("numRPFugitiveHeightMeasure")) Then
                    txtRPFugitiveHeightMeasure.Text = ""
                Else
                    RPFugitiveHeightMeasure = dr.Item("numRPFugitiveHeightMeasure")
                    If RPFugitiveHeightMeasure = -1 Then
                        txtRPFugitiveHeightMeasure.Text = ""
                    Else
                        txtRPFugitiveHeightMeasure.Text = RPFugitiveHeightMeasure
                    End If
                End If

                If IsDBNull(dr("numRPFugitiveWidthMeasure")) Then
                    txtRPFugitiveWidthMeasure.Text = ""
                Else
                    RPFugitiveWidthMeasure = dr.Item("numRPFugitiveWidthMeasure")
                    If RPFugitiveWidthMeasure = -1 Then
                        txtRPFugitiveWidthMeasure.Text = ""
                    Else
                        txtRPFugitiveWidthMeasure.Text = RPFugitiveWidthMeasure
                    End If
                End If

                'Description
                If IsDBNull(dr("numRPFugitiveLengthMeasure")) Then
                    txtRPFugitiveLengthMeasure.Text = ""
                Else
                    RPFugitiveLengthMeasure = dr.Item("numRPFugitiveLengthMeasure")
                    If RPFugitiveLengthMeasure = -1 Then
                        txtRPFugitiveLengthMeasure.Text = ""
                    Else
                        txtRPFugitiveLengthMeasure.Text = RPFugitiveLengthMeasure
                    End If
                End If
                If IsDBNull(dr("numRPFugitiveAngleMeasure")) Then
                    txtRPFugitiveAngleMeasure.Text = ""
                Else
                    RPFugitiveAngleMeasure = dr.Item("numRPFugitiveAngleMeasure")
                    If RPFugitiveAngleMeasure = -1 Then
                        txtRPFugitiveAngleMeasure.Text = ""
                    Else
                        txtRPFugitiveAngleMeasure.Text = RPFugitiveAngleMeasure
                    End If
                End If
                If IsDBNull(dr("strRPComment")) Then
                    txtRPComment.Text = ""
                    txtRPComment.Visible = False
                Else
                    txtRPComment.Text = dr.Item("strRPComment")
                End If
                If IsDBNull(dr("LastEISSubmitDate")) Then
                    txtLastEISSubmit.Text = "Never submitted"
                Else
                    txtLastEISSubmit.Text = dr.Item("LastEISSubmitDate")
                End If
                If IsDBNull(dr("UpdateUser")) Then
                    UpdateUser = ""
                Else
                    UpdateUser = dr.Item("UpdateUser")
                    UpdateUser = Mid(UpdateUser, InStr(UpdateUser, "-") + 1)
                End If
                If IsDBNull(dr("UpdateDateTime")) Then
                    UpdateDateTime = ""
                Else
                    UpdateDateTime = dr.Item("UpdateDateTime")
                End If
                txtLastUpdate.Text = UpdateDateTime & " by " & UpdateUser

            End If

            'Load Fugitive geographic coordinate information
            query = "select numLatitudeMeasure, " &
                            " numLongitudeMeasure, " &
                            " STRHORCOLLMETCode, " &
                            " INTHORACCURACYMEASURE, " &
                            " STRHORREFDATUMCode, " &
                            " convert(char, UpdateDateTime, 20) As UpdateDateTime,  " &
                            " convert(char, LastEISSubmitDate, 101) As LastEISSubmitDate, " &
                            " UpdateUser, " &
                            " strGeographicComment " &
                            " FROM EIS_RPGeoCoordinates where EIS_RPGeoCoordinates.FACILITYSITEID = @FacilitySiteID and " &
                            " RELEASEPOINTID = @RPid "

            Dim dr3 As DataRow = DB.GetDataRow(query, params)

            If dr3 IsNot Nothing Then

                'G.C. info
                If IsDBNull(dr3("numLatitudeMeasure")) Then
                    TxtLatitudeMeasure.Text = ""
                Else
                    TxtLatitudeMeasure.Text = dr3.Item("numLatitudeMeasure")
                End If
                If IsDBNull(dr3("numLongitudeMeasure")) Then
                    TxtLongitudeMeasure.Text = ""
                Else
                    TxtLongitudeMeasure.Text = dr3.Item("numLongitudeMeasure")
                End If

                'Render Google map
                If TxtLatitudeMeasure.Text <> "" AndAlso TxtLongitudeMeasure.Text <> "" Then
                    FacilityLatitude = TxtLatitudeMeasure.Text
                    FacilityLongitude = TxtLongitudeMeasure.Text
                Else
                    FacilityLatitude = GetFacilityLatitude(FacilitySiteID)
                    FacilityLongitude = GetFacilityLongitude(FacilitySiteID)
                End If
                imgGoogleStaticMap.ImageUrl = GoogleMaps.GetStaticMapUrl(New Coordinate(FacilityLatitude, FacilityLongitude))

                If IsDBNull(dr3("INTHORACCURACYMEASURE")) Then
                    TxtHorizontalAccuracyMeasure.Text = ""
                Else
                    HORACCURACYMEASURE = dr3.Item("INTHORACCURACYMEASURE")
                    If HORACCURACYMEASURE = -1 Then
                        TxtHorizontalAccuracyMeasure.Text = ""
                    Else
                        TxtHorizontalAccuracyMeasure.Text = HORACCURACYMEASURE
                    End If
                End If
                If IsDBNull(dr3("STRHORCOLLMETCode")) Then
                    HCCcode = ""
                    TxtHorCollectionMetCode.Text = ""
                Else
                    HCCcode = dr3.Item("STRHORCOLLMETCode")
                    HCCdesc = GetHorCollMetDesc(HCCcode)
                    TxtHorCollectionMetCode.Text = HCCdesc
                End If
                If IsDBNull(dr3("STRHORREFDATUMCode")) Then
                    HRCcode = ""
                    TxtHorReferenceDatCode.Text = ""
                Else
                    HRCcode = dr3.Item("STRHORREFDATUMCode")
                    HRCdesc = GetHorRefDatumDesc(HRCcode)
                    TxtHorReferenceDatCode.Text = HRCdesc
                End If
                If IsDBNull(dr3("strGeographicComment")) Then
                    TxtGeographicComment.Text = ""
                    TxtGeographicComment.Visible = False
                Else
                    TxtGeographicComment.Text = dr3.Item("strGeographicComment")
                End If

                If IsDBNull(dr3("LastEISSubmitDate")) Then
                    txtLastEISSubmit_FGC.Text = "Never submitted"
                Else
                    txtLastEISSubmit_FGC.Text = dr3.Item("LastEISSubmitDate")
                End If
                If IsDBNull(dr3("UpdateUser")) Then
                    UpdateUser = "Unknown"
                Else
                    UpdateUser = dr3.Item("UpdateUser")
                    UpdateUser = Mid(UpdateUser, InStr(UpdateUser, "-") + 1)
                End If
                If IsDBNull(dr3("UpdateDateTime")) Then
                    UpdateDateTime = ""
                Else
                    UpdateDateTime = dr3.Item("UpdateDateTime")
                End If
                txtLastUpdate_FGC.Text = UpdateDateTime & " by " & UpdateUser

            End If

            If CheckRPGCData(FacilitySiteID, RPid) Then
                lblNoRPGeoCoordInfo.Text = "Release point geographic coordinate info need to be provided. Click the Edit button above."
            End If

        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Protected Sub LoadRPApportionment(fsid As String, RPid As String)
        SqlDataSourceRPApp.ConnectionString = DBConnectionString

        SqlDataSourceRPApp.SelectCommand = "select eis_process.emissionsunitid, " &
                " eis_process.processid, " &
                " eis_process.strprocessdescription, " &
                " eis_rpapportionment.releasepointid, " &
                " concat(eis_rpapportionment.intaveragepercentemissions, '%') as intaveragepercentemissions " &
                " FROM eis_rpapportionment, eis_process " &
                " where eis_process.facilitysiteid = eis_rpapportionment.facilitysiteid " &
                " and eis_process.emissionsunitid = eis_rpapportionment.emissionsunitid " &
                " and eis_process.processid = eis_rpapportionment.processid " &
                " and eis_process.facilitysiteid= @fsid " &
                " and eis_rpapportionment.releasepointid= @RPid " &
                " and EIS_RPAPPORTIONMENT.ACTIVE = '1' " &
                " and eis_process.Active = '1'"

        SqlDataSourceRPApp.SelectParameters.Add("fsid", fsid)
        SqlDataSourceRPApp.SelectParameters.Add("RPid", RPid)

        gvwRPApportionment.DataBind()

        If gvwRPApportionment.Rows.Count = 0 Then
            lblReleasePointAppMessage.Text = "The Release Point is not used in any Process Release Point Apportionments."
            lblReleasePointAppMessage.Visible = True
        End If

        If RPStatusCode <> "OP" Then
            lblRPShutdownMessage.Text = "Release point is shutdown."
            lblRPShutdownMessage.Visible = True
        Else
            lblRPShutdownMessage.Text = ""
            lblRPShutdownMessage.Visible = False
        End If
    End Sub

End Class