using System;
using System.IO;
using Heluo.Wulin;
using UnityEngine;

// Token: 0x02000147 RID: 327
public class ErrorReporter : MonoBehaviour
{
	// Token: 0x060006EA RID: 1770 RVA: 0x00047D28 File Offset: 0x00045F28
	private void OnEnable()
	{
		Application.RegisterLogCallback(new Application.LogCallback(this.CaptureLog));
		string text = Game.g_strAssetDataPath + "ErrorReport";
		if (!Directory.Exists(text))
		{
			Directory.CreateDirectory(text);
		}
	}

	// Token: 0x060006EB RID: 1771 RVA: 0x00006247 File Offset: 0x00004447
	private void OnDisable()
	{
		this.WriteErrorReport();
		Application.RegisterLogCallback(null);
	}

	// Token: 0x060006EC RID: 1772 RVA: 0x00047D68 File Offset: 0x00045F68
	private void WriteErrorReport()
	{
		if (this.debugText.Length > 0)
		{
			string text = DateTime.Now.Year.ToString();
			string text2 = DateTime.Now.Month.ToString();
			string text3 = DateTime.Now.Day.ToString();
			string text4 = DateTime.Now.Hour.ToString();
			string text5 = DateTime.Now.Minute.ToString();
			string text6 = string.Concat(new string[]
			{
				text,
				"-",
				text2,
				"-",
				text3,
				" ",
				text4,
				":",
				text5
			});
			string text7 = Game.g_strAssetDataPath + "ErrorReport/errorreport_" + text6 + ".txt";
			StreamWriter streamWriter = new StreamWriter(text7);
			streamWriter.Write(this.debugText);
			streamWriter.Close();
		}
	}

	// Token: 0x060006ED RID: 1773 RVA: 0x00047E78 File Offset: 0x00046078
	private void CaptureLog(string condition, string stacktrace, LogType type)
	{
		if (type == 4)
		{
			this.debugText = string.Concat(new string[]
			{
				type.ToString(),
				" ",
				Time.realtimeSinceStartup.ToString(),
				" ",
				condition,
				" ",
				stacktrace,
				"\n"
			});
			this.WriteErrorReport();
		}
	}

	// Token: 0x04000757 RID: 1879
	private string debugText = string.Empty;
}
