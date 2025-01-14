/*
{
    "votingTimeSeconds": 30,
    "story": {
        "topic": "Das Thema, auf dem die Geschichte basiert, ist das Ziel 8 Menschenwürdige Arbeit und Wirtschafts­wachstum der 17 Ziele für nachhaltige Entwicklung; dieses Ziel hat die folgende Beschreibung: Dauerhaftes, breitenwirksames und nachhaltiges Wirtschaftswachstum, produktive Vollbeschäftigung und menschenwürdige Arbeit für alle fördern. Achte darauf, dass du die unterschiedlichen Inhalte dieses Ziels behandelst und die Entscheidungspunkte auch zu diesem Ziel passen.",
        "targetGroup": 2,
        "length": 3,
        "temperature": 0.8,
        "topP": 0.7
    }
}
*/




import { SdgAsset } from './sdgs';

export class Configuration {
    public static generatePromptFromSdg(sdgId: number): string {
        if(sdgId < 1 || sdgId > 17) {
            throw new Error("Invalid SDG ID");
        }
        let sdgAsset = SdgAsset.findSdgById(sdgId)!.value;
        return `Das Thema, auf dem die Geschichte basiert, ist das Ziel ${sdgId} ${sdgAsset.name}
            der 17 Ziele für nachhaltige Entwicklung; dieses Ziel hat die folgende Beschreibung: ${sdgAsset.description}.
            Achte darauf, dass du die unterschiedlichen Inhalte dieses Ziels behandelst und die
            Entscheidungspunkte auch zu diesem Ziel passen.`
    }

    public static generatePromptFromTopic() {

    }
    
}

export enum TargetGroups {
    VOLKSSCHULE = 0,
    SEKUNDARSTUFE_EINS = 1,
    SEKUNDARSTUFE_ZWEI = 2,
}