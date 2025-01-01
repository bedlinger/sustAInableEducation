<template>
    <div class="w-full h-full">
        <div class="background animate-anim" />
        <div class="w-screen flex items-center h-full bg-slate-50">
            <div class="w-80 h-full pt-16">
                <div class="sidebar w-full h-full flex flex-col p-2">
                    <div id="sidebar-header">
                        <IconField class="">
                            <InputIcon>
                                <Icon name="ic:baseline-search" />
                            </InputIcon>
                            <InputText placeholder="Suchen" v-model="searchInput" class="w-full" />
                        </IconField>
                        <Divider class="!w-full" />
                    </div>
                    <div id="sidebar-content">
                        <EcoSpaceListEntry v-for="ecoSpace in searchedSpaces" :ecoSpace="ecoSpace" v-model="spaceRefsById[ecoSpace.id].value" v-on:click="selectSpaceById(ecoSpace.id)" />
                    </div>

                </div>
            </div>
            <div class="content bg-blue-500 flex-1 h-full">
              <div v-if="selectedSpace" class="w-full h-full pt-20 p-4">
                <div class="flex justify-between items-center flex-col w-full h-full bg-purple-500">
                  <h1 class="text-2xl font-bold">{{ selectedSpace.story.title }}</h1>
                </div>
              </div>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import type { EcoSpace } from '~/types/EcoSpace';


