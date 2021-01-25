using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

// Token: 0x02000499 RID: 1177
[Serializable]
public class EventDelegate
{
	// Token: 0x06001C67 RID: 7271 RVA: 0x00002672 File Offset: 0x00000872
	public EventDelegate()
	{
	}

	// Token: 0x06001C68 RID: 7272 RVA: 0x00012DE2 File Offset: 0x00010FE2
	public EventDelegate(EventDelegate.Callback call)
	{
		this.Set(call);
	}

	// Token: 0x06001C69 RID: 7273 RVA: 0x00012DF1 File Offset: 0x00010FF1
	public EventDelegate(MonoBehaviour target, string methodName)
	{
		this.Set(target, methodName);
	}

	// Token: 0x17000245 RID: 581
	// (get) Token: 0x06001C6B RID: 7275 RVA: 0x00012E12 File Offset: 0x00011012
	// (set) Token: 0x06001C6C RID: 7276 RVA: 0x00012E1A File Offset: 0x0001101A
	public MonoBehaviour target
	{
		get
		{
			return this.mTarget;
		}
		set
		{
			this.mTarget = value;
			this.mCachedCallback = null;
			this.mRawDelegate = false;
			this.mCached = false;
			this.mMethod = null;
			this.mParameters = null;
		}
	}

	// Token: 0x17000246 RID: 582
	// (get) Token: 0x06001C6D RID: 7277 RVA: 0x00012E46 File Offset: 0x00011046
	// (set) Token: 0x06001C6E RID: 7278 RVA: 0x00012E4E File Offset: 0x0001104E
	public string methodName
	{
		get
		{
			return this.mMethodName;
		}
		set
		{
			this.mMethodName = value;
			this.mCachedCallback = null;
			this.mRawDelegate = false;
			this.mCached = false;
			this.mMethod = null;
			this.mParameters = null;
		}
	}

	// Token: 0x17000247 RID: 583
	// (get) Token: 0x06001C6F RID: 7279 RVA: 0x00012E7A File Offset: 0x0001107A
	public EventDelegate.Parameter[] parameters
	{
		get
		{
			if (!this.mCached)
			{
				this.Cache();
			}
			return this.mParameters;
		}
	}

	// Token: 0x17000248 RID: 584
	// (get) Token: 0x06001C70 RID: 7280 RVA: 0x000DD198 File Offset: 0x000DB398
	public bool isValid
	{
		get
		{
			if (!this.mCached)
			{
				this.Cache();
			}
			return (this.mRawDelegate && this.mCachedCallback != null) || (this.mTarget != null && !string.IsNullOrEmpty(this.mMethodName));
		}
	}

	// Token: 0x17000249 RID: 585
	// (get) Token: 0x06001C71 RID: 7281 RVA: 0x000DD1F4 File Offset: 0x000DB3F4
	public bool isEnabled
	{
		get
		{
			if (!this.mCached)
			{
				this.Cache();
			}
			if (this.mRawDelegate && this.mCachedCallback != null)
			{
				return true;
			}
			if (this.mTarget == null)
			{
				return false;
			}
			MonoBehaviour monoBehaviour = this.mTarget;
			return monoBehaviour == null || monoBehaviour.enabled;
		}
	}

	// Token: 0x06001C72 RID: 7282 RVA: 0x00012E93 File Offset: 0x00011093
	private static string GetMethodName(EventDelegate.Callback callback)
	{
		return callback.Method.Name;
	}

	// Token: 0x06001C73 RID: 7283 RVA: 0x00012EA0 File Offset: 0x000110A0
	private static bool IsValid(EventDelegate.Callback callback)
	{
		return callback != null && callback.Method != null;
	}

