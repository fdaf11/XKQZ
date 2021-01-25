using System;
using UnityEngine;

namespace Xft
{
	// Token: 0x02000589 RID: 1417
	public class XftSmoothRandom
	{
		// Token: 0x06002389 RID: 9097 RVA: 0x001163F0 File Offset: 0x001145F0
		public static Vector3 GetVector3(float speed)
		{
			float x = Time.time * 0.01f * speed;
			return new Vector3(XftSmoothRandom.Get().HybridMultifractal(x, 15.73f, 0.58f), XftSmoothRandom.Get().HybridMultifractal(x, 63.94f, 0.58f), XftSmoothRandom.Get().HybridMultifractal(x, 0.2f, 0.58f));
		}

		// Token: 0x0600238A RID: 9098 RVA: 0x00116450 File Offset: 0x00114650
		public static Vector3 GetVector3Centered(float speed)
		{
			float x = Time.time * 0.01f * speed;
			float x2 = (Time.time - 1f) * 0.01f * speed;
			Vector3 vector;
			vector..ctor(XftSmoothRandom.Get().HybridMultifractal(x, 15.73f, 0.58f), XftSmoothRandom.Get().HybridMultifractal(x, 63.94f, 0.58f), XftSmoothRandom.Get().HybridMultifractal(x, 0.2f, 0.58f));
			Vector3 vector2;
			vector2..ctor(XftSmoothRandom.Get().HybridMultifractal(x2, 15.73f, 0.58f), XftSmoothRandom.Get().HybridMultifractal(x2, 63.94f, 0.58f), XftSmoothRandom.Get().HybridMultifractal(x2, 0.2f, 0.58f));
			return vector - vector2;
		}

		// Token: 0x0600238B RID: 9099 RVA: 0x00116514 File Offset: 0x00114714
		public static float Get(float speed)
		{
			float num = Time.time * 0.01f * speed;
			return XftSmoothRandom.Get().HybridMultifractal(num * 0.01f, 15.7f, 0.65f);
		}

		// Token: 0x0600238C RID: 9100 RVA: 0x00017A71 File Offset: 0x00015C71
		private static FractalNoise Get()
		{
			if (XftSmoothRandom.s_Noise == null)
			{
				XftSmoothRandom.s_Noise = new FractalNoise(1.27f, 2.04f, 8.36f);
			}
			return XftSmoothRandom.s_Noise;
		}

		// Token: 0x04002AF2 RID: 10994
		private static FractalNoise s_Noise;
	}
}
