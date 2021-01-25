using System;
using UnityEngine;

namespace AIBehaviorEditor
{
	// Token: 0x02000019 RID: 25
	public class AIBehaviorsStyles
	{
		// Token: 0x06000079 RID: 121 RVA: 0x00022DF0 File Offset: 0x00020FF0
		public AIBehaviorsStyles()
		{
			this.InitStyles();
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600007A RID: 122 RVA: 0x000029B0 File Offset: 0x00000BB0
		public int arrowButtonWidths
		{
			get
			{
				return 18;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600007B RID: 123 RVA: 0x000029B4 File Offset: 0x00000BB4
		public int addRemoveButtonWidths
		{
			get
			{
				return 16;
			}
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00022E40 File Offset: 0x00021040
		private void InitStyles()
		{
			this.upStyle.normal.background = this.LoadButtonImage("up");
			this.upStyle.active.background = this.LoadButtonImage("up_rollover");
			this.downStyle.normal.background = this.LoadButtonImage("down");
			this.downStyle.active.background = this.LoadButtonImage("down_rollover");
			this.addStyle.normal.background = this.LoadButtonImage("add");
			this.addStyle.active.background = this.LoadButtonImage("add_rollover");
			this.removeStyle.normal.background = this.LoadButtonImage("remove");
			this.removeStyle.active.background = this.LoadButtonImage("remove_rollover");
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00022F28 File Offset: 0x00021128
		private Texture2D LoadButtonImage(string imageName)
		{
			string text = "Assets/AIBehavior/Editor/AIBehaviorsMadeEasy/Images/" + imageName + ".png";
			return Resources.LoadAssetAtPath(text, typeof(Texture2D)) as Texture2D;
		}

		// Token: 0x04000050 RID: 80
		public GUIContent blankContent = new GUIContent();

		// Token: 0x04000051 RID: 81
		public GUIStyle upStyle = new GUIStyle();

		// Token: 0x04000052 RID: 82
		public GUIStyle downStyle = new GUIStyle();

		// Token: 0x04000053 RID: 83
		public GUIStyle addStyle = new GUIStyle();

		// Token: 0x04000054 RID: 84
		public GUIStyle removeStyle = new GUIStyle();
	}
}
