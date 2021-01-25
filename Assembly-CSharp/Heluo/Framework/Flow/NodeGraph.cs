using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UnityEngine;

namespace HeLuo.Framework.Flow
{
	// Token: 0x02000189 RID: 393
	public class NodeGraph : ScriptableObject, IOutput, ICloneable, ISerializationCallbackReceiver
	{
		// Token: 0x060007DA RID: 2010 RVA: 0x0000264F File Offset: 0x0000084F
		protected virtual void OnDestroy()
		{
		}

		// Token: 0x060007DB RID: 2011 RVA: 0x000496E4 File Offset: 0x000478E4
		public void SetVariable(string name, object value)
		{
			if (!this.variables.ContainsKey(name))
			{
				Type type = typeof(VariableNode<>).MakeGenericType(new Type[]
				{
					value.GetType()
				});
				this.variables.Add(name, Activator.CreateInstance(type) as OutputNode);
			}
			(this.variables[name] as IInput).SetValue(value);
		}

		// Token: 0x060007DC RID: 2012 RVA: 0x00006D3B File Offset: 0x00004F3B
		public T GetVariable<T>(string name)
		{
			return (T)((object)this.GetVariable(name));
		}

		// Token: 0x060007DD RID: 2013 RVA: 0x00006D49 File Offset: 0x00004F49
		public object GetVariable(string name)
		{
			if (this.variables.ContainsKey(name))
			{
				return (this.variables[name] as IOutput).GetValue();
			}
			return null;
		}

		// Token: 0x060007DE RID: 2014 RVA: 0x00049750 File Offset: 0x00047950
		public virtual T GetValue<T>()
		{
			return (T)((object)this.GetValue());
		}

		// Token: 0x060007DF RID: 2015 RVA: 0x00006D74 File Offset: 0x00004F74
		public virtual object GetValue()
		{
			if (this.output != null)
			{
				return this.output.GetValue();
			}
			return null;
		}

		// Token: 0x060007E0 RID: 2016 RVA: 0x00006D8E File Offset: 0x00004F8E
		private void OnDestory()
		{
			this.variables.Clear();
		}

		// Token: 0x060007E1 RID: 2017 RVA: 0x0004976C File Offset: 0x0004796C
		public void OnAddObjectReference(Guid guid, Object o)
		{
			Debug.Log(string.Concat(new object[]
			{
				"Add ",
				guid,
				" : ",
				o
			}));
			if (!this.objectReference.ContainsKey(guid.ToString()))
			{
				this.objectReference.Add(guid.ToString(), o);
			}
		}

		// Token: 0x060007E2 RID: 2018 RVA: 0x00006D9B File Offset: 0x00004F9B
		public void OnRemoveObjectReference(Guid guid)
		{
			Debug.Log("Remove " + guid);
			this.objectReference.Remove(guid.ToString());
		}

		// Token: 0x060007E3 RID: 2019 RVA: 0x000497D0 File Offset: 0x000479D0
		protected OutputNode AddVariableIfNotFound<T>(string name)
		{
			if (!this.variables.ContainsKey(name))
			{
				Type type = typeof(VariableNode<>).MakeGenericType(new Type[]
				{
					typeof(T)
				});
				object obj = Activator.CreateInstance(type);
				this.variables.Add(name, obj as OutputNode);
			}
			return this.variables[name];
		}

		// Token: 0x060007E4 RID: 2020 RVA: 0x00049838 File Offset: 0x00047A38
		public virtual void OnAfterDeserialize()
		{
			DiagnosticsTraceWriter traceWriter = new DiagnosticsTraceWriter();
			JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
			{
				TypeNameHandling = 3,
				PreserveReferencesHandling = 3,
				Formatting = 1,
				ReferenceLoopHandling = 1,
				Binder = new CustomSerializationBinder(),
				TraceWriter = traceWriter
			};
			using (StringReader stringReader = new StringReader(this.serializedData))
			{
				using (JsonTextReader jsonTextReader = new JsonTextReader(stringReader))
				{
					JsonSerializer jsonSerializer = JsonSerializer.Create(jsonSerializerSettings);
					SerializeBundle serializeBundle = jsonSerializer.Deserialize<SerializeBundle>(jsonTextReader);
					this.variables = serializeBundle.variables;
					this.nodes = serializeBundle.nodes;
					this.output = serializeBundle.output;
				}
			}
			this.objectReference.Clear();
			for (int i = 0; i < this.objectKey.Count; i++)
			{
				this.objectReference.Add(this.objectKey[i], this.objectValue[i]);
			}
			foreach (OutputNode outputNode in this.nodes)
			{
				outputNode.rect.x = outputNode.x;
				outputNode.rect.y = outputNode.y;
				outputNode.OnAfterDeserialize(this);
			}
		}

