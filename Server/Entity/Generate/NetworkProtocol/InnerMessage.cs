using MessagePack;
using System.Collections.Generic;
using Fantasy;
// ReSharper disable InconsistentNaming
// ReSharper disable RedundantUsingDirective
// ReSharper disable RedundantOverriddenMember
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable CheckNamespace
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
#pragma warning disable CS8618

namespace Fantasy
{	
	[MessagePackObject]
	public partial class G2A_TestRequest : AMessage, IRouteRequest
	{
		public static G2A_TestRequest Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<G2A_TestRequest>();
		}
		public override void Dispose()
		{
#if FANTASY_NET || FANTASY_UNITY
			Scene.MessagePoolComponent.Return<G2A_TestRequest>(this);
#endif
		}
		[IgnoreMember]
		public G2A_TestResponse ResponseType { get; set; }
		public uint OpCode() { return InnerOpcode.G2A_TestRequest; }
		public long RouteTypeOpCode() { return InnerRouteType.Route; }
	}
	[MessagePackObject]
	public partial class G2A_TestResponse : AMessage, IRouteResponse
	{
		public static G2A_TestResponse Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<G2A_TestResponse>();
		}
		public override void Dispose()
		{
			ErrorCode = default;
#if FANTASY_NET || FANTASY_UNITY
			Scene.MessagePoolComponent.Return<G2A_TestResponse>(this);
#endif
		}
		public uint OpCode() { return InnerOpcode.G2A_TestResponse; }
		[Key(0)]
		public uint ErrorCode { get; set; }
	}
	[MessagePackObject]
	public partial class G2M_RequestAddressableId : AMessage, IRouteRequest
	{
		public static G2M_RequestAddressableId Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<G2M_RequestAddressableId>();
		}
		public override void Dispose()
		{
			ClientID = default;
#if FANTASY_NET || FANTASY_UNITY
			Scene.MessagePoolComponent.Return<G2M_RequestAddressableId>(this);
#endif
		}
		[IgnoreMember]
		public M2G_ResponseAddressableId ResponseType { get; set; }
		public uint OpCode() { return InnerOpcode.G2M_RequestAddressableId; }
		public long RouteTypeOpCode() { return InnerRouteType.Route; }
		[Key(0)]
		public long ClientID { get; set; }
	}
	[MessagePackObject]
	public partial class M2G_ResponseAddressableId : AMessage, IRouteResponse
	{
		public static M2G_ResponseAddressableId Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<M2G_ResponseAddressableId>();
		}
		public override void Dispose()
		{
			ErrorCode = default;
			AddressableId = default;
#if FANTASY_NET || FANTASY_UNITY
			Scene.MessagePoolComponent.Return<M2G_ResponseAddressableId>(this);
#endif
		}
		public uint OpCode() { return InnerOpcode.M2G_ResponseAddressableId; }
		[Key(0)]
		public long AddressableId { get; set; }
		[Key(1)]
		public uint ErrorCode { get; set; }
	}
	[MessagePackObject]
	public partial class M2G_CreateNetworkObjectId : AMessage, IRouteMessage
	{
		public static M2G_CreateNetworkObjectId Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<M2G_CreateNetworkObjectId>();
		}
		public override void Dispose()
		{
			ClientID = default;
			data = default;
			Authority = default;
#if FANTASY_NET || FANTASY_UNITY
			Scene.MessagePoolComponent.Return<M2G_CreateNetworkObjectId>(this);
#endif
		}
		public uint OpCode() { return InnerOpcode.M2G_CreateNetworkObjectId; }
		public long RouteTypeOpCode() { return InnerRouteType.Route; }
		[Key(0)]
		public long ClientID { get; set; }
		[Key(1)]
		public InitData data { get; set; }
		[Key(2)]
		public bool Authority { get; set; }
	}
	[MessagePackObject]
	public partial class G2M_RemoveClient : AMessage, IRouteMessage
	{
		public static G2M_RemoveClient Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<G2M_RemoveClient>();
		}
		public override void Dispose()
		{
			ClientID = default;
#if FANTASY_NET || FANTASY_UNITY
			Scene.MessagePoolComponent.Return<G2M_RemoveClient>(this);
#endif
		}
		public uint OpCode() { return InnerOpcode.G2M_RemoveClient; }
		public long RouteTypeOpCode() { return InnerRouteType.Route; }
		[Key(0)]
		public long ClientID { get; set; }
	}
	[MessagePackObject]
	public partial class M2G_SyncTransform : AMessage, IRouteMessage
	{
		public static M2G_SyncTransform Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<M2G_SyncTransform>();
		}
		public override void Dispose()
		{
			ClientID = default;
			NetworkObjectID = default;
			Transform = default;
#if FANTASY_NET || FANTASY_UNITY
			Scene.MessagePoolComponent.Return<M2G_SyncTransform>(this);
#endif
		}
		public uint OpCode() { return InnerOpcode.M2G_SyncTransform; }
		public long RouteTypeOpCode() { return InnerRouteType.Route; }
		[Key(0)]
		public long ClientID { get; set; }
		[Key(1)]
		public long NetworkObjectID { get; set; }
		[Key(2)]
		public TransformData Transform { get; set; }
	}
}
