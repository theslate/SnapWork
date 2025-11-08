using System;
using System.Collections.Generic;

namespace SnapWork.Export;

internal static class DesktopSelectionResolver
{
    public static Guid? Resolve(IReadOnlyList<EnumeratedWindow> windows, string? selector)
    {
        ArgumentNullException.ThrowIfNull(windows);

        if (string.IsNullOrWhiteSpace(selector))
        {
            return null;
        }

        List<Guid> orderedDesktops = BuildOrderedDesktopList(windows);
        if (orderedDesktops.Count == 0)
        {
            throw new DesktopSelectionException("No desktops were detected during export.");
        }

        if (int.TryParse(selector, out int index))
        {
            if (index < 0 || index >= orderedDesktops.Count)
            {
                throw new DesktopSelectionException(
                    $"Desktop index '{selector}' is out of range. Valid indices: 0..{orderedDesktops.Count - 1}."
                );
            }

            return orderedDesktops[index];
        }

        if (!Guid.TryParse(selector, out Guid parsed))
        {
            throw new DesktopSelectionException(
                $"Desktop selector '{selector}' is not a valid index or GUID."
            );
        }

        if (!orderedDesktops.Contains(parsed))
        {
            throw new DesktopSelectionException(
                $"Desktop '{selector}' was not found in the enumerated windows."
            );
        }

        return parsed;
    }

    private static List<Guid> BuildOrderedDesktopList(IReadOnlyList<EnumeratedWindow> windows)
    {
        List<Guid> ordered = [];
        HashSet<Guid> seen = [];

        foreach (EnumeratedWindow window in windows)
        {
            Guid desktopId = window.DesktopId;
            if (desktopId == Guid.Empty)
            {
                continue;
            }

            if (seen.Add(desktopId))
            {
                ordered.Add(desktopId);
            }
        }

        return ordered;
    }
}
