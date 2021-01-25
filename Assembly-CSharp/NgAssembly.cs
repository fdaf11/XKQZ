using System;
using System.Reflection;
using UnityEngine;

// Token: 0x020003ED RID: 1005
public class NgAssembly
{
	// Token: 0x06001808 RID: 6152 RVA: 0x0000FB55 File Offset: 0x0000DD55
	public static T GetReference<T>(object inObj, string fieldName) where T : class
	{
		return NgAssembly.GetField(inObj, fieldName) as T;
	}

	// Token: 0x06001809 RID: 6153 RVA: 0x0000FB68 File Offset: 0x0000DD68
	public static T GetValue<T>(object inObj, string fieldName) where T : struct
	{
		return (T)((object)NgAssembly.GetField(inObj, fieldName));
	}

	// Token: 0x0600180A RID: 6154 RVA: 0x000C60FC File Offset: 0x000C42FC
	public static void SetField(object inObj, string fieldName, object newValue)
	{
		FieldInfo field = inObj.GetType().GetField(fieldName);
		if (field != null)
		{
			field.SetValue(inObj, newValue);
		}
	}

	// Token: 0x0600180B RID: 6155 RVA: 0x000C6124 File Offset: 0x000C4324
	private static object GetField(object inObj, string fieldName)
	{
		object result = null;
		FieldInfo field = inObj.GetType().GetField(fieldName);
		if (field != null)
		{
			result = field.GetValue(inObj);
		}
		return result;
	}

	// Token: 0x0600180C RID: 6156 RVA: 0x000C6150 File Offset: 0x000C4350
	public static void SetProperty(object srcObj, string fieldName, object newValue)
	{
		PropertyInfo property = srcObj.GetType().GetProperty(fieldName, NgAssembly.m_bindingAttr);
		if (property != null && property.CanWrite)
		{
			property.SetValue(srcObj, newValue, null);
		}
		else
		{
			Debug.LogWarning(fieldName + " could not be write.");
		}
	}

	// Token: 0x0600180D RID: 6157 RVA: 0x000C61A0 File Offset: 0x000C43A0
	public static object GetProperty(object srcObj, string fieldName)
	{
		object result = null;
		PropertyInfo property = srcObj.GetType().GetProperty(fieldName, NgAssembly.m_bindingAttr);
		if (property != null && property.CanRead && property.GetIndexParameters().Length == 0)
		{
			result = property.GetValue(srcObj, null);
		}
		else
		{
			Debug.LogWarning(fieldName + " could not be read.");
		}
		return result;
	}

	// Token: 0x0600180E RID: 6158 RVA: 0x000C6200 File Offset: 0x000C4400
	public static void LogFieldsPropertis(Object srcObj)
	{
		if (srcObj == null)
		{
			return;
		}
		string text = "=====================================================================\r\n";
		FieldInfo[] fields = srcObj.GetType().GetFields(NgAssembly.m_bindingAttr);
		foreach (FieldInfo fieldInfo in fields)
		{
			text += string.Format("{0}   {1,-30}\r\n", fieldInfo.Name, fieldInfo.GetValue(srcObj).ToString());
		}
		Debug.Log(text);
		text = string.Empty;
		PropertyInfo[] properties = srcObj.GetType().GetProperties(NgAssembly.m_bindingAttr);
		foreach (PropertyInfo propertyInfo in properties)
		{
			if (propertyInfo.CanRead && propertyInfo.GetIndexParameters().Length == 0)
			{
				text += string.Format("{0,-10}{1,-30}   {2,-30}\r\n", propertyInfo.CanWrite, propertyInfo.Name, propertyInfo.GetValue(srcObj, null).ToString());
			}
		}
		text += "=====================================================================\r\n";
		Debug.Log(text);
	}

	// Token: 0x04001CF5 RID: 7413
	public static BindingFlags m_bindingAttr = 52;
}
