<template>
    <div class="w-full h-full">
        <div class="w-screen flex items-center h-full bg-slate-50">
            <div class="w-80 h-full pt-16 border-solid border-black border-r-2">
                <div class="sidebar w-full h-full flex flex-col p-2 overflow-y-scroll">
                    <div id="sidebar-header">
                        <div class="flex items-center">
                            <IconField class="mr-2">
                                <InputIcon>
                                    <Icon name="ic:baseline-search" />
                                </InputIcon>
                                <InputText placeholder="Suchen" v-model="searchInput" class="w-full" />
                            </IconField>
                            <Button class="!aspect-square !p-0 !" @click="toggleShowFilters">
                                <template #default>
                                    <div class="flex items-center justify-center size-11">
                                        <Icon :name="!showFilters ? 'ic:baseline-filter-alt' : 'ic:baseline-close'" :class="[showFilters ? 'size-6' : 'size-4']"/>                                     
                                    </div>
                                </template>
                            </Button>
                        </div>
                        <Panel class="mt-2" v-if="showFilters">
                            <div>
                                <h3 class="text-lg font-bold">Filter</h3>
                                <Divider />
                            </div>
                            <div class="flex flex-col mb-4">
                                <label class="mb-1" for="sortSelect">Sortierrichtung:</label>
                                <div class="flex">
                                    <Select class="w-32 flex-1 mr-1" id="sortSelect" size="small"
                                        v-model="filters.refs.sort.subject.value" :options="filters.options.sort" />
                                    <ToggleButton class="w-fit aspect-square !p-2"
                                        v-model="filters.refs.sort.direction.value">
                                        <template #default>
                                            <Icon v-if="filters.refs.sort.direction.value"
                                                name="ic:baseline-arrow-upward" />
                                            <Icon v-else name="ic:baseline-arrow-downward" />
                                        </template>
                                    </ToggleButton>
                                </div>
                            </div>
                            <div class="flex flex-col items-start mb-4">
                                <label class="text-md mb-1" for="finishedSelect">Abgeschlossen:</label>
                                <Select class="w-full" id="finishedSelect" size="small"
                                    v-model="filters.refs.finished.value" :options="filters.options.finished" />
                            </div>
                            <div class="flex flex-col items-start">
                                <label class="mb-1" for="datePicker">Erstellungsdatum:</label>
                                <DatePicker class="w-full" id="datePicker" v-model="filters.refs.date.value"
                                    size="small" dateFormat="dd.mm.yy" showButtonBar selectionMode="range"
                                    :manualInput="false" />
                            </div>
                            <Message v-if="!isFilterApplied" class="mt-4">
                                Die ausgewählten Filter wurden noch nicht angewandt.
                            </Message>
                            <Divider />
                            <div class="flex justify-between">
                                <Button label="Anwenden" size="small" @click="applyFilters()" />
                                <Button label="Zurücksetzen" size="small" variant="text" @click="resetFilters()" />

                            </div>
                        </Panel>
                        <Divider class="!w-full" />

                    </div>
                    <div id="sidebar-content">
                        <EcoSpaceListEntry v-for="ecoSpace in searchedSpaces" :ecoSpace="ecoSpace"
                            v-model="spaceRefsById[ecoSpace.id].value" v-on:click="selectSpaceById(ecoSpace.id)" />
                    </div>
                </div>
            </div>
            <div class="content flex-1 h-full overflow-y-scroll">
                <div v-if="selectedSpace" class="w-full pt-20 p-4">
                    <div class="flex items-start flex-col w-full h-full">
                        <h1 class="text-4xl font-bold mb-4">{{ selectedSpace.story.title }}</h1>
                        <Message class="w-full mb-4" v-if="!ecoSpaceIsFinished(selectedSpace)">
                            <template #icon>
                                <div class="flex items-center">
                                    <Icon name="ic:baseline-info" class="size-5" />
                                </div>
                            </template>
                            <span class="text-md">
                                Dieser EcoSpace wurde noch nicht beendet. Wenn Sie diesen EcoSpace fortsetzen wollen,
                                können Sie <Button variant="link" class="!p-0">
                                    <template #default>
                                        <span
                                            class="text-blue-700 hover:text-blue-500 hover:underline font-bold">hier</span>
                                    </template>
                                </Button>
                                klicken um diesem beizutreten.
                            </span>
                        </Message>
                        <Panel header="Informationen" class="w-full mb-4">
                            <Divider />
                            <div class="w-full h-full flex justify-between">
                                <div class="flex flex-col justify-between flex-1 mr-40">
                                    <div class="text-lg">
                                        <p class="mb-2">
                                            <span class="font-bold">Erstellt am</span>
                                            {{ formatDate(selectedSpace.createdAt) }}
                                        </p>
                                        <p class="mb-2">
                                            <span class="font-bold">Anzahl der Entscheidungspunkte:</span>
                                            {{ selectedSpace.story.length }}
                                        </p>
                                        <p class="mb-2">
                                            <span class="font-bold">Zielgruppe:</span>
                                            TODO
                                        </p>
                                    </div>
                                    <div class="w-full">
                                        <MeterGroup :value="getProgressData()" labelPosition="start"
                                            v-tooltip.bottom="{ value: getProgressLabel(), showDelay: 50 }">
                                            <template #label="{ totalPercent }">
                                                <p v-if="totalPercent < 100">Zu {{ totalPercent }}% Abgeschlossen
                                                </p>
                                                <p v-else class="flex items-center">
                                                    Abgeschlossen
                                                    <Icon name="ic:baseline-check" class="ml-2 size-5" />
                                                </p>
                                            </template>
                                        </MeterGroup>
                                    </div>
                                </div>
                                <Fieldset legend="Teilnehmer" class="max-h-64 overflow-scroll">
                                    <div class="h-full w-full">
                                        <DataTable :value="selectedSpace.participants">
                                            <Column field="userName" header="Username">
                                                <template #body="{ data }">
                                                    <div class="flex items-center">
                                                        <div class="size-5 mr-1">
                                                            <Icon name="ic:baseline-star-rate"
                                                                class="size-5 bg-yellow-500"
                                                                v-tooltip.bottom="{ value: 'Host', showDelay: 50 }"
                                                                v-if="data.isHost" />
                                                        </div>
                                                        <span>{{ data.userName }}</span>
                                                    </div>
                                                </template>
                                            </Column>
                                            <Column field="impact" header="Impact">
                                                <template #body="{ data }">
                                                    <div class="flex items-center justify-center">
                                                        <span v-if="ecoSpaceIsFinished(selectedSpace)">{{ data.impact
                                                            }}</span>
                                                        <span v-else>?</span>
                                                    </div>

                                                </template>
                                            </Column>
                                        </DataTable>
                                    </div>
                                </Fieldset>
                            </div>
                        </Panel>
                        <h2 class="text-2xl mb-2">Storyteile</h2>
                        <Accordion :value="Array.from(Array(selectedSpace.story.parts.length).keys())" multiple
                            class="w-full my-2">
                            <AccordionPanel v-for="part, index in selectedSpace.story.parts" :key="part.intertitle"
                                :value="index">
                                <AccordionHeader class="!text-xl">{{ part.intertitle }}</AccordionHeader>
                                <AccordionContent>
                                    <p class="m-0 mb-2">{{ part.text }}</p>
                                    <h3 class="text-lg font-bold mb-2">Optionen</h3>
                                    <div class="flex flex-col">
                                        <Button v-for="choice in part.choices" class="!w-full !mb-2">
                                            <template #default>
                                                <div class="flex w-full items-center">
                                                    <div class="flex items-center size-5 mr-3">
                                                        <Icon name="ic:baseline-check" class="size-5"
                                                            v-if="choice.number === part.chosenNumber" />
                                                    </div>
                                                    <div class="flex justify-between w-full items-center">
                                                        <p>{{ choice.text }}</p>
                                                        <Badge class="!bg-white !text-black"
                                                            :value="choice.numberVotes.toString() + ' Stimmen'" />
                                                    </div>
                                                </div>
                                            </template>
                                        </Button>
                                    </div>
                                </AccordionContent>
                            </AccordionPanel>
                            <h2 class="text-xl mt-4 mb-2" v-if="ecoSpaceIsFinished(selectedSpace)">Ergebnis</h2>
                            <AccordionPanel v-if="ecoSpaceIsFinished(selectedSpace)"
                                :value="selectedSpace.story.parts.length">
                                <AccordionHeader>{{ selectedSpace.story.result!.text }}</AccordionHeader>
                                <AccordionContent>
                                    <p>{{ selectedSpace.story.result!.summary }}</p>
                                </AccordionContent>
                            </AccordionPanel>
                        </Accordion>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { Button, Message } from 'primevue';
