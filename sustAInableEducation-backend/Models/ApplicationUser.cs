using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using sustAInableEducation_backend.Repository;

namespace sustAInableEducation_backend.Models;

public class ApplicationUser : IdentityUser
{
    [JsonIgnore] public ICollection<SpaceParticipant> Participations { get; set; } = new List<SpaceParticipant>();

    public string AnonUserName { get; set; } = UserNameGenService.GenerateUserName();
    public string? ProfileImage { get; set; }
}

public class ChangeEmailRequest
{
    public string Password { get; set; } = null!;
    public string NewEmail { get; set; } = null!;
}

public class ChangePasswordRequest
{
    public string OldPassword { get; set; } = null!;
    public string NewPassword { get; set; } = null!;
}

public class ImageRequest
{
    public ImageStyle Style { get; set; }
}

public enum ImageStyle
{
    [EnumMember(Value =
        "Cartoon – 'A vibrant and playful cartoon-style image with bold outlines, exaggerated proportions, and bright, flat colors. The style is clean and simple, similar to classic animated TV shows or comic strips.'")]
    Cartoon,

    [EnumMember(Value =
        "Pop-Art – 'A bold and colorful pop-art illustration with high-contrast colors, halftone dots, and thick black outlines. The image has a vintage comic book feel, inspired by 1950s-60s mass media and pop culture aesthetics.'")]
    PopArt,

    [EnumMember(Value =
        "PixelArt – 'A retro pixel art design with small, visible pixels, a limited color palette, and blocky, 8-bit or 16-bit style. The image has a nostalgic, old-school video game aesthetic.'")]
    PixelArt,

    [EnumMember(Value =
        "FantasyArt – 'A detailed and immersive fantasy artwork featuring mythical creatures, enchanted landscapes, and heroic characters. The colors are rich and dramatic, with a dreamlike, medieval-fantasy atmosphere.'")]
    FantasyArt,

    [EnumMember(Value =
        "Stencil – 'A striking stencil-style image with sharp, high-contrast shapes, solid black and white (or bold primary colors), and a spray-painted or cut-out effect, similar to street graffiti or political posters.'")]
    Stencil,

    [EnumMember(Value =
        "Papercraft – 'A delicate and intricate papercraft-style illustration that appears to be made from layered, cut-out paper. The edges are smooth, and the lighting creates soft shadows for a handmade, 3D effect.'")]
    Papercraft,

    [EnumMember(Value =
        "Risograph – 'A vintage risograph-style print with a limited color palette, grainy textures, and slight misalignments. The image has an indie zine aesthetic, with a mix of bold shapes and soft gradients.'")]
    Risograph,

    [EnumMember(Value =
        "Cyberpunk – 'A futuristic cyberpunk scene with neon lights, dark urban environments, and a high-tech, dystopian feel. The image includes glowing holograms, rain-slicked streets, and a mix of deep blues, purples, and pinks.'")]
    Cyberpunk,

    [EnumMember(Value =
        "PencilSketch – 'A detailed pencil sketch with visible strokes, hatching, and shading. The artwork has a hand-drawn quality, with soft, textured lines and a monochrome or lightly toned appearance.'")]
    PencilSketch,

    [EnumMember(Value =
        "PaperCollage – 'A creative paper collage-style artwork with torn, layered paper elements arranged into a dynamic composition. The textures are varied, with visible rough edges and overlapping pieces.'")]
    PaperCollage,

    [EnumMember(Value =
        "Psychedelic – 'A trippy psychedelic artwork with swirling patterns, bright neon colors, and surreal, distorted shapes. The image is inspired by 1960s counterculture, featuring hypnotic designs and dreamlike visuals.'")]
    Psychedelic,

    [EnumMember(Value =
        "StreetArt – 'A bold and expressive street art piece with graffiti-style lettering, spray-paint textures, and vibrant urban colors. The image has an edgy, rebellious feel, resembling large-scale murals or subway graffiti.'")]
    StreetArt,

    [EnumMember(Value =
        "Ukiyo-e – 'A traditional Japanese ukiyo-e woodblock print-style image with flat colors, delicate linework, and stylized depictions of nature, people, and folklore. The composition is elegant and detailed, with an old-world aesthetic.'")]
    UkiyoE,

    [EnumMember(Value =
        "Manga – 'A dynamic manga-style illustration with expressive characters, bold linework, and highly detailed backgrounds. The image has a black-and-white or cel-shaded look, with dramatic shading and action-oriented poses.'")]
    Manga,

    [EnumMember(Value =
        "Medieval – 'A richly detailed medieval illustration inspired by illuminated manuscripts and old-world paintings. The colors are muted, with ornate patterns, historical clothing, and a sense of ancient storytelling.'")]
    Medieval
}