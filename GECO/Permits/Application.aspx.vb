Imports GECO.GecoModels
Imports GECO.DAL

Public Class Permit_Application
    Inherits Page

    Private Property AppNumber As Integer
    Private Property PermitApplication As PermitApplication
    Private Property CurrentUser As GecoUser
    Private Property FacilityAccess As FacilityAccess

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If IsPostBack Then
            If Not Integer.TryParse(hAppNumber.Value, AppNumber) Then
                Throw New HttpException(400, "Permit application number is not valid.")
            End If
        Else
            If Not Integer.TryParse(Request.QueryString("id"), AppNumber) Then
                Throw New HttpException(404, "Permit application number is not valid.")
            End If
        End If

        If Not ApplicationExists(AppNumber) Then
            Throw New HttpException(404, "Permit application not found.")
        End If

        PermitApplication = GetPermitApplication(AppNumber)

        If PermitApplication Is Nothing Then
            Throw New HttpException(404, "Permit application not found.")
        End If

        CurrentUser = GetCurrentUser()

        If PermitApplication.FacilityID IsNot Nothing AndAlso CurrentUser IsNot Nothing Then
            FacilityAccess = CurrentUser.GetFacilityAccess(PermitApplication.FacilityID)
        Else
            FacilityAccess = New FacilityAccess(PermitApplication.FacilityID, False)
        End If

        If Not IsPostBack Then
            DisplayPermitApplication()
        End If
    End Sub

    Private Sub DisplayPermitApplication()
        lblAppNum.Text = AppNumber.ToString
        hAppNumber.Value = AppNumber
        Title = "Permit Application #" & AppNumber.ToString & " - GECO"

        DisplayApplicationDetails()
        DisplayFacilityInfo()
        DisplayContact()
        DisplayTracking()
        DisplayFeeStatus()
        DisplayInvoices()
        DisplayPayments()
    End Sub

    Private Sub DisplayApplicationDetails()
        With PermitApplication
            tApplicationDetails.AddTableRow("Reason for Application", .ReasonForApplication, classList:={"preserve-linebreaks"})
            tApplicationDetails.AddTableRow("Application Type", .ApplicationType)
            tApplicationDetails.AddTableRow("Application Action", .ApplicationResult)

            If .StatusDate.HasValue Then
                tApplicationDetails.AddTableRow("Status", If(.Status, "N/A") & " as of " & .StatusDate.Value.ToString(ShortishDateFormat))
            Else
                tApplicationDetails.AddTableRow("Status", .Status)
            End If

            tApplicationDetails.AddTableRow("Permit Issued", .DatePermitIssued)

            If Not String.IsNullOrEmpty(.PermitFileName) Then
                Dim anchor As New HyperLink With {
                    .Target = "_blank",
                    .NavigateUrl = PermitApplication.PermitFileLink,
                    .Text = "Permit"
                }

                anchor.Attributes.Add("rel", "noopener")

                If Not String.IsNullOrEmpty(.PermitNumber) Then
                    anchor.Text = PermitApplication.PermitNumber
                End If

                tApplicationDetails.AddTableRow("Permit Number", anchor)
            Else
                If Not String.IsNullOrEmpty(.PermitNumber) Then
                    tApplicationDetails.AddTableRow("Permit Number", PermitApplication.PermitNumber)
                End If
            End If

            If .ApplicationWithdrawn Then
                tApplicationDetails.AddTableRow("Date Application Withdrawn", .DateApplicationWithdrawn, False)
            End If
        End With
    End Sub

    Private Sub DisplayFacilityInfo()
        With PermitApplication
            If .FacilityID Is Nothing Then
                tFacilityInfo.AddTableRow("AIRS Number", "N/A")
            Else
                tFacilityInfo.AddTableRow("AIRS Number", .FacilityID.FormattedString)
            End If

            tFacilityInfo.AddTableRow("Facility Name", .FacilityName)
            tFacilityInfo.AddTableRow("Facility Address", .FacilityAddress.ToLinearString)
            tFacilityInfo.AddTableRow("County", .FacilityCounty)
            tFacilityInfo.AddTableRow("District Office", .FacilityDistrict)
            tFacilityInfo.AddTableRow("SIC Code", .FacilitySicCode)
            tFacilityInfo.AddTableRow("NAICS Code", .FacilityNaicsCode)
            tFacilityInfo.AddTableRow("Facility Description", .FacilityDescription)
        End With
    End Sub

    Private Sub DisplayContact()
        With PermitApplication
            tContact.AddTableRow("Permitting Program Unit", .UnitResponsible)
        End With

        If PermitApplication.StaffResponsible Is Nothing Then
            pNoContact.Visible = True
            Exit Sub
        End If

        With PermitApplication.StaffResponsible
            tContact.AddTableRow("Assigned Staff", .FullName)

            If .ActiveEmployee Then
                tContact.AddTableRow("Phone", .PhoneFormatted)

                If Not String.IsNullOrEmpty(.Email) Then
                    Dim anchor As New HyperLink With {
                        .NavigateUrl = "mailto:" & PermitApplication.StaffResponsible.Email,
                        .Text = PermitApplication.StaffResponsible.Email
                    }

                    tContact.AddTableRow("Email", anchor)
                End If
            End If
        End With
    End Sub

    Private Sub DisplayTracking()
        With PermitApplication
            tTracking.AddTableRow("Sent by Facility", .DateSentByFacility, False)
            tTracking.AddTableRow("Received by EPD", .DateReceivedByApb, False)
            tTracking.AddTableRow("Assigned to Staff", .DateAssignedToStaff, False)
            tTracking.AddTableRow("Reassigned", .DateReassignedToStaff)
            tTracking.AddTableRow("Acknowledgement Letter Sent", .DateAcknowledgementLetterSent, False)
            tTracking.AddTableRow("Public Advisory", .PublicAdvisoryNeeded)
            tTracking.AddTableRow("Public Advisory Expires", .DatePAExpires)
            tTracking.AddTableRow("Sent to Unit Manager", .DateToUnitManager, False)
            tTracking.AddTableRow("Sent Program Manager", .DateToProgramManager, False)
            tTracking.AddTableRow("Draft Issued", .DateDraftIssued)
            tTracking.AddTableRow("Public Notice Expires", .DatePNExpires)
            tTracking.AddTableRow("EPA 45-Day Review Ends", .DateEpaCommentPeriodEnds)
            tTracking.AddTableRow("EPA 45-Day Review Waived", .DateEpaCommentPeriodWaived)
            tTracking.AddTableRow("Sent to Administrative Review", .DateToAdministrativeReview)
            tTracking.AddTableRow("Final Action", .DateApplicationFinalized, False)
        End With
    End Sub

    Private Sub DisplayFeeStatus()
        If PermitApplication.ApplicationFeeInfo Is Nothing Then
            pFeesNotApplicable.Visible = True
            Exit Sub
        End If

        With PermitApplication.ApplicationFeeInfo
            ' Applicability
            If Not .FeeDataFinalized Then
                pFeesNotDetermined.Visible = True
                Exit Sub
            End If

            If Not (.ExpeditedFeeApplies Or .ApplicationFeeApplies) Then
                pFeesNotApplicable.Visible = True
                Exit Sub
            End If

            ' Fee amounts table
            tblFeesSummary.Visible = True

            If .ApplicationFeeApplies Then
                tblFeesSummary.AddTableRow("Permit Application Fee: " & .ApplicationFeeDescription, .ApplicationFeeAmount, True, False)
            End If

            If .ExpeditedFeeApplies Then
                tblFeesSummary.AddTableRow("Expedited Processing Fee: " & .ExpeditedFeeDescription, .ExpeditedFeeAmount, True, False)
            End If

            tblFeesSummary.AddTableFooterRow("TOTAL", .ApplicationFeeAmount + .ExpeditedFeeAmount, True, True)

            ' Invoicing
            If Not PermitApplication.IsInvoiceGenerated And FacilityAccess.FeeAccess Then
                pGenerateInvoice.Visible = True
            End If

        End With
    End Sub

    Private Sub DisplayInvoices()
        Dim dt As DataTable = GetApplicationInvoices(AppNumber)

        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            grdInvoices.DataSource = dt
            grdInvoices.DataBind()
            pnlInvoices.Visible = True
        End If
    End Sub

    Private Sub DisplayPayments()
        Dim dt As DataTable = GetApplicationPayments(AppNumber)

        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            grdPayments.DataSource = dt
            grdPayments.DataBind()
            pnlPayments.Visible = True
        End If
    End Sub

    Protected Sub btnGenerateInvoice_Click() Handles btnGenerateInvoice.Click
        If Not IsPostBack Then
            Throw New HttpException(400, "Application error.")
        End If

        pGenerateInvoice.Visible = False

        If IsInvoiceGeneratedForApplication(AppNumber) Then
            pGenerateExists.Visible = True
            Exit Sub
        End If

        Dim newInvoiceID As Integer
        Dim result As GenerateInvoiceResult = GenerateInvoice(AppNumber, CurrentUser.UserId, newInvoiceID)

        Select Case result
            Case GenerateInvoiceResult.DbError
                pGenerateDbError.Visible = True

            Case GenerateInvoiceResult.InvoiceExists
                pGenerateExists.Visible = True

            Case GenerateInvoiceResult.NoApplication
                Throw New HttpException(400, "Permit application number invalid.")

            Case GenerateInvoiceResult.NoLineItems
                pGenerateNoItems.Visible = True

            Case GenerateInvoiceResult.Success
                pGenerateSuccess.Visible = True

        End Select

        DisplayInvoices()
        DisplayPayments()
    End Sub

End Class