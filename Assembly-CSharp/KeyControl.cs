using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001B6 RID: 438
public class KeyControl
{
	// Token: 0x06000942 RID: 2370 RVA: 0x00002672 File Offset: 0x00000872
	public KeyControl()
	{
	}

	// Token: 0x06000943 RID: 2371 RVA: 0x0004FFCC File Offset: 0x0004E1CC
	public KeyControl(Dictionary<KeyControl.Key, KeyCode> mapping)
	{
		Array values = Enum.GetValues(typeof(KeyControl.Key));
		this.status = new bool[values.Length];
		this.key = new KeyCode[values.Length];
		foreach (KeyValuePair<KeyControl.Key, KeyCode> keyValuePair in mapping)
		{
			this.key[(int)keyValuePair.Key] = keyValuePair.Value;
		}
	}

	// Token: 0x06000944 RID: 2372 RVA: 0x00050068 File Offset: 0x0004E268
	public void SetMapping(Dictionary<KeyControl.Key, KeyCode> mapping)
	{
		Array values = Enum.GetValues(typeof(KeyControl.Key));
		this.status = new bool[values.Length];
		this.key = new KeyCode[values.Length];
		foreach (KeyValuePair<KeyControl.Key, KeyCode> keyValuePair in mapping)
		{
			this.key[(int)keyValuePair.Key] = keyValuePair.Value;
		}
	}

	// Token: 0x06000945 RID: 2373 RVA: 0x00050100 File Offset: 0x0004E300
	public virtual void Update()
	{
		for (int i = 0; i < this.status.Length; i++)
		{
			bool flag = Input.GetKey(this.key[i]);
			if (flag != this.status[i])
			{
				this.status[i] = flag;
				if (flag)
				{
					this.KeyDown.Invoke((KeyControl.Key)i);
				}
				else
				{
					this.KeyUp.Invoke((KeyControl.Key)i);
				}
			}
			else if (this.status[i])
			{
				this.KeyHeld.Invoke((KeyControl.Key)i);
			}
		}
	}

	// Token: 0x06000946 RID: 2374 RVA: 0x00007A30 File Offset: 0x00005C30
	public virtual void SetKeyCode(KeyControl.Key idx, KeyCode value)
	{
		this.key[(int)idx] = value;
	}

	// Token: 0x06000947 RID: 2375 RVA: 0x00007A3B File Offset: 0x00005C3B
	public KeyCode[] GetKeyCodeArray()
	{
		return this.key;
	}

	// Token: 0x040008CA RID: 2250
	public Action<KeyControl.Key> KeyDown;

	// Token: 0x040008CB RID: 2251
	public Action<KeyControl.Key> KeyUp;

	// Token: 0x040008CC RID: 2252
	public Action<KeyControl.Key> KeyHeld;

	// Token: 0x040008CD RID: 2253
	public Vector2 Direction;

	// Token: 0x040008CE RID: 2254
	protected Vector2 prevDirection;

	// Token: 0x040008CF RID: 2255
	protected KeyCode[] key;

	// Token: 0x040008D0 RID: 2256
	protected bool[] status;

	// Token: 0x020001B7 RID: 439
	public enum Key
	{
		// Token: 0x040008D2 RID: 2258
		Up,
		// Token: 0x040008D3 RID: 2259
		Down,
		// Token: 0x040008D4 RID: 2260
		Left,
		// Token: 0x040008D5 RID: 2261
		Right,
		// Token: 0x040008D6 RID: 2262
		OK,
		// Token: 0x040008D7 RID: 2263
		Cancel,
		// Token: 0x040008D8 RID: 2264
		X,
		// Token: 0x040008D9 RID: 2265
		Y,
		// Token: 0x040008DA RID: 2266
		L1,
		// Token: 0x040008DB RID: 2267
		R1,
		// Token: 0x040008DC RID: 2268
		L2,
		// Token: 0x040008DD RID: 2269
		R2,
		// Token: 0x040008DE RID: 2270
		L3,
		// Token: 0x040008DF RID: 2271
		R3,
		// Token: 0x040008E0 RID: 2272
		Select,
		// Token: 0x040008E1 RID: 2273
		Menu,
		// Token: 0x040008E2 RID: 2274
		Jump,
		// Token: 0x040008E3 RID: 2275
		ChangeModel,
		// Token: 0x040008E4 RID: 2276
		TalkLog,
		// Token: 0x040008E5 RID: 2277
		AddSpeed,
		// Token: 0x040008E6 RID: 2278
		ShowHideItemEft,
		// Token: 0x040008E7 RID: 2279
		ArrowUp,
		// Token: 0x040008E8 RID: 2280
		ArrowDown,
		// Token: 0x040008E9 RID: 2281
		ArrowLeft,
		// Token: 0x040008EA RID: 2282
		ArrowRight,
		// Token: 0x040008EB RID: 2283
		RotateLeft,
		// Token: 0x040008EC RID: 2284
		RotateRight,
		// Token: 0x040008ED RID: 2285
		BattlePrevUnit,
		// Token: 0x040008EE RID: 2286
		BattleNextUnit,
		// Token: 0x040008EF RID: 2287
		UnitInfo,
		// Token: 0x040008F0 RID: 2288
		FindTile,
		// Token: 0x040008F1 RID: 2289
		PlaceAllUnit,
		// Token: 0x040008F2 RID: 2290
		Character,
		// Token: 0x040008F3 RID: 2291
		Team,
		// Token: 0x040008F4 RID: 2292
		Backpack,
		// Token: 0x040008F5 RID: 2293
		Mission,
		// Token: 0x040008F6 RID: 2294
		Rumor,
		// Token: 0x040008F7 RID: 2295
		Save,
		// Token: 0x040008F8 RID: 2296
		Load,
		// Token: 0x040008F9 RID: 2297
		Skill1,
		// Token: 0x040008FA RID: 2298
		Skill2,
		// Token: 0x040008FB RID: 2299
		Skill3,
		// Token: 0x040008FC RID: 2300
		Skill4,
		// Token: 0x040008FD RID: 2301
		Skill5,
		// Token: 0x040008FE RID: 2302
		Skill6,
		// Token: 0x040008FF RID: 2303
		SelectItem,
		// Token: 0x04000900 RID: 2304
		RestTurn
	}
}
