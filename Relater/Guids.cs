// Guids.cs
// MUST match guids.h
using System;

namespace Relater
{
    static class GuidList
    {
        public const string guidVSPackage1PkgString = "d255e352-5421-450e-9349-d4aafefd6395";
        public const string guidVSPackage1CmdSetString = "034a8cf8-f4af-44be-8a2e-bdc86d8fa460";
        public const string guidToolWindowPersistanceString = "2185cee2-6849-455a-9c90-7d4dfb635b09";

        public static readonly Guid guidVSPackage1CmdSet = new Guid(guidVSPackage1CmdSetString);
    };
}