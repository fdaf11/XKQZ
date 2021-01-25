using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200049C RID: 1180
public static class Localization
{
	// Token: 0x1700024C RID: 588
	// (get) Token: 0x06001C91 RID: 7313 RVA: 0x0001300A File Offset: 0x0001120A
	// (set) Token: 0x06001C92 RID: 7314 RVA: 0x0001302F File Offset: 0x0001122F
	public static Dictionary<string, string[]> dictionary
	{
		get
		{
			if (!Localization.localizationHasBeenSet)
			{
				Localization.language = PlayerPrefs.GetString("Language", "English");
			}
			return Localization.mDictionary;
		}
		set
		{
			Localization.localizationHasBeenSet = (value != null);
			Localization.mDictionary = value;
		}
	}

	// Token: 0x1700024D RID: 589
	// (get) Token: 0x06001C93 RID: 7315 RVA: 0x00013043 File Offset: 0x00011243
	public static string[] knownLanguages
	{
		get
		{
			if (!Localization.localizationHasBeenSet)
			{
				Localization.LoadDictionary(PlayerPrefs.GetString("Language", "English"));
			}
			return Localization.mLanguages;
		}
	}

	// Token: 0x1700024E RID: 590
	// (get) Token: 0x06001C94 RID: 7316 RVA: 0x000DDD64 File Offset: 0x000DBF64
	// (set) Token: 0x06001C95 RID: 7317 RVA: 0x00013069 File Offset: 0x00011269
	public static string language
	{
		get
		{
			if (string.IsNullOrEmpty(Localization.mLanguage))
			{
				string[] knownLanguages = Localization.knownLanguages;
				Localization.mLanguage = PlayerPrefs.GetString("Language", (knownLanguages == null) ? "English" : knownLanguages[0]);
				Localization.LoadAndSelect(Localization.mLanguage);
			}
			return Localization.mLanguage;
		}
		set
		{
			if (Localization.mLanguage != value)
			{
				Localization.mLanguage = value;
				Localization.LoadAndSelect(value);
			}
		}
	}

	// Token: 0x06001C96 RID: 7318 RVA: 0x000DDDB8 File Offset: 0x000DBFB8
	private static bool LoadDictionary(string value)
	{
		byte[] array = null;
		if (!Localization.localizationHasBeenSet)
		{
			if (Localization.loadFunction == null)
			{
				TextAsset textAsset = Resources.Load<TextAsset>("Localization");
				if (textAsset != null)
				{
					array = textAsset.bytes;
				}
			}
			else
			{
				array = Localization.loadFunction("Localization");
			}
			Localization.localizationHasBeenSet = true;
		}
		if (Localization.LoadCSV(array))
		{
			return true;
		}
		if (string.IsNullOrEmpty(value))
		{
			return false;
		}
		if (Localization.loadFunction == null)
		{
			TextAsset textAsset2 = Resources.Load<TextAsset>(value);
			if (textAsset2 != null)
			{
				array = textAsset2.bytes;
			}
		}
		else
		{
			array = Localization.loadFunction(value);
		}
		if (array != null)
		{
			Localization.Set(value, array);
			return true;
		}
		return false;
	}

	// Token: 0x06001C97 RID: 7319 RVA: 0x000DDE74 File Offset: 0x000DC074
	private static bool LoadAndSelect(string value)
	{
		if (!string.IsNullOrEmpty(value))
		{
			if (Localization.mDictionary.Count == 0 && !Localization.LoadDictionary(value))
			{
				return false;
			}
			if (Localization.SelectLanguage(value))
			{
				return true;
			}
		}
		if (Localization.mOldDictionary.Count > 0)
		{
			return true;
		}
		Localization.mOldDictionary.Clear();
		Localization.mDictionary.Clear();
		if (string.IsNullOrEmpty(value))
		{
			PlayerPrefs.DeleteKey("Language");
		}
		return false;
	}

	// Token: 0x06001C98 RID: 7320 RVA: 0x000DDEF4 File Offset: 0x000DC0F4
	public static void Load(TextAsset asset)
	{
		ByteReader byteReader = new ByteReader(asset);
		Localization.Set(asset.name, byteReader.ReadDictionary());
	}

	// Token: 0x06001C99 RID: 7321 RVA: 0x000DDF1C File Offset: 0x000DC11C
	public static void Set(string languageName, byte[] bytes)
	{
		ByteReader byteReader = new ByteReader(bytes);
		Localization.Set(languageName, byteReader.ReadDictionary());
	}

	// Token: 0x06001C9A RID: 7322 RVA: 0x00013088 File Offset: 0x00011288
	public static bool LoadCSV(TextAsset asset)
	{
		return Localization.LoadCSV(asset.bytes, asset);
	}

	// Token: 0x06001C9B RID: 7323 RVA: 0x00013096 File Offset: 0x00011296
	public static bool LoadCSV(byte[] bytes)
	{
		return Localization.LoadCSV(bytes, null);
	}

