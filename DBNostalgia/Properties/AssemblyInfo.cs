﻿using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("DBNostalgia")]
[assembly: AssemblyDescription("Stored Procedure centered alternative for handling your database interactions.")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("https://github.com/cg-h4voK/db-nostalgia-csharp")]
[assembly: AssemblyProduct("DBNostalgia")]
[assembly: AssemblyCopyright("Copyright ©  2018")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("d50f2732-ebd7-4a07-a196-0cb3a2191f9f")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.1.4")]
[assembly: AssemblyFileVersion("1.0.1.4")]

// Enable testing of internal methods to our test project
[assembly: InternalsVisibleTo("DBNostalgia.Test")]

// Allow a MoQ dependency to also see our internals
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]