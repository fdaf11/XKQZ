using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using UnityEngine;

// Token: 0x020004A4 RID: 1188
public static class NGUITools
{
	// Token: 0x17000251 RID: 593
	// (get) Token: 0x06001D0B RID: 7435 RVA: 0x000133DB File Offset: 0x000115DB
	// (set) Token: 0x06001D0C RID: 7436 RVA: 0x00013406 File Offset: 0x00011606
	public static float soundVolume
	{
		get
		{
			if (!NGUITools.mLoaded)
			{
				NGUITools.mLoaded = true;
				NGUITools.mGlobalVolume = PlayerPrefs.GetFloat("Sound", 1f);
			}
			return NGUITools.mGlobalVolume;
		}
		set
		{
			if (NGUITools.mGlobalVolume != value)
			{
				NGUITools.mLoaded = true;
				NGUITools.mGlobalVolume = value;
				PlayerPrefs.SetFloat("Sound", value);
			}
		}
	}

	// Token: 0x17000252 RID: 594
	// (get) Token: 0x06001D0D RID: 7437 RVA: 0x0001342A File Offset: 0x0001162A
	public static bool fileAccess
	{
		get
		{
			return Application.platform != 5 && Application.platform != 3;
		}
	}

	// Token: 0x06001D0E RID: 7438 RVA: 0x00013445 File Offset: 0x00011645
	public static AudioSource PlaySound(AudioClip clip)
	{
		return NGUITools.PlaySound(clip, 1f, 1f);
	}

	// Token: 0x06001D0F RID: 7439 RVA: 0x00013457 File Offset: 0x00011657
	public static AudioSource PlaySound(AudioClip clip, float volume)
	{
		return NGUITools.PlaySound(clip, volume, 1f);
	}

	// Token: 0x06001D10 RID: 7440 RVA: 0x000E3900 File Offset: 0x000E1B00
	public static AudioSource PlaySound(AudioClip clip, float volume, float pitch)
	{
		volume *= NGUITools.soundVolume;
		if (clip != null && volume > 0.01f)
		{
			if (NGUITools.mListener == null || !NGUITools.GetActive(NGUITools.mListener))
			{
				AudioListener[] array = Object.FindObjectsOfType(typeof(AudioListener)) as AudioListener[];
				if (array != null)
				{
					for (int i = 0; i < array.Length; i++)
					{
						if (NGUITools.GetActive(array[i]))
						{
							NGUITools.mListener = array[i];
							break;
						}
					}
				}
				if (NGUITools.mListener == null)
				{
					Camera camera = Camera.main;
					if (camera == null)
					{
						camera = (Object.FindObjectOfType(typeof(Camera)) as Camera);
					}
					if (camera != null)
					{
						NGUITools.mListener = camera.gameObject.AddComponent<AudioListener>();
					}
				}
			}
			if (NGUITools.mListener != null && NGUITools.mListener.enabled && NGUITools.GetActive(NGUITools.mListener.gameObject))
			{
				AudioSource audioSource = NGUITools.mListener.audio;
				if (audioSource == null)
				{
					audioSource = NGUITools.mListener.gameObject.AddComponent<AudioSource>();
				}
				audioSource.pitch = pitch;
				audioSource.PlayOneShot(clip, volume);
				return audioSource;
			}
		}
		return null;
	}

	// Token: 0x06001D11 RID: 7441 RVA: 0x00013465 File Offset: 0x00011665
	public static int RandomRange(int min, int max)
	{
		if (min == max)
		{
			return min;
		}
		return Random.Range(min, max + 1);
	}

	// Token: 0x06001D12 RID: 7442 RVA: 0x000E3A54 File Offset: 0x000E1C54
	public static string GetHierarchy(GameObject obj)
	{
		if (obj == null)
		{
			return string.Empty;
		}
		string text = obj.name;
		while (obj.transform.parent != null)
		{
			obj = obj.transform.parent.gameObject;
			text = obj.name + "\\" + text;
		}
		return text;
	}

	// Token: 0x06001D13 RID: 7443 RVA: 0x00013479 File Offset: 0x00011679
	public static T[] FindActive<T>() where T : Component
	{
		return Object.FindObjectsOfType(typeof(T)) as T[];
	}

	// Token: 0x06001D14 RID: 7444 RVA: 0x000E3ABC File Offset: 0x000E1CBC
	public static Camera FindCameraForLayer(int layer)
	{
		int num = 1 << layer;
		Camera camera;
		for (int i = 0; i < UICamera.list.size; i++)
		{
			camera = UICamera.list.buffer[i].cachedCamera;
			if (camera && (camera.cullingMask & num) != 0)
			{
				return camera;
			}
		}
		camera = Camera.main;
		if (camera && (camera.cullingMask & num) != 0)
		{
			return camera;
		}
		Camera[] array = new Camera[Camera.allCamerasCount];
		int allCameras = Camera.GetAllCameras(array);
		for (int j = 0; j < allCameras; j++)
		{
			camera = array[j];
			if (camera && camera.enabled && (camera.cullingMask & num) != 0)
			{
				return camera;
			}
		}
		return null;
	}

	// Token: 0x06001D15 RID: 7445 RVA: 0x0001348F File Offset: 0x0001168F
	public static void AddWidgetCollider(GameObject go)
	{
		NGUITools.AddWidgetCollider(go, false);
	}

