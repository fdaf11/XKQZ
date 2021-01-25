using System;
using UnityEngine;

namespace HeLuo.Framework.Flow
{
	// Token: 0x02000188 RID: 392
	[Description("Flow Graph")]
	public class FlowGraph : NodeGraph
	{
		// Token: 0x060007D4 RID: 2004 RVA: 0x00006D19 File Offset: 0x00004F19
		public FlowGraph()
		{
			base.AddVariableIfNotFound<FlowBehaviour>("root");
		}

		// Token: 0x060007D5 RID: 2005 RVA: 0x00049544 File Offset: 0x00047744
		public void RegisterEvent()
		{
			for (int i = 0; i < this.nodes.Count; i++)
			{
				OutputNode outputNode = this.nodes[i];
				if (outputNode is ObjectEventNode)
				{
					ObjectEventNode objectEventNode = outputNode as ObjectEventNode;
					objectEventNode.GetValue();
				}
			}
		}

		// Token: 0x060007D6 RID: 2006 RVA: 0x00049598 File Offset: 0x00047798
		public void UnregisterEvent()
		{
			for (int i = 0; i < this.nodes.Count; i++)
			{
				OutputNode outputNode = this.nodes[i];
				if (outputNode is ObjectEventNode)
				{
					ObjectEventNode objectEventNode = outputNode as ObjectEventNode;
					objectEventNode.Unregister();
				}
			}
		}

		// Token: 0x060007D7 RID: 2007 RVA: 0x00006D2D File Offset: 0x00004F2D
		protected override void OnDestroy()
		{
			base.OnDestroy();
			this.UnregisterEvent();
		}

		// Token: 0x060007D8 RID: 2008 RVA: 0x000495EC File Offset: 0x000477EC
		public override object Clone()
		{
			FlowGraph flowGraph = ScriptableObject.CreateInstance<FlowGraph>();
			try
			{
				SerializeBundle serializeBundle = base.CopySerializeBundle();
				flowGraph.nodes = serializeBundle.nodes;
				flowGraph.variables = serializeBundle.variables;
				flowGraph.output = serializeBundle.output;
				flowGraph.objectReference = this.objectReference;
				flowGraph.name = base.name + "(Copy)";
			}
			catch (Exception ex)
			{
				Debug.LogError(base.name);
				Debug.LogException(ex);
				return null;
			}
			return flowGraph;
		}
	}
}
