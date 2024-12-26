using System.Text;
using System.Text.Json;
using sustAInableEducation_backend.Models;

namespace sustAInableEducation_backend.Repository
{
    public class AIService : IAIService
    {
        private readonly IConfiguration _config;
        private List<ChatMessage> _chatMessages { get; set; } = new List<ChatMessage>();
        private static HttpClient? _client;

        /**
         * Benjamin Edlinger
         */
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
        public async Task<StoryPart> StartStory(Story story)
        {
            _chatMessages.Add(new ChatMessage
            {
                Role = ValidRoles.System,
                Content = story.Prompt
            });
            _chatMessages.Add(new ChatMessage
            {
                Role = ValidRoles.User,
                Content = "Alle Teilnehmer sind bereit, beginne mit dem ersten Teil der Geschichte."
            });
            var response = await PostAsync(story.Temperature, story.TopP);
            return response.Item1;
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
        private async Task<(StoryPart, string)> PostAsync(float temprature, float topP)
        {
            if (_chatMessages.Count == 0)
            {
                throw new ArgumentException("No messages to send");
            }
            if (temprature < 0 || temprature > 1)
            {
                throw new ArgumentException("Invalid temperature");
            }
            if (topP < 0 || topP > 1)
            {
                throw new ArgumentException("Invalid topP");
            }
            using StringContent jsonData = new(
                JsonSerializer.Serialize(new
                {
                    model = "meta-llama/Llama-3.3-70B-Instruct",
                    messages = _chatMessages,
                    response_format = new
                    {
                        type = "json_object"
                    },
                    temprature,
                    top_p = topP
                }),
                    Encoding.UTF8,
                    "application/json"
                    );

            var response = await _client.PostAsync("v1/openai/chat/completions", jsonData);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            var responseObject = JsonSerializer.Deserialize<Response>(responseContent);

            if (responseObject?.Data?.Choices == null || responseObject.Data.Choices.Count == 0)
            {
                throw new InvalidOperationException("Invalid response from the server");
            }

            var title = responseObject.Data.Choices[0].Message.Content.Title;
            var storyPart = new StoryPart
            {
                Text = responseObject.Data.Choices[0].Message.Content.Story,
                Choices = new List<StoryChoice>()
                {
                    new StoryChoice
                    {
                        Text = responseObject.Data.Choices[0].Message.Content.Options[0].Text,
                        Number = 1,
                        Impact = responseObject.Data.Choices[0].Message.Content.Options[0].Impact
                    },
                    new StoryChoice
                    {
                        Text = responseObject.Data.Choices[0].Message.Content.Options[1].Text,
                        Number = 2,
                        Impact = responseObject.Data.Choices[0].Message.Content.Options[1].Impact
                    },
                    new StoryChoice
                    {
                        Text = responseObject.Data.Choices[0].Message.Content.Options[2].Text,
                        Number = 3,
                        Impact = responseObject.Data.Choices[0].Message.Content.Options[2].Impact
                    },
                    new StoryChoice
                    {
                        Text = responseObject.Data.Choices[0].Message.Content.Options[3].Text,
                        Number = 4,
                        Impact = responseObject.Data.Choices[0].Message.Content.Options[3].Impact
                    }
                }
            };
            _chatMessages.Add(new ChatMessage
            {
                Role = ValidRoles.Assistant,
                Content = responseObject?.Data?.Choices[0]?.Message?.ToString() ?? string.Empty
            });
            return (storyPart, title);
        }

        public Task<Quiz> GenerateQuiz(Story story, QuizRequest config)
        {
            throw new NotImplementedException();
        }
    }

    /**
     * Benjamin Edlinger
     */
    public class ChatMessage
    {
        private string _role = null!;
        public string Role
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
        public string Content { get; set; } = null!;
    }

    /**
     * Benjamin Edlinger
     */
    public class Response
    {
        public Data Data { get; set; } = null!;
    }

    /**
     * Benjamin Edlinger
     */
    public class Data
    {
        public List<Choice> Choices { get; set; } = null!;
    }

    /**
     * Benjamin Edlinger
     */
    public class Choice
    {
        public int Index { get; set; }
        public Message Message { get; set; } = null!;
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

    /**
     * Benjamin Edlinger
     */
    public class Message
    {
        private string _role = null!;
        public string Role
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
        public Content Content { get; set; } = null!;
    }

    /**
     * Benjamin Edlinger
     */
    public class Content
    {
        public string Title { get; set; } = null!;
        public string Intertitle { get; set; } = null!;
        public string Story { get; set; } = null!;
        public List<Option> Options { get; set; } = null!;

    }

    /**
     * Benjamin Edlinger
     */
    public class Option
    {
        public float Impact { get; set; }
        public string Text { get; set; } = null!;
    }
}