	// Token: 0x06001D16 RID: 7446 RVA: 0x000E3B8C File Offset: 0x000E1D8C
	public static void AddWidgetCollider(GameObject go, bool considerInactive)
	{
		if (go != null)
		{
			Collider component = go.GetComponent<Collider>();
			BoxCollider boxCollider = component as BoxCollider;
			if (boxCollider != null)
			{
				NGUITools.UpdateWidgetCollider(boxCollider, considerInactive);
				return;
			}
			if (component != null)
			{
				return;
			}
			BoxCollider2D boxCollider2D = go.GetComponent<BoxCollider2D>();
			if (boxCollider2D != null)
			{
				NGUITools.UpdateWidgetCollider(boxCollider2D, considerInactive);
				return;
			}
			UICamera uicamera = UICamera.FindCameraForLayer(go.layer);
			if (uicamera != null && (uicamera.eventType == UICamera.EventType.World_2D || uicamera.eventType == UICamera.EventType.UI_2D))
			{
				boxCollider2D = go.AddComponent<BoxCollider2D>();
				boxCollider2D.isTrigger = true;
				UIWidget component2 = go.GetComponent<UIWidget>();
				if (component2 != null)
				{
					component2.autoResizeBoxCollider = true;
				}
				NGUITools.UpdateWidgetCollider(boxCollider2D, considerInactive);
				return;
			}
			boxCollider = go.AddComponent<BoxCollider>();
			boxCollider.isTrigger = true;
			UIWidget component3 = go.GetComponent<UIWidget>();
			if (component3 != null)
			{
				component3.autoResizeBoxCollider = true;
			}
			NGUITools.UpdateWidgetCollider(boxCollider, considerInactive);
		}
	}

	// Token: 0x06001D17 RID: 7447 RVA: 0x00013498 File Offset: 0x00011698
	public static void UpdateWidgetCollider(GameObject go)
	{
		NGUITools.UpdateWidgetCollider(go, false);
	}

	// Token: 0x06001D18 RID: 7448 RVA: 0x000E3C84 File Offset: 0x000E1E84
	public static void UpdateWidgetCollider(GameObject go, bool considerInactive)
	{
		if (go != null)
		{
			BoxCollider component = go.GetComponent<BoxCollider>();
			if (component != null)
			{
				NGUITools.UpdateWidgetCollider(component, considerInactive);
				return;
			}
			BoxCollider2D component2 = go.GetComponent<BoxCollider2D>();
			if (component2 != null)
			{
				NGUITools.UpdateWidgetCollider(component2, considerInactive);
			}
		}
	}

	// Token: 0x06001D19 RID: 7449 RVA: 0x000E3CD4 File Offset: 0x000E1ED4
	public static void UpdateWidgetCollider(BoxCollider box, bool considerInactive)
	{
		if (box != null)
		{
			GameObject gameObject = box.gameObject;
			UIWidget component = gameObject.GetComponent<UIWidget>();
			if (component != null)
			{
				Vector4 drawRegion = component.drawRegion;
				if (drawRegion.x != 0f || drawRegion.y != 0f || drawRegion.z != 1f || drawRegion.w != 1f)
				{
					Vector4 drawingDimensions = component.drawingDimensions;
					box.center = new Vector3((drawingDimensions.x + drawingDimensions.z) * 0.5f, (drawingDimensions.y + drawingDimensions.w) * 0.5f);
					box.size = new Vector3(drawingDimensions.z - drawingDimensions.x, drawingDimensions.w - drawingDimensions.y);
				}
				else
				{
					Vector3[] localCorners = component.localCorners;
					box.center = Vector3.Lerp(localCorners[0], localCorners[2], 0.5f);
					box.size = localCorners[2] - localCorners[0];
				}
			}
			else
			{
				Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(gameObject.transform, considerInactive);
				box.center = bounds.center;
				box.size = new Vector3(bounds.size.x, bounds.size.y, 0f);
			}
		}
	}

	// Token: 0x06001D1A RID: 7450 RVA: 0x000E3E64 File Offset: 0x000E2064
	public static void UpdateWidgetCollider(BoxCollider2D box, bool considerInactive)
	{
		if (box != null)
		{
			GameObject gameObject = box.gameObject;
			UIWidget component = gameObject.GetComponent<UIWidget>();
			if (component != null)
			{
				Vector3[] localCorners = component.localCorners;
				box.center = Vector3.Lerp(localCorners[0], localCorners[2], 0.5f);
				box.size = localCorners[2] - localCorners[0];
			}
			else
			{
				Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(gameObject.transform, considerInactive);
				box.center = bounds.center;
				box.size = new Vector2(bounds.size.x, bounds.size.y);
			}
		}
	}

	// Token: 0x06001D1B RID: 7451 RVA: 0x000E3F44 File Offset: 0x000E2144
	public static string GetTypeName<T>()
	{
		string text = typeof(T).ToString();
		if (text.StartsWith("UI"))
		{
			text = text.Substring(2);
		}
		else if (text.StartsWith("UnityEngine."))
		{
			text = text.Substring(12);
		}
		return text;
	}

	// Token: 0x06001D1C RID: 7452 RVA: 0x000E3F98 File Offset: 0x000E2198
	public static string GetTypeName(Object obj)
	{
		if (obj == null)
		{
			return "Null";
		}
		string text = obj.GetType().ToString();
		if (text.StartsWith("UI"))
		{
			text = text.Substring(2);
		}
		else if (text.StartsWith("UnityEngine."))
		{
			text = text.Substring(12);
		}
		return text;
	}

	// Token: 0x06001D1D RID: 7453 RVA: 0x0000264F File Offset: 0x0000084F
	public static void RegisterUndo(Object obj, string name)
	{
	}

	// Token: 0x06001D1E RID: 7454 RVA: 0x0000264F File Offset: 0x0000084F
	public static void SetDirty(Object obj)
	{
	}

	// Token: 0x06001D1F RID: 7455 RVA: 0x000134A1 File Offset: 0x000116A1
	public static GameObject AddChild(GameObject parent)
	{
		return NGUITools.AddChild(parent, true);
	}

