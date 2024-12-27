using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using sustAInableEducation_backend.Models;

namespace sustAInableEducation_backend.Repository
{
    public class AIService : IAIService
    {
        /**
         * Benjamin Edlinger
         */
        private readonly IConfiguration _config;
        private static HttpClient? _client;

        /**
         * Benjamin Edlinger
         */
        public AIService(IConfiguration config)
        {
            _config = config;
            _client = new()
            {
                BaseAddress = new Uri(_config["deepinfra:url"] ?? throw new ArgumentNullException("deepinfra:url configuration is missing"))
            };
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_config["deepinfra:api_key"] ?? throw new ArgumentNullException("deepinfra:api_key configuration is missing")}");
        }

        /**
         * Benjamin Edlinger
         */
        public async Task<StoryPart> StartStory(Story story)
        {
            if (story == null) throw new ArgumentNullException("Story is null");

            var chatMessages = RebuildChatMessages(story);
            var response = await FetchStoryPartAsync(chatMessages, story.Temperature, story.TopP);
            return response.Item1;
        }

        /**
         * Benjamin Edlinger
         */
        public async Task<StoryPart> GenerateNextPart(Story story)
        {
            if (story == null) throw new ArgumentNullException("Story is null");

            var chatMessages = RebuildChatMessages(story);
            var response = await FetchStoryPartAsync(chatMessages, story.Temperature, story.TopP);
            return response.Item1;
        }

        /**
         * Benjamin Edlinger
         */
        public Task<StoryPart> GenerateResult(Story story)
        {
            if (story == null) throw new ArgumentNullException("Story is null");

            Thread.Sleep(2000);
            Console.WriteLine("Story is complete --> Generating Result...");
            Thread.Sleep(2000);

            throw new NotImplementedException();
        }

        /**
         * Benjamin Edlinger
         */
        private List<ChatMessage> RebuildChatMessages(Story story)
        {
            if (story == null) throw new ArgumentNullException("Story is null");
            if (story.Length == 0) throw new ArgumentException("Story has set no length");
            if (story.Prompt == null) throw new ArgumentException("Story has no prompt");

            var chatMessages = new List<ChatMessage>
            {
                new ChatMessage { Role = ValidRoles.System, Content = story.Prompt },
                new ChatMessage { Role = ValidRoles.User, Content = "Alle Teilnehmer sind bereit, beginne mit dem ersten Teil der Geschichte." }
            };

            foreach (var part in story.Parts)
            {
                var assitentContent = new StoryContent
                {
                    // Title = story.Title ?? throw new ArgumentNullException("Story has no title"),
                    Title = "Title", // #TODO: Title in StoryPart.cs hinzufügen
                    // Intertitle = part.Intertitle ?? throw new ArgumentNullException("Part has no intertitle"),
                    Intertitle = "Intertitle", // #TODO: Intertitle in StoryPart.cs hinzufügen
                    Story = part.Text,
                    Options = part.Choices.Select(choice => new Option
                    {
                        ImpactString = choice.Impact.ToString(CultureInfo.InvariantCulture),
                        Text = choice.Text
                    }).ToList()
                };
                chatMessages.Add(new ChatMessage { Role = ValidRoles.Assistant, Content = JsonSerializer.Serialize(assitentContent) });

                if (!part.ChosenNumber.HasValue || part.ChosenNumber < 1 || part.ChosenNumber > 4)
                {
                    throw new ArgumentException("Story part has invalid choice number");
                }
                else if (story.Parts.Count >= story.Length)
                {
                    throw new NotImplementedException("Prompts for generating a result is not implemented");
                }
                else
                {
                    chatMessages.Add(new ChatMessage { Role = ValidRoles.User, Content = $"Die Option {part.ChosenNumber} wurde gewählt. Führe die Geschichte mit dieser Option weiter fort, bis zum nächsten Entscheidungspunkt." });
                }
            }

            return chatMessages;
        }

