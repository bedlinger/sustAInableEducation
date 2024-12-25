using sustAInableEducation_backend.Models;

namespace sustAInableEducation_backend.Repository
{
    public class AIService : IAIService
    {
        private ICollection<Message> _messages { get; set; } = new List<Message>();

        /**
         * Benjamin Edlinger
         */
        public Task<StoryPart> StartStory(Story story)
        {
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
