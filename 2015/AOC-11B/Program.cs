using System;

public static class Program {
    private const string INPUT = "vzbxkghb";

    private static Password _password;

    private static void Main(string[] args) {
        _password = new Password(INPUT);

        GetNextPassword();
        GetNextPassword();
    }

    private static void GetNextPassword() {
        do {
            _password.Increment();
        } while (!_password.IsValid());

        Console.WriteLine($"Next valid password: {_password}");
    }
}
