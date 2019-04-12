
' This file is used by Code Analysis to maintain SuppressMessage 
' attributes that are applied to this project.
' Project-level suppressions either have no target or are given 
' a specific target and scoped to a namespace, type, member, etc.

<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification:="GECO was designed around this pattern.")>
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification:="Rule does not apply to GECO.")>
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "CA5368:Set ViewStateUserKey For Classes Derived From Page", Justification:="Applying the recommended fix broke the GECO login system.")>
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2213:Disposable fields should be disposed", Justification:="Crystal Reports document is disposed in Page Unload event.", Scope:="member", Target:="~F:GECO.AnnualFees_Invoice.CrReportDocument")>
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "CA5351:Do Not Use Broken Cryptographic Algorithms", Justification:="MD5 is only used prior to BCrypt hashing for backwards compatibility with existing passwords.", Scope:="member", Target:="~M:GECO.Md5Hashing.GetMd5Hash(System.String)~System.String")>
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "CA5351:Do Not Use Broken Cryptographic Algorithms", Justification:="EncryptDecrypt is only used to obscure cookie data which is already restricted to HTTPS only.", Scope:="member", Target:="~M:GECO.EncryptDecrypt.Encrypt(System.String,System.String)~System.String")>
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "CA5351:Do Not Use Broken Cryptographic Algorithms", Justification:="EncryptDecrypt is only used to obscure cookie data which is already restricted to HTTPS only.", Scope:="member", Target:="~M:GECO.EncryptDecrypt.Decrypt(System.String,System.String)~System.String")>