import type { EcoSpace } from '~/types/EcoSpace';
import type { OverviewFilter } from '~/types/filter';

const sampleSpaces = { "ecoSpaces": [{ "id": "eco1", "votingTimeSeconds": 300, "createdAt": "2024-12-30T10:00:00Z", "participants": [{ "userId": "user1", "userName": "Alice", "isHost": true, "isOnline": true, "impact": 50 }, { "userId": "user2", "userName": "Bob", "isHost": false, "isOnline": false, "impact": 30 }], "story": { "title": "Saving the Rainforest", "prompt": "How can we save the rainforest?", "length": 500, "temperature": 0.7, "topP": 0.9, "totalImpact": 80, "parts": [{ "intertitle": "Introduction", "text": "The rainforest is under threat from deforestation, climate change, and illegal logging. It is critical to take immediate action to preserve this vital ecosystem for future generations.", "votingEndAt": "2024-12-30T12:00:00Z", "chosenNumber": 1, "choices": [{ "number": 1, "text": "Plant more trees.", "numberVotes": 5 }, { "number": 2, "text": "Reduce deforestation.", "numberVotes": 3 }] }, { "intertitle": "Community Involvement", "text": "Engaging local communities in conservation efforts is essential. By providing education and sustainable alternatives, we can reduce dependency on destructive practices.", "votingEndAt": "2024-12-30T13:00:00Z", "chosenNumber": 2, "choices": [{ "number": 1, "text": "Educate locals.", "numberVotes": 4 }, { "number": 2, "text": "Offer economic incentives.", "numberVotes": 6 }] }], "result": undefined } }, { "id": "eco2", "votingTimeSeconds": 600, "createdAt": "2024-12-02T14:00:00Z", "participants": [{ "userId": "user3", "userName": "Charlie", "isHost": true, "isOnline": true, "impact": 70 }, { "userId": "user4", "userName": "Diana", "isHost": false, "isOnline": true, "impact": 40 }], "story": { "title": "Ocean Cleanup", "prompt": "How can we clean up the oceans?", "length": 600, "temperature": 0.8, "topP": 0.85, "totalImpact": 110, "parts": [{ "intertitle": "The Challenge", "text": "Plastic pollution has reached alarming levels, threatening marine life and ecosystems. Immediate and innovative solutions are required to mitigate this crisis.", "votingEndAt": "2024-12-30T13:00:00Z", "chosenNumber": 2, "choices": [{ "number": 1, "text": "Use biodegradable materials.", "numberVotes": 2 }, { "number": 2, "text": "Organize beach cleanups.", "numberVotes": 6 }] }, { "intertitle": "Innovative Technologies", "text": "Developing and deploying technologies like ocean skimmers and biodegradable materials can significantly reduce the amount of waste entering our oceans.", "votingEndAt": "2024-12-30T14:00:00Z", "chosenNumber": 1, "choices": [{ "number": 1, "text": "Invest in technology.", "numberVotes": 5 }, { "number": 2, "text": "Focus on education.", "numberVotes": 3 }] }], "result": undefined } }, { "id": "eco3", "votingTimeSeconds": 900, "createdAt": "2024-12-31T08:00:00Z", "participants": [{ "userId": "user11", "userName": "Anna", "isHost": true, "isOnline": true, "impact": 100 }, { "userId": "user12", "userName": "Ben", "isHost": false, "isOnline": true, "impact": 50 }, { "userId": "user13", "userName": "Clara", "isHost": false, "isOnline": false, "impact": 30 }, { "userId": "user14", "userName": "David", "isHost": false, "isOnline": true, "impact": 60 }, { "userId": "user15", "userName": "Ella", "isHost": false, "isOnline": true, "impact": 40 }, { "userId": "user16", "userName": "Frank", "isHost": false, "isOnline": false, "impact": 35 }, { "userId": "user17", "userName": "Grace", "isHost": false, "isOnline": true, "impact": 70 }, { "userId": "user18", "userName": "Hank", "isHost": false, "isOnline": false, "impact": 25 }, { "userId": "user19", "userName": "Ivy", "isHost": false, "isOnline": true, "impact": 90 }, { "userId": "user20", "userName": "Jack", "isHost": false, "isOnline": true, "impact": 50 }, { "userId": "user21", "userName": "Kara", "isHost": false, "isOnline": false, "impact": 45 }, { "userId": "user22", "userName": "Liam", "isHost": false, "isOnline": true, "impact": 55 }], "story": { "title": "Renewable Water Resources", "prompt": "How can we ensure sustainable water use?", "length": 800, "temperature": 0.65, "topP": 0.88, "totalImpact": 300, "parts": [{ "intertitle": "The Water Crisis", "text": "Water scarcity is a growing concern worldwide. Unsustainable practices, pollution, and climate change are reducing the availability of clean water, impacting both humans and ecosystems.", "votingEndAt": "2024-12-31T10:00:00Z", "chosenNumber": 1, "choices": [{ "number": 1, "text": "Promote water conservation.", "numberVotes": 8 }, { "number": 2, "text": "Invest in desalination plants.", "numberVotes": 4 }] }, { "intertitle": "Community Solutions", "text": "Engaging communities in water-saving practices and implementing local solutions can significantly improve water availability and sustainability over time.", "votingEndAt": "2024-12-31T11:00:00Z", "chosenNumber": 2, "choices": [{ "number": 1, "text": "Launch awareness campaigns.", "numberVotes": 6 }, { "number": 2, "text": "Build rainwater harvesting systems.", "numberVotes": 9 }] }], "result": { "text": "Sustainable water practices adopted globally.", "summary": "Communities and governments collaborated to ensure water sustainability.", "positiveChoices": ["Build rainwater harvesting systems."], "negativeChoices": ["Ignore water wastage."], "learnings": ["Collaboration is key.", "Technology and education matter."], "discussionQuestions": ["How can we scale these solutions?", "What role does policy play in water sustainability?"] } } }, { "id": "eco4", "votingTimeSeconds": 1200, "createdAt": "2025-01-01T09:00:00Z", "participants": [{ "userId": "user23", "userName": "Alice", "isHost": true, "isOnline": true, "impact": 120 }, { "userId": "user24", "userName": "Bob", "isHost": false, "isOnline": true, "impact": 60 }, { "userId": "user25", "userName": "Charlie", "isHost": false, "isOnline": false, "impact": 45 }, { "userId": "user26", "userName": "Diana", "isHost": false, "isOnline": true, "impact": 80 }, { "userId": "user27", "userName": "Eve", "isHost": false, "isOnline": true, "impact": 70 }, { "userId": "user28", "userName": "Frank", "isHost": false, "isOnline": false, "impact": 40 }, { "userId": "user29", "userName": "Grace", "isHost": false, "isOnline": true, "impact": 90 }, { "userId": "user30", "userName": "Hank", "isHost": false, "isOnline": false, "impact": 30 }, { "userId": "user31", "userName": "Ivy", "isHost": false, "isOnline": true, "impact": 85 }, { "userId": "user32", "userName": "Jack", "isHost": false, "isOnline": true, "impact": 75 }, { "userId": "user33", "userName": "Kara", "isHost": false, "isOnline": false, "impact": 50 }, { "userId": "user34", "userName": "Liam", "isHost": false, "isOnline": true, "impact": 65 }, { "userId": "user35", "userName": "Mia", "isHost": false, "isOnline": true, "impact": 95 }, { "userId": "user36", "userName": "Noah", "isHost": false, "isOnline": false, "impact": 35 }, { "userId": "user37", "userName": "Olivia", "isHost": false, "isOnline": true, "impact": 100 }, { "userId": "user38", "userName": "Paul", "isHost": false, "isOnline": false, "impact": 25 }, { "userId": "user39", "userName": "Quinn", "isHost": false, "isOnline": true, "impact": 55 }, { "userId": "user40", "userName": "Ruby", "isHost": false, "isOnline": true, "impact": 60 }, { "userId": "user41", "userName": "Sam", "isHost": false, "isOnline": false, "impact": 20 }, { "userId": "user42", "userName": "Tina", "isHost": false, "isOnline": true, "impact": 110 }, { "userId": "user43", "userName": "Uma", "isHost": false, "isOnline": true, "impact": 85 }, { "userId": "user44", "userName": "Victor", "isHost": false, "isOnline": false, "impact": 45 }, { "userId": "user45", "userName": "Wendy", "isHost": false, "isOnline": true, "impact": 95 }, { "userId": "user46", "userName": "Xander", "isHost": false, "isOnline": false, "impact": 50 }, { "userId": "user47", "userName": "Yara", "isHost": false, "isOnline": true, "impact": 70 }], "story": { "title": "Global Reforestation", "prompt": "How can we implement global reforestation effectively?", "length": 1000, "temperature": 0.7, "topP": 0.9, "totalImpact": 1500, "parts": [{ "intertitle": "The Reforestation Challenge", "text": "Deforestation has led to the loss of millions of hectares of forests worldwide. This has caused significant environmental issues, including climate change and biodiversity loss. Addressing this requires coordinated global efforts.", "votingEndAt": "2025-01-01T11:00:00Z", "chosenNumber": 1, "choices": [{ "number": 1, "text": "Plant trees in deforested areas.", "numberVotes": 15 }, { "number": 2, "text": "Protect existing forests.", "numberVotes": 10 }] }, { "intertitle": "Community Engagement", "text": "Involving local communities in reforestation projects is crucial. Providing education, economic incentives, and resources can empower people to participate actively in restoring forests.", "votingEndAt": "2025-01-01T12:00:00Z", "chosenNumber": 2, "choices": [{ "number": 1, "text": "Launch educational programs.", "numberVotes": 12 }, { "number": 2, "text": "Provide financial support for reforestation.", "numberVotes": 13 }] }, { "intertitle": "Technology and Innovation", "text": "Leveraging technology, such as drones for planting trees and satellite monitoring for tracking deforestation, can significantly enhance the efficiency of reforestation efforts.", "votingEndAt": "2025-01-01T13:00:00Z", "chosenNumber": 1, "choices": [{ "number": 1, "text": "Use drones for planting.", "numberVotes": 14 }, { "number": 2, "text": "Monitor forests with satellites.", "numberVotes": 11 }] }], "result": { "text": "Global reforestation efforts were successful.", "summary": "Collaborative and innovative approaches restored millions of hectares of forest worldwide.", "positiveChoices": ["Plant trees in deforested areas.", "Use drones for planting."], "negativeChoices": ["Ignore deforestation."], "learnings": ["Community involvement is essential.", "Technology accelerates progress."], "discussionQuestions": ["How can we ensure sustainability in reforestation?", "What are the next steps for global collaboration?"] } } }, { "id": "eco5", "votingTimeSeconds": 900, "createdAt": "2025-01-01T12:00:00Z", "participants": [{ "userId": "user48", "userName": "Alice", "isHost": true, "isOnline": true, "impact": 100 }, { "userId": "user49", "userName": "Bob", "isHost": false, "isOnline": true, "impact": 80 }, { "userId": "user50", "userName": "Charlie", "isHost": false, "isOnline": false, "impact": 60 }, { "userId": "user51", "userName": "Diana", "isHost": false, "isOnline": true, "impact": 70 }, { "userId": "user52", "userName": "Eve", "isHost": false, "isOnline": true, "impact": 90 }, { "userId": "user53", "userName": "Frank", "isHost": false, "isOnline": false, "impact": 50 }, { "userId": "user54", "userName": "Grace", "isHost": false, "isOnline": true, "impact": 75 }, { "userId": "user55", "userName": "Hank", "isHost": false, "isOnline": true, "impact": 65 }], "story": { "title": "Clean Energy Revolution", "prompt": "How can we accelerate the shift to clean energy globally?", "length": 1200, "temperature": 0.75, "topP": 0.85, "totalImpact": 1200, "parts": [{ "intertitle": "The Need for Clean Energy", "text": "Fossil fuels dominate global energy use, causing pollution and climate change. Transitioning to clean energy is essential for a sustainable future.", "votingEndAt": "2025-01-01T13:00:00Z", "chosenNumber": 3, "choices": [{ "number": 1, "text": "Invest in wind farms.", "numberVotes": 8 }, { "number": 2, "text": "Expand solar power.", "numberVotes": 10 }, { "number": 3, "text": "Promote geothermal energy.", "numberVotes": 12 }, { "number": 4, "text": "Enhance hydroelectric plants.", "numberVotes": 7 }] }, { "intertitle": "Overcoming Barriers", "text": "The shift to clean energy faces challenges such as high costs, lack of infrastructure, and resistance from stakeholders. Addressing these barriers is crucial.", "votingEndAt": "2025-01-01T14:00:00Z", "chosenNumber": 2, "choices": [{ "number": 1, "text": "Subsidize clean energy projects.", "numberVotes": 15 }, { "number": 2, "text": "Strengthen global policies.", "numberVotes": 18 }, { "number": 3, "text": "Educate communities on benefits.", "numberVotes": 10 }, { "number": 4, "text": "Reduce fossil fuel subsidies.", "numberVotes": 12 }] }, { "intertitle": "Innovative Solutions", "text": "Emerging technologies can accelerate clean energy adoption. Innovations like advanced batteries and smart grids are reshaping the energy landscape.", "votingEndAt": "2025-01-01T15:00:00Z", "chosenNumber": 4, "choices": [{ "number": 1, "text": "Develop next-gen batteries.", "numberVotes": 14 }, { "number": 2, "text": "Implement smart grid systems.", "numberVotes": 16 }, { "number": 3, "text": "Research nuclear fusion.", "numberVotes": 9 }, { "number": 4, "text": "Adopt AI for energy management.", "numberVotes": 20 }] }], "result": { "text": "The clean energy revolution succeeded.", "summary": "Global collaboration and innovation led to a rapid shift to sustainable energy sources.", "positiveChoices": ["Promote geothermal energy.", "Adopt AI for energy management."], "negativeChoices": ["Delay investment in renewables."], "learnings": ["Collaboration drives progress.", "Innovation is a game-changer."], "discussionQuestions": ["How can we maintain momentum in clean energy adoption?", "What lessons can be applied to other global challenges?"] } } }, { "id": "eco6", "votingTimeSeconds": 350, "createdAt": "2024-12-30T08:00:00Z", "participants": [{ "userId": "user11", "userName": "Liam", "isHost": true, "isOnline": true, "impact": 75 }, { "userId": "user12", "userName": "Sophia", "isHost": false, "isOnline": false, "impact": 45 }], "story": { "title": "Recycling Revolution", "prompt": "How can we revolutionize recycling?", "length": 8, "temperature": 0.65, "topP": 0.87, "totalImpact": 120, "parts": [{ "intertitle": "Understanding Recycling", "text": "Many materials are not recycled.", "votingEndAt": "2024-12-30T09:00:00Z", "chosenNumber": 2, "choices": [{ "number": 1, "text": "Educate the public.", "numberVotes": 3 }, { "number": 2, "text": "Improve collection systems.", "numberVotes": 5 }] }, { "intertitle": "Innovative Solutions", "text": "What new methods can be introduced?", "votingEndAt": "2024-12-30T10:00:00Z", "chosenNumber": 1, "choices": [{ "number": 1, "text": "Use AI for sorting waste.", "numberVotes": 4 }, { "number": 2, "text": "Incentivize recycling.", "numberVotes": 3 }] }], "result": { "text": "Recycling rates increased significantly.", "summary": "Innovative approaches transformed recycling.", "positiveChoices": ["Use AI for sorting waste."], "negativeChoices": ["Ignore advancements."], "learnings": ["Technology is key.", "Behavioral change matters."], "discussionQuestions": ["What other technologies can help?", "How can we make recycling universal?"] } } }, { "id": "eco7", "votingTimeSeconds": 600, "createdAt": "2024-12-29T12:00:00Z", "participants": [{ "userId": "user13", "userName": "Oliver", "isHost": true, "isOnline": true, "impact": 100 }, { "userId": "user14", "userName": "Emma", "isHost": false, "isOnline": true, "impact": 60 }], "story": { "title": "Wildlife Conservation", "prompt": "How can we protect endangered species?", "length": 10, "temperature": 0.7, "topP": 0.9, "totalImpact": 200, "parts": [{ "intertitle": "Introduction", "text": "Wildlife is under threat.", "votingEndAt": "2024-12-29T13:00:00Z", "chosenNumber": 1, "choices": [{ "number": 1, "text": "Raise awareness.", "numberVotes": 4 }, { "number": 2, "text": "Implement stricter laws.", "numberVotes": 3 }] }, { "intertitle": "Habitat Restoration", "text": "Restoring natural habitats.", "votingEndAt": "2024-12-29T14:00:00Z", "chosenNumber": 2, "choices": [{ "number": 1, "text": "Plant more trees.", "numberVotes": 2 }, { "number": 2, "text": "Protect existing forests.", "numberVotes": 5 }] }, { "intertitle": "Community Involvement", "text": "How can communities help?", "votingEndAt": "2024-12-29T15:00:00Z", "chosenNumber": 1, "choices": [{ "number": 1, "text": "Organize local campaigns.", "numberVotes": 6 }, { "number": 2, "text": "Volunteer for conservation projects.", "numberVotes": 4 }] }, { "intertitle": "Research and Technology", "text": "Using technology to track species.", "votingEndAt": "2024-12-29T16:00:00Z", "chosenNumber": 1, "choices": [{ "number": 1, "text": "Use drones for monitoring.", "numberVotes": 7 }, { "number": 2, "text": "Develop tracking devices.", "numberVotes": 3 }] }, { "intertitle": "Education", "text": "Educating the next generation.", "votingEndAt": "2024-12-29T17:00:00Z", "chosenNumber": 2, "choices": [{ "number": 1, "text": "Add wildlife topics to school curricula.", "numberVotes": 3 }, { "number": 2, "text": "Create wildlife clubs.", "numberVotes": 5 }] }, { "intertitle": "International Collaboration", "text": "Global efforts to save species.", "votingEndAt": "2024-12-29T18:00:00Z", "chosenNumber": 1, "choices": [{ "number": 1, "text": "Sign conservation treaties.", "numberVotes": 6 }, { "number": 2, "text": "Share resources internationally.", "numberVotes": 4 }] }, { "intertitle": "Funding Conservation", "text": "How to secure funding.", "votingEndAt": "2024-12-29T19:00:00Z", "chosenNumber": 2, "choices": [{ "number": 1, "text": "Organize fundraising events.", "numberVotes": 3 }, { "number": 2, "text": "Seek corporate sponsorships.", "numberVotes": 5 }] }, { "intertitle": "Sustainable Tourism", "text": "Eco-tourism to support wildlife.", "votingEndAt": "2024-12-29T20:00:00Z", "chosenNumber": 1, "choices": [{ "number": 1, "text": "Promote wildlife-friendly tourism.", "numberVotes": 6 }, { "number": 2, "text": "Limit access to sensitive areas.", "numberVotes": 4 }] }, { "intertitle": "Legal Protection", "text": "Strengthening wildlife laws.", "votingEndAt": "2024-12-29T21:00:00Z", "chosenNumber": 2, "choices": [{ "number": 1, "text": "Increase penalties for poaching.", "numberVotes": 3 }, { "number": 2, "text": "Enforce existing laws more strictly.", "numberVotes": 5 }] }, { "intertitle": "Conclusion", "text": "Summarizing efforts and future plans.", "votingEndAt": "2024-12-29T22:00:00Z", "chosenNumber": 1, "choices": [{ "number": 1, "text": "Continue collaboration.", "numberVotes": 7 }, { "number": 2, "text": "Focus on local initiatives.", "numberVotes": 3 }] }], "result": { "text": "Wildlife conservation improved globally.", "summary": "Comprehensive efforts yielded success.", "positiveChoices": ["Promote wildlife-friendly tourism."], "negativeChoices": ["Ignore international cooperation."], "learnings": ["Collaboration is essential.", "Education is powerful."], "discussionQuestions": ["What species need urgent attention?", "How can funding be increased?"] } } }, { "id": "eco8", "votingTimeSeconds": 450, "createdAt": "2024-12-28T15:00:00Z", "participants": [{ "userId": "user15", "userName": "Mia", "isHost": true, "isOnline": false, "impact": 55 }, { "userId": "user16", "userName": "Noah", "isHost": false, "isOnline": true, "impact": 50 }], "story": { "title": "Reducing Carbon Footprint", "prompt": "How can we reduce our carbon footprint?", "length": 6, "temperature": 0.7, "topP": 0.85, "totalImpact": 150, "parts": [{ "intertitle": "Individual Actions", "text": "Simple steps to reduce carbon emissions.", "votingEndAt": "2024-12-28T16:00:00Z", "chosenNumber": 2, "choices": [{ "number": 1, "text": "Use energy-efficient appliances.", "numberVotes": 3 }, { "number": 2, "text": "Switch to public transport.", "numberVotes": 4 }] }], "result": undefined } }] }

