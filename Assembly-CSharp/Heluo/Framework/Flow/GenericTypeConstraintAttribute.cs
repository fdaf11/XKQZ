using System;
using System.Collections.Generic;
using UnityEngine;

namespace HeLuo.Framework.Flow
{
	// Token: 0x02000153 RID: 339
	[AttributeUsage(4, Inherited = false, AllowMultiple = true)]
	public sealed class GenericTypeConstraintAttribute : Attribute
	{
		// Token: 0x0600071F RID: 1823 RVA: 0x000063FB File Offset: 0x000045FB
		public GenericTypeConstraintAttribute(TypeConstraint constraint)
		{
			this.FillWithConstraint(constraint);
		}

		// Token: 0x06000720 RID: 1824 RVA: 0x00006415 File Offset: 0x00004615
		public GenericTypeConstraintAttribute(bool inherit, params Type[] types)
		{
			this.FillWithTypes(types, inherit);
		}

		// Token: 0x06000721 RID: 1825 RVA: 0x00006430 File Offset: 0x00004630
		public GenericTypeConstraintAttribute(TypeConstraint constraint, bool inherit, params Type[] types)
		{
			this.FillWithConstraint(constraint);
			this.FillWithTypes(types, inherit);
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000723 RID: 1827 RVA: 0x00006452 File Offset: 0x00004652
		public List<Type> AllowTypes
		{
			get
			{
				return this.allowTypes;
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000724 RID: 1828 RVA: 0x0000645A File Offset: 0x0000465A
		// (set) Token: 0x06000725 RID: 1829 RVA: 0x00006462 File Offset: 0x00004662
		public Type[] Customs { get; private set; }

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000726 RID: 1830 RVA: 0x0000646B File Offset: 0x0000466B
		// (set) Token: 0x06000727 RID: 1831 RVA: 0x00006473 File Offset: 0x00004673
		public TypeConstraint Constraint { get; private set; }

		// Token: 0x06000728 RID: 1832 RVA: 0x00048460 File Offset: 0x00046660
		private void FillWithConstraint(TypeConstraint constraint)
		{
			if (this.HasFlag(constraint, TypeConstraint.Primitive))
			{
				this.AllowTypes.AddRange(GenericTypeConstraintAttribute.primitiveTypes);
			}
			if (this.HasFlag(constraint, TypeConstraint.UnityPrimitive))
			{
				this.AllowTypes.AddRange(GenericTypeConstraintAttribute.unityPrimitiveTypes);
			}
			if (this.HasFlag(constraint, TypeConstraint.UnityObject))
			{
				this.AllowTypes.AddRange(GenericTypeConstraintAttribute.unityObjectTypes);
			}
		}

		// Token: 0x06000729 RID: 1833 RVA: 0x000484C4 File Offset: 0x000466C4
		private void FillWithTypes(Type[] types, bool inherit)
		{
			this.AllowTypes.AddRange(types);
			if (!inherit)
			{
				return;
			}
			Type[] types2 = typeof(OutputNode).Assembly.GetTypes();
			foreach (Type type in types2)
			{
				foreach (Type type2 in types)
				{
					if (type.IsSubclassOf(type2))
					{
						this.AllowTypes.Add(type);
					}
				}
			}
		}

		// Token: 0x0600072A RID: 1834 RVA: 0x0000647C File Offset: 0x0000467C
		private bool HasFlag(TypeConstraint a, TypeConstraint b)
		{
			return (a & b) != TypeConstraint.None;
		}

		// Token: 0x04000779 RID: 1913
		private static readonly Type[] primitiveTypes = new Type[]
		{
			typeof(bool),
			typeof(int),
			typeof(float),
			typeof(string)
		};

		// Token: 0x0400077A RID: 1914
		private static readonly Type[] unityPrimitiveTypes = new Type[]
		{
			typeof(Vector2),
			typeof(Vector3),
			typeof(Vector4),
			typeof(Rect),
			typeof(Color),
			typeof(Bounds),
			typeof(AnimationCurve)
		};

		// Token: 0x0400077B RID: 1915
		private static readonly Type[] unityObjectTypes = new Type[]
		{
			typeof(Object),
			typeof(GameObject),
			typeof(ScriptableObject),
			typeof(AudioClip),
			typeof(Material),
			typeof(Texture),
			typeof(Texture2D)
		};

		// Token: 0x0400077C RID: 1916
		private List<Type> allowTypes = new List<Type>();
	}
}
