using System;
using System.Collections.Generic;
using System.IO;
using Heluo.Wulin;
using UnityEngine;

// Token: 0x020001B5 RID: 437
public class InputManager
{
	// Token: 0x06000925 RID: 2341 RVA: 0x0004F608 File Offset: 0x0004D808
	public InputManager()
	{
		bool flag = false;
		if (!this.loadFromData<KeyboardControl>("hotkey.ini"))
		{
			flag = true;
			Dictionary<KeyControl.Key, KeyCode> dictionary = new Dictionary<KeyControl.Key, KeyCode>();
			dictionary.Add(KeyControl.Key.Up, 119);
			dictionary.Add(KeyControl.Key.Down, 115);
			dictionary.Add(KeyControl.Key.Left, 97);
			dictionary.Add(KeyControl.Key.Right, 100);
			dictionary.Add(KeyControl.Key.ArrowUp, 273);
			dictionary.Add(KeyControl.Key.ArrowDown, 274);
			dictionary.Add(KeyControl.Key.ArrowLeft, 276);
			dictionary.Add(KeyControl.Key.ArrowRight, 275);
			dictionary.Add(KeyControl.Key.OK, 13);
			dictionary.Add(KeyControl.Key.Cancel, 27);
			dictionary.Add(KeyControl.Key.X, 117);
			dictionary.Add(KeyControl.Key.Y, 105);
			dictionary.Add(KeyControl.Key.Menu, 27);
			dictionary.Add(KeyControl.Key.Select, 13);
			dictionary.Add(KeyControl.Key.Jump, 117);
			dictionary.Add(KeyControl.Key.ChangeModel, 9);
			dictionary.Add(KeyControl.Key.L1, 117);
			dictionary.Add(KeyControl.Key.R1, 105);
			dictionary.Add(KeyControl.Key.TalkLog, 102);
			dictionary.Add(KeyControl.Key.Character, 282);
			dictionary.Add(KeyControl.Key.Team, 283);
			dictionary.Add(KeyControl.Key.Backpack, 284);
			dictionary.Add(KeyControl.Key.Mission, 285);
			dictionary.Add(KeyControl.Key.Rumor, 286);
			dictionary.Add(KeyControl.Key.Save, 287);
			dictionary.Add(KeyControl.Key.Load, 288);
			dictionary.Add(KeyControl.Key.AddSpeed, 306);
			dictionary.Add(KeyControl.Key.ShowHideItemEft, 278);
			dictionary.Add(KeyControl.Key.RotateLeft, 113);
			dictionary.Add(KeyControl.Key.RotateRight, 101);
			dictionary.Add(KeyControl.Key.BattlePrevUnit, 117);
			dictionary.Add(KeyControl.Key.BattleNextUnit, 9);
			dictionary.Add(KeyControl.Key.UnitInfo, 105);
			dictionary.Add(KeyControl.Key.FindTile, 106);
			dictionary.Add(KeyControl.Key.PlaceAllUnit, 104);
			dictionary.Add(KeyControl.Key.Skill1, 49);
			dictionary.Add(KeyControl.Key.Skill2, 50);
			dictionary.Add(KeyControl.Key.Skill3, 51);
			dictionary.Add(KeyControl.Key.Skill4, 52);
			dictionary.Add(KeyControl.Key.Skill5, 53);
			dictionary.Add(KeyControl.Key.Skill6, 54);
			dictionary.Add(KeyControl.Key.SelectItem, 55);
			dictionary.Add(KeyControl.Key.RestTurn, 56);
			KeyboardControl keyboardControl = new KeyboardControl(dictionary)
			{
				KeyDown = new Action<KeyControl.Key>(this.OnKeyDown),
				KeyUp = new Action<KeyControl.Key>(this.OnKeyUp),
				KeyHeld = new Action<KeyControl.Key>(this.OnKeyHeld)
			};
			this.input.Add(keyboardControl);
		}
		if (!this.loadFromData<JoystickControl>("hotkey_joystick.ini"))
		{
			flag = true;
			Dictionary<KeyControl.Key, KeyCode> dictionary = new Dictionary<KeyControl.Key, KeyCode>();
			dictionary.Add(KeyControl.Key.OK, 350);
			dictionary.Add(KeyControl.Key.Cancel, 351);
			dictionary.Add(KeyControl.Key.X, 352);
			dictionary.Add(KeyControl.Key.Y, 353);
			dictionary.Add(KeyControl.Key.Menu, 357);
			dictionary.Add(KeyControl.Key.Select, 356);
			dictionary.Add(KeyControl.Key.L1, 354);
			dictionary.Add(KeyControl.Key.R1, 355);
			dictionary.Add(KeyControl.Key.L2, 358);
			dictionary.Add(KeyControl.Key.R2, 359);
			dictionary.Add(KeyControl.Key.Jump, 352);
			dictionary.Add(KeyControl.Key.ChangeModel, 353);
			dictionary.Add(KeyControl.Key.AddSpeed, 351);
			dictionary.Add(KeyControl.Key.TalkLog, 359);
			dictionary.Add(KeyControl.Key.RotateLeft, 358);
			dictionary.Add(KeyControl.Key.RotateRight, 359);
			dictionary.Add(KeyControl.Key.BattlePrevUnit, 354);
			dictionary.Add(KeyControl.Key.BattleNextUnit, 355);
			dictionary.Add(KeyControl.Key.UnitInfo, 352);
			dictionary.Add(KeyControl.Key.FindTile, 353);
			dictionary.Add(KeyControl.Key.PlaceAllUnit, 356);
			JoystickControl joystickControl = new JoystickControl(dictionary)
			{
				KeyDown = new Action<KeyControl.Key>(this.OnKeyDown),
				KeyUp = new Action<KeyControl.Key>(this.OnKeyUp),
				KeyHeld = new Action<KeyControl.Key>(this.OnKeyHeld)
			};
			this.input.Add(joystickControl);
		}
		if (flag)
		{
			this.SaveData<KeyboardControl>("hotkey.ini");
			this.SaveData<JoystickControl>("hotkey_joystick.ini");
		}
	}