	// Token: 0x06001C9C RID: 7324 RVA: 0x000DDF3C File Offset: 0x000DC13C
	private static bool LoadCSV(byte[] bytes, TextAsset asset)
	{
		if (bytes == null)
		{
			return false;
		}
		ByteReader byteReader = new ByteReader(bytes);
		BetterList<string> betterList = byteReader.ReadCSV();
		if (betterList.size < 2)
		{
			return false;
		}
		betterList[0] = "KEY";
		if (!string.Equals(betterList[0], "KEY"))
		{
			Debug.LogError("Invalid localization CSV file. The first value is expected to be 'KEY', followed by language columns.\nInstead found '" + betterList[0] + "'", asset);
			return false;
		}
		Localization.mLanguages = new string[betterList.size - 1];
		for (int i = 0; i < Localization.mLanguages.Length; i++)
		{
			Localization.mLanguages[i] = betterList[i + 1];
		}
		Localization.mDictionary.Clear();
		while (betterList != null)
		{
			Localization.AddCSV(betterList);
			betterList = byteReader.ReadCSV();
		}
		return true;
	}

	// Token: 0x06001C9D RID: 7325 RVA: 0x000DE00C File Offset: 0x000DC20C
	private static bool SelectLanguage(string language)
	{
		Localization.mLanguageIndex = -1;
		if (Localization.mDictionary.Count == 0)
		{
			return false;
		}
		string[] array;
		if (Localization.mDictionary.TryGetValue("KEY", ref array))
		{
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == language)
				{
					Localization.mOldDictionary.Clear();
					Localization.mLanguageIndex = i;
					Localization.mLanguage = language;
					PlayerPrefs.SetString("Language", Localization.mLanguage);
					UIRoot.Broadcast("OnLocalize");
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06001C9E RID: 7326 RVA: 0x000DE09C File Offset: 0x000DC29C
	private static void AddCSV(BetterList<string> values)
	{
		if (values.size < 2)
		{
			return;
		}
		string[] array = new string[values.size - 1];
		for (int i = 1; i < values.size; i++)
		{
			array[i - 1] = values[i];
		}
		try
		{
			Localization.mDictionary.Add(values[0], array);
		}
		catch (Exception ex)
		{
			Debug.LogError("Unable to add '" + values[0] + "' to the Localization dictionary.\n" + ex.Message);
		}
	}

	// Token: 0x06001C9F RID: 7327 RVA: 0x000DE138 File Offset: 0x000DC338
	public static void Set(string languageName, Dictionary<string, string> dictionary)
	{
		Localization.mLanguage = languageName;
		PlayerPrefs.SetString("Language", Localization.mLanguage);
		Localization.mOldDictionary = dictionary;
		Localization.localizationHasBeenSet = false;
		Localization.mLanguageIndex = -1;
		Localization.mLanguages = new string[]
		{
			languageName
		};
		UIRoot.Broadcast("OnLocalize");
	}

	// Token: 0x06001CA0 RID: 7328 RVA: 0x000DE188 File Offset: 0x000DC388
	public static string Get(string key)
	{
		if (!Localization.localizationHasBeenSet)
		{
			Localization.language = PlayerPrefs.GetString("Language", "English");
		}
		string[] array;
		string result;
		if (Localization.mLanguageIndex != -1 && Localization.mDictionary.TryGetValue(key, ref array))
		{
			if (Localization.mLanguageIndex < array.Length)
			{
				return array[Localization.mLanguageIndex];
			}
		}
		else if (Localization.mOldDictionary.TryGetValue(key, ref result))
		{
			return result;
		}
		return key;
	}

	// Token: 0x06001CA1 RID: 7329 RVA: 0x0001309F File Offset: 0x0001129F
	public static string Format(string key, params object[] parameters)
	{
		return string.Format(Localization.Get(key), parameters);
	}

	// Token: 0x1700024F RID: 591
	// (get) Token: 0x06001CA2 RID: 7330 RVA: 0x00002B59 File Offset: 0x00000D59
	[Obsolete("Localization is now always active. You no longer need to check this property.")]
	public static bool isActive
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001CA3 RID: 7331 RVA: 0x000130AD File Offset: 0x000112AD
	[Obsolete("Use Localization.Get instead")]
	public static string Localize(string key)
	{
		return Localization.Get(key);
	}

	// Token: 0x06001CA4 RID: 7332 RVA: 0x000130B5 File Offset: 0x000112B5
	public static bool Exists(string key)
	{
		if (!Localization.localizationHasBeenSet)
		{
			Localization.language = PlayerPrefs.GetString("Language", "English");
		}
		return Localization.mDictionary.ContainsKey(key) || Localization.mOldDictionary.ContainsKey(key);
	}

	// Token: 0x04002150 RID: 8528
	public static Localization.LoadFunction loadFunction;

	// Token: 0x04002151 RID: 8529
	public static bool localizationHasBeenSet = false;

	// Token: 0x04002152 RID: 8530
	private static string[] mLanguages = null;

	// Token: 0x04002153 RID: 8531
	private static Dictionary<string, string> mOldDictionary = new Dictionary<string, string>();

	// Token: 0x04002154 RID: 8532
	private static Dictionary<string, string[]> mDictionary = new Dictionary<string, string[]>();

	// Token: 0x04002155 RID: 8533
	private static int mLanguageIndex = -1;

	// Token: 0x04002156 RID: 8534
	private static string mLanguage;

	// Token: 0x0200049D RID: 1181
	// (Invoke) Token: 0x06001CA6 RID: 7334
	public delegate byte[] LoadFunction(string path);
}
