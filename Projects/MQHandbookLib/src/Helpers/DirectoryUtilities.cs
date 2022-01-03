using System;
using System.IO;

namespace MQHandbookLib.src.Helpers;

public static class DirectoryUtilities
{
    public static bool TryCreateDirectory(string filePath) {
        FileInfo outputFileInfo;
        try {
            outputFileInfo = new FileInfo(filePath);

            if (!Directory.Exists(outputFileInfo.DirectoryName))
                Directory.CreateDirectory(outputFileInfo.DirectoryName);
            return true;
        }
        catch (Exception ex) {
            Console.WriteLine($"Failed to create directory.\n {ex.Message}");
            return false;
        }
    }
}
