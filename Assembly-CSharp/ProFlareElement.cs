using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000689 RID: 1673
[Serializable]
public class ProFlareElement
{
	// Token: 0x0400331A RID: 13082
	public bool Editing;

	// Token: 0x0400331B RID: 13083
	public bool Visible = true;

	// Token: 0x0400331C RID: 13084
	public int elementTextureID;

	// Token: 0x0400331D RID: 13085
	public string SpriteName;

	// Token: 0x0400331E RID: 13086
	public ProFlare flare;

	// Token: 0x0400331F RID: 13087
	public ProFlareAtlas flareAtlas;

	// Token: 0x04003320 RID: 13088
	public float Brightness = 1f;

	// Token: 0x04003321 RID: 13089
	public float Scale = 1f;

	// Token: 0x04003322 RID: 13090
	public float ScaleRandom;

	// Token: 0x04003323 RID: 13091
	public float ScaleFinal = 1f;

	// Token: 0x04003324 RID: 13092
	public Vector4 RandomColorAmount = Vector4.zero;

	// Token: 0x04003325 RID: 13093
	public float position;

	// Token: 0x04003326 RID: 13094
	public bool useRangeOffset;

	// Token: 0x04003327 RID: 13095
	public float SubElementPositionRange_Min = -1f;

	// Token: 0x04003328 RID: 13096
	public float SubElementPositionRange_Max = 1f;

	// Token: 0x04003329 RID: 13097
	public float SubElementAngleRange_Min = -180f;

	// Token: 0x0400332A RID: 13098
	public float SubElementAngleRange_Max = 180f;

	// Token: 0x0400332B RID: 13099
	public Vector3 OffsetPosition;

	// Token: 0x0400332C RID: 13100
	public Vector3 Anamorphic = Vector3.zero;

	// Token: 0x0400332D RID: 13101
	public Vector3 OffsetPostion = Vector3.zero;

	// Token: 0x0400332E RID: 13102
	public float angle;

	// Token: 0x0400332F RID: 13103
	public float FinalAngle;

	// Token: 0x04003330 RID: 13104
	public bool useRandomAngle;

	// Token: 0x04003331 RID: 13105
	public bool useStarRotation;

	// Token: 0x04003332 RID: 13106
	public float AngleRandom_Min;

	// Token: 0x04003333 RID: 13107
	public float AngleRandom_Max;

	// Token: 0x04003334 RID: 13108
	public bool OrientToSource;

	// Token: 0x04003335 RID: 13109
	public bool rotateToFlare;

	// Token: 0x04003336 RID: 13110
	public float rotationSpeed;

	// Token: 0x04003337 RID: 13111
	public float rotationOverTime;

	// Token: 0x04003338 RID: 13112
	public bool useColorRange;

	// Token: 0x04003339 RID: 13113
	public Color ElementFinalColor;

	// Token: 0x0400333A RID: 13114
	public Color ElementTint = new Color(1f, 1f, 1f, 0.33f);

	// Token: 0x0400333B RID: 13115
	public Color SubElementColor_Start = Color.white;

	// Token: 0x0400333C RID: 13116
	public Color SubElementColor_End = Color.white;

	// Token: 0x0400333D RID: 13117
	public bool useScaleCurve;

	// Token: 0x0400333E RID: 13118
	public AnimationCurve ScaleCurve = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0.1f),
		new Keyframe(0.5f, 1f),
		new Keyframe(1f, 0.1f)
	});

	// Token: 0x0400333F RID: 13119
	public bool OverrideDynamicEdgeBoost;

	// Token: 0x04003340 RID: 13120
	public float DynamicEdgeBoostOverride = 1f;

	// Token: 0x04003341 RID: 13121
	public bool OverrideDynamicCenterBoost;

	// Token: 0x04003342 RID: 13122
	public float DynamicCenterBoostOverride = 1f;

	// Token: 0x04003343 RID: 13123
	public bool OverrideDynamicEdgeBrightness;

	// Token: 0x04003344 RID: 13124
	public float DynamicEdgeBrightnessOverride = 0.4f;

	// Token: 0x04003345 RID: 13125
	public bool OverrideDynamicCenterBrightness;

	// Token: 0x04003346 RID: 13126
	public float DynamicCenterBrightnessOverride = 0.4f;

	// Token: 0x04003347 RID: 13127
	public List<SubElement> subElements = new List<SubElement>();

	// Token: 0x04003348 RID: 13128
	public Vector2 size = Vector2.one;

	// Token: 0x04003349 RID: 13129
	public ProFlareElement.Type type;

	// Token: 0x0200068A RID: 1674
	public enum Type
	{
		// Token: 0x0400334B RID: 13131
		Single,
		// Token: 0x0400334C RID: 13132
		Multi
	}
}