	// Token: 0x06001D20 RID: 7456 RVA: 0x000E3FFC File Offset: 0x000E21FC
	public static GameObject AddChild(GameObject parent, bool undo)
	{
		GameObject gameObject = new GameObject();
		if (parent != null)
		{
			Transform transform = gameObject.transform;
			transform.parent = parent.transform;
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
			transform.localScale = Vector3.one;
			gameObject.layer = parent.layer;
		}
		return gameObject;
	}

	// Token: 0x06001D21 RID: 7457 RVA: 0x000E405C File Offset: 0x000E225C
	public static GameObject AddChild(GameObject parent, GameObject prefab)
	{
		GameObject gameObject = Object.Instantiate(prefab) as GameObject;
		if (gameObject != null && parent != null)
		{
			Transform transform = gameObject.transform;
			transform.parent = parent.transform;
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
			transform.localScale = Vector3.one;
			gameObject.layer = parent.layer;
		}
		return gameObject;
	}

	// Token: 0x06001D22 RID: 7458 RVA: 0x000E40D0 File Offset: 0x000E22D0
	public static int CalculateRaycastDepth(GameObject go)
	{
		UIWidget component = go.GetComponent<UIWidget>();
		if (component != null)
		{
			return component.raycastDepth;
		}
		UIWidget[] componentsInChildren = go.GetComponentsInChildren<UIWidget>();
		if (componentsInChildren.Length == 0)
		{
			return 0;
		}
		int num = int.MaxValue;
		int i = 0;
		int num2 = componentsInChildren.Length;
		while (i < num2)
		{
			if (componentsInChildren[i].enabled)
			{
				num = Mathf.Min(num, componentsInChildren[i].raycastDepth);
			}
			i++;
		}
		return num;
	}

	// Token: 0x06001D23 RID: 7459 RVA: 0x000E4144 File Offset: 0x000E2344
	public static int CalculateNextDepth(GameObject go)
	{
		int num = -1;
		UIWidget[] componentsInChildren = go.GetComponentsInChildren<UIWidget>();
		int i = 0;
		int num2 = componentsInChildren.Length;
		while (i < num2)
		{
			num = Mathf.Max(num, componentsInChildren[i].depth);
			i++;
		}
		return num + 1;
	}

	// Token: 0x06001D24 RID: 7460 RVA: 0x000E4184 File Offset: 0x000E2384
	public static int CalculateNextDepth(GameObject go, bool ignoreChildrenWithColliders)
	{
		if (ignoreChildrenWithColliders)
		{
			int num = -1;
			UIWidget[] componentsInChildren = go.GetComponentsInChildren<UIWidget>();
			int i = 0;
			int num2 = componentsInChildren.Length;
			while (i < num2)
			{
				UIWidget uiwidget = componentsInChildren[i];
				if (!(uiwidget.cachedGameObject != go) || (!(uiwidget.collider != null) && !(uiwidget.GetComponent<Collider2D>() != null)))
				{
					num = Mathf.Max(num, uiwidget.depth);
				}
				i++;
			}
			return num + 1;
		}
		return NGUITools.CalculateNextDepth(go);
	}

	// Token: 0x06001D25 RID: 7461 RVA: 0x000E4210 File Offset: 0x000E2410
	public static int AdjustDepth(GameObject go, int adjustment)
	{
		if (!(go != null))
		{
			return 0;
		}
		UIPanel uipanel = go.GetComponent<UIPanel>();
		if (uipanel != null)
		{
			foreach (UIPanel uipanel2 in go.GetComponentsInChildren<UIPanel>(true))
			{
				uipanel2.depth += adjustment;
			}
			return 1;
		}
		uipanel = NGUITools.FindInParents<UIPanel>(go);
		if (uipanel == null)
		{
			return 0;
		}
		UIWidget[] componentsInChildren2 = go.GetComponentsInChildren<UIWidget>(true);
		int j = 0;
		int num = componentsInChildren2.Length;
		while (j < num)
		{
			UIWidget uiwidget = componentsInChildren2[j];
			if (!(uiwidget.panel != uipanel))
			{
				uiwidget.depth += adjustment;
			}
			j++;
		}
		return 2;
	}

	// Token: 0x06001D26 RID: 7462 RVA: 0x000E42D8 File Offset: 0x000E24D8
	public static void BringForward(GameObject go)
	{
		int num = NGUITools.AdjustDepth(go, 1000);
		if (num == 1)
		{
			NGUITools.NormalizePanelDepths();
		}
		else if (num == 2)
		{
			NGUITools.NormalizeWidgetDepths();
		}
	}

	// Token: 0x06001D27 RID: 7463 RVA: 0x000E4310 File Offset: 0x000E2510
	public static void PushBack(GameObject go)
	{
		int num = NGUITools.AdjustDepth(go, -1000);
		if (num == 1)
		{
			NGUITools.NormalizePanelDepths();
		}
		else if (num == 2)
		{
			NGUITools.NormalizeWidgetDepths();
		}
	}

	// Token: 0x06001D28 RID: 7464 RVA: 0x000134AA File Offset: 0x000116AA
	public static void NormalizeDepths()
	{
		NGUITools.NormalizeWidgetDepths();
		NGUITools.NormalizePanelDepths();
	}

	// Token: 0x06001D29 RID: 7465 RVA: 0x000134B6 File Offset: 0x000116B6
	public static void NormalizeWidgetDepths()
	{
		NGUITools.NormalizeWidgetDepths(NGUITools.FindActive<UIWidget>());
	}

	// Token: 0x06001D2A RID: 7466 RVA: 0x000134C2 File Offset: 0x000116C2
	public static void NormalizeWidgetDepths(GameObject go)
	{
		NGUITools.NormalizeWidgetDepths(go.GetComponentsInChildren<UIWidget>());
	}

