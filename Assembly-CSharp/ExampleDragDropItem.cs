using System;
using UnityEngine;

// Token: 0x0200042F RID: 1071
[AddComponentMenu("NGUI/Examples/Drag and Drop Item (Example)")]
public class ExampleDragDropItem : UIDragDropItem
{
	// Token: 0x060019EE RID: 6638 RVA: 0x000D01C8 File Offset: 0x000CE3C8
	protected override void OnDragDropRelease(GameObject surface)
	{
		if (surface != null)
		{
			ExampleDragDropSurface component = surface.GetComponent<ExampleDragDropSurface>();
			if (component != null)
			{
				GameObject gameObject = NGUITools.AddChild(component.gameObject, this.prefab);
				gameObject.transform.localScale = component.transform.localScale;
				Transform transform = gameObject.transform;
				transform.position = UICamera.lastWorldPosition;
				if (component.rotatePlacedObject)
				{
					transform.rotation = Quaternion.LookRotation(UICamera.lastHit.normal) * Quaternion.Euler(90f, 0f, 0f);
				}
				NGUITools.Destroy(base.gameObject);
				return;
			}
		}
		base.OnDragDropRelease(surface);
	}

	// Token: 0x04001EB5 RID: 7861
	public GameObject prefab;
}
