﻿using System.Drawing;
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using sustAInableEducation_backend.Models;

namespace sustAInableEducation_backend.Repository
{
    public class AIService : IAIService
    {
        // Benjamin Edlinger
        private readonly IConfiguration _config;
        private static HttpClient? _client;
        const int MAX_RETRY_ATTEMPTS = 2; // Maximum number of retry attempts for a failed request or deserialization

        // Benjamin Edlinger
        public AIService(IConfiguration config)
        {
            ArgumentNullException.ThrowIfNull(config);
            _config = config;
            _client = new()
            {
                BaseAddress = new Uri(_config["DeepInfra:Url"] ?? throw new ArgumentNullException("DeepInfra:Url configuration is missing")),
                Timeout = TimeSpan.FromMinutes(5)
            };
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_config["DeepInfra:ApiKey"] ?? throw new ArgumentNullException("DeepInfra:ApiKey configuration is missing")}");
        }

        // Benjamin Edlinger
        /// <summary>
        /// Starts a new story with the given story object
        /// </summary>
        /// <param name="story"> The story object to start</param>
        /// <returns>The first part of the story and the title of the story</returns>
        /// <exception cref="ArgumentException">If the story object is invalid</exception>
        /// <exception cref="AIException">If the story could not be started due to an error while fetching the content or deserializing</exception>
        public async Task<(StoryPart, string)> StartStory(Story story)
        {
            ArgumentNullException.ThrowIfNull(story);

            List<ChatMessage> chatMessages;
            try
            {
                chatMessages = RebuildChatMessagesStory(story);
            }
            catch (Exception e)
            {
                throw new ArgumentException("Failed to rebuild chat messages because of error in story object", e);
            }

            string assistantContent = null!;
            int attempt = 0;
            while (attempt < MAX_RETRY_ATTEMPTS)
            {
                try
                {
                    assistantContent = await FetchAssitantContent(chatMessages, story.Temperature, story.TopP);
                    break;
                }
                catch (Exception e)
                {
                    if (attempt >= MAX_RETRY_ATTEMPTS - 1)
                    {
                        throw new AIException("Failed to start story", e);
                    }
                    attempt++;
                }
            }

            attempt = 0;
            while (attempt < MAX_RETRY_ATTEMPTS)
            {
                try
                {
                    var storyPart = GetStoryPart(assistantContent);
                    storyPart.Item1.Image = await FetchStoryImage(storyPart.Item1.Text);
                    return storyPart;
                }
                catch (Exception e)
                {
                    if (attempt >= MAX_RETRY_ATTEMPTS - 1)
                    {
                        throw new AIException("Failed to start story", e);
                    }
                    attempt++;
                }
            }

            throw new AIException("Failed to start story after maximum retry attempts");
        }

        // Benjamin Edlinger
        /// <summary>
        /// Generates the next part of the story based on the given story object
        /// </summary>
        /// <param name="story">The story object to generate the next part for</param>
        /// <returns>The next part of the story</returns>
        /// <exception cref="ArgumentException">If the story object is invalid</exception>
        /// <exception cref="AIException">If the next part could not be generated due to an error while fetching the content or deserializing</exception>
        public async Task<StoryPart> GenerateNextPart(Story story)
        {
            ArgumentNullException.ThrowIfNull(story);

            List<ChatMessage> chatMessages;
            try
            {
                chatMessages = RebuildChatMessagesStory(story);
            }
            catch (Exception e)
            {
                throw new ArgumentException("Failed to rebuild chat messages because of error in story object", e);
            }

            string assistantContent = null!;
            int attempt = 0;
            while (attempt < MAX_RETRY_ATTEMPTS)
            {
                try
                {
                    assistantContent = await FetchAssitantContent(chatMessages, story.Temperature, story.TopP);
                    break;
                }
                catch (Exception e)
                {
                    if (attempt >= MAX_RETRY_ATTEMPTS - 1)
                    {
                        throw new AIException("Failed to generate next part", e);
                    }
                    attempt++;
                }
            }

            attempt = 0;
            while (attempt < MAX_RETRY_ATTEMPTS)
            {
                try
                {
                    StoryPart storyPart = GetStoryPart(assistantContent).Item1;
                    storyPart.Image = await FetchStoryImage(storyPart.Text);
                    return storyPart;
                }
                catch (Exception e)
                {
                    if (attempt >= MAX_RETRY_ATTEMPTS - 1)
                    {
                        throw new AIException("Failed to generate next part", e);
                    }
                    attempt++;
                }
            }

            throw new AIException("Failed to generate next part after maximum retry attempts");
        }

