using System.Diagnostics;

public class Password {
    public const int LENGTH = 8;

    private string _data;

    public Password(string data) {
        Debug.Assert(data.Length == LENGTH, $"Passwords must be {LENGTH} characters long");

        _data = data;
    }

    public void Increment() => IncrementChar(LENGTH - 1);

    private void IncrementChar(int index) {
        if (index == -1) {
            return;
        }

        char c = _data[index];
        if (++c > 'z') {
            c = 'a';
            IncrementChar(index - 1);
        }

        _data = _data.Remove(index, 1).Insert(index, $"{c}");
    }

    public bool IsValid() => CheckRule1() && CheckRule2() && CheckRule3();

    private bool CheckRule1() {
        int straight = 1;

        char prev = _data[0];
        for (int i = 1; i < LENGTH; ++i) {
            char next = _data[i];

            if (prev + 1 == next) {
                if (++straight == 3) {
                    return true;
                }
            } else {
                straight = 1;
            }

            prev = next;
        }

        return false;
    }

    private bool CheckRule2() {
        return !_data.Contains("i")
            && !_data.Contains("o")
            && !_data.Contains("l");
    }

    private bool CheckRule3() {
        int pairs = 0;

        char prev = _data[0];
        for (int i = 1; i < LENGTH; ++i) {
            char next = _data[i];

            if (prev == next) {
                if (++pairs == 2) {
                    return true;
                }

                // Skip this pair (other pairs can't overlap this one)
                if (++i < LENGTH) {
                    next = _data[i];
                }
            }

            prev = next;
        }

        return false;
    }

    public override string ToString() => _data;
}
