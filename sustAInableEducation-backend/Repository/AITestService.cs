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
    }
}
