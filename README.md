# Library to Get HC products' builds

A basic library that provides methods to get build information for different HC products from thier releases API.

Targeted framework is netstandard2.0

## Usage

* Call the static async HcReleasesHelpers.GetHcProductReleasesData with an HC product name and an instance of HttpClient (optional) to get a HcReleasesData instance.
* Use the HcReleasesData methods to get build information:
  * GetBuild() - pass the needed OS, architecture and version. Version may be omitted in which case the latest stable version build will be returned
  * GetBuildUrl() - same usage as GetBuild(). Will call GetBuild() internally and return the build url as string
  * GetLatestVersion() - returnes the latest version as string. Pass true/false (default is true) to skip pre-releases.
