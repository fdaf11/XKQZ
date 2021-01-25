using System;

namespace Heluo.Wulin
{
	// Token: 0x02000260 RID: 608
	public enum TalentEffect
	{
		// Token: 0x04000CA8 RID: 3240
		MaxHP = 1,
		// Token: 0x04000CA9 RID: 3241
		MaxSP = 3,
		// Token: 0x04000CAA RID: 3242
		MoveStep,
		// Token: 0x04000CAB RID: 3243
		StrengthMax = 15,
		// Token: 0x04000CAC RID: 3244
		ConstitutionMax,
		// Token: 0x04000CAD RID: 3245
		IntelligenceMax,
		// Token: 0x04000CAE RID: 3246
		DexterityMax,
		// Token: 0x04000CAF RID: 3247
		ALLMartialExp = 20,
		// Token: 0x04000CB0 RID: 3248
		UseBlade = 22,
		// Token: 0x04000CB1 RID: 3249
		UseSword = 21,
		// Token: 0x04000CB2 RID: 3250
		UseArrow = 23,
		// Token: 0x04000CB3 RID: 3251
		UseFist,
		// Token: 0x04000CB4 RID: 3252
		UseGas,
		// Token: 0x04000CB5 RID: 3253
		UseRope,
		// Token: 0x04000CB6 RID: 3254
		UseWhip,
		// Token: 0x04000CB7 RID: 3255
		UsePike,
		// Token: 0x04000CB8 RID: 3256
		Negiong,
		// Token: 0x04000CB9 RID: 3257
		ALLMartialMax,
		// Token: 0x04000CBA RID: 3258
		BladeMax = 32,
		// Token: 0x04000CBB RID: 3259
		SwordMax = 31,
		// Token: 0x04000CBC RID: 3260
		ArrowMax = 33,
		// Token: 0x04000CBD RID: 3261
		FistMax,
		// Token: 0x04000CBE RID: 3262
		GasMax,
		// Token: 0x04000CBF RID: 3263
		RopeMax,
		// Token: 0x04000CC0 RID: 3264
		WhipMax,
		// Token: 0x04000CC1 RID: 3265
		PikeMax,
		// Token: 0x04000CC2 RID: 3266
		Dodge = 51,
		// Token: 0x04000CC3 RID: 3267
		Counter,
		// Token: 0x04000CC4 RID: 3268
		Critical,
		// Token: 0x04000CC5 RID: 3269
		DefendCounter = 56,
		// Token: 0x04000CC6 RID: 3270
		DefendDodge = 55,
		// Token: 0x04000CC7 RID: 3271
		DefendCritical = 57,
		// Token: 0x04000CC8 RID: 3272
		AllWeapon = 60,
		// Token: 0x04000CC9 RID: 3273
		Sword,
		// Token: 0x04000CCA RID: 3274
		Blade,
		// Token: 0x04000CCB RID: 3275
		Arrow,
		// Token: 0x04000CCC RID: 3276
		Fist,
		// Token: 0x04000CCD RID: 3277
		Gas,
		// Token: 0x04000CCE RID: 3278
		Rope,
		// Token: 0x04000CCF RID: 3279
		Whip,
		// Token: 0x04000CD0 RID: 3280
		Pike,
		// Token: 0x04000CD1 RID: 3281
		Stealth = 100,
		// Token: 0x04000CD2 RID: 3282
		SelfRemainTacticAll,
		// Token: 0x04000CD3 RID: 3283
		SelfRemainTacticEnemy,
		// Token: 0x04000CD4 RID: 3284
		SelfRemainTacticFriend,
		// Token: 0x04000CD5 RID: 3285
		Steal,
		// Token: 0x04000CD6 RID: 3286
		TeamMateRemainTactic,
		// Token: 0x04000CD7 RID: 3287
		AutoQuitBattle,
		// Token: 0x04000CD8 RID: 3288
		BuddhaMercy,
		// Token: 0x04000CD9 RID: 3289
		AttackFromFace,
		// Token: 0x04000CDA RID: 3290
		AttackFromSide,
		// Token: 0x04000CDB RID: 3291
		AttackFromBack,
		// Token: 0x04000CDC RID: 3292
		LastOneUnit,
		// Token: 0x04000CDD RID: 3293
		LowHPHighDamageUp,
		// Token: 0x04000CDE RID: 3294
		LowSPHighDamageUp,
		// Token: 0x04000CDF RID: 3295
		AroundEnemyDamageUp,
		// Token: 0x04000CE0 RID: 3296
		AroundFriendDamageUp,
		// Token: 0x04000CE1 RID: 3297
		AroundMaleDamageUp,
		// Token: 0x04000CE2 RID: 3298
		AroundFemaleDamageUp,
		// Token: 0x04000CE3 RID: 3299
		CounterAttackTactic,
		// Token: 0x04000CE4 RID: 3300
		BeCriticalTactic,
		// Token: 0x04000CE5 RID: 3301
		ItemEffectPlus,
		// Token: 0x04000CE6 RID: 3302
		WeaponeEffectPlus,
		// Token: 0x04000CE7 RID: 3303
		WristEffectPlus,
		// Token: 0x04000CE8 RID: 3304
		TeamManaCost,
		// Token: 0x04000CE9 RID: 3305
		EnemyManaCost,
		// Token: 0x04000CEA RID: 3306
		FirstAction,
		// Token: 0x04000CEB RID: 3307
		RecoverHP,
		// Token: 0x04000CEC RID: 3308
		RecoverSP,
		// Token: 0x04000CED RID: 3309
		NightFragrance,
		// Token: 0x04000CEE RID: 3310
		SelfRemainEyeToEye,
		// Token: 0x04000CEF RID: 3311
		SelfRemainRendomEnemyConfuse,
		// Token: 0x04000CF0 RID: 3312
		EatSomthing,
		// Token: 0x04000CF1 RID: 3313
		KillOnePlusBuff,
		// Token: 0x04000CF2 RID: 3314
		NearMaleDamageUp,
		// Token: 0x04000CF3 RID: 3315
		NearFemaleDamageUp,
		// Token: 0x04000CF4 RID: 3316
		DrinkSomthing,
		// Token: 0x04000CF5 RID: 3317
		TeamMateRemainBuddyJoin,
		// Token: 0x04000CF6 RID: 3318
		AssistAttack,
		// Token: 0x04000CF7 RID: 3319
		PinkSkeleton,
		// Token: 0x04000CF8 RID: 3320
		TeamItemCD,
		// Token: 0x04000CF9 RID: 3321
		EnemyItemCD,
		// Token: 0x04000CFA RID: 3322
		MoneyAttack,
		// Token: 0x04000CFB RID: 3323
		StroeItemDiscounts = 200,
		// Token: 0x04000CFC RID: 3324
		AuctionMaster,
		// Token: 0x04000CFD RID: 3325
		Observant,
		// Token: 0x04000CFE RID: 3326
		AvoidAlert,
		// Token: 0x04000CFF RID: 3327
		SharpEye,
		// Token: 0x04000D00 RID: 3328
		Plunder,
		// Token: 0x04000D01 RID: 3329
		MoreExperiences,
		// Token: 0x04000D02 RID: 3330
		MoreAttributePoints,
		// Token: 0x04000D03 RID: 3331
		ExploreHerbs,
		// Token: 0x04000D04 RID: 3332
		ExploreMine,
		// Token: 0x04000D05 RID: 3333
		ExplorePoison,
		// Token: 0x04000D06 RID: 3334
		TeamLast
	}
}
