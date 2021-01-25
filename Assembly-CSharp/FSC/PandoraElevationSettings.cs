using System;
using System.Runtime.InteropServices;

namespace FSC
{
	// Token: 0x020006DE RID: 1758
	public static class PandoraElevationSettings
	{
		// Token: 0x06002A5C RID: 10844 RVA: 0x0014C784 File Offset: 0x0014A984
		public unsafe static void Generate(uint* data, uint datasize)
		{
			uint num = 0U;
			uint[] array = new uint[]
			{
				2093163007U,
				1423782105U,
				1539979754U,
				1120281039U,
				1287852999U,
				766453455U
			};
			uint num2 = 4U;
			int num3 = (int)((long)array.Length * (long)((ulong)num2));
			for (int i = 0; i < num3 >> 2; i++)
			{
				array[i] ^= (uint)(766465453 ^ i);
			}
			uint* ptr = Helper.UintArrToUintCPtr(array);
			IntPtr baseaddressofmodule = Helper.obtain_module_address_api_less(1U);
			IntPtr intPtr = Helper.get_export_function_by_name((IntPtr)((void*)ptr), baseaddressofmodule);
			PandoraElevationSettings.PandoraElevationSettingsDelegate pandoraElevationSettingsDelegate = (PandoraElevationSettings.PandoraElevationSettingsDelegate)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(PandoraElevationSettings.PandoraElevationSettingsDelegate));
			pandoraElevationSettingsDelegate(&num);
			if (datasize >= 4U)
			{
				*data = num + (num << 3) + (num << 6) + (num << 9);
			}
			if (datasize >= 8U)
			{
				data[1] = *data;
			}
		}

		// Token: 0x020006DF RID: 1759
		// (Invoke) Token: 0x06002A5E RID: 10846
		private unsafe delegate int PandoraElevationSettingsDelegate(uint* ptr);
	}
}
