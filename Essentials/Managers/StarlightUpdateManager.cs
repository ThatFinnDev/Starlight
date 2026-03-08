using System;
using System.Collections;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.Networking;

namespace Starlight.Managers;

internal static class StarlightUpdateManager
{
    internal static bool updatedStarlight = false;
    internal static string newVersion = null;
    static bool IsLatestVersion => newVersion == BuildInfo.DisplayVersion;
    static string branchJson = "";
    internal static IEnumerator GetBranchJson()
    {
        string checkLink = BuildInfo.PreInfo[StarlightEntryPoint.UpdateBranch].Item2;
        if (string.IsNullOrEmpty(checkLink)) yield break;
        UnityWebRequest uwr = UnityWebRequest.Get(checkLink);
        yield return uwr.SendWebRequest();
        if (uwr.isNetworkError || uwr.isHttpError) yield break;
        string json = uwr.downloadHandler.text;
        try
        {
            var jobject = JObject.Parse(json);
            if (!CheckSignature(jobject))
            {
                MelonLogger.Msg("Starlight API's signature is invalid, is the date & time set correctly?");
                throw new Exception();
            }
        }
        catch { MelonLogger.Msg("Starlight API either changed or is broken."); yield break; }
        branchJson = json;
        MelonCoroutines.Start(CheckForNewVersion());
    }
    internal static IEnumerator CheckForNewVersion()
    {
        if (string.IsNullOrWhiteSpace(branchJson)) yield break;
        try
        {
            var jobject = JObject.Parse(branchJson);
            string latest = jobject["latest"].ToObject<string>();
            newVersion = latest;
            if (!IsLatestVersion) if (AllowAutoUpdate.HasFlag()) if (StarlightEntryPoint.autoUpdate)
                MelonCoroutines.Start(UpdateVersion());
        }
        catch { MelonLogger.Msg("Starlight API either changed or is broken."); }
    }
    internal static IEnumerator UpdateVersion()
    {
        if (string.IsNullOrWhiteSpace(branchJson)) yield break;
        string updateLink = "";
        try
        {
            var jobject = JObject.Parse(branchJson);
            string latest = jobject["latest"].ToObject<string>();
            var latestVersion = jobject["versions_info"][latest];
            updateLink = latestVersion["download_url"].ToObject<string>();
        }
        catch { MelonLogger.Msg("Starlight API either changed or is broken."); yield break; }
        if (string.IsNullOrEmpty(updateLink)) yield break;
        UnityWebRequest uwr = UnityWebRequest.Get(updateLink);
        yield return uwr.SendWebRequest();
        if (!uwr.isNetworkError && !uwr.isHttpError)
            if (uwr.result == UnityWebRequest.Result.Success)
            {
                MelonLogger.Msg("Downloading Starlight complete");
                string path = StarlightEntryPoint.Instance.MelonAssembly.Assembly.Location;
                if (File.Exists(path))
                {
                    if(File.Exists(path + ".old")) File.Delete(path + ".old");
                    File.Move(path, path + ".old");
                }
                File.WriteAllBytes(Path.Combine(new FileInfo(path).Directory.FullName, "Starlight.dll"), uwr.downloadHandler.data);
                updatedStarlight = true;
                MelonLogger.Msg("Restart needed for applying Starlight update");
            }
    }
    
    internal static bool CheckSignature(JObject jobject)
    {
        try
        {
            if (!jobject.ContainsKey("signature") || !jobject.ContainsKey("expires")) return false;
            var signatureBase64 = jobject["signature"].Value<string>();
            var expires = jobject["expires"].Value<long>();

            if (DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() > expires) return false;

            var copy = (JObject)jobject.DeepClone();
            copy.Remove("signature");
            var dict = copy.ToObject<Dictionary<string, object>>();
            var sortedDict = new SortedDictionary<string, object>(dict);
            var canonicalJson = JsonConvert.SerializeObject(sortedDict, Formatting.None);
    
            var dataBytes = Encoding.UTF8.GetBytes(canonicalJson);
            var signatureBytes = Convert.FromBase64String(signatureBase64);
        
            var pubKeyBase64 = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAvW1COeevhDiQl5G4BbLTIZi+oGZNQSyorDs/ue1YGACa/KQEysp3akgFgR86sudcAnaA+b6qaR50hD+/Wd+ZofYxv8LRI1J6RqKWNzZDK8yJePo6eVbqPz1F/JP96+pNXHEGy7yJH0VRjO+h/Jb15vNfOSwfnoIWY4XUm/+LONTCTDXOfAS9HBHy6r3zc0AI0A+101Q8LYDhMrINiirVkQRZw4W5FzNF0Ouvj0emkQfyJ7pRZcD+GwrXhN5Sad65QeWF8joKq8aCcyEN0oifieDCtGyBCGbV+Jm3zElraS1NIKWDjQnT+b4QOJtSKRHSymVqX3oFWahV3cadi/CfqQIDAQAB"; 
            var publicKeyBytes = Convert.FromBase64String(pubKeyBase64);

            using (var rsa = RSA.Create())
            {
                rsa.ImportSubjectPublicKeyInfo(publicKeyBytes, out _);
                return rsa.VerifyData(dataBytes, signatureBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            }
        }
        catch (Exception e)
        {
            MelonLogger.Error("Signature verification error: " + e.Message);
            return false;
        }
    }
}