        // Benjamin Edlinger
        /// <summary>
        /// Generates the result of the story based on the given story object
        /// </summary>
        /// <param name="story">The story object to generate the result for</param>
        /// <returns>The result of the story</returns>
        /// <exception cref="ArgumentException">If the story object is invalid</exception>
        /// <exception cref="AIException">If the result could not be generated due to an error while fetching the content or deserializing</exception>
        public async Task<StoryResult> GenerateResult(Story story)
        {
            ArgumentNullException.ThrowIfNull(story);

            List<ChatMessage> chatMessages;
            try
            {
                chatMessages = RebuildChatMessagesStory(story);
            }
            catch (Exception e)
            {
                throw new ArgumentException("Failed to rebuild chat messages because of error in story object", e);
            }

            string assistantContent = null!;
            int attempt = 0;
            while (attempt < MAX_RETRY_ATTEMPTS)
            {
                try
                {
                    assistantContent = await FetchAssitantContent(chatMessages, story.Temperature, story.TopP);
                    break;
                }
                catch (Exception e)
                {
                    if (attempt >= MAX_RETRY_ATTEMPTS - 1)
                    {
                        throw new AIException("Failed to generate result", e);
                    }
                    attempt++;
                }
            }

            StoryPart endPart = null!;
            attempt = 0;
            while (attempt < MAX_RETRY_ATTEMPTS)
            {
                try
                {
                    endPart = GetStoryPart(assistantContent).Item1;
                    break;
                }
                catch (Exception e)
                {
                    if (attempt >= MAX_RETRY_ATTEMPTS - 1)
                    {
                        throw new AIException("Failed to generate result", e);
                    }
                    attempt++;
                }
            }

            string end = endPart.Text;
            try
            {
                chatMessages = RebuildChatMessagesResult(story, chatMessages, end);
            }
            catch (Exception e)
            {
                throw new ArgumentException("Failed to rebuild chat messages", e);
            }

            attempt = 0;
            while (attempt < MAX_RETRY_ATTEMPTS)
            {
                try
                {
                    assistantContent = await FetchAssitantContent(chatMessages, story.Temperature, story.TopP);
                    break;
                }
                catch (Exception e)
                {
                    if (attempt >= MAX_RETRY_ATTEMPTS - 1)
                    {
                        throw new AIException("Failed to generate result", e);
                    }
                    attempt++;
                }
            }

            attempt = 0;
            while (attempt < MAX_RETRY_ATTEMPTS)
            {
                try
                {
                    StoryResult storyResult = GetStoryResult(assistantContent, end);
                    storyResult.Image = await FetchStoryImage(storyResult.Text);
                    return storyResult;
                }
                catch (Exception e)
                {
                    if (attempt >= MAX_RETRY_ATTEMPTS - 1)
                    {
                        throw new AIException("Failed to generate result", e);
                    }
                    attempt++;
                }
            }

            throw new AIException("Failed to generate result after maximum retry attempts");
        }

        string IAIService.GenerateStoryImage(Story story)
        {
            throw new NotImplementedException();
        }

