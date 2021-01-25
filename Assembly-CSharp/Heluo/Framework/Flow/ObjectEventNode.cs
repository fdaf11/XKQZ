using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Newtonsoft.Json;
using UnityEngine;

namespace HeLuo.Framework.Flow
{
	// Token: 0x0200017F RID: 383
	[Description("內部使用/EventNode")]
	[GenericTypeConstraint(TypeConstraint.All)]
	public class ObjectEventNode : ObjectMemberNode<bool>
	{
		// Token: 0x0600078A RID: 1930 RVA: 0x0000686C File Offset: 0x00004A6C
		public override bool GetValue()
		{
			this.Register();
			return false;
		}

		// Token: 0x0600078B RID: 1931 RVA: 0x00049004 File Offset: 0x00047204
		public void Register()
		{
			EventInfo eventInfo = this.memberInfo as EventInfo;
			object value = this.objectNode.GetValue();
			if (eventInfo == null || value == null)
			{
				return;
			}
			if (this.handler != null)
			{
				this.Unregister();
			}
			this.handler = this.MakeDelegateForEvent(eventInfo);
			eventInfo.AddEventHandler(value, this.handler);
		}

		// Token: 0x0600078C RID: 1932 RVA: 0x00049064 File Offset: 0x00047264
		public void Unregister()
		{
			EventInfo eventInfo = this.memberInfo as EventInfo;
			object value = this.objectNode.GetValue();
			if (eventInfo == null || value == null)
			{
				return;
			}
			if (this.handler != null)
			{
				eventInfo.RemoveEventHandler(value, this.handler);
			}
		}

		// Token: 0x0600078D RID: 1933 RVA: 0x000490B0 File Offset: 0x000472B0
		private Delegate MakeDelegateForEvent(EventInfo e)
		{
			ConstantExpression constantExpression = Expression.Constant(this);
			Type[] genericArguments = e.EventHandlerType.GetGenericArguments();
			ParameterExpression[] array = new ParameterExpression[genericArguments.Length];
			Expression[] array2 = new Expression[genericArguments.Length + 1];
			MethodInfo method = base.GetType().GetMethod("Assign", 36);
			MethodInfo method2 = base.GetType().GetMethod("OnCallback", 36);
			array2[array2.Length - 1] = Expression.Call(constantExpression, method2);
			for (int i = 0; i < genericArguments.Length; i++)
			{
				array[i] = Expression.Parameter(genericArguments[i], "param" + i);
				UnaryExpression unaryExpression = Expression.Convert(array[i], typeof(object));
				ConstantExpression constantExpression2 = Expression.Constant(this.objectListNode[i]);
				array2[i] = Expression.Call(constantExpression, method, new Expression[]
				{
					unaryExpression,
					constantExpression2
				});
			}
			Expression expression = ObjectEventNode.Block(array2);
			LambdaExpression lambdaExpression = Expression.Lambda(expression, array);
			return lambdaExpression.Compile();
		}

		// Token: 0x0600078E RID: 1934 RVA: 0x000491B0 File Offset: 0x000473B0
		private static Expression Block(params Expression[] expr)
		{
			Action<Action[]> action = delegate(Action[] a)
			{
				for (int j = 0; j < a.Length; j++)
				{
					a[j].Invoke();
				}
			};
			Expression<Action>[] array = new Expression<Action>[expr.Length];
			for (int i = 0; i < expr.Length; i++)
			{
				array[i] = Expression.Lambda<Action>(expr[i], new ParameterExpression[0]);
			}
			NewArrayExpression newArrayExpression = Expression.NewArrayInit(typeof(Action), array);
			return Expression.Call(action.Method, new Expression[]
			{
				newArrayExpression
			});
		}

		// Token: 0x0600078F RID: 1935 RVA: 0x00006875 File Offset: 0x00004A75
		private void Print(object o)
		{
			Debug.Log(o);
		}

		// Token: 0x06000790 RID: 1936 RVA: 0x00049230 File Offset: 0x00047430
		private void Assign(object obj, OutputNode node)
		{
			Console.WriteLine("Assign");
			Type type = node.GetType();
			FieldInfo field = type.GetField("value");
			if (field != null)
			{
				field.SetValue(node, obj);
			}
		}

		// Token: 0x06000791 RID: 1937 RVA: 0x0000687D File Offset: 0x00004A7D
		private void OnCallback()
		{
			if (this.actionNode != null)
			{
				this.actionNode.GetValue();
			}
		}

		// Token: 0x040007CF RID: 1999
		[Argument("參數")]
		public List<OutputNode> objectListNode = new List<OutputNode>();

		// Token: 0x040007D0 RID: 2000
		[Argument("動作", Optional = true)]
		public ActionNode actionNode;

		// Token: 0x040007D1 RID: 2001
		[JsonIgnore]
		protected Delegate handler;
	}
}
