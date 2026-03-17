using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Starlight.Utils;

public static class EmbeddedResourceEUtil
{
    static Assembly ToAssembly(MethodBase method)
    {
        if (method != null&&method.ReflectedType!=null)
        {
            return method.ReflectedType.Assembly;
        }
        return null;
    }
    public static Sprite LoadSprite(string fileName)
    {
        return LoadSprite(fileName,ToAssembly(new StackTrace().GetFrame(1)?.GetMethod()));
    }
    public static Sprite LoadSprite(string fileName, Assembly assembly) => LoadTexture2D(fileName,assembly).Texture2DToSprite();
    
    
    public static Texture2D LoadTexture2D(string fileName)
    {
        return LoadTexture2D(fileName, ToAssembly(new StackTrace().GetFrame(1)?.GetMethod()));
    }
    public static Texture2D LoadTexture2D(string filename, Assembly assembly)
    {
        if (assembly == null) return null;
        var realFilename = filename.Replace("/",".");
        if (!(realFilename.EndsWith(".png") || realFilename.EndsWith(".jpg") || realFilename.EndsWith(".exr"))) return null;
        
        var stream = assembly.GetManifestResourceStream(GetPrefix(assembly) + "." + filename);
        if (stream != null)
        {
            byte[] array = new byte[stream.Length];
            var unused=stream.Read(array, 0, array.Length);

            var texture2D = new Texture2D(1, 1);
            ImageConversion.LoadImage(texture2D, array);
        
            texture2D.filterMode = FilterMode.Bilinear;
        
            return texture2D;
        }

        return null;
    }

    
    
    public static Dictionary<string, byte[]> LoadResources(string folderNamespace, bool recursive = false)
    {
        folderNamespace=folderNamespace.Replace("/",".");
        var method = new StackTrace().GetFrame(1)?.GetMethod();
        if (method != null&&method.ReflectedType!=null)
        {
            var assembly = method.ReflectedType.Assembly;
            var baseNamespace = GetPrefix(assembly) + (string.IsNullOrEmpty(folderNamespace) ? "":"." + folderNamespace);

            var resourceNames = assembly.GetManifestResourceNames().Where(r =>
                    recursive
                        ? r.StartsWith(baseNamespace + ".")
                        : r.Substring(0, r.LastIndexOf('.')).Equals(baseNamespace))
                .ToArray();
            var result = new Dictionary<string, byte[]>();

            foreach (var resourceName in resourceNames)
            {
                using var stream = assembly.GetManifestResourceStream(resourceName);
                if (stream == null) continue;
                byte[] bytes = new byte[stream.Length];
                var unused = stream.Read(bytes, 0, bytes.Length);

                string fileName = resourceName.Substring(baseNamespace.Length + 1);
                result[fileName] = bytes;
            }

            return result;
        }

        return null;
    }

    
    
    public static byte[] LoadResource(string filename)
    {
        var method = new StackTrace().GetFrame(1)?.GetMethod();
        if (method != null&&method.ReflectedType!=null)
        {
            var assembly = method.ReflectedType.Assembly;
            return LoadResource(filename, assembly);
        }

        return null;
    }
    public static byte[] LoadResource(string filename, Assembly assembly)
    {
        if(assembly == null) return null;
        filename=filename.Replace("/",".");
        var stream = assembly.GetManifestResourceStream(GetPrefix(assembly) + "." + filename);
        if (stream != null)
        {
            byte[] array = new byte[stream.Length];
            var unused = stream.Read(array, 0, array.Length);
            return array;
        }

        return null;
    }
    
    
    
    public static string LoadString(string filename)
    {
        var method = new StackTrace().GetFrame(1)?.GetMethod();
        if (method != null&&method.ReflectedType!=null)
        {
            var assembly = method.ReflectedType.Assembly;
            return LoadString(filename, assembly);
        }
        return null;
    }
    public static string LoadString(string filename, Assembly assembly)
    {
        if(assembly == null) return null;
        filename=filename.Replace("/",".");
        var stream = assembly.GetManifestResourceStream(GetPrefix(assembly) + "." + filename);
        if (stream != null)
        {
            byte[] array = new byte[stream.Length];
            var unused = stream.Read(array, 0, array.Length);
            return System.Text.Encoding.Default.GetString(array);
        }

        return null;
    }
    
    public static Il2CppAssetBundle LoadIl2CppBundle(string filename)
    {
        var method = new StackTrace().GetFrame(1)?.GetMethod();
        if (method != null&&method.ReflectedType!=null)
        {
            var assembly = method.ReflectedType.Assembly;
            return LoadIl2CppBundle(filename, assembly);
        }

        return null;
    }
    public static Il2CppAssetBundle LoadIl2CppBundle(string filename, Assembly assembly)
    {
        if(assembly == null) return null;
        filename=filename.Replace("/",".");
        var stream = assembly.GetManifestResourceStream(GetPrefix(assembly) + "." + filename);
        if (stream != null)
        {
            byte[] array = new byte[stream.Length];
            var unused = stream.Read(array, 0, array.Length);
            return Il2CppAssetBundleManager.LoadFromMemory(array);
        }

        return null;
    }
    
    public static AssetBundle LoadBundle(string filename)
    {
        var method = new StackTrace().GetFrame(1)?.GetMethod();
        if (method != null&&method.ReflectedType!=null)
        {
            var assembly = method.ReflectedType.Assembly;
            return LoadBundle(filename, assembly);
        }

        return null;
    }

    static string GetPrefix(Assembly assembly) => assembly.GetManifestResourceNames().FirstOrDefault()?.Split('.')[0];
    
    public static AssetBundle LoadBundle(string filename, Assembly assembly)
    {
        if(assembly == null) return null;
        filename=filename.Replace("/",".");
        var stream = assembly.GetManifestResourceStream(GetPrefix(assembly) + "." + filename);
        if (stream != null)
        {
            byte[] array = new byte[stream.Length];
            var unused = stream.Read(array, 0, array.Length);

            return AssetBundle.LoadFromMemory(array);
        }

        return null;
    }
    
}