	// Token: 0x06001D2B RID: 7467 RVA: 0x000E4348 File Offset: 0x000E2548
	public static void NormalizeWidgetDepths(UIWidget[] list)
	{
		int num = list.Length;
		if (num > 0)
		{
			Array.Sort<UIWidget>(list, new Comparison<UIWidget>(UIWidget.FullCompareFunc));
			int num2 = 0;
			int depth = list[0].depth;
			for (int i = 0; i < num; i++)
			{
				UIWidget uiwidget = list[i];
				if (uiwidget.depth == depth)
				{
					uiwidget.depth = num2;
				}
				else
				{
					depth = uiwidget.depth;
					num2 = (uiwidget.depth = num2 + 1);
				}
			}
		}
	}

	// Token: 0x06001D2C RID: 7468 RVA: 0x000E43C4 File Offset: 0x000E25C4
	public static void NormalizePanelDepths()
	{
		UIPanel[] array = NGUITools.FindActive<UIPanel>();
		int num = array.Length;
		if (num > 0)
		{
			Array.Sort<UIPanel>(array, new Comparison<UIPanel>(UIPanel.CompareFunc));
			int num2 = 0;
			int depth = array[0].depth;
			for (int i = 0; i < num; i++)
			{
				UIPanel uipanel = array[i];
				if (uipanel.depth == depth)
				{
					uipanel.depth = num2;
				}
				else
				{
					depth = uipanel.depth;
					num2 = (uipanel.depth = num2 + 1);
				}
			}
		}
	}

	// Token: 0x06001D2D RID: 7469 RVA: 0x000134CF File Offset: 0x000116CF
	public static UIPanel CreateUI(bool advanced3D)
	{
		return NGUITools.CreateUI(null, advanced3D, -1);
	}

	// Token: 0x06001D2E RID: 7470 RVA: 0x000134D9 File Offset: 0x000116D9
	public static UIPanel CreateUI(bool advanced3D, int layer)
	{
		return NGUITools.CreateUI(null, advanced3D, layer);
	}

	// Token: 0x06001D2F RID: 7471 RVA: 0x000E444C File Offset: 0x000E264C
	public static UIPanel CreateUI(Transform trans, bool advanced3D, int layer)
	{
		UIRoot uiroot = (!(trans != null)) ? null : NGUITools.FindInParents<UIRoot>(trans.gameObject);
		if (uiroot == null && UIRoot.list.Count > 0)
		{
			foreach (UIRoot uiroot2 in UIRoot.list)
			{
				if (uiroot2.gameObject.layer == layer)
				{
					uiroot = uiroot2;
					break;
				}
			}
		}
		if (uiroot != null)
		{
			UICamera componentInChildren = uiroot.GetComponentInChildren<UICamera>();
			if (componentInChildren != null && componentInChildren.camera.isOrthoGraphic == advanced3D)
			{
				trans = null;
				uiroot = null;
			}
		}
		if (uiroot == null)
		{
			GameObject gameObject = NGUITools.AddChild(null, false);
			uiroot = gameObject.AddComponent<UIRoot>();
			if (layer == -1)
			{
				layer = LayerMask.NameToLayer("UI");
			}
			if (layer == -1)
			{
				layer = LayerMask.NameToLayer("2D UI");
			}
			gameObject.layer = layer;
			if (advanced3D)
			{
				gameObject.name = "UI Root (3D)";
				uiroot.scalingStyle = UIRoot.Scaling.Constrained;
			}
			else
			{
				gameObject.name = "UI Root";
				uiroot.scalingStyle = UIRoot.Scaling.Flexible;
			}
		}
		UIPanel uipanel = uiroot.GetComponentInChildren<UIPanel>();
		if (uipanel == null)
		{
			Camera[] array = NGUITools.FindActive<Camera>();
			float num = -1f;
			bool flag = false;
			int num2 = 1 << uiroot.gameObject.layer;
			foreach (Camera camera in array)
			{
				if (camera.clearFlags == 2 || camera.clearFlags == 1)
				{
					flag = true;
				}
				num = Mathf.Max(num, camera.depth);
				camera.cullingMask &= ~num2;
			}
			Camera camera2 = NGUITools.AddChild<Camera>(uiroot.gameObject, false);
			camera2.gameObject.AddComponent<UICamera>();
			camera2.clearFlags = ((!flag) ? 2 : 3);
			camera2.backgroundColor = Color.grey;
			camera2.cullingMask = num2;
			camera2.depth = num + 1f;
			if (advanced3D)
			{
				camera2.nearClipPlane = 0.1f;
				camera2.farClipPlane = 4f;
				camera2.transform.localPosition = new Vector3(0f, 0f, -700f);
			}
			else
			{
				camera2.orthographic = true;
				camera2.orthographicSize = 1f;
				camera2.nearClipPlane = -10f;
				camera2.farClipPlane = 10f;
			}
			AudioListener[] array2 = NGUITools.FindActive<AudioListener>();
			if (array2 == null || array2.Length == 0)
			{
				camera2.gameObject.AddComponent<AudioListener>();
			}
			uipanel = uiroot.gameObject.AddComponent<UIPanel>();
		}
		if (trans != null)
		{
			while (trans.parent != null)
			{
				trans = trans.parent;
			}
			if (NGUITools.IsChild(trans, uipanel.transform))
			{
				uipanel = trans.gameObject.AddComponent<UIPanel>();
			}
			else
			{
				trans.parent = uipanel.transform;
				trans.localScale = Vector3.one;
				trans.localPosition = Vector3.zero;
				NGUITools.SetChildLayer(uipanel.cachedTransform, uipanel.cachedGameObject.layer);
			}
		}
		return uipanel;
	}