const showFilters = ref(false);

const filters: OverviewFilter = {
    applied: {
        finished: ref('Alle'),
        date: ref<Date | Date[] | (Date | null)[] | null | undefined>(undefined),
        sort: {
            subject: ref('Erstellungsdatum'),
            direction: ref(false) // false = ascending, true = descending
        }
    },
    refs: {
        finished: ref('Alle'),
        date: ref<Date | Date[] | (Date | null)[] | null | undefined>(),
        sort: {
            subject: ref('Erstellungsdatum'),
            direction: ref(false) // false = ascending, true = descending
        }
    },
    options: {
        finished: [
            'Alle',
            'Beendet',
            'Nicht beendet'
        ],
        sort: [
            'Erstellungsdatum',
            'Titel',
            'Anzahl der Entscheidungspunkte',
            'Anzahl der Teilnehmer'
        ]
    }
};

const spaceRefsById = sampleSpaces.ecoSpaces.reduce((acc, space) => {
    acc[space.id] = ref(false);
    return acc;
}, {} as Record<string, Ref<boolean>>);

const selectedSpace = ref<EcoSpace>();

const searchInput = ref('');

const searchedSpaces = computed(() => {
    return sortedSpaces.value.filter(space => space.story.title.toLowerCase().includes(searchInput.value.toLowerCase()));
});

