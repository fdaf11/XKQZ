using System;
using UnityEngine;

namespace WellFired
{
	// Token: 0x02000867 RID: 2151
	[USequencerEvent("Fullscreen/Display Image")]
	[USequencerFriendlyName("Display Image")]
	public class USDisplayImageEvent : USEventBase
	{
		// Token: 0x060033FB RID: 13307 RVA: 0x00020B05 File Offset: 0x0001ED05
		public override void FireEvent()
		{
			if (!this.displayImage)
			{
				Debug.LogWarning("Trying to use a DisplayImage Event, but you didn't give it an image to display", this);
			}
		}

		// Token: 0x060033FC RID: 13308 RVA: 0x00020B22 File Offset: 0x0001ED22
		public override void ProcessEvent(float deltaTime)
		{
			this.currentCurveSampleTime = deltaTime;
		}

		// Token: 0x060033FD RID: 13309 RVA: 0x0018EEE4 File Offset: 0x0018D0E4
		public override void EndEvent()
		{
			float num = this.fadeCurve.Evaluate(this.fadeCurve.keys[this.fadeCurve.length - 1].time);
			num = Mathf.Min(Mathf.Max(0f, num), 1f);
		}

		// Token: 0x060033FE RID: 13310 RVA: 0x00020B2B File Offset: 0x0001ED2B
		public override void StopEvent()
		{
			this.UndoEvent();
		}

		// Token: 0x060033FF RID: 13311 RVA: 0x00020B33 File Offset: 0x0001ED33
		public override void UndoEvent()
		{
			this.currentCurveSampleTime = 0f;
		}

		// Token: 0x06003400 RID: 13312 RVA: 0x0018EF38 File Offset: 0x0018D138
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
			float num2 = this.fadeCurve.Evaluate(this.currentCurveSampleTime);
			num2 = Mathf.Min(Mathf.Max(0f, num2), 1f);
			if (!this.displayImage)
			{
				return;
			}
			Rect rect;
			rect..ctor((float)Screen.width * 0.5f, (float)Screen.height * 0.5f, (float)this.displayImage.width, (float)this.displayImage.height);
			switch (this.displayPosition)
			{
			case USDisplayImageEvent.ImageDisplayPosition.TopLeft:
				rect.x = 0f;
				rect.y = 0f;
				break;
			case USDisplayImageEvent.ImageDisplayPosition.TopRight:
				rect.x = (float)Screen.width;
				rect.y = 0f;
				break;
			case USDisplayImageEvent.ImageDisplayPosition.BottomLeft:
				rect.x = 0f;
				rect.y = (float)Screen.height;
				break;
			case USDisplayImageEvent.ImageDisplayPosition.BottomRight:
				rect.x = (float)Screen.width;
				rect.y = (float)Screen.height;
				break;
			}
			switch (this.anchorPosition)
			{
			case USDisplayImageEvent.ImageDisplayPosition.Center:
				rect.x -= (float)this.displayImage.width * 0.5f;
				rect.y -= (float)this.displayImage.height * 0.5f;
				break;
			case USDisplayImageEvent.ImageDisplayPosition.TopRight:
				rect.x -= (float)this.displayImage.width;
				break;
			case USDisplayImageEvent.ImageDisplayPosition.BottomLeft:
				rect.y -= (float)this.displayImage.height;
				break;
			case USDisplayImageEvent.ImageDisplayPosition.BottomRight:
				rect.x -= (float)this.displayImage.width;
				rect.y -= (float)this.displayImage.height;
				break;
			}
			GUI.depth = this.uiLayer;
			Color color = GUI.color;
			GUI.color = new Color(1f, 1f, 1f, num2);
			GUI.DrawTexture(rect, this.displayImage);
			GUI.color = color;
		}

		// Token: 0x04004004 RID: 16388
		public USEventBase.UILayer uiLayer;

		// Token: 0x04004005 RID: 16389
		public AnimationCurve fadeCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f),
			new Keyframe(3f, 1f),
			new Keyframe(4f, 0f)
		});

		// Token: 0x04004006 RID: 16390
		public Texture2D displayImage;

		// Token: 0x04004007 RID: 16391
		public USDisplayImageEvent.ImageDisplayPosition displayPosition;

		// Token: 0x04004008 RID: 16392
		public USDisplayImageEvent.ImageDisplayPosition anchorPosition;

		// Token: 0x04004009 RID: 16393
		private float currentCurveSampleTime;

		// Token: 0x02000868 RID: 2152
		public enum ImageDisplayPosition
		{
			// Token: 0x0400400B RID: 16395
			Center,
			// Token: 0x0400400C RID: 16396
			TopLeft,
			// Token: 0x0400400D RID: 16397
			TopRight,
			// Token: 0x0400400E RID: 16398
			BottomLeft,
			// Token: 0x0400400F RID: 16399
			BottomRight
		}
	}
}
