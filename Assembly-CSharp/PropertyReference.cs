using System;
using System.Diagnostics;
using System.Reflection;
using UnityEngine;

// Token: 0x020004A8 RID: 1192
[Serializable]
public class PropertyReference
{
	// Token: 0x06001D69 RID: 7529 RVA: 0x00002672 File Offset: 0x00000872
	public PropertyReference()
	{
	}

	// Token: 0x06001D6A RID: 7530 RVA: 0x00013780 File Offset: 0x00011980
	public PropertyReference(Component target, string fieldName)
	{
		this.mTarget = target;
		this.mName = fieldName;
	}

	// Token: 0x17000255 RID: 597
	// (get) Token: 0x06001D6C RID: 7532 RVA: 0x000137A7 File Offset: 0x000119A7
	// (set) Token: 0x06001D6D RID: 7533 RVA: 0x000137AF File Offset: 0x000119AF
	public Component target
	{
		get
		{
			return this.mTarget;
		}
		set
		{
			this.mTarget = value;
			this.mProperty = null;
			this.mField = null;
		}
	}

	// Token: 0x17000256 RID: 598
	// (get) Token: 0x06001D6E RID: 7534 RVA: 0x000137C6 File Offset: 0x000119C6
	// (set) Token: 0x06001D6F RID: 7535 RVA: 0x000137CE File Offset: 0x000119CE
	public string name
	{
		get
		{
			return this.mName;
		}
		set
		{
			this.mName = value;
			this.mProperty = null;
			this.mField = null;
		}
	}

	// Token: 0x17000257 RID: 599
	// (get) Token: 0x06001D70 RID: 7536 RVA: 0x000137E5 File Offset: 0x000119E5
	public bool isValid
	{
		get
		{
			return this.mTarget != null && !string.IsNullOrEmpty(this.mName);
		}
	}

	// Token: 0x17000258 RID: 600
	// (get) Token: 0x06001D71 RID: 7537 RVA: 0x000E5548 File Offset: 0x000E3748
	public bool isEnabled
	{
		get
		{
			if (this.mTarget == null)
			{
				return false;
			}
			MonoBehaviour monoBehaviour = this.mTarget as MonoBehaviour;
			return monoBehaviour == null || monoBehaviour.enabled;
		}
	}

	// Token: 0x06001D72 RID: 7538 RVA: 0x000E558C File Offset: 0x000E378C
	public Type GetPropertyType()
	{
		if (this.mProperty == null && this.mField == null && this.isValid)
		{
			this.Cache();
		}
		if (this.mProperty != null)
		{
			return this.mProperty.PropertyType;
		}
		if (this.mField != null)
		{
			return this.mField.FieldType;
		}
		return typeof(void);
	}

