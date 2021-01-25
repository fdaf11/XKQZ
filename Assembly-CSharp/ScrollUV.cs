using System;
using UnityEngine;

// Token: 0x02000145 RID: 325
public class ScrollUV : MonoBehaviour
{
	// Token: 0x060006E8 RID: 1768 RVA: 0x00047C6C File Offset: 0x00045E6C
	private void Update()
	{
		if (this.direction == ScrollUV.Direction.Vertical)
		{
			this.offset.y = this.offset.y + this.speed * Time.deltaTime;
			if (this.offset.y >= 1f)
			{
				this.offset.y = 0f;
			}
		}
		else
		{
			this.offset.x = this.offset.x + this.speed * Time.deltaTime;
			if (this.offset.x >= 1f)
			{
				this.offset.x = 0f;
			}
		}
		base.renderer.material.mainTextureOffset = this.offset;
	}

	// Token: 0x04000751 RID: 1873
	public float speed;

	// Token: 0x04000752 RID: 1874
	public ScrollUV.Direction direction;

	// Token: 0x04000753 RID: 1875
	private Vector2 offset;

	// Token: 0x02000146 RID: 326
	public enum Direction
	{
		// Token: 0x04000755 RID: 1877
		Vertical,
		// Token: 0x04000756 RID: 1878
		Horizontal
	}
}
