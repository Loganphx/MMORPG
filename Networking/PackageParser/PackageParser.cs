using System.Reflection;
using Microsoft.Extensions.Logging;

namespace Networking.PackageParser;

public class PackageParser : IPackageParser
{

    private readonly ILogger<PackageParser> _logger;
    private Dictionary<CommuncationPackage, Type> packageTypes = new Dictionary<CommuncationPackage, Type>();

    public PackageParser(ILogger<PackageParser> logger)
    {
        this._logger = logger;
        ResolvePackage();
    }

    private void ResolvePackage()
    {
        var packageClasses = GetType().Assembly
            .GetTypes()
            .Where(x => x.IsSubclassOf(typeof(PackageBase)));

        foreach (var classType in packageClasses)
        {
            var attribute = classType.GetCustomAttributes(typeof(PackageTypeAttribute), false);

            if (attribute.FirstOrDefault() is PackageTypeAttribute packageTypeAttribute)
            {
                packageTypes.Add(packageTypeAttribute.PackageType, classType);
            }
        }
        
        _logger.LogInformation($"Scanned {packageTypes.Count} Package Types");
    }
    public PackageBase ParsePackageFromStream(BinaryReader reader)
    {
        var packageType = (CommuncationPackage)reader.ReadUInt32();

        if (packageTypes.TryGetValue(packageType, out var type))
        {
            var package = Activator.CreateInstance(type) as PackageBase;
            package!.DeserializeFromStream(reader);
            _logger.LogInformation($"Received package from stream type: {package.GetType()}");
        }

        throw new InvalidOperationException("Package is unknown");
    }

    public void ParsePackageToStream(PackageBase package, BinaryWriter writer)
    {
        _logger.LogInformation($"Write Package to Stream Type: {package.GetType()}");
        package.SerializeToStream(writer);
        writer.Flush();
    }
}