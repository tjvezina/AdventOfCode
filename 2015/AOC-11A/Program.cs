using System;

public static class Program {
    private const string INPUT = "vzbxkghb";

    private static void Main(string[] args) {
        Password password = new Password(INPUT);

        do {
            password.Increment();
        } while (!password.IsValid());

        Console.WriteLine($"Next valid password: {password}");
    }
}