const sampleSpaces = {"ecoSpaces":[{"id":"eco1","votingTimeSeconds":300,"createdAt":"2024-12-30T10:00:00Z","participants":[{"userId":"user1","userName":"Alice","isHost":true,"isOnline":true,"impact":50},{"userId":"user2","userName":"Bob","isHost":false,"isOnline":false,"impact":30}],"story":{"title":"Saving the Rainforest","prompt":"How can we save the rainforest?","length":500,"temperature":0.7,"topP":0.9,"totalImpact":80,"parts":[{"intertitle":"Introduction","text":"The rainforest is under threat.","votingEndAt":"2024-12-30T12:00:00Z","chosenNumber":1,"choices":[{"number":1,"text":"Plant more trees.","numberVotes":5},{"number":2,"text":"Reduce deforestation.","numberVotes":3}]}],"result":{"text":"The rainforest was saved.","summary":"Efforts to save the rainforest were successful.","positiveChoices":["Plant more trees."],"negativeChoices":["Ignore deforestation."],"learnings":["Teamwork is crucial.","Education helps."],"discussionQuestions":["What can we do next?","How can we involve more people?"]}}},{"id":"eco2","votingTimeSeconds":600,"createdAt":"2024-12-02T14:00:00Z","participants":[{"userId":"user3","userName":"Charlie","isHost":true,"isOnline":true,"impact":70},{"userId":"user4","userName":"Diana","isHost":false,"isOnline":true,"impact":40}],"story":{"title":"Ocean Cleanup","prompt":"How can we clean up the oceans?","length":600,"temperature":0.8,"topP":0.85,"totalImpact":110,"parts":[{"intertitle":"The Challenge","text":"Plastic pollution is a major issue.","votingEndAt":"2024-12-30T13:00:00Z","chosenNumber":2,"choices":[{"number":1,"text":"Use biodegradable materials.","numberVotes":2},{"number":2,"text":"Organize beach cleanups.","numberVotes":6}]}],"result":{"text":"The oceans are cleaner.","summary":"Collaborative efforts led to significant cleanup.","positiveChoices":["Organize beach cleanups."],"negativeChoices":["Ignore the problem."],"learnings":["Small actions add up.","Awareness is key."],"discussionQuestions":["How can we maintain this?","What are other pollutants to address?"]}}},{"id":"eco3","votingTimeSeconds":400,"createdAt":"2024-12-28T16:00:00Z","participants":[{"userId":"user5","userName":"Eve","isHost":true,"isOnline":false,"impact":60},{"userId":"user6","userName":"Frank","isHost":false,"isOnline":true,"impact":35}],"story":{"title":"Sustainable Energy","prompt":"How can we transition to sustainable energy?","length":700,"temperature":0.6,"topP":0.92,"totalImpact":95,"parts":[{"intertitle":"Energy Crisis","text":"Fossil fuels are depleting.","votingEndAt":"2024-12-30T14:00:00Z","chosenNumber":1,"choices":[{"number":1,"text":"Invest in solar power.","numberVotes":4},{"number":2,"text":"Promote wind energy.","numberVotes":3}]}],"result":{"text":"Energy transition achieved.","summary":"Renewable energy adoption increased.","positiveChoices":["Invest in solar power."],"negativeChoices":["Delay renewable projects."],"learnings":["Innovation is vital.","Policy changes matter."],"discussionQuestions":["What is the next step?","How can we involve more stakeholders?"]}}},{"id":"eco4","votingTimeSeconds":500,"createdAt":"2024-12-27T18:00:00Z","participants":[{"userId":"user7","userName":"Grace","isHost":true,"isOnline":true,"impact":80},{"userId":"user8","userName":"Hank","isHost":false,"isOnline":false,"impact":25}],"story":{"title":"Urban Farming","prompt":"How can cities adopt urban farming?","length":450,"temperature":0.7,"topP":0.88,"totalImpact":105,"parts":[{"intertitle":"City Green Spaces","text":"Urban areas lack greenery.","votingEndAt":"2024-12-30T15:00:00Z","chosenNumber":2,"choices":[{"number":1,"text":"Create rooftop gardens.","numberVotes":3},{"number":2,"text":"Set up community farms.","numberVotes":5}]}],"result":{"text":"Urban farming flourished.","summary":"Communities benefited from fresh produce.","positiveChoices":["Set up community farms."],"negativeChoices":["Ignore urban farming."],"learnings":["Community engagement is key.","Utilizing space is important."],"discussionQuestions":["How can we scale this?","What challenges remain?"]}}},{"id":"eco5","votingTimeSeconds":450,"createdAt":"2024-12-26T20:00:00Z","participants":[{"userId":"user9","userName":"Ivy","isHost":true,"isOnline":true,"impact":90},{"userId":"user10","userName":"Jack","isHost":false,"isOnline":true,"impact":50}],"story":{"title":"Reducing Food Waste","prompt":"How can we reduce food waste?","length":550,"temperature":0.75,"topP":0.9,"totalImpact":140,"parts":[{"intertitle":"The Waste Problem","text":"Food waste is a global issue.","votingEndAt":"2024-12-30T16:00:00Z","chosenNumber":1,"choices":[{"number":1,"text":"Promote composting.","numberVotes":6},{"number":2,"text":"Encourage food donations.","numberVotes":4}]}],"result":{"text":"Food waste was minimized.","summary":"Composting and donations reduced waste.","positiveChoices":["Promote composting."],"negativeChoices":["Discard food unnecessarily."],"learnings":["Awareness campaigns work.","Systems need to be in place."],"discussionQuestions":["How can we improve distribution?","What are the next steps?"]}}}]}

const spaceRefs = Array.from({ length: sampleSpaces.ecoSpaces.length }, () => ref(false));

const spaceRefsById = sampleSpaces.ecoSpaces.reduce((acc, space) => {
    acc[space.id] = ref(false);
    return acc;
}, {} as Record<string, Ref<boolean>>);

console.log(spaceRefsById);

const selectedSpace = ref<EcoSpace>();

const searchInput = ref('');

const searchedSpaces = computed(() => {
    return sampleSpaces.ecoSpaces.filter(space => space.story.title.toLowerCase().includes(searchInput.value.toLowerCase()));
});

function selectSpace(index: number) {
    spaceRefs.forEach((ref, i) => {
        if(i === index) {
            ref.value = true;
            selectedSpace.value = sampleSpaces.ecoSpaces[i];
        } else {
            ref.value = false;
        }
    });
}

function selectSpaceById(id: string) {
    spaceRefsById[id].value = true;
    selectedSpace.value = sampleSpaces.ecoSpaces.find(space => space.id === id);
    Object.keys(spaceRefsById).forEach(key => {
        if(key !== id) {
            spaceRefsById[key].value = false;
        }
    });
}

</script>