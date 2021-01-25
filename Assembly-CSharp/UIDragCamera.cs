using System;
using UnityEngine;

// Token: 0x02000451 RID: 1105
[ExecuteInEditMode]
[AddComponentMenu("NGUI/Interaction/Drag Camera")]
public class UIDragCamera : MonoBehaviour
{
	// Token: 0x06001A88 RID: 6792 RVA: 0x00011675 File Offset: 0x0000F875
	private void Awake()
	{
		if (this.draggableCamera == null)
		{
			this.draggableCamera = NGUITools.FindInParents<UIDraggableCamera>(base.gameObject);
		}
	}

	// Token: 0x06001A89 RID: 6793 RVA: 0x00011699 File Offset: 0x0000F899
	private void OnPress(bool isPressed)
	{
		if (base.enabled && NGUITools.GetActive(base.gameObject) && this.draggableCamera != null)
		{
			this.draggableCamera.Press(isPressed);
		}
	}

	// Token: 0x06001A8A RID: 6794 RVA: 0x000116D3 File Offset: 0x0000F8D3
	private void OnDrag(Vector2 delta)
	{
		if (base.enabled && NGUITools.GetActive(base.gameObject) && this.draggableCamera != null)
		{
			this.draggableCamera.Drag(delta);
		}
	}

	// Token: 0x06001A8B RID: 6795 RVA: 0x0001170D File Offset: 0x0000F90D
	private void OnScroll(float delta)
	{
		if (base.enabled && NGUITools.GetActive(base.gameObject) && this.draggableCamera != null)
		{
			this.draggableCamera.Scroll(delta);
		}
	}

	// Token: 0x04001F49 RID: 8009
	public UIDraggableCamera draggableCamera;
}
