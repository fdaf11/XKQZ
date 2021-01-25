using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x0200088F RID: 2191
public class iTweenEvent : MonoBehaviour
{
	// Token: 0x06003591 RID: 13713 RVA: 0x001A07F0 File Offset: 0x0019E9F0
	public static iTweenEvent GetEvent(GameObject obj, string name)
	{
		iTweenEvent[] components = obj.GetComponents<iTweenEvent>();
		if (components.Length > 0)
		{
			iTweenEvent iTweenEvent = Enumerable.FirstOrDefault<iTweenEvent>(components, (iTweenEvent tween) => tween.tweenName == name);
			if (iTweenEvent != null)
			{
				return iTweenEvent;
			}
		}
		throw new ArgumentException(string.Concat(new string[]
		{
			"No tween with the name '",
			name,
			"' could be found on the GameObject named '",
			obj.name,
			"'"
		}));
	}

	// Token: 0x17000496 RID: 1174
	// (get) Token: 0x06003592 RID: 13714 RVA: 0x00021AE9 File Offset: 0x0001FCE9
	// (set) Token: 0x06003593 RID: 13715 RVA: 0x00021B02 File Offset: 0x0001FD02
	public Dictionary<string, object> Values
	{
		get
		{
			if (this.values == null)
			{
				this.DeserializeValues();
			}
			return this.values;
		}
		set
		{
			this.values = value;
			this.SerializeValues();
		}
	}

	// Token: 0x06003594 RID: 13716 RVA: 0x00021B11 File Offset: 0x0001FD11
	public void Start()
	{
		if (this.playAutomatically)
		{
			this.Play();
		}
	}

	// Token: 0x06003595 RID: 13717 RVA: 0x00021B24 File Offset: 0x0001FD24
	public void Play()
	{
		if (!string.IsNullOrEmpty(this.internalName))
		{
			this.Stop();
		}
		this.stopped = false;
		base.StartCoroutine(this.StartEvent());
	}

	// Token: 0x06003596 RID: 13718 RVA: 0x00021B50 File Offset: 0x0001FD50
	public void Stop()
	{
		iTween.StopByName(base.gameObject, this.internalName);
		this.internalName = string.Empty;
		this.stopped = true;
	}

	// Token: 0x06003597 RID: 13719 RVA: 0x00021B75 File Offset: 0x0001FD75
	public void OnDrawGizmos()
	{
		if (this.showIconInInspector)
		{
			Gizmos.DrawIcon(base.transform.position, "iTweenIcon.tif");
		}
	}