const filteredSpaces = computed<EcoSpace[]>(() => {

    const normalizeDate = (date: Date) => {
        return new Date(date.getFullYear(), date.getMonth(), date.getDate());
    }
    const result = sampleSpaces.ecoSpaces.filter(space => {

        let finished;
        switch (filters.applied.finished.value) {
            case 'Alle':
                finished = true;
                break;
            case 'Beendet':
                finished = ecoSpaceIsFinished(space)
                break;
            case 'Nicht beendet':
                finished = !ecoSpaceIsFinished(space)
                break;
        }

        if (filters.applied.date.value) {
            if (Array.isArray(filters.applied.date.value)) {
                if (filters.applied.date.value[0] !== null) {
                    let fromDate = normalizeDate(new Date(space.createdAt)) >= filters.applied.date.value[0];
                    if (filters.applied.date.value[1] !== null) {
                        let toDate = normalizeDate(new Date(space.createdAt)) <= filters.applied.date.value[1];
                        return finished && fromDate && toDate;
                    } else {
                        return finished && fromDate;
                    }
                }
            } else {
                return finished;
            }
        } else {
            return finished;
        }
    });
    return Array.isArray(result) ? result : [result];
});

const sortedSpaces = computed<EcoSpace[]>(() => {
    const sortedList = filteredSpaces.value.sort((a, b) => {
        switch (filters.applied.sort.subject.value) {
            case 'Erstellungsdatum':
                return filters.applied.sort.direction.value ? new Date(a.createdAt).getTime() - new Date(b.createdAt).getTime(): new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime();
            case 'Titel':
                return filters.applied.sort.direction.value ? b.story.title.localeCompare(a.story.title) : a.story.title.localeCompare(b.story.title);
            case 'Anzahl der Entscheidungspunkte':
                return filters.applied.sort.direction.value ? a.story.length - b.story.length : b.story.length - a.story.length;
            case 'Anzahl der Teilnehmer':
                return filters.applied.sort.direction.value ? a.participants.length - b.participants.length : b.participants.length - a.participants.length;
            default:
                return 0;
        }
    });
    return sortedList;
})

