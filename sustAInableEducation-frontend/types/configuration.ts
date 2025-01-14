import { SdgAsset } from './sdgs';

export class Configuration {

    static defaultTemperature = 0.8;
    static defaultTopP = 0.7;

    public static generatePromptFromSdg(sdgId: number): string {
        if(sdgId < 1 || sdgId > 17) {
            throw new Error("Invalid SDG ID");
        }
        let sdgAsset = SdgAsset.findSdgById(sdgId)!.value;
        return "Das Thema, auf dem die Geschichte basiert, ist das Ziel " + `${sdgId} ${sdgAsset.name}` + 
            " der 17 Ziele für nachhaltige Entwicklung; dieses Ziel hat die folgende Beschreibung: " + sdgAsset.description +
            ". Achte darauf, dass du die unterschiedlichen Inhalte dieses Ziels behandelst und die Entscheidungspunkte auch zu diesem Ziel passen."
    }

    public static generatePromptFromTopic(topicDescription: string, bulletPoints: string[]): string {
        let prompt = `Das Thema, welches die Geschichte behandeln soll, ist das Thema ${topicDescription};` +
        "dieses Thema soll folgende Punkte thematisieren: "

        bulletPoints.forEach((point, index) => {
            if(index == bulletPoints.length - 1) {
                prompt += `- ${point}`;
            } else {
                prompt += `- ${point}, `;
            }
        });

        return prompt;
    }
  
}

export enum TargetGroups {
    VOLKSSCHULE = 0,        
    SEKUNDARSTUFE_EINS = 1,
    SEKUNDARSTUFE_ZWEI = 2,
}