	// Token: 0x06001D30 RID: 7472 RVA: 0x000E47B8 File Offset: 0x000E29B8
	public static void SetChildLayer(Transform t, int layer)
	{
		for (int i = 0; i < t.childCount; i++)
		{
			Transform child = t.GetChild(i);
			child.gameObject.layer = layer;
			NGUITools.SetChildLayer(child, layer);
		}
	}

	// Token: 0x06001D31 RID: 7473 RVA: 0x000E47F8 File Offset: 0x000E29F8
	public static T AddChild<T>(GameObject parent) where T : Component
	{
		GameObject gameObject = NGUITools.AddChild(parent);
		gameObject.name = NGUITools.GetTypeName<T>();
		return gameObject.AddComponent<T>();
	}

	// Token: 0x06001D32 RID: 7474 RVA: 0x000E4820 File Offset: 0x000E2A20
	public static T AddChild<T>(GameObject parent, bool undo) where T : Component
	{
		GameObject gameObject = NGUITools.AddChild(parent, undo);
		gameObject.name = NGUITools.GetTypeName<T>();
		return gameObject.AddComponent<T>();
	}

	// Token: 0x06001D33 RID: 7475 RVA: 0x000E4848 File Offset: 0x000E2A48
	public static T AddWidget<T>(GameObject go) where T : UIWidget
	{
		int depth = NGUITools.CalculateNextDepth(go);
		T result = NGUITools.AddChild<T>(go);
		result.width = 100;
		result.height = 100;
		result.depth = depth;
		return result;
	}

	// Token: 0x06001D34 RID: 7476 RVA: 0x000E4890 File Offset: 0x000E2A90
	public static UISprite AddSprite(GameObject go, UIAtlas atlas, string spriteName)
	{
		UISpriteData uispriteData = (!(atlas != null)) ? null : atlas.GetSprite(spriteName);
		UISprite uisprite = NGUITools.AddWidget<UISprite>(go);
		uisprite.type = ((uispriteData != null && uispriteData.hasBorder) ? UIBasicSprite.Type.Sliced : UIBasicSprite.Type.Simple);
		uisprite.atlas = atlas;
		uisprite.spriteName = spriteName;
		return uisprite;
	}

	// Token: 0x06001D35 RID: 7477 RVA: 0x000E48EC File Offset: 0x000E2AEC
	public static GameObject GetRoot(GameObject go)
	{
		Transform transform = go.transform;
		for (;;)
		{
			Transform parent = transform.parent;
			if (parent == null)
			{
				break;
			}
			transform = parent;
		}
		return transform.gameObject;
	}

	// Token: 0x06001D36 RID: 7478 RVA: 0x000E492C File Offset: 0x000E2B2C
	public static T FindInParents<T>(GameObject go) where T : Component
	{
		if (go == null)
		{
			return (T)((object)null);
		}
		T component = go.GetComponent<T>();
		if (component == null)
		{
			Transform parent = go.transform.parent;
			while (parent != null && component == null)
			{
				component = parent.gameObject.GetComponent<T>();
				parent = parent.parent;
			}
		}
		return component;
	}

	// Token: 0x06001D37 RID: 7479 RVA: 0x000134E3 File Offset: 0x000116E3
	public static T FindInParents<T>(Transform trans) where T : Component
	{
		if (trans == null)
		{
			return (T)((object)null);
		}
		return trans.GetComponentInParent<T>();
	}

	// Token: 0x06001D38 RID: 7480 RVA: 0x000E49A8 File Offset: 0x000E2BA8
	public static void Destroy(Object obj)
	{
		if (obj != null)
		{
			if (Application.isPlaying)
			{
				if (obj is GameObject)
				{
					GameObject gameObject = obj as GameObject;
					gameObject.transform.parent = null;
				}
				Object.Destroy(obj);
			}
			else
			{
				Object.DestroyImmediate(obj);
			}
		}
	}

	// Token: 0x06001D39 RID: 7481 RVA: 0x000134FE File Offset: 0x000116FE
	public static void DestroyImmediate(Object obj)
	{
		if (obj != null)
		{
			if (Application.isEditor)
			{
				Object.DestroyImmediate(obj);
			}
			else
			{
				Object.Destroy(obj);
			}
		}
	}

	// Token: 0x06001D3A RID: 7482 RVA: 0x000E49FC File Offset: 0x000E2BFC
	public static void Broadcast(string funcName)
	{
		GameObject[] array = Object.FindObjectsOfType(typeof(GameObject)) as GameObject[];
		int i = 0;
		int num = array.Length;
		while (i < num)
		{
			array[i].SendMessage(funcName, 1);
			i++;
		}
	}

	// Token: 0x06001D3B RID: 7483 RVA: 0x000E4A40 File Offset: 0x000E2C40
	public static void Broadcast(string funcName, object param)
	{
		GameObject[] array = Object.FindObjectsOfType(typeof(GameObject)) as GameObject[];
		int i = 0;
		int num = array.Length;
		while (i < num)
		{
			array[i].SendMessage(funcName, param, 1);
			i++;
		}
	}

	// Token: 0x06001D3C RID: 7484 RVA: 0x000E4A84 File Offset: 0x000E2C84
	public static bool IsChild(Transform parent, Transform child)
	{
		if (parent == null || child == null)
		{
			return false;
		}
		while (child != null)
		{
			if (child == parent)
			{
				return true;
			}
			child = child.parent;
		}
		return false;
	}

	// Token: 0x06001D3D RID: 7485 RVA: 0x00013527 File Offset: 0x00011727
	private static void Activate(Transform t)
	{
		NGUITools.Activate(t, false);
	}

