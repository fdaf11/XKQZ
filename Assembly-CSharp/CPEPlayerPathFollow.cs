using System;
using UnityEngine;

// Token: 0x02000052 RID: 82
public class CPEPlayerPathFollow : MonoBehaviour
{
	// Token: 0x06000176 RID: 374 RVA: 0x0002551C File Offset: 0x0002371C
	private void Start()
	{
		float nearestPoint = this.path.GetNearestPoint(this.player.position, this.ignoreNormalise, 5);
		this.lastPercent = nearestPoint;
		Vector3 pathPosition = this.path.GetPathPosition(nearestPoint, this.ignoreNormalise);
		this.cam.position = pathPosition;
		switch (this._orientationMode)
		{
		case CPEPlayerPathFollow.OrientationModes.lookAtTarget:
			this.cam.rotation = Quaternion.LookRotation(this.player.position - this.cam.position);
			break;
		case CPEPlayerPathFollow.OrientationModes.lookAtPathDirection:
			this.cam.rotation = Quaternion.LookRotation(this.path.GetPathDirection(nearestPoint));
			break;
		}
	}

	// Token: 0x06000177 RID: 375 RVA: 0x000255E4 File Offset: 0x000237E4
	private void LateUpdate()
	{
		float nearestPoint = this.path.GetNearestPoint(this.player.position, this.ignoreNormalise, this.accuracy);
		float num = nearestPoint - this.lastPercent;
		if (num > 0.5f)
		{
			this.lastPercent += 1f;
		}
		else if (num < -0.5f)
		{
			this.lastPercent += -1f;
		}
		float percentage = nearestPoint;
		this.lastPercent = percentage;
		Vector3 pathPosition = this.path.GetPathPosition(percentage, this.ignoreNormalise);
		Vector3 vector = -this.path.GetPathDirection(percentage, !this.ignoreNormalise);
		this.cam.position = pathPosition + vector * this.pathLag;
		switch (this._orientationMode)
		{
		case CPEPlayerPathFollow.OrientationModes.lookAtTarget:
			this.cam.rotation = Quaternion.LookRotation(this.player.position - this.cam.position);
			break;
		case CPEPlayerPathFollow.OrientationModes.lookAtPathDirection:
			this.cam.rotation = Quaternion.LookRotation(this.path.GetPathDirection(percentage));
			break;
		}
	}

	// Token: 0x0400012C RID: 300
	[SerializeField]
	private CPEPlayerPathFollow.OrientationModes _orientationMode = CPEPlayerPathFollow.OrientationModes.lookAtTarget;

	// Token: 0x0400012D RID: 301
	[SerializeField]
	private Transform player;

	// Token: 0x0400012E RID: 302
	[SerializeField]
	private Transform cam;

	// Token: 0x0400012F RID: 303
	public CameraPath path;

	// Token: 0x04000130 RID: 304
	private float lastPercent;

	// Token: 0x04000131 RID: 305
	private bool ignoreNormalise;

	// Token: 0x04000132 RID: 306
	private int accuracy = 3;

	// Token: 0x04000133 RID: 307
	[SerializeField]
	private float pathLag;

	// Token: 0x02000053 RID: 83
	public enum OrientationModes
	{
		// Token: 0x04000135 RID: 309
		none,
		// Token: 0x04000136 RID: 310
		lookAtTarget,
		// Token: 0x04000137 RID: 311
		lookAtPathDirection
	}
}
