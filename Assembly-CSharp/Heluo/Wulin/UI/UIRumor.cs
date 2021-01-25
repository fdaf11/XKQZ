using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x0200031E RID: 798
	public class UIRumor : UILayer
	{
		// Token: 0x06001157 RID: 4439 RVA: 0x0000A503 File Offset: 0x00008703
		protected override void Awake()
		{
			base.Awake();
		}

		// Token: 0x06001158 RID: 4440 RVA: 0x0000264F File Offset: 0x0000084F
		private void Start()
		{
		}

		// Token: 0x06001159 RID: 4441 RVA: 0x0000B4D9 File Offset: 0x000096D9
		public void ClearRumor()
		{
			this.m_StrMsgQueue.Clear();
		}

		// Token: 0x0600115A RID: 4442 RVA: 0x000954DC File Offset: 0x000936DC
		protected override void AssignControl(Transform sender)
		{
			string name = sender.name;
			if (name != null)
			{
				if (UIRumor.<>f__switch$map16 == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(7);
					dictionary.Add("RumorSprite", 0);
					dictionary.Add("Content", 1);
					dictionary.Add("StrMsgSprite", 2);
					dictionary.Add("DelayTime", 3);
					dictionary.Add("CharacterTexture", 4);
					dictionary.Add("Name", 5);
					dictionary.Add("StrMsg", 6);
					UIRumor.<>f__switch$map16 = dictionary;
				}
				int num;
				if (UIRumor.<>f__switch$map16.TryGetValue(name, ref num))
				{
					switch (num)
					{
					case 0:
						this.m_RumorSprite = sender;
						break;
					case 1:
						this.m_Content = sender;
						break;
					case 2:
						this.m_StrMsgSprite = sender;
						break;
					case 3:
						this.m_DelayTime = sender;
						break;
					case 4:
						this.m_CharacterTexture = sender;
						break;
					case 5:
						this.m_Name = sender;
						break;
					case 6:
						this.m_StrMsg = sender;
						break;
					}
				}
			}
		}

		// Token: 0x0600115B RID: 4443 RVA: 0x0000B4E6 File Offset: 0x000096E6
		public override void ClearUIData()
		{
			this.m_StrMsgQueue.Clear();
			this.SetActive(false);
		}

		// Token: 0x0600115C RID: 4444 RVA: 0x0000B4FA File Offset: 0x000096FA
		private void DoNextTrue()
		{
			this.doNext = true;
			this.m_NowRumor = null;
		}

		// Token: 0x0600115D RID: 4445 RVA: 0x0000264F File Offset: 0x0000084F
		private void OnEnable()
		{
		}

		// Token: 0x0600115E RID: 4446 RVA: 0x0000B50A File Offset: 0x0000970A
		public void AddStrMsg(Rumor rumor, SaveRumor SR)
		{
			if (GameGlobal.m_bDLCMode)
			{
				return;
			}
			Game.UI.Get<UISaveRumor>().AddRumorList(rumor);
			Save.m_Instance.AddSaveRumor(SR);
			this.m_StrMsgQueue.Enqueue(rumor);
			this.lockUpdate = true;
		}

		// Token: 0x0600115F RID: 4447 RVA: 0x00095614 File Offset: 0x00093814
		public void ShowRumor()
		{
			this.SetActive(true);
			base.animation.Stop("Rumor");
			base.animation.Play("Rumor");
			if (this.m_NowRumor == null)
			{
				this.m_NowRumor = (this.m_StrMsgQueue.Dequeue() as Rumor);
			}
			Texture texture = Game.g_BigHeadBundle.Load("2dtexture/gameui/bighead/" + this.m_NowRumor.m_strImageId) as Texture;
			if (texture == null)
			{
				texture = (Game.g_BigHeadBundle.Load("2dtexture/gameui/bighead/B000001") as Texture);
			}
			this.m_CharacterTexture.Texture = texture;
			this.m_Content.Text = this.m_NowRumor.m_strTip;
			this.m_Name.Text = this.m_NowRumor.m_strName;
			this.m_StrMsg.Text = this.m_NowRumor.m_strLine;
			this.doNext = false;
		}

		// Token: 0x06001160 RID: 4448 RVA: 0x00095708 File Offset: 0x00093908
		private void SetActive(bool bActive)
		{
			this.m_RumorSprite.GameObject.SetActive(bActive);
			this.m_Content.GameObject.SetActive(bActive);
			this.m_StrMsgSprite.GameObject.SetActive(bActive);
			this.m_DelayTime.GameObject.SetActive(bActive);
		}

		// Token: 0x06001161 RID: 4449 RVA: 0x0009575C File Offset: 0x0009395C
		private void Update()
		{
			if (this.lockUpdate)
			{
				this.lockUpdate = false;
				return;
			}
			if (!base.animation.isPlaying && !GameGlobal.m_bLoading && !GameGlobal.m_bBattle && !GameGlobal.m_bMovie)
			{
				if (this.m_StrMsgQueue.Count > 0)
				{
					this.ShowRumor();
					Debug.Log("ShowRumor");
				}
				else
				{
					this.SetActive(false);
				}
			}
		}

		// Token: 0x04001505 RID: 5381
		private Control m_Content;

		// Token: 0x04001506 RID: 5382
		private Control m_RumorSprite;

		// Token: 0x04001507 RID: 5383
		private Control m_StrMsgSprite;

		// Token: 0x04001508 RID: 5384
		private Control m_DelayTime;

		// Token: 0x04001509 RID: 5385
		private Control m_CharacterTexture;

		// Token: 0x0400150A RID: 5386
		private Control m_Name;

		// Token: 0x0400150B RID: 5387
		private Control m_StrMsg;

		// Token: 0x0400150C RID: 5388
		private Queue m_StrMsgQueue = new Queue();

		// Token: 0x0400150D RID: 5389
		private Rumor m_NowRumor;

		// Token: 0x0400150E RID: 5390
		private bool doNext = true;

		// Token: 0x0400150F RID: 5391
		private bool lockUpdate;
	}
}
