export class SdgAsset {
    public id: number;
    public name: string;
    public iconPath: string;
    public gifPath: string;

    static readonly sdgs = {
        noPoverty: new SdgAsset(1, 'Keine Armut', '/img/sdgs/icons/sdg_icon_01.png', '/img/sdgs/gifs/sdg_gif_01.gif'),
        zeroHunger: new SdgAsset(2, 'Kein Hunger', '/img/sdgs/icons/sdg_icon_02.png', '/img/sdgs/gifs/sdg_gif_02.gif'),
        goodHealth: new SdgAsset(3, 'Gesundheit und Wohlergehen', '/img/sdgs/icons/sdg_icon_03.png', '/img/sdgs/gifs/sdg_gif_03.gif'),
        qualityEducation: new SdgAsset(4, 'Hochwertige Bildung', '/img/sdgs/icons/sdg_icon_04.png', '/img/sdgs/gifs/sdg_gif_04.gif'),
        genderEquality: new SdgAsset(5, 'Geschlechtergleichheit', '/img/sdgs/icons/sdg_icon_05.png', '/img/sdgs/gifs/sdg_gif_05.gif'),
        cleanWater: new SdgAsset(6, 'Sauberes Wasser und Sanitäreinrichtungen', '/img/sdgs/icons/sdg_icon_06.png', '/img/sdgs/gifs/sdg_gif_06.gif'),
        affordableEnergy: new SdgAsset(7, 'Bezahlbare und saubere Energie', '/img/sdgs/icons/sdg_icon_07.png', '/img/sdgs/gifs/sdg_gif_07.gif'),
        decentWork: new SdgAsset(8, 'Menschenwürdige Arbeit und Wirtschaftswachstum', '/img/sdgs/icons/sdg_icon_08.png', '/img/sdgs/gifs/sdg_gif_08.gif'),
        industryInnovation: new SdgAsset(9, 'Industrie, Innovation und Infrastruktur', '/img/sdgs/icons/sdg_icon_09.png', '/img/sdgs/gifs/sdg_gif_09.gif'),
        reducedInequalities: new SdgAsset(10, 'Weniger Ungleichheiten', '/img/sdgs/icons/sdg_icon_10.png', '/img/sdgs/gifs/sdg_gif_10.gif'),
        sustainableCities: new SdgAsset(11, 'Nachhaltige Städte und Gemeinden', '/img/sdgs/icons/sdg_icon_11.png', '/img/sdgs/gifs/sdg_gif_11.gif'),
        responsibleConsumption: new SdgAsset(12, 'Nachhaltige/r Konsum und Produktion', '/img/sdgs/icons/sdg_icon_12.png', '/img/sdgs/gifs/sdg_gif_12.gif'),
        climateAction: new SdgAsset(13, 'Maßnahmen zum Klimaschutz', '/img/sdgs/icons/sdg_icon_13.png', '/img/sdgs/gifs/sdg_gif_13.gif'),
        lifeBelowWater: new SdgAsset(14, 'Leben unter Wasser', '/img/sdgs/icons/sdg_icon_14.png', '/img/sdgs/gifs/sdg_gif_14.gif'),
        lifeOnLand: new SdgAsset(15, 'Leben an Land', '/img/sdgs/icons/sdg_icon_15.png', '/img/sdgs/gifs/sdg_gif_15.gif'),
        peaceJustice: new SdgAsset(16, 'Frieden, Gerechtigkeit und starke Institutionen', '/img/sdgs/icons/sdg_icon_16.png', '/img/sdgs/gifs/sdg_gif_16.gif'),
        partnerships: new SdgAsset(17, 'Partnerschaften zur Erreichung der Ziele', '/img/sdgs/icons/sdg_icon_17.png', '/img/sdgs/gifs/sdg_gif_17.gif')
    };

    constructor(id: number, name: string, iconPath: string, gifPath: string) {
        this.id = id;
        this.name = name;
        this.iconPath = iconPath;
        this.gifPath = gifPath;
    }
}