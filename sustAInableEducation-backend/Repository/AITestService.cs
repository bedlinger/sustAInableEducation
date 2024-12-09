using sustAInableEducation_backend.Models;

namespace sustAInableEducation_backend.Repository
{
    public class AITestService : IAIService
    {
        public async Task<StoryPart> StartStory(Story story)
        {
            Thread.Sleep(2000);
            return new StoryPart
            {
                Text = "Once upon a time...",
                Choices = new List<StoryChoice>
                {
                    new StoryChoice
                    {
                        Text = "Option 1",
                        Number = 1
                    },
                    new StoryChoice
                    {
                        Text = "Option 2",
                        Number = 2
                    },
                    new StoryChoice
                    {
                        Text = "Option 3",
                        Number = 3
                    },
                    new StoryChoice
                    {
                        Text = "Option 4",
                        Number = 4
                    }
                }
            };
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
                        Number = 1
                    },
                    new StoryChoice
                    {
                        Text = "Option 2",
                        Number = 2
                    },
                    new StoryChoice
                    {
                        Text = "Option 3",
                        Number = 3
                    },
                    new StoryChoice
                    {
                        Text = "Option 4",
                        Number = 4
                    }
                }
            };
        }

        public async Task<StoryPart> GenerateResult(Story story)
        {
            Thread.Sleep(3000);
            return new StoryPart
            {
                Text = "The end."
            };
        }

        public async Task<Quiz> GenerateQuiz(Story story, ICollection<QuizType> types)
        {
            Thread.Sleep(2000);
            return new Quiz
            {
                Title = "Test Quiz",
                Questions = new List<QuizQuestion>
                {
                    new QuizQuestion
                    {
                        Text = "What is the answer to life, the universe, and everything?",
                        Number = 1,
                        Choices = new List<QuizChoice>
                        {
                            new QuizChoice
                            {
                                Text = "42",
                                Number = 1,
                                IsCorrect = true
                            },
                            new QuizChoice
                            {
                                Text = "24",
                                Number = 2,
                                IsCorrect = false
                            },
                            new QuizChoice
                            {
                                Text = "12",
                                Number = 3,
                                IsCorrect = false
                            },
                            new QuizChoice
                            {
                                Text = "21",
                                Number = 4,
                                IsCorrect = false
                            }
                        },
                        IsMultipleResponse = false
                    },
                    new QuizQuestion
                    {
                        Text = "What is the answer to life, the universe, and everything?",
                        Number = 2,
                        Choices = new List<QuizChoice>
                        {
                            new QuizChoice
                            {
                                Text = "42",
                                Number = 1,
                                IsCorrect = true
                            },
                            new QuizChoice
                            {
                                Text = "24",
                                Number = 2,
                                IsCorrect = false
                            },
                            new QuizChoice
                            {
                                Text = "12",
                                Number = 3,
                                IsCorrect = false
                            },
                            new QuizChoice
                            {
                                Text = "21",
                                Number = 4,
                                IsCorrect = false
                            }
                        },
                        IsMultipleResponse = false
                    }
                }
            };
        }
    }
}
