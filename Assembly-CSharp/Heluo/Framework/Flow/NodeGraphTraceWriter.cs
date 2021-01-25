using System;
using System.Diagnostics;
using Newtonsoft.Json.Serialization;
using UnityEngine;

namespace HeLuo.Framework.Flow
{
	// Token: 0x0200018C RID: 396
	internal class NodeGraphTraceWriter : ITraceWriter
	{
		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060007ED RID: 2029 RVA: 0x00006DCD File Offset: 0x00004FCD
		public TraceLevel LevelFilter
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x060007EE RID: 2030 RVA: 0x00049E80 File Offset: 0x00048080
		public void Trace(TraceLevel level, string message, Exception ex)
		{
			Debug.Log(string.Concat(new object[]
			{
				"[",
				level,
				"] ",
				message
			}));
			if (ex != null)
			{
				Debug.LogException(ex);
			}
			Debug.Log(string.Concat(new object[]
			{
				"Heap = ",
				Profiler.GetMonoHeapSize(),
				", Used = ",
				Profiler.GetMonoUsedSize()
			}));
		}
	}
}