	// Token: 0x06001D73 RID: 7539 RVA: 0x000E55FC File Offset: 0x000E37FC
	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return !this.isValid;
		}
		if (obj is PropertyReference)
		{
			PropertyReference propertyReference = obj as PropertyReference;
			return this.mTarget == propertyReference.mTarget && string.Equals(this.mName, propertyReference.mName);
		}
		return false;
	}

	// Token: 0x06001D74 RID: 7540 RVA: 0x00013809 File Offset: 0x00011A09
	public override int GetHashCode()
	{
		return PropertyReference.s_Hash;
	}

	// Token: 0x06001D75 RID: 7541 RVA: 0x00013810 File Offset: 0x00011A10
	public void Set(Component target, string methodName)
	{
		this.mTarget = target;
		this.mName = methodName;
	}

	// Token: 0x06001D76 RID: 7542 RVA: 0x00013820 File Offset: 0x00011A20
	public void Clear()
	{
		this.mTarget = null;
		this.mName = null;
	}

	// Token: 0x06001D77 RID: 7543 RVA: 0x00013830 File Offset: 0x00011A30
	public void Reset()
	{
		this.mField = null;
		this.mProperty = null;
	}

	// Token: 0x06001D78 RID: 7544 RVA: 0x00013840 File Offset: 0x00011A40
	public override string ToString()
	{
		return PropertyReference.ToString(this.mTarget, this.name);
	}

	// Token: 0x06001D79 RID: 7545 RVA: 0x000E5658 File Offset: 0x000E3858
	public static string ToString(Component comp, string property)
	{
		if (!(comp != null))
		{
			return null;
		}
		string text = comp.GetType().ToString();
		int num = text.LastIndexOf('.');
		if (num > 0)
		{
			text = text.Substring(num + 1);
		}
		if (!string.IsNullOrEmpty(property))
		{
			return text + "." + property;
		}
		return text + ".[property]";
	}

	// Token: 0x06001D7A RID: 7546 RVA: 0x000E56BC File Offset: 0x000E38BC
	[DebuggerHidden]
	[DebuggerStepThrough]
	public object Get()
	{
		if (this.mProperty == null && this.mField == null && this.isValid)
		{
			this.Cache();
		}
		if (this.mProperty != null)
		{
			if (this.mProperty.CanRead)
			{
				return this.mProperty.GetValue(this.mTarget, null);
			}
		}
		else if (this.mField != null)
		{
			return this.mField.GetValue(this.mTarget);
		}
		return null;
	}

	// Token: 0x06001D7B RID: 7547 RVA: 0x000E5744 File Offset: 0x000E3944
	[DebuggerStepThrough]
	[DebuggerHidden]
	public bool Set(object value)
	{
		if (this.mProperty == null && this.mField == null && this.isValid)
		{
			this.Cache();
		}
		if (this.mProperty == null && this.mField == null)
		{
			return false;
		}
		if (value == null)
		{
			try
			{
				if (this.mProperty == null)
				{
					this.mField.SetValue(this.mTarget, null);
					return true;
				}
				if (this.mProperty.CanWrite)
				{
					this.mProperty.SetValue(this.mTarget, null, null);
					return true;
				}
			}
			catch (Exception)
			{
				return false;
			}
		}
		if (!this.Convert(ref value))
		{
			if (Application.isPlaying)
			{
				Debug.LogError(string.Concat(new object[]
				{
					"Unable to convert ",
					value.GetType(),
					" to ",
					this.GetPropertyType()
				}));
			}
		}
		else
		{
			if (this.mField != null)
			{
				this.mField.SetValue(this.mTarget, value);
				return true;
			}
			if (this.mProperty.CanWrite)
			{
				this.mProperty.SetValue(this.mTarget, value, null);
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001D7C RID: 7548 RVA: 0x000E58A4 File Offset: 0x000E3AA4
	[DebuggerStepThrough]
	[DebuggerHidden]
	private bool Cache()
	{
		if (this.mTarget != null && !string.IsNullOrEmpty(this.mName))
		{
			Type type = this.mTarget.GetType();
			this.mField = type.GetField(this.mName);
			this.mProperty = type.GetProperty(this.mName);
		}
		else
		{
			this.mField = null;
			this.mProperty = null;
		}
		return this.mField != null || this.mProperty != null;
	}

	// Token: 0x06001D7D RID: 7549 RVA: 0x000E5930 File Offset: 0x000E3B30
	private bool Convert(ref object value)
	{
		if (this.mTarget == null)
		{
			return false;
		}
		Type propertyType = this.GetPropertyType();
		Type from;
		if (value == null)
		{
			if (!propertyType.IsClass)
			{
				return false;
			}
			from = propertyType;
		}
		else
		{
			from = value.GetType();
		}
		return PropertyReference.Convert(ref value, from, propertyType);
	}

	// Token: 0x06001D7E RID: 7550 RVA: 0x000E5984 File Offset: 0x000E3B84
	public static bool Convert(Type from, Type to)
	{
		object obj = null;
		return PropertyReference.Convert(ref obj, from, to);
	}

	// Token: 0x06001D7F RID: 7551 RVA: 0x00013853 File Offset: 0x00011A53
	public static bool Convert(object value, Type to)
	{
		if (value == null)
		{
			value = null;
			return PropertyReference.Convert(ref value, to, to);
		}
		return PropertyReference.Convert(ref value, value.GetType(), to);
	}

	// Token: 0x06001D80 RID: 7552 RVA: 0x000E599C File Offset: 0x000E3B9C
	public static bool Convert(ref object value, Type from, Type to)
	{
		if (to.IsAssignableFrom(from))
		{
			return true;
		}
		if (to == typeof(string))
		{
			value = ((value == null) ? "null" : value.ToString());
			return true;
		}
		if (value == null)
		{
			return false;
		}
		float num2;
		if (to == typeof(int))
		{
			if (from == typeof(string))
			{
				int num;
				if (int.TryParse((string)value, ref num))
				{
					value = num;
					return true;
				}
			}
			else if (from == typeof(float))
			{
				value = Mathf.RoundToInt((float)value);
				return true;
			}
		}
		else if (to == typeof(float) && from == typeof(string) && float.TryParse((string)value, ref num2))
		{
			value = num2;
			return true;
		}
		return false;
	}

	// Token: 0x040021A5 RID: 8613
	[SerializeField]
	private Component mTarget;

	// Token: 0x040021A6 RID: 8614
	[SerializeField]
	private string mName;

	// Token: 0x040021A7 RID: 8615
	private FieldInfo mField;

	// Token: 0x040021A8 RID: 8616
	private PropertyInfo mProperty;

	// Token: 0x040021A9 RID: 8617
	private static int s_Hash = "PropertyBinding".GetHashCode();
}