	// Token: 0x06003598 RID: 13720 RVA: 0x001A0878 File Offset: 0x0019EA78
	private IEnumerator StartEvent()
	{
		if (this.delay > 0f)
		{
			yield return new WaitForSeconds(this.delay);
		}
		if (this.stopped)
		{
			yield return null;
		}
		Hashtable optionsHash = new Hashtable();
		foreach (KeyValuePair<string, object> pair in this.Values)
		{
			if ("path" == pair.Key && pair.Value.GetType() == typeof(string))
			{
				optionsHash.Add(pair.Key, iTweenPath.GetPath((string)pair.Value));
			}
			else
			{
				optionsHash.Add(pair.Key, pair.Value);
			}
		}
		this.internalName = ((!string.IsNullOrEmpty(this.tweenName)) ? this.tweenName : string.Empty);
		this.internalName = string.Format("{0}-{1}", this.internalName, Guid.NewGuid().ToString());
		optionsHash.Add("name", this.internalName);
		switch (this.type)
		{
		case iTweenEvent.TweenType.AudioFrom:
			iTween.AudioFrom(base.gameObject, optionsHash);
			break;
		case iTweenEvent.TweenType.AudioTo:
			iTween.AudioTo(base.gameObject, optionsHash);
			break;
		case iTweenEvent.TweenType.AudioUpdate:
			iTween.AudioUpdate(base.gameObject, optionsHash);
			break;
		case iTweenEvent.TweenType.CameraFadeFrom:
			iTween.CameraFadeFrom(optionsHash);
			break;
		case iTweenEvent.TweenType.CameraFadeTo:
			iTween.CameraFadeTo(optionsHash);
			break;
		case iTweenEvent.TweenType.ColorFrom:
			iTween.ColorFrom(base.gameObject, optionsHash);
			break;
		case iTweenEvent.TweenType.ColorTo:
			iTween.ColorTo(base.gameObject, optionsHash);
			break;
		case iTweenEvent.TweenType.ColorUpdate:
			iTween.ColorUpdate(base.gameObject, optionsHash);
			break;
		case iTweenEvent.TweenType.FadeFrom:
			iTween.FadeFrom(base.gameObject, optionsHash);
			break;
		case iTweenEvent.TweenType.FadeTo:
			iTween.FadeTo(base.gameObject, optionsHash);
			break;
		case iTweenEvent.TweenType.FadeUpdate:
			iTween.FadeUpdate(base.gameObject, optionsHash);
			break;
		case iTweenEvent.TweenType.LookFrom:
			iTween.LookFrom(base.gameObject, optionsHash);
			break;
		case iTweenEvent.TweenType.LookTo:
			iTween.LookTo(base.gameObject, optionsHash);
			break;
		case iTweenEvent.TweenType.LookUpdate:
			iTween.LookUpdate(base.gameObject, optionsHash);
			break;
		case iTweenEvent.TweenType.MoveAdd:
			iTween.MoveAdd(base.gameObject, optionsHash);
			break;
		case iTweenEvent.TweenType.MoveBy:
			iTween.MoveBy(base.gameObject, optionsHash);
			break;
		case iTweenEvent.TweenType.MoveFrom:
			iTween.MoveFrom(base.gameObject, optionsHash);
			break;
		case iTweenEvent.TweenType.MoveTo:
			iTween.MoveTo(base.gameObject, optionsHash);
			break;
		case iTweenEvent.TweenType.MoveUpdate:
			iTween.MoveUpdate(base.gameObject, optionsHash);
			break;
		case iTweenEvent.TweenType.PunchPosition:
			iTween.PunchPosition(base.gameObject, optionsHash);
			break;
		case iTweenEvent.TweenType.PunchRotation:
			iTween.PunchRotation(base.gameObject, optionsHash);
			break;
		case iTweenEvent.TweenType.PunchScale:
			iTween.PunchScale(base.gameObject, optionsHash);
			break;
		case iTweenEvent.TweenType.RotateAdd:
			iTween.RotateAdd(base.gameObject, optionsHash);
			break;
		case iTweenEvent.TweenType.RotateBy:
			iTween.RotateBy(base.gameObject, optionsHash);
			break;
		case iTweenEvent.TweenType.RotateFrom:
			iTween.RotateFrom(base.gameObject, optionsHash);
			break;
		case iTweenEvent.TweenType.RotateTo:
			iTween.RotateTo(base.gameObject, optionsHash);
			break;
		case iTweenEvent.TweenType.RotateUpdate:
			iTween.RotateUpdate(base.gameObject, optionsHash);
			break;
		case iTweenEvent.TweenType.ScaleAdd:
			iTween.ScaleAdd(base.gameObject, optionsHash);
			break;
		case iTweenEvent.TweenType.ScaleBy:
			iTween.ScaleBy(base.gameObject, optionsHash);
			break;
		case iTweenEvent.TweenType.ScaleFrom:
			iTween.ScaleFrom(base.gameObject, optionsHash);
			break;
		case iTweenEvent.TweenType.ScaleTo:
			iTween.ScaleTo(base.gameObject, optionsHash);
			break;
		case iTweenEvent.TweenType.ScaleUpdate:
			iTween.ScaleUpdate(base.gameObject, optionsHash);
			break;
		case iTweenEvent.TweenType.ShakePosition:
			iTween.ShakePosition(base.gameObject, optionsHash);
			break;
		case iTweenEvent.TweenType.ShakeRotation:
			iTween.ShakeRotation(base.gameObject, optionsHash);
			break;
		case iTweenEvent.TweenType.ShakeScale:
			iTween.ShakeScale(base.gameObject, optionsHash);
			break;
		case iTweenEvent.TweenType.Stab:
			iTween.Stab(base.gameObject, optionsHash);
			break;
		default:
			throw new ArgumentException("Invalid tween type: " + this.type);
		}
		yield break;
	}