        // Benjamin Edlinger
        /// <summary>
        /// Rebuilds the chat messages of the story for the given story object
        /// </summary>
        /// <param name="story">The story object to rebuild the chat messages for</param>
        /// <returns>The rebuilt chat messages</returns>
        /// <exception cref="ArgumentException">If the story object is invalid</exception>
        /// <exception cref="ArgumentNullException">If the story object is null</exception>
        private static List<ChatMessage> RebuildChatMessagesStory(Story story)
        {
            ArgumentNullException.ThrowIfNull(story);

            string targetGroupString = story.TargetGroup switch
            {
                TargetGroup.PrimarySchool => "Die Teilnehmer, welche deine Geschichte lesen, sind Volksschüler im Alter von 6 bis 10 Jahren. Pass deinen Stil an diese Zielgruppe an und verwende einfache Sprache mit kurzen und klaren Sätzen. Achte darauf, dass die Geschichte einen direkten Bezug auf die Lebenswelt der Schüler hat, damit diese sich leicht in diese versetzen können.",
                TargetGroup.MiddleSchool => "Die Teilnehmer, welche deine Geschichte lesen, sind Schüler der Sekundarstufe eins im Alter von 11 bis 14 Jahren. Pass deinen Stil an diese Zielgruppe und verwende einen passend anspruchsvollen Wortschatz und Satzbau. Die Entscheidungspunkte sollen moralische Dilemmas und praxisnahe Probleme beschreiben.",
                TargetGroup.HighSchool => "Die Teilnehmer, welche deine Geschichte lesen, sind Schüler der Sekundarstufe zwei im Alter von 15 bis 19 Jahren. Pass deinen Stil an diese Zielgruppe an und verwende eine anspruchsvollere Sprache mit komplexeren Satzstrukturen und Fachbegriffen. Die Geschichte soll die globale und wissenschaftliche Relevanz von Nachhaltigkeit aufzeigen. Entscheidungspunkte sollen kritisches Denken und die Analyse verschiedener Perspektiven fördern.",
                _ => throw new ArgumentException("Invalid target group")
            };
            string systemPrompt = "Du bist ein Geschichtenerzähler, welcher eine interaktive und textbasierte Geschichte erstellt. Deine Geschichte ist immersiv, spannend und soll Teilnehmer zum Nachdenken über das Thema Nachhaltigkeit anstoßen. " +
                "Deine Geschichte wird interaktiv über Entscheidungspunkte gesteuert. " +
                $"Deine Geschichte umfasst genau {story.Length} Entscheidungspunkte. " +
                "Entscheidungspunkte sind essenziell, denn sie bestimmen, wie die Geschichte weiter verläuft. Über Entscheidungspunkte stimmt immer eine Gruppe an Teilnehmer ab. Ein Entscheidungspunkt besteht aus vier Optionen, wie die Geschichte verlaufen wird. Nicht jede Option muss einen positiven Einfluss, sondern kann auch einen negativen Einfluss auf die Geschichte haben und soll die Teilnehmer vor diversen Aufgaben und Problemen stellen, welche die Teilnehmer durch ihre Entscheidungen lösen müssen. Der Einfluss einer Option wird mittels eines Werts zwischen -1 (negativer Einfluss) und 1 (positiver Einfluss) angegeben. Eine Option mit negativem Einfluss liegt näher an -1, und eine Option mit positivem Einfluss liegt näher an 1. Die Summe aller Einflüsse der vier Optionen in einen Entscheidungspunkt muss immer 0 ergeben. Wenn du bei einem Entscheidungspunkt in der Geschichte angelangt bist, präsentiere die vier Optionen und warte auf die Entscheidung der Teilnehmer." +
                targetGroupString +
                "Ebenso benötigt jede Geschichte einen Titel. Der Titel deiner Geschichte soll die Teilnehmer fesseln und Lust auf die kommende Geschichte machen, aber achte auch darauf, dass der Titel die Geschichte passend beschreibt. Füge zu jedem Abschnitt einen Zwischentitel hinzu, dieser soll den folgenden Abschnitt beschreiben." +
                story.Topic +
                "Antworte ausschließlich im gültigen JSON-Format, damit deine Antworten den Teilnehmern richtig dargestellt werden können. Jede deiner Antworten hat den identen JSON-Aufbau. Erstens den Titel der Geschichte, dieser bleibt immer gleich und der Zwischentitel des Abschnittes. Darauf folgt die Geschichte, also der Abschnitt der Geschichte, welchen du geschrieben hast. Dann die vier Optionen und jeweiligen Einfluss des Entscheidungspunkts in einem Array. Wenn es der letzte Teil der Geschichte ist, befülle das Array, mit den Optionen, mit leeren Inhalten, welche trotzdem valide sind. Hier ist die JSON-Struktur, welche immer geliefert werden soll: { \"title\": \"Titel der Geschichte\", \"intertitle\": \"Zwischentitel des Abschnitt\", \"story\": \"Aktueller Teil der Geschichte basierend auf den bisherigen Entscheidungen.\", \"options\": [ { \"impact\": \"Wert zwischen -1 und 1\", \"text\": \"Option 1 Beschreibung\" }, { \"impact\": \"Wert zwischen -1 und 1\", \"text\": \"Option 2 Beschreibung\" } , { \"impact\": \"Wert zwischen -1 und 1\", \"text\": \"Option 3 Beschreibung\" } , { \"impact\": \"Wert zwischen -1 und 1\", \"text \": \"Option 4 Beschreibung\" } ] }";

            List<ChatMessage> chatMessages =
            [
                new() { Role = ValidRoles.System, Content = systemPrompt },
                new() { Role = ValidRoles.User, Content = "Alle Teilnehmer sind bereit, beginne mit dem ersten Teil der Geschichte." }
            ];

            foreach (var part in story.Parts.Select((value, index) => new { value, index }))
            {
                StoryContent assitentContent = new()
                {
                    Title = story.Title ?? throw new ArgumentNullException("Story has no title"),
                    Intertitle = part.value.Intertitle,
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

        // Benjamin Edlinger
        /// <summary>
        /// Rebuilds the chat messages of the result for already rebuilt chat messages and the given story object
        /// </summary>
        /// <param name="story">The story object to rebuild the chat messages for</param>
        /// <param name="chatMessages">The already rebuilt chat messages based on the story</param>
        /// <param name="end">The end of the story</param>
        /// <returns>The rebuilt chat messages</returns>
        /// <exception cref="ArgumentNullException">If the story object is null, the chat messages are null or the end is null</exception>
        private static List<ChatMessage> RebuildChatMessagesResult(Story story, List<ChatMessage> chatMessages, string end)
        {
            ArgumentNullException.ThrowIfNull(story);
            ArgumentNullException.ThrowIfNull(chatMessages);
            ArgumentNullException.ThrowIfNull(end);

            chatMessages.Add(new ChatMessage { Role = ValidRoles.Assistant, Content = end });
            chatMessages.Add(new ChatMessage { Role = ValidRoles.System, Content = "Du schlüpfst in die Rolle einer Lehrkraft, welche die durchlebte Geschichte mit den Teilnehmern bespricht. Deine Aufgabe besteht nicht darin, die Handlung der Geschichte selbst zu analysieren, sondern das nachhaltige Thema zu beleuchten, das die Geschichte behandelt. Präsentiere die zentralen Aspekte faktenbasiert und leicht verständlich, um den Teilnehmern einen klaren Zugang zum Thema zu ermöglichen. Gleichzeitig sollst du die Teilnehmer dazu anregen, ihr eigenes Handeln und ihre Einstellungen kritisch zu hinterfragen. Schaffe Raum für eine offene und respektvolle Diskussion, in der unterschiedliche Perspektiven ausgetauscht werden können. Stelle gezielte Fragen, die zum Nachdenken anregen, und nutze klare Erklärungen sowie passende Beispiele, um komplexe Zusammenhänge greifbar zu machen. Die folgenden Inhalte sollen alle Teil deiner Analyse sein: - Erstelle eine umfassende Analyse der Geschichte, die sich aus mehreren klar strukturierten Teilen zusammensetzt. Beginne mit einer kurzen und prägnanten Zusammenfassung der Geschichte, die den Verlauf verständlich darstellt und die zentralen Ereignisse hervorhebt. Anschließend analysiere den Verlauf und arbeite heraus, wie sich die Entscheidungen und Handlungen der Figuren auf den Verlauf ausgewirkt haben. - Erstelle danach eine Liste mit positiven Entscheidungen, die innerhalb der Geschichte getroffen wurden. Erkläre zu jeder Entscheidung, warum sie sich positiv auf den Verlauf ausgewirkt hat und welche konkreten Vorteile daraus entstanden sind. Im Anschluss folgt eine Liste mit negativen Entscheidungen, die getroffen wurden. Erkläre hier ebenfalls, warum diese Entscheidungen negative Konsequenzen hatten und wie sie den Verlauf der Geschichte beeinflusst haben. - Ziehe daraus abgeleitete Erkenntnisse und übertrage sie auf die reale Welt. Erstelle eine Liste von praktischen Lehren, die aus der Geschichte gezogen werden können, und zeige auf, wie diese Erkenntnisse im Alltag oder in konkreten Situationen angewendet werden könnten. - Abschließend präsentiere eine Liste mit gezielten Fragen, die als Grundlage für eine tiefere Diskussion in der Gruppe dienen sollen. Diese Fragen sollten sowohl zum Nachdenken anregen als auch Raum für unterschiedliche Perspektiven schaffen und eine lebendige Diskussion ermöglichen. Antworte ausschließlich im gültigen JSON-Format, um sicherzustellen, dass deine Analyse korrekt dargestellt wird. Jede Antwort folgt exakt dieser Struktur: {\"summary\": \"Zusammenfassung und Analyse der Geschichte als Fließtext\",\"positive_choices\": [\"Beschreibung der positiven Entscheidung 1\",\"Weitere positive Entscheidungen je nach Bedarf\"],\"negative_choices\": [\"Beschreibung der negativen Entscheidung 1\",\"Weitere negative Entscheidungen je nach Bedarf\"],\"learnings\": [\"Erkenntnis 1\",\"Weitere Erkenntnisse je nach Bedarf\"],\"discussion_questions\": [\"Frage 1\",\"Weitere Fragen je nach Bedarf\"]}" });
            chatMessages.Add(new ChatMessage { Role = ValidRoles.User, Content = "Die Geschichte ist soeben vorbei. Du kannst jetzt die Analyse der durchlebten Geschichte erstellen." });

            if (story.Result != null)
            {
                AnalysisContent assistentContent = new()
                {
                    Summary = story.Result.Summary,
                    PositiveChoices = story.Result.PositiveChoices,
                    NegativeChoices = story.Result.NegativeChoices,
                    Learnings = story.Result.Learnings,
                    DiscussionQuestions = story.Result.DiscussionQuestions
                };
                chatMessages.Add(new ChatMessage { Role = ValidRoles.Assistant, Content = JsonSerializer.Serialize(assistentContent) });
            }

            return chatMessages;
        }

        // Benjamin Edlinger
        /// <summary>
        /// Gets the story part from the given assistant content
        /// </summary>
        /// <param name="assistantContent">The assistant content to get the story part from</param>
        /// <returns>The story part and the title of the story</returns>
        /// <exception cref="InvalidOperationException">If the message content is null</exception>
        /// <exception cref="JsonException">If the assistant content could not be deserialized</exception>
        private static (StoryPart, string) GetStoryPart(string assistantContent)
        {
            ArgumentNullException.ThrowIfNull(assistantContent);

            StoryContent messageContent;
            try
            {
                messageContent = JsonSerializer.Deserialize<StoryContent>(assistantContent) ?? throw new InvalidOperationException("Message content is null");
            }
            catch (JsonException e)
            {
                throw new JsonException("Failed to deserialize assistant content", e);
            }

            StoryPart storyPart = new()
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

        // Benjamin Edlinger
        /// <summary>
        /// Gets the story result from the given assistant content
        /// </summary>
        /// <param name="assistantContent">The assistant content to get the story result from</param>
        /// <param name="end">The end of the story</param>
        /// <returns>The story result</returns>
        /// <exception cref="InvalidOperationException">If the message content is null</exception>
        /// <exception cref="JsonException">If the assistant content could not be deserialized</exception>
        private static StoryResult GetStoryResult(string assistantContent, string end)
        {
            ArgumentNullException.ThrowIfNull(assistantContent);
            ArgumentNullException.ThrowIfNull(end);

            AnalysisContent messageContent;
            try
            {
                messageContent = JsonSerializer.Deserialize<AnalysisContent>(assistantContent) ?? throw new InvalidOperationException("Message content is null");
            }
            catch (JsonException e)
            {
                throw new JsonException("Failed to deserialize assistant content", e);
            }

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

        // Benjamin Edlinger
        /// <summary>
        /// Fetches the assistant content based on the given chat messages, temperature and topP
        /// </summary>
        /// <param name="chatMessages">The chat messages to fetch the assistant content for</param>
        /// <param name="temperature">The temperature for the assistant content</param>
        /// <param name="topP">The topP for the assistant content</param>
        /// <returns>The assistant content</returns>
        /// <exception cref="ArgumentException">If the chat messages are empty, the temperature is invalid or the topP is invalid</exception>
        /// <exception cref="HttpRequestException">If the request failed</exception>
        /// <exception cref="InvalidOperationException">If the response object is null or the assistant content is null or empty</exception>
        /// <exception cref="JsonException">If the response content could not be deserialized</exception>
        private static async Task<string> FetchAssitantContent(List<ChatMessage> chatMessages, float temperature, float topP)
        {
            ArgumentNullException.ThrowIfNull(_client);
            ArgumentNullException.ThrowIfNull(chatMessages);
            if (chatMessages.Count == 0) throw new ArgumentException("No messages to send");
            if (temperature < 0 || temperature > 1) throw new ArgumentException("Invalid temperature");
            if (topP < 0 || topP > 1) throw new ArgumentException("Invalid topP");

            HttpRequestMessage request = new(HttpMethod.Post, "/v1/openai/chat/completions")
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

            HttpResponseMessage response = null!;
            string responseString;
            try
            {
                response = await _client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                responseString = await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException e)
            {
                throw new HttpRequestException($"Request failed with status code {response?.StatusCode}", e);
            }

            try
            {
                Response responseObject = JsonSerializer.Deserialize<Response>(responseString) ?? throw new InvalidOperationException("Response object is null");
                return responseObject.Choices[0].Message.Content ?? throw new InvalidOperationException("Assistant content is null or empty");
            }
            catch (JsonException e)
            {
                throw new JsonException("Failed to deserialize response content", e);
            }
        }

        // Benjamin Edlinger
        /// <summary>
        /// Fetches the story image based on the given text
        /// </summary>
        /// <param name="text">The text to fetch the story image for</param>
        /// <returns>The path to the story image</returns>
        /// <exception cref="HttpRequestException">If the request failed</exception>
        /// <exception cref="InvalidOperationException">If the response object is null</exception>
        /// <exception cref="JsonException">If the response content could not be deserialized</exception>
        private static async Task<string> FetchStoryImage(string text)
        {
            ArgumentNullException.ThrowIfNull(_client);
            ArgumentNullException.ThrowIfNull(text);

            List<ChatMessage> chatMessages =
            [
                new ChatMessage { Role = ValidRoles.User, Content = $"Generate me only a prompt and nothing else for another ai to generate an image based on the following story: {text}" },
            ];
            HttpRequestMessage requestPrompt = new(HttpMethod.Post, "/v1/openai/chat/completions")
            {
                Content = new StringContent(JsonSerializer.Serialize(new
                {
                    model = "meta-llama/Llama-3.3-70B-Instruct",
                    messages = chatMessages,
                }), Encoding.UTF8, "application/json")
            };
            HttpResponseMessage responsePrompt = null!;
            string responseStringPrompt;
            try
            {
                responsePrompt = await _client.SendAsync(requestPrompt);
                responsePrompt.EnsureSuccessStatusCode();
                responseStringPrompt = await responsePrompt.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException e)
            {
                throw new HttpRequestException($"Request failed with status code {responsePrompt?.StatusCode}", e);
            }
            string imagePrompt = null!;
            try
            {
                Response responseObjectPrompt = JsonSerializer.Deserialize<Response>(responseStringPrompt) ?? throw new InvalidOperationException("Response object is null");
                imagePrompt = responseObjectPrompt.Choices[0].Message.Content ?? throw new InvalidOperationException("Assistant content is null or empty");
            }
            catch (JsonException e)
            {
                throw new JsonException("Failed to deserialize response content", e);
            }

            HttpRequestMessage requestImage = new(HttpMethod.Post, "/v1/inference/black-forest-labs/FLUX-1-dev")
            {
                Content = new StringContent(JsonSerializer.Serialize(new
                {
                    prompt = imagePrompt,
                }), Encoding.UTF8, "application/json")
            };
            HttpResponseMessage responseImage = null!;
            string responseStringImage;
            try
            {
                responseImage = await _client.SendAsync(requestImage);
                responseImage.EnsureSuccessStatusCode();
                responseStringImage = await responseImage.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException e)
            {
                throw new HttpRequestException($"Request failed with status code {responseImage?.StatusCode}", e);
            }
            string base64String;
            try
            {
                ImageContent responseObjectImage = JsonSerializer.Deserialize<ImageContent>(responseStringImage) ?? throw new InvalidOperationException("Response object is null");
                base64String = responseObjectImage.Images[0];
                if (base64String.StartsWith("data:image/png;base64,"))
                {
                    base64String = base64String.Replace("data:image/png;base64,", string.Empty);
                }
            }
            catch (JsonException e)
            {
                throw new JsonException("Failed to deserialize response content", e);
            }

            var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            var filePath = Path.Combine(directoryPath, $"{Guid.NewGuid()}.png");
            byte[] imageBytes = Convert.FromBase64String(base64String);
            using (var ms = new MemoryStream(imageBytes))
            {
                Image storyImage = Image.FromStream(ms);
                storyImage.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
            }

            return filePath;

        }

        public Task<Quiz> GenerateQuiz(Story story, QuizRequest config)
        {
            throw new NotImplementedException();
        }
    }

    // Benjamin Edlinger
    /// <summary>
    /// Exception for AI service
    /// </summary>
    public class AIException : Exception
    {
        public AIException()
        {
        }

        public AIException(string message)
            : base(message)
        {
        }

        public AIException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    // Benjamin Edlinger
    public static class ValidRoles
    {
        public const string System = "system";
        public const string User = "user";
        public const string Assistant = "assistant";
    }

    // Benjamin Edlinger
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

    // Benjamin Edlinger
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

    // Benjamin Edlinger
    public class Choice
    {
        [JsonPropertyName("index")]
        public int Index { get; set; }

        [JsonPropertyName("message")]
        public Message Message { get; set; } = null!;

        [JsonPropertyName("finish_reason")]
        public string FinishReason { get; set; } = null!;
    }

    // Benjamin Edlinger
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

    // Benjamin Edlinger
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

    // Benjamin Edlinger
    public class Option
    {
        [JsonPropertyName("impact")]
        public string ImpactString { get; set; } = null!;

        [JsonIgnore]
        public float Impact => float.Parse(ImpactString, CultureInfo.InvariantCulture);

        [JsonPropertyName("text")]
        public string Text { get; set; } = null!;
    }

    // Benjamin Edlinger
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

    // Benjamin Edlinger
    public class ImageContent
    {
        [JsonPropertyName("request_id")]
        public string RequestId { get; set; } = null!;

        [JsonPropertyName("inference_status")]
        public InferenceStatus InferenceStatus { get; set; } = null!;

        [JsonPropertyName("images")]
        public List<string> Images { get; set; } = null!;

        [JsonPropertyName("nsfw_content_detected")]
        public List<bool> NsfwContentDetected { get; set; } = null!;

        [JsonPropertyName("seed")]
        public long Seed { get; set; }
    }

    // Benjamin Edlinger
    public class InferenceStatus
    {
        [JsonPropertyName("status")]
        public string Status { get; set; } = null!;

        [JsonPropertyName("runtime_ms")]
        public int RuntimeMs { get; set; }

        [JsonPropertyName("cost")]
        public double Cost { get; set; }

        [JsonPropertyName("tokens_generated")]
        public int? TokensGenerated { get; set; }

        [JsonPropertyName("tokens_input")]
        public int? TokensInput { get; set; }
    }

    // Benjamin Edlinger
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
