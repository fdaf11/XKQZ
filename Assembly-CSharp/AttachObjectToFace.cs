using System;
using UnityEngine;

// Token: 0x02000670 RID: 1648
[ExecuteInEditMode]
public class AttachObjectToFace : MonoBehaviour
{
	// Token: 0x0600284F RID: 10319 RVA: 0x0013F08C File Offset: 0x0013D28C
	private void Awake()
	{
		this.mTransform = base.transform;
		if (this.meshAnimator)
		{
			this.mMeshAnimatorTransform = this.meshAnimator.transform;
			this.triangles = this.meshAnimator.GetComponent<MeshFilter>().sharedMesh.triangles;
			this.vertices = this.meshAnimator.GetComponent<MeshFilter>().sharedMesh.vertices;
		}
	}

	// Token: 0x06002850 RID: 10320 RVA: 0x0013F0FC File Offset: 0x0013D2FC
	private void LateUpdate()
	{
		if (this.meshAnimator)
		{
			if (!this.mMeshAnimatorTransform)
			{
				this.mMeshAnimatorTransform = this.meshAnimator.transform;
			}
			if (this.triangles.Length == 0)
			{
				this.triangles = this.meshAnimator.GetComponent<MeshFilter>().sharedMesh.triangles;
				this.vertices = this.meshAnimator.GetComponent<MeshFilter>().sharedMesh.vertices;
			}
			this.faceIndex = Mathf.Clamp(this.faceIndex, 0, this.triangles.Length);
			Vector3 vector;
			Vector3 vector2;
			Vector3 vector3;
			if (!Application.isPlaying || !this.meshAnimator || !this.meshAnimator.currentAnimation)
			{
				vector = this.vertices[this.triangles[this.faceIndex]];
				vector2 = this.vertices[this.triangles[this.faceIndex + 1]];
				vector3 = this.vertices[this.triangles[this.faceIndex + 2]];
			}
			else
			{
				vector = this.meshAnimator.currentAnimation.GetFrame(this.meshAnimator.currentFrame)[this.triangles[this.faceIndex]];
				vector2 = this.meshAnimator.currentAnimation.GetFrame(this.meshAnimator.currentFrame)[this.triangles[this.faceIndex + 1]];
				vector3 = this.meshAnimator.currentAnimation.GetFrame(this.meshAnimator.currentFrame)[this.triangles[this.faceIndex + 2]];
			}
			Vector3 vector4 = Vector3.zero;
			vector4 += vector;
			vector4 += vector2;
			vector4 += vector3;
			vector4 /= 3f;
			Vector3 vector5 = vector - vector2;
			Vector3 vector6 = vector - vector3;
			Vector3 vector7 = Vector3.Cross(vector5, vector6);
			vector7.Normalize();
			Quaternion quaternion = Quaternion.identity;
			if (vector7 != Vector3.zero)
			{
				quaternion = Quaternion.LookRotation(vector7);
			}
			this.mTransform.position = this.mMeshAnimatorTransform.TransformPoint(vector4) + quaternion * this.offset;
			this.mTransform.rotation = quaternion;
			this.mTransform.Rotate(this.rotationOffset);
		}
	}

	// Token: 0x04003291 RID: 12945
	public MeshAnimator meshAnimator;

	// Token: 0x04003292 RID: 12946
	public int faceIndex;

	// Token: 0x04003293 RID: 12947
	public Vector3 offset;

	// Token: 0x04003294 RID: 12948
	public Vector3 rotationOffset;

	// Token: 0x04003295 RID: 12949
	public bool drawFaceDebugInfo;

	// Token: 0x04003296 RID: 12950
	public Color debugColor = Color.black;

	// Token: 0x04003297 RID: 12951
	private Transform mTransform;

	// Token: 0x04003298 RID: 12952
	private Transform mMeshAnimatorTransform;

	// Token: 0x04003299 RID: 12953
	private int[] triangles;

	// Token: 0x0400329A RID: 12954
	private Vector3[] vertices;
}