        /**
         * Benjamin Edlinger
         */
        private async Task<(StoryPart, string)> FetchStoryPartAsync(List<ChatMessage> chatMessages, float temperature, float topP)
        {
            if (_client == null) throw new InvalidOperationException("Client is null");
            if (chatMessages.Count == 0) throw new ArgumentException("No messages to send");
            if (temperature < 0 || temperature > 1) throw new ArgumentException("Invalid temperature");
            if (topP < 0 || topP > 1) throw new ArgumentException("Invalid topP");

            var request = new HttpRequestMessage(HttpMethod.Post, "/v1/openai/chat/completions")
            {
                Content = new StringContent(JsonSerializer.Serialize(new
                {
                    model = "meta-llama/Llama-3.3-70B-Instruct",
                    messages = chatMessages,
                    response_format = new { type = "json_object" },
                    temperature,
                    top_p = topP
                }), Encoding.UTF8, "application/json")
            };

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();

            var responseObject = JsonSerializer.Deserialize<Response>(responseContent) ?? throw new InvalidOperationException("Response is null");
            var assistantContent = responseObject.Choices[0].Message.Content ?? throw new InvalidOperationException("Assistant content is null or empty");
            var messageContent = JsonSerializer.Deserialize<StoryContent>(assistantContent) ?? throw new InvalidOperationException("Message content is null");

            var storyPart = new StoryPart
            {
                Text = messageContent.Story,
                // Intertitle = messageContent.Intertitle, #TODO: Intertitle in StoryPart.cs hinzufügen
                Choices = messageContent.Options.Select((option, index) => new StoryChoice
                {
                    Text = option.Text,
                    Number = index + 1,
                    Impact = option.Impact
                }).ToList()
            };

            return (storyPart, messageContent.Title);
        }

        public Task<Quiz> GenerateQuiz(Story story, QuizRequest config)
        {
            throw new NotImplementedException();
        }
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
    public class ChatMessage
    {
        private string _role = null!;

        [JsonPropertyName("role")]
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

        [JsonPropertyName("content")]
        public string Content { get; set; } = null!;
    }

    /**
     * Benjamin Edlinger
     */
    public class Response
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = null!;

        [JsonPropertyName("object")]
        public string Object { get; set; } = null!;

        [JsonPropertyName("created")]
        public long Created { get; set; }

        [JsonPropertyName("model")]
        public string Model { get; set; } = null!;

        [JsonPropertyName("choices")]
        public List<Choice> Choices { get; set; } = null!;

        [JsonPropertyName("usage")]
        public Usage Usage { get; set; } = null!;
    }

    /**
     * Benjamin Edlinger
     */
    public class Choice
    {
        [JsonPropertyName("index")]
        public int Index { get; set; }

        [JsonPropertyName("message")]
        public Message Message { get; set; } = null!;

        [JsonPropertyName("finish_reason")]
        public string FinishReason { get; set; } = null!;
    }

    /**
     * Benjamin Edlinger
     */
    public class Message
    {
        private string _role = null!;

        [JsonPropertyName("role")]
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

        [JsonPropertyName("content")]
        public string Content { get; set; } = null!;
    }

    /**
     * Benjamin Edlinger
     */
    public class StoryContent
    {
        [JsonPropertyName("title")]
        public string Title { get; set; } = null!;

        [JsonPropertyName("intertitle")]
        public string Intertitle { get; set; } = null!;

        [JsonPropertyName("story")]
        public string Story { get; set; } = null!;

        [JsonPropertyName("options")]
        public List<Option> Options { get; set; } = null!;
    }

    /**
     * Benjamin Edlinger
     */
    public class Option
    {
        [JsonPropertyName("impact")]
        public string ImpactString { get; set; } = null!;

        [JsonIgnore]
        public float Impact => float.Parse(ImpactString, CultureInfo.InvariantCulture);

        [JsonPropertyName("text")]
        public string Text { get; set; } = null!;
    }

    /**
     * Benjamin Edlinger
     */
    public class Usage
    {
        [JsonPropertyName("prompt_tokens")]
        public int PromptTokens { get; set; }

        [JsonPropertyName("completion_tokens")]
        public int CompletionTokens { get; set; }

        [JsonPropertyName("total_tokens")]
        public int TotalTokens { get; set; }

        [JsonPropertyName("estimated_cost")]
        public double EstimatedCost { get; set; }
    }
}