	// Token: 0x06001D3E RID: 7486 RVA: 0x000E4AD4 File Offset: 0x000E2CD4
	private static void Activate(Transform t, bool compatibilityMode)
	{
		NGUITools.SetActiveSelf(t.gameObject, true);
		if (compatibilityMode)
		{
			int i = 0;
			int childCount = t.childCount;
			while (i < childCount)
			{
				Transform child = t.GetChild(i);
				if (child.gameObject.activeSelf)
				{
					return;
				}
				i++;
			}
			int j = 0;
			int childCount2 = t.childCount;
			while (j < childCount2)
			{
				Transform child2 = t.GetChild(j);
				NGUITools.Activate(child2, true);
				j++;
			}
		}
	}

	// Token: 0x06001D3F RID: 7487 RVA: 0x00013530 File Offset: 0x00011730
	private static void Deactivate(Transform t)
	{
		NGUITools.SetActiveSelf(t.gameObject, false);
	}

	// Token: 0x06001D40 RID: 7488 RVA: 0x0001353E File Offset: 0x0001173E
	public static void SetActive(GameObject go, bool state)
	{
		NGUITools.SetActive(go, state, true);
	}

	// Token: 0x06001D41 RID: 7489 RVA: 0x00013548 File Offset: 0x00011748
	public static void SetActive(GameObject go, bool state, bool compatibilityMode)
	{
		if (go)
		{
			if (state)
			{
				NGUITools.Activate(go.transform, compatibilityMode);
				NGUITools.CallCreatePanel(go.transform);
			}
			else
			{
				NGUITools.Deactivate(go.transform);
			}
		}
	}

	// Token: 0x06001D42 RID: 7490 RVA: 0x000E4B54 File Offset: 0x000E2D54
	[DebuggerHidden]
	[DebuggerStepThrough]
	private static void CallCreatePanel(Transform t)
	{
		UIWidget component = t.GetComponent<UIWidget>();
		if (component != null)
		{
			component.CreatePanel();
		}
		int i = 0;
		int childCount = t.childCount;
		while (i < childCount)
		{
			NGUITools.CallCreatePanel(t.GetChild(i));
			i++;
		}
	}

	// Token: 0x06001D43 RID: 7491 RVA: 0x000E4BA0 File Offset: 0x000E2DA0
	public static void SetActiveChildren(GameObject go, bool state)
	{
		Transform transform = go.transform;
		if (state)
		{
			int i = 0;
			int childCount = transform.childCount;
			while (i < childCount)
			{
				Transform child = transform.GetChild(i);
				NGUITools.Activate(child);
				i++;
			}
		}
		else
		{
			int j = 0;
			int childCount2 = transform.childCount;
			while (j < childCount2)
			{
				Transform child2 = transform.GetChild(j);
				NGUITools.Deactivate(child2);
				j++;
			}
		}
	}

	// Token: 0x06001D44 RID: 7492 RVA: 0x00013582 File Offset: 0x00011782
	[Obsolete("Use NGUITools.GetActive instead")]
	public static bool IsActive(Behaviour mb)
	{
		return mb != null && mb.enabled && mb.gameObject.activeInHierarchy;
	}

	// Token: 0x06001D45 RID: 7493 RVA: 0x000135A9 File Offset: 0x000117A9
	[DebuggerStepThrough]
	[DebuggerHidden]
	public static bool GetActive(Behaviour mb)
	{
		return mb && mb.enabled && mb.gameObject.activeInHierarchy;
	}

	// Token: 0x06001D46 RID: 7494 RVA: 0x000135CF File Offset: 0x000117CF
	[DebuggerStepThrough]
	[DebuggerHidden]
	public static bool GetActive(GameObject go)
	{
		return go && go.activeInHierarchy;
	}

	// Token: 0x06001D47 RID: 7495 RVA: 0x0000FF26 File Offset: 0x0000E126
	[DebuggerStepThrough]
	[DebuggerHidden]
	public static void SetActiveSelf(GameObject go, bool state)
	{
		go.SetActive(state);
	}

	// Token: 0x06001D48 RID: 7496 RVA: 0x000E4C18 File Offset: 0x000E2E18
	public static void SetLayer(GameObject go, int layer)
	{
		go.layer = layer;
		Transform transform = go.transform;
		int i = 0;
		int childCount = transform.childCount;
		while (i < childCount)
		{
			Transform child = transform.GetChild(i);
			NGUITools.SetLayer(child.gameObject, layer);
			i++;
		}
	}

	// Token: 0x06001D49 RID: 7497 RVA: 0x000135E5 File Offset: 0x000117E5
	public static Vector3 Round(Vector3 v)
	{
		v.x = Mathf.Round(v.x);
		v.y = Mathf.Round(v.y);
		v.z = Mathf.Round(v.z);
		return v;
	}

	// Token: 0x06001D4A RID: 7498 RVA: 0x000E4C60 File Offset: 0x000E2E60
	public static void MakePixelPerfect(Transform t)
	{
		UIWidget component = t.GetComponent<UIWidget>();
		if (component != null)
		{
			component.MakePixelPerfect();
		}
		if (t.GetComponent<UIAnchor>() == null && t.GetComponent<UIRoot>() == null)
		{
			t.localPosition = NGUITools.Round(t.localPosition);
			t.localScale = NGUITools.Round(t.localScale);
		}
		int i = 0;
		int childCount = t.childCount;
		while (i < childCount)
		{
			NGUITools.MakePixelPerfect(t.GetChild(i));
			i++;
		}
	}

	// Token: 0x06001D4B RID: 7499 RVA: 0x000E4CF0 File Offset: 0x000E2EF0
	public static bool Save(string fileName, byte[] bytes)
	{
		if (!NGUITools.fileAccess)
		{
			return false;
		}
		string text = Application.persistentDataPath + "/" + fileName;
		if (bytes == null)
		{
			if (File.Exists(text))
			{
				File.Delete(text);
			}
			return true;
		}
		FileStream fileStream = null;
		try
		{
			fileStream = File.Create(text);
		}
		catch (Exception ex)
		{
			Debug.LogError(ex.Message);
			return false;
		}
		fileStream.Write(bytes, 0, bytes.Length);
		fileStream.Close();
		return true;
	}

