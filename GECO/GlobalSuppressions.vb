
' This file is used by Code Analysis to maintain SuppressMessage 
' attributes that are applied to this project.
' Project-level suppressions either have no target or are given 
' a specific target and scoped to a namespace, type, member, etc.

<Assembly: CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification:="GECO was designed around this pattern.")>
<Assembly: CodeAnalysis.SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification:="Rule does not apply to GECO.")>
<Assembly: CodeAnalysis.SuppressMessage("Security", "CA5368:Set ViewStateUserKey For Classes Derived From Page", Justification:="Applying the recommended fix broke the GECO login system.")>
<Assembly: CodeAnalysis.SuppressMessage("Security", "CA5351:Do Not Use Broken Cryptographic Algorithms", Justification:="MD5 is only used prior to BCrypt hashing for backwards compatibility with existing passwords.", Scope:="member", Target:="~M:GECO.Md5Hashing.GetMd5Hash(System.String)~System.String")>
<Assembly: CodeAnalysis.SuppressMessage("Security", "CA5351:Do Not Use Broken Cryptographic Algorithms", Justification:="EncryptDecrypt is only used to obscure cookie data which is already restricted to HTTPS only.", Scope:="member", Target:="~M:GECO.EncryptDecrypt.Encrypt(System.String,System.String)~System.String")>
<Assembly: CodeAnalysis.SuppressMessage("Security", "CA5351:Do Not Use Broken Cryptographic Algorithms", Justification:="EncryptDecrypt is only used to obscure cookie data which is already restricted to HTTPS only.", Scope:="member", Target:="~M:GECO.EncryptDecrypt.Decrypt(System.String,System.String)~System.String")>
<Assembly: CodeAnalysis.SuppressMessage("Style", "IDE0044:Add readonly modifier", Justification:="The MyWebExtension class was auto-generated. I don't even know what it does.", Scope:="member", Target:="~F:GECO.My.MyWebExtension.s_Computer")>
<Assembly: CodeAnalysis.SuppressMessage("Style", "IDE0044:Add readonly modifier", Justification:="The MyWebExtension class was auto-generated. I don't even know what it does.", Scope:="member", Target:="~F:GECO.My.MyWebExtension.s_User")>
<Assembly: CodeAnalysis.SuppressMessage("Style", "IDE0044:Add readonly modifier", Justification:="The MyWebExtension class was auto-generated. I don't even know what it does.", Scope:="member", Target:="~F:GECO.My.MyWebExtension.s_Log")>
<Assembly: CodeAnalysis.SuppressMessage("Globalization", "CA1303", Justification:="Low impact/high effort to fix.")>
<Assembly: CodeAnalysis.SuppressMessage("Globalization", "CA1304:Specify CultureInfo", Justification:="Low impact/high effort to fix.")>
<Assembly: CodeAnalysis.SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification:="Low impact/high effort to fix.")>
<Assembly: CodeAnalysis.SuppressMessage("Globalization", "CA1307:Specify StringComparison", Justification:="Low impact/high effort to fix.")>
<Assembly: CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification:="Existing code style")>
