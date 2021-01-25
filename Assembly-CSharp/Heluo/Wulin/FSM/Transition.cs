using System;

namespace Heluo.Wulin.FSM
{
	// Token: 0x020001A3 RID: 419
	public class Transition
	{
		// Token: 0x060008C6 RID: 2246 RVA: 0x0000747E File Offset: 0x0000567E
		public Transition()
		{
			this.Enable = true;
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x060008C7 RID: 2247 RVA: 0x0000748D File Offset: 0x0000568D
		// (set) Token: 0x060008C8 RID: 2248 RVA: 0x00007495 File Offset: 0x00005695
		public string Name { get; set; }

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x060008C9 RID: 2249 RVA: 0x0000749E File Offset: 0x0000569E
		// (set) Token: 0x060008CA RID: 2250 RVA: 0x000074A6 File Offset: 0x000056A6
		public IState Prev { get; set; }

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x060008CB RID: 2251 RVA: 0x000074AF File Offset: 0x000056AF
		// (set) Token: 0x060008CC RID: 2252 RVA: 0x000074B7 File Offset: 0x000056B7
		public IState NextState { get; set; }

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x060008CD RID: 2253 RVA: 0x000074C0 File Offset: 0x000056C0
		// (set) Token: 0x060008CE RID: 2254 RVA: 0x000074C8 File Offset: 0x000056C8
		public bool Enable { get; set; }

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x060008CF RID: 2255 RVA: 0x000074D1 File Offset: 0x000056D1
		// (set) Token: 0x060008D0 RID: 2256 RVA: 0x000074D9 File Offset: 0x000056D9
		public virtual bool Triggered { get; set; }
	}
}