const isFilterApplied = computed(() => {
    return filters.applied.finished.value === filters.refs.finished.value && filters.applied.date.value === filters.refs.date.value && filters.applied.sort.subject.value === filters.refs.sort.subject.value && filters.applied.sort.direction.value === filters.refs.sort.direction.value;
})

function selectSpaceById(id: string) {
    spaceRefsById[id].value = true;
    selectedSpace.value = sampleSpaces.ecoSpaces.find(space => space.id === id);
    Object.keys(spaceRefsById).forEach(key => {
        if (key !== id) {
            spaceRefsById[key].value = false;
        }
    });
}

function formatDate(dateString: string) {
    const date = new Date(dateString);
    const day = String(date.getDate()).padStart(2, '0');
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const year = date.getFullYear();
    return `${day}.${month}.${year}`;
}

function ecoSpaceIsFinished(space: EcoSpace): boolean {
    return !!space.story.result;
}

function getProgressData() {
    let percentage = (selectedSpace.value!.story.parts.length / selectedSpace.value!.story.length) * 100;
    return [{ label: '', value: percentage, color: 'var(--p-primary-color)' }];
}

function getProgressLabel() {
    let relative = selectedSpace.value!.story.parts.length / selectedSpace.value!.story.length;
    if (relative < 1) {
        return `${selectedSpace.value!.story.parts.length} von ${selectedSpace.value!.story.length} Entscheidungspunkten wurden abgeschlossen`;
    }
    return `Alle ${selectedSpace.value!.story.length} Entscheidungspunkten wurden abgeschlossen`;
}

function toggleShowFilters() {
    showFilters.value = !showFilters.value;
}

function resetFilters() {
    filters.refs.finished.value = 'Alle';
    filters.refs.date.value = undefined;
    filters.refs.sort.subject.value = 'Erstellungsdatum';
    filters.refs.sort.direction.value = false;
}

function applyFilters() {
    filters.applied.finished.value = filters.refs.finished.value;
    filters.applied.date.value = filters.refs.date.value;
    filters.applied.sort.subject.value = filters.refs.sort.subject.value;
    filters.applied.sort.direction.value = filters.refs.sort.direction.value;
}

</script>

<style scoped></style>