	// Token: 0x14000029 RID: 41
	// (add) Token: 0x06000926 RID: 2342 RVA: 0x00007802 File Offset: 0x00005A02
	// (remove) Token: 0x06000927 RID: 2343 RVA: 0x0000781B File Offset: 0x00005A1B
	public event Action<KeyControl.Key> KeyDown;

	// Token: 0x1400002A RID: 42
	// (add) Token: 0x06000928 RID: 2344 RVA: 0x00007834 File Offset: 0x00005A34
	// (remove) Token: 0x06000929 RID: 2345 RVA: 0x0000784D File Offset: 0x00005A4D
	public event Action<KeyControl.Key> KeyUp;

	// Token: 0x1400002B RID: 43
	// (add) Token: 0x0600092A RID: 2346 RVA: 0x00007866 File Offset: 0x00005A66
	// (remove) Token: 0x0600092B RID: 2347 RVA: 0x0000787F File Offset: 0x00005A7F
	public event Action<KeyControl.Key> KeyHeld;

	// Token: 0x1400002C RID: 44
	// (add) Token: 0x0600092C RID: 2348 RVA: 0x00007898 File Offset: 0x00005A98
	// (remove) Token: 0x0600092D RID: 2349 RVA: 0x000078B1 File Offset: 0x00005AB1
	public event Action<Vector2> Move;

	// Token: 0x1400002D RID: 45
	// (add) Token: 0x0600092E RID: 2350 RVA: 0x000078CA File Offset: 0x00005ACA
	// (remove) Token: 0x0600092F RID: 2351 RVA: 0x000078E3 File Offset: 0x00005AE3
	public event Action<bool> MouseControl;

	// Token: 0x1700010C RID: 268
	// (get) Token: 0x06000930 RID: 2352 RVA: 0x000078FC File Offset: 0x00005AFC
	public int Count
	{
		get
		{
			return this.controller.Count;
		}
	}

	// Token: 0x06000931 RID: 2353 RVA: 0x0004F9F8 File Offset: 0x0004DBF8
	public void SaveData<T>(string filename) where T : KeyControl
	{
		string text = Application.dataPath.Replace("/Assets", string.Empty);
		text = text + "/" + filename;
		StreamWriter streamWriter = new StreamWriter(text);
		T t = (T)((object)null);
		for (int i = 0; i < this.input.Count; i++)
		{
			KeyControl keyControl = this.input[i];
			if (keyControl.GetType() == typeof(T))
			{
				t = (keyControl as T);
			}
		}
		if (t == null)
		{
			return;
		}
		KeyCode[] keyCodeArray = t.GetKeyCodeArray();
		if (keyCodeArray == null)
		{
			return;
		}
		string text2 = string.Format("#{0}\t{1}\n", "Key", "KeyCode");
		for (int j = 0; j < keyCodeArray.Length; j++)
		{
			if (keyCodeArray[j] != null)
			{
				string text3 = string.Format("{0}\t{1}\n", ((KeyControl.Key)j).ToString(), keyCodeArray[j].ToString());
				text2 += text3;
			}
		}
		streamWriter.Write(text2);
		streamWriter.Close();
	}

