using System;
using System.IO;
using SnapWork.Models;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace SnapWork.Serialization;

public static class WorkspaceSerializer
{
    private static readonly ISerializer Serializer = new SerializerBuilder()
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitNull)
        .Build();

    private static readonly IDeserializer Deserializer = new DeserializerBuilder()
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .IgnoreUnmatchedProperties()
        .Build();

    public static Workspace Load(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        string yaml = File.ReadAllText(path);
        return Deserialize(yaml);
    }

    public static void Save(Workspace workspace, string path)
    {
        ArgumentNullException.ThrowIfNull(workspace);
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        string yaml = Serialize(workspace);
        File.WriteAllText(path, yaml);
    }

    public static Workspace Deserialize(string yaml)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(yaml);
        return Deserializer.Deserialize<Workspace>(yaml);
    }

    public static string Serialize(Workspace workspace)
    {
        ArgumentNullException.ThrowIfNull(workspace);
        return Serializer.Serialize(workspace);
    }
}
