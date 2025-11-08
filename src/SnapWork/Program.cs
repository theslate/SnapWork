using System;
using SnapWork.Export;
using SnapWork.Models;
using SnapWork.Serialization;
using SnapWork.Validation;

namespace SnapWork;

public static class Program
{
    public static Task<int> Main(string[] args)
    {
        int exitCode = Run(args);
        return Task.FromResult(exitCode);
    }

    private static int Run(string[] args)
    {
        if (args.Length == 0 || IsHelpRequest(args[0]))
        {
            PrintUsage();
            return 1;
        }

        string command = args[0].ToLowerInvariant();

        try
        {
            return command switch
            {
                "validate" => RunValidate(args),
                "print" => RunPrint(args),
                "export" => RunExport(args),
                _ => HandleUnknownCommand(command),
            };
        }
        catch (DesktopSelectionException exception)
        {
            Console.Error.WriteLine($"Desktop selection error: {exception.Message}");
            return 3;
        }
        catch (VirtualDesktopNotSupportedException exception)
        {
            Console.Error.WriteLine($"Virtual desktop is unsupported: {exception.Message}");
            return 4;
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine($"Error: {exception.Message}");
            return 1;
        }
    }

    private static bool IsHelpRequest(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return true;
        }

        return value is "-h" or "--help" or "/?" or "help";
    }

    private static int HandleUnknownCommand(string command)
    {
        Console.Error.WriteLine($"Unknown command '{command}'.");
        PrintUsage();
        return 1;
    }

    private static int RunValidate(string[] args)
    {
        string? filePath = GetRequiredFilePath(args);
        if (filePath is null)
        {
            return 1;
        }

        Workspace workspace = WorkspaceSerializer.Load(filePath);
        WorkspaceValidationResult result = WorkspaceValidator.Validate(workspace);

        if (!result.IsValid)
        {
            Console.Error.WriteLine("Validation failed:");
            foreach (string error in result.Errors)
            {
                Console.Error.WriteLine($" - {error}");
            }

            return 1;
        }

        Console.WriteLine("Workspace is valid.");
        return 0;
    }

    private static int RunExport(string[] args)
    {
        string? filePath = GetRequiredFilePath(args);
        if (filePath is null)
        {
            return 1;
        }

        string? desktopSelector = ResolveDesktopSelector(args);
        var exporter = new WorkspaceExporter(CreateWindowEnumerator());
        Workspace workspace = exporter.Export(filePath, desktopSelector);

        Console.WriteLine($"Exported {workspace.Windows.Count} window(s) to '{filePath}'.");
        if (!string.IsNullOrWhiteSpace(desktopSelector))
        {
            Console.WriteLine($"Desktop selector: {desktopSelector}");
        }

        return 0;
    }

    private static int RunPrint(string[] args)
    {
        string? filePath = GetRequiredFilePath(args);
        if (filePath is null)
        {
            return 1;
        }

        Workspace workspace = WorkspaceSerializer.Load(filePath);
        var windows = workspace.Windows ?? Array.Empty<WindowSpec>();

        Console.WriteLine($"Version: {workspace.Version}");
        Console.WriteLine($"GeneratedUtc: {workspace.GeneratedUtc:O}");
        Console.WriteLine($"Windows ({windows.Count}):");

        for (int index = 0; index < windows.Count; index++)
        {
            WindowSpec window = windows[index];
            Console.WriteLine($"[{index}] {window.Title}");
            Console.WriteLine($" ProcessPath: {window.ProcessPath}");
            Console.WriteLine($" MonitorId: {window.MonitorId}");
            Console.WriteLine($" Bounds: ({window.X}, {window.Y}) {window.Width}x{window.Height}");
        }

        return 0;
    }

    private static string? GetRequiredFilePath(string[] args)
    {
        string? filePath = ResolveFilePath(args);
        if (string.IsNullOrWhiteSpace(filePath))
        {
            Console.Error.WriteLine("A file path must be provided using --file <path>.");
            return null;
        }

        return filePath;
    }

    private static string? ResolveFilePath(string[] args)
    {
        const string longOption = "--file";
        const string shortOption = "-f";

        for (int index = 1; index < args.Length; index++)
        {
            string current = args[index];

            if (IsOptionMatch(current, longOption) || IsOptionMatch(current, shortOption))
            {
                if (index + 1 < args.Length)
                {
                    return args[index + 1];
                }

                return null;
            }

            string? fromAssignment =
                ExtractAssignedValue(current, longOption)
                ?? ExtractAssignedValue(current, shortOption);
            if (!string.IsNullOrWhiteSpace(fromAssignment))
            {
                return fromAssignment;
            }
        }

        return null;
    }

    private static bool IsOptionMatch(string value, string option) =>
        string.Equals(value, option, StringComparison.OrdinalIgnoreCase);

    private static string? ExtractAssignedValue(string value, string option)
    {
        string prefix = option + "=";
        if (value.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
        {
            return value.Substring(prefix.Length);
        }

        return null;
    }

    private static string? ResolveDesktopSelector(string[] args)
    {
        const string option = "--desktop";

        for (int index = 1; index < args.Length; index++)
        {
            string current = args[index];

            if (IsOptionMatch(current, option))
            {
                if (index + 1 < args.Length)
                {
                    return args[index + 1];
                }

                return null;
            }

            string? assigned = ExtractAssignedValue(current, option);
            if (!string.IsNullOrWhiteSpace(assigned))
            {
                return assigned;
            }
        }

        return null;
    }

    private static IWindowEnumerator CreateWindowEnumerator()
    {
        if (!OperatingSystem.IsWindows())
        {
            throw new VirtualDesktopNotSupportedException("Virtual desktops require Windows.");
        }

        return new WindowEnumerator(new ComDesktopIdProvider());
    }

    private static void PrintUsage()
    {
        Console.WriteLine("Usage: snapwork <command> [options]");
        Console.WriteLine();
        Console.WriteLine("Commands:");
        Console.WriteLine("  validate   Validates a workspace definition.");
        Console.WriteLine("  print      Prints workspace details.");
        Console.WriteLine(
            "  export     Captures the current desktop layout into a workspace file."
        );
        Console.WriteLine();
        Console.WriteLine("Options:");
        Console.WriteLine("  --file, -f <path>   Path to workspace YAML file.");
        Console.WriteLine(
            "  --desktop <index|guid>   Filter export to a specific virtual desktop."
        );
    }
}
