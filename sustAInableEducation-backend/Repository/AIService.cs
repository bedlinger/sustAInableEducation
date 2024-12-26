using sustAInableEducation_backend.Models;

namespace sustAInableEducation_backend.Repository
{
    public class AIService : IAIService
    {
        private readonly IConfiguration _config;
        private ICollection<Message> _messages { get; set; } = new List<Message>();
        private static HttpClient _client;

        public AIService(IConfiguration config)
        {
            _config = config;
            var baseUrl = _config["deepinfra:url"] ?? throw new ArgumentNullException("deepinfra:url configuration is missing");
            var apiKey = _config["deepinfra:api_key"] ?? throw new ArgumentNullException("deepinfra:api_key configuration is missing");
            _client = new()
            {
                BaseAddress = new Uri(baseUrl)
            };
            _client.DefaultRequestHeaders.Add("Content-Type", "application/json");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
        }

        /**
         * Benjamin Edlinger
         */
        public Task<StoryPart> StartStory(Story story)
        {
            _messages.Add(new Message
            {
                role = ValidRoles.System,
                content = story.Prompt
            });
            _messages.Add(new Message
            {
                role = ValidRoles.User,
                content = "Once upon a time..."
            });
            throw new NotImplementedException();
        }

        /**
         * Benjamin Edlinger
         */
        public Task<StoryPart> GenerateNextPart(Story story)
        {
            throw new NotImplementedException();
        }

        /**
         * Benjamin Edlinger
         */
        public Task<StoryPart> GenerateResult(Story story)
        {
            throw new NotImplementedException();
        }

        /**
         * Benjamin Edlinger
         */
        public async Task<StoryPart> PostAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Quiz> GenerateQuiz(Story story, QuizRequest config)
        {
            throw new NotImplementedException();
        }
    }

    /**
     * Benjamin Edlinger
     */
    public class Message
    {
        private string _role = null!;
        public string role
        {
            get => _role;
            set
            {
                if (value != ValidRoles.System && value != ValidRoles.User && value != ValidRoles.Assistant)
                {
                    throw new ArgumentException("Invalid role");
                }
                _role = value;
            }
        }

        public string content { get; set; } = null!;
    }

    /**
     * Benjamin Edlinger
     */
    public static class ValidRoles
    {
        public const string System = "system";
        public const string User = "user";
        public const string Assistant = "assistant";
    }
}
