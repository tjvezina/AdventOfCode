namespace AdventOfCode.Year2015.Day11
{
    public class Challenge : BaseChallenge
    {
        private const string Input = "vzbxkghb";

        private readonly Password _password = new Password(Input);

        public override object part1ExpectedAnswer => "vzbxxyzz";
        public override (string message, object answer) SolvePart1()
        {
            NextValidPassword();
            return ("Next valid password: ", _password);
        }
        
        public override object part2ExpectedAnswer => "vzcaabcc";
        public override (string message, object answer) SolvePart2()
        {
            NextValidPassword();
            return ("Next valid password: ", _password);
        }

        private void NextValidPassword()
        {
            do
            {
                _password.Increment();
            } while (!_password.IsValid());
        }
    }
}
