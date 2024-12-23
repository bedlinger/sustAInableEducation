export class SdgAsset {
    public id: number;
    public name: string;
    public iconPath: string;
    public gifPath: string;

    static readonly sdgs = {
        noPoverty: new SdgAsset(1, 'No Poverty', '/img/sdgs/icons/sdg_icon_01.png', '/img/sdgs/gifs/sdg_gif_01.gif'),
        zeroHunger: new SdgAsset(2, 'Zero Hunger', '/img/sdgs/icons/sdg_icon_02.png', '/img/sdgs/gifs/sdg_gif_02.gif'),
        goodHealth: new SdgAsset(3, 'Good Health and Well-being', '/img/sdgs/icons/sdg_icon_03.png', '/img/sdgs/gifs/sdg_gif_03.gif'),
        qualityEducation: new SdgAsset(4, 'Quality Education', '/img/sdgs/icons/sdg_icon_04.png', '/img/sdgs/gifs/sdg_gif_04.gif'),
        genderEquality: new SdgAsset(5, 'Gender Equality', '/img/sdgs/icons/sdg_icon_05.png', '/img/sdgs/gifs/sdg_gif_05.gif'),
        cleanWater: new SdgAsset(6, 'Clean Water and Sanitation', '/img/sdgs/icons/sdg_icon_06.png', '/img/sdgs/gifs/sdg_gif_06.gif'),
        affordableEnergy: new SdgAsset(7, 'Affordable and Clean Energy', '/img/sdgs/icons/sdg_icon_07.png', '/img/sdgs/gifs/sdg_gif_07.gif'),
        decentWork: new SdgAsset(8, 'Decent Work and Economic Growth', '/img/sdgs/icons/sdg_icon_08.png', '/img/sdgs/gifs/sdg_gif_08.gif'),
        industryInnovation: new SdgAsset(9, 'Industry, Innovation and Infrastructure', '/img/sdgs/icons/sdg_icon_09.png', '/img/sdgs/gifs/sdg_gif_09.gif'),
        reducedInequalities: new SdgAsset(10, 'Reduced Inequalities', '/img/sdgs/icons/sdg_icon_10.png', '/img/sdgs/gifs/sdg_gif_10.gif'),
        sustainableCities: new SdgAsset(11, 'Sustainable Cities and Communities', '/img/sdgs/icons/sdg_icon_11.png', '/img/sdgs/gifs/sdg_gif_11.gif'),
        responsibleConsumption: new SdgAsset(12, 'Responsible Consumption and Production', '/img/sdgs/icons/sdg_icon_12.png', '/img/sdgs/gifs/sdg_gif_12.gif'),
        climateAction: new SdgAsset(13, 'Climate Action', '/img/sdgs/icons/sdg_icon_13.png', '/img/sdgs/gifs/sdg_gif_13.gif'),
        lifeBelowWater: new SdgAsset(14, 'Life Below Water', '/img/sdgs/icons/sdg_icon_14.png', '/img/sdgs/gifs/sdg_gif_14.gif'),
        lifeOnLand: new SdgAsset(15, 'Life on Land', '/img/sdgs/icons/sdg_icon_15.png', '/img/sdgs/gifs/sdg_gif_15.gif'),
        peaceJustice: new SdgAsset(16, 'Peace and Justice Strong Institutions', '/img/sdgs/icons/sdg_icon_16.png', '/img/sdgs/gifs/sdg_gif_16.gif'),
        partnerships: new SdgAsset(17, 'Partnerships to achieve the Goal', '/img/sdgs/icons/sdg_icon_17.png', '/img/sdgs/gifs/sdg_gif_17.gif')
    };

    constructor(id: number, name: string, iconPath: string, gifPath: string) {
        this.id = id;
        this.name = name;
        this.iconPath = iconPath;
        this.gifPath = gifPath;
    }
}