using System;
using ProtoBuf;

namespace BierAlyzer.Contracts.Dto
{
    [ProtoContract]
    public class DrinkDto
    {
        [ProtoMember(10)]
        public Guid DrinkId { get; set; }

        [ProtoMember(20)]
        public string Name { get; set; }

        [ProtoMember(30)]
        public double Amount { get; set; }

        [ProtoMember(40)]
        public double Percentage { get; set; }

        [ProtoMember(50)]
        public double AlcoholAmount { get; set; }

        [ProtoMember(60)]
        public bool Visible { get; set; }

        [ProtoMember(70)]
        public DateTime Created { get; set; }

        [ProtoMember(80)]
        public DateTime Modified { get; set; }
    }
}
