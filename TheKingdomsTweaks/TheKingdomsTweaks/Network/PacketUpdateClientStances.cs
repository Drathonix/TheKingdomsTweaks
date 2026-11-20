using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace TheKingdomsTweaks.Network;

[ProtoContract]
public class PacketUpdateClientStances
{
    [ProtoMember(1)]
    public required string[] Codes;
}
