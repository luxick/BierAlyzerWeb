using ProtoBuf;

namespace BierAlyzer.Contracts.Model
{
    [ProtoContract]
    public enum RequestResultStatus
    {
        [ProtoEnum] Success = 0,
        [ProtoEnum] NoContent = 10,
        [ProtoEnum] InvalidParameter = 20,
        [ProtoEnum] TokenError = 30,
        [ProtoEnum] ServerError = 40,
    }

    [ProtoContract]
    public enum UserType
    {
        [ProtoEnum] User = 10,
        [ProtoEnum] Admin = 100
    }

    [ProtoContract]
    public enum EventType
    {
        [ProtoEnum] Public = 10,
        [ProtoEnum] Private = 20,
        [ProtoEnum] Hidden = 30,
    }

    [ProtoContract]
    public enum EventStatus
    {
        [ProtoEnum] NotYet = 10,
        [ProtoEnum] Open = 20,
        [ProtoEnum] Closed = 30
    }
}