	// Token: 0x06001C74 RID: 7284 RVA: 0x000DD25C File Offset: 0x000DB45C
	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return !this.isValid;
		}
		if (obj is EventDelegate.Callback)
		{
			EventDelegate.Callback callback = obj as EventDelegate.Callback;
			if (callback.Equals(this.mCachedCallback))
			{
				return true;
			}
			MonoBehaviour monoBehaviour = callback.Target as MonoBehaviour;
			return this.mTarget == monoBehaviour && string.Equals(this.mMethodName, EventDelegate.GetMethodName(callback));
		}
		else
		{
			if (obj is EventDelegate)
			{
				EventDelegate eventDelegate = obj as EventDelegate;
				return this.mTarget == eventDelegate.mTarget && string.Equals(this.mMethodName, eventDelegate.mMethodName);
			}
			return false;
		}
	}

	// Token: 0x06001C75 RID: 7285 RVA: 0x00012EB7 File Offset: 0x000110B7
	public override int GetHashCode()
	{
		return EventDelegate.s_Hash;
	}

	// Token: 0x06001C76 RID: 7286 RVA: 0x000DD310 File Offset: 0x000DB510
	private void Set(EventDelegate.Callback call)
	{
		this.Clear();
		if (call != null && EventDelegate.IsValid(call))
		{
			this.mTarget = (call.Target as MonoBehaviour);
			if (this.mTarget == null)
			{
				this.mRawDelegate = true;
				this.mCachedCallback = call;
				this.mMethodName = null;
			}
			else
			{
				this.mMethodName = EventDelegate.GetMethodName(call);
				this.mRawDelegate = false;
			}
		}
	}

	// Token: 0x06001C77 RID: 7287 RVA: 0x00012EBE File Offset: 0x000110BE
	public void Set(MonoBehaviour target, string methodName)
	{
		this.Clear();
		this.mTarget = target;
		this.mMethodName = methodName;
	}

	// Token: 0x06001C78 RID: 7288 RVA: 0x000DD384 File Offset: 0x000DB584
	private void Cache()
	{
		this.mCached = true;
		if (this.mRawDelegate)
		{
			return;
		}
		if ((this.mCachedCallback == null || this.mCachedCallback.Target as MonoBehaviour != this.mTarget || EventDelegate.GetMethodName(this.mCachedCallback) != this.mMethodName) && this.mTarget != null && !string.IsNullOrEmpty(this.mMethodName))
		{
			Type type = this.mTarget.GetType();
			this.mMethod = null;
			while (type != null)
			{
				try
				{
					this.mMethod = type.GetMethod(this.mMethodName, 52);
					if (this.mMethod != null)
					{
						break;
					}
				}
				catch (Exception)
				{
				}
				type = type.BaseType;
			}
			if (this.mMethod == null)
			{
				Debug.LogError(string.Concat(new object[]
				{
					"Could not find method '",
					this.mMethodName,
					"' on ",
					this.mTarget.GetType()
				}), this.mTarget);
				return;
			}
			if (this.mMethod.ReturnType != typeof(void))
			{
				Debug.LogError(string.Concat(new object[]
				{
					this.mTarget.GetType(),
					".",
					this.mMethodName,
					" must have a 'void' return type."
				}), this.mTarget);
				return;
			}
			ParameterInfo[] parameters = this.mMethod.GetParameters();
			if (parameters.Length == 0)
			{
				this.mCachedCallback = (EventDelegate.Callback)Delegate.CreateDelegate(typeof(EventDelegate.Callback), this.mTarget, this.mMethodName);
				this.mArgs = null;
				this.mParameters = null;
				return;
			}
			this.mCachedCallback = null;
			if (this.mParameters == null || this.mParameters.Length != parameters.Length)
			{
				this.mParameters = new EventDelegate.Parameter[parameters.Length];
				int i = 0;
				int num = this.mParameters.Length;
				while (i < num)
				{
					this.mParameters[i] = new EventDelegate.Parameter();
					i++;
				}
			}
			int j = 0;
			int num2 = this.mParameters.Length;
			while (j < num2)
			{
				this.mParameters[j].expectedType = parameters[j].ParameterType;
				j++;
			}
		}
	}

	// Token: 0x06001C79 RID: 7289 RVA: 0x000DD5EC File Offset: 0x000DB7EC
	public bool Execute()
	{
		if (!this.mCached)
		{
			this.Cache();
		}
		if (this.mCachedCallback != null)
		{
			this.mCachedCallback();
			return true;
		}
		if (this.mMethod != null)
		{
			if (this.mParameters == null || this.mParameters.Length == 0)
			{
				this.mMethod.Invoke(this.mTarget, null);
			}
			else
			{
				if (this.mArgs == null || this.mArgs.Length != this.mParameters.Length)
				{
					this.mArgs = new object[this.mParameters.Length];
				}
				int i = 0;
				int num = this.mParameters.Length;
				while (i < num)
				{
					this.mArgs[i] = this.mParameters[i].value;
					i++;
				}
				try
				{
					this.mMethod.Invoke(this.mTarget, this.mArgs);
				}
				catch (ArgumentException ex)
				{
					string text = "Error calling ";
					if (this.mTarget == null)
					{
						text += this.mMethod.Name;
					}
					else
					{
						string text2 = text;
						text = string.Concat(new object[]
						{
							text2,
							this.mTarget.GetType(),
							".",
							this.mMethod.Name
						});
					}
					text = text + ": " + ex.Message;
					text += "\n  Expected: ";
					ParameterInfo[] parameters = this.mMethod.GetParameters();
					if (parameters.Length == 0)
					{
						text += "no arguments";
					}
					else
					{
						text += parameters[0];
						for (int j = 1; j < parameters.Length; j++)
						{
							text = text + ", " + parameters[j].ParameterType;
						}
					}
					text += "\n  Received: ";
					if (this.mParameters.Length == 0)
					{
						text += "no arguments";
					}
					else
					{
						text += this.mParameters[0].type;
						for (int k = 1; k < this.mParameters.Length; k++)
						{
							text = text + ", " + this.mParameters[k].type;
						}
					}
					text += "\n";
					Debug.LogError(text);
				}
				int l = 0;
				int num2 = this.mArgs.Length;
				while (l < num2)
				{
					this.mArgs[l] = null;
					l++;
				}
			}
			return true;
		}
		return false;
	}

	// Token: 0x06001C7A RID: 7290 RVA: 0x00012ED4 File Offset: 0x000110D4
	public void Clear()
	{
		this.mTarget = null;
		this.mMethodName = null;
		this.mRawDelegate = false;
		this.mCachedCallback = null;
		this.mParameters = null;
		this.mCached = false;
		this.mMethod = null;
		this.mArgs = null;
	}

	// Token: 0x06001C7B RID: 7291 RVA: 0x000DD8BC File Offset: 0x000DBABC
	public override string ToString()
	{
		if (!(this.mTarget != null))
		{
			return (!this.mRawDelegate) ? null : "[delegate]";
		}
		string text = this.mTarget.GetType().ToString();
		int num = text.LastIndexOf('.');
		if (num > 0)
		{
			text = text.Substring(num + 1);
		}
		if (!string.IsNullOrEmpty(this.methodName))
		{
			return text + "/" + this.methodName;
		}
		return text + "/[delegate]";
	}

	// Token: 0x06001C7C RID: 7292 RVA: 0x000DD94C File Offset: 0x000DBB4C
	public static void Execute(List<EventDelegate> list)
	{
		if (list != null)
		{
			for (int i = 0; i < list.Count; i++)
			{
				EventDelegate eventDelegate = list[i];
				if (eventDelegate != null)
				{
					try
					{
						eventDelegate.Execute();
					}
					catch (Exception ex)
					{
						if (ex.InnerException != null)
						{
							Debug.LogError(ex.InnerException.Message);
						}
						else
						{
							Debug.LogError(ex.Message);
						}
					}
					if (i >= list.Count)
					{
						break;
					}
					if (list[i] != eventDelegate)
					{
						continue;
					}
					if (eventDelegate.oneShot)
					{
						list.RemoveAt(i);
						continue;
					}
				}
			}
		}
	}

	// Token: 0x06001C7D RID: 7293 RVA: 0x000DDA0C File Offset: 0x000DBC0C
	public static bool IsValid(List<EventDelegate> list)
	{
		if (list != null)
		{
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				EventDelegate eventDelegate = list[i];
				if (eventDelegate != null && eventDelegate.isValid)
				{
					return true;
				}
				i++;
			}
		}
		return false;
	}

	// Token: 0x06001C7E RID: 7294 RVA: 0x000DDA54 File Offset: 0x000DBC54
	public static EventDelegate Set(List<EventDelegate> list, EventDelegate.Callback callback)
	{
		if (list != null)
		{
			EventDelegate eventDelegate = new EventDelegate(callback);
			list.Clear();
			list.Add(eventDelegate);
			return eventDelegate;
		}
		return null;
	}

	// Token: 0x06001C7F RID: 7295 RVA: 0x00012F0E File Offset: 0x0001110E
	public static void Set(List<EventDelegate> list, EventDelegate del)
	{
		if (list != null)
		{
			list.Clear();
			list.Add(del);
		}
	}

	// Token: 0x06001C80 RID: 7296 RVA: 0x00012F23 File Offset: 0x00011123
	public static EventDelegate Add(List<EventDelegate> list, EventDelegate.Callback callback)
	{
		return EventDelegate.Add(list, callback, false);
	}

	// Token: 0x06001C81 RID: 7297 RVA: 0x000DDA80 File Offset: 0x000DBC80
	public static EventDelegate Add(List<EventDelegate> list, EventDelegate.Callback callback, bool oneShot)
	{
		if (list != null)
		{
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				EventDelegate eventDelegate = list[i];
				if (eventDelegate != null && eventDelegate.Equals(callback))
				{
					return eventDelegate;
				}
				i++;
			}
			EventDelegate eventDelegate2 = new EventDelegate(callback);
			eventDelegate2.oneShot = oneShot;
			list.Add(eventDelegate2);
			return eventDelegate2;
		}
		Debug.LogWarning("Attempting to add a callback to a list that's null");
		return null;
	}

	// Token: 0x06001C82 RID: 7298 RVA: 0x00012F2D File Offset: 0x0001112D
	public static void Add(List<EventDelegate> list, EventDelegate ev)
	{
		EventDelegate.Add(list, ev, ev.oneShot);
	}

	// Token: 0x06001C83 RID: 7299 RVA: 0x000DDAEC File Offset: 0x000DBCEC
	public static void Add(List<EventDelegate> list, EventDelegate ev, bool oneShot)
	{
		if (ev.mRawDelegate || ev.target == null || string.IsNullOrEmpty(ev.methodName))
		{
			EventDelegate.Add(list, ev.mCachedCallback, oneShot);
		}
		else if (list != null)
		{
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				EventDelegate eventDelegate = list[i];
				if (eventDelegate != null && eventDelegate.Equals(ev))
				{
					return;
				}
				i++;
			}
			EventDelegate eventDelegate2 = new EventDelegate(ev.target, ev.methodName);
			eventDelegate2.oneShot = oneShot;
			if (ev.mParameters != null && ev.mParameters.Length > 0)
			{
				eventDelegate2.mParameters = new EventDelegate.Parameter[ev.mParameters.Length];
				for (int j = 0; j < ev.mParameters.Length; j++)
				{
					eventDelegate2.mParameters[j] = ev.mParameters[j];
				}
			}
			list.Add(eventDelegate2);
		}
		else
		{
			Debug.LogWarning("Attempting to add a callback to a list that's null");
		}
	}

	// Token: 0x06001C84 RID: 7300 RVA: 0x000DDBFC File Offset: 0x000DBDFC
	public static bool Remove(List<EventDelegate> list, EventDelegate.Callback callback)
	{
		if (list != null)
		{
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				EventDelegate eventDelegate = list[i];
				if (eventDelegate != null && eventDelegate.Equals(callback))
				{
					list.RemoveAt(i);
					return true;
				}
				i++;
			}
		}
		return false;
	}

	// Token: 0x06001C85 RID: 7301 RVA: 0x000DDBFC File Offset: 0x000DBDFC
	public static bool Remove(List<EventDelegate> list, EventDelegate ev)
	{
		if (list != null)
		{
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				EventDelegate eventDelegate = list[i];
				if (eventDelegate != null && eventDelegate.Equals(ev))
				{
					list.RemoveAt(i);
					return true;
				}
				i++;
			}
		}
		return false;
	}

	// Token: 0x0400213F RID: 8511
	[SerializeField]
	private MonoBehaviour mTarget;

	// Token: 0x04002140 RID: 8512
	[SerializeField]
	private string mMethodName;

	// Token: 0x04002141 RID: 8513
	[SerializeField]
	private EventDelegate.Parameter[] mParameters;

	// Token: 0x04002142 RID: 8514
	public bool oneShot;

	// Token: 0x04002143 RID: 8515
	[NonSerialized]
	private EventDelegate.Callback mCachedCallback;

	// Token: 0x04002144 RID: 8516
	[NonSerialized]
	private bool mRawDelegate;

	// Token: 0x04002145 RID: 8517
	[NonSerialized]
	private bool mCached;

	// Token: 0x04002146 RID: 8518
	[NonSerialized]
	private MethodInfo mMethod;

	// Token: 0x04002147 RID: 8519
	[NonSerialized]
	private object[] mArgs;

	// Token: 0x04002148 RID: 8520
	private static int s_Hash = "EventDelegate".GetHashCode();

	// Token: 0x0200049A RID: 1178
	[Serializable]
	public class Parameter
	{
		// Token: 0x06001C86 RID: 7302 RVA: 0x00012F3C File Offset: 0x0001113C
		public Parameter()
		{
		}

		// Token: 0x06001C87 RID: 7303 RVA: 0x00012F54 File Offset: 0x00011154
		public Parameter(Object obj, string field)
		{
			this.obj = obj;
			this.field = field;
		}

		// Token: 0x06001C88 RID: 7304 RVA: 0x00012F7A File Offset: 0x0001117A
		public Parameter(object val)
		{
			this.mValue = val;
		}

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x06001C89 RID: 7305 RVA: 0x000DDC4C File Offset: 0x000DBE4C
		// (set) Token: 0x06001C8A RID: 7306 RVA: 0x00012F99 File Offset: 0x00011199
		public object value
		{
			get
			{
				if (this.mValue != null)
				{
					return this.mValue;
				}
				if (!this.cached)
				{
					this.cached = true;
					this.fieldInfo = null;
					this.propInfo = null;
					if (this.obj != null && !string.IsNullOrEmpty(this.field))
					{
						Type type = this.obj.GetType();
						this.propInfo = type.GetProperty(this.field);
						if (this.propInfo == null)
						{
							this.fieldInfo = type.GetField(this.field);
						}
					}
				}
				if (this.propInfo != null)
				{
					return this.propInfo.GetValue(this.obj, null);
				}
				if (this.fieldInfo != null)
				{
					return this.fieldInfo.GetValue(this.obj);
				}
				if (this.obj != null)
				{
					return this.obj;
				}
				if (this.expectedType != null && this.expectedType.IsValueType)
				{
					return null;
				}
				return Convert.ChangeType(null, this.expectedType);
			}
			set
			{
				this.mValue = value;
			}
		}

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x06001C8B RID: 7307 RVA: 0x00012FA2 File Offset: 0x000111A2
		public Type type
		{
			get
			{
				if (this.mValue != null)
				{
					return this.mValue.GetType();
				}
				if (this.obj == null)
				{
					return typeof(void);
				}
				return this.obj.GetType();
			}
		}

		// Token: 0x04002149 RID: 8521
		public Object obj;

		// Token: 0x0400214A RID: 8522
		public string field;

		// Token: 0x0400214B RID: 8523
		[NonSerialized]
		private object mValue;

		// Token: 0x0400214C RID: 8524
		[NonSerialized]
		public Type expectedType = typeof(void);

		// Token: 0x0400214D RID: 8525
		[NonSerialized]
		public bool cached;

		// Token: 0x0400214E RID: 8526
		[NonSerialized]
		public PropertyInfo propInfo;

		// Token: 0x0400214F RID: 8527
		[NonSerialized]
		public FieldInfo fieldInfo;
	}

	// Token: 0x0200049B RID: 1179
	// (Invoke) Token: 0x06001C8D RID: 7309
	public delegate void Callback();
}
