namespace sustAInableEducation_backend.Repository
{
    public class Message
    {
        private static readonly string[] ValidRoles = { "system", "user", "assitant" };

        private string _role = null!;
        public string role
        {
            get => _role;
            set
            {
                if (!ValidRoles.Contains(value))
                {
                    throw new ArgumentException($"Invalid role: {value}. Valid roles are: {string.Join(", ", ValidRoles)}");
                }
                _role = value;
            }
        }

        public string content { get; set; } = null!;
    }
}