	// Token: 0x06003599 RID: 13721 RVA: 0x001A0894 File Offset: 0x0019EA94
	private void SerializeValues()
	{
		List<string> list = new List<string>();
		List<int> list2 = new List<int>();
		List<string> list3 = new List<string>();
		List<int> list4 = new List<int>();
		List<float> list5 = new List<float>();
		List<bool> list6 = new List<bool>();
		List<string> list7 = new List<string>();
		List<Vector3> list8 = new List<Vector3>();
		List<Color> list9 = new List<Color>();
		List<Space> list10 = new List<Space>();
		List<iTween.EaseType> list11 = new List<iTween.EaseType>();
		List<iTween.LoopType> list12 = new List<iTween.LoopType>();
		List<GameObject> list13 = new List<GameObject>();
		List<Transform> list14 = new List<Transform>();
		List<AudioClip> list15 = new List<AudioClip>();
		List<AudioSource> list16 = new List<AudioSource>();
		List<ArrayIndexes> list17 = new List<ArrayIndexes>();
		List<ArrayIndexes> list18 = new List<ArrayIndexes>();
		foreach (KeyValuePair<string, object> pair in this.values)
		{
			Dictionary<string, Type> dictionary = EventParamMappings.mappings[this.type];
			Type type = dictionary[pair.Key];
			if (typeof(int) == type)
			{
				this.AddToList<int>(list, list2, list4, list3, pair);
			}
			if (typeof(float) == type)
			{
				this.AddToList<float>(list, list2, list5, list3, pair);
			}
			else if (typeof(bool) == type)
			{
				this.AddToList<bool>(list, list2, list6, list3, pair);
			}
			else if (typeof(string) == type)
			{
				this.AddToList<string>(list, list2, list7, list3, pair);
			}
			else if (typeof(Vector3) == type)
			{
				this.AddToList<Vector3>(list, list2, list8, list3, pair);
			}
			else if (typeof(Color) == type)
			{
				this.AddToList<Color>(list, list2, list9, list3, pair);
			}
			else if (typeof(Space) == type)
			{
				this.AddToList<Space>(list, list2, list10, list3, pair);
			}
			else if (typeof(iTween.EaseType) == type)
			{
				this.AddToList<iTween.EaseType>(list, list2, list11, list3, pair);
			}
			else if (typeof(iTween.LoopType) == type)
			{
				this.AddToList<iTween.LoopType>(list, list2, list12, list3, pair);
			}
			else if (typeof(GameObject) == type)
			{
				this.AddToList<GameObject>(list, list2, list13, list3, pair);
			}
			else if (typeof(Transform) == type)
			{
				this.AddToList<Transform>(list, list2, list14, list3, pair);
			}
			else if (typeof(AudioClip) == type)
			{
				this.AddToList<AudioClip>(list, list2, list15, list3, pair);
			}
			else if (typeof(AudioSource) == type)
			{
				this.AddToList<AudioSource>(list, list2, list16, list3, pair);
			}
			else if (typeof(Vector3OrTransform) == type)
			{
				if (pair.Value == null || typeof(Transform) == pair.Value.GetType())
				{
					this.AddToList<Transform>(list, list2, list14, list3, pair.Key, pair.Value, "t");
				}
				else
				{
					this.AddToList<Vector3>(list, list2, list8, list3, pair.Key, pair.Value, "v");
				}
			}
			else if (typeof(Vector3OrTransformArray) == type)
			{
				if (typeof(Vector3[]) == pair.Value.GetType())
				{
					Vector3[] array = (Vector3[])pair.Value;
					ArrayIndexes arrayIndexes = new ArrayIndexes();
					int[] array2 = new int[array.Length];
					for (int i = 0; i < array.Length; i++)
					{
						list8.Add(array[i]);
						array2[i] = list8.Count - 1;
					}
					arrayIndexes.indexes = array2;
					this.AddToList<ArrayIndexes>(list, list2, list17, list3, pair.Key, arrayIndexes, "v");
				}
				else if (typeof(Transform[]) == pair.Value.GetType())
				{
					Transform[] array3 = (Transform[])pair.Value;
					ArrayIndexes arrayIndexes2 = new ArrayIndexes();
					int[] array4 = new int[array3.Length];
					for (int j = 0; j < array3.Length; j++)
					{
						list14.Add(array3[j]);
						array4[j] = list14.Count - 1;
					}
					arrayIndexes2.indexes = array4;
					this.AddToList<ArrayIndexes>(list, list2, list18, list3, pair.Key, arrayIndexes2, "t");
				}
				else if (typeof(string) == pair.Value.GetType())
				{
					this.AddToList<string>(list, list2, list7, list3, pair.Key, pair.Value, "p");
				}
			}
		}
		this.keys = list.ToArray();
		this.indexes = list2.ToArray();
		this.metadatas = list3.ToArray();
		this.ints = list4.ToArray();
		this.floats = list5.ToArray();
		this.bools = list6.ToArray();
		this.strings = list7.ToArray();
		this.vector3s = list8.ToArray();
		this.colors = list9.ToArray();
		this.spaces = list10.ToArray();
		this.easeTypes = list11.ToArray();
		this.loopTypes = list12.ToArray();
		this.gameObjects = list13.ToArray();
		this.transforms = list14.ToArray();
		this.audioClips = list15.ToArray();
		this.audioSources = list16.ToArray();
		this.vector3Arrays = list17.ToArray();
		this.transformArrays = list18.ToArray();
	}

