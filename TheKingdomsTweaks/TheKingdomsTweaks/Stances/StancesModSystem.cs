using System.Collections.Generic;
using TheKingdomsTweaks.API;
using TheKingdomsTweaks.Network;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Server;
using Vintagestory.API.Util;
using Vintagestory.Common;

namespace TheKingdomsTweaks.Stances;

public class StancesModSystem: ModSystem
{
    private const string STANCES_FOLDER = "stances/";

    private const string ATTR_STANCE = "stances:stance";
    private const string CHANGE_STANCE_KEY = "stances:changestance";

    private readonly Dictionary<string, Stance> StancesByCode = [];

    private Dictionary<string,Stance> ClientStances = [];
    public Stance DefaultStance;
    public override void AssetsFinalize(ICoreAPI api)
    {
        foreach (var item in Gather(api))
        {
            StancesByCode[item.Code] = item;
        }
        DefaultStance = StancesByCode["default"];
    }
    public override void Start(ICoreAPI api)
    {        
        var channel = api.Network.RegisterChannel("stances");
        channel.RegisterMessageType<PacketRequestChangeStance>();
        channel.RegisterMessageType<PacketUpdateClientStances>();
    }

    public override void StartServerSide(ICoreServerAPI api)
    {
        var channel = api.Network.GetChannel("stances");
        channel.SetMessageHandler<PacketRequestChangeStance>((fromPlayer, packet) =>
        {
            if (fromPlayer.Entity.Stats.Enabled($"stance_{packet.Code}"))
            {
                SetActiveStance(fromPlayer.Entity, StancesByCode[packet.Code]);
            } else
            {
                //TODO: this is not important for now but in the future I will create some library for reporting violations like this one without spamming the logs.
                //api.Logger.Audit("Player {0} attempted to change their stance to {1} which they don't have access to. This is either caused by intentional client modification or a bug.", [fromPlayer.PlayerName,packet.Code]);
            }
        });

    }

    public override void StartClientSide(ICoreClientAPI api)
    {
        var channel = api.Network.GetChannel("stances");
        channel.SetMessageHandler<PacketUpdateClientStances>(packet => {
            for (int i = 0; i < packet.Codes.Length; i++)
            {
                string code = packet.Codes[i];
                if(i+1 == packet.Codes.Length)
                {
                    ClientStances[code] = DefaultStance;
                }
                else
                {
                    ClientStances[code] = StancesByCode[packet.Codes[i + 1]];
                }
            }
        });
        api.Input.RegisterHotKey(CHANGE_STANCE_KEY, "Change Stance", GlKeys.G);
        HotKey nextStance = api.Input.HotKeys[CHANGE_STANCE_KEY];
        nextStance.TriggerOnUpAlso = true;
        nextStance.Handler = (combos) =>
        {
            if (combos.OnKeyUp)
            {
                channel.SendPacket(new PacketRequestChangeStance()
                {
                    Code = NextStance(api.World.Player.Entity).Code
                });
            }
            return true;
        };
    }

    private Stance NextStance(Entity entity)
    {
        Stance current = GetActiveStance(entity);
        Stance? next = ClientStances.GetNullable(current.Code);
        return next ?? DefaultStance;
    }

    public Stance GetStance(string code)
    {
        return StancesByCode.Get(code,DefaultStance);
    }

    public Stance GetActiveStance(Entity entity)
    {
        string stanceCode = entity.WatchedAttributes.GetString(ATTR_STANCE);
        if(stanceCode == null)
        {
            return DefaultStance;
        }
        else
        {
            return StancesByCode.Get(stanceCode,DefaultStance);
        }
    }

    public void SetActiveStance(Entity entity, Stance stance)
    {
        entity.WatchedAttributes.SetString(ATTR_STANCE, stance.Code);
    }
    private static List<Stance> Gather(ICoreAPI api)
    {
        if (api?.Assets == null) {
            return [];
        }
        List<IAsset> many = api.Assets.GetManyInCategory("config",STANCES_FOLDER);
        if (many == null)
        {
            return [];
        }

        List<Stance> stances = [];
        foreach (IAsset asset in many)
        {
            Stance[] s = asset.ToObject<Stance[]>();
            stances.AddRange(s);
        }
        return stances;
    }
}
