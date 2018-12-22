using System;
using Xunit;
using HcGetProduct.Classes;
using System.Linq;
using NuGet;

namespace HcGetProduct.Tests
{
    public class IsPopulated_HcReleaseData
    {
        private readonly HcReleaseData[] hcData;
        private readonly string[] hcProducts = new string[] { "terraform", "vagrant", "packer" };
        private readonly string hcLinkPattern = "https://.*";

        public IsPopulated_HcReleaseData()
        {
            hcData = new HcReleaseData[hcProducts.Length];
            for (int i = 0; i < hcProducts.Length; i++)
            {
                hcData[i] = HcHelpers.GetHcProductReleasesData(hcProducts[i]).Result;
            }
        }

        /// <summary>
        /// Test if the the method HcHelpers.GetHcProductReleasesData(string product)
        /// working correctly.
        /// </summary>
        [Theory]
        [InlineData("terraform")]
        [InlineData("vagrant")]
        [InlineData("packer")]
        public void IsPopulatingCorrectly_GetHcProductReleasesData(string product)
        {
            HcReleaseData testedInstance = hcData.Where(data => data.Name.Equals(product)).SingleOrDefault();

            // check if the correct product was received
            Assert.Equal(testedInstance.Name, product);

            // Check if the versions list is empty
            Assert.True(testedInstance.Versions.Count > 0);

            // check that a version contains builds
            Assert.True(testedInstance.Versions.First().Value.Builds.Count > 0);
        }


        /// <summary>
        /// Verify HcreleasesData.GetLatestVersion returns a version
        /// and can be parsed as SemanticVersion
        /// </summary>
        [Fact]
        public void IsWorking_HcRleasesData_GetLatestVersion()
        {
            string latestVer = null;
            latestVer = hcData[0].GetLatestVersion();

            // check if value was returned
            Assert.False(String.IsNullOrEmpty(latestVer));

            // check if value is a parsable version
            Assert.True(SemanticVersion.TryParse(latestVer, out SemanticVersion parsedVer));

        }

        /// <summary>
        /// Test if the latest version for each product can be found
        /// and if it the builds in it are returned.
        /// </summary>
        [Theory]
        [InlineData("terraform", "linux", "amd64")]
        [InlineData("vagrant", "darwin", "x86_64")]
        [InlineData("packer", "linux", "amd64")]
        public void IsLatestVersionBuildOK(string product, string singleOs, string singleArch)
        {
            HcReleaseData testedInstance = hcData.Where(data => data.Name.Equals(product)).SingleOrDefault();

            HcReleasesBuild build = testedInstance.GetBuild(singleOs, singleArch);

            // test if a build was returned for the latest version and os/arch combination
            Assert.NotNull(build);

            // test if the build url matches the expected pattern
            Assert.Matches(hcLinkPattern, build.Url);
        }
    }
}
