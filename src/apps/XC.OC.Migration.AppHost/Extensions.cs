namespace XC.OC.Migration.AppHost;

public static class Extensions
{
    public static IResourceBuilder<ExecutableResource> AddAzureFunction<TServiceMetadata>(
        this IDistributedApplicationBuilder builder, string name)
    where TServiceMetadata : IProjectMetadata, new()
    {
        var serviceMetadata = new TServiceMetadata();
        var projectPath = serviceMetadata.ProjectPath;
        var projectDirectory = Path.GetDirectoryName(projectPath);

        var args = new[]
        {
            "host",
            "start"
        };

        return builder.AddResource(new ExecutableResource(name, "func", $"{projectDirectory!}"))
            .WithArgs(args)
            .WithOtlpExporter();
    }
}