	// Token: 0x06001D4C RID: 7500 RVA: 0x000E4D80 File Offset: 0x000E2F80
	public static byte[] Load(string fileName)
	{
		if (!NGUITools.fileAccess)
		{
			return null;
		}
		string text = Application.persistentDataPath + "/" + fileName;
		if (File.Exists(text))
		{
			return File.ReadAllBytes(text);
		}
		return null;
	}

	// Token: 0x06001D4D RID: 7501 RVA: 0x000E4DC0 File Offset: 0x000E2FC0
	public static Color ApplyPMA(Color c)
	{
		if (c.a != 1f)
		{
			c.r *= c.a;
			c.g *= c.a;
			c.b *= c.a;
		}
		return c;
	}

	// Token: 0x06001D4E RID: 7502 RVA: 0x000E4E20 File Offset: 0x000E3020
	public static void MarkParentAsChanged(GameObject go)
	{
		UIRect[] componentsInChildren = go.GetComponentsInChildren<UIRect>();
		int i = 0;
		int num = componentsInChildren.Length;
		while (i < num)
		{
			componentsInChildren[i].ParentHasChanged();
			i++;
		}
	}

	// Token: 0x17000253 RID: 595
	// (get) Token: 0x06001D4F RID: 7503 RVA: 0x000E4E54 File Offset: 0x000E3054
	// (set) Token: 0x06001D50 RID: 7504 RVA: 0x000E4E7C File Offset: 0x000E307C
	public static string clipboard
	{
		get
		{
			TextEditor textEditor = new TextEditor();
			textEditor.Paste();
			return textEditor.content.text;
		}
		set
		{
			TextEditor textEditor = new TextEditor();
			textEditor.content = new GUIContent(value);
			textEditor.OnFocus();
			textEditor.Copy();
		}
	}

	// Token: 0x06001D51 RID: 7505 RVA: 0x00013349 File Offset: 0x00011549
	[Obsolete("Use NGUIText.EncodeColor instead")]
	public static string EncodeColor(Color c)
	{
		return NGUIText.EncodeColor24(c);
	}

	// Token: 0x06001D52 RID: 7506 RVA: 0x00013340 File Offset: 0x00011540
	[Obsolete("Use NGUIText.ParseColor instead")]
	public static Color ParseColor(string text, int offset)
	{
		return NGUIText.ParseColor24(text, offset);
	}

	// Token: 0x06001D53 RID: 7507 RVA: 0x00013621 File Offset: 0x00011821
	[Obsolete("Use NGUIText.StripSymbols instead")]
	public static string StripSymbols(string text)
	{
		return NGUIText.StripSymbols(text);
	}

	// Token: 0x06001D54 RID: 7508 RVA: 0x000E4EA8 File Offset: 0x000E30A8
	public static T AddMissingComponent<T>(this GameObject go) where T : Component
	{
		T t = go.GetComponent<T>();
		if (t == null)
		{
			t = go.AddComponent<T>();
		}
		return t;
	}

	// Token: 0x06001D55 RID: 7509 RVA: 0x00013629 File Offset: 0x00011829
	public static Vector3[] GetSides(this Camera cam)
	{
		return cam.GetSides(Mathf.Lerp(cam.nearClipPlane, cam.farClipPlane, 0.5f), null);
	}

	// Token: 0x06001D56 RID: 7510 RVA: 0x00013648 File Offset: 0x00011848
	public static Vector3[] GetSides(this Camera cam, float depth)
	{
		return cam.GetSides(depth, null);
	}

	// Token: 0x06001D57 RID: 7511 RVA: 0x00013652 File Offset: 0x00011852
	public static Vector3[] GetSides(this Camera cam, Transform relativeTo)
	{
		return cam.GetSides(Mathf.Lerp(cam.nearClipPlane, cam.farClipPlane, 0.5f), relativeTo);
	}

	// Token: 0x06001D58 RID: 7512 RVA: 0x000E4ED8 File Offset: 0x000E30D8
	public static Vector3[] GetSides(this Camera cam, float depth, Transform relativeTo)
	{
		if (cam.isOrthoGraphic)
		{
			float orthographicSize = cam.orthographicSize;
			float num = -orthographicSize;
			float num2 = orthographicSize;
			float num3 = -orthographicSize;
			float num4 = orthographicSize;
			Rect rect = cam.rect;
			Vector2 screenSize = NGUITools.screenSize;
			float num5 = screenSize.x / screenSize.y;
			num5 *= rect.width / rect.height;
			num *= num5;
			num2 *= num5;
			Transform transform = cam.transform;
			Quaternion rotation = transform.rotation;
			Vector3 position = transform.position;
			NGUITools.mSides[0] = rotation * new Vector3(num, 0f, depth) + position;
			NGUITools.mSides[1] = rotation * new Vector3(0f, num4, depth) + position;
			NGUITools.mSides[2] = rotation * new Vector3(num2, 0f, depth) + position;
			NGUITools.mSides[3] = rotation * new Vector3(0f, num3, depth) + position;
		}
		else
		{
			NGUITools.mSides[0] = cam.ViewportToWorldPoint(new Vector3(0f, 0.5f, depth));
			NGUITools.mSides[1] = cam.ViewportToWorldPoint(new Vector3(0.5f, 1f, depth));
			NGUITools.mSides[2] = cam.ViewportToWorldPoint(new Vector3(1f, 0.5f, depth));
			NGUITools.mSides[3] = cam.ViewportToWorldPoint(new Vector3(0.5f, 0f, depth));
		}
		if (relativeTo != null)
		{
			for (int i = 0; i < 4; i++)
			{
				NGUITools.mSides[i] = relativeTo.InverseTransformPoint(NGUITools.mSides[i]);
			}
		}
		return NGUITools.mSides;
	}

