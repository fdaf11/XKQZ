using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Exploder
{
	// Token: 0x020000BA RID: 186
	public static class Profiler
	{
		// Token: 0x060003E5 RID: 997 RVA: 0x00032318 File Offset: 0x00030518
		public static void Start(string key)
		{
			Stopwatch stopwatch = null;
			if (Profiler.timeSegments.TryGetValue(key, ref stopwatch))
			{
				stopwatch.Reset();
				stopwatch.Start();
			}
			else
			{
				stopwatch = new Stopwatch();
				stopwatch.Start();
				Profiler.timeSegments.Add(key, stopwatch);
			}
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x00004AF0 File Offset: 0x00002CF0
		public static void End(string key)
		{
			Profiler.timeSegments[key].Stop();
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x00032364 File Offset: 0x00030564
		public static string[] PrintResults()
		{
			string[] array = new string[Profiler.timeSegments.Count];
			int num = 0;
			foreach (KeyValuePair<string, Stopwatch> keyValuePair in Profiler.timeSegments)
			{
				array[num++] = keyValuePair.Key + " " + keyValuePair.Value.ElapsedMilliseconds.ToString() + " [ms]";
			}
			return array;
		}

		// Token: 0x04000314 RID: 788
		private static readonly Dictionary<string, Stopwatch> timeSegments = new Dictionary<string, Stopwatch>();
	}
}
