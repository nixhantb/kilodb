using System.Text;
using System.Threading.Tasks.Dataflow;

namespace KiloDb.Cli;

// small helper function that makes console output easy to read
public static class Ui
{
    public static void Title(string text)
    {
        Console.WriteLine();
        Write(ConsoleColor.Cyan, text);
        Write(ConsoleColor.Cyan, new string('=', text.Length));
        
    }

    public static void Section(string text)
    {
        Console.WriteLine();
        Write(ConsoleColor.Yellow, "--- "+ text + " ---");

    }

    public static void Info(string text) => Console.WriteLine(" " + text);
    public static void Ok(string text) => Write(ConsoleColor.Green, "   [OK] " + text);
    public static void Warn(string text) => Write(ConsoleColor .Magenta, "  [!!] " + text);


    public static string FreshFolder(string name)
    {
        string path = Path.Combine("data", name);
        if(Directory.Exists(path)) Directory.Delete(path, recursive: true);
        Directory.CreateDirectory(path);
        return path;
    }

    public static string HexDump(ReadOnlySpan<byte> bytes , int bytesPerLine = 16)
    {
        // example byte[] bytes = { 0x48, 0x65, 0x6C, 0x6C, 0x6F };
        // bytesPerLen = 16 

        var sb = new StringBuilder();

        for(int offset = 0; offset < bytes.Length; offset += bytesPerLine)
        {
            var line = bytes.Slice(offset, Math.Min(bytesPerLine, bytes.Length - offset));
            sb.Append($"    {offset: X4}    "); // 0000 
            for(int i=0; i < bytesPerLine; i++)
            {
                sb.Append(i < line.Length ? $"{line[i]: X2} " : "   ");
            }
            sb.Append(' ');
            foreach(byte b in line)
            {
                sb.Append(b is >= 32 and < 127 ? (char)b : '.');
            }
            sb.Append('\n');
        }
        return sb.ToString().Trim('\n');
    }
    public static void Write(ConsoleColor color, string text)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ResetColor();
    }
}