	// Token: 0x06001D59 RID: 7513 RVA: 0x000E50F0 File Offset: 0x000E32F0
	public static Vector3[] GetWorldCorners(this Camera cam)
	{
		float depth = Mathf.Lerp(cam.nearClipPlane, cam.farClipPlane, 0.5f);
		return cam.GetWorldCorners(depth, null);
	}

	// Token: 0x06001D5A RID: 7514 RVA: 0x00013671 File Offset: 0x00011871
	public static Vector3[] GetWorldCorners(this Camera cam, float depth)
	{
		return cam.GetWorldCorners(depth, null);
	}

	// Token: 0x06001D5B RID: 7515 RVA: 0x0001367B File Offset: 0x0001187B
	public static Vector3[] GetWorldCorners(this Camera cam, Transform relativeTo)
	{
		return cam.GetWorldCorners(Mathf.Lerp(cam.nearClipPlane, cam.farClipPlane, 0.5f), relativeTo);
	}

	// Token: 0x06001D5C RID: 7516 RVA: 0x000E511C File Offset: 0x000E331C
	public static Vector3[] GetWorldCorners(this Camera cam, float depth, Transform relativeTo)
	{
		if (cam.isOrthoGraphic)
		{
			float orthographicSize = cam.orthographicSize;
			float num = -orthographicSize;
			float num2 = orthographicSize;
			float num3 = -orthographicSize;
			float num4 = orthographicSize;
			Rect rect = cam.rect;
			Vector2 screenSize = NGUITools.screenSize;
			float num5 = screenSize.x / screenSize.y;
			num5 *= rect.width / rect.height;
			num *= num5;
			num2 *= num5;
			Transform transform = cam.transform;
			Quaternion rotation = transform.rotation;
			Vector3 position = transform.position;
			NGUITools.mSides[0] = rotation * new Vector3(num, num3, depth) + position;
			NGUITools.mSides[1] = rotation * new Vector3(num, num4, depth) + position;
			NGUITools.mSides[2] = rotation * new Vector3(num2, num4, depth) + position;
			NGUITools.mSides[3] = rotation * new Vector3(num2, num3, depth) + position;
		}
		else
		{
			NGUITools.mSides[0] = cam.ViewportToWorldPoint(new Vector3(0f, 0f, depth));
			NGUITools.mSides[1] = cam.ViewportToWorldPoint(new Vector3(0f, 1f, depth));
			NGUITools.mSides[2] = cam.ViewportToWorldPoint(new Vector3(1f, 1f, depth));
			NGUITools.mSides[3] = cam.ViewportToWorldPoint(new Vector3(1f, 0f, depth));
		}
		if (relativeTo != null)
		{
			for (int i = 0; i < 4; i++)
			{
				NGUITools.mSides[i] = relativeTo.InverseTransformPoint(NGUITools.mSides[i]);
			}
		}
		return NGUITools.mSides;
	}

	// Token: 0x06001D5D RID: 7517 RVA: 0x000E5324 File Offset: 0x000E3524
	public static string GetFuncName(object obj, string method)
	{
		if (obj == null)
		{
			return "<null>";
		}
		string text = obj.GetType().ToString();
		int num = text.LastIndexOf('/');
		if (num > 0)
		{
			text = text.Substring(num + 1);
		}
		return (!string.IsNullOrEmpty(method)) ? (text + "/" + method) : text;
	}

	// Token: 0x06001D5E RID: 7518 RVA: 0x000E5380 File Offset: 0x000E3580
	public static void Execute<T>(GameObject go, string funcName) where T : Component
	{
		T[] components = go.GetComponents<T>();
		foreach (T t in components)
		{
			MethodInfo method = t.GetType().GetMethod(funcName, 52);
			if (method != null)
			{
				method.Invoke(t, null);
			}
		}
	}

	// Token: 0x06001D5F RID: 7519 RVA: 0x000E53E0 File Offset: 0x000E35E0
	public static void ExecuteAll<T>(GameObject root, string funcName) where T : Component
	{
		NGUITools.Execute<T>(root, funcName);
		Transform transform = root.transform;
		int i = 0;
		int childCount = transform.childCount;
		while (i < childCount)
		{
			NGUITools.ExecuteAll<T>(transform.GetChild(i).gameObject, funcName);
			i++;
		}
	}

	// Token: 0x06001D60 RID: 7520 RVA: 0x0001369A File Offset: 0x0001189A
	public static void ImmediatelyCreateDrawCalls(GameObject root)
	{
		NGUITools.ExecuteAll<UIWidget>(root, "Start");
		NGUITools.ExecuteAll<UIPanel>(root, "Start");
		NGUITools.ExecuteAll<UIWidget>(root, "Update");
		NGUITools.ExecuteAll<UIPanel>(root, "Update");
		NGUITools.ExecuteAll<UIPanel>(root, "LateUpdate");
	}

	// Token: 0x17000254 RID: 596
	// (get) Token: 0x06001D61 RID: 7521 RVA: 0x000136D3 File Offset: 0x000118D3
	public static Vector2 screenSize
	{
		get
		{
			return new Vector2((float)Screen.width, (float)Screen.height);
		}
	}

	// Token: 0x04002192 RID: 8594
	private static AudioListener mListener;

	// Token: 0x04002193 RID: 8595
	private static bool mLoaded = false;

	// Token: 0x04002194 RID: 8596
	private static float mGlobalVolume = 1f;

	// Token: 0x04002195 RID: 8597
	private static Vector3[] mSides = new Vector3[4];
}
