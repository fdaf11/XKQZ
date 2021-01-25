using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x0200008D RID: 141
public class ModelAssetBundle
{
	// Token: 0x06000325 RID: 805 RVA: 0x0002B63C File Offset: 0x0002983C
	public void CreateForPath(string Dir)
	{
		if (!Directory.Exists(Dir))
		{
			return;
		}
		this.strDirectory = Dir;
		string[] files = Directory.GetFiles(Dir, "*.pk");
		this.strModelNameList.AddRange(files);
		this.strModelNameList.Sort(new Comparison<string>(ModelAssetBundle.StringCompareFunc));
		foreach (string text in this.strModelNameList)
		{
			AssetBundle assetBundle = AssetBundle.CreateFromFile(text);
			if (assetBundle != null)
			{
				this.modelABList.Add(assetBundle);
			}
		}
	}

	// Token: 0x06000326 RID: 806 RVA: 0x00004422 File Offset: 0x00002622
	private static int StringCompareFunc(string S1, string S2)
	{
		if (S1.CompareTo(S2) > 0)
		{
			return -1;
		}
		return 1;
	}

	// Token: 0x06000327 RID: 807 RVA: 0x0002B6F0 File Offset: 0x000298F0
	public Object Load(string fileName)
	{
		if (fileName == null)
		{
			return null;
		}
		if (fileName == string.Empty)
		{
			return null;
		}
		Object @object = null;
		foreach (AssetBundle assetBundle in this.modelABList)
		{
			if (assetBundle.Contains(fileName))
			{
				@object = assetBundle.Load(fileName);
				if (@object != null)
				{
					return @object;
				}
			}
		}
		return @object;
	}

	// Token: 0x06000328 RID: 808 RVA: 0x0002B790 File Offset: 0x00029990
	public AssetBundle GetAssetBundle(string fileName)
	{
		foreach (AssetBundle assetBundle in this.modelABList)
		{
			if (assetBundle.Contains(fileName))
			{
				return assetBundle;
			}
		}
		return null;
	}

	// Token: 0x06000329 RID: 809 RVA: 0x00004434 File Offset: 0x00002634
	public int GetAssetBundleCount()
	{
		return this.modelABList.Count;
	}

	// Token: 0x0600032A RID: 810 RVA: 0x0002B800 File Offset: 0x00029A00
	public void FreeAllAssetBundleAndReCreate(int iPos)
	{
		if (iPos >= this.modelABList.Count)
		{
			return;
		}
		this.modelABList[iPos].Unload(false);
		this.modelABList[iPos] = AssetBundle.CreateFromFile(this.strModelNameList[iPos]);
	}

	// Token: 0x0600032B RID: 811 RVA: 0x0002B850 File Offset: 0x00029A50
	public void LoadAnimation(Animation anim, string clipName)
	{
		string text = string.Empty;
		if (anim[clipName] != null)
		{
			return;
		}
		NpcCollider component = anim.gameObject.GetComponent<NpcCollider>();
		PlayerController component2 = anim.gameObject.GetComponent<PlayerController>();
		if (component != null)
		{
			text = component.m_strModelName;
		}
		else if (component2 != null)
		{
			text = component2.m_strModelName;
		}
		else
		{
			text = anim.gameObject.name;
		}
		string fileName = text + "@" + clipName;
		AnimationClip animationClip = this.Load(fileName) as AnimationClip;
		if (animationClip != null)
		{
			anim.AddClip(animationClip, clipName);
		}
	}

	// Token: 0x0600032C RID: 812 RVA: 0x0002B8FC File Offset: 0x00029AFC
	public void LoadAnimationGroup(GameObject goModel, string GroupName)
	{
		if (goModel.animation == null)
		{
			return;
		}
		string text = string.Empty;
		NpcCollider component = goModel.GetComponent<NpcCollider>();
		PlayerController component2 = goModel.GetComponent<PlayerController>();
		if (component != null)
		{
			text = component.m_strModelName;
		}
		else if (component2 != null)
		{
			text = component2.m_strModelName;
		}
		else
		{
			text = goModel.name;
		}
		string fileName = text + "_AnimationText";
		TextAsset textAsset = this.Load(fileName) as TextAsset;
		if (textAsset == null)
		{
			return;
		}
		string[] array = textAsset.text.Split(new char[]
		{
			"\n".get_Chars(0)
		});
		foreach (string text2 in array)
		{
			string text3 = text2.Trim();
			if (text3.IndexOf(GroupName) >= 0 && goModel.animation[text3] == null)
			{
				string fileName2 = text + "@" + text3;
				AnimationClip animationClip = this.Load(fileName2) as AnimationClip;
				if (animationClip != null)
				{
					goModel.animation.AddClip(animationClip, text3);
				}
			}
		}
	}

	// Token: 0x0600032D RID: 813 RVA: 0x0002BA40 File Offset: 0x00029C40
	public void LoadAllBattleAction(GameObject goModel)
	{
		if (goModel.animation == null)
		{
			return;
		}
		string text = string.Empty;
		NpcCollider component = goModel.GetComponent<NpcCollider>();
		PlayerController component2 = goModel.GetComponent<PlayerController>();
		if (component != null)
		{
			text = component.m_strModelName;
		}
		else if (component2 != null)
		{
			text = component2.m_strModelName;
		}
		else
		{
			text = goModel.name;
		}
		string fileName = text + "_AnimationText";
		TextAsset textAsset = this.Load(fileName) as TextAsset;
		if (textAsset == null)
		{
			return;
		}
		string[] array = textAsset.text.Split(new char[]
		{
			"\n".get_Chars(0)
		});
		foreach (string text2 in array)
		{
			string text3 = text2.Trim();
			text3 = text3.Replace("\r", string.Empty);
			if ((text3.IndexOf("die") >= 0 || text3.IndexOf("dodge") >= 0 || text3.IndexOf("idle") >= 0 || text3.IndexOf("run") >= 0 || text3.IndexOf("hurt01") >= 0 || text3.IndexOf("hurt02") >= 0 || text3.IndexOf("stand") >= 0) && goModel.animation[text3] == null)
			{
				string fileName2 = text + "@" + text3;
				AnimationClip animationClip = this.Load(fileName2) as AnimationClip;
				if (animationClip != null)
				{
					goModel.animation.AddClip(animationClip, text3);
				}
			}
		}
	}

	// Token: 0x04000250 RID: 592
	private List<AssetBundle> modelABList = new List<AssetBundle>();

	// Token: 0x04000251 RID: 593
	private List<string> strModelNameList = new List<string>();

	// Token: 0x04000252 RID: 594
	private string strDirectory;
}
