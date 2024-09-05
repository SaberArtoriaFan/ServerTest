using System.ComponentModel.DataAnnotations;
using ProtoBuf;
// ReSharper disable InconsistentNaming
// ReSharper disable PropertyCanBeMadeInitOnly.Global
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Fantasy
{
    [ProtoContract]
    public sealed partial class Response : AMessage, IResponse
    {
        public uint OpCode()
        {
            return Fantasy.OpCode.DefaultResponse;
        }
        [ProtoMember(1)]
        public long RpcId { get; set; }
        [ProtoMember(2)]
        public uint ErrorCode { get; set; }
    }
    [ProtoContract]
    public sealed partial class RouteResponse : AMessage, IRouteResponse
    {
        public uint OpCode()
        {
            return Fantasy.OpCode.DefaultRouteResponse;
        }
        [ProtoMember(1)]
        public long RpcId { get; set; }
        [ProtoMember(2)]
        public uint ErrorCode { get; set; }
    }
    [ProtoContract]
    public partial class PingRequest : AMessage, IRequest
    {
        public uint OpCode()
        {
            return Fantasy.OpCode.PingRequest;
        }
        [ProtoIgnore]
        public PingResponse ResponseType { get; set; }
        [ProtoMember(1)]
        public long RpcId { get; set; }
    }

    [ProtoContract]
    public partial class PingResponse : AMessage, IResponse
    {
        public uint OpCode()
        {
            return Fantasy.OpCode.PingResponse;
        }
        [ProtoMember(1)]
        public long RpcId { get; set; }
        [ProtoMember(2)]
        public uint ErrorCode { get; set; }
        [ProtoMember(3)]
        public long Now;
    }
    [ProtoContract]
    public partial class I_AddressableAdd_Request : AMessage, IRouteRequest
    {
        [ProtoIgnore]
        public I_AddressableAdd_Response ResponseType { get; set; }
        public uint OpCode() { return Fantasy.OpCode.AddressableAddRequest; }
        public long RouteTypeOpCode() { return 1; }
        [ProtoMember(1)]
        public long AddressableId { get; set; }
        [ProtoMember(2)]
        public long RouteId { get; set; }
        [ProtoMember(3)]
        public bool IsLock { get; set; }
    }
    [ProtoContract]
    public partial class I_AddressableAdd_Response : AMessage, IRouteResponse
    {
        public uint OpCode() { return Fantasy.OpCode.AddressableAddResponse; }
        [ProtoMember(1)]
        public uint ErrorCode { get; set; }
    }
    [ProtoContract]
    public partial class I_AddressableGet_Request : AMessage, IRouteRequest
    {
        [ProtoIgnore]
        public I_AddressableGet_Response ResponseType { get; set; }
        public uint OpCode() { return Fantasy.OpCode.AddressableGetRequest; }
        public long RouteTypeOpCode() { return 1; }
        [ProtoMember(1)]
        public long AddressableId { get; set; }
    }
    [ProtoContract]
    public partial class I_AddressableGet_Response : AMessage, IRouteResponse
    {
        public uint OpCode() { return Fantasy.OpCode.AddressableGetResponse; }
        [ProtoMember(2)]
        public uint ErrorCode { get; set; }
        [ProtoMember(1)]
        public long RouteId { get; set; }
    }
    [ProtoContract]
    public partial class I_AddressableRemove_Request : AMessage, IRouteRequest
    {
        [ProtoIgnore]
        public I_AddressableRemove_Response ResponseType { get; set; }
        public uint OpCode() { return Fantasy.OpCode.AddressableRemoveRequest; }
        public long RouteTypeOpCode() { return 1; }
        [ProtoMember(1)]
        public long AddressableId { get; set; }
    }
    [ProtoContract]
    public partial class I_AddressableRemove_Response : AMessage, IRouteResponse
    {
        public uint OpCode() { return Fantasy.OpCode.AddressableRemoveResponse; }
        [ProtoMember(1)]
        public uint ErrorCode { get; set; }
    }
    [ProtoContract]
    public partial class I_AddressableLock_Request : AMessage, IRouteRequest
    {
        [ProtoIgnore]
        public I_AddressableLock_Response ResponseType { get; set; }
        public uint OpCode() { return Fantasy.OpCode.AddressableLockRequest; }
        public long RouteTypeOpCode() { return 1; }
        [ProtoMember(1)]
        public long AddressableId { get; set; }
    }
    [ProtoContract]
    public partial class I_AddressableLock_Response : AMessage, IRouteResponse
    {
        public uint OpCode() { return Fantasy.OpCode.AddressableLockResponse; }
        [ProtoMember(1)]
        public uint ErrorCode { get; set; }
    }
    [ProtoContract]
    public partial class I_AddressableUnLock_Request : AMessage, IRouteRequest
    {
        [ProtoIgnore]
        public I_AddressableUnLock_Response ResponseType { get; set; }
        public uint OpCode() { return Fantasy.OpCode.AddressableUnLockRequest; }
        public long RouteTypeOpCode() { return 1; }
        [ProtoMember(1)]
        public long AddressableId { get; set; }
        [ProtoMember(2)]
        public long RouteId { get; set; }
        [ProtoMember(3)]
        public string Source { get; set; }
    }
    [ProtoContract]
    public partial class I_AddressableUnLock_Response : AMessage, IRouteResponse
    {
        public uint OpCode() { return Fantasy.OpCode.AddressableUnLockResponse; }
        [ProtoMember(1)]
        public uint ErrorCode { get; set; }
    }
    [ProtoContract]
    public partial class LinkEntity_Request : AMessage, IRouteRequest
    {
        public uint OpCode() { return Fantasy.OpCode.LinkEntityRequest; }
        public long RouteTypeOpCode() { return 1; }
        [ProtoMember(1)]
        public int EntityType { get; set; }
        [ProtoMember(2)]
        public long RuntimeId { get; set; }
        [ProtoMember(3)]
        public long LinkGateSessionRuntimeId { get; set; }
    }
    [ProtoContract]
    public partial class LinkEntity_Response : AMessage, IRouteResponse
    {
        public uint OpCode() { return Fantasy.OpCode.LinkEntityResponse; }
        [ProtoMember(1)]
        public uint ErrorCode { get; set; }
    }
}