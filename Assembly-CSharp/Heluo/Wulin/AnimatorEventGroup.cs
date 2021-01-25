using System;
using JsonFx.Json;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x020000E9 RID: 233
	public class AnimatorEventGroup
	{
		// Token: 0x060004ED RID: 1261 RVA: 0x0003A600 File Offset: 0x00038800
		public void Load(Transform t)
		{
			string text = this.animationAsset.Substring("Assets/Resources/".Length);
			text = text.Substring(0, text.Length - 4);
			AnimationClip clip = Resources.Load(text, typeof(AnimationClip)) as AnimationClip;
			foreach (AnimatorEvent animatorEvent in this.events)
			{
				animatorEvent.Load(t, clip);
			}
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x0003A678 File Offset: 0x00038878
		public void Load(Transform t, AnimationClip clip)
		{
			foreach (AnimatorEvent animatorEvent in this.events)
			{
				animatorEvent.Load(t, clip);
			}
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x0003A6AC File Offset: 0x000388AC
		public string GetSkillName()
		{
			if (this.animationAsset.Length == 0 || this.animationAsset == string.Empty)
			{
				this.modelname = string.Empty;
				return this.animationName;
			}
			int num = this.animationAsset.LastIndexOf("/");
			string text;
			if (num > 0)
			{
				text = this.animationAsset.Substring(num + 1);
			}
			else
			{
				text = this.animationAsset;
			}
			num = text.LastIndexOf(".");
			if (num > 0)
			{
				text = text.Substring(0, num);
			}
			num = text.LastIndexOf("@");
			string text2;
			if (num > 0)
			{
				this.modelname = text.Substring(0, num);
				text2 = text.Substring(num + 1);
			}
			else
			{
				this.modelname = string.Empty;
				text2 = text;
			}
			if (this.animationName.ToLower().CompareTo(text2.ToLower()) != 0)
			{
				this.animationName = text2;
			}
			return text;
		}

		// Token: 0x040004AE RID: 1198
		private const string ResourcePath = "Assets/Resources/";

		// Token: 0x040004AF RID: 1199
		public string filename;

		// Token: 0x040004B0 RID: 1200
		[JsonName("m_Ani_Asset")]
		public string animationAsset;

		// Token: 0x040004B1 RID: 1201
		[JsonName("m_Ani_name")]
		public string animationName;

		// Token: 0x040004B2 RID: 1202
		[JsonName("m_BoundSize")]
		public AnimatorEvent.Vector bound;

		// Token: 0x040004B3 RID: 1203
		[JsonName("m_Events")]
		public AnimatorEvent[] events;

		// Token: 0x040004B4 RID: 1204
		public string modelname;
	}
}