	// Token: 0x0600359A RID: 13722 RVA: 0x00021B97 File Offset: 0x0001FD97
	private void AddToList<T>(List<string> keyList, List<int> indexList, IList<T> valueList, List<string> metadataList, KeyValuePair<string, object> pair)
	{
		this.AddToList<T>(keyList, indexList, valueList, metadataList, pair.Key, pair.Value);
	}

	// Token: 0x0600359B RID: 13723 RVA: 0x00021BB2 File Offset: 0x0001FDB2
	private void AddToList<T>(List<string> keyList, List<int> indexList, IList<T> valueList, List<string> metadataList, KeyValuePair<string, object> pair, string metadata)
	{
		this.AddToList<T>(keyList, indexList, valueList, metadataList, pair.Key, pair.Value, metadata);
	}

	// Token: 0x0600359C RID: 13724 RVA: 0x00021BCF File Offset: 0x0001FDCF
	private void AddToList<T>(List<string> keyList, List<int> indexList, IList<T> valueList, List<string> metadataList, string key, object value)
	{
		this.AddToList<T>(keyList, indexList, valueList, metadataList, key, value, null);
	}

	// Token: 0x0600359D RID: 13725 RVA: 0x00021BE1 File Offset: 0x0001FDE1
	private void AddToList<T>(List<string> keyList, List<int> indexList, IList<T> valueList, List<string> metadataList, string key, object value, string metadata)
	{
		keyList.Add(key);
		valueList.Add((T)((object)value));
		indexList.Add(valueList.Count - 1);
		metadataList.Add(metadata);
	}

