using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

// Token: 0x02000093 RID: 147
public class TestFileHidding : MonoBehaviour
{
	// Token: 0x0600033B RID: 827 RVA: 0x0002BEB0 File Offset: 0x0002A0B0
	private void Start()
	{
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		FileStream fileStream = null;
		IntPtr intPtr;
		intPtr..ctor(-1);
		IntPtr intPtr2 = MyTextFile.fileCreate("D:\\aaa.txt:bbb.txt");
		if (intPtr2 != intPtr)
		{
			fileStream = new FileStream(intPtr2, 2);
		}
		if (fileStream == null)
		{
			Debug.Log("D:\\aaa.txt:bbb.txt");
		}
		else
		{
			binaryFormatter.Serialize(fileStream, TestFileHidding.savedGames);
			fileStream.Close();
		}
		MyTextFile.fileClose(intPtr2);
	}

	// Token: 0x0400025E RID: 606
	private static List<MainGameObj> savedGames = new List<MainGameObj>();
}
