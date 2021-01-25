using System;

// Token: 0x020002D1 RID: 721
public enum UnityLayer
{
	// Token: 0x040010DF RID: 4319
	Default = 1,
	// Token: 0x040010E0 RID: 4320
	TransparentFX,
	// Token: 0x040010E1 RID: 4321
	IgnoreRaycast = 4,
	// Token: 0x040010E2 RID: 4322
	Water = 16,
	// Token: 0x040010E3 RID: 4323
	UI = 32,
	// Token: 0x040010E4 RID: 4324
	Player = 256,
	// Token: 0x040010E5 RID: 4325
	Enemy = 512,
	// Token: 0x040010E6 RID: 4326
	Item = 1024,
	// Token: 0x040010E7 RID: 4327
	Ground = 2048,
	// Token: 0x040010E8 RID: 4328
	MinimapSign = 4096,
	// Token: 0x040010E9 RID: 4329
	Npc = 8192,
	// Token: 0x040010EA RID: 4330
	NonRender = 16384,
	// Token: 0x040010EB RID: 4331
	ColliderForHighlighting = 32768,
	// Token: 0x040010EC RID: 4332
	ColliderForPushCamera = 65536,
	// Token: 0x040010ED RID: 4333
	ColliderForDisapper = 131072,
	// Token: 0x040010EE RID: 4334
	HerdSim = 67108864,
	// Token: 0x040010EF RID: 4335
	ForReflection = 134217728
}
