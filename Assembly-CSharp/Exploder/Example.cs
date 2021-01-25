using System;
using UnityEngine;

namespace Exploder
{
	// Token: 0x020000AC RID: 172
	public class Example : MonoBehaviour
	{
		// Token: 0x060003A7 RID: 935 RVA: 0x00004849 File Offset: 0x00002A49
		public void ExplodeObject(GameObject obj)
		{
			this.Exploder.Explode(new ExploderObject.OnExplosion(this.OnExplosion));
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x0000475B File Offset: 0x0000295B
		private void OnExplosion(float time, ExploderObject.ExplosionState state)
		{
			if (state == ExploderObject.ExplosionState.ExplosionFinished)
			{
			}
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x00004862 File Offset: 0x00002A62
		private void CrackAndExplodeObject(GameObject obj)
		{
			this.Exploder.Crack(new ExploderObject.OnCracked(this.OnCracked));
		}

		// Token: 0x060003AA RID: 938 RVA: 0x0000487B File Offset: 0x00002A7B
		private void OnCracked()
		{
			this.Exploder.ExplodeCracked(new ExploderObject.OnExplosion(this.OnExplosion));
		}

		// Token: 0x040002DD RID: 733
		public ExploderObject Exploder;
	}
}
