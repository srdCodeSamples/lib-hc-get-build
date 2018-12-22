using System;
using System.Collections.Generic;

namespace HcGetProduct.Classes
{
    // A class that represents the version object returned by HC releases API
    public class HcReleasesVersion
    {
        public string Name { get; set; }
        public string Version  { get; set; }
        public string shasums { get; set; }
        public string shasums_signature { get; set; }
        public List<HcReleasesBuild> Builds { get; set; }

        public HcReleasesVersion (string name) 
        {
            Name = name;
            Builds = new List<HcReleasesBuild>();
        }

        public HcReleasesVersion (): this(String.Empty) {}
    }
}