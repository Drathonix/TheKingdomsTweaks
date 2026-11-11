using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace TheKingdomsTweaks.Network;

[ProtoContract]
public class PacketUpdateClientStances
{
    [ProtoMember(0)]
    public required string[] Codes;
}
