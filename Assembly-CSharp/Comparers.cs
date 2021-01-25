using System;
using System.Collections.Generic;

// Token: 0x0200067E RID: 1662
public static class Comparers
{
	// Token: 0x06002896 RID: 10390 RVA: 0x0001AC58 File Offset: 0x00018E58
	public static IEqualityComparer<T> Create<T>(Func<T, T, bool> equals, Func<T, int> hash = null)
	{
		return new Comparers.FuncEqualityComparer<T>(equals, hash ?? ((T t) => 1));
	}

	// Token: 0x04003300 RID: 13056
	public static readonly Comparers.StringComparer String = new Comparers.StringComparer();

	// Token: 0x04003301 RID: 13057
	public static readonly Comparers.FloatComparer Float = new Comparers.FloatComparer();

	// Token: 0x04003302 RID: 13058
	public static readonly Comparers.IntComparer Int = new Comparers.IntComparer();

	// Token: 0x04003303 RID: 13059
	public static readonly Comparers.ByteComparer Byte = new Comparers.ByteComparer();

	// Token: 0x04003304 RID: 13060
	public static readonly Comparers.BoolComparer Bool = new Comparers.BoolComparer();

	// Token: 0x0200067F RID: 1663
	[Serializable]
	public class StringComparer : IEqualityComparer<string>
	{
		// Token: 0x06002899 RID: 10393 RVA: 0x0001AC74 File Offset: 0x00018E74
		bool IEqualityComparer<string>.Equals(string x, string y)
		{
			return x == y;
		}

		// Token: 0x0600289A RID: 10394 RVA: 0x0001AC7D File Offset: 0x00018E7D
		int IEqualityComparer<string>.GetHashCode(string obj)
		{
			return obj.GetHashCode();
		}
	}

	// Token: 0x02000680 RID: 1664
	[Serializable]
	public class FloatComparer : IEqualityComparer<float>
	{
		// Token: 0x0600289C RID: 10396 RVA: 0x0001AC85 File Offset: 0x00018E85
		bool IEqualityComparer<float>.Equals(float x, float y)
		{
			return x == y;
		}

		// Token: 0x0600289D RID: 10397 RVA: 0x0001AC8B File Offset: 0x00018E8B
		int IEqualityComparer<float>.GetHashCode(float obj)
		{
			return obj.GetHashCode();
		}
	}

	// Token: 0x02000681 RID: 1665
	[Serializable]
	public class IntComparer : IEqualityComparer<int>
	{
		// Token: 0x0600289F RID: 10399 RVA: 0x0001AC85 File Offset: 0x00018E85
		bool IEqualityComparer<int>.Equals(int x, int y)
		{
			return x == y;
		}

		// Token: 0x060028A0 RID: 10400 RVA: 0x0001AC94 File Offset: 0x00018E94
		int IEqualityComparer<int>.GetHashCode(int obj)
		{
			return obj.GetHashCode();
		}
	}

	// Token: 0x02000682 RID: 1666
	[Serializable]
	public class ByteComparer : IEqualityComparer<byte>
	{
		// Token: 0x060028A2 RID: 10402 RVA: 0x0001AC85 File Offset: 0x00018E85
		bool IEqualityComparer<byte>.Equals(byte x, byte y)
		{
			return x == y;
		}

		// Token: 0x060028A3 RID: 10403 RVA: 0x0001AC9D File Offset: 0x00018E9D
		int IEqualityComparer<byte>.GetHashCode(byte obj)
		{
			return obj.GetHashCode();
		}
	}

	// Token: 0x02000683 RID: 1667
	[Serializable]
	public class BoolComparer : IEqualityComparer<bool>
	{
		// Token: 0x060028A5 RID: 10405 RVA: 0x0001AC85 File Offset: 0x00018E85
		bool IEqualityComparer<bool>.Equals(bool x, bool y)
		{
			return x == y;
		}

		// Token: 0x060028A6 RID: 10406 RVA: 0x0001ACA6 File Offset: 0x00018EA6
		int IEqualityComparer<bool>.GetHashCode(bool obj)
		{
			return obj.GetHashCode();
		}
	}

	// Token: 0x02000684 RID: 1668
	[Serializable]
	private class FuncEqualityComparer<T> : EqualityComparer<T>
	{
		// Token: 0x060028A7 RID: 10407 RVA: 0x0001ACAF File Offset: 0x00018EAF
		public FuncEqualityComparer(Func<T, T, bool> equals, Func<T, int> hash)
		{
			this.equals = equals;
			this.hash = hash;
		}

		// Token: 0x060028A8 RID: 10408 RVA: 0x0001ACC5 File Offset: 0x00018EC5
		public override bool Equals(T a, T b)
		{
			if (this.equals == null)
			{
				return a.Equals(b);
			}
			return this.equals.Invoke(a, b);
		}

		// Token: 0x060028A9 RID: 10409 RVA: 0x0001ACF3 File Offset: 0x00018EF3
		public override int GetHashCode(T obj)
		{
			if (this.hash == null)
			{
				return obj.GetHashCode();
			}
			return this.hash.Invoke(obj);
		}

		// Token: 0x04003305 RID: 13061
		private readonly Func<T, T, bool> equals;

		// Token: 0x04003306 RID: 13062
		private readonly Func<T, int> hash;
	}
}
