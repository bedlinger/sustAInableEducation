using Newtonsoft.Json;

namespace sustAInableEducation_backend.Repository;

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
        var email = "example@example.com"; // Replace with your email
        var username = GenerateUsername(email, sustainableNouns, sustainabilityAdjectives);
        return username;
    }

    // Function to generate a random username
    private static string GenerateUsername(string email, List<string> sustainableNouns,
        List<string> sustainabilityAdjectives)
    {
        // Extract the part before the '@' symbol from the email
        var usernamePart = email.Split('@')[0];

        // Choose a random adjective and a random noun from the lists
        var random = new Random();
        var randomAdjective = sustainabilityAdjectives[random.Next(sustainabilityAdjectives.Count)];
        var randomNoun = sustainableNouns[random.Next(sustainableNouns.Count)];

        // Generate a random number (e.g., between 1000 and 9999)
        var randomNumber = random.Next(1000, 10000);

        // Create the new username
        var newUsername = $"{randomAdjective}{randomNoun}{randomNumber}";

        return newUsername;
    }
}