	// Token: 0x0600359E RID: 13726 RVA: 0x001A0E34 File Offset: 0x0019F034
	private void DeserializeValues()
	{
		this.values = new Dictionary<string, object>();
		if (this.keys == null)
		{
			return;
		}
		for (int i = 0; i < this.keys.Length; i++)
		{
			Dictionary<string, Type> dictionary = EventParamMappings.mappings[this.type];
			Type type = dictionary[this.keys[i]];
			if (typeof(int) == type)
			{
				this.values.Add(this.keys[i], this.ints[this.indexes[i]]);
			}
			else if (typeof(float) == type)
			{
				this.values.Add(this.keys[i], this.floats[this.indexes[i]]);
			}
			else if (typeof(bool) == type)
			{
				this.values.Add(this.keys[i], this.bools[this.indexes[i]]);
			}
			else if (typeof(string) == type)
			{
				this.values.Add(this.keys[i], this.strings[this.indexes[i]]);
			}
			else if (typeof(Vector3) == type)
			{
				this.values.Add(this.keys[i], this.vector3s[this.indexes[i]]);
			}
			else if (typeof(Color) == type)
			{
				this.values.Add(this.keys[i], this.colors[this.indexes[i]]);
			}
			else if (typeof(Space) == type)
			{
				this.values.Add(this.keys[i], this.spaces[this.indexes[i]]);
			}
			else if (typeof(iTween.EaseType) == type)
			{
				this.values.Add(this.keys[i], this.easeTypes[this.indexes[i]]);
			}
			else if (typeof(iTween.LoopType) == type)
			{
				this.values.Add(this.keys[i], this.loopTypes[this.indexes[i]]);
			}
			else if (typeof(GameObject) == type)
			{
				this.values.Add(this.keys[i], this.gameObjects[this.indexes[i]]);
			}
			else if (typeof(Transform) == type)
			{
				this.values.Add(this.keys[i], this.transforms[this.indexes[i]]);
			}
			else if (typeof(AudioClip) == type)
			{
				this.values.Add(this.keys[i], this.audioClips[this.indexes[i]]);
			}
			else if (typeof(AudioSource) == type)
			{
				this.values.Add(this.keys[i], this.audioSources[this.indexes[i]]);
			}
			else if (typeof(Vector3OrTransform) == type)
			{
				if ("v" == this.metadatas[i])
				{
					this.values.Add(this.keys[i], this.vector3s[this.indexes[i]]);
				}
				else if ("t" == this.metadatas[i])
				{
					this.values.Add(this.keys[i], this.transforms[this.indexes[i]]);
				}
			}
			else if (typeof(Vector3OrTransformArray) == type)
			{
				if ("v" == this.metadatas[i])
				{
					ArrayIndexes arrayIndexes = this.vector3Arrays[this.indexes[i]];
					Vector3[] array = new Vector3[arrayIndexes.indexes.Length];
					for (int j = 0; j < arrayIndexes.indexes.Length; j++)
					{
						array[j] = this.vector3s[arrayIndexes.indexes[j]];
					}
					this.values.Add(this.keys[i], array);
				}
				else if ("t" == this.metadatas[i])
				{
					ArrayIndexes arrayIndexes2 = this.transformArrays[this.indexes[i]];
					Transform[] array2 = new Transform[arrayIndexes2.indexes.Length];
					for (int k = 0; k < arrayIndexes2.indexes.Length; k++)
					{
						array2[k] = this.transforms[arrayIndexes2.indexes[k]];
					}
					this.values.Add(this.keys[i], array2);
				}
				else if ("p" == this.metadatas[i])
				{
					this.values.Add(this.keys[i], this.strings[this.indexes[i]]);
				}
			}
		}
	}

	// Token: 0x040040D4 RID: 16596
	public const string VERSION = "0.6.1";

	// Token: 0x040040D5 RID: 16597
	public string tweenName = string.Empty;

	// Token: 0x040040D6 RID: 16598
	public bool playAutomatically = true;

	// Token: 0x040040D7 RID: 16599
	public float delay;

	// Token: 0x040040D8 RID: 16600
	public iTweenEvent.TweenType type = iTweenEvent.TweenType.MoveTo;

	// Token: 0x040040D9 RID: 16601
	public bool showIconInInspector = true;

	// Token: 0x040040DA RID: 16602
	[SerializeField]
	private string[] keys;

	// Token: 0x040040DB RID: 16603
	[SerializeField]
	private int[] indexes;

	// Token: 0x040040DC RID: 16604
	[SerializeField]
	private string[] metadatas;

	// Token: 0x040040DD RID: 16605
	[SerializeField]
	private int[] ints;

	// Token: 0x040040DE RID: 16606
	[SerializeField]
	private float[] floats;

	// Token: 0x040040DF RID: 16607
	[SerializeField]
	private bool[] bools;

	// Token: 0x040040E0 RID: 16608
	[SerializeField]
	private string[] strings;

	// Token: 0x040040E1 RID: 16609
	[SerializeField]
	private Vector3[] vector3s;

	// Token: 0x040040E2 RID: 16610
	[SerializeField]
	private Color[] colors;

