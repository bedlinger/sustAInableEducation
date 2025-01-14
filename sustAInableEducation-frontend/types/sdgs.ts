export class SdgAsset {
    public id: number;
    public name: string;
    public description: string;
    public iconPath: string;
    public gifPath: string;

    static readonly sdgs = {
        noPoverty: new SdgAsset(1, 'Keine Armut', 'Armut in all ihren Formen und überall beenden', '/img/sdgs/icons/sdg_icon_01.png', '/img/sdgs/gifs/sdg_gif_01.gif'),
        zeroHunger: new SdgAsset(2, 'Kein Hunger', 'Den Hunger beenden, Ernährungssicherheit und eine bessere Ernährung erreichen und eine nachhaltige Landwirtschaft fördern', '/img/sdgs/icons/sdg_icon_02.png', '/img/sdgs/gifs/sdg_gif_02.gif'),
        goodHealth: new SdgAsset(3, 'Gesundheit und Wohlergehen', 'Ein gesundes Leben für alle Menschen jeden Alters gewährleisten und ihr Wohlergehen fördern', '/img/sdgs/icons/sdg_icon_03.png', '/img/sdgs/gifs/sdg_gif_03.gif'),
        qualityEducation: new SdgAsset(4, 'Hochwertige Bildung', 'Inklusive, gleichberechtigte und hochwertige Bildung gewährleisten und Möglichkeiten des lebenslangen Lernens für alle fördern', '/img/sdgs/icons/sdg_icon_04.png', '/img/sdgs/gifs/sdg_gif_04.gif'),
        genderEquality: new SdgAsset(5, 'Geschlechtergleichheit', 'Geschlechtergleichstellung erreichen und alle Frauen und Mädchen zur Selbstbestimmung befähigen', '/img/sdgs/icons/sdg_icon_05.png', '/img/sdgs/gifs/sdg_gif_05.gif'),
        cleanWater: new SdgAsset(6, 'Sauberes Wasser und Sanitäreinrichtungen', 'Verfügbarkeit und nachhaltige Bewirtschaftung von Wasser und Sanitärversorgung für alle gewährleisten', '/img/sdgs/icons/sdg_icon_06.png', '/img/sdgs/gifs/sdg_gif_06.gif'),
        affordableEnergy: new SdgAsset(7, 'Bezahlbare und saubere Energie', 'Zugang zu bezahlbarer, verlässlicher, nachhaltiger und moderner Energie für alle sichern', '/img/sdgs/icons/sdg_icon_07.png', '/img/sdgs/gifs/sdg_gif_07.gif'),
        decentWork: new SdgAsset(8, 'Menschenwürdige Arbeit und Wirtschaftswachstum', 'Dauerhaftes, breitenwirksames und nachhaltiges Wirtschaftswachstum, produktive Vollbeschäftigung und menschenwürdige Arbeit für alle fördern', '/img/sdgs/icons/sdg_icon_08.png', '/img/sdgs/gifs/sdg_gif_08.gif'),
        industryInnovation: new SdgAsset(9, 'Industrie, Innovation und Infrastruktur', 'Eine widerstandsfähige Infrastruktur aufbauen, breitenwirksame und nachhaltige Industrialisierung fördern und Innovationen unterstützen', '/img/sdgs/icons/sdg_icon_09.png', '/img/sdgs/gifs/sdg_gif_09.gif'),
        reducedInequalities: new SdgAsset(10, 'Weniger Ungleichheiten', 'Ungleichheit in und zwischen Ländern verringern', '/img/sdgs/icons/sdg_icon_10.png', '/img/sdgs/gifs/sdg_gif_10.gif'),
        sustainableCities: new SdgAsset(11, 'Nachhaltige Städte und Gemeinden', 'Städte und Siedlungen inklusiv, sicher, widerstandsfähig und nachhaltig gestalten', '/img/sdgs/icons/sdg_icon_11.png', '/img/sdgs/gifs/sdg_gif_11.gif'),
        responsibleConsumption: new SdgAsset(12, 'Nachhaltige/r Konsum und Produktion', 'Nachhaltige Konsum- und Produktionsmuster sicherstellen', '/img/sdgs/icons/sdg_icon_12.png', '/img/sdgs/gifs/sdg_gif_12.gif'),
        climateAction: new SdgAsset(13, 'Maßnahmen zum Klimaschutz', 'Umgehend Maßnahmen zur Bekämpfung des Klimawandels und seiner Auswirkungen ergreifen', '/img/sdgs/icons/sdg_icon_13.png', '/img/sdgs/gifs/sdg_gif_13.gif'),
        lifeBelowWater: new SdgAsset(14, 'Leben unter Wasser', 'Ozeane, Meere und Meeresressourcen im Sinne nachhaltiger Entwicklung erhalten und nachhaltig nutzen', '/img/sdgs/icons/sdg_icon_14.png', '/img/sdgs/gifs/sdg_gif_14.gif'),
        lifeOnLand: new SdgAsset(15, 'Leben an Land', 'Landökosysteme schützen, wiederherstellen und ihre nachhaltige Nutzung fördern, Wälder nachhaltig bewirtschaften, Wüstenbildung bekämpfen, Bodendegradation beenden und umkehren und dem Verlust der biologischen Vielfalt ein Ende setzen', '/img/sdgs/icons/sdg_icon_15.png', '/img/sdgs/gifs/sdg_gif_15.gif'),
        peaceJustice: new SdgAsset(16, 'Frieden, Gerechtigkeit und starke Institutionen', 'Friedliche und inklusive Gesellschaften für eine nachhaltige Entwicklung fördern, allen Menschen Zugang zur Justiz ermöglichen und leistungsfähige, rechenschaftspflichtige und inklusive Institutionen auf allen Ebenen aufbauen', '/img/sdgs/icons/sdg_icon_16.png', '/img/sdgs/gifs/sdg_gif_16.gif'),
        partnerships: new SdgAsset(17, 'Partnerschaften zur Erreichung der Ziele', 'Umsetzungsmittel stärken und die Globale Partnerschaft für nachhaltige Entwicklung mit neuem Leben erfüllen', '/img/sdgs/icons/sdg_icon_17.png', '/img/sdgs/gifs/sdg_gif_17.gif')
    };

    constructor(id: number, name: string, description: string, iconPath: string, gifPath: string) {
        this.id = id;
        this.name = name;
        this.description = description;
        this.iconPath = iconPath;
        this.gifPath = gifPath;
    }

    static findSdgById(sdgId: number) {
        if(sdgId < 1 || sdgId > 17) {
            throw new Error("Invalid SDG ID");
        }
        for (const [key, value] of Object.entries(SdgAsset.sdgs)) {
            if (value.id === sdgId) {
                return { key, value };
            }
        }
        return null;
    }
}