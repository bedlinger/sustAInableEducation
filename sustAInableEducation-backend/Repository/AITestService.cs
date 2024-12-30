using sustAInableEducation_backend.Models;

namespace sustAInableEducation_backend.Repository
{
    public class AITestService : IAIService
    {
        public async Task<(StoryPart, string)> StartStory(Story story)
        {
            Thread.Sleep(2000);
            return (new StoryPart
            {
                Text = "Once upon a time...",
                Choices = new List<StoryChoice>
                {
                    new StoryChoice
                    {
                        Text = "Option 1",
                        Number = 1,
                        Impact = 0.6f
                    },
                    new StoryChoice
                    {
                        Text = "Option 2",
                        Number = 2,
                        Impact = 0.2f
                    },
                    new StoryChoice
                    {
                        Text = "Option 3",
                        Number = 3,
                        Impact = -0.2f
                    },
                    new StoryChoice
                    {
                        Text = "Option 4",
                        Number = 4,
                        Impact = -0.6f,
                    }
                }
            }, "Test title");
        }

        public async Task<StoryPart> GenerateNextPart(Story story)
        {
            Thread.Sleep(2000);
            return new StoryPart
            {
                Text = "And they lived happily ever after...",
                Choices = new List<StoryChoice>
                {
                    new StoryChoice
                    {
                        Text = "Option 1",
                        Number = 1,
                        Impact = 0.4f,
                    },
                    new StoryChoice
                    {
                        Text = "Option 2",
                        Number = 2,
                        Impact = 0.1f,
                    },
                    new StoryChoice
                    {
                        Text = "Option 3",
                        Number = 3,
                        Impact = -0.1f,
                    },
                    new StoryChoice
                    {
                        Text = "Option 4",
                        Number = 4,
                        Impact = -0.4f,
                    }
                }
            };
        }

        public async Task<StoryResult> GenerateResult(Story story)
        {
            Thread.Sleep(3000);
            return new StoryResult {
                Text = "The end",
                Summary = "Summary",
                PositiveChoices = new List<string> { "Option 1" },
                NegativeChoices = new List<string> { "Option 2" },
                Learnings = new List<string> { "Learning 1" },
                DiscussionQuestions = new List<string> { "Question 1" }
            };
        }

        public async Task<Quiz> GenerateQuiz(Story story, QuizRequest config)
        {
            Thread.Sleep(4000);
            var quiz = new Quiz
            {
                Title = "Test Quiz",
                NumberQuestions = config.NumberQuestions
            };
            for (var i = 1; i <= config.NumberQuestions; i++)
            {
                quiz.Questions.Add(new QuizQuestion
                {
                    Text = "Question " + i,
                    Number = i,
                    Choices = new List<QuizChoice>
                    {
                        new QuizChoice
                        {
                            Number = 1,
                            Text = "Option 1",
                            IsCorrect = true
                        },
                        new QuizChoice
                        {
                            Number = 2,
                            Text = "Option 2",
                            IsCorrect = false
                        },
                        new QuizChoice
                        {
                            Number = 3,
                            Text = "Option 3",
                            IsCorrect = true
                        },
                        new QuizChoice
                        {
                            Number = 4,
                            Text = "Option 4",
                            IsCorrect = false
                        }
                    }
                });
            }
            return quiz;
        }
    }
}
