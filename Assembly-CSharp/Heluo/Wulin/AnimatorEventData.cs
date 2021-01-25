using System;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x020000E6 RID: 230
	[Serializable]
	public class AnimatorEventData
	{
		// Token: 0x060004E3 RID: 1251 RVA: 0x0003A2A0 File Offset: 0x000384A0
		public void OnEvent(GameObject go)
		{
			GameObject gameObject = (!(this.Target == null)) ? this.Target : go;
			switch (this.type)
			{
			case AnimatorEventData.ParamType.None:
				gameObject.SendMessage(this.Method);
				break;
			case AnimatorEventData.ParamType.Object:
				if (!(this.objParam is AudioClip))
				{
					gameObject.SendMessage(this.Method, this.objParam);
				}
				break;
			case AnimatorEventData.ParamType.String:
				gameObject.SendMessage(this.Method, this.strParam);
				break;
			case AnimatorEventData.ParamType.Int:
				gameObject.SendMessage(this.Method, this.intParam);
				break;
			case AnimatorEventData.ParamType.Float:
				gameObject.SendMessage(this.Method, this.floatParam);
				break;
			case AnimatorEventData.ParamType.Vector:
				gameObject.SendMessage(this.Method, this.vectorParam);
				break;
			default:
				gameObject.SendMessage(this.Method);
				break;
			}
			this.FireTime += 1f;
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x00005076 File Offset: 0x00003276
		public void Reset()
		{
			this.FireTime = this.Time;
		}

		// Token: 0x04000495 RID: 1173
		public int Layer;

		// Token: 0x04000496 RID: 1174
		public AnimationClip Animation;

		// Token: 0x04000497 RID: 1175
		public float Time;

		// Token: 0x04000498 RID: 1176
		public float FireTime;

		// Token: 0x04000499 RID: 1177
		public GameObject Target;

		// Token: 0x0400049A RID: 1178
		public string Method;

		// Token: 0x0400049B RID: 1179
		public AnimatorEventData.ParamType type;

		// Token: 0x0400049C RID: 1180
		public Object objParam;

		// Token: 0x0400049D RID: 1181
		public string strParam;

		// Token: 0x0400049E RID: 1182
		public int intParam;

		// Token: 0x0400049F RID: 1183
		public float floatParam;

		// Token: 0x040004A0 RID: 1184
		public Vector3 vectorParam;

		// Token: 0x020000E7 RID: 231
		public enum ParamType
		{
			// Token: 0x040004A2 RID: 1186
			None,
			// Token: 0x040004A3 RID: 1187
			Object,
			// Token: 0x040004A4 RID: 1188
			String,
			// Token: 0x040004A5 RID: 1189
			Int,
			// Token: 0x040004A6 RID: 1190
			Float,
			// Token: 0x040004A7 RID: 1191
			Vector
		}
	}
}
