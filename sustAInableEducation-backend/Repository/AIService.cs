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
                BaseAddress = new Uri(_config["DeepInfra:Url"] ?? throw new ArgumentNullException("DeepInfra:Url configuration is missing")),
                Timeout = TimeSpan.FromMinutes(5)
            };
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_config["DeepInfra:ApiKey"] ?? throw new ArgumentNullException("DeepInfra:ApiKey configuration is missing")}");
        }

        /**
         * Benjamin Edlinger
         */
        public async Task<(StoryPart, string)> StartStory(Story story)
        {
            ArgumentNullException.ThrowIfNull(story);

            var chatMessages = RebuildChatMessages(story);
            var assistantContent = await FetchAssitantContent(chatMessages, story.Temperature, story.TopP);
            return GetStoryPart(assistantContent);
        }

        /**
         * Benjamin Edlinger
         */
        public async Task<StoryPart> GenerateNextPart(Story story)
        {
            ArgumentNullException.ThrowIfNull(story);

            var chatMessages = RebuildChatMessages(story);
            var assistantContent = await FetchAssitantContent(chatMessages, story.Temperature, story.TopP);
            return GetStoryPart(assistantContent).Item1;
        }

        /**
         * Benjamin Edlinger
         */
        public async Task<StoryResult> GenerateResult(Story story)
        {
            ArgumentNullException.ThrowIfNull(story);

            var chatMessages = RebuildChatMessages(story);
            var assistantContent = await FetchAssitantContent(chatMessages, story.Temperature, story.TopP);
            var endPart = GetStoryPart(assistantContent).Item1;
            var end = endPart.Text;
            chatMessages.Add(new ChatMessage { Role = ValidRoles.Assistant, Content = assistantContent });
            chatMessages.Add(new ChatMessage { Role = ValidRoles.System, Content = "Du schlüpfst in die Rolle einer Lehrkraft, welche die durchlebte Geschichte mit den Teilnehmern bespricht. Deine Aufgabe besteht nicht darin, die Handlung der Geschichte selbst zu analysieren, sondern das nachhaltige Thema zu beleuchten, das die Geschichte behandelt. Präsentiere die zentralen Aspekte faktenbasiert und leicht verständlich, um den Teilnehmern einen klaren Zugang zum Thema zu ermöglichen. Gleichzeitig sollst du die Teilnehmer dazu anregen, ihr eigenes Handeln und ihre Einstellungen kritisch zu hinterfragen. Schaffe Raum für eine offene und respektvolle Diskussion, in der unterschiedliche Perspektiven ausgetauscht werden können. Stelle gezielte Fragen, die zum Nachdenken anregen, und nutze klare Erklärungen sowie passende Beispiele, um komplexe Zusammenhänge greifbar zu machen. Die folgenden Inhalte sollen alle Teil deiner Analyse sein: - Erstelle eine umfassende Analyse der Geschichte, die sich aus mehreren klar strukturierten Teilen zusammensetzt. Beginne mit einer kurzen und prägnanten Zusammenfassung der Geschichte, die den Verlauf verständlich darstellt und die zentralen Ereignisse hervorhebt. Anschließend analysiere den Verlauf und arbeite heraus, wie sich die Entscheidungen und Handlungen der Figuren auf den Verlauf ausgewirkt haben. - Erstelle danach eine Liste mit positiven Entscheidungen, die innerhalb der Geschichte getroffen wurden. Erkläre zu jeder Entscheidung, warum sie sich positiv auf den Verlauf ausgewirkt hat und welche konkreten Vorteile daraus entstanden sind. Im Anschluss folgt eine Liste mit negativen Entscheidungen, die getroffen wurden. Erkläre hier ebenfalls, warum diese Entscheidungen negative Konsequenzen hatten und wie sie den Verlauf der Geschichte beeinflusst haben. - Ziehe daraus abgeleitete Erkenntnisse und übertrage sie auf die reale Welt. Erstelle eine Liste von praktischen Lehren, die aus der Geschichte gezogen werden können, und zeige auf, wie diese Erkenntnisse im Alltag oder in konkreten Situationen angewendet werden könnten. - Abschließend präsentiere eine Liste mit gezielten Fragen, die als Grundlage für eine tiefere Diskussion in der Gruppe dienen sollen. Diese Fragen sollten sowohl zum Nachdenken anregen als auch Raum für unterschiedliche Perspektiven schaffen und eine lebendige Diskussion ermöglichen. Antworte ausschließlich im gültigen JSON-Format, um sicherzustellen, dass deine Analyse korrekt dargestellt wird. Jede Antwort folgt exakt dieser Struktur: {\"summary\": \"Zusammenfassung und Analyse der Geschichte als Fließtext\",\"positive_choices\": [\"Beschreibung der positiven Entscheidung 1\",\"Weitere positive Entscheidungen je nach Bedarf\"],\"negative_choices\": [\"Beschreibung der negativen Entscheidung 1\",\"Weitere negative Entscheidungen je nach Bedarf\"],\"learnings\": [\"Erkenntnis 1\",\"Weitere Erkenntnisse je nach Bedarf\"],\"discussion_questions\": [\"Frage 1\",\"Weitere Fragen je nach Bedarf\"]}" });
            chatMessages.Add(new ChatMessage { Role = ValidRoles.User, Content = "Die Geschichte ist soeben vorbei. Du kannst jetzt die Analyse der durchlebten Geschichte erstellen." });
            assistantContent = await FetchAssitantContent(chatMessages, story.Temperature, story.TopP);

            return GetStoryResult(assistantContent, end);
        }

        /**
         * Benjamin Edlinger
         */
        private static List<ChatMessage> RebuildChatMessages(Story story)
        {
            ArgumentNullException.ThrowIfNull(story);
            if (story.Length == 0) throw new ArgumentException("Story has set no length");

            var chatMessages = new List<ChatMessage>
            {
                new() { Role = ValidRoles.System, Content = story.Prompt },
                new() { Role = ValidRoles.User, Content = "Alle Teilnehmer sind bereit, beginne mit dem ersten Teil der Geschichte." }
            };

            foreach (var part in story.Parts.Select((value, index) => new { value, index }))
            {
                var assitentContent = new StoryContent
                {
                    Title = story.Title ?? throw new ArgumentNullException("Story has no title"),
                    Intertitle = part.value.Intertitle ?? throw new ArgumentNullException("Part has no intertitle"),
                    Story = part.value.Text,
                    Options = part.value.Choices.Select(choice => new Option
                    {
                        ImpactString = choice.Impact.ToString(CultureInfo.InvariantCulture),
                        Text = choice.Text
                    }).ToList()
                };
                chatMessages.Add(new ChatMessage { Role = ValidRoles.Assistant, Content = JsonSerializer.Serialize(assitentContent) });

                if (!part.value.ChosenNumber.HasValue || part.value.ChosenNumber < 1 || part.value.ChosenNumber > 4)
                {
                    throw new ArgumentException("Story part has invalid choice number");
                }
                else if (story.Length == part.index + 1)
                {
                    chatMessages.Add(new ChatMessage { Role = ValidRoles.User, Content = $"Die Option {part.value.ChosenNumber} wurde gewählt. Führe die Geschichte mit dieser Option weiter fort, nachdem es der letzte Entscheidungspunkt war, kommt nun der Schluss der Geschichte." });
                }
                else
                {
                    chatMessages.Add(new ChatMessage { Role = ValidRoles.User, Content = $"Die Option {part.value.ChosenNumber} wurde gewählt. Führe die Geschichte mit dieser Option weiter fort, bis zum nächsten Entscheidungspunkt." });
                }
            }

            return chatMessages;
        }

        /**
         * Benjamin Edlinger
         */
        private static (StoryPart, string) GetStoryPart(string assistantContent)
        {
            var messageContent = JsonSerializer.Deserialize<StoryContent>(assistantContent) ?? throw new InvalidOperationException("Message content is null");

            var storyPart = new StoryPart
            {
                Text = messageContent.Story,
                Intertitle = messageContent.Intertitle,
                Choices = messageContent.Options.Select((option, index) => new StoryChoice
                {
                    Text = option.Text,
                    Number = index + 1,
                    Impact = option.Impact
                }).ToList()
            };

            return (storyPart, messageContent.Title);
        }

        /**
         * Benjamin Edlinger
         */
        private static StoryResult GetStoryResult(string assistantContent, string end)
        {
            var messageContent = JsonSerializer.Deserialize<AnalysisContent>(assistantContent) ?? throw new InvalidOperationException("Message content is null");
            return new StoryResult
            {
                Text = end,
                Summary = messageContent.Summary,
                PositiveChoices = messageContent.PositiveChoices,
                NegativeChoices = messageContent.NegativeChoices,
                Learnings = messageContent.Learnings,
                DiscussionQuestions = messageContent.DiscussionQuestions
            };
        }

        /**
         * Benjamin Edlinger
         */
        private static async Task<string> FetchAssitantContent(List<ChatMessage> chatMessages, float temperature, float topP)
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
            return responseObject.Choices[0].Message.Content ?? throw new InvalidOperationException("Assistant content is null or empty");
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
    public class AnalysisContent
    {
        [JsonPropertyName("summary")]
        public string Summary { get; set; } = null!;

        [JsonPropertyName("positive_choices")]
        public string[] PositiveChoices { get; set; } = null!;

        [JsonPropertyName("negative_choices")]
        public string[] NegativeChoices { get; set; } = null!;

        [JsonPropertyName("learnings")]
        public string[] Learnings { get; set; } = null!;

        [JsonPropertyName("discussion_questions")]
        public string[] DiscussionQuestions { get; set; } = null!;
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
