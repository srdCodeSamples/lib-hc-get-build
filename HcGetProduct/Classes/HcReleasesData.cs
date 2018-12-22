using System;
using System.Collections.Generic;
using NuGet;
using System.Linq;

namespace HcGetProduct.Classes
{
    // Class that represents the object returned by the HC releases API
    public class HcReleaseData
    {
        // The HC product for which the data is
        public string Name { get; set; }

        // Contains all available versions
        public Dictionary<string, HcReleasesVersion> Versions { get; set; }

        // class should be initialized using HcReleseHelpers.GetHcProductReleasesData.
        private HcReleaseData(string name) 
        {
            Name = name;
            Versions = new Dictionary<string, HcReleasesVersion>();           
        }

        private HcReleaseData() : this(string.Empty) {}

        /// <summary>
        /// Get an HcReleasesBuild object by the OS, architecture and version properties.async
        /// If build is not found returns null
        /// <summary>
        public HcReleasesBuild GetBuild(string os, string arch, string version = "latest") 
        {
            string requestedVersion = version;
            if(version == "latest") {
                requestedVersion = GetLatestVersion();
            }

            HcReleasesBuild hcBuild = null;
            HcReleasesVersion hcVersion = Versions[requestedVersion];
            if (hcVersion != null)
            {
                hcBuild = hcVersion.Builds.Where( 
                build => build.Os == os && build.Arch == arch
                ).SingleOrDefault();
            }
            return hcBuild;
            
        }

        /// <summary>
        /// Get the url of a HC build by the  OS, architecture and version properties
        /// <summary>
        public string GetBuildUrl(string os, string arch, string version = "latest") 
        {
            HcReleasesBuild hcBuild = GetBuild(os, arch, version);
            if (hcBuild != null)
            {
                return hcBuild.Url;
            }
            else
            {
                return null;
            }
            
        }

        /// <summary>
        /// Determine the latest version in the Versions dictionary
        /// <summary>
        public string GetLatestVersion (bool skipPreRelease = true)
        {
            SemanticVersion semVersion = new SemanticVersion("0.0.0");
            List<string> allVersions = new List<string>(Versions.Keys);
            

            foreach (string version in allVersions)
            {
                SemanticVersion versionCandidate = new SemanticVersion(version);

                if (skipPreRelease && !string.IsNullOrEmpty(versionCandidate.SpecialVersion)) 
                {
                    continue;
                }

                if (versionCandidate.CompareTo(semVersion) > 0 )
                {
                    semVersion = versionCandidate;
                }

            }
            return semVersion.ToOriginalString();
        }
    }
}