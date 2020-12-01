using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class DelayedWriter : TextWriter
{
    private struct WriteData
    {
        public string value;
        public ConsoleColor foreground;
        public ConsoleColor background;
    }

    public override Encoding Encoding => Encoding.GetEncoding(850);

    public bool isEmpty => queue.Count == 0;

    private Queue<WriteData> queue = new Queue<WriteData>();

    public override void Write(string value)
    {
        queue.Enqueue(new WriteData
        {
            value = value,
            foreground = Console.ForegroundColor,
            background = Console.BackgroundColor
        });
    }

    public override void WriteLine(string value) => Write(value + NewLine);

    public override void Flush()
    {
        while (queue.Count > 0)
        {
            WriteData data = queue.Dequeue();
            Console.ForegroundColor = data.foreground;
            Console.BackgroundColor = data.background;
            Console.Write(data.value);
        }
    }

    public override void Write(bool value) => Write($"{value}");
    public override void Write(char value) => Write($"{value}");
    public override void Write(char[] buffer, int index, int count) => Write(new string(buffer).Substring(index, count));
    public override void Write(char[] buffer) => Write(new string(buffer));
    public override void Write(decimal value) => Write($"{value}");
    public override void Write(double value) => Write($"{value}");
    public override void Write(float value) => Write($"{value}");
    public override void Write(int value) => Write($"{value}");
    public override void Write(long value) => Write($"{value}");
    public override void Write(object value) => Write($"{value}");
    public override void Write(ReadOnlySpan<char> buffer) => Write(new string(buffer));
    public override void Write(string format, object arg0) => Write(string.Format(format, arg0));
    public override void Write(string format, object arg0, object arg1) => Write(string.Format(format, arg0, arg1));
    public override void Write(string format, object arg0, object arg1, object arg2) => Write(string.Format(format, arg0, arg1, arg2));
    public override void Write(string format, params object[] arg) => Write(string.Format(format, arg));
    public override void Write(StringBuilder value) => Write($"{value}");
    public override void Write(uint value) => Write($"{value}");
    public override void Write(ulong value) => Write($"{value}");

    public override void WriteLine() => WriteLine(string.Empty);
    public override void WriteLine(bool value) => WriteLine($"{value}");
    public override void WriteLine(char value) => WriteLine($"{value}");
    public override void WriteLine(char[] buffer, int index, int count) => WriteLine(new string(buffer).Substring(index, count));
    public override void WriteLine(char[] buffer) => WriteLine(new string(buffer));
    public override void WriteLine(decimal value) => WriteLine($"{value}");
    public override void WriteLine(double value) => WriteLine($"{value}");
    public override void WriteLine(float value) => WriteLine($"{value}");
    public override void WriteLine(int value) => WriteLine($"{value}");
    public override void WriteLine(long value) => WriteLine($"{value}");
    public override void WriteLine(object value) => WriteLine($"{value}");
    public override void WriteLine(ReadOnlySpan<char> buffer) => WriteLine(new string(buffer));
    public override void WriteLine(string format, object arg0) => WriteLine(string.Format(format, arg0));
    public override void WriteLine(string format, object arg0, object arg1) => WriteLine(string.Format(format, arg0, arg1));
    public override void WriteLine(string format, object arg0, object arg1, object arg2) => WriteLine(string.Format(format, arg0, arg1, arg2));
    public override void WriteLine(string format, params object[] arg) => WriteLine(string.Format(format, arg));
    public override void WriteLine(StringBuilder value) => WriteLine($"{value}");
    public override void WriteLine(uint value) => WriteLine($"{value}");
    public override void WriteLine(ulong value) => WriteLine($"{value}");
}
