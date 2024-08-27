namespace Fantasy
{
	public static partial class OuterOpcode
	{
		 public const int C2G_TestMessage = 100000001;
		 public const int C2G_TestRequest = 110000001;
		 public const int G2C_TestResponse = 160000001;
		 public const int C2G_CreateAddressableRequest = 110000002;
		 public const int G2C_CreateAddressableResponse = 160000002;
		 public const int C2M_TestMessage = 190000001;
		 public const int C2M_TestRequest = 200000001;
		 public const int M2C_TestResponse = 250000001;
		 public const int C2M_RequestInit = 200000002;
		 public const int M2C_ResponseInit = 250000002;
		 public const int C2M_RequestNetworkObjectId = 200000003;
		 public const int M2C_ResponseNetworkObjectId = 250000003;
		 public const int G2C_CreateNetworkObjectId = 100000002;
		 public const int C2M_SyncTransform = 190000002;
		 public const int G2C_SyncTransform = 100000003;
		 public const int G2C_DeleteNetworkObj = 100000004;
		 public const int C2M_DeleteNetworkObj = 190000003;
	}
}
