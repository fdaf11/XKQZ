using System;
using System.Collections.Generic;
using UnityEngine;

namespace Xft
{
	// Token: 0x0200057E RID: 1406
	[ExecuteInEditMode]
	public class XftCameraEffectComp : MonoBehaviour
	{
		// Token: 0x06002337 RID: 9015 RVA: 0x00014DE8 File Offset: 0x00012FE8
		private void Awake()
		{
			base.enabled = false;
		}

		// Token: 0x06002338 RID: 9016 RVA: 0x00017513 File Offset: 0x00015713
		public void ResetEvent(CameraEffectEvent e)
		{
			if (this.m_eventList.Contains(e))
			{
				this.m_eventList.Remove(e);
			}
			if (this.m_eventList.Count == 0)
			{
				base.enabled = false;
			}
		}

		// Token: 0x06002339 RID: 9017 RVA: 0x00114D9C File Offset: 0x00112F9C
		public void AddEvent(CameraEffectEvent e)
		{
			for (int i = 0; i < this.m_eventList.Count; i++)
			{
				if (this.m_eventList[i].EffectType == e.EffectType)
				{
					Debug.LogWarning("can't add camera effect duplicated:" + e.EffectType);
					return;
				}
			}
			this.m_eventList.Add(e);
			this.m_eventList.Sort();
			base.enabled = true;
		}

		// Token: 0x0600233A RID: 9018 RVA: 0x00114E1C File Offset: 0x0011301C
		private void OnPreRender()
		{
			for (int i = 0; i < this.m_eventList.Count; i++)
			{
				this.m_eventList[i].OnPreRender();
			}
		}

		// Token: 0x0600233B RID: 9019 RVA: 0x00114E58 File Offset: 0x00113058
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (this.m_eventList.Count == 0)
			{
				return;
			}
			RenderTexture temporary = RenderTexture.GetTemporary(source.width, source.height, 0);
			RenderTexture temporary2 = RenderTexture.GetTemporary(source.width, source.height, 0);
			this.m_eventList[0].OnRenderImage(source, temporary);
			bool flag = true;
			for (int i = 1; i < this.m_eventList.Count; i++)
			{
				if (flag)
				{
					this.m_eventList[i].OnRenderImage(temporary, temporary2);
					temporary.DiscardContents();
				}
				else
				{
					this.m_eventList[i].OnRenderImage(temporary2, temporary);
					temporary2.DiscardContents();
				}
				flag = !flag;
			}
			if (flag)
			{
				Graphics.Blit(temporary, destination);
			}
			else
			{
				Graphics.Blit(temporary2, destination);
			}
			RenderTexture.ReleaseTemporary(temporary);
			RenderTexture.ReleaseTemporary(temporary2);
		}

		// Token: 0x04002AB6 RID: 10934
		protected List<CameraEffectEvent> m_eventList = new List<CameraEffectEvent>();
	}
}
