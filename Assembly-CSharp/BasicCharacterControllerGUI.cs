using System;
using UnityEngine;

// Token: 0x0200084D RID: 2125
public class BasicCharacterControllerGUI : MonoBehaviour
{
	// Token: 0x06003389 RID: 13193 RVA: 0x00020715 File Offset: 0x0001E915
	private void OnGUI()
	{
		GUILayout.TextArea("This is a small Sample Scene, showing how you might seamlessly transition from a Dynamic Gameplay camera into a cutscene.", new GUILayoutOption[0]);
		GUILayout.Space(10f);
		GUILayout.TextArea("W, A, S and D : Move Character.", new GUILayoutOption[0]);
		GUILayout.TextArea("Move into the RED trigger volume to trigger the small sequence.", new GUILayoutOption[0]);
	}
}