	// Token: 0x06000932 RID: 2354 RVA: 0x0004FB20 File Offset: 0x0004DD20
	private bool loadFromData<T>(string filename) where T : KeyControl, new()
	{
		string text = Application.dataPath.Replace("/Assets", string.Empty);
		text = text + "/" + filename;
		if (!File.Exists(text))
		{
			return false;
		}
		StreamReader streamReader = new StreamReader(text);
		Dictionary<KeyControl.Key, KeyCode> dictionary = new Dictionary<KeyControl.Key, KeyCode>();
		string text2;
		while ((text2 = streamReader.ReadLine()) != null)
		{
			if (text2.get_Chars(0) != '#')
			{
				text2 = text2.Replace("\n", string.Empty);
				string[] array = text2.Split(new char[]
				{
					"\t".get_Chars(0)
				});
				KeyControl.Key key = (KeyControl.Key)((int)Enum.Parse(typeof(KeyControl.Key), array[0]));
				KeyCode keyCode = (int)Enum.Parse(typeof(KeyCode), array[1]);
				if (!dictionary.ContainsKey(key))
				{
					dictionary.Add(key, keyCode);
				}
			}
		}
		if (dictionary.Count == 0)
		{
			return false;
		}
		T t = Activator.CreateInstance<T>();
		t.KeyDown = new Action<KeyControl.Key>(this.OnKeyDown);
		t.KeyUp = new Action<KeyControl.Key>(this.OnKeyUp);
		t.KeyHeld = new Action<KeyControl.Key>(this.OnKeyHeld);
		T t2 = t;
		t2.SetMapping(dictionary);
		this.input.Add(t2);
		streamReader.Close();
		return true;
	}

	// Token: 0x06000933 RID: 2355 RVA: 0x0004FC90 File Offset: 0x0004DE90
	public void SetKeyCode<T>(KeyControl.Key idx, KeyCode value) where T : KeyControl
	{
		T t = (T)((object)null);
		for (int i = 0; i < this.input.Count; i++)
		{
			KeyControl keyControl = this.input[i];
			if (keyControl.GetType() == typeof(T))
			{
				t = (keyControl as T);
			}
		}
		if (t == null)
		{
			return;
		}
		t.SetKeyCode(idx, value);
	}

	// Token: 0x06000934 RID: 2356 RVA: 0x0004FD0C File Offset: 0x0004DF0C
	public KeyCode GetKeyCode<T>(KeyControl.Key controlKey) where T : KeyControl
	{
		T t = (T)((object)null);
		for (int i = 0; i < this.input.Count; i++)
		{
			KeyControl keyControl = this.input[i];
			if (keyControl.GetType() == typeof(T))
			{
				t = (keyControl as T);
			}
		}
		KeyCode[] keyCodeArray = t.GetKeyCodeArray();
		return keyCodeArray[(int)controlKey];
	}

	// Token: 0x06000935 RID: 2357 RVA: 0x0004FD80 File Offset: 0x0004DF80
	public KeyCode[] GetKeyCodeArray<T>() where T : KeyControl
	{
		T t = (T)((object)null);
		for (int i = 0; i < this.input.Count; i++)
		{
			KeyControl keyControl = this.input[i];
			if (keyControl.GetType() == typeof(T))
			{
				t = (keyControl as T);
			}
		}
		if (t == null)
		{
			return null;
		}
		return t.GetKeyCodeArray();
	}

	// Token: 0x06000936 RID: 2358 RVA: 0x0004FDF8 File Offset: 0x0004DFF8
	public void Push(IController ctrl)
	{
		GameDebugTool.Log("Push : " + ctrl.GetType());
		if (this.controller.Count != 0)
		{
			IController controller = this.controller.Peek();
			if (controller.GetType() == ctrl.GetType())
			{
				GameDebugTool.Log(controller.GetType().ToString() + " 重複加入!!");
				this.Pop();
			}
		}
		this.controller.Push(ctrl);
		this.OnMouseControl(Screen.showCursor);
	}

