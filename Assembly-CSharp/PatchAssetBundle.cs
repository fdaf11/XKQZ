using System;
using System.Collections.Generic;
using System.IO;
using Heluo.Wulin;
using UnityEngine;

// Token: 0x020001CF RID: 463
public class PatchAssetBundle
{
	// Token: 0x06000986 RID: 2438 RVA: 0x00051560 File Offset: 0x0004F760
	public static PatchAssetBundle CreateFromFile(string path, List<AssetBundle> list)
	{
		int num = path.LastIndexOf(".");
		PatchAssetBundle.strOutSidePath = path.Substring(0, num) + "/";
		PatchAssetBundle patchAssetBundle = new PatchAssetBundle();
		patchAssetBundle.CreateOrigAssetBundle(path);
		if (patchAssetBundle.origAssetBundle == null)
		{
			return null;
		}
		patchAssetBundle.SetPatchAssetBundleList(list);
		return patchAssetBundle;
	}

	// Token: 0x06000987 RID: 2439 RVA: 0x00007C5E File Offset: 0x00005E5E
	private void SetPatchAssetBundleList(List<AssetBundle> list)
	{
		this.patchAssetBundleList = list;
	}

	// Token: 0x06000988 RID: 2440 RVA: 0x00007C67 File Offset: 0x00005E67
	private void CreateOrigAssetBundle(string path)
	{
		this.origAssetBundle = AssetBundle.CreateFromFile(path);
	}

	// Token: 0x06000989 RID: 2441 RVA: 0x000515B8 File Offset: 0x0004F7B8
	public bool Contains(string name)
	{
		if (File.Exists(PatchAssetBundle.strOutSidePath + name + ".png"))
		{
			return true;
		}
		for (int i = 0; i < this.patchAssetBundleList.Count; i++)
		{
			if (this.patchAssetBundleList[i].Contains(name))
			{
				return true;
			}
		}
		return this.origAssetBundle != null && this.origAssetBundle.Contains(name);
	}

	// Token: 0x0600098A RID: 2442 RVA: 0x00051638 File Offset: 0x0004F838
	public Object Load(string name)
	{
		if (File.Exists(PatchAssetBundle.strOutSidePath + name + ".png"))
		{
			Texture2D texture2D = new Texture2D(2, 2, 5, false, false);
			texture2D.LoadImage(File.ReadAllBytes(PatchAssetBundle.strOutSidePath + name + ".png"));
			return texture2D;
		}
		for (int i = 0; i < this.patchAssetBundleList.Count; i++)
		{
			if (this.patchAssetBundleList[i].Contains(name))
			{
				return this.patchAssetBundleList[i].Load(name);
			}
		}
		if (this.origAssetBundle != null)
		{
			return this.origAssetBundle.Load(name);
		}
		return null;
	}

	// Token: 0x0600098B RID: 2443 RVA: 0x00007C75 File Offset: 0x00005E75
	public void Unload(bool unloadAllLoadedObjects)
	{
		if (this.origAssetBundle != null)
		{
			this.origAssetBundle.Unload(unloadAllLoadedObjects);
		}
	}

	// Token: 0x0600098C RID: 2444 RVA: 0x000516F0 File Offset: 0x0004F8F0
	public Sprite mod_Load(string fileName)
	{
		string text = Game.g_strDataPathToApplicationPath + "Mods/1.0.2.8/Config/Iamge/" + fileName + ".png";
		Texture2D texture2D = new Texture2D(2, 2, 5, false, false);
		texture2D.LoadImage(File.ReadAllBytes(text));
		return Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), Vector2.zero);
	}

	// Token: 0x04000978 RID: 2424
	public List<AssetBundle> patchAssetBundleList;

	// Token: 0x04000979 RID: 2425
	public AssetBundle origAssetBundle;

	// Token: 0x0400097A RID: 2426
	public static string strOutSidePath;
}