	// Token: 0x040040E3 RID: 16611
	[SerializeField]
	private Space[] spaces;

	// Token: 0x040040E4 RID: 16612
	[SerializeField]
	private iTween.EaseType[] easeTypes;

	// Token: 0x040040E5 RID: 16613
	[SerializeField]
	private iTween.LoopType[] loopTypes;

	// Token: 0x040040E6 RID: 16614
	[SerializeField]
	private GameObject[] gameObjects;

	// Token: 0x040040E7 RID: 16615
	[SerializeField]
	private Transform[] transforms;

	// Token: 0x040040E8 RID: 16616
	[SerializeField]
	private AudioClip[] audioClips;

	// Token: 0x040040E9 RID: 16617
	[SerializeField]
	private AudioSource[] audioSources;

	// Token: 0x040040EA RID: 16618
	[SerializeField]
	private ArrayIndexes[] vector3Arrays;

	// Token: 0x040040EB RID: 16619
	[SerializeField]
	private ArrayIndexes[] transformArrays;

	// Token: 0x040040EC RID: 16620
	[SerializeField]
	private iTweenPath[] paths;

	// Token: 0x040040ED RID: 16621
	private Dictionary<string, object> values;

	// Token: 0x040040EE RID: 16622
	private bool stopped;

	// Token: 0x040040EF RID: 16623
	private iTween instantiatedTween;

	// Token: 0x040040F0 RID: 16624
	private string internalName;

	// Token: 0x02000890 RID: 2192
	public enum TweenType
	{
		// Token: 0x040040F2 RID: 16626
		AudioFrom,
		// Token: 0x040040F3 RID: 16627
		AudioTo,
		// Token: 0x040040F4 RID: 16628
		AudioUpdate,
		// Token: 0x040040F5 RID: 16629
		CameraFadeFrom,
		// Token: 0x040040F6 RID: 16630
		CameraFadeTo,
		// Token: 0x040040F7 RID: 16631
		ColorFrom,
		// Token: 0x040040F8 RID: 16632
		ColorTo,
		// Token: 0x040040F9 RID: 16633
		ColorUpdate,
		// Token: 0x040040FA RID: 16634
		FadeFrom,
		// Token: 0x040040FB RID: 16635
		FadeTo,
		// Token: 0x040040FC RID: 16636
		FadeUpdate,
		// Token: 0x040040FD RID: 16637
		LookFrom,
		// Token: 0x040040FE RID: 16638
		LookTo,
		// Token: 0x040040FF RID: 16639
		LookUpdate,
		// Token: 0x04004100 RID: 16640
		MoveAdd,
		// Token: 0x04004101 RID: 16641
		MoveBy,
		// Token: 0x04004102 RID: 16642
		MoveFrom,
		// Token: 0x04004103 RID: 16643
		MoveTo,
		// Token: 0x04004104 RID: 16644
		MoveUpdate,
		// Token: 0x04004105 RID: 16645
		PunchPosition,
		// Token: 0x04004106 RID: 16646
		PunchRotation,
		// Token: 0x04004107 RID: 16647
		PunchScale,
		// Token: 0x04004108 RID: 16648
		RotateAdd,
		// Token: 0x04004109 RID: 16649
		RotateBy,
		// Token: 0x0400410A RID: 16650
		RotateFrom,
		// Token: 0x0400410B RID: 16651
		RotateTo,
		// Token: 0x0400410C RID: 16652
		RotateUpdate,
		// Token: 0x0400410D RID: 16653
		ScaleAdd,
		// Token: 0x0400410E RID: 16654
		ScaleBy,
		// Token: 0x0400410F RID: 16655
		ScaleFrom,
		// Token: 0x04004110 RID: 16656
		ScaleTo,
		// Token: 0x04004111 RID: 16657
		ScaleUpdate,
		// Token: 0x04004112 RID: 16658
		ShakePosition,
		// Token: 0x04004113 RID: 16659
		ShakeRotation,
		// Token: 0x04004114 RID: 16660
		ShakeScale,
		// Token: 0x04004115 RID: 16661
		Stab
	}
}
