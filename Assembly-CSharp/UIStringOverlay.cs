using System;
using System.Collections;
using System.Collections.Generic;
using Heluo.Wulin;
using UnityEngine;

// Token: 0x02000326 RID: 806
public class UIStringOverlay : MonoBehaviour
{
	// Token: 0x060011C4 RID: 4548 RVA: 0x00098570 File Offset: 0x00096770
	private void LateUpdate()
	{
		Camera main = Camera.main;
		if (main == null)
		{
			return;
		}
		if (!main.gameObject.activeSelf)
		{
			return;
		}
		int i = 0;
		while (i < this.overlayList.Count)
		{
			GameObjPlusOverlay gameObjPlusOverlay = this.overlayList[i];
			if (gameObjPlusOverlay.goActor != null && gameObjPlusOverlay.goOverlay != null)
			{
				Vector3 vector = gameObjPlusOverlay.goActor.transform.position;
				vector.y += 2f;
				vector = main.WorldToScreenPoint(vector);
				vector *= Game.UI.Root.pixelSizeAdjustment;
				if (!gameObjPlusOverlay.goOverlay.name.Contains("Battle"))
				{
					vector.y += (float)(gameObjPlusOverlay.goOverlay.GetComponent<UISprite>().height / 2);
					vector.x += (float)(gameObjPlusOverlay.goOverlay.GetComponent<UISprite>().width / 2);
				}
				gameObjPlusOverlay.goOverlay.transform.localPosition = vector;
				i++;
			}
			else
			{
				Object.Destroy(gameObjPlusOverlay.goOverlay);
				this.overlayList.RemoveAt(i);
			}
		}
	}

	// Token: 0x060011C5 RID: 4549 RVA: 0x000986B8 File Offset: 0x000968B8
	public void AddActorString(GameObject go, string Str)
	{
		GameObject gameObject = Object.Instantiate(this.goBaseOverlay) as GameObject;
		gameObject.transform.parent = this.goBaseOverlay.transform.parent;
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		gameObject.GetComponentInChildren<UILabel>().text = Str;
		foreach (GameObjPlusOverlay gameObjPlusOverlay in this.overlayList)
		{
			if (gameObjPlusOverlay.goActor == go)
			{
				this.overlayList.Remove(gameObjPlusOverlay);
				Object.Destroy(gameObjPlusOverlay.goOverlay);
				break;
			}
		}
		GameObjPlusOverlay gameObjPlusOverlay2 = new GameObjPlusOverlay();
		gameObjPlusOverlay2.goOverlay = gameObject;
		gameObjPlusOverlay2.goActor = go;
		this.overlayList.Add(gameObjPlusOverlay2);
		Object.Destroy(gameObject, 3f);
	}

	// Token: 0x060011C6 RID: 4550 RVA: 0x000987BC File Offset: 0x000969BC
	public void AddOneLineString(GameObject go, string Str, float fDestroyTime)
	{
		GameObject gameObject = Object.Instantiate(this.goOneLineOverlay) as GameObject;
		gameObject.transform.parent = this.goOneLineOverlay.transform.parent;
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		UILabel componentInChildren = gameObject.GetComponentInChildren<UILabel>();
		UISprite component = gameObject.GetComponent<UISprite>();
		componentInChildren.text = Str;
		component.width = componentInChildren.width + 8;
		component.height = componentInChildren.height + 8;
		foreach (GameObjPlusOverlay gameObjPlusOverlay in this.overlayList)
		{
			if (gameObjPlusOverlay.goActor == go)
			{
				this.overlayList.Remove(gameObjPlusOverlay);
				Object.Destroy(gameObjPlusOverlay.goOverlay);
				break;
			}
		}
		GameObjPlusOverlay gameObjPlusOverlay2 = new GameObjPlusOverlay();
		gameObjPlusOverlay2.goOverlay = gameObject;
		gameObjPlusOverlay2.goActor = go;
		this.overlayList.Add(gameObjPlusOverlay2);
		Object.Destroy(gameObject, fDestroyTime);
	}

	// Token: 0x060011C7 RID: 4551 RVA: 0x000988E8 File Offset: 0x00096AE8
	public void AlertShowImage(GameObject go, int i)
	{
		GameObject gameObject = Object.Instantiate(this.goBattleAlertImage) as GameObject;
		gameObject.transform.parent = this.goBattleAlertImage.transform.parent;
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		UISprite component = gameObject.GetComponent<UISprite>();
		foreach (GameObjPlusOverlay gameObjPlusOverlay in this.overlayList)
		{
			if (gameObjPlusOverlay.goActor == go)
			{
				this.overlayList.Remove(gameObjPlusOverlay);
				Object.Destroy(gameObjPlusOverlay.goOverlay);
				break;
			}
		}
		GameObjPlusOverlay gameObjPlusOverlay2 = new GameObjPlusOverlay();
		gameObjPlusOverlay2.goOverlay = gameObject;
		gameObjPlusOverlay2.goActor = go;
		component.width = 175;
		component.height = 80;
		this.overlayList.Add(gameObjPlusOverlay2);
		if (i == 1)
		{
			base.StartCoroutine(this.BattleAlertScale(component));
		}
		else
		{
			Object.Destroy(gameObject);
		}
	}

	// Token: 0x060011C8 RID: 4552 RVA: 0x00098A14 File Offset: 0x00096C14
	private IEnumerator BattleAlertScale(UISprite sprite)
	{
		yield return new WaitForSeconds(0.1f);
		sprite.width = 200;
		sprite.height = 100;
		yield break;
	}

	// Token: 0x04001574 RID: 5492
	public GameObject goBaseOverlay;

	// Token: 0x04001575 RID: 5493
	public GameObject goOneLineOverlay;

	// Token: 0x04001576 RID: 5494
	public GameObject goTalkOverlay;

	// Token: 0x04001577 RID: 5495
	private List<GameObjPlusOverlay> overlayList = new List<GameObjPlusOverlay>();

	// Token: 0x04001578 RID: 5496
	public GameObject m_TalkNoImage;

	// Token: 0x04001579 RID: 5497
	public GameObject goBattleAlertImage;
}
