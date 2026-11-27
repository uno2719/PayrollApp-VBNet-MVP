' File: Modules/MainMenu/Employee/Models/WageReference.vb
Namespace Employee.Models

    ''' <summary>Minimum wage na batayan ng isang branch/store: ang Wage Area nito at ang rate sa petsa (Nothing kung wala).</summary>
    Public Class WageReference
        Public Property WageArea As String
        Public Property Rate As GlobalShared.Models.MinimumWageModel
    End Class

End Namespace
