' File: GlobalShared/Constants/CompanyAgencyTypeRegistry.vb
Namespace GlobalShared.Constants

    Public Class CompanyAgencyTypeInfo
        Public Property AgencyType As String
        Public Property DisplayName As String          ' Tab label: "SSS", "PhilHealth", "Pag-IBIG", "BIR"
        Public Property RegistrationNoLabel As String   ' "SSS No.", "PhilHealth No.", "Pag-IBIG No.", "TIN"
        Public Property BranchLabel As String           ' "Branch", "Branch", "Branch", "RDO"
    End Class

    ''' <summary>
    ''' Whitelist ng 4 Agency types na naka-share sa tblCompanyAgencyRegistration.
    ''' Kapareho ng function ng StatutorySettingsTableRegistry, pero dito
    ''' AgencyType discriminator ang key, hindi table name - dahil isang
    ''' table lang talaga, 4 lang ang magkaibang value.
    ''' </summary>
    Public NotInheritable Class CompanyAgencyTypeRegistry

        Public Shared ReadOnly Types As New Dictionary(Of String, CompanyAgencyTypeInfo) From {
            {"SSS", New CompanyAgencyTypeInfo With {.AgencyType = "SSS", .DisplayName = "SSS", .RegistrationNoLabel = "SSS No.", .BranchLabel = "Branch"}},
            {"PHILHEALTH", New CompanyAgencyTypeInfo With {.AgencyType = "PHILHEALTH", .DisplayName = "PhilHealth", .RegistrationNoLabel = "PhilHealth No.", .BranchLabel = "Branch"}},
            {"PAGIBIG", New CompanyAgencyTypeInfo With {.AgencyType = "PAGIBIG", .DisplayName = "Pag-IBIG", .RegistrationNoLabel = "Pag-IBIG No.", .BranchLabel = "Branch"}},
            {"BIR", New CompanyAgencyTypeInfo With {.AgencyType = "BIR", .DisplayName = "BIR", .RegistrationNoLabel = "TIN", .BranchLabel = "RDO"}}
        }

        Public Shared Function IsAllowed(agencyType As String) As Boolean
            Return Types.ContainsKey(agencyType)
        End Function

        Public Shared Function GetInfo(agencyType As String) As CompanyAgencyTypeInfo
            If Not IsAllowed(agencyType) Then
                Throw New ArgumentException($"Invalid company agency type: {agencyType}")
            End If
            Return Types(agencyType)
        End Function

    End Class
End Namespace