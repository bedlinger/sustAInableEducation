using Newtonsoft.Json;

namespace sustAInableEducation_backend.Repository
{
    public class UserNameGenService
    {
        public static string GenerateUserName()
        {
            // Load data from the JSON file
            var json = File.ReadAllText("sustainable_words.json");
            var data = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(json);

            var sustainableNouns = data["sustainable_nouns"];
            var sustainabilityAdjectives = data["sustainability_adjectives"];

            // Example usage
            string email = "example@example.com";  // Replace with your email
            string username = GenerateUsername(email, sustainableNouns, sustainabilityAdjectives);
            return username;
        }

        // Function to generate a random username
        static string GenerateUsername(string email, List<string> sustainableNouns, List<string> sustainabilityAdjectives)
        {
            // Extract the part before the '@' symbol from the email
            string usernamePart = email.Split('@')[0];

            // Choose a random adjective and a random noun from the lists
            Random random = new Random();
            string randomAdjective = sustainabilityAdjectives[random.Next(sustainabilityAdjectives.Count)];
            string randomNoun = sustainableNouns[random.Next(sustainableNouns.Count)];

            // Generate a random number (e.g., between 1000 and 9999)
            int randomNumber = random.Next(1000, 10000);

            // Create the new username
            string newUsername = $"{randomAdjective}{randomNoun}{randomNumber}";

            return newUsername;
        }
    }
}
