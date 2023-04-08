namespace ProactiveOptimizationForLoops
{
    public class User
    {
        private readonly string _fullName;

        public User(string name)
        {
            _fullName = name;
        }

        public string GetFullName()
        {
            return _fullName;
        }
    }
}