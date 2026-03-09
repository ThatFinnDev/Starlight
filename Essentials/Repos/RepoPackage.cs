using System;
using Starlight.Enums;

namespace Starlight.Repos;

[System.Serializable]
public class RepoPackage
{
    public string name;
    public string author;
    public string coauthors;
    public string contributors;
    public string description;
    public string company;
    public string trademark;
    public string team;
    public string copyright;
    public string sourcecode;
    public string github_repo;
    public string nexus;
    public string website;
    public string header_url;
    public string icon_url;
    public byte colorR;
    public byte colorG;
    public byte colorB;
    public byte colorA;
    public PackageType type = PackageType.MelonMod;
    public bool universal;
    public List<RepoPackageVersion> versions = new List<RepoPackageVersion>();

    public RepoPackageVersion getLatestVersion(string branch)
    {
        RepoPackageVersion latestVersion = null;
        foreach (var version in versions)
        {
            if (branch == version.branch)
            {
                if(latestVersion == null)
                    latestVersion = version;
                else
                {
                    DateTime dateNew = DateTime.Parse(version.release_date, null, System.Globalization.DateTimeStyles.AdjustToUniversal);
                    DateTime dateOld = DateTime.Parse(latestVersion.release_date, null, System.Globalization.DateTimeStyles.AdjustToUniversal);
                    if(dateNew>dateOld)
                        latestVersion = version;
                }
            }
        }

        return latestVersion;
    }
}