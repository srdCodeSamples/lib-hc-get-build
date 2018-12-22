using System;

namespace HcGetProduct.Classes
{
    public class HcReleasesBuild
    {
         // A class that represents the build object returned by HC releases API
        public string Name { get; set; }
        public string Version  { get; set; }
        public string Os { get; set; }
        public string Arch { get; set; }
        public string Filename { get; set; }
        public string Url { get; set; }

        public HcReleasesBuild(string name) => Name = name;

        public HcReleasesBuild(): this(String.Empty) {}
    }
}