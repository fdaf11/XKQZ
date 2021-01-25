using System;
using SWS;
using UnityEngine;

// Token: 0x020006A6 RID: 1702
public class RuntimeDemo : MonoBehaviour
{
	// Token: 0x06002942 RID: 10562 RVA: 0x0001B266 File Offset: 0x00019466
	private void OnGUI()
	{
		this.DrawExample1();
		this.DrawExample2();
		this.DrawExample3();
		this.DrawExample4();
		this.DrawExample5();
		this.DrawExample6();
	}

	// Token: 0x06002943 RID: 10563 RVA: 0x00146FB0 File Offset: 0x001451B0
	private void DrawExample1()
	{
		GUI.Label(new Rect(10f, 10f, 20f, 20f), "1:");
		string text = "Walker (Path1)";
		string text2 = "Path1 (Instantiation)";
		Vector3 vector;
		vector..ctor(-25f, 0f, 10f);
		if (!this.example1.done && GUI.Button(new Rect(30f, 10f, 100f, 20f), "Instantiate"))
		{
			GameObject gameObject = (GameObject)Object.Instantiate(this.example1.walkerPrefab, vector, Quaternion.identity);
			gameObject.name = text;
			GameObject gameObject2 = (GameObject)Object.Instantiate(this.example1.pathPrefab, vector, Quaternion.identity);
			gameObject2.name = text2;
			WaypointManager.AddPath(gameObject2);
			gameObject.GetComponent<splineMove>().SetPath(WaypointManager.Paths[text2]);
			this.example1.done = true;
		}
		if (this.example1.done && GUI.Button(new Rect(30f, 10f, 100f, 20f), "Destroy"))
		{
			Object.Destroy(GameObject.Find(text));
			Object.Destroy(GameObject.Find(text2));
			WaypointManager.Paths.Remove(text2);
			this.example1.done = false;
		}
	}

	// Token: 0x06002944 RID: 10564 RVA: 0x00147114 File Offset: 0x00145314
	private void DrawExample2()
	{
		GUI.Label(new Rect(10f, 30f, 20f, 20f), "2:");
		if (GUI.Button(new Rect(30f, 30f, 100f, 20f), "Switch Path"))
		{
			string name = this.example2.moveRef.pathContainer.name;
			this.example2.moveRef.moveToPath = true;
			if (name == this.example2.pathName1)
			{
				this.example2.moveRef.SetPath(WaypointManager.Paths[this.example2.pathName2]);
			}
			else
			{
				this.example2.moveRef.SetPath(WaypointManager.Paths[this.example2.pathName1]);
			}
		}
	}

	// Token: 0x06002945 RID: 10565 RVA: 0x001471F8 File Offset: 0x001453F8
	private void DrawExample3()
	{
		GUI.Label(new Rect(10f, 50f, 20f, 20f), "3:");
		if (this.example3.moveRef.tween == null && GUI.Button(new Rect(30f, 50f, 100f, 20f), "Start"))
		{
			this.example3.moveRef.StartMove();
		}
		if (this.example3.moveRef.tween != null && GUI.Button(new Rect(30f, 50f, 100f, 20f), "Stop"))
		{
			this.example3.moveRef.Stop();
		}
		if (this.example3.moveRef.tween != null && GUI.Button(new Rect(30f, 70f, 100f, 20f), "Reset"))
		{
			this.example3.moveRef.ResetMove();
		}
	}

	// Token: 0x06002946 RID: 10566 RVA: 0x00147310 File Offset: 0x00145510
	private void DrawExample4()
	{
		GUI.Label(new Rect(10f, 90f, 20f, 20f), "4:");
		if (this.example4.moveRef.tween != null && !this.example4.moveRef.tween.isPaused && GUI.Button(new Rect(30f, 90f, 100f, 20f), "Pause"))
		{
			this.example4.moveRef.Pause();
		}
		if (this.example4.moveRef.tween != null && this.example4.moveRef.tween.isPaused && GUI.Button(new Rect(30f, 90f, 100f, 20f), "Resume"))
		{
			this.example4.moveRef.Resume();
		}
	}

	// Token: 0x06002947 RID: 10567 RVA: 0x00147410 File Offset: 0x00145610
	private void DrawExample5()
	{
		GUI.Label(new Rect(10f, 110f, 20f, 20f), "5:");
		if (GUI.Button(new Rect(30f, 110f, 100f, 20f), "Change Speed"))
		{
			float speed = this.example5.moveRef.speed;
			float num = 1.5f;
			if (speed == num)
			{
				num = 4f;
			}
			this.example5.moveRef.ChangeSpeed(num);
		}
	}

	// Token: 0x06002948 RID: 10568 RVA: 0x001474A0 File Offset: 0x001456A0
	private void DrawExample6()
	{
		GUI.Label(new Rect(10f, 130f, 20f, 20f), "6:");
		if (!this.example6.done && GUI.Button(new Rect(30f, 130f, 100f, 20f), "Add Message"))
		{
			Messages messages = this.example6.moveRef.messages;
			MessageOptions messageOption = messages.GetMessageOption(1);
			messageOption.message.Add("ActivateForTime");
			messageOption.type.Add(MessageOptions.ValueType.Object);
			messageOption.obj.Add(this.example6.target);
			messages.FillOptionWithValues(messageOption);
			this.example6.done = true;
		}
	}

	// Token: 0x0400343B RID: 13371
	public RuntimeDemo.ExampleClass1 example1;

	// Token: 0x0400343C RID: 13372
	public RuntimeDemo.ExampleClass2 example2;

	// Token: 0x0400343D RID: 13373
	public RuntimeDemo.ExampleClass3 example3;

	// Token: 0x0400343E RID: 13374
	public RuntimeDemo.ExampleClass4 example4;

	// Token: 0x0400343F RID: 13375
	public RuntimeDemo.ExampleClass5 example5;

	// Token: 0x04003440 RID: 13376
	public RuntimeDemo.ExampleClass6 example6;

	// Token: 0x020006A7 RID: 1703
	[Serializable]
	public class ExampleClass1
	{
		// Token: 0x04003441 RID: 13377
		public GameObject walkerPrefab;

		// Token: 0x04003442 RID: 13378
		public GameObject pathPrefab;

		// Token: 0x04003443 RID: 13379
		public bool done;
	}

	// Token: 0x020006A8 RID: 1704
	[Serializable]
	public class ExampleClass2
	{
		// Token: 0x04003444 RID: 13380
		public splineMove moveRef;

		// Token: 0x04003445 RID: 13381
		public string pathName1;

		// Token: 0x04003446 RID: 13382
		public string pathName2;
	}

	// Token: 0x020006A9 RID: 1705
	[Serializable]
	public class ExampleClass3
	{
		// Token: 0x04003447 RID: 13383
		public splineMove moveRef;
	}

	// Token: 0x020006AA RID: 1706
	[Serializable]
	public class ExampleClass4
	{
		// Token: 0x04003448 RID: 13384
		public splineMove moveRef;
	}

	// Token: 0x020006AB RID: 1707
	[Serializable]
	public class ExampleClass5
	{
		// Token: 0x04003449 RID: 13385
		public splineMove moveRef;
	}

	// Token: 0x020006AC RID: 1708
	[Serializable]
	public class ExampleClass6
	{
		// Token: 0x0400344A RID: 13386
		public splineMove moveRef;

		// Token: 0x0400344B RID: 13387
		public GameObject target;

		// Token: 0x0400344C RID: 13388
		public bool done;
	}
}
