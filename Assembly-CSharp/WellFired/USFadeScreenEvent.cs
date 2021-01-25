using System;
using UnityEngine;

namespace WellFired
{
	// Token: 0x02000869 RID: 2153
	[USequencerFriendlyName("Fade Screen")]
	[USequencerEvent("Fullscreen/Fade Screen")]
	public class USFadeScreenEvent : USEventBase
	{
		// Token: 0x06003403 RID: 13315 RVA: 0x0000264F File Offset: 0x0000084F
		public override void FireEvent()
		{
		}

		// Token: 0x06003404 RID: 13316 RVA: 0x0018F278 File Offset: 0x0018D478
		public override void ProcessEvent(float deltaTime)
		{
			this.currentCurveSampleTime = deltaTime;
			if (!USFadeScreenEvent.texture)
			{
				USFadeScreenEvent.texture = new Texture2D(1, 1, 5, false);
			}
			float num = this.fadeCurve.Evaluate(this.currentCurveSampleTime);
			num = Mathf.Min(Mathf.Max(0f, num), 1f);
			USFadeScreenEvent.texture.SetPixel(0, 0, new Color(this.fadeColour.r, this.fadeColour.g, this.fadeColour.b, num));
			USFadeScreenEvent.texture.Apply();
		}

		// Token: 0x06003405 RID: 13317 RVA: 0x0018F310 File Offset: 0x0018D510
		public override void EndEvent()
		{
			if (!USFadeScreenEvent.texture)
			{
				USFadeScreenEvent.texture = new Texture2D(1, 1, 5, false);
			}
			float num = this.fadeCurve.Evaluate(this.fadeCurve.keys[this.fadeCurve.length - 1].time);
			num = Mathf.Min(Mathf.Max(0f, num), 1f);
			USFadeScreenEvent.texture.SetPixel(0, 0, new Color(this.fadeColour.r, this.fadeColour.g, this.fadeColour.b, num));
			USFadeScreenEvent.texture.Apply();
		}

		// Token: 0x06003406 RID: 13318 RVA: 0x00020B40 File Offset: 0x0001ED40
		public override void StopEvent()
		{
			this.UndoEvent();
		}

		// Token: 0x06003407 RID: 13319 RVA: 0x0018F3BC File Offset: 0x0018D5BC
		public override void UndoEvent()
		{
			this.currentCurveSampleTime = 0f;
			if (!USFadeScreenEvent.texture)
			{
				USFadeScreenEvent.texture = new Texture2D(1, 1, 5, false);
			}
			USFadeScreenEvent.texture.SetPixel(0, 0, new Color(this.fadeColour.r, this.fadeColour.g, this.fadeColour.b, 0f));
			USFadeScreenEvent.texture.Apply();
		}

		// Token: 0x06003408 RID: 13320 RVA: 0x0018F434 File Offset: 0x0018D634
		private void OnGUI()
		{
			if (!base.Sequence.IsPlaying)
			{
				return;
			}
			float num = 0f;
			foreach (Keyframe keyframe in this.fadeCurve.keys)
			{
				if (keyframe.time > num)
				{
					num = keyframe.time;
				}
			}
			base.Duration = num;
			if (!USFadeScreenEvent.texture)
			{
				return;
			}
			int depth = GUI.depth;
			GUI.depth = this.uiLayer;
			GUI.DrawTexture(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), USFadeScreenEvent.texture);
			GUI.depth = depth;
		}

		// Token: 0x06003409 RID: 13321 RVA: 0x0018F4F0 File Offset: 0x0018D6F0
		private void OnEnable()
		{
			if (USFadeScreenEvent.texture == null)
			{
				USFadeScreenEvent.texture = new Texture2D(1, 1, 5, false);
			}
			USFadeScreenEvent.texture.SetPixel(0, 0, new Color(this.fadeColour.r, this.fadeColour.g, this.fadeColour.b, 0f));
			USFadeScreenEvent.texture.Apply();
		}

		// Token: 0x04004010 RID: 16400
		public USEventBase.UILayer uiLayer;

		// Token: 0x04004011 RID: 16401
		public AnimationCurve fadeCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f),
			new Keyframe(3f, 1f),
			new Keyframe(4f, 0f)
		});

		// Token: 0x04004012 RID: 16402
		public Color fadeColour = Color.black;

		// Token: 0x04004013 RID: 16403
		private float currentCurveSampleTime;

		// Token: 0x04004014 RID: 16404
		public static Texture2D texture;
	}
}