		// Token: 0x060007E5 RID: 2021 RVA: 0x000499D8 File Offset: 0x00047BD8
		public virtual void OnBeforeSerialize()
		{
			foreach (OutputNode outputNode in this.nodes)
			{
				outputNode.x = outputNode.rect.x;
				outputNode.y = outputNode.rect.y;
			}
			SerializeBundle serializeBundle = new SerializeBundle
			{
				variables = this.variables,
				nodes = this.nodes,
				output = this.output
			};
			JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
			{
				TypeNameHandling = 3,
				PreserveReferencesHandling = 3,
				Formatting = 1,
				ReferenceLoopHandling = 1,
				Binder = new CustomSerializationBinder()
			};
			this.serializedData = JsonConvert.SerializeObject(serializeBundle, jsonSerializerSettings);
			this.objectKey.Clear();
			this.objectValue.Clear();
			foreach (KeyValuePair<string, Object> keyValuePair in this.objectReference)
			{
				this.objectKey.Add(keyValuePair.Key);
				this.objectValue.Add(keyValuePair.Value);
			}
		}

		// Token: 0x060007E6 RID: 2022 RVA: 0x00004A78 File Offset: 0x00002C78
		public virtual object Clone()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060007E7 RID: 2023 RVA: 0x00049B40 File Offset: 0x00047D40
		protected SerializeBundle CopySerializeBundle()
		{
			Dictionary<int, OutputNode> dictionary = new Dictionary<int, OutputNode>();
			List<OutputNode> list = new List<OutputNode>();
			Dictionary<string, OutputNode> dictionary2 = new Dictionary<string, OutputNode>();
			foreach (OutputNode outputNode in this.nodes)
			{
				OutputNode outputNode2 = outputNode.Clone() as OutputNode;
				list.Add(outputNode2);
				dictionary.Add(outputNode.GetHashCode(), outputNode2);
			}
			foreach (KeyValuePair<string, OutputNode> keyValuePair in this.variables)
			{
				OutputNode outputNode3 = keyValuePair.Value.Clone() as OutputNode;
				dictionary2.Add(keyValuePair.Key, outputNode3);
				dictionary.Add(keyValuePair.Value.GetHashCode(), outputNode3);
			}
			foreach (KeyValuePair<int, OutputNode> keyValuePair2 in dictionary)
			{
				List<Resolver> memberResolver = this.GetMemberResolver(keyValuePair2.Value);
				foreach (Resolver resolver in memberResolver)
				{
					resolver.Resolve(dictionary);
				}
			}
			SerializeBundle serializeBundle = new SerializeBundle
			{
				nodes = list,
				variables = dictionary2
			};
			if (this.output != null && dictionary.ContainsKey(this.output.GetHashCode()))
			{
				serializeBundle.output = (dictionary[this.output.GetHashCode()] as IOutput);
			}
			return serializeBundle;
		}

		// Token: 0x060007E8 RID: 2024 RVA: 0x00049D38 File Offset: 0x00047F38
		private List<Resolver> GetMemberResolver(object node)
		{
			List<Resolver> list = new List<Resolver>();
			foreach (FieldInfo fieldInfo in node.GetType().GetFields())
			{
				object[] customAttributes = fieldInfo.GetCustomAttributes(typeof(ArgumentAttribute), true);
				if (customAttributes.Length != 0)
				{
					object value = fieldInfo.GetValue(node);
					if (value != null)
					{
						Type type = value.GetType();
						Type @interface = type.GetInterface("IList");
						if (@interface != null)
						{
							IList list2 = value as IList;
							IList list3 = Activator.CreateInstance(type) as IList;
							for (int j = 0; j < list2.Count; j++)
							{
								object obj = list2[j];
								if (obj != null)
								{
									int hashCode = obj.GetHashCode();
									list.Add(new ListItemResolver(list3, hashCode));
								}
							}
							fieldInfo.SetValue(node, list3);
						}
						else
						{
							int hashCode2 = value.GetHashCode();
							list.Add(new FieldResolver(fieldInfo, node, hashCode2));
						}
					}
				}
			}
			return list;
		}

		// Token: 0x040007EA RID: 2026
		[NonSerialized]
		public Dictionary<string, OutputNode> variables = new Dictionary<string, OutputNode>();

		// Token: 0x040007EB RID: 2027
		[NonSerialized]
		public List<OutputNode> nodes = new List<OutputNode>();

		// Token: 0x040007EC RID: 2028
		[NonSerialized]
		public IOutput output;

		// Token: 0x040007ED RID: 2029
		[SerializeField]
		[HideInInspector]
		private string serializedData = string.Empty;

		// Token: 0x040007EE RID: 2030
		[HideInInspector]
		[SerializeField]
		private List<string> objectKey = new List<string>();

		// Token: 0x040007EF RID: 2031
		[SerializeField]
		[HideInInspector]
		private List<Object> objectValue = new List<Object>();

		// Token: 0x040007F0 RID: 2032
		internal Dictionary<string, Object> objectReference = new Dictionary<string, Object>();

		// Token: 0x040007F1 RID: 2033
		[HideInInspector]
		[SerializeField]
		public List<Vector2> pos = new List<Vector2>();
	}
}
