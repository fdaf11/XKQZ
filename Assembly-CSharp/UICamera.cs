using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004E0 RID: 1248
[AddComponentMenu("NGUI/UI/NGUI Event System (UICamera)")]
[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class UICamera : MonoBehaviour
{
	// Token: 0x170002CF RID: 719
	// (get) Token: 0x06001F6A RID: 8042 RVA: 0x00002B59 File Offset: 0x00000D59
	[Obsolete("Use new OnDragStart / OnDragOver / OnDragOut / OnDragEnd events instead")]
	public bool stickyPress
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170002D0 RID: 720
	// (get) Token: 0x06001F6B RID: 8043 RVA: 0x000EF748 File Offset: 0x000ED948
	public static Ray currentRay
	{
		get
		{
			return (!(UICamera.currentCamera != null) || UICamera.currentTouch == null) ? default(Ray) : UICamera.currentCamera.ScreenPointToRay(UICamera.currentTouch.pos);
		}
	}

	// Token: 0x170002D1 RID: 721
	// (get) Token: 0x06001F6C RID: 8044 RVA: 0x00015014 File Offset: 0x00013214
	// (set) Token: 0x06001F6D RID: 8045 RVA: 0x0001501B File Offset: 0x0001321B
	[Obsolete("Use delegates instead such as UICamera.onClick, UICamera.onHover, etc.")]
	public static GameObject genericEventHandler
	{
		get
		{
			return UICamera.mGenericHandler;
		}
		set
		{
			UICamera.mGenericHandler = value;
		}
	}

	// Token: 0x170002D2 RID: 722
	// (get) Token: 0x06001F6E RID: 8046 RVA: 0x00015023 File Offset: 0x00013223
	private bool handlesEvents
	{
		get
		{
			return UICamera.eventHandler == this;
		}
	}

	// Token: 0x170002D3 RID: 723
	// (get) Token: 0x06001F6F RID: 8047 RVA: 0x00015030 File Offset: 0x00013230
	public Camera cachedCamera
	{
		get
		{
			if (this.mCam == null)
			{
				this.mCam = base.camera;
			}
			return this.mCam;
		}
	}

	// Token: 0x170002D4 RID: 724
	// (get) Token: 0x06001F70 RID: 8048 RVA: 0x000EF798 File Offset: 0x000ED998
	public static bool isOverUI
	{
		get
		{
			if (UICamera.currentTouch != null)
			{
				return UICamera.currentTouch.isOverUI;
			}
			return !(UICamera.hoveredObject == null) && !(UICamera.hoveredObject == UICamera.fallThrough) && NGUITools.FindInParents<UIRoot>(UICamera.hoveredObject) != null;
		}
	}

	// Token: 0x170002D5 RID: 725
	// (get) Token: 0x06001F71 RID: 8049 RVA: 0x00015055 File Offset: 0x00013255
	// (set) Token: 0x06001F72 RID: 8050 RVA: 0x0001505C File Offset: 0x0001325C
	public static GameObject selectedObject
	{
		get
		{
			return UICamera.mCurrentSelection;
		}
		set
		{
			UICamera.SetSelection(value, UICamera.currentScheme);
		}
	}

	// Token: 0x06001F73 RID: 8051 RVA: 0x000EF7F4 File Offset: 0x000ED9F4
	public static bool IsPressed(GameObject go)
	{
		for (int i = 0; i < 3; i++)
		{
			if (UICamera.mMouse[i].pressed == go)
			{
				return true;
			}
		}
		foreach (KeyValuePair<int, UICamera.MouseOrTouch> keyValuePair in UICamera.mTouches)
		{
			if (keyValuePair.Value.pressed == go)
			{
				return true;
			}
		}
		return UICamera.controller.pressed == go;
	}

	// Token: 0x06001F74 RID: 8052 RVA: 0x000EF8AC File Offset: 0x000EDAAC
	protected static void SetSelection(GameObject go, UICamera.ControlScheme scheme)
	{
		if (UICamera.mNextSelection != null)
		{
			UICamera.mNextSelection = go;
		}
		else if (UICamera.mCurrentSelection != go)
		{
			UICamera.mNextSelection = go;
			UICamera.mNextScheme = scheme;
			if (UICamera.list.size > 0)
			{
				UICamera uicamera = (!(UICamera.mNextSelection != null)) ? UICamera.list[0] : UICamera.FindCameraForLayer(UICamera.mNextSelection.layer);
				if (uicamera != null)
				{
					uicamera.StartCoroutine(uicamera.ChangeSelection());
				}
			}
		}
	}

	// Token: 0x06001F75 RID: 8053 RVA: 0x000EF94C File Offset: 0x000EDB4C
	private IEnumerator ChangeSelection()
	{
		yield return new WaitForEndOfFrame();
		if (UICamera.onSelect != null)
		{
			UICamera.onSelect(UICamera.mCurrentSelection, false);
		}
		UICamera.Notify(UICamera.mCurrentSelection, "OnSelect", false);
		UICamera.mCurrentSelection = UICamera.mNextSelection;
		UICamera.mNextSelection = null;
		if (UICamera.mCurrentSelection != null)
		{
			UICamera.current = this;
			UICamera.currentCamera = this.mCam;
			UICamera.currentScheme = UICamera.mNextScheme;
			UICamera.inputHasFocus = (UICamera.mCurrentSelection.GetComponent<UIInput>() != null);
			if (UICamera.onSelect != null)
			{
				UICamera.onSelect(UICamera.mCurrentSelection, true);
			}
			UICamera.Notify(UICamera.mCurrentSelection, "OnSelect", true);
			UICamera.current = null;
		}
		else
		{
			UICamera.inputHasFocus = false;
		}
		yield break;
	}

	// Token: 0x170002D6 RID: 726
	// (get) Token: 0x06001F76 RID: 8054 RVA: 0x000EF968 File Offset: 0x000EDB68
	public static int touchCount
	{
		get
		{
			int num = 0;
			foreach (KeyValuePair<int, UICamera.MouseOrTouch> keyValuePair in UICamera.mTouches)
			{
				if (keyValuePair.Value.pressed != null)
				{
					num++;
				}
			}
			for (int i = 0; i < UICamera.mMouse.Length; i++)
			{
				if (UICamera.mMouse[i].pressed != null)
				{
					num++;
				}
			}
			if (UICamera.controller.pressed != null)
			{
				num++;
			}
			return num;
		}
	}

	// Token: 0x170002D7 RID: 727
	// (get) Token: 0x06001F77 RID: 8055 RVA: 0x000EFA24 File Offset: 0x000EDC24
	public static int dragCount
	{
		get
		{
			int num = 0;
			foreach (KeyValuePair<int, UICamera.MouseOrTouch> keyValuePair in UICamera.mTouches)
			{
				if (keyValuePair.Value.dragged != null)
				{
					num++;
				}
			}
			for (int i = 0; i < UICamera.mMouse.Length; i++)
			{
				if (UICamera.mMouse[i].dragged != null)
				{
					num++;
				}
			}
			if (UICamera.controller.dragged != null)
			{
				num++;
			}
			return num;
		}
	}

	// Token: 0x170002D8 RID: 728
	// (get) Token: 0x06001F78 RID: 8056 RVA: 0x000EFAE0 File Offset: 0x000EDCE0
	public static Camera mainCamera
	{
		get
		{
			UICamera eventHandler = UICamera.eventHandler;
			return (!(eventHandler != null)) ? null : eventHandler.cachedCamera;
		}
	}

	// Token: 0x170002D9 RID: 729
	// (get) Token: 0x06001F79 RID: 8057 RVA: 0x000EFB0C File Offset: 0x000EDD0C
	public static UICamera eventHandler
	{
		get
		{
			for (int i = 0; i < UICamera.list.size; i++)
			{
				UICamera uicamera = UICamera.list.buffer[i];
				if (!(uicamera == null) && uicamera.enabled && NGUITools.GetActive(uicamera.gameObject))
				{
					return uicamera;
				}
			}
			return null;
		}
	}

	// Token: 0x06001F7A RID: 8058 RVA: 0x00015069 File Offset: 0x00013269
	private static int CompareFunc(UICamera a, UICamera b)
	{
		if (a.cachedCamera.depth < b.cachedCamera.depth)
		{
			return 1;
		}
		if (a.cachedCamera.depth > b.cachedCamera.depth)
		{
			return -1;
		}
		return 0;
	}

	// Token: 0x06001F7B RID: 8059 RVA: 0x000EFB70 File Offset: 0x000EDD70
	private static Rigidbody FindRootRigidbody(Transform trans)
	{
		while (trans != null)
		{
			if (trans.GetComponent<UIPanel>() != null)
			{
				return null;
			}
			Rigidbody rigidbody = trans.rigidbody;
			if (rigidbody != null)
			{
				return rigidbody;
			}
			trans = trans.parent;
		}
		return null;
	}

	// Token: 0x06001F7C RID: 8060 RVA: 0x000EFBC0 File Offset: 0x000EDDC0
	private static Rigidbody2D FindRootRigidbody2D(Transform trans)
	{
		while (trans != null)
		{
			if (trans.GetComponent<UIPanel>() != null)
			{
				return null;
			}
			Rigidbody2D rigidbody2D = trans.rigidbody2D;
			if (rigidbody2D != null)
			{
				return rigidbody2D;
			}
			trans = trans.parent;
		}
		return null;
	}

	// Token: 0x06001F7D RID: 8061 RVA: 0x000EFC10 File Offset: 0x000EDE10
	public static bool Raycast(Vector3 inPos)
	{
		for (int i = 0; i < UICamera.list.size; i++)
		{
			UICamera uicamera = UICamera.list.buffer[i];
			if (uicamera.enabled && NGUITools.GetActive(uicamera.gameObject))
			{
				UICamera.currentCamera = uicamera.cachedCamera;
				Vector3 vector = UICamera.currentCamera.ScreenToViewportPoint(inPos);
				if (!float.IsNaN(vector.x) && !float.IsNaN(vector.y))
				{
					if (vector.x >= 0f && vector.x <= 1f && vector.y >= 0f && vector.y <= 1f)
					{
						Ray ray = UICamera.currentCamera.ScreenPointToRay(inPos);
						int num = UICamera.currentCamera.cullingMask & uicamera.eventReceiverMask;
						float num2 = (uicamera.rangeDistance <= 0f) ? (UICamera.currentCamera.farClipPlane - UICamera.currentCamera.nearClipPlane) : uicamera.rangeDistance;
						if (uicamera.eventType == UICamera.EventType.World_3D)
						{
							if (Physics.Raycast(ray, ref UICamera.lastHit, num2, num))
							{
								UICamera.lastWorldPosition = UICamera.lastHit.point;
								UICamera.hoveredObject = UICamera.lastHit.collider.gameObject;
								if (!uicamera.eventsGoToColliders)
								{
									Rigidbody rigidbody = UICamera.FindRootRigidbody(UICamera.hoveredObject.transform);
									if (rigidbody != null)
									{
										UICamera.hoveredObject = rigidbody.gameObject;
									}
								}
								return true;
							}
						}
						else if (uicamera.eventType == UICamera.EventType.UI_3D)
						{
							RaycastHit[] array = Physics.RaycastAll(ray, num2, num);
							if (array.Length > 1)
							{
								int j = 0;
								while (j < array.Length)
								{
									GameObject gameObject = array[j].collider.gameObject;
									UIWidget component = gameObject.GetComponent<UIWidget>();
									if (component != null)
									{
										if (component.isVisible)
										{
											if (component.hitCheck == null || component.hitCheck(array[j].point))
											{
												goto IL_256;
											}
										}
									}
									else
									{
										UIRect uirect = NGUITools.FindInParents<UIRect>(gameObject);
										if (!(uirect != null) || uirect.finalAlpha >= 0.001f)
										{
											goto IL_256;
										}
									}
									IL_2D7:
									j++;
									continue;
									IL_256:
									UICamera.mHit.depth = NGUITools.CalculateRaycastDepth(gameObject);
									if (UICamera.mHit.depth != 2147483647)
									{
										UICamera.mHit.hit = array[j];
										UICamera.mHit.point = array[j].point;
										UICamera.mHit.go = array[j].collider.gameObject;
										UICamera.mHits.Add(UICamera.mHit);
										goto IL_2D7;
									}
									goto IL_2D7;
								}
								UICamera.mHits.Sort((UICamera.DepthEntry r1, UICamera.DepthEntry r2) => r2.depth.CompareTo(r1.depth));
								for (int k = 0; k < UICamera.mHits.size; k++)
								{
									if (UICamera.IsVisible(ref UICamera.mHits.buffer[k]))
									{
										UICamera.lastHit = UICamera.mHits[k].hit;
										UICamera.hoveredObject = UICamera.mHits[k].go;
										UICamera.lastWorldPosition = UICamera.mHits[k].point;
										UICamera.mHits.Clear();
										return true;
									}
								}
								UICamera.mHits.Clear();
							}
							else if (array.Length == 1)
							{
								GameObject gameObject2 = array[0].collider.gameObject;
								UIWidget component2 = gameObject2.GetComponent<UIWidget>();
								if (component2 != null)
								{
									if (!component2.isVisible)
									{
										goto IL_7D8;
									}
									if (component2.hitCheck != null && !component2.hitCheck(array[0].point))
									{
										goto IL_7D8;
									}
								}
								else
								{
									UIRect uirect2 = NGUITools.FindInParents<UIRect>(gameObject2);
									if (uirect2 != null && uirect2.finalAlpha < 0.001f)
									{
										goto IL_7D8;
									}
								}
								if (UICamera.IsVisible(array[0].point, array[0].collider.gameObject))
								{
									UICamera.lastHit = array[0];
									UICamera.lastWorldPosition = array[0].point;
									UICamera.hoveredObject = UICamera.lastHit.collider.gameObject;
									return true;
								}
							}
						}
						else if (uicamera.eventType == UICamera.EventType.World_2D)
						{
							if (UICamera.m2DPlane.Raycast(ray, ref num2))
							{
								Vector3 point = ray.GetPoint(num2);
								Collider2D collider2D = Physics2D.OverlapPoint(point, num);
								if (collider2D)
								{
									UICamera.lastWorldPosition = point;
									UICamera.hoveredObject = collider2D.gameObject;
									if (!uicamera.eventsGoToColliders)
									{
										Rigidbody2D rigidbody2D = UICamera.FindRootRigidbody2D(UICamera.hoveredObject.transform);
										if (rigidbody2D != null)
										{
											UICamera.hoveredObject = rigidbody2D.gameObject;
										}
									}
									return true;
								}
							}
						}
						else if (uicamera.eventType == UICamera.EventType.UI_2D)
						{
							if (UICamera.m2DPlane.Raycast(ray, ref num2))
							{
								UICamera.lastWorldPosition = ray.GetPoint(num2);
								Collider2D[] array2 = Physics2D.OverlapPointAll(UICamera.lastWorldPosition, num);
								if (array2.Length > 1)
								{
									int l = 0;
									while (l < array2.Length)
									{
										GameObject gameObject3 = array2[l].gameObject;
										UIWidget component3 = gameObject3.GetComponent<UIWidget>();
										if (component3 != null)
										{
											if (component3.isVisible)
											{
												if (component3.hitCheck == null || component3.hitCheck(UICamera.lastWorldPosition))
												{
													goto IL_62F;
												}
											}
										}
										else
										{
											UIRect uirect3 = NGUITools.FindInParents<UIRect>(gameObject3);
											if (!(uirect3 != null) || uirect3.finalAlpha >= 0.001f)
											{
												goto IL_62F;
											}
										}
										IL_67E:
										l++;
										continue;
										IL_62F:
										UICamera.mHit.depth = NGUITools.CalculateRaycastDepth(gameObject3);
										if (UICamera.mHit.depth != 2147483647)
										{
											UICamera.mHit.go = gameObject3;
											UICamera.mHit.point = UICamera.lastWorldPosition;
											UICamera.mHits.Add(UICamera.mHit);
											goto IL_67E;
										}
										goto IL_67E;
									}
									UICamera.mHits.Sort((UICamera.DepthEntry r1, UICamera.DepthEntry r2) => r2.depth.CompareTo(r1.depth));
									for (int m = 0; m < UICamera.mHits.size; m++)
									{
										if (UICamera.IsVisible(ref UICamera.mHits.buffer[m]))
										{
											UICamera.hoveredObject = UICamera.mHits[m].go;
											UICamera.mHits.Clear();
											return true;
										}
									}
									UICamera.mHits.Clear();
								}
								else if (array2.Length == 1)
								{
									GameObject gameObject4 = array2[0].gameObject;
									UIWidget component4 = gameObject4.GetComponent<UIWidget>();
									if (component4 != null)
									{
										if (!component4.isVisible)
										{
											goto IL_7D8;
										}
										if (component4.hitCheck != null && !component4.hitCheck(UICamera.lastWorldPosition))
										{
											goto IL_7D8;
										}
									}
									else
									{
										UIRect uirect4 = NGUITools.FindInParents<UIRect>(gameObject4);
										if (uirect4 != null && uirect4.finalAlpha < 0.001f)
										{
											goto IL_7D8;
										}
									}
									if (UICamera.IsVisible(UICamera.lastWorldPosition, gameObject4))
									{
										UICamera.hoveredObject = gameObject4;
										return true;
									}
								}
							}
						}
					}
				}
			}
			IL_7D8:;
		}
		return false;
	}

	// Token: 0x06001F7E RID: 8062 RVA: 0x000F040C File Offset: 0x000EE60C
	private static bool IsVisible(Vector3 worldPoint, GameObject go)
	{
		UIPanel uipanel = NGUITools.FindInParents<UIPanel>(go);
		while (uipanel != null)
		{
			if (!uipanel.IsVisible(worldPoint))
			{
				return false;
			}
			uipanel = uipanel.parentPanel;
		}
		return true;
	}

	// Token: 0x06001F7F RID: 8063 RVA: 0x000F0448 File Offset: 0x000EE648
	private static bool IsVisible(ref UICamera.DepthEntry de)
	{
		UIPanel uipanel = NGUITools.FindInParents<UIPanel>(de.go);
		while (uipanel != null)
		{
			if (!uipanel.IsVisible(de.point))
			{
				return false;
			}
			uipanel = uipanel.parentPanel;
		}
		return true;
	}

	// Token: 0x06001F80 RID: 8064 RVA: 0x000150A6 File Offset: 0x000132A6
	public static bool IsHighlighted(GameObject go)
	{
		if (UICamera.currentScheme == UICamera.ControlScheme.Mouse)
		{
			return UICamera.hoveredObject == go;
		}
		return UICamera.currentScheme == UICamera.ControlScheme.Controller && UICamera.selectedObject == go;
	}

	// Token: 0x06001F81 RID: 8065 RVA: 0x000F0490 File Offset: 0x000EE690
	public static UICamera FindCameraForLayer(int layer)
	{
		int num = 1 << layer;
		for (int i = 0; i < UICamera.list.size; i++)
		{
			UICamera uicamera = UICamera.list.buffer[i];
			Camera cachedCamera = uicamera.cachedCamera;
			if (cachedCamera != null && (cachedCamera.cullingMask & num) != 0)
			{
				return uicamera;
			}
		}
		return null;
	}

	// Token: 0x06001F82 RID: 8066 RVA: 0x000150D6 File Offset: 0x000132D6
	private static int GetDirection(KeyCode up, KeyCode down)
	{
		if (UICamera.GetKeyDown(up))
		{
			return 1;
		}
		if (UICamera.GetKeyDown(down))
		{
			return -1;
		}
		return 0;
	}

	// Token: 0x06001F83 RID: 8067 RVA: 0x000F04F0 File Offset: 0x000EE6F0
	private static int GetDirection(KeyCode up0, KeyCode up1, KeyCode down0, KeyCode down1)
	{
		if (UICamera.GetKeyDown(up0) || UICamera.GetKeyDown(up1))
		{
			return 1;
		}
		if (UICamera.GetKeyDown(down0) || UICamera.GetKeyDown(down1))
		{
			return -1;
		}
		return 0;
	}

	// Token: 0x06001F84 RID: 8068 RVA: 0x000F0544 File Offset: 0x000EE744
	private static int GetDirection(string axis)
	{
		float time = RealTime.time;
		if (UICamera.mNextEvent < time && !string.IsNullOrEmpty(axis))
		{
			float num = UICamera.GetAxis(axis);
			if (num > 0.75f)
			{
				UICamera.mNextEvent = time + 0.25f;
				return 1;
			}
			if (num < -0.75f)
			{
				UICamera.mNextEvent = time + 0.25f;
				return -1;
			}
		}
		return 0;
	}

	// Token: 0x06001F85 RID: 8069 RVA: 0x000F05AC File Offset: 0x000EE7AC
	public static void Notify(GameObject go, string funcName, object obj)
	{
		if (UICamera.mNotifying)
		{
			return;
		}
		UICamera.mNotifying = true;
		if (NGUITools.GetActive(go))
		{
			go.SendMessage(funcName, obj, 1);
			if (UICamera.mGenericHandler != null && UICamera.mGenericHandler != go)
			{
				UICamera.mGenericHandler.SendMessage(funcName, obj, 1);
			}
		}
		UICamera.mNotifying = false;
	}

	// Token: 0x06001F86 RID: 8070 RVA: 0x000150FD File Offset: 0x000132FD
	public static UICamera.MouseOrTouch GetMouse(int button)
	{
		return UICamera.mMouse[button];
	}

	// Token: 0x06001F87 RID: 8071 RVA: 0x000F0614 File Offset: 0x000EE814
	public static UICamera.MouseOrTouch GetTouch(int id)
	{
		UICamera.MouseOrTouch mouseOrTouch = null;
		if (id < 0)
		{
			return UICamera.GetMouse(-id - 1);
		}
		if (!UICamera.mTouches.TryGetValue(id, ref mouseOrTouch))
		{
			mouseOrTouch = new UICamera.MouseOrTouch();
			mouseOrTouch.pressTime = RealTime.time;
			mouseOrTouch.touchBegan = true;
			UICamera.mTouches.Add(id, mouseOrTouch);
		}
		return mouseOrTouch;
	}

	// Token: 0x06001F88 RID: 8072 RVA: 0x00015106 File Offset: 0x00013306
	public static void RemoveTouch(int id)
	{
		UICamera.mTouches.Remove(id);
	}

	// Token: 0x06001F89 RID: 8073 RVA: 0x000F066C File Offset: 0x000EE86C
	private void Awake()
	{
		UICamera.mWidth = Screen.width;
		UICamera.mHeight = Screen.height;
		if (Application.platform == 11 || Application.platform == 8 || Application.platform == 21 || Application.platform == 22)
		{
			this.useTouch = true;
			if (Application.platform == 8)
			{
				this.useMouse = false;
				this.useKeyboard = false;
				this.useController = false;
			}
		}
		else if (Application.platform == 9 || Application.platform == 10)
		{
			this.useMouse = false;
			this.useTouch = false;
			this.useKeyboard = false;
			this.useController = true;
		}
		UICamera.mMouse[0].pos = Input.mousePosition;
		for (int i = 1; i < 3; i++)
		{
			UICamera.mMouse[i].pos = UICamera.mMouse[0].pos;
			UICamera.mMouse[i].lastPos = UICamera.mMouse[0].pos;
		}
		UICamera.lastTouchPosition = UICamera.mMouse[0].pos;
	}

	// Token: 0x06001F8A RID: 8074 RVA: 0x00015114 File Offset: 0x00013314
	private void OnEnable()
	{
		UICamera.list.Add(this);
		UICamera.list.Sort(new BetterList<UICamera>.CompareFunc(UICamera.CompareFunc));
	}

	// Token: 0x06001F8B RID: 8075 RVA: 0x00015137 File Offset: 0x00013337
	private void OnDisable()
	{
		UICamera.list.Remove(this);
	}

	// Token: 0x06001F8C RID: 8076 RVA: 0x000F0788 File Offset: 0x000EE988
	private void Start()
	{
		if (this.eventType != UICamera.EventType.World_3D && this.cachedCamera.transparencySortMode != 2)
		{
			this.cachedCamera.transparencySortMode = 2;
		}
		if (Application.isPlaying)
		{
			if (UICamera.fallThrough == null)
			{
				UIRoot uiroot = NGUITools.FindInParents<UIRoot>(base.gameObject);
				if (uiroot != null)
				{
					UICamera.fallThrough = uiroot.gameObject;
				}
				else
				{
					Transform transform = base.transform;
					UICamera.fallThrough = ((!(transform.parent != null)) ? base.gameObject : transform.parent.gameObject);
				}
			}
			this.cachedCamera.eventMask = 0;
		}
		if (this.handlesEvents)
		{
			NGUIDebug.debugRaycast = this.debug;
		}
	}

	// Token: 0x06001F8D RID: 8077 RVA: 0x000F0854 File Offset: 0x000EEA54
	private void Update()
	{
		if (!this.handlesEvents)
		{
			return;
		}
		UICamera.current = this;
		if (this.useTouch)
		{
			this.ProcessTouches();
		}
		else if (this.useMouse)
		{
			this.ProcessMouse();
		}
		if (UICamera.onCustomInput != null)
		{
			UICamera.onCustomInput();
		}
		if (this.useMouse && UICamera.mCurrentSelection != null)
		{
			if (this.cancelKey0 != null && UICamera.GetKeyDown(this.cancelKey0))
			{
				UICamera.currentScheme = UICamera.ControlScheme.Controller;
				UICamera.currentKey = this.cancelKey0;
				UICamera.selectedObject = null;
			}
			else if (this.cancelKey1 != null && UICamera.GetKeyDown(this.cancelKey1))
			{
				UICamera.currentScheme = UICamera.ControlScheme.Controller;
				UICamera.currentKey = this.cancelKey1;
				UICamera.selectedObject = null;
			}
		}
		if (UICamera.mCurrentSelection == null)
		{
			UICamera.inputHasFocus = false;
		}
		if (UICamera.mCurrentSelection != null)
		{
			this.ProcessOthers();
		}
		if (this.useMouse && UICamera.mHover != null)
		{
			float num = string.IsNullOrEmpty(this.scrollAxisName) ? 0f : UICamera.GetAxis(this.scrollAxisName);
			if (num != 0f)
			{
				if (UICamera.onScroll != null)
				{
					UICamera.onScroll(UICamera.mHover, num);
				}
				UICamera.Notify(UICamera.mHover, "OnScroll", num);
			}
			if (UICamera.showTooltips && this.mTooltipTime != 0f && (this.mTooltipTime < RealTime.time || UICamera.GetKey(304) || UICamera.GetKey(303)))
			{
				this.mTooltip = UICamera.mHover;
				this.ShowTooltip(true);
			}
		}
		UICamera.current = null;
	}

	// Token: 0x06001F8E RID: 8078 RVA: 0x000F0A54 File Offset: 0x000EEC54
	private void LateUpdate()
	{
		if (!this.handlesEvents)
		{
			return;
		}
		int width = Screen.width;
		int height = Screen.height;
		if (width != UICamera.mWidth || height != UICamera.mHeight)
		{
			UICamera.mWidth = width;
			UICamera.mHeight = height;
			UIRoot.Broadcast("UpdateAnchors");
			if (UICamera.onScreenResize != null)
			{
				UICamera.onScreenResize();
			}
		}
	}

	// Token: 0x06001F8F RID: 8079 RVA: 0x000F0ABC File Offset: 0x000EECBC
	public void ProcessMouse()
	{
		UICamera.lastTouchPosition = Input.mousePosition;
		UICamera.mMouse[0].delta = UICamera.lastTouchPosition - UICamera.mMouse[0].pos;
		UICamera.mMouse[0].pos = UICamera.lastTouchPosition;
		bool flag = UICamera.mMouse[0].delta.sqrMagnitude > 0.001f;
		for (int i = 1; i < 3; i++)
		{
			UICamera.mMouse[i].pos = UICamera.mMouse[0].pos;
			UICamera.mMouse[i].delta = UICamera.mMouse[0].delta;
		}
		bool flag2 = false;
		bool flag3 = false;
		for (int j = 0; j < 3; j++)
		{
			if (Input.GetMouseButtonDown(j))
			{
				UICamera.currentScheme = UICamera.ControlScheme.Mouse;
				flag3 = true;
				flag2 = true;
			}
			else if (Input.GetMouseButton(j))
			{
				UICamera.currentScheme = UICamera.ControlScheme.Mouse;
				flag2 = true;
			}
		}
		if (flag2 || flag || this.mNextRaycast < RealTime.time)
		{
			this.mNextRaycast = RealTime.time + 0.02f;
			if (!UICamera.Raycast(Input.mousePosition))
			{
				UICamera.hoveredObject = UICamera.fallThrough;
			}
			if (UICamera.hoveredObject == null)
			{
				UICamera.hoveredObject = UICamera.mGenericHandler;
			}
			for (int k = 0; k < 3; k++)
			{
				UICamera.mMouse[k].current = UICamera.hoveredObject;
			}
		}
		bool flag4 = UICamera.mMouse[0].last != UICamera.mMouse[0].current;
		if (flag4)
		{
			UICamera.currentScheme = UICamera.ControlScheme.Mouse;
		}
		if (flag2)
		{
			this.mTooltipTime = 0f;
		}
		else if (flag && (!this.stickyTooltip || flag4))
		{
			if (this.mTooltipTime != 0f)
			{
				this.mTooltipTime = RealTime.time + this.tooltipDelay;
			}
			else if (this.mTooltip != null)
			{
				this.ShowTooltip(false);
			}
		}
		if (flag && UICamera.onMouseMove != null)
		{
			UICamera.currentTouch = UICamera.mMouse[0];
			UICamera.onMouseMove(UICamera.currentTouch.delta);
			UICamera.currentTouch = null;
		}
		if ((flag3 || !flag2) && UICamera.mHover != null && flag4)
		{
			UICamera.currentScheme = UICamera.ControlScheme.Mouse;
			UICamera.currentTouch = UICamera.mMouse[0];
			if (this.mTooltip != null)
			{
				this.ShowTooltip(false);
			}
			if (UICamera.onHover != null)
			{
				UICamera.onHover(UICamera.mHover, false);
			}
			UICamera.Notify(UICamera.mHover, "OnHover", false);
			UICamera.mHover = null;
		}
		for (int l = 0; l < 3; l++)
		{
			bool mouseButtonDown = Input.GetMouseButtonDown(l);
			bool mouseButtonUp = Input.GetMouseButtonUp(l);
			if (mouseButtonDown || mouseButtonUp)
			{
				UICamera.currentScheme = UICamera.ControlScheme.Mouse;
			}
			UICamera.currentTouch = UICamera.mMouse[l];
			UICamera.currentTouchID = -1 - l;
			UICamera.currentKey = 323 + l;
			if (mouseButtonDown)
			{
				UICamera.currentTouch.pressedCam = UICamera.currentCamera;
			}
			else if (UICamera.currentTouch.pressed != null)
			{
				UICamera.currentCamera = UICamera.currentTouch.pressedCam;
			}
			this.ProcessTouch(mouseButtonDown, mouseButtonUp);
			UICamera.currentKey = 0;
		}
		if (!flag2 && flag4)
		{
			UICamera.currentScheme = UICamera.ControlScheme.Mouse;
			this.mTooltipTime = RealTime.time + this.tooltipDelay;
			UICamera.mHover = UICamera.mMouse[0].current;
			UICamera.currentTouch = UICamera.mMouse[0];
			if (UICamera.onHover != null)
			{
				UICamera.onHover(UICamera.mHover, true);
			}
			UICamera.Notify(UICamera.mHover, "OnHover", true);
		}
		UICamera.currentTouch = null;
		UICamera.mMouse[0].last = UICamera.mMouse[0].current;
		for (int m = 1; m < 3; m++)
		{
			UICamera.mMouse[m].last = UICamera.mMouse[0].last;
		}
	}

	// Token: 0x06001F90 RID: 8080 RVA: 0x000F0EF4 File Offset: 0x000EF0F4
	public void ProcessTouches()
	{
		UICamera.currentScheme = UICamera.ControlScheme.Touch;
		for (int i = 0; i < Input.touchCount; i++)
		{
			Touch touch = Input.GetTouch(i);
			UICamera.currentTouchID = ((!this.allowMultiTouch) ? 1 : touch.fingerId);
			UICamera.currentTouch = UICamera.GetTouch(UICamera.currentTouchID);
			bool flag = touch.phase == null || UICamera.currentTouch.touchBegan;
			bool flag2 = touch.phase == 4 || touch.phase == 3;
			UICamera.currentTouch.touchBegan = false;
			UICamera.currentTouch.delta = ((!flag) ? (touch.position - UICamera.currentTouch.pos) : Vector2.zero);
			UICamera.currentTouch.pos = touch.position;
			if (!UICamera.Raycast(UICamera.currentTouch.pos))
			{
				UICamera.hoveredObject = UICamera.fallThrough;
			}
			if (UICamera.hoveredObject == null)
			{
				UICamera.hoveredObject = UICamera.mGenericHandler;
			}
			UICamera.currentTouch.last = UICamera.currentTouch.current;
			UICamera.currentTouch.current = UICamera.hoveredObject;
			UICamera.lastTouchPosition = UICamera.currentTouch.pos;
			if (flag)
			{
				UICamera.currentTouch.pressedCam = UICamera.currentCamera;
			}
			else if (UICamera.currentTouch.pressed != null)
			{
				UICamera.currentCamera = UICamera.currentTouch.pressedCam;
			}
			if (touch.tapCount > 1)
			{
				UICamera.currentTouch.clickTime = RealTime.time;
			}
			this.ProcessTouch(flag, flag2);
			if (flag2)
			{
				UICamera.RemoveTouch(UICamera.currentTouchID);
			}
			UICamera.currentTouch.last = null;
			UICamera.currentTouch = null;
			if (!this.allowMultiTouch)
			{
				break;
			}
		}
		if (Input.touchCount == 0 && this.useMouse)
		{
			this.ProcessMouse();
		}
	}

	// Token: 0x06001F91 RID: 8081 RVA: 0x000F10F0 File Offset: 0x000EF2F0
	private void ProcessFakeTouches()
	{
		bool mouseButtonDown = Input.GetMouseButtonDown(0);
		bool mouseButtonUp = Input.GetMouseButtonUp(0);
		bool mouseButton = Input.GetMouseButton(0);
		if (mouseButtonDown || mouseButtonUp || mouseButton)
		{
			UICamera.currentTouchID = 1;
			UICamera.currentTouch = UICamera.mMouse[0];
			UICamera.currentTouch.touchBegan = mouseButtonDown;
			if (mouseButtonDown)
			{
				UICamera.currentTouch.pressTime = RealTime.time;
			}
			Vector2 vector = Input.mousePosition;
			UICamera.currentTouch.delta = ((!mouseButtonDown) ? (vector - UICamera.currentTouch.pos) : Vector2.zero);
			UICamera.currentTouch.pos = vector;
			if (!UICamera.Raycast(UICamera.currentTouch.pos))
			{
				UICamera.hoveredObject = UICamera.fallThrough;
			}
			if (UICamera.hoveredObject == null)
			{
				UICamera.hoveredObject = UICamera.mGenericHandler;
			}
			UICamera.currentTouch.last = UICamera.currentTouch.current;
			UICamera.currentTouch.current = UICamera.hoveredObject;
			UICamera.lastTouchPosition = UICamera.currentTouch.pos;
			if (mouseButtonDown)
			{
				UICamera.currentTouch.pressedCam = UICamera.currentCamera;
			}
			else if (UICamera.currentTouch.pressed != null)
			{
				UICamera.currentCamera = UICamera.currentTouch.pressedCam;
			}
			this.ProcessTouch(mouseButtonDown, mouseButtonUp);
			if (mouseButtonUp)
			{
				UICamera.RemoveTouch(UICamera.currentTouchID);
			}
			UICamera.currentTouch.last = null;
			UICamera.currentTouch = null;
		}
	}

	// Token: 0x06001F92 RID: 8082 RVA: 0x000F126C File Offset: 0x000EF46C
	public void ProcessOthers()
	{
		UICamera.currentTouchID = -100;
		UICamera.currentTouch = UICamera.controller;
		bool flag = false;
		bool flag2 = false;
		if (this.submitKey0 != null && UICamera.GetKeyDown(this.submitKey0))
		{
			UICamera.currentKey = this.submitKey0;
			flag = true;
		}
		if (this.submitKey1 != null && UICamera.GetKeyDown(this.submitKey1))
		{
			UICamera.currentKey = this.submitKey1;
			flag = true;
		}
		if (this.submitKey0 != null && UICamera.GetKeyUp(this.submitKey0))
		{
			UICamera.currentKey = this.submitKey0;
			flag2 = true;
		}
		if (this.submitKey1 != null && UICamera.GetKeyUp(this.submitKey1))
		{
			UICamera.currentKey = this.submitKey1;
			flag2 = true;
		}
		if (flag || flag2)
		{
			UICamera.currentScheme = UICamera.ControlScheme.Controller;
			UICamera.currentTouch.last = UICamera.currentTouch.current;
			UICamera.currentTouch.current = UICamera.mCurrentSelection;
			this.ProcessTouch(flag, flag2);
			UICamera.currentTouch.last = null;
		}
		int num = 0;
		int num2 = 0;
		if (this.useKeyboard)
		{
			if (UICamera.inputHasFocus)
			{
				num += UICamera.GetDirection(273, 274);
				num2 += UICamera.GetDirection(275, 276);
			}
			else
			{
				num += UICamera.GetDirection(119, 273, 115, 274);
				num2 += UICamera.GetDirection(100, 275, 97, 276);
			}
		}
		if (this.useController)
		{
			if (!string.IsNullOrEmpty(this.verticalAxisName))
			{
				num += UICamera.GetDirection(this.verticalAxisName);
			}
			if (!string.IsNullOrEmpty(this.horizontalAxisName))
			{
				num2 += UICamera.GetDirection(this.horizontalAxisName);
			}
		}
		if (num != 0)
		{
			UICamera.currentScheme = UICamera.ControlScheme.Controller;
			KeyCode keyCode = (num <= 0) ? 274 : 273;
			if (UICamera.onKey != null)
			{
				UICamera.onKey(UICamera.mCurrentSelection, keyCode);
			}
			UICamera.Notify(UICamera.mCurrentSelection, "OnKey", keyCode);
		}
		if (num2 != 0)
		{
			UICamera.currentScheme = UICamera.ControlScheme.Controller;
			KeyCode keyCode2 = (num2 <= 0) ? 276 : 275;
			if (UICamera.onKey != null)
			{
				UICamera.onKey(UICamera.mCurrentSelection, keyCode2);
			}
			UICamera.Notify(UICamera.mCurrentSelection, "OnKey", keyCode2);
		}
		if (this.useKeyboard && UICamera.GetKeyDown(9))
		{
			UICamera.currentKey = 9;
			UICamera.currentScheme = UICamera.ControlScheme.Controller;
			if (UICamera.onKey != null)
			{
				UICamera.onKey(UICamera.mCurrentSelection, 9);
			}
			UICamera.Notify(UICamera.mCurrentSelection, "OnKey", 9);
		}
		if (this.cancelKey0 != null && UICamera.GetKeyDown(this.cancelKey0))
		{
			UICamera.currentKey = this.cancelKey0;
			UICamera.currentScheme = UICamera.ControlScheme.Controller;
			if (UICamera.onKey != null)
			{
				UICamera.onKey(UICamera.mCurrentSelection, 27);
			}
			UICamera.Notify(UICamera.mCurrentSelection, "OnKey", 27);
		}
		if (this.cancelKey1 != null && UICamera.GetKeyDown(this.cancelKey1))
		{
			UICamera.currentKey = this.cancelKey1;
			UICamera.currentScheme = UICamera.ControlScheme.Controller;
			if (UICamera.onKey != null)
			{
				UICamera.onKey(UICamera.mCurrentSelection, 27);
			}
			UICamera.Notify(UICamera.mCurrentSelection, "OnKey", 27);
		}
		UICamera.currentTouch = null;
		UICamera.currentKey = 0;
	}

	// Token: 0x06001F93 RID: 8083 RVA: 0x000F1614 File Offset: 0x000EF814
	public void ProcessTouch(bool pressed, bool unpressed)
	{
		bool flag = UICamera.currentScheme == UICamera.ControlScheme.Mouse;
		float num = (!flag) ? this.touchDragThreshold : this.mouseDragThreshold;
		float num2 = (!flag) ? this.touchClickThreshold : this.mouseClickThreshold;
		num *= num;
		num2 *= num2;
		if (pressed)
		{
			if (this.mTooltip != null)
			{
				this.ShowTooltip(false);
			}
			UICamera.currentTouch.pressStarted = true;
			if (UICamera.onPress != null)
			{
				UICamera.onPress(UICamera.currentTouch.pressed, false);
			}
			UICamera.Notify(UICamera.currentTouch.pressed, "OnPress", false);
			UICamera.currentTouch.pressed = UICamera.currentTouch.current;
			UICamera.currentTouch.dragged = UICamera.currentTouch.current;
			UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;
			UICamera.currentTouch.totalDelta = Vector2.zero;
			UICamera.currentTouch.dragStarted = false;
			if (UICamera.onPress != null)
			{
				UICamera.onPress(UICamera.currentTouch.pressed, true);
			}
			UICamera.Notify(UICamera.currentTouch.pressed, "OnPress", true);
			if (UICamera.currentTouch.pressed != UICamera.mCurrentSelection)
			{
				if (this.mTooltip != null)
				{
					this.ShowTooltip(false);
				}
				UICamera.currentScheme = UICamera.ControlScheme.Touch;
				UICamera.selectedObject = UICamera.currentTouch.pressed;
			}
		}
		else if (UICamera.currentTouch.pressed != null && (UICamera.currentTouch.delta.sqrMagnitude != 0f || UICamera.currentTouch.current != UICamera.currentTouch.last))
		{
			UICamera.currentTouch.totalDelta += UICamera.currentTouch.delta;
			float sqrMagnitude = UICamera.currentTouch.totalDelta.sqrMagnitude;
			bool flag2 = false;
			if (!UICamera.currentTouch.dragStarted && UICamera.currentTouch.last != UICamera.currentTouch.current)
			{
				UICamera.currentTouch.dragStarted = true;
				UICamera.currentTouch.delta = UICamera.currentTouch.totalDelta;
				UICamera.isDragging = true;
				if (UICamera.onDragStart != null)
				{
					UICamera.onDragStart(UICamera.currentTouch.dragged);
				}
				UICamera.Notify(UICamera.currentTouch.dragged, "OnDragStart", null);
				if (UICamera.onDragOver != null)
				{
					UICamera.onDragOver(UICamera.currentTouch.last, UICamera.currentTouch.dragged);
				}
				UICamera.Notify(UICamera.currentTouch.last, "OnDragOver", UICamera.currentTouch.dragged);
				UICamera.isDragging = false;
			}
			else if (!UICamera.currentTouch.dragStarted && num < sqrMagnitude)
			{
				flag2 = true;
				UICamera.currentTouch.dragStarted = true;
				UICamera.currentTouch.delta = UICamera.currentTouch.totalDelta;
			}
			if (UICamera.currentTouch.dragStarted)
			{
				if (this.mTooltip != null)
				{
					this.ShowTooltip(false);
				}
				UICamera.isDragging = true;
				bool flag3 = UICamera.currentTouch.clickNotification == UICamera.ClickNotification.None;
				if (flag2)
				{
					if (UICamera.onDragStart != null)
					{
						UICamera.onDragStart(UICamera.currentTouch.dragged);
					}
					UICamera.Notify(UICamera.currentTouch.dragged, "OnDragStart", null);
					if (UICamera.onDragOver != null)
					{
						UICamera.onDragOver(UICamera.currentTouch.last, UICamera.currentTouch.dragged);
					}
					UICamera.Notify(UICamera.currentTouch.current, "OnDragOver", UICamera.currentTouch.dragged);
				}
				else if (UICamera.currentTouch.last != UICamera.currentTouch.current)
				{
					if (UICamera.onDragStart != null)
					{
						UICamera.onDragStart(UICamera.currentTouch.dragged);
					}
					UICamera.Notify(UICamera.currentTouch.last, "OnDragOut", UICamera.currentTouch.dragged);
					if (UICamera.onDragOver != null)
					{
						UICamera.onDragOver(UICamera.currentTouch.last, UICamera.currentTouch.dragged);
					}
					UICamera.Notify(UICamera.currentTouch.current, "OnDragOver", UICamera.currentTouch.dragged);
				}
				if (UICamera.onDrag != null)
				{
					UICamera.onDrag(UICamera.currentTouch.dragged, UICamera.currentTouch.delta);
				}
				UICamera.Notify(UICamera.currentTouch.dragged, "OnDrag", UICamera.currentTouch.delta);
				UICamera.currentTouch.last = UICamera.currentTouch.current;
				UICamera.isDragging = false;
				if (flag3)
				{
					UICamera.currentTouch.clickNotification = UICamera.ClickNotification.None;
				}
				else if (UICamera.currentTouch.clickNotification == UICamera.ClickNotification.BasedOnDelta && num2 < sqrMagnitude)
				{
					UICamera.currentTouch.clickNotification = UICamera.ClickNotification.None;
				}
			}
		}
		if (unpressed)
		{
			UICamera.currentTouch.pressStarted = false;
			if (this.mTooltip != null)
			{
				this.ShowTooltip(false);
			}
			if (UICamera.currentTouch.pressed != null)
			{
				if (UICamera.currentTouch.dragStarted)
				{
					if (UICamera.onDragOut != null)
					{
						UICamera.onDragOut(UICamera.currentTouch.last, UICamera.currentTouch.dragged);
					}
					UICamera.Notify(UICamera.currentTouch.last, "OnDragOut", UICamera.currentTouch.dragged);
					if (UICamera.onDragEnd != null)
					{
						UICamera.onDragEnd(UICamera.currentTouch.dragged);
					}
					UICamera.Notify(UICamera.currentTouch.dragged, "OnDragEnd", null);
				}
				if (UICamera.onPress != null)
				{
					UICamera.onPress(UICamera.currentTouch.pressed, false);
				}
				UICamera.Notify(UICamera.currentTouch.pressed, "OnPress", false);
				if (flag)
				{
					if (UICamera.onHover != null)
					{
						UICamera.onHover(UICamera.currentTouch.current, true);
					}
					UICamera.Notify(UICamera.currentTouch.current, "OnHover", true);
				}
				UICamera.mHover = UICamera.currentTouch.current;
				if (UICamera.currentTouch.dragged == UICamera.currentTouch.current || (UICamera.currentScheme != UICamera.ControlScheme.Controller && UICamera.currentTouch.clickNotification != UICamera.ClickNotification.None && UICamera.currentTouch.totalDelta.sqrMagnitude < num))
				{
					if (UICamera.currentTouch.pressed != UICamera.mCurrentSelection)
					{
						UICamera.mNextSelection = null;
						UICamera.mCurrentSelection = UICamera.currentTouch.pressed;
						if (UICamera.onSelect != null)
						{
							UICamera.onSelect(UICamera.currentTouch.pressed, true);
						}
						UICamera.Notify(UICamera.currentTouch.pressed, "OnSelect", true);
					}
					else
					{
						UICamera.mNextSelection = null;
						UICamera.mCurrentSelection = UICamera.currentTouch.pressed;
					}
					if (UICamera.currentTouch.clickNotification != UICamera.ClickNotification.None && UICamera.currentTouch.pressed == UICamera.currentTouch.current)
					{
						float time = RealTime.time;
						if (UICamera.onClick != null)
						{
							UICamera.onClick(UICamera.currentTouch.pressed);
						}
						UICamera.Notify(UICamera.currentTouch.pressed, "OnClick", null);
						if (UICamera.currentTouch.clickTime + 0.35f > time)
						{
							if (UICamera.onDoubleClick != null)
							{
								UICamera.onDoubleClick(UICamera.currentTouch.pressed);
							}
							UICamera.Notify(UICamera.currentTouch.pressed, "OnDoubleClick", null);
						}
						UICamera.currentTouch.clickTime = time;
					}
				}
				else if (UICamera.currentTouch.dragStarted)
				{
					if (UICamera.onDrop != null)
					{
						UICamera.onDrop(UICamera.currentTouch.current, UICamera.currentTouch.dragged);
					}
					UICamera.Notify(UICamera.currentTouch.current, "OnDrop", UICamera.currentTouch.dragged);
				}
			}
			UICamera.currentTouch.dragStarted = false;
			UICamera.currentTouch.pressed = null;
			UICamera.currentTouch.dragged = null;
		}
	}

	// Token: 0x06001F94 RID: 8084 RVA: 0x000F1E74 File Offset: 0x000F0074
	public void ShowTooltip(bool val)
	{
		this.mTooltipTime = 0f;
		if (UICamera.onTooltip != null)
		{
			UICamera.onTooltip(this.mTooltip, val);
		}
		UICamera.Notify(this.mTooltip, "OnTooltip", val);
		if (!val)
		{
			this.mTooltip = null;
		}
	}

	// Token: 0x06001F95 RID: 8085 RVA: 0x000F1ECC File Offset: 0x000F00CC
	private void OnApplicationPause()
	{
		UICamera.MouseOrTouch mouseOrTouch = UICamera.currentTouch;
		if (this.useTouch)
		{
			BetterList<int> betterList = new BetterList<int>();
			foreach (KeyValuePair<int, UICamera.MouseOrTouch> keyValuePair in UICamera.mTouches)
			{
				if (keyValuePair.Value != null && keyValuePair.Value.pressed)
				{
					UICamera.currentTouch = keyValuePair.Value;
					UICamera.currentTouchID = keyValuePair.Key;
					UICamera.currentScheme = UICamera.ControlScheme.Touch;
					UICamera.currentTouch.clickNotification = UICamera.ClickNotification.None;
					this.ProcessTouch(false, true);
					betterList.Add(UICamera.currentTouchID);
				}
			}
			for (int i = 0; i < betterList.size; i++)
			{
				UICamera.RemoveTouch(betterList[i]);
			}
		}
		if (this.useMouse)
		{
			for (int j = 0; j < 3; j++)
			{
				if (UICamera.mMouse[j].pressed)
				{
					UICamera.currentTouch = UICamera.mMouse[j];
					UICamera.currentTouchID = -1 - j;
					UICamera.currentKey = 323 + j;
					UICamera.currentScheme = UICamera.ControlScheme.Mouse;
					UICamera.currentTouch.clickNotification = UICamera.ClickNotification.None;
					this.ProcessTouch(false, true);
				}
			}
		}
		if (this.useController && UICamera.controller.pressed)
		{
			UICamera.currentTouch = UICamera.controller;
			UICamera.currentTouchID = -100;
			UICamera.currentScheme = UICamera.ControlScheme.Controller;
			UICamera.currentTouch.last = UICamera.currentTouch.current;
			UICamera.currentTouch.current = UICamera.mCurrentSelection;
			UICamera.currentTouch.clickNotification = UICamera.ClickNotification.None;
			this.ProcessTouch(false, true);
			UICamera.currentTouch.last = null;
		}
		UICamera.currentTouch = mouseOrTouch;
	}

	// Token: 0x04002302 RID: 8962
	public static BetterList<UICamera> list = new BetterList<UICamera>();

	// Token: 0x04002303 RID: 8963
	public static UICamera.GetKeyStateFunc GetKeyDown = new UICamera.GetKeyStateFunc(Input.GetKeyDown);

	// Token: 0x04002304 RID: 8964
	public static UICamera.GetKeyStateFunc GetKeyUp = new UICamera.GetKeyStateFunc(Input.GetKeyUp);

	// Token: 0x04002305 RID: 8965
	public static UICamera.GetKeyStateFunc GetKey = new UICamera.GetKeyStateFunc(Input.GetKey);

	// Token: 0x04002306 RID: 8966
	public static UICamera.GetAxisFunc GetAxis = new UICamera.GetAxisFunc(Input.GetAxis);

	// Token: 0x04002307 RID: 8967
	public static UICamera.OnScreenResize onScreenResize;

	// Token: 0x04002308 RID: 8968
	public UICamera.EventType eventType = UICamera.EventType.UI_3D;

	// Token: 0x04002309 RID: 8969
	public bool eventsGoToColliders;

	// Token: 0x0400230A RID: 8970
	public LayerMask eventReceiverMask = -1;

	// Token: 0x0400230B RID: 8971
	public bool debug;

	// Token: 0x0400230C RID: 8972
	public bool useMouse = true;

	// Token: 0x0400230D RID: 8973
	public bool useTouch = true;

	// Token: 0x0400230E RID: 8974
	public bool allowMultiTouch = true;

	// Token: 0x0400230F RID: 8975
	public bool useKeyboard = true;

	// Token: 0x04002310 RID: 8976
	public bool useController = true;

	// Token: 0x04002311 RID: 8977
	public bool stickyTooltip = true;

	// Token: 0x04002312 RID: 8978
	public float tooltipDelay = 1f;

	// Token: 0x04002313 RID: 8979
	public float mouseDragThreshold = 4f;

	// Token: 0x04002314 RID: 8980
	public float mouseClickThreshold = 10f;

	// Token: 0x04002315 RID: 8981
	public float touchDragThreshold = 40f;

	// Token: 0x04002316 RID: 8982
	public float touchClickThreshold = 40f;

	// Token: 0x04002317 RID: 8983
	public float rangeDistance = -1f;

	// Token: 0x04002318 RID: 8984
	public string scrollAxisName = "Mouse ScrollWheel";

	// Token: 0x04002319 RID: 8985
	public string verticalAxisName = "Vertical";

	// Token: 0x0400231A RID: 8986
	public string horizontalAxisName = "Horizontal";

	// Token: 0x0400231B RID: 8987
	public KeyCode submitKey0 = 13;

	// Token: 0x0400231C RID: 8988
	public KeyCode submitKey1 = 330;

	// Token: 0x0400231D RID: 8989
	public KeyCode cancelKey0 = 27;

	// Token: 0x0400231E RID: 8990
	public KeyCode cancelKey1 = 331;

	// Token: 0x0400231F RID: 8991
	public static UICamera.OnCustomInput onCustomInput;

	// Token: 0x04002320 RID: 8992
	public static bool showTooltips = true;

	// Token: 0x04002321 RID: 8993
	public static Vector2 lastTouchPosition = Vector2.zero;

	// Token: 0x04002322 RID: 8994
	public static Vector3 lastWorldPosition = Vector3.zero;

	// Token: 0x04002323 RID: 8995
	public static RaycastHit lastHit;

	// Token: 0x04002324 RID: 8996
	public static UICamera current = null;

	// Token: 0x04002325 RID: 8997
	public static Camera currentCamera = null;

	// Token: 0x04002326 RID: 8998
	public static UICamera.ControlScheme currentScheme = UICamera.ControlScheme.Mouse;

	// Token: 0x04002327 RID: 8999
	public static int currentTouchID = -1;

	// Token: 0x04002328 RID: 9000
	public static KeyCode currentKey = 0;

	// Token: 0x04002329 RID: 9001
	public static UICamera.MouseOrTouch currentTouch = null;

	// Token: 0x0400232A RID: 9002
	public static bool inputHasFocus = false;

	// Token: 0x0400232B RID: 9003
	private static GameObject mGenericHandler;

	// Token: 0x0400232C RID: 9004
	public static GameObject fallThrough;

	// Token: 0x0400232D RID: 9005
	public static UICamera.VoidDelegate onClick;

	// Token: 0x0400232E RID: 9006
	public static UICamera.VoidDelegate onDoubleClick;

	// Token: 0x0400232F RID: 9007
	public static UICamera.BoolDelegate onHover;

	// Token: 0x04002330 RID: 9008
	public static UICamera.BoolDelegate onPress;

	// Token: 0x04002331 RID: 9009
	public static UICamera.BoolDelegate onSelect;

	// Token: 0x04002332 RID: 9010
	public static UICamera.FloatDelegate onScroll;

	// Token: 0x04002333 RID: 9011
	public static UICamera.VectorDelegate onDrag;

	// Token: 0x04002334 RID: 9012
	public static UICamera.VoidDelegate onDragStart;

	// Token: 0x04002335 RID: 9013
	public static UICamera.ObjectDelegate onDragOver;

	// Token: 0x04002336 RID: 9014
	public static UICamera.ObjectDelegate onDragOut;

	// Token: 0x04002337 RID: 9015
	public static UICamera.VoidDelegate onDragEnd;

	// Token: 0x04002338 RID: 9016
	public static UICamera.ObjectDelegate onDrop;

	// Token: 0x04002339 RID: 9017
	public static UICamera.KeyCodeDelegate onKey;

	// Token: 0x0400233A RID: 9018
	public static UICamera.BoolDelegate onTooltip;

	// Token: 0x0400233B RID: 9019
	public static UICamera.MoveDelegate onMouseMove;

	// Token: 0x0400233C RID: 9020
	private static GameObject mCurrentSelection = null;

	// Token: 0x0400233D RID: 9021
	private static GameObject mNextSelection = null;

	// Token: 0x0400233E RID: 9022
	private static UICamera.ControlScheme mNextScheme = UICamera.ControlScheme.Controller;

	// Token: 0x0400233F RID: 9023
	private static UICamera.MouseOrTouch[] mMouse = new UICamera.MouseOrTouch[]
	{
		new UICamera.MouseOrTouch(),
		new UICamera.MouseOrTouch(),
		new UICamera.MouseOrTouch()
	};

	// Token: 0x04002340 RID: 9024
	private static GameObject mHover;

	// Token: 0x04002341 RID: 9025
	public static UICamera.MouseOrTouch controller = new UICamera.MouseOrTouch();

	// Token: 0x04002342 RID: 9026
	private static float mNextEvent = 0f;

	// Token: 0x04002343 RID: 9027
	private static Dictionary<int, UICamera.MouseOrTouch> mTouches = new Dictionary<int, UICamera.MouseOrTouch>();

	// Token: 0x04002344 RID: 9028
	private static int mWidth = 0;

	// Token: 0x04002345 RID: 9029
	private static int mHeight = 0;

	// Token: 0x04002346 RID: 9030
	private GameObject mTooltip;

	// Token: 0x04002347 RID: 9031
	private Camera mCam;

	// Token: 0x04002348 RID: 9032
	private float mTooltipTime;

	// Token: 0x04002349 RID: 9033
	private float mNextRaycast;

	// Token: 0x0400234A RID: 9034
	public static bool isDragging = false;

	// Token: 0x0400234B RID: 9035
	public static GameObject hoveredObject;

	// Token: 0x0400234C RID: 9036
	private static UICamera.DepthEntry mHit = default(UICamera.DepthEntry);

	// Token: 0x0400234D RID: 9037
	private static BetterList<UICamera.DepthEntry> mHits = new BetterList<UICamera.DepthEntry>();

	// Token: 0x0400234E RID: 9038
	private static Plane m2DPlane = new Plane(Vector3.back, 0f);

	// Token: 0x0400234F RID: 9039
	private static bool mNotifying = false;

	// Token: 0x020004E1 RID: 1249
	public enum ControlScheme
	{
		// Token: 0x04002353 RID: 9043
		Mouse,
		// Token: 0x04002354 RID: 9044
		Touch,
		// Token: 0x04002355 RID: 9045
		Controller
	}

	// Token: 0x020004E2 RID: 1250
	public enum ClickNotification
	{
		// Token: 0x04002357 RID: 9047
		None,
		// Token: 0x04002358 RID: 9048
		Always,
		// Token: 0x04002359 RID: 9049
		BasedOnDelta
	}

	// Token: 0x020004E3 RID: 1251
	public class MouseOrTouch
	{
		// Token: 0x170002DA RID: 730
		// (get) Token: 0x06001F99 RID: 8089 RVA: 0x00015170 File Offset: 0x00013370
		public float deltaTime
		{
			get
			{
				return (!this.touchBegan) ? 0f : (RealTime.time - this.pressTime);
			}
		}

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x06001F9A RID: 8090 RVA: 0x00015193 File Offset: 0x00013393
		public bool isOverUI
		{
			get
			{
				return this.current != null && this.current != UICamera.fallThrough && NGUITools.FindInParents<UIRoot>(this.current) != null;
			}
		}

		// Token: 0x0400235A RID: 9050
		public Vector2 pos;

		// Token: 0x0400235B RID: 9051
		public Vector2 lastPos;

		// Token: 0x0400235C RID: 9052
		public Vector2 delta;

		// Token: 0x0400235D RID: 9053
		public Vector2 totalDelta;

		// Token: 0x0400235E RID: 9054
		public Camera pressedCam;

		// Token: 0x0400235F RID: 9055
		public GameObject last;

		// Token: 0x04002360 RID: 9056
		public GameObject current;

		// Token: 0x04002361 RID: 9057
		public GameObject pressed;

		// Token: 0x04002362 RID: 9058
		public GameObject dragged;

		// Token: 0x04002363 RID: 9059
		public float pressTime;

		// Token: 0x04002364 RID: 9060
		public float clickTime;

		// Token: 0x04002365 RID: 9061
		public UICamera.ClickNotification clickNotification = UICamera.ClickNotification.Always;

		// Token: 0x04002366 RID: 9062
		public bool touchBegan = true;

		// Token: 0x04002367 RID: 9063
		public bool pressStarted;

		// Token: 0x04002368 RID: 9064
		public bool dragStarted;
	}

	// Token: 0x020004E4 RID: 1252
	public enum EventType
	{
		// Token: 0x0400236A RID: 9066
		World_3D,
		// Token: 0x0400236B RID: 9067
		UI_3D,
		// Token: 0x0400236C RID: 9068
		World_2D,
		// Token: 0x0400236D RID: 9069
		UI_2D
	}

	// Token: 0x020004E5 RID: 1253
	private struct DepthEntry
	{
		// Token: 0x0400236E RID: 9070
		public int depth;

		// Token: 0x0400236F RID: 9071
		public RaycastHit hit;

		// Token: 0x04002370 RID: 9072
		public Vector3 point;

		// Token: 0x04002371 RID: 9073
		public GameObject go;
	}

	// Token: 0x020004E6 RID: 1254
	// (Invoke) Token: 0x06001F9C RID: 8092
	public delegate bool GetKeyStateFunc(KeyCode key);

	// Token: 0x020004E7 RID: 1255
	// (Invoke) Token: 0x06001FA0 RID: 8096
	public delegate float GetAxisFunc(string name);

	// Token: 0x020004E8 RID: 1256
	// (Invoke) Token: 0x06001FA4 RID: 8100
	public delegate void OnScreenResize();

	// Token: 0x020004E9 RID: 1257
	// (Invoke) Token: 0x06001FA8 RID: 8104
	public delegate void OnCustomInput();

	// Token: 0x020004EA RID: 1258
	// (Invoke) Token: 0x06001FAC RID: 8108
	public delegate void MoveDelegate(Vector2 delta);

	// Token: 0x020004EB RID: 1259
	// (Invoke) Token: 0x06001FB0 RID: 8112
	public delegate void VoidDelegate(GameObject go);

	// Token: 0x020004EC RID: 1260
	// (Invoke) Token: 0x06001FB4 RID: 8116
	public delegate void BoolDelegate(GameObject go, bool state);

	// Token: 0x020004ED RID: 1261
	// (Invoke) Token: 0x06001FB8 RID: 8120
	public delegate void FloatDelegate(GameObject go, float delta);

	// Token: 0x020004EE RID: 1262
	// (Invoke) Token: 0x06001FBC RID: 8124
	public delegate void VectorDelegate(GameObject go, Vector2 delta);

	// Token: 0x020004EF RID: 1263
	// (Invoke) Token: 0x06001FC0 RID: 8128
	public delegate void ObjectDelegate(GameObject go, GameObject obj);

	// Token: 0x020004F0 RID: 1264
	// (Invoke) Token: 0x06001FC4 RID: 8132
	public delegate void KeyCodeDelegate(GameObject go, KeyCode key);
}