	// Token: 0x06000937 RID: 2359 RVA: 0x00007909 File Offset: 0x00005B09
	public void ControllerClear()
	{
		this.controller.Clear();
	}

	// Token: 0x06000938 RID: 2360 RVA: 0x00007916 File Offset: 0x00005B16
	public IController Pop()
	{
		if (this.controller.Count == 0)
		{
			return null;
		}
		Debug.Log("Pop : " + this.controller.Peek().GetType());
		return this.controller.Pop();
	}

	// Token: 0x06000939 RID: 2361 RVA: 0x00007954 File Offset: 0x00005B54
	public IController Peek()
	{
		return this.controller.Peek();
	}

	// Token: 0x0600093A RID: 2362 RVA: 0x0004FE80 File Offset: 0x0004E080
	public void Update()
	{
		if (this.input.Count == 0)
		{
			return;
		}
		float num = 0f;
		int num2 = 0;
		for (int i = 0; i < this.input.Count; i++)
		{
			this.input[i].Update();
			float sqrMagnitude = this.input[i].Direction.sqrMagnitude;
			if (sqrMagnitude > num)
			{
				num = sqrMagnitude;
				num2 = i;
			}
		}
		this.OnMove(this.input[num2].Direction);
		this.CheckMouse();
	}

	// Token: 0x0600093B RID: 2363 RVA: 0x00007961 File Offset: 0x00005B61
	private void CheckMouse()
	{
		if (Input.GetAxis("Mouse X") != 0f && !GameCursor.IsShow)
		{
			GameCursor.Instance.ShowCursor();
			this.OnMouseControl(GameCursor.IsShow);
		}
	}

	// Token: 0x0600093C RID: 2364 RVA: 0x00007996 File Offset: 0x00005B96
	private void OnKeyUp(KeyControl.Key key)
	{
		if (this.KeyUp != null)
		{
			this.KeyUp.Invoke(key);
		}
		if (this.controller.Count > 0)
		{
			this.controller.Peek().OnKeyUp(key);
		}
	}

	// Token: 0x0600093D RID: 2365 RVA: 0x0004FF14 File Offset: 0x0004E114
	private void OnKeyDown(KeyControl.Key key)
	{
		if (GameCursor.IsShow)
		{
			GameCursor.Instance.HideCursor();
			this.OnMouseControl(GameCursor.IsShow);
		}
		if (this.KeyDown != null)
		{
			this.KeyDown.Invoke(key);
		}
		if (this.controller.Count > 0)
		{
			this.controller.Peek().OnKeyDown(key);
		}
	}

	// Token: 0x0600093E RID: 2366 RVA: 0x000079D1 File Offset: 0x00005BD1
	private void OnKeyHeld(KeyControl.Key key)
	{
		if (this.KeyHeld != null)
		{
			this.KeyHeld.Invoke(key);
		}
		if (this.controller.Count > 0)
		{
			this.controller.Peek().OnKeyHeld(key);
		}
	}

	// Token: 0x0600093F RID: 2367 RVA: 0x00007A0C File Offset: 0x00005C0C
	private void OnMouseControl(bool bCtrl)
	{
		if (this.controller.Count > 0)
		{
			this.controller.Peek().OnMouseControl(bCtrl);
		}
	}

	// Token: 0x06000940 RID: 2368 RVA: 0x0004FF7C File Offset: 0x0004E17C
	private void OnMove(Vector2 direction)
	{
		if (this.Move != null)
		{
			this.Move.Invoke(direction);
		}
		if (this.controller.Count > 0)
		{
			this.controller.Peek().OnMove(direction);
		}
		this.prevDirection = direction;
	}

	// Token: 0x06000941 RID: 2369 RVA: 0x00004A78 File Offset: 0x00002C78
	private void OnRightStickMove(Vector2 direction)
	{
		throw new NotImplementedException();
	}

	// Token: 0x040008C2 RID: 2242
	private Stack<IController> controller = new Stack<IController>();

	// Token: 0x040008C3 RID: 2243
	private List<KeyControl> input = new List<KeyControl>();

	// Token: 0x040008C4 RID: 2244
	private Vector2 prevDirection;
}
