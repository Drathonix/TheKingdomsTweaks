using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TheKingdomsTweaks.Stances;

public class Stance
{
    [JsonProperty]
    public string Code { get; set; } = null;
    [JsonProperty]
    public float MoveSpeedMult { get; set; } = 1F;
    [JsonProperty]
    public float JumpHeightMult { get; set; } = 1F;
    [JsonProperty]
    public EyeHeightMultipliers EyeHeightMultipliers { get; set; } = new EyeHeightMultipliers();
    [JsonProperty]
    public CollisionBoxMultipliers CollisionBoxMultipliers { get; set; } = new CollisionBoxMultipliers();

}

public class EyeHeightMultipliers
{
    [JsonProperty]
    public float WalkMultiplier { get; set; } = 1F;
    [JsonProperty]
    public float SprintMultiplier { get; set; } = 1F;
    [JsonProperty]
    public float SneakMultiplier { get; set; } = 1F;
}

public class CollisionBoxMultipliers
{
    [JsonProperty]
    public float[] WalkMultiplier { get; set; } = [1F,1F,1F];
    [JsonProperty]
    public float[] SprintMultiplier { get; set; } = [1F, 1F, 1F];
    [JsonProperty]
    public float[] SneakMultiplier { get; set; } = [1F, 1F, 1F];
}
