using System;
using System.Collections.Generic;
using Heluo.Wulin.UI;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x02000125 RID: 293
	public class BigMapNode : PlayerTarget
	{
		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060005FA RID: 1530 RVA: 0x00005770 File Offset: 0x00003970
		public Vector3 OutPosition
		{
			get
			{
				if (this.m_Transform == null)
				{
					this.m_Transform = base.transform;
				}
				return this.m_Transform.position + this.Offset;
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060005FB RID: 1531 RVA: 0x000057A5 File Offset: 0x000039A5
		public Vector3 Pos
		{
			get
			{
				if (this.m_Transform == null)
				{
					this.m_Transform = base.transform;
				}
				return this.m_Transform.position;
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060005FC RID: 1532 RVA: 0x000057CF File Offset: 0x000039CF
		public float Range
		{
			get
			{
				if (this.m_Collider == null)
				{
					this.m_Collider = base.gameObject.GetComponent<SphereCollider>();
				}
				return this.m_Collider.radius;
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060005FE RID: 1534 RVA: 0x00005807 File Offset: 0x00003A07
		// (set) Token: 0x060005FD RID: 1533 RVA: 0x000057FE File Offset: 0x000039FE
		public MapIDNode MapIDNode { get; private set; }

		// Token: 0x060005FF RID: 1535 RVA: 0x0000580F File Offset: 0x00003A0F
		private void Awake()
		{
			this.m_TargetType = PlayerTarget.eTargetType.BigMapNode;
			base.AddPlayerTargetList(this);
		}

		// Token: 0x06000600 RID: 1536 RVA: 0x00043520 File Offset: 0x00041720
		private void Start()
		{
			if (!BigMapNode.BigMapNodeList.ContainsKey(this.NodeID))
			{
				BigMapNode.BigMapNodeList.Add(this.NodeID, this);
			}
			this.MapIDNode = Game.MapID.GetMapIDNode(this.NodeID);
			if (this.MapIDNode == null)
			{
				GameDebugTool.Log("No Map Data");
				return;
			}
			this.NodeName = this.MapIDNode.m_strMapName;
			this.m_bOpen = true;
			if (this.MapIDNode.m_OpenCdn.Count != 0)
			{
				this.m_bOpen = ConditionManager.CheckCondition(this.MapIDNode.m_OpenCdn, this.MapIDNode.IsAllOpenCdn, 0, string.Empty);
			}
			bool flag = false;
			if (this.MapIDNode.m_CloseCdn.Count != 0)
			{
				flag = ConditionManager.CheckCondition(this.MapIDNode.m_CloseCdn, this.MapIDNode.IsAllCloseCdn, 0, string.Empty);
			}
			if (flag)
			{
				this.m_bOpen = false;
			}
		}

		// Token: 0x06000601 RID: 1537 RVA: 0x0004361C File Offset: 0x0004181C
		public void InitData()
		{
			if (!BigMapNode.BigMapNodeList.ContainsKey(this.NodeID))
			{
				BigMapNode.BigMapNodeList.Add(this.NodeID, this);
			}
			this.MapIDNode = Game.MapID.GetMapIDNode(this.NodeID);
			if (this.MapIDNode == null)
			{
				GameDebugTool.Log("No Map Data");
				return;
			}
			this.NodeName = this.MapIDNode.m_strMapName;
			this.m_bOpen = true;
			if (this.MapIDNode.m_OpenCdn.Count != 0)
			{
				this.m_bOpen = ConditionManager.CheckCondition(this.MapIDNode.m_OpenCdn, true, 0, string.Empty);
			}
			bool flag = false;
			if (this.MapIDNode.m_CloseCdn.Count != 0)
			{
				flag = ConditionManager.CheckCondition(this.MapIDNode.m_CloseCdn, true, 0, string.Empty);
			}
			if (flag)
			{
				this.m_bOpen = false;
			}
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x00043704 File Offset: 0x00041904
		private void OnTriggerEnter(Collider other)
		{
			if (!this.m_bOpen)
			{
				return;
			}
			if (GameGlobal.m_bCFormOpen || GameGlobal.m_bPlayerTalk)
			{
				return;
			}
			if (other.tag == "Player")
			{
				GameDebugTool.Log("你撞到 : " + this.NodeName);
				UIBigMapEnter uibigMapEnter = Game.UI.Get<UIBigMapEnter>();
				if (uibigMapEnter == null)
				{
					Game.UI.CreateUI("cFormBigMapEnter");
					uibigMapEnter = Game.UI.Get<UIBigMapEnter>();
				}
				uibigMapEnter.SetUI(this);
				uibigMapEnter.Show();
				this.setBigMapCircle(true);
			}
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x000437A4 File Offset: 0x000419A4
		private void setBigMapCircle(bool show)
		{
			if (BigMapController.BigMapCircle == null && Game.g_EffectsBundle.Contains("effects/BigMapCircle"))
			{
				GameObject gameObject = Game.g_EffectsBundle.Load("effects/BigMapCircle") as GameObject;
				BigMapController.BigMapCircle = (Object.Instantiate(gameObject) as GameObject);
			}
			BigMapController.BigMapCircle.transform.position = base.transform.position + Vector3.up * 10f;
			BigMapController.BigMapCircle.SetActive(show);
		}

		// Token: 0x06000604 RID: 1540 RVA: 0x00043834 File Offset: 0x00041A34
		private void OnTriggerExit(Collider other)
		{
			if (!this.m_bOpen)
			{
				return;
			}
			if (other.tag == "Player")
			{
				GameDebugTool.Log("你離開 : " + this.NodeName);
				UIBigMapEnter uibigMapEnter = Game.UI.Get<UIBigMapEnter>();
				if (uibigMapEnter == null)
				{
					Game.UI.CreateUI("cFormBigMapEnter");
					uibigMapEnter = Game.UI.Get<UIBigMapEnter>();
				}
				uibigMapEnter.Hide();
				this.setBigMapCircle(false);
			}
		}

		// Token: 0x06000605 RID: 1541 RVA: 0x0000581F File Offset: 0x00003A1F
		private void OnDestroy()
		{
			base.RemovePlayerTargetList(this);
			if (BigMapNode.BigMapNodeList.ContainsKey(this.NodeID))
			{
				BigMapNode.BigMapNodeList.Remove(this.NodeID);
			}
		}

		// Token: 0x04000673 RID: 1651
		public static Dictionary<string, BigMapNode> BigMapNodeList = new Dictionary<string, BigMapNode>();

		// Token: 0x04000674 RID: 1652
		public Vector3 Offset = new Vector3(1f, 0f, 1f);

		// Token: 0x04000675 RID: 1653
		private Transform m_Transform;

		// Token: 0x04000676 RID: 1654
		private SphereCollider m_Collider;

		// Token: 0x04000677 RID: 1655
		public string NodeID;

		// Token: 0x04000678 RID: 1656
		public string NodeName;

		// Token: 0x04000679 RID: 1657
		public bool m_bOpen;
	}
}
