using sustAInableEducation_backend.Models;

namespace sustAInableEducation_backend.Repository
{
    public class AIService : IAIService
    {
        public Task<StoryPart> GenerateNextPart(Story story)
        {
            throw new NotImplementedException();
        }

        public Task<Quiz> GenerateQuiz(Story story, QuizRequest config)
        {
            throw new NotImplementedException();
        }

        public Task<StoryPart> GenerateResult(Story story)
        {
            throw new NotImplementedException();
        }

        public Task<StoryPart> StartStory(Story story)
        {
            throw new NotImplementedException();
        }
    }
}
