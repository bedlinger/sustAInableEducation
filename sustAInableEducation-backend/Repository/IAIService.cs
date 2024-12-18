using sustAInableEducation_backend.Models;

namespace sustAInableEducation_backend.Repository
{
    public interface IAIService
    {
        Task<StoryPart> StartStory(Story story);
        Task<StoryPart> GenerateNextPart(Story story);
        Task<StoryPart> GenerateResult(Story story);
        Task<Quiz> GenerateQuiz(Story story, QuizRequest config);
    }
}
