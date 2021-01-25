using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000883 RID: 2179
public class iTween : MonoBehaviour
{
	// Token: 0x06003476 RID: 13430 RVA: 0x00020E46 File Offset: 0x0001F046
	public static void Init(GameObject target)
	{
		iTween.MoveBy(target, Vector3.zero, 0f);
	}

	// Token: 0x06003477 RID: 13431 RVA: 0x0019529C File Offset: 0x0019349C
	public static void CameraFadeFrom(float amount, float time)
	{
		if (iTween.cameraFade)
		{
			iTween.CameraFadeFrom(iTween.Hash(new object[]
			{
				"amount",
				amount,
				"time",
				time
			}));
		}
		else
		{
			Debug.LogError("iTween Error: You must first add a camera fade object with CameraFadeAdd() before atttempting to use camera fading.");
		}
	}

	// Token: 0x06003478 RID: 13432 RVA: 0x00020E58 File Offset: 0x0001F058
	public static void CameraFadeFrom(Hashtable args)
	{
		if (iTween.cameraFade)
		{
			iTween.ColorFrom(iTween.cameraFade, args);
		}
		else
		{
			Debug.LogError("iTween Error: You must first add a camera fade object with CameraFadeAdd() before atttempting to use camera fading.");
		}
	}

	// Token: 0x06003479 RID: 13433 RVA: 0x001952FC File Offset: 0x001934FC
	public static void CameraFadeTo(float amount, float time)
	{
		if (iTween.cameraFade)
		{
			iTween.CameraFadeTo(iTween.Hash(new object[]
			{
				"amount",
				amount,
				"time",
				time
			}));
		}
		else
		{
			Debug.LogError("iTween Error: You must first add a camera fade object with CameraFadeAdd() before atttempting to use camera fading.");
		}
	}

	// Token: 0x0600347A RID: 13434 RVA: 0x00020E83 File Offset: 0x0001F083
	public static void CameraFadeTo(Hashtable args)
	{
		if (iTween.cameraFade)
		{
			iTween.ColorTo(iTween.cameraFade, args);
		}
		else
		{
			Debug.LogError("iTween Error: You must first add a camera fade object with CameraFadeAdd() before atttempting to use camera fading.");
		}
	}

	// Token: 0x0600347B RID: 13435 RVA: 0x0019535C File Offset: 0x0019355C
	public static void ValueTo(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		if (!args.Contains("onupdate") || !args.Contains("from") || !args.Contains("to"))
		{
			Debug.LogError("iTween Error: ValueTo() requires an 'onupdate' callback function and a 'from' and 'to' property.  The supplied 'onupdate' callback must accept a single argument that is the same type as the supplied 'from' and 'to' properties!");
			return;
		}
		args["type"] = "value";
		if (args["from"].GetType() == typeof(Vector2))
		{
			args["method"] = "vector2";
		}
		else if (args["from"].GetType() == typeof(Vector3))
		{
			args["method"] = "vector3";
		}
		else if (args["from"].GetType() == typeof(Rect))
		{
			args["method"] = "rect";
		}
		else if (args["from"].GetType() == typeof(float))
		{
			args["method"] = "float";
		}
		else
		{
			if (args["from"].GetType() != typeof(Color))
			{
				Debug.LogError("iTween Error: ValueTo() only works with interpolating Vector3s, Vector2s, floats, ints, Rects and Colors!");
				return;
			}
			args["method"] = "color";
		}
		if (!args.Contains("easetype"))
		{
			args.Add("easetype", iTween.EaseType.linear);
		}
		iTween.Launch(target, args);
	}

	// Token: 0x0600347C RID: 13436 RVA: 0x00020EAE File Offset: 0x0001F0AE
	public static void FadeFrom(GameObject target, float alpha, float time)
	{
		iTween.FadeFrom(target, iTween.Hash(new object[]
		{
			"alpha",
			alpha,
			"time",
			time
		}));
	}

	// Token: 0x0600347D RID: 13437 RVA: 0x00020EE3 File Offset: 0x0001F0E3
	public static void FadeFrom(GameObject target, Hashtable args)
	{
		iTween.ColorFrom(target, args);
	}

	// Token: 0x0600347E RID: 13438 RVA: 0x00020EEC File Offset: 0x0001F0EC
	public static void FadeTo(GameObject target, float alpha, float time)
	{
		iTween.FadeTo(target, iTween.Hash(new object[]
		{
			"alpha",
			alpha,
			"time",
			time
		}));
	}

	// Token: 0x0600347F RID: 13439 RVA: 0x00020F21 File Offset: 0x0001F121
	public static void FadeTo(GameObject target, Hashtable args)
	{
		iTween.ColorTo(target, args);
	}

	// Token: 0x06003480 RID: 13440 RVA: 0x00020F2A File Offset: 0x0001F12A
	public static void ColorFrom(GameObject target, Color color, float time)
	{
		iTween.ColorFrom(target, iTween.Hash(new object[]
		{
			"color",
			color,
			"time",
			time
		}));
	}

	// Token: 0x06003481 RID: 13441 RVA: 0x001954F4 File Offset: 0x001936F4
	public static void ColorFrom(GameObject target, Hashtable args)
	{
		Color color = default(Color);
		Color color2 = default(Color);
		args = iTween.CleanArgs(args);
		if (!args.Contains("includechildren") || (bool)args["includechildren"])
		{
			foreach (object obj in target.transform)
			{
				Transform transform = (Transform)obj;
				Hashtable hashtable = (Hashtable)args.Clone();
				hashtable["ischild"] = true;
				iTween.ColorFrom(transform.gameObject, hashtable);
			}
		}
		if (!args.Contains("easetype"))
		{
			args.Add("easetype", iTween.EaseType.linear);
		}
		if (target.GetComponent(typeof(GUITexture)))
		{
			color = (color2 = target.guiTexture.color);
		}
		else if (target.GetComponent(typeof(GUIText)))
		{
			color = (color2 = target.guiText.material.color);
		}
		else if (target.renderer)
		{
			color = (color2 = target.renderer.material.color);
		}
		else if (target.light)
		{
			color = (color2 = target.light.color);
		}
		if (args.Contains("color"))
		{
			color = (Color)args["color"];
		}
		else
		{
			if (args.Contains("r"))
			{
				color.r = (float)args["r"];
			}
			if (args.Contains("g"))
			{
				color.g = (float)args["g"];
			}
			if (args.Contains("b"))
			{
				color.b = (float)args["b"];
			}
			if (args.Contains("a"))
			{
				color.a = (float)args["a"];
			}
		}
		if (args.Contains("amount"))
		{
			color.a = (float)args["amount"];
			args.Remove("amount");
		}
		else if (args.Contains("alpha"))
		{
			color.a = (float)args["alpha"];
			args.Remove("alpha");
		}
		if (target.GetComponent(typeof(GUITexture)))
		{
			target.guiTexture.color = color;
		}
		else if (target.GetComponent(typeof(GUIText)))
		{
			target.guiText.material.color = color;
		}
		else if (target.renderer)
		{
			target.renderer.material.color = color;
		}
		else if (target.light)
		{
			target.light.color = color;
		}
		args["color"] = color2;
		args["type"] = "color";
		args["method"] = "to";
		iTween.Launch(target, args);
	}

	// Token: 0x06003482 RID: 13442 RVA: 0x00020F5F File Offset: 0x0001F15F
	public static void ColorTo(GameObject target, Color color, float time)
	{
		iTween.ColorTo(target, iTween.Hash(new object[]
		{
			"color",
			color,
			"time",
			time
		}));
	}

	// Token: 0x06003483 RID: 13443 RVA: 0x00195884 File Offset: 0x00193A84
	public static void ColorTo(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		if (!args.Contains("includechildren") || (bool)args["includechildren"])
		{
			foreach (object obj in target.transform)
			{
				Transform transform = (Transform)obj;
				Hashtable hashtable = (Hashtable)args.Clone();
				hashtable["ischild"] = true;
				iTween.ColorTo(transform.gameObject, hashtable);
			}
		}
		if (!args.Contains("easetype"))
		{
			args.Add("easetype", iTween.EaseType.linear);
		}
		args["type"] = "color";
		args["method"] = "to";
		iTween.Launch(target, args);
	}

	// Token: 0x06003484 RID: 13444 RVA: 0x00195980 File Offset: 0x00193B80
	public static void AudioFrom(GameObject target, float volume, float pitch, float time)
	{
		iTween.AudioFrom(target, iTween.Hash(new object[]
		{
			"volume",
			volume,
			"pitch",
			pitch,
			"time",
			time
		}));
	}

	// Token: 0x06003485 RID: 13445 RVA: 0x001959D4 File Offset: 0x00193BD4
	public static void AudioFrom(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		AudioSource audioSource;
		if (args.Contains("audiosource"))
		{
			audioSource = (AudioSource)args["audiosource"];
		}
		else
		{
			if (!target.GetComponent(typeof(AudioSource)))
			{
				Debug.LogError("iTween Error: AudioFrom requires an AudioSource.");
				return;
			}
			audioSource = target.audio;
		}
		Vector2 vector;
		Vector2 vector2;
		vector.x = (vector2.x = audioSource.volume);
		vector.y = (vector2.y = audioSource.pitch);
		if (args.Contains("volume"))
		{
			vector2.x = (float)args["volume"];
		}
		if (args.Contains("pitch"))
		{
			vector2.y = (float)args["pitch"];
		}
		audioSource.volume = vector2.x;
		audioSource.pitch = vector2.y;
		args["volume"] = vector.x;
		args["pitch"] = vector.y;
		if (!args.Contains("easetype"))
		{
			args.Add("easetype", iTween.EaseType.linear);
		}
		args["type"] = "audio";
		args["method"] = "to";
		iTween.Launch(target, args);
	}

	// Token: 0x06003486 RID: 13446 RVA: 0x00195B50 File Offset: 0x00193D50
	public static void AudioTo(GameObject target, float volume, float pitch, float time)
	{
		iTween.AudioTo(target, iTween.Hash(new object[]
		{
			"volume",
			volume,
			"pitch",
			pitch,
			"time",
			time
		}));
	}

	// Token: 0x06003487 RID: 13447 RVA: 0x00195BA4 File Offset: 0x00193DA4
	public static void AudioTo(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		if (!args.Contains("easetype"))
		{
			args.Add("easetype", iTween.EaseType.linear);
		}
		args["type"] = "audio";
		args["method"] = "to";
		iTween.Launch(target, args);
	}

	// Token: 0x06003488 RID: 13448 RVA: 0x00020F94 File Offset: 0x0001F194
	public static void Stab(GameObject target, AudioClip audioclip, float delay)
	{
		iTween.Stab(target, iTween.Hash(new object[]
		{
			"audioclip",
			audioclip,
			"delay",
			delay
		}));
	}

	// Token: 0x06003489 RID: 13449 RVA: 0x00020FC4 File Offset: 0x0001F1C4
	public static void Stab(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		args["type"] = "stab";
		iTween.Launch(target, args);
	}

	// Token: 0x0600348A RID: 13450 RVA: 0x00020FE5 File Offset: 0x0001F1E5
	public static void LookFrom(GameObject target, Vector3 looktarget, float time)
	{
		iTween.LookFrom(target, iTween.Hash(new object[]
		{
			"looktarget",
			looktarget,
			"time",
			time
		}));
	}

	// Token: 0x0600348B RID: 13451 RVA: 0x00195C04 File Offset: 0x00193E04
	public static void LookFrom(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		Vector3 eulerAngles = target.transform.eulerAngles;
		if (args["looktarget"].GetType() == typeof(Transform))
		{
			Transform transform = target.transform;
			Transform transform2 = (Transform)args["looktarget"];
			Vector3? vector = (Vector3?)args["up"];
			transform.LookAt(transform2, (vector == null) ? iTween.Defaults.up : vector.Value);
		}
		else if (args["looktarget"].GetType() == typeof(Vector3))
		{
			Transform transform3 = target.transform;
			Vector3 vector2 = (Vector3)args["looktarget"];
			Vector3? vector3 = (Vector3?)args["up"];
			transform3.LookAt(vector2, (vector3 == null) ? iTween.Defaults.up : vector3.Value);
		}
		if (args.Contains("axis"))
		{
			Vector3 eulerAngles2 = target.transform.eulerAngles;
			string text = (string)args["axis"];
			if (text != null)
			{
				if (iTween.<>f__switch$map6B == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(3);
					dictionary.Add("x", 0);
					dictionary.Add("y", 1);
					dictionary.Add("z", 2);
					iTween.<>f__switch$map6B = dictionary;
				}
				int num;
				if (iTween.<>f__switch$map6B.TryGetValue(text, ref num))
				{
					switch (num)
					{
					case 0:
						eulerAngles2.y = eulerAngles.y;
						eulerAngles2.z = eulerAngles.z;
						break;
					case 1:
						eulerAngles2.x = eulerAngles.x;
						eulerAngles2.z = eulerAngles.z;
						break;
					case 2:
						eulerAngles2.x = eulerAngles.x;
						eulerAngles2.y = eulerAngles.y;
						break;
					}
				}
			}
			target.transform.eulerAngles = eulerAngles2;
		}
		args["rotation"] = eulerAngles;
		args["type"] = "rotate";
		args["method"] = "to";
		iTween.Launch(target, args);
	}

	// Token: 0x0600348C RID: 13452 RVA: 0x0002101A File Offset: 0x0001F21A
	public static void LookTo(GameObject target, Vector3 looktarget, float time)
	{
		iTween.LookTo(target, iTween.Hash(new object[]
		{
			"looktarget",
			looktarget,
			"time",
			time
		}));
	}

	// Token: 0x0600348D RID: 13453 RVA: 0x00195E44 File Offset: 0x00194044
	public static void LookTo(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		if (args.Contains("looktarget") && args["looktarget"].GetType() == typeof(Transform))
		{
			Transform transform = (Transform)args["looktarget"];
			args["position"] = new Vector3(transform.position.x, transform.position.y, transform.position.z);
			args["rotation"] = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
		}
		args["type"] = "look";
		args["method"] = "to";
		iTween.Launch(target, args);
	}

	// Token: 0x0600348E RID: 13454 RVA: 0x0002104F File Offset: 0x0001F24F
	public static void MoveTo(GameObject target, Vector3 position, float time)
	{
		iTween.MoveTo(target, iTween.Hash(new object[]
		{
			"position",
			position,
			"time",
			time
		}));
	}

	// Token: 0x0600348F RID: 13455 RVA: 0x00195F44 File Offset: 0x00194144
	public static void MoveTo(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		if (args.Contains("position") && args["position"].GetType() == typeof(Transform))
		{
			Transform transform = (Transform)args["position"];
			args["position"] = new Vector3(transform.position.x, transform.position.y, transform.position.z);
			args["rotation"] = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
			args["scale"] = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
		}
		args["type"] = "move";
		args["method"] = "to";
		iTween.Launch(target, args);
	}

	// Token: 0x06003490 RID: 13456 RVA: 0x00021084 File Offset: 0x0001F284
	public static void MoveFrom(GameObject target, Vector3 position, float time)
	{
		iTween.MoveFrom(target, iTween.Hash(new object[]
		{
			"position",
			position,
			"time",
			time
		}));
	}

	// Token: 0x06003491 RID: 13457 RVA: 0x00196084 File Offset: 0x00194284
	public static void MoveFrom(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		bool flag;
		if (args.Contains("islocal"))
		{
			flag = (bool)args["islocal"];
		}
		else
		{
			flag = iTween.Defaults.isLocal;
		}
		if (args.Contains("path"))
		{
			Vector3[] array2;
			if (args["path"].GetType() == typeof(Vector3[]))
			{
				Vector3[] array = (Vector3[])args["path"];
				array2 = new Vector3[array.Length];
				Array.Copy(array, array2, array.Length);
			}
			else
			{
				Transform[] array3 = (Transform[])args["path"];
				array2 = new Vector3[array3.Length];
				for (int i = 0; i < array3.Length; i++)
				{
					array2[i] = array3[i].position;
				}
			}
			if (array2[array2.Length - 1] != target.transform.position)
			{
				Vector3[] array4 = new Vector3[array2.Length + 1];
				Array.Copy(array2, array4, array2.Length);
				if (flag)
				{
					array4[array4.Length - 1] = target.transform.localPosition;
					target.transform.localPosition = array4[0];
				}
				else
				{
					array4[array4.Length - 1] = target.transform.position;
					target.transform.position = array4[0];
				}
				args["path"] = array4;
			}
			else
			{
				if (flag)
				{
					target.transform.localPosition = array2[0];
				}
				else
				{
					target.transform.position = array2[0];
				}
				args["path"] = array2;
			}
		}
		else
		{
			Vector3 vector2;
			Vector3 vector;
			if (flag)
			{
				vector = (vector2 = target.transform.localPosition);
			}
			else
			{
				vector = (vector2 = target.transform.position);
			}
			if (args.Contains("position"))
			{
				if (args["position"].GetType() == typeof(Transform))
				{
					Transform transform = (Transform)args["position"];
					vector = transform.position;
				}
				else if (args["position"].GetType() == typeof(Vector3))
				{
					vector = (Vector3)args["position"];
				}
			}
			else
			{
				if (args.Contains("x"))
				{
					vector.x = (float)args["x"];
				}
				if (args.Contains("y"))
				{
					vector.y = (float)args["y"];
				}
				if (args.Contains("z"))
				{
					vector.z = (float)args["z"];
				}
			}
			if (flag)
			{
				target.transform.localPosition = vector;
			}
			else
			{
				target.transform.position = vector;
			}
			args["position"] = vector2;
		}
		args["type"] = "move";
		args["method"] = "to";
		iTween.Launch(target, args);
	}

	// Token: 0x06003492 RID: 13458 RVA: 0x000210B9 File Offset: 0x0001F2B9
	public static void MoveAdd(GameObject target, Vector3 amount, float time)
	{
		iTween.MoveAdd(target, iTween.Hash(new object[]
		{
			"amount",
			amount,
			"time",
			time
		}));
	}

	// Token: 0x06003493 RID: 13459 RVA: 0x000210EE File Offset: 0x0001F2EE
	public static void MoveAdd(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		args["type"] = "move";
		args["method"] = "add";
		iTween.Launch(target, args);
	}

	// Token: 0x06003494 RID: 13460 RVA: 0x0002111F File Offset: 0x0001F31F
	public static void MoveBy(GameObject target, Vector3 amount, float time)
	{
		iTween.MoveBy(target, iTween.Hash(new object[]
		{
			"amount",
			amount,
			"time",
			time
		}));
	}

	// Token: 0x06003495 RID: 13461 RVA: 0x00021154 File Offset: 0x0001F354
	public static void MoveBy(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		args["type"] = "move";
		args["method"] = "by";
		iTween.Launch(target, args);
	}

	// Token: 0x06003496 RID: 13462 RVA: 0x00021185 File Offset: 0x0001F385
	public static void ScaleTo(GameObject target, Vector3 scale, float time)
	{
		iTween.ScaleTo(target, iTween.Hash(new object[]
		{
			"scale",
			scale,
			"time",
			time
		}));
	}

	// Token: 0x06003497 RID: 13463 RVA: 0x001963F0 File Offset: 0x001945F0
	public static void ScaleTo(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		if (args.Contains("scale") && args["scale"].GetType() == typeof(Transform))
		{
			Transform transform = (Transform)args["scale"];
			args["position"] = new Vector3(transform.position.x, transform.position.y, transform.position.z);
			args["rotation"] = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
			args["scale"] = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
		}
		args["type"] = "scale";
		args["method"] = "to";
		iTween.Launch(target, args);
	}

	// Token: 0x06003498 RID: 13464 RVA: 0x000211BA File Offset: 0x0001F3BA
	public static void ScaleFrom(GameObject target, Vector3 scale, float time)
	{
		iTween.ScaleFrom(target, iTween.Hash(new object[]
		{
			"scale",
			scale,
			"time",
			time
		}));
	}

	// Token: 0x06003499 RID: 13465 RVA: 0x00196530 File Offset: 0x00194730
	public static void ScaleFrom(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		Vector3 localScale2;
		Vector3 localScale = localScale2 = target.transform.localScale;
		if (args.Contains("scale"))
		{
			if (args["scale"].GetType() == typeof(Transform))
			{
				Transform transform = (Transform)args["scale"];
				localScale = transform.localScale;
			}
			else if (args["scale"].GetType() == typeof(Vector3))
			{
				localScale = (Vector3)args["scale"];
			}
		}
		else
		{
			if (args.Contains("x"))
			{
				localScale.x = (float)args["x"];
			}
			if (args.Contains("y"))
			{
				localScale.y = (float)args["y"];
			}
			if (args.Contains("z"))
			{
				localScale.z = (float)args["z"];
			}
		}
		target.transform.localScale = localScale;
		args["scale"] = localScale2;
		args["type"] = "scale";
		args["method"] = "to";
		iTween.Launch(target, args);
	}

	// Token: 0x0600349A RID: 13466 RVA: 0x000211EF File Offset: 0x0001F3EF
	public static void ScaleAdd(GameObject target, Vector3 amount, float time)
	{
		iTween.ScaleAdd(target, iTween.Hash(new object[]
		{
			"amount",
			amount,
			"time",
			time
		}));
	}

	// Token: 0x0600349B RID: 13467 RVA: 0x00021224 File Offset: 0x0001F424
	public static void ScaleAdd(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		args["type"] = "scale";
		args["method"] = "add";
		iTween.Launch(target, args);
	}

	// Token: 0x0600349C RID: 13468 RVA: 0x00021255 File Offset: 0x0001F455
	public static void ScaleBy(GameObject target, Vector3 amount, float time)
	{
		iTween.ScaleBy(target, iTween.Hash(new object[]
		{
			"amount",
			amount,
			"time",
			time
		}));
	}

	// Token: 0x0600349D RID: 13469 RVA: 0x0002128A File Offset: 0x0001F48A
	public static void ScaleBy(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		args["type"] = "scale";
		args["method"] = "by";
		iTween.Launch(target, args);
	}

	// Token: 0x0600349E RID: 13470 RVA: 0x000212BB File Offset: 0x0001F4BB
	public static void RotateTo(GameObject target, Vector3 rotation, float time)
	{
		iTween.RotateTo(target, iTween.Hash(new object[]
		{
			"rotation",
			rotation,
			"time",
			time
		}));
	}

	// Token: 0x0600349F RID: 13471 RVA: 0x00196690 File Offset: 0x00194890
	public static void RotateTo(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		if (args.Contains("rotation") && args["rotation"].GetType() == typeof(Transform))
		{
			Transform transform = (Transform)args["rotation"];
			args["position"] = new Vector3(transform.position.x, transform.position.y, transform.position.z);
			args["rotation"] = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
			args["scale"] = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
		}
		args["type"] = "rotate";
		args["method"] = "to";
		iTween.Launch(target, args);
	}

	// Token: 0x060034A0 RID: 13472 RVA: 0x000212F0 File Offset: 0x0001F4F0
	public static void RotateFrom(GameObject target, Vector3 rotation, float time)
	{
		iTween.RotateFrom(target, iTween.Hash(new object[]
		{
			"rotation",
			rotation,
			"time",
			time
		}));
	}

	// Token: 0x060034A1 RID: 13473 RVA: 0x001967D0 File Offset: 0x001949D0
	public static void RotateFrom(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		bool flag;
		if (args.Contains("islocal"))
		{
			flag = (bool)args["islocal"];
		}
		else
		{
			flag = iTween.Defaults.isLocal;
		}
		Vector3 vector2;
		Vector3 vector;
		if (flag)
		{
			vector = (vector2 = target.transform.localEulerAngles);
		}
		else
		{
			vector = (vector2 = target.transform.eulerAngles);
		}
		if (args.Contains("rotation"))
		{
			if (args["rotation"].GetType() == typeof(Transform))
			{
				Transform transform = (Transform)args["rotation"];
				vector = transform.eulerAngles;
			}
			else if (args["rotation"].GetType() == typeof(Vector3))
			{
				vector = (Vector3)args["rotation"];
			}
		}
		else
		{
			if (args.Contains("x"))
			{
				vector.x = (float)args["x"];
			}
			if (args.Contains("y"))
			{
				vector.y = (float)args["y"];
			}
			if (args.Contains("z"))
			{
				vector.z = (float)args["z"];
			}
		}
		if (flag)
		{
			target.transform.localEulerAngles = vector;
		}
		else
		{
			target.transform.eulerAngles = vector;
		}
		args["rotation"] = vector2;
		args["type"] = "rotate";
		args["method"] = "to";
		iTween.Launch(target, args);
	}

	// Token: 0x060034A2 RID: 13474 RVA: 0x00021325 File Offset: 0x0001F525
	public static void RotateAdd(GameObject target, Vector3 amount, float time)
	{
		iTween.RotateAdd(target, iTween.Hash(new object[]
		{
			"amount",
			amount,
			"time",
			time
		}));
	}

	// Token: 0x060034A3 RID: 13475 RVA: 0x0002135A File Offset: 0x0001F55A
	public static void RotateAdd(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		args["type"] = "rotate";
		args["method"] = "add";
		iTween.Launch(target, args);
	}

	// Token: 0x060034A4 RID: 13476 RVA: 0x0002138B File Offset: 0x0001F58B
	public static void RotateBy(GameObject target, Vector3 amount, float time)
	{
		iTween.RotateBy(target, iTween.Hash(new object[]
		{
			"amount",
			amount,
			"time",
			time
		}));
	}

	// Token: 0x060034A5 RID: 13477 RVA: 0x000213C0 File Offset: 0x0001F5C0
	public static void RotateBy(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		args["type"] = "rotate";
		args["method"] = "by";
		iTween.Launch(target, args);
	}

	// Token: 0x060034A6 RID: 13478 RVA: 0x000213F1 File Offset: 0x0001F5F1
	public static void ShakePosition(GameObject target, Vector3 amount, float time)
	{
		iTween.ShakePosition(target, iTween.Hash(new object[]
		{
			"amount",
			amount,
			"time",
			time
		}));
	}

	// Token: 0x060034A7 RID: 13479 RVA: 0x00021426 File Offset: 0x0001F626
	public static void ShakePosition(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		args["type"] = "shake";
		args["method"] = "position";
		iTween.Launch(target, args);
	}

	// Token: 0x060034A8 RID: 13480 RVA: 0x00021457 File Offset: 0x0001F657
	public static void ShakeScale(GameObject target, Vector3 amount, float time)
	{
		iTween.ShakeScale(target, iTween.Hash(new object[]
		{
			"amount",
			amount,
			"time",
			time
		}));
	}

	// Token: 0x060034A9 RID: 13481 RVA: 0x0002148C File Offset: 0x0001F68C
	public static void ShakeScale(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		args["type"] = "shake";
		args["method"] = "scale";
		iTween.Launch(target, args);
	}

	// Token: 0x060034AA RID: 13482 RVA: 0x000214BD File Offset: 0x0001F6BD
	public static void ShakeRotation(GameObject target, Vector3 amount, float time)
	{
		iTween.ShakeRotation(target, iTween.Hash(new object[]
		{
			"amount",
			amount,
			"time",
			time
		}));
	}

	// Token: 0x060034AB RID: 13483 RVA: 0x000214F2 File Offset: 0x0001F6F2
	public static void ShakeRotation(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		args["type"] = "shake";
		args["method"] = "rotation";
		iTween.Launch(target, args);
	}

	// Token: 0x060034AC RID: 13484 RVA: 0x00021523 File Offset: 0x0001F723
	public static void PunchPosition(GameObject target, Vector3 amount, float time)
	{
		iTween.PunchPosition(target, iTween.Hash(new object[]
		{
			"amount",
			amount,
			"time",
			time
		}));
	}

	// Token: 0x060034AD RID: 13485 RVA: 0x0019698C File Offset: 0x00194B8C
	public static void PunchPosition(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		args["type"] = "punch";
		args["method"] = "position";
		args["easetype"] = iTween.EaseType.punch;
		iTween.Launch(target, args);
	}

	// Token: 0x060034AE RID: 13486 RVA: 0x00021558 File Offset: 0x0001F758
	public static void PunchRotation(GameObject target, Vector3 amount, float time)
	{
		iTween.PunchRotation(target, iTween.Hash(new object[]
		{
			"amount",
			amount,
			"time",
			time
		}));
	}

	// Token: 0x060034AF RID: 13487 RVA: 0x001969DC File Offset: 0x00194BDC
	public static void PunchRotation(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		args["type"] = "punch";
		args["method"] = "rotation";
		args["easetype"] = iTween.EaseType.punch;
		iTween.Launch(target, args);
	}

	// Token: 0x060034B0 RID: 13488 RVA: 0x0002158D File Offset: 0x0001F78D
	public static void PunchScale(GameObject target, Vector3 amount, float time)
	{
		iTween.PunchScale(target, iTween.Hash(new object[]
		{
			"amount",
			amount,
			"time",
			time
		}));
	}

	// Token: 0x060034B1 RID: 13489 RVA: 0x00196A2C File Offset: 0x00194C2C
	public static void PunchScale(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		args["type"] = "punch";
		args["method"] = "scale";
		args["easetype"] = iTween.EaseType.punch;
		iTween.Launch(target, args);
	}

	// Token: 0x060034B2 RID: 13490 RVA: 0x00196A7C File Offset: 0x00194C7C
	private void GenerateTargets()
	{
		string text = this.type;
		if (text != null)
		{
			if (iTween.<>f__switch$map75 == null)
			{
				Dictionary<string, int> dictionary = new Dictionary<string, int>(10);
				dictionary.Add("value", 0);
				dictionary.Add("color", 1);
				dictionary.Add("audio", 2);
				dictionary.Add("move", 3);
				dictionary.Add("scale", 4);
				dictionary.Add("rotate", 5);
				dictionary.Add("shake", 6);
				dictionary.Add("punch", 7);
				dictionary.Add("look", 8);
				dictionary.Add("stab", 9);
				iTween.<>f__switch$map75 = dictionary;
			}
			int num;
			if (iTween.<>f__switch$map75.TryGetValue(text, ref num))
			{
				switch (num)
				{
				case 0:
				{
					string text2 = this.method;
					if (text2 != null)
					{
						if (iTween.<>f__switch$map6C == null)
						{
							Dictionary<string, int> dictionary = new Dictionary<string, int>(5);
							dictionary.Add("float", 0);
							dictionary.Add("vector2", 1);
							dictionary.Add("vector3", 2);
							dictionary.Add("color", 3);
							dictionary.Add("rect", 4);
							iTween.<>f__switch$map6C = dictionary;
						}
						int num2;
						if (iTween.<>f__switch$map6C.TryGetValue(text2, ref num2))
						{
							switch (num2)
							{
							case 0:
								this.GenerateFloatTargets();
								this.apply = new iTween.ApplyTween(this.ApplyFloatTargets);
								break;
							case 1:
								this.GenerateVector2Targets();
								this.apply = new iTween.ApplyTween(this.ApplyVector2Targets);
								break;
							case 2:
								this.GenerateVector3Targets();
								this.apply = new iTween.ApplyTween(this.ApplyVector3Targets);
								break;
							case 3:
								this.GenerateColorTargets();
								this.apply = new iTween.ApplyTween(this.ApplyColorTargets);
								break;
							case 4:
								this.GenerateRectTargets();
								this.apply = new iTween.ApplyTween(this.ApplyRectTargets);
								break;
							}
						}
					}
					break;
				}
				case 1:
				{
					string text2 = this.method;
					if (text2 != null)
					{
						if (iTween.<>f__switch$map6D == null)
						{
							Dictionary<string, int> dictionary = new Dictionary<string, int>(1);
							dictionary.Add("to", 0);
							iTween.<>f__switch$map6D = dictionary;
						}
						int num2;
						if (iTween.<>f__switch$map6D.TryGetValue(text2, ref num2))
						{
							if (num2 == 0)
							{
								this.GenerateColorToTargets();
								this.apply = new iTween.ApplyTween(this.ApplyColorToTargets);
							}
						}
					}
					break;
				}
				case 2:
				{
					string text2 = this.method;
					if (text2 != null)
					{
						if (iTween.<>f__switch$map6E == null)
						{
							Dictionary<string, int> dictionary = new Dictionary<string, int>(1);
							dictionary.Add("to", 0);
							iTween.<>f__switch$map6E = dictionary;
						}
						int num2;
						if (iTween.<>f__switch$map6E.TryGetValue(text2, ref num2))
						{
							if (num2 == 0)
							{
								this.GenerateAudioToTargets();
								this.apply = new iTween.ApplyTween(this.ApplyAudioToTargets);
							}
						}
					}
					break;
				}
				case 3:
				{
					string text2 = this.method;
					if (text2 != null)
					{
						if (iTween.<>f__switch$map6F == null)
						{
							Dictionary<string, int> dictionary = new Dictionary<string, int>(3);
							dictionary.Add("to", 0);
							dictionary.Add("by", 1);
							dictionary.Add("add", 1);
							iTween.<>f__switch$map6F = dictionary;
						}
						int num2;
						if (iTween.<>f__switch$map6F.TryGetValue(text2, ref num2))
						{
							if (num2 != 0)
							{
								if (num2 == 1)
								{
									this.GenerateMoveByTargets();
									this.apply = new iTween.ApplyTween(this.ApplyMoveByTargets);
								}
							}
							else if (this.tweenArguments.Contains("path"))
							{
								this.GenerateMoveToPathTargets();
								this.apply = new iTween.ApplyTween(this.ApplyMoveToPathTargets);
							}
							else
							{
								this.GenerateMoveToTargets();
								this.apply = new iTween.ApplyTween(this.ApplyMoveToTargets);
							}
						}
					}
					break;
				}
				case 4:
				{
					string text2 = this.method;
					if (text2 != null)
					{
						if (iTween.<>f__switch$map70 == null)
						{
							Dictionary<string, int> dictionary = new Dictionary<string, int>(3);
							dictionary.Add("to", 0);
							dictionary.Add("by", 1);
							dictionary.Add("add", 2);
							iTween.<>f__switch$map70 = dictionary;
						}
						int num2;
						if (iTween.<>f__switch$map70.TryGetValue(text2, ref num2))
						{
							switch (num2)
							{
							case 0:
								this.GenerateScaleToTargets();
								this.apply = new iTween.ApplyTween(this.ApplyScaleToTargets);
								break;
							case 1:
								this.GenerateScaleByTargets();
								this.apply = new iTween.ApplyTween(this.ApplyScaleToTargets);
								break;
							case 2:
								this.GenerateScaleAddTargets();
								this.apply = new iTween.ApplyTween(this.ApplyScaleToTargets);
								break;
							}
						}
					}
					break;
				}
				case 5:
				{
					string text2 = this.method;
					if (text2 != null)
					{
						if (iTween.<>f__switch$map71 == null)
						{
							Dictionary<string, int> dictionary = new Dictionary<string, int>(3);
							dictionary.Add("to", 0);
							dictionary.Add("add", 1);
							dictionary.Add("by", 2);
							iTween.<>f__switch$map71 = dictionary;
						}
						int num2;
						if (iTween.<>f__switch$map71.TryGetValue(text2, ref num2))
						{
							switch (num2)
							{
							case 0:
								this.GenerateRotateToTargets();
								this.apply = new iTween.ApplyTween(this.ApplyRotateToTargets);
								break;
							case 1:
								this.GenerateRotateAddTargets();
								this.apply = new iTween.ApplyTween(this.ApplyRotateAddTargets);
								break;
							case 2:
								this.GenerateRotateByTargets();
								this.apply = new iTween.ApplyTween(this.ApplyRotateAddTargets);
								break;
							}
						}
					}
					break;
				}
				case 6:
				{
					string text2 = this.method;
					if (text2 != null)
					{
						if (iTween.<>f__switch$map72 == null)
						{
							Dictionary<string, int> dictionary = new Dictionary<string, int>(3);
							dictionary.Add("position", 0);
							dictionary.Add("scale", 1);
							dictionary.Add("rotation", 2);
							iTween.<>f__switch$map72 = dictionary;
						}
						int num2;
						if (iTween.<>f__switch$map72.TryGetValue(text2, ref num2))
						{
							switch (num2)
							{
							case 0:
								this.GenerateShakePositionTargets();
								this.apply = new iTween.ApplyTween(this.ApplyShakePositionTargets);
								break;
							case 1:
								this.GenerateShakeScaleTargets();
								this.apply = new iTween.ApplyTween(this.ApplyShakeScaleTargets);
								break;
							case 2:
								this.GenerateShakeRotationTargets();
								this.apply = new iTween.ApplyTween(this.ApplyShakeRotationTargets);
								break;
							}
						}
					}
					break;
				}
				case 7:
				{
					string text2 = this.method;
					if (text2 != null)
					{
						if (iTween.<>f__switch$map73 == null)
						{
							Dictionary<string, int> dictionary = new Dictionary<string, int>(3);
							dictionary.Add("position", 0);
							dictionary.Add("rotation", 1);
							dictionary.Add("scale", 2);
							iTween.<>f__switch$map73 = dictionary;
						}
						int num2;
						if (iTween.<>f__switch$map73.TryGetValue(text2, ref num2))
						{
							switch (num2)
							{
							case 0:
								this.GeneratePunchPositionTargets();
								this.apply = new iTween.ApplyTween(this.ApplyPunchPositionTargets);
								break;
							case 1:
								this.GeneratePunchRotationTargets();
								this.apply = new iTween.ApplyTween(this.ApplyPunchRotationTargets);
								break;
							case 2:
								this.GeneratePunchScaleTargets();
								this.apply = new iTween.ApplyTween(this.ApplyPunchScaleTargets);
								break;
							}
						}
					}
					break;
				}
				case 8:
				{
					string text2 = this.method;
					if (text2 != null)
					{
						if (iTween.<>f__switch$map74 == null)
						{
							Dictionary<string, int> dictionary = new Dictionary<string, int>(1);
							dictionary.Add("to", 0);
							iTween.<>f__switch$map74 = dictionary;
						}
						int num2;
						if (iTween.<>f__switch$map74.TryGetValue(text2, ref num2))
						{
							if (num2 == 0)
							{
								this.GenerateLookToTargets();
								this.apply = new iTween.ApplyTween(this.ApplyLookToTargets);
							}
						}
					}
					break;
				}
				case 9:
					this.GenerateStabTargets();
					this.apply = new iTween.ApplyTween(this.ApplyStabTargets);
					break;
				}
			}
		}
	}

	// Token: 0x060034B3 RID: 13491 RVA: 0x00197218 File Offset: 0x00195418
	private void GenerateRectTargets()
	{
		this.rects = new Rect[3];
		this.rects[0] = (Rect)this.tweenArguments["from"];
		this.rects[1] = (Rect)this.tweenArguments["to"];
	}

	// Token: 0x060034B4 RID: 13492 RVA: 0x00197280 File Offset: 0x00195480
	private void GenerateColorTargets()
	{
		this.colors = new Color[1, 3];
		this.colors[0, 0] = (Color)this.tweenArguments["from"];
		this.colors[0, 1] = (Color)this.tweenArguments["to"];
	}

	// Token: 0x060034B5 RID: 13493 RVA: 0x001972E0 File Offset: 0x001954E0
	private void GenerateVector3Targets()
	{
		this.vector3s = new Vector3[3];
		this.vector3s[0] = (Vector3)this.tweenArguments["from"];
		this.vector3s[1] = (Vector3)this.tweenArguments["to"];
		if (this.tweenArguments.Contains("speed"))
		{
			float num = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
			this.time = num / (float)this.tweenArguments["speed"];
		}
	}

	// Token: 0x060034B6 RID: 13494 RVA: 0x001973A4 File Offset: 0x001955A4
	private void GenerateVector2Targets()
	{
		this.vector2s = new Vector2[3];
		this.vector2s[0] = (Vector2)this.tweenArguments["from"];
		this.vector2s[1] = (Vector2)this.tweenArguments["to"];
		if (this.tweenArguments.Contains("speed"))
		{
			Vector3 vector;
			vector..ctor(this.vector2s[0].x, this.vector2s[0].y, 0f);
			Vector3 vector2;
			vector2..ctor(this.vector2s[1].x, this.vector2s[1].y, 0f);
			float num = Math.Abs(Vector3.Distance(vector, vector2));
			this.time = num / (float)this.tweenArguments["speed"];
		}
	}

	// Token: 0x060034B7 RID: 13495 RVA: 0x001974A4 File Offset: 0x001956A4
	private void GenerateFloatTargets()
	{
		this.floats = new float[3];
		this.floats[0] = (float)this.tweenArguments["from"];
		this.floats[1] = (float)this.tweenArguments["to"];
		if (this.tweenArguments.Contains("speed"))
		{
			float num = Math.Abs(this.floats[0] - this.floats[1]);
			this.time = num / (float)this.tweenArguments["speed"];
		}
	}

	// Token: 0x060034B8 RID: 13496 RVA: 0x00197540 File Offset: 0x00195740
	private void GenerateColorToTargets()
	{
		if (base.GetComponent(typeof(GUITexture)))
		{
			this.colors = new Color[1, 3];
			this.colors[0, 0] = (this.colors[0, 1] = base.guiTexture.color);
		}
		else if (base.GetComponent(typeof(GUIText)))
		{
			this.colors = new Color[1, 3];
			this.colors[0, 0] = (this.colors[0, 1] = base.guiText.material.color);
		}
		else if (base.renderer)
		{
			this.colors = new Color[base.renderer.materials.Length, 3];
			for (int i = 0; i < base.renderer.materials.Length; i++)
			{
				this.colors[i, 0] = base.renderer.materials[i].GetColor(this.namedcolorvalue.ToString());
				this.colors[i, 1] = base.renderer.materials[i].GetColor(this.namedcolorvalue.ToString());
			}
		}
		else if (base.light)
		{
			this.colors = new Color[1, 3];
			this.colors[0, 0] = (this.colors[0, 1] = base.light.color);
		}
		else
		{
			this.colors = new Color[1, 3];
		}
		if (this.tweenArguments.Contains("color"))
		{
			for (int j = 0; j < this.colors.GetLength(0); j++)
			{
				this.colors[j, 1] = (Color)this.tweenArguments["color"];
			}
		}
		else
		{
			if (this.tweenArguments.Contains("r"))
			{
				for (int k = 0; k < this.colors.GetLength(0); k++)
				{
					this.colors[k, 1].r = (float)this.tweenArguments["r"];
				}
			}
			if (this.tweenArguments.Contains("g"))
			{
				for (int l = 0; l < this.colors.GetLength(0); l++)
				{
					this.colors[l, 1].g = (float)this.tweenArguments["g"];
				}
			}
			if (this.tweenArguments.Contains("b"))
			{
				for (int m = 0; m < this.colors.GetLength(0); m++)
				{
					this.colors[m, 1].b = (float)this.tweenArguments["b"];
				}
			}
			if (this.tweenArguments.Contains("a"))
			{
				for (int n = 0; n < this.colors.GetLength(0); n++)
				{
					this.colors[n, 1].a = (float)this.tweenArguments["a"];
				}
			}
		}
		if (this.tweenArguments.Contains("amount"))
		{
			for (int num = 0; num < this.colors.GetLength(0); num++)
			{
				this.colors[num, 1].a = (float)this.tweenArguments["amount"];
			}
		}
		else if (this.tweenArguments.Contains("alpha"))
		{
			for (int num2 = 0; num2 < this.colors.GetLength(0); num2++)
			{
				this.colors[num2, 1].a = (float)this.tweenArguments["alpha"];
			}
		}
	}

	// Token: 0x060034B9 RID: 13497 RVA: 0x0019798C File Offset: 0x00195B8C
	private void GenerateAudioToTargets()
	{
		this.vector2s = new Vector2[3];
		if (this.tweenArguments.Contains("audiosource"))
		{
			this.audioSource = (AudioSource)this.tweenArguments["audiosource"];
		}
		else if (base.GetComponent(typeof(AudioSource)))
		{
			this.audioSource = base.audio;
		}
		else
		{
			Debug.LogError("iTween Error: AudioTo requires an AudioSource.");
			this.Dispose();
		}
		this.vector2s[0] = (this.vector2s[1] = new Vector2(this.audioSource.volume, this.audioSource.pitch));
		if (this.tweenArguments.Contains("volume"))
		{
			this.vector2s[1].x = (float)this.tweenArguments["volume"];
		}
		if (this.tweenArguments.Contains("pitch"))
		{
			this.vector2s[1].y = (float)this.tweenArguments["pitch"];
		}
	}

	// Token: 0x060034BA RID: 13498 RVA: 0x00197ACC File Offset: 0x00195CCC
	private void GenerateStabTargets()
	{
		if (this.tweenArguments.Contains("audiosource"))
		{
			this.audioSource = (AudioSource)this.tweenArguments["audiosource"];
		}
		else if (base.GetComponent(typeof(AudioSource)))
		{
			this.audioSource = base.audio;
		}
		else
		{
			base.gameObject.AddComponent(typeof(AudioSource));
			this.audioSource = base.audio;
			this.audioSource.playOnAwake = false;
		}
		this.audioSource.clip = (AudioClip)this.tweenArguments["audioclip"];
		if (this.tweenArguments.Contains("pitch"))
		{
			this.audioSource.pitch = (float)this.tweenArguments["pitch"];
		}
		if (this.tweenArguments.Contains("volume"))
		{
			this.audioSource.volume = (float)this.tweenArguments["volume"];
		}
		this.time = this.audioSource.clip.length / this.audioSource.pitch;
	}

	// Token: 0x060034BB RID: 13499 RVA: 0x00197C14 File Offset: 0x00195E14
	private void GenerateLookToTargets()
	{
		this.vector3s = new Vector3[3];
		this.vector3s[0] = base.transform.eulerAngles;
		if (this.tweenArguments.Contains("looktarget"))
		{
			if (this.tweenArguments["looktarget"].GetType() == typeof(Transform))
			{
				Transform transform = base.transform;
				Transform transform2 = (Transform)this.tweenArguments["looktarget"];
				Vector3? vector = (Vector3?)this.tweenArguments["up"];
				transform.LookAt(transform2, (vector == null) ? iTween.Defaults.up : vector.Value);
			}
			else if (this.tweenArguments["looktarget"].GetType() == typeof(Vector3))
			{
				Transform transform3 = base.transform;
				Vector3 vector2 = (Vector3)this.tweenArguments["looktarget"];
				Vector3? vector3 = (Vector3?)this.tweenArguments["up"];
				transform3.LookAt(vector2, (vector3 == null) ? iTween.Defaults.up : vector3.Value);
			}
		}
		else
		{
			Debug.LogError("iTween Error: LookTo needs a 'looktarget' property!");
			this.Dispose();
		}
		this.vector3s[1] = base.transform.eulerAngles;
		base.transform.eulerAngles = this.vector3s[0];
		if (this.tweenArguments.Contains("axis"))
		{
			string text = (string)this.tweenArguments["axis"];
			if (text != null)
			{
				if (iTween.<>f__switch$map76 == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(3);
					dictionary.Add("x", 0);
					dictionary.Add("y", 1);
					dictionary.Add("z", 2);
					iTween.<>f__switch$map76 = dictionary;
				}
				int num;
				if (iTween.<>f__switch$map76.TryGetValue(text, ref num))
				{
					switch (num)
					{
					case 0:
						this.vector3s[1].y = this.vector3s[0].y;
						this.vector3s[1].z = this.vector3s[0].z;
						break;
					case 1:
						this.vector3s[1].x = this.vector3s[0].x;
						this.vector3s[1].z = this.vector3s[0].z;
						break;
					case 2:
						this.vector3s[1].x = this.vector3s[0].x;
						this.vector3s[1].y = this.vector3s[0].y;
						break;
					}
				}
			}
		}
		this.vector3s[1] = new Vector3(this.clerp(this.vector3s[0].x, this.vector3s[1].x, 1f), this.clerp(this.vector3s[0].y, this.vector3s[1].y, 1f), this.clerp(this.vector3s[0].z, this.vector3s[1].z, 1f));
		if (this.tweenArguments.Contains("speed"))
		{
			float num2 = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
			this.time = num2 / (float)this.tweenArguments["speed"];
		}
	}

	// Token: 0x060034BC RID: 13500 RVA: 0x00198010 File Offset: 0x00196210
	private void GenerateMoveToPathTargets()
	{
		Vector3[] array2;
		if (this.tweenArguments["path"].GetType() == typeof(Vector3[]))
		{
			Vector3[] array = (Vector3[])this.tweenArguments["path"];
			if (array.Length == 1)
			{
				Debug.LogError("iTween Error: Attempting a path movement with MoveTo requires an array of more than 1 entry!");
				this.Dispose();
			}
			array2 = new Vector3[array.Length];
			Array.Copy(array, array2, array.Length);
		}
		else
		{
			Transform[] array3 = (Transform[])this.tweenArguments["path"];
			if (array3.Length == 1)
			{
				Debug.LogError("iTween Error: Attempting a path movement with MoveTo requires an array of more than 1 entry!");
				this.Dispose();
			}
			array2 = new Vector3[array3.Length];
			for (int i = 0; i < array3.Length; i++)
			{
				array2[i] = array3[i].position;
			}
		}
		bool flag;
		int num;
		if (base.transform.position != array2[0])
		{
			if (!this.tweenArguments.Contains("movetopath") || (bool)this.tweenArguments["movetopath"])
			{
				flag = true;
				num = 3;
			}
			else
			{
				flag = false;
				num = 2;
			}
		}
		else
		{
			flag = false;
			num = 2;
		}
		this.vector3s = new Vector3[array2.Length + num];
		if (flag)
		{
			this.vector3s[1] = base.transform.position;
			num = 2;
		}
		else
		{
			num = 1;
		}
		Array.Copy(array2, 0, this.vector3s, num, array2.Length);
		this.vector3s[0] = this.vector3s[1] + (this.vector3s[1] - this.vector3s[2]);
		this.vector3s[this.vector3s.Length - 1] = this.vector3s[this.vector3s.Length - 2] + (this.vector3s[this.vector3s.Length - 2] - this.vector3s[this.vector3s.Length - 3]);
		if (this.vector3s[1] == this.vector3s[this.vector3s.Length - 2])
		{
			Vector3[] array4 = new Vector3[this.vector3s.Length];
			Array.Copy(this.vector3s, array4, this.vector3s.Length);
			array4[0] = array4[array4.Length - 3];
			array4[array4.Length - 1] = array4[2];
			this.vector3s = new Vector3[array4.Length];
			Array.Copy(array4, this.vector3s, array4.Length);
		}
		this.path = new iTween.CRSpline(this.vector3s);
		if (this.tweenArguments.Contains("speed"))
		{
			float num2 = iTween.PathLength(this.vector3s);
			this.time = num2 / (float)this.tweenArguments["speed"];
		}
	}

	// Token: 0x060034BD RID: 13501 RVA: 0x00198370 File Offset: 0x00196570
	private void GenerateMoveToTargets()
	{
		this.vector3s = new Vector3[3];
		if (this.isLocal)
		{
			this.vector3s[0] = (this.vector3s[1] = base.transform.localPosition);
		}
		else
		{
			this.vector3s[0] = (this.vector3s[1] = base.transform.position);
		}
		if (this.tweenArguments.Contains("position"))
		{
			if (this.tweenArguments["position"].GetType() == typeof(Transform))
			{
				Transform transform = (Transform)this.tweenArguments["position"];
				this.vector3s[1] = transform.position;
			}
			else if (this.tweenArguments["position"].GetType() == typeof(Vector3))
			{
				this.vector3s[1] = (Vector3)this.tweenArguments["position"];
			}
		}
		else
		{
			if (this.tweenArguments.Contains("x"))
			{
				this.vector3s[1].x = (float)this.tweenArguments["x"];
			}
			if (this.tweenArguments.Contains("y"))
			{
				this.vector3s[1].y = (float)this.tweenArguments["y"];
			}
			if (this.tweenArguments.Contains("z"))
			{
				this.vector3s[1].z = (float)this.tweenArguments["z"];
			}
		}
		if (this.tweenArguments.Contains("orienttopath") && (bool)this.tweenArguments["orienttopath"])
		{
			this.tweenArguments["looktarget"] = this.vector3s[1];
		}
		if (this.tweenArguments.Contains("speed"))
		{
			float num = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
			this.time = num / (float)this.tweenArguments["speed"];
		}
	}

	// Token: 0x060034BE RID: 13502 RVA: 0x00198618 File Offset: 0x00196818
	private void GenerateMoveByTargets()
	{
		this.vector3s = new Vector3[6];
		this.vector3s[4] = base.transform.eulerAngles;
		this.vector3s[0] = (this.vector3s[1] = (this.vector3s[3] = base.transform.position));
		if (this.tweenArguments.Contains("amount"))
		{
			this.vector3s[1] = this.vector3s[0] + (Vector3)this.tweenArguments["amount"];
		}
		else
		{
			if (this.tweenArguments.Contains("x"))
			{
				this.vector3s[1].x = this.vector3s[0].x + (float)this.tweenArguments["x"];
			}
			if (this.tweenArguments.Contains("y"))
			{
				this.vector3s[1].y = this.vector3s[0].y + (float)this.tweenArguments["y"];
			}
			if (this.tweenArguments.Contains("z"))
			{
				this.vector3s[1].z = this.vector3s[0].z + (float)this.tweenArguments["z"];
			}
		}
		base.transform.Translate(this.vector3s[1], this.space);
		this.vector3s[5] = base.transform.position;
		base.transform.position = this.vector3s[0];
		if (this.tweenArguments.Contains("orienttopath") && (bool)this.tweenArguments["orienttopath"])
		{
			this.tweenArguments["looktarget"] = this.vector3s[1];
		}
		if (this.tweenArguments.Contains("speed"))
		{
			float num = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
			this.time = num / (float)this.tweenArguments["speed"];
		}
	}

	// Token: 0x060034BF RID: 13503 RVA: 0x001988DC File Offset: 0x00196ADC
	private void GenerateScaleToTargets()
	{
		this.vector3s = new Vector3[3];
		this.vector3s[0] = (this.vector3s[1] = base.transform.localScale);
		if (this.tweenArguments.Contains("scale"))
		{
			if (this.tweenArguments["scale"].GetType() == typeof(Transform))
			{
				Transform transform = (Transform)this.tweenArguments["scale"];
				this.vector3s[1] = transform.localScale;
			}
			else if (this.tweenArguments["scale"].GetType() == typeof(Vector3))
			{
				this.vector3s[1] = (Vector3)this.tweenArguments["scale"];
			}
		}
		else
		{
			if (this.tweenArguments.Contains("x"))
			{
				this.vector3s[1].x = (float)this.tweenArguments["x"];
			}
			if (this.tweenArguments.Contains("y"))
			{
				this.vector3s[1].y = (float)this.tweenArguments["y"];
			}
			if (this.tweenArguments.Contains("z"))
			{
				this.vector3s[1].z = (float)this.tweenArguments["z"];
			}
		}
		if (this.tweenArguments.Contains("speed"))
		{
			float num = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
			this.time = num / (float)this.tweenArguments["speed"];
		}
	}

	// Token: 0x060034C0 RID: 13504 RVA: 0x00198AF0 File Offset: 0x00196CF0
	private void GenerateScaleByTargets()
	{
		this.vector3s = new Vector3[3];
		this.vector3s[0] = (this.vector3s[1] = base.transform.localScale);
		if (this.tweenArguments.Contains("amount"))
		{
			this.vector3s[1] = Vector3.Scale(this.vector3s[1], (Vector3)this.tweenArguments["amount"]);
		}
		else
		{
			if (this.tweenArguments.Contains("x"))
			{
				Vector3[] array = this.vector3s;
				int num = 1;
				array[num].x = array[num].x * (float)this.tweenArguments["x"];
			}
			if (this.tweenArguments.Contains("y"))
			{
				Vector3[] array2 = this.vector3s;
				int num2 = 1;
				array2[num2].y = array2[num2].y * (float)this.tweenArguments["y"];
			}
			if (this.tweenArguments.Contains("z"))
			{
				Vector3[] array3 = this.vector3s;
				int num3 = 1;
				array3[num3].z = array3[num3].z * (float)this.tweenArguments["z"];
			}
		}
		if (this.tweenArguments.Contains("speed"))
		{
			float num4 = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
			this.time = num4 / (float)this.tweenArguments["speed"];
		}
	}

	// Token: 0x060034C1 RID: 13505 RVA: 0x00198CB4 File Offset: 0x00196EB4
	private void GenerateScaleAddTargets()
	{
		this.vector3s = new Vector3[3];
		this.vector3s[0] = (this.vector3s[1] = base.transform.localScale);
		if (this.tweenArguments.Contains("amount"))
		{
			this.vector3s[1] += (Vector3)this.tweenArguments["amount"];
		}
		else
		{
			if (this.tweenArguments.Contains("x"))
			{
				Vector3[] array = this.vector3s;
				int num = 1;
				array[num].x = array[num].x + (float)this.tweenArguments["x"];
			}
			if (this.tweenArguments.Contains("y"))
			{
				Vector3[] array2 = this.vector3s;
				int num2 = 1;
				array2[num2].y = array2[num2].y + (float)this.tweenArguments["y"];
			}
			if (this.tweenArguments.Contains("z"))
			{
				Vector3[] array3 = this.vector3s;
				int num3 = 1;
				array3[num3].z = array3[num3].z + (float)this.tweenArguments["z"];
			}
		}
		if (this.tweenArguments.Contains("speed"))
		{
			float num4 = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
			this.time = num4 / (float)this.tweenArguments["speed"];
		}
	}

	// Token: 0x060034C2 RID: 13506 RVA: 0x00198E70 File Offset: 0x00197070
	private void GenerateRotateToTargets()
	{
		this.vector3s = new Vector3[3];
		if (this.isLocal)
		{
			this.vector3s[0] = (this.vector3s[1] = base.transform.localEulerAngles);
		}
		else
		{
			this.vector3s[0] = (this.vector3s[1] = base.transform.eulerAngles);
		}
		if (this.tweenArguments.Contains("rotation"))
		{
			if (this.tweenArguments["rotation"].GetType() == typeof(Transform))
			{
				Transform transform = (Transform)this.tweenArguments["rotation"];
				this.vector3s[1] = transform.eulerAngles;
			}
			else if (this.tweenArguments["rotation"].GetType() == typeof(Vector3))
			{
				this.vector3s[1] = (Vector3)this.tweenArguments["rotation"];
			}
		}
		else
		{
			if (this.tweenArguments.Contains("x"))
			{
				this.vector3s[1].x = (float)this.tweenArguments["x"];
			}
			if (this.tweenArguments.Contains("y"))
			{
				this.vector3s[1].y = (float)this.tweenArguments["y"];
			}
			if (this.tweenArguments.Contains("z"))
			{
				this.vector3s[1].z = (float)this.tweenArguments["z"];
			}
		}
		this.vector3s[1] = new Vector3(this.clerp(this.vector3s[0].x, this.vector3s[1].x, 1f), this.clerp(this.vector3s[0].y, this.vector3s[1].y, 1f), this.clerp(this.vector3s[0].z, this.vector3s[1].z, 1f));
		if (this.tweenArguments.Contains("speed"))
		{
			float num = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
			this.time = num / (float)this.tweenArguments["speed"];
		}
	}

	// Token: 0x060034C3 RID: 13507 RVA: 0x00199160 File Offset: 0x00197360
	private void GenerateRotateAddTargets()
	{
		this.vector3s = new Vector3[5];
		this.vector3s[0] = (this.vector3s[1] = (this.vector3s[3] = base.transform.eulerAngles));
		if (this.tweenArguments.Contains("amount"))
		{
			this.vector3s[1] += (Vector3)this.tweenArguments["amount"];
		}
		else
		{
			if (this.tweenArguments.Contains("x"))
			{
				Vector3[] array = this.vector3s;
				int num = 1;
				array[num].x = array[num].x + (float)this.tweenArguments["x"];
			}
			if (this.tweenArguments.Contains("y"))
			{
				Vector3[] array2 = this.vector3s;
				int num2 = 1;
				array2[num2].y = array2[num2].y + (float)this.tweenArguments["y"];
			}
			if (this.tweenArguments.Contains("z"))
			{
				Vector3[] array3 = this.vector3s;
				int num3 = 1;
				array3[num3].z = array3[num3].z + (float)this.tweenArguments["z"];
			}
		}
		if (this.tweenArguments.Contains("speed"))
		{
			float num4 = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
			this.time = num4 / (float)this.tweenArguments["speed"];
		}
	}

	// Token: 0x060034C4 RID: 13508 RVA: 0x00199330 File Offset: 0x00197530
	private void GenerateRotateByTargets()
	{
		this.vector3s = new Vector3[4];
		this.vector3s[0] = (this.vector3s[1] = (this.vector3s[3] = base.transform.eulerAngles));
		if (this.tweenArguments.Contains("amount"))
		{
			this.vector3s[1] += Vector3.Scale((Vector3)this.tweenArguments["amount"], new Vector3(360f, 360f, 360f));
		}
		else
		{
			if (this.tweenArguments.Contains("x"))
			{
				Vector3[] array = this.vector3s;
				int num = 1;
				array[num].x = array[num].x + 360f * (float)this.tweenArguments["x"];
			}
			if (this.tweenArguments.Contains("y"))
			{
				Vector3[] array2 = this.vector3s;
				int num2 = 1;
				array2[num2].y = array2[num2].y + 360f * (float)this.tweenArguments["y"];
			}
			if (this.tweenArguments.Contains("z"))
			{
				Vector3[] array3 = this.vector3s;
				int num3 = 1;
				array3[num3].z = array3[num3].z + 360f * (float)this.tweenArguments["z"];
			}
		}
		if (this.tweenArguments.Contains("speed"))
		{
			float num4 = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
			this.time = num4 / (float)this.tweenArguments["speed"];
		}
	}

	// Token: 0x060034C5 RID: 13509 RVA: 0x00199528 File Offset: 0x00197728
	private void GenerateShakePositionTargets()
	{
		this.vector3s = new Vector3[4];
		this.vector3s[3] = base.transform.eulerAngles;
		this.vector3s[0] = base.transform.position;
		if (this.tweenArguments.Contains("amount"))
		{
			this.vector3s[1] = (Vector3)this.tweenArguments["amount"];
		}
		else
		{
			if (this.tweenArguments.Contains("x"))
			{
				this.vector3s[1].x = (float)this.tweenArguments["x"];
			}
			if (this.tweenArguments.Contains("y"))
			{
				this.vector3s[1].y = (float)this.tweenArguments["y"];
			}
			if (this.tweenArguments.Contains("z"))
			{
				this.vector3s[1].z = (float)this.tweenArguments["z"];
			}
		}
	}

	// Token: 0x060034C6 RID: 13510 RVA: 0x0019966C File Offset: 0x0019786C
	private void GenerateShakeScaleTargets()
	{
		this.vector3s = new Vector3[3];
		this.vector3s[0] = base.transform.localScale;
		if (this.tweenArguments.Contains("amount"))
		{
			this.vector3s[1] = (Vector3)this.tweenArguments["amount"];
		}
		else
		{
			if (this.tweenArguments.Contains("x"))
			{
				this.vector3s[1].x = (float)this.tweenArguments["x"];
			}
			if (this.tweenArguments.Contains("y"))
			{
				this.vector3s[1].y = (float)this.tweenArguments["y"];
			}
			if (this.tweenArguments.Contains("z"))
			{
				this.vector3s[1].z = (float)this.tweenArguments["z"];
			}
		}
	}

	// Token: 0x060034C7 RID: 13511 RVA: 0x00199794 File Offset: 0x00197994
	private void GenerateShakeRotationTargets()
	{
		this.vector3s = new Vector3[3];
		this.vector3s[0] = base.transform.eulerAngles;
		if (this.tweenArguments.Contains("amount"))
		{
			this.vector3s[1] = (Vector3)this.tweenArguments["amount"];
		}
		else
		{
			if (this.tweenArguments.Contains("x"))
			{
				this.vector3s[1].x = (float)this.tweenArguments["x"];
			}
			if (this.tweenArguments.Contains("y"))
			{
				this.vector3s[1].y = (float)this.tweenArguments["y"];
			}
			if (this.tweenArguments.Contains("z"))
			{
				this.vector3s[1].z = (float)this.tweenArguments["z"];
			}
		}
	}

	// Token: 0x060034C8 RID: 13512 RVA: 0x001998BC File Offset: 0x00197ABC
	private void GeneratePunchPositionTargets()
	{
		this.vector3s = new Vector3[5];
		this.vector3s[4] = base.transform.eulerAngles;
		this.vector3s[0] = base.transform.position;
		this.vector3s[1] = (this.vector3s[3] = Vector3.zero);
		if (this.tweenArguments.Contains("amount"))
		{
			this.vector3s[1] = (Vector3)this.tweenArguments["amount"];
		}
		else
		{
			if (this.tweenArguments.Contains("x"))
			{
				this.vector3s[1].x = (float)this.tweenArguments["x"];
			}
			if (this.tweenArguments.Contains("y"))
			{
				this.vector3s[1].y = (float)this.tweenArguments["y"];
			}
			if (this.tweenArguments.Contains("z"))
			{
				this.vector3s[1].z = (float)this.tweenArguments["z"];
			}
		}
	}

	// Token: 0x060034C9 RID: 13513 RVA: 0x00199A28 File Offset: 0x00197C28
	private void GeneratePunchRotationTargets()
	{
		this.vector3s = new Vector3[4];
		this.vector3s[0] = base.transform.eulerAngles;
		this.vector3s[1] = (this.vector3s[3] = Vector3.zero);
		if (this.tweenArguments.Contains("amount"))
		{
			this.vector3s[1] = (Vector3)this.tweenArguments["amount"];
		}
		else
		{
			if (this.tweenArguments.Contains("x"))
			{
				this.vector3s[1].x = (float)this.tweenArguments["x"];
			}
			if (this.tweenArguments.Contains("y"))
			{
				this.vector3s[1].y = (float)this.tweenArguments["y"];
			}
			if (this.tweenArguments.Contains("z"))
			{
				this.vector3s[1].z = (float)this.tweenArguments["z"];
			}
		}
	}

	// Token: 0x060034CA RID: 13514 RVA: 0x00199B78 File Offset: 0x00197D78
	private void GeneratePunchScaleTargets()
	{
		this.vector3s = new Vector3[3];
		this.vector3s[0] = base.transform.localScale;
		this.vector3s[1] = Vector3.zero;
		if (this.tweenArguments.Contains("amount"))
		{
			this.vector3s[1] = (Vector3)this.tweenArguments["amount"];
		}
		else
		{
			if (this.tweenArguments.Contains("x"))
			{
				this.vector3s[1].x = (float)this.tweenArguments["x"];
			}
			if (this.tweenArguments.Contains("y"))
			{
				this.vector3s[1].y = (float)this.tweenArguments["y"];
			}
			if (this.tweenArguments.Contains("z"))
			{
				this.vector3s[1].z = (float)this.tweenArguments["z"];
			}
		}
	}

	// Token: 0x060034CB RID: 13515 RVA: 0x00199CB4 File Offset: 0x00197EB4
	private void ApplyRectTargets()
	{
		this.rects[2].x = this.ease(this.rects[0].x, this.rects[1].x, this.percentage);
		this.rects[2].y = this.ease(this.rects[0].y, this.rects[1].y, this.percentage);
		this.rects[2].width = this.ease(this.rects[0].width, this.rects[1].width, this.percentage);
		this.rects[2].height = this.ease(this.rects[0].height, this.rects[1].height, this.percentage);
		this.tweenArguments["onupdateparams"] = this.rects[2];
		if (this.percentage == 1f)
		{
			this.tweenArguments["onupdateparams"] = this.rects[1];
		}
	}

	// Token: 0x060034CC RID: 13516 RVA: 0x00199E30 File Offset: 0x00198030
	private void ApplyColorTargets()
	{
		this.colors[0, 2].r = this.ease(this.colors[0, 0].r, this.colors[0, 1].r, this.percentage);
		this.colors[0, 2].g = this.ease(this.colors[0, 0].g, this.colors[0, 1].g, this.percentage);
		this.colors[0, 2].b = this.ease(this.colors[0, 0].b, this.colors[0, 1].b, this.percentage);
		this.colors[0, 2].a = this.ease(this.colors[0, 0].a, this.colors[0, 1].a, this.percentage);
		this.tweenArguments["onupdateparams"] = this.colors[0, 2];
		if (this.percentage == 1f)
		{
			this.tweenArguments["onupdateparams"] = this.colors[0, 1];
		}
	}

	// Token: 0x060034CD RID: 13517 RVA: 0x00199FB0 File Offset: 0x001981B0
	private void ApplyVector3Targets()
	{
		this.vector3s[2].x = this.ease(this.vector3s[0].x, this.vector3s[1].x, this.percentage);
		this.vector3s[2].y = this.ease(this.vector3s[0].y, this.vector3s[1].y, this.percentage);
		this.vector3s[2].z = this.ease(this.vector3s[0].z, this.vector3s[1].z, this.percentage);
		this.tweenArguments["onupdateparams"] = this.vector3s[2];
		if (this.percentage == 1f)
		{
			this.tweenArguments["onupdateparams"] = this.vector3s[1];
		}
	}

	// Token: 0x060034CE RID: 13518 RVA: 0x0019A0E8 File Offset: 0x001982E8
	private void ApplyVector2Targets()
	{
		this.vector2s[2].x = this.ease(this.vector2s[0].x, this.vector2s[1].x, this.percentage);
		this.vector2s[2].y = this.ease(this.vector2s[0].y, this.vector2s[1].y, this.percentage);
		this.tweenArguments["onupdateparams"] = this.vector2s[2];
		if (this.percentage == 1f)
		{
			this.tweenArguments["onupdateparams"] = this.vector2s[1];
		}
	}

	// Token: 0x060034CF RID: 13519 RVA: 0x0019A1DC File Offset: 0x001983DC
	private void ApplyFloatTargets()
	{
		this.floats[2] = this.ease(this.floats[0], this.floats[1], this.percentage);
		this.tweenArguments["onupdateparams"] = this.floats[2];
		if (this.percentage == 1f)
		{
			this.tweenArguments["onupdateparams"] = this.floats[1];
		}
	}

	// Token: 0x060034D0 RID: 13520 RVA: 0x0019A25C File Offset: 0x0019845C
	private void ApplyColorToTargets()
	{
		for (int i = 0; i < this.colors.GetLength(0); i++)
		{
			this.colors[i, 2].r = this.ease(this.colors[i, 0].r, this.colors[i, 1].r, this.percentage);
			this.colors[i, 2].g = this.ease(this.colors[i, 0].g, this.colors[i, 1].g, this.percentage);
			this.colors[i, 2].b = this.ease(this.colors[i, 0].b, this.colors[i, 1].b, this.percentage);
			this.colors[i, 2].a = this.ease(this.colors[i, 0].a, this.colors[i, 1].a, this.percentage);
		}
		if (base.GetComponent(typeof(GUITexture)))
		{
			base.guiTexture.color = this.colors[0, 2];
		}
		else if (base.GetComponent(typeof(GUIText)))
		{
			base.guiText.material.color = this.colors[0, 2];
		}
		else if (base.renderer)
		{
			for (int j = 0; j < this.colors.GetLength(0); j++)
			{
				base.renderer.materials[j].SetColor(this.namedcolorvalue.ToString(), this.colors[j, 2]);
			}
		}
		else if (base.light)
		{
			base.light.color = this.colors[0, 2];
		}
		if (this.percentage == 1f)
		{
			if (base.GetComponent(typeof(GUITexture)))
			{
				base.guiTexture.color = this.colors[0, 1];
			}
			else if (base.GetComponent(typeof(GUIText)))
			{
				base.guiText.material.color = this.colors[0, 1];
			}
			else if (base.renderer)
			{
				for (int k = 0; k < this.colors.GetLength(0); k++)
				{
					base.renderer.materials[k].SetColor(this.namedcolorvalue.ToString(), this.colors[k, 1]);
				}
			}
			else if (base.light)
			{
				base.light.color = this.colors[0, 1];
			}
		}
	}

	// Token: 0x060034D1 RID: 13521 RVA: 0x0019A5AC File Offset: 0x001987AC
	private void ApplyAudioToTargets()
	{
		this.vector2s[2].x = this.ease(this.vector2s[0].x, this.vector2s[1].x, this.percentage);
		this.vector2s[2].y = this.ease(this.vector2s[0].y, this.vector2s[1].y, this.percentage);
		this.audioSource.volume = this.vector2s[2].x;
		this.audioSource.pitch = this.vector2s[2].y;
		if (this.percentage == 1f)
		{
			this.audioSource.volume = this.vector2s[1].x;
			this.audioSource.pitch = this.vector2s[1].y;
		}
	}

	// Token: 0x060034D2 RID: 13522 RVA: 0x0000264F File Offset: 0x0000084F
	private void ApplyStabTargets()
	{
	}

	// Token: 0x060034D3 RID: 13523 RVA: 0x0019A6C4 File Offset: 0x001988C4
	private void ApplyMoveToPathTargets()
	{
		this.preUpdate = base.transform.position;
		float num = this.ease(0f, 1f, this.percentage);
		if (this.isLocal)
		{
			base.transform.localPosition = this.path.Interp(Mathf.Clamp(num, 0f, 1f));
		}
		else
		{
			base.transform.position = this.path.Interp(Mathf.Clamp(num, 0f, 1f));
		}
		if (this.tweenArguments.Contains("orienttopath") && (bool)this.tweenArguments["orienttopath"])
		{
			float num2;
			if (this.tweenArguments.Contains("lookahead"))
			{
				num2 = (float)this.tweenArguments["lookahead"];
			}
			else
			{
				num2 = iTween.Defaults.lookAhead;
			}
			float num3 = this.ease(0f, 1f, Mathf.Min(1f, this.percentage + num2));
			this.tweenArguments["looktarget"] = this.path.Interp(Mathf.Clamp(num3, 0f, 1f));
		}
		this.postUpdate = base.transform.position;
		if (this.physics)
		{
			base.transform.position = this.preUpdate;
			base.rigidbody.MovePosition(this.postUpdate);
		}
	}

	// Token: 0x060034D4 RID: 13524 RVA: 0x0019A858 File Offset: 0x00198A58
	private void ApplyMoveToTargets()
	{
		this.preUpdate = base.transform.position;
		this.vector3s[2].x = this.ease(this.vector3s[0].x, this.vector3s[1].x, this.percentage);
		this.vector3s[2].y = this.ease(this.vector3s[0].y, this.vector3s[1].y, this.percentage);
		this.vector3s[2].z = this.ease(this.vector3s[0].z, this.vector3s[1].z, this.percentage);
		if (this.isLocal)
		{
			base.transform.localPosition = this.vector3s[2];
		}
		else
		{
			base.transform.position = this.vector3s[2];
		}
		if (this.percentage == 1f)
		{
			if (this.isLocal)
			{
				base.transform.localPosition = this.vector3s[1];
			}
			else
			{
				base.transform.position = this.vector3s[1];
			}
		}
		this.postUpdate = base.transform.position;
		if (this.physics)
		{
			base.transform.position = this.preUpdate;
			base.rigidbody.MovePosition(this.postUpdate);
		}
	}

	// Token: 0x060034D5 RID: 13525 RVA: 0x0019AA20 File Offset: 0x00198C20
	private void ApplyMoveByTargets()
	{
		this.preUpdate = base.transform.position;
		Vector3 eulerAngles = default(Vector3);
		if (this.tweenArguments.Contains("looktarget"))
		{
			eulerAngles = base.transform.eulerAngles;
			base.transform.eulerAngles = this.vector3s[4];
		}
		this.vector3s[2].x = this.ease(this.vector3s[0].x, this.vector3s[1].x, this.percentage);
		this.vector3s[2].y = this.ease(this.vector3s[0].y, this.vector3s[1].y, this.percentage);
		this.vector3s[2].z = this.ease(this.vector3s[0].z, this.vector3s[1].z, this.percentage);
		base.transform.Translate(this.vector3s[2] - this.vector3s[3], this.space);
		this.vector3s[3] = this.vector3s[2];
		if (this.tweenArguments.Contains("looktarget"))
		{
			base.transform.eulerAngles = eulerAngles;
		}
		this.postUpdate = base.transform.position;
		if (this.physics)
		{
			base.transform.position = this.preUpdate;
			base.rigidbody.MovePosition(this.postUpdate);
		}
	}

	// Token: 0x060034D6 RID: 13526 RVA: 0x0019AC08 File Offset: 0x00198E08
	private void ApplyScaleToTargets()
	{
		this.vector3s[2].x = this.ease(this.vector3s[0].x, this.vector3s[1].x, this.percentage);
		this.vector3s[2].y = this.ease(this.vector3s[0].y, this.vector3s[1].y, this.percentage);
		this.vector3s[2].z = this.ease(this.vector3s[0].z, this.vector3s[1].z, this.percentage);
		base.transform.localScale = this.vector3s[2];
		if (this.percentage == 1f)
		{
			base.transform.localScale = this.vector3s[1];
		}
	}

	// Token: 0x060034D7 RID: 13527 RVA: 0x0019AD2C File Offset: 0x00198F2C
	private void ApplyLookToTargets()
	{
		this.vector3s[2].x = this.ease(this.vector3s[0].x, this.vector3s[1].x, this.percentage);
		this.vector3s[2].y = this.ease(this.vector3s[0].y, this.vector3s[1].y, this.percentage);
		this.vector3s[2].z = this.ease(this.vector3s[0].z, this.vector3s[1].z, this.percentage);
		if (this.isLocal)
		{
			base.transform.localRotation = Quaternion.Euler(this.vector3s[2]);
		}
		else
		{
			base.transform.rotation = Quaternion.Euler(this.vector3s[2]);
		}
	}

	// Token: 0x060034D8 RID: 13528 RVA: 0x0019AE58 File Offset: 0x00199058
	private void ApplyRotateToTargets()
	{
		this.preUpdate = base.transform.eulerAngles;
		this.vector3s[2].x = this.ease(this.vector3s[0].x, this.vector3s[1].x, this.percentage);
		this.vector3s[2].y = this.ease(this.vector3s[0].y, this.vector3s[1].y, this.percentage);
		this.vector3s[2].z = this.ease(this.vector3s[0].z, this.vector3s[1].z, this.percentage);
		if (this.isLocal)
		{
			base.transform.localRotation = Quaternion.Euler(this.vector3s[2]);
		}
		else
		{
			base.transform.rotation = Quaternion.Euler(this.vector3s[2]);
		}
		if (this.percentage == 1f)
		{
			if (this.isLocal)
			{
				base.transform.localRotation = Quaternion.Euler(this.vector3s[1]);
			}
			else
			{
				base.transform.rotation = Quaternion.Euler(this.vector3s[1]);
			}
		}
		this.postUpdate = base.transform.eulerAngles;
		if (this.physics)
		{
			base.transform.eulerAngles = this.preUpdate;
			base.rigidbody.MoveRotation(Quaternion.Euler(this.postUpdate));
		}
	}

	// Token: 0x060034D9 RID: 13529 RVA: 0x0019B03C File Offset: 0x0019923C
	private void ApplyRotateAddTargets()
	{
		this.preUpdate = base.transform.eulerAngles;
		this.vector3s[2].x = this.ease(this.vector3s[0].x, this.vector3s[1].x, this.percentage);
		this.vector3s[2].y = this.ease(this.vector3s[0].y, this.vector3s[1].y, this.percentage);
		this.vector3s[2].z = this.ease(this.vector3s[0].z, this.vector3s[1].z, this.percentage);
		base.transform.Rotate(this.vector3s[2] - this.vector3s[3], this.space);
		this.vector3s[3] = this.vector3s[2];
		this.postUpdate = base.transform.eulerAngles;
		if (this.physics)
		{
			base.transform.eulerAngles = this.preUpdate;
			base.rigidbody.MoveRotation(Quaternion.Euler(this.postUpdate));
		}
	}

	// Token: 0x060034DA RID: 13530 RVA: 0x0019B1C4 File Offset: 0x001993C4
	private void ApplyShakePositionTargets()
	{
		if (this.isLocal)
		{
			this.preUpdate = base.transform.localPosition;
		}
		else
		{
			this.preUpdate = base.transform.position;
		}
		Vector3 eulerAngles = default(Vector3);
		if (this.tweenArguments.Contains("looktarget"))
		{
			eulerAngles = base.transform.eulerAngles;
			base.transform.eulerAngles = this.vector3s[3];
		}
		if (this.percentage == 0f)
		{
			base.transform.Translate(this.vector3s[1], this.space);
		}
		if (this.isLocal)
		{
			base.transform.localPosition = this.vector3s[0];
		}
		else
		{
			base.transform.position = this.vector3s[0];
		}
		float num = 1f - this.percentage;
		this.vector3s[2].x = Random.Range(-this.vector3s[1].x * num, this.vector3s[1].x * num);
		this.vector3s[2].y = Random.Range(-this.vector3s[1].y * num, this.vector3s[1].y * num);
		this.vector3s[2].z = Random.Range(-this.vector3s[1].z * num, this.vector3s[1].z * num);
		if (this.isLocal)
		{
			base.transform.localPosition += this.vector3s[2];
		}
		else
		{
			base.transform.position += this.vector3s[2];
		}
		if (this.tweenArguments.Contains("looktarget"))
		{
			base.transform.eulerAngles = eulerAngles;
		}
		this.postUpdate = base.transform.position;
		if (this.physics)
		{
			base.transform.position = this.preUpdate;
			base.rigidbody.MovePosition(this.postUpdate);
		}
	}

	// Token: 0x060034DB RID: 13531 RVA: 0x0019B444 File Offset: 0x00199644
	private void ApplyShakeScaleTargets()
	{
		if (this.percentage == 0f)
		{
			base.transform.localScale = this.vector3s[1];
		}
		base.transform.localScale = this.vector3s[0];
		float num = 1f - this.percentage;
		this.vector3s[2].x = Random.Range(-this.vector3s[1].x * num, this.vector3s[1].x * num);
		this.vector3s[2].y = Random.Range(-this.vector3s[1].y * num, this.vector3s[1].y * num);
		this.vector3s[2].z = Random.Range(-this.vector3s[1].z * num, this.vector3s[1].z * num);
		base.transform.localScale += this.vector3s[2];
	}

	// Token: 0x060034DC RID: 13532 RVA: 0x0019B584 File Offset: 0x00199784
	private void ApplyShakeRotationTargets()
	{
		this.preUpdate = base.transform.eulerAngles;
		if (this.percentage == 0f)
		{
			base.transform.Rotate(this.vector3s[1], this.space);
		}
		base.transform.eulerAngles = this.vector3s[0];
		float num = 1f - this.percentage;
		this.vector3s[2].x = Random.Range(-this.vector3s[1].x * num, this.vector3s[1].x * num);
		this.vector3s[2].y = Random.Range(-this.vector3s[1].y * num, this.vector3s[1].y * num);
		this.vector3s[2].z = Random.Range(-this.vector3s[1].z * num, this.vector3s[1].z * num);
		base.transform.Rotate(this.vector3s[2], this.space);
		this.postUpdate = base.transform.eulerAngles;
		if (this.physics)
		{
			base.transform.eulerAngles = this.preUpdate;
			base.rigidbody.MoveRotation(Quaternion.Euler(this.postUpdate));
		}
	}

	// Token: 0x060034DD RID: 13533 RVA: 0x0019B71C File Offset: 0x0019991C
	private void ApplyPunchPositionTargets()
	{
		this.preUpdate = base.transform.position;
		Vector3 eulerAngles = default(Vector3);
		if (this.tweenArguments.Contains("looktarget"))
		{
			eulerAngles = base.transform.eulerAngles;
			base.transform.eulerAngles = this.vector3s[4];
		}
		if (this.vector3s[1].x > 0f)
		{
			this.vector3s[2].x = this.punch(this.vector3s[1].x, this.percentage);
		}
		else if (this.vector3s[1].x < 0f)
		{
			this.vector3s[2].x = -this.punch(Mathf.Abs(this.vector3s[1].x), this.percentage);
		}
		if (this.vector3s[1].y > 0f)
		{
			this.vector3s[2].y = this.punch(this.vector3s[1].y, this.percentage);
		}
		else if (this.vector3s[1].y < 0f)
		{
			this.vector3s[2].y = -this.punch(Mathf.Abs(this.vector3s[1].y), this.percentage);
		}
		if (this.vector3s[1].z > 0f)
		{
			this.vector3s[2].z = this.punch(this.vector3s[1].z, this.percentage);
		}
		else if (this.vector3s[1].z < 0f)
		{
			this.vector3s[2].z = -this.punch(Mathf.Abs(this.vector3s[1].z), this.percentage);
		}
		base.transform.Translate(this.vector3s[2] - this.vector3s[3], this.space);
		this.vector3s[3] = this.vector3s[2];
		if (this.tweenArguments.Contains("looktarget"))
		{
			base.transform.eulerAngles = eulerAngles;
		}
		this.postUpdate = base.transform.position;
		if (this.physics)
		{
			base.transform.position = this.preUpdate;
			base.rigidbody.MovePosition(this.postUpdate);
		}
	}

	// Token: 0x060034DE RID: 13534 RVA: 0x0019BA10 File Offset: 0x00199C10
	private void ApplyPunchRotationTargets()
	{
		this.preUpdate = base.transform.eulerAngles;
		if (this.vector3s[1].x > 0f)
		{
			this.vector3s[2].x = this.punch(this.vector3s[1].x, this.percentage);
		}
		else if (this.vector3s[1].x < 0f)
		{
			this.vector3s[2].x = -this.punch(Mathf.Abs(this.vector3s[1].x), this.percentage);
		}
		if (this.vector3s[1].y > 0f)
		{
			this.vector3s[2].y = this.punch(this.vector3s[1].y, this.percentage);
		}
		else if (this.vector3s[1].y < 0f)
		{
			this.vector3s[2].y = -this.punch(Mathf.Abs(this.vector3s[1].y), this.percentage);
		}
		if (this.vector3s[1].z > 0f)
		{
			this.vector3s[2].z = this.punch(this.vector3s[1].z, this.percentage);
		}
		else if (this.vector3s[1].z < 0f)
		{
			this.vector3s[2].z = -this.punch(Mathf.Abs(this.vector3s[1].z), this.percentage);
		}
		base.transform.Rotate(this.vector3s[2] - this.vector3s[3], this.space);
		this.vector3s[3] = this.vector3s[2];
		this.postUpdate = base.transform.eulerAngles;
		if (this.physics)
		{
			base.transform.eulerAngles = this.preUpdate;
			base.rigidbody.MoveRotation(Quaternion.Euler(this.postUpdate));
		}
	}

	// Token: 0x060034DF RID: 13535 RVA: 0x0019BCA4 File Offset: 0x00199EA4
	private void ApplyPunchScaleTargets()
	{
		if (this.vector3s[1].x > 0f)
		{
			this.vector3s[2].x = this.punch(this.vector3s[1].x, this.percentage);
		}
		else if (this.vector3s[1].x < 0f)
		{
			this.vector3s[2].x = -this.punch(Mathf.Abs(this.vector3s[1].x), this.percentage);
		}
		if (this.vector3s[1].y > 0f)
		{
			this.vector3s[2].y = this.punch(this.vector3s[1].y, this.percentage);
		}
		else if (this.vector3s[1].y < 0f)
		{
			this.vector3s[2].y = -this.punch(Mathf.Abs(this.vector3s[1].y), this.percentage);
		}
		if (this.vector3s[1].z > 0f)
		{
			this.vector3s[2].z = this.punch(this.vector3s[1].z, this.percentage);
		}
		else if (this.vector3s[1].z < 0f)
		{
			this.vector3s[2].z = -this.punch(Mathf.Abs(this.vector3s[1].z), this.percentage);
		}
		base.transform.localScale = this.vector3s[0] + this.vector3s[2];
	}

	// Token: 0x060034E0 RID: 13536 RVA: 0x0019BEBC File Offset: 0x0019A0BC
	private IEnumerator TweenDelay()
	{
		this.delayStarted = Time.time;
		yield return new WaitForSeconds(this.delay);
		if (this.wasPaused)
		{
			this.wasPaused = false;
			this.TweenStart();
		}
		yield break;
	}

	// Token: 0x060034E1 RID: 13537 RVA: 0x0019BED8 File Offset: 0x0019A0D8
	private void TweenStart()
	{
		this.CallBack("onstart");
		if (!this.loop)
		{
			this.ConflictCheck();
			this.GenerateTargets();
		}
		if (this.type == "stab")
		{
			this.audioSource.PlayOneShot(this.audioSource.clip);
		}
		if (this.type == "move" || this.type == "scale" || this.type == "rotate" || this.type == "punch" || this.type == "shake" || this.type == "curve" || this.type == "look")
		{
			this.EnableKinematic();
		}
		this.isRunning = true;
	}

	// Token: 0x060034E2 RID: 13538 RVA: 0x0019BFD4 File Offset: 0x0019A1D4
	private IEnumerator TweenRestart()
	{
		if (this.delay > 0f)
		{
			this.delayStarted = Time.time;
			yield return new WaitForSeconds(this.delay);
		}
		this.loop = true;
		this.TweenStart();
		yield break;
	}

	// Token: 0x060034E3 RID: 13539 RVA: 0x000215C2 File Offset: 0x0001F7C2
	private void TweenUpdate()
	{
		this.apply();
		this.CallBack("onupdate");
		this.UpdatePercentage();
	}

	// Token: 0x060034E4 RID: 13540 RVA: 0x0019BFF0 File Offset: 0x0019A1F0
	private void TweenComplete()
	{
		this.isRunning = false;
		if (this.percentage > 0.5f)
		{
			this.percentage = 1f;
		}
		else
		{
			this.percentage = 0f;
		}
		this.apply();
		if (this.type == "value")
		{
			this.CallBack("onupdate");
		}
		if (this.loopType == iTween.LoopType.none)
		{
			this.Dispose();
		}
		else
		{
			this.TweenLoop();
		}
		this.CallBack("oncomplete");
	}

	// Token: 0x060034E5 RID: 13541 RVA: 0x0019C084 File Offset: 0x0019A284
	private void TweenLoop()
	{
		this.DisableKinematic();
		iTween.LoopType loopType = this.loopType;
		if (loopType != iTween.LoopType.loop)
		{
			if (loopType == iTween.LoopType.pingPong)
			{
				this.reverse = !this.reverse;
				this.runningTime = 0f;
				base.StartCoroutine("TweenRestart");
			}
		}
		else
		{
			this.percentage = 0f;
			this.runningTime = 0f;
			this.apply();
			base.StartCoroutine("TweenRestart");
		}
	}

	// Token: 0x060034E6 RID: 13542 RVA: 0x0019C110 File Offset: 0x0019A310
	public static Rect RectUpdate(Rect currentValue, Rect targetValue, float speed)
	{
		Rect result;
		result..ctor(iTween.FloatUpdate(currentValue.x, targetValue.x, speed), iTween.FloatUpdate(currentValue.y, targetValue.y, speed), iTween.FloatUpdate(currentValue.width, targetValue.width, speed), iTween.FloatUpdate(currentValue.height, targetValue.height, speed));
		return result;
	}

	// Token: 0x060034E7 RID: 13543 RVA: 0x0019C178 File Offset: 0x0019A378
	public static Vector3 Vector3Update(Vector3 currentValue, Vector3 targetValue, float speed)
	{
		Vector3 vector = targetValue - currentValue;
		currentValue += vector * speed * Time.deltaTime;
		return currentValue;
	}

	// Token: 0x060034E8 RID: 13544 RVA: 0x0019C1A8 File Offset: 0x0019A3A8
	public static Vector2 Vector2Update(Vector2 currentValue, Vector2 targetValue, float speed)
	{
		Vector2 vector = targetValue - currentValue;
		currentValue += vector * speed * Time.deltaTime;
		return currentValue;
	}

	// Token: 0x060034E9 RID: 13545 RVA: 0x0019C1D8 File Offset: 0x0019A3D8
	public static float FloatUpdate(float currentValue, float targetValue, float speed)
	{
		float num = targetValue - currentValue;
		currentValue += num * speed * Time.deltaTime;
		return currentValue;
	}

	// Token: 0x060034EA RID: 13546 RVA: 0x000215E0 File Offset: 0x0001F7E0
	public static void FadeUpdate(GameObject target, Hashtable args)
	{
		args["a"] = args["alpha"];
		iTween.ColorUpdate(target, args);
	}

	// Token: 0x060034EB RID: 13547 RVA: 0x000215FF File Offset: 0x0001F7FF
	public static void FadeUpdate(GameObject target, float alpha, float time)
	{
		iTween.FadeUpdate(target, iTween.Hash(new object[]
		{
			"alpha",
			alpha,
			"time",
			time
		}));
	}

	// Token: 0x060034EC RID: 13548 RVA: 0x0019C1F8 File Offset: 0x0019A3F8
	public static void ColorUpdate(GameObject target, Hashtable args)
	{
		iTween.CleanArgs(args);
		Color[] array = new Color[4];
		if (!args.Contains("includechildren") || (bool)args["includechildren"])
		{
			foreach (object obj in target.transform)
			{
				Transform transform = (Transform)obj;
				iTween.ColorUpdate(transform.gameObject, args);
			}
		}
		float num;
		if (args.Contains("time"))
		{
			num = (float)args["time"];
			num *= iTween.Defaults.updateTimePercentage;
		}
		else
		{
			num = iTween.Defaults.updateTime;
		}
		if (target.GetComponent(typeof(GUITexture)))
		{
			array[0] = (array[1] = target.guiTexture.color);
		}
		else if (target.GetComponent(typeof(GUIText)))
		{
			array[0] = (array[1] = target.guiText.material.color);
		}
		else if (target.renderer)
		{
			array[0] = (array[1] = target.renderer.material.color);
		}
		else if (target.light)
		{
			array[0] = (array[1] = target.light.color);
		}
		if (args.Contains("color"))
		{
			array[1] = (Color)args["color"];
		}
		else
		{
			if (args.Contains("r"))
			{
				array[1].r = (float)args["r"];
			}
			if (args.Contains("g"))
			{
				array[1].g = (float)args["g"];
			}
			if (args.Contains("b"))
			{
				array[1].b = (float)args["b"];
			}
			if (args.Contains("a"))
			{
				array[1].a = (float)args["a"];
			}
		}
		array[3].r = Mathf.SmoothDamp(array[0].r, array[1].r, ref array[2].r, num);
		array[3].g = Mathf.SmoothDamp(array[0].g, array[1].g, ref array[2].g, num);
		array[3].b = Mathf.SmoothDamp(array[0].b, array[1].b, ref array[2].b, num);
		array[3].a = Mathf.SmoothDamp(array[0].a, array[1].a, ref array[2].a, num);
		if (target.GetComponent(typeof(GUITexture)))
		{
			target.guiTexture.color = array[3];
		}
		else if (target.GetComponent(typeof(GUIText)))
		{
			target.guiText.material.color = array[3];
		}
		else if (target.renderer)
		{
			target.renderer.material.color = array[3];
		}
		else if (target.light)
		{
			target.light.color = array[3];
		}
	}

	// Token: 0x060034ED RID: 13549 RVA: 0x00021634 File Offset: 0x0001F834
	public static void ColorUpdate(GameObject target, Color color, float time)
	{
		iTween.ColorUpdate(target, iTween.Hash(new object[]
		{
			"color",
			color,
			"time",
			time
		}));
	}

	// Token: 0x060034EE RID: 13550 RVA: 0x0019C65C File Offset: 0x0019A85C
	public static void AudioUpdate(GameObject target, Hashtable args)
	{
		iTween.CleanArgs(args);
		Vector2[] array = new Vector2[4];
		float num;
		if (args.Contains("time"))
		{
			num = (float)args["time"];
			num *= iTween.Defaults.updateTimePercentage;
		}
		else
		{
			num = iTween.Defaults.updateTime;
		}
		AudioSource audioSource;
		if (args.Contains("audiosource"))
		{
			audioSource = (AudioSource)args["audiosource"];
		}
		else
		{
			if (!target.GetComponent(typeof(AudioSource)))
			{
				Debug.LogError("iTween Error: AudioUpdate requires an AudioSource.");
				return;
			}
			audioSource = target.audio;
		}
		array[0] = (array[1] = new Vector2(audioSource.volume, audioSource.pitch));
		if (args.Contains("volume"))
		{
			array[1].x = (float)args["volume"];
		}
		if (args.Contains("pitch"))
		{
			array[1].y = (float)args["pitch"];
		}
		array[3].x = Mathf.SmoothDampAngle(array[0].x, array[1].x, ref array[2].x, num);
		array[3].y = Mathf.SmoothDampAngle(array[0].y, array[1].y, ref array[2].y, num);
		audioSource.volume = array[3].x;
		audioSource.pitch = array[3].y;
	}

	// Token: 0x060034EF RID: 13551 RVA: 0x0019C818 File Offset: 0x0019AA18
	public static void AudioUpdate(GameObject target, float volume, float pitch, float time)
	{
		iTween.AudioUpdate(target, iTween.Hash(new object[]
		{
			"volume",
			volume,
			"pitch",
			pitch,
			"time",
			time
		}));
	}

	// Token: 0x060034F0 RID: 13552 RVA: 0x0019C86C File Offset: 0x0019AA6C
	public static void RotateUpdate(GameObject target, Hashtable args)
	{
		iTween.CleanArgs(args);
		Vector3[] array = new Vector3[4];
		Vector3 eulerAngles = target.transform.eulerAngles;
		float num;
		if (args.Contains("time"))
		{
			num = (float)args["time"];
			num *= iTween.Defaults.updateTimePercentage;
		}
		else
		{
			num = iTween.Defaults.updateTime;
		}
		bool flag;
		if (args.Contains("islocal"))
		{
			flag = (bool)args["islocal"];
		}
		else
		{
			flag = iTween.Defaults.isLocal;
		}
		if (flag)
		{
			array[0] = target.transform.localEulerAngles;
		}
		else
		{
			array[0] = target.transform.eulerAngles;
		}
		if (args.Contains("rotation"))
		{
			if (args["rotation"].GetType() == typeof(Transform))
			{
				Transform transform = (Transform)args["rotation"];
				array[1] = transform.eulerAngles;
			}
			else if (args["rotation"].GetType() == typeof(Vector3))
			{
				array[1] = (Vector3)args["rotation"];
			}
		}
		array[3].x = Mathf.SmoothDampAngle(array[0].x, array[1].x, ref array[2].x, num);
		array[3].y = Mathf.SmoothDampAngle(array[0].y, array[1].y, ref array[2].y, num);
		array[3].z = Mathf.SmoothDampAngle(array[0].z, array[1].z, ref array[2].z, num);
		if (flag)
		{
			target.transform.localEulerAngles = array[3];
		}
		else
		{
			target.transform.eulerAngles = array[3];
		}
		if (target.rigidbody != null)
		{
			Vector3 eulerAngles2 = target.transform.eulerAngles;
			target.transform.eulerAngles = eulerAngles;
			target.rigidbody.MoveRotation(Quaternion.Euler(eulerAngles2));
		}
	}

	// Token: 0x060034F1 RID: 13553 RVA: 0x00021669 File Offset: 0x0001F869
	public static void RotateUpdate(GameObject target, Vector3 rotation, float time)
	{
		iTween.RotateUpdate(target, iTween.Hash(new object[]
		{
			"rotation",
			rotation,
			"time",
			time
		}));
	}

	// Token: 0x060034F2 RID: 13554 RVA: 0x0019CAD8 File Offset: 0x0019ACD8
	public static void ScaleUpdate(GameObject target, Hashtable args)
	{
		iTween.CleanArgs(args);
		Vector3[] array = new Vector3[4];
		float num;
		if (args.Contains("time"))
		{
			num = (float)args["time"];
			num *= iTween.Defaults.updateTimePercentage;
		}
		else
		{
			num = iTween.Defaults.updateTime;
		}
		array[0] = (array[1] = target.transform.localScale);
		if (args.Contains("scale"))
		{
			if (args["scale"].GetType() == typeof(Transform))
			{
				Transform transform = (Transform)args["scale"];
				array[1] = transform.localScale;
			}
			else if (args["scale"].GetType() == typeof(Vector3))
			{
				array[1] = (Vector3)args["scale"];
			}
		}
		else
		{
			if (args.Contains("x"))
			{
				array[1].x = (float)args["x"];
			}
			if (args.Contains("y"))
			{
				array[1].y = (float)args["y"];
			}
			if (args.Contains("z"))
			{
				array[1].z = (float)args["z"];
			}
		}
		array[3].x = Mathf.SmoothDamp(array[0].x, array[1].x, ref array[2].x, num);
		array[3].y = Mathf.SmoothDamp(array[0].y, array[1].y, ref array[2].y, num);
		array[3].z = Mathf.SmoothDamp(array[0].z, array[1].z, ref array[2].z, num);
		target.transform.localScale = array[3];
	}

	// Token: 0x060034F3 RID: 13555 RVA: 0x0002169E File Offset: 0x0001F89E
	public static void ScaleUpdate(GameObject target, Vector3 scale, float time)
	{
		iTween.ScaleUpdate(target, iTween.Hash(new object[]
		{
			"scale",
			scale,
			"time",
			time
		}));
	}

	// Token: 0x060034F4 RID: 13556 RVA: 0x0019CD24 File Offset: 0x0019AF24
	public static void MoveUpdate(GameObject target, Hashtable args)
	{
		iTween.CleanArgs(args);
		Vector3[] array = new Vector3[4];
		Vector3 position = target.transform.position;
		float num;
		if (args.Contains("time"))
		{
			num = (float)args["time"];
			num *= iTween.Defaults.updateTimePercentage;
		}
		else
		{
			num = iTween.Defaults.updateTime;
		}
		bool flag;
		if (args.Contains("islocal"))
		{
			flag = (bool)args["islocal"];
		}
		else
		{
			flag = iTween.Defaults.isLocal;
		}
		if (flag)
		{
			array[0] = (array[1] = target.transform.localPosition);
		}
		else
		{
			array[0] = (array[1] = target.transform.position);
		}
		if (args.Contains("position"))
		{
			if (args["position"].GetType() == typeof(Transform))
			{
				Transform transform = (Transform)args["position"];
				array[1] = transform.position;
			}
			else if (args["position"].GetType() == typeof(Vector3))
			{
				array[1] = (Vector3)args["position"];
			}
		}
		else
		{
			if (args.Contains("x"))
			{
				array[1].x = (float)args["x"];
			}
			if (args.Contains("y"))
			{
				array[1].y = (float)args["y"];
			}
			if (args.Contains("z"))
			{
				array[1].z = (float)args["z"];
			}
		}
		array[3].x = Mathf.SmoothDamp(array[0].x, array[1].x, ref array[2].x, num);
		array[3].y = Mathf.SmoothDamp(array[0].y, array[1].y, ref array[2].y, num);
		array[3].z = Mathf.SmoothDamp(array[0].z, array[1].z, ref array[2].z, num);
		if (args.Contains("orienttopath") && (bool)args["orienttopath"])
		{
			args["looktarget"] = array[3];
		}
		if (args.Contains("looktarget"))
		{
			iTween.LookUpdate(target, args);
		}
		if (flag)
		{
			target.transform.localPosition = array[3];
		}
		else
		{
			target.transform.position = array[3];
		}
		if (target.rigidbody != null)
		{
			Vector3 position2 = target.transform.position;
			target.transform.position = position;
			target.rigidbody.MovePosition(position2);
		}
	}

	// Token: 0x060034F5 RID: 13557 RVA: 0x000216D3 File Offset: 0x0001F8D3
	public static void MoveUpdate(GameObject target, Vector3 position, float time)
	{
		iTween.MoveUpdate(target, iTween.Hash(new object[]
		{
			"position",
			position,
			"time",
			time
		}));
	}

	// Token: 0x060034F6 RID: 13558 RVA: 0x0019D090 File Offset: 0x0019B290
	public static void LookUpdate(GameObject target, Hashtable args)
	{
		iTween.CleanArgs(args);
		Vector3[] array = new Vector3[5];
		float num;
		if (args.Contains("looktime"))
		{
			num = (float)args["looktime"];
			num *= iTween.Defaults.updateTimePercentage;
		}
		else if (args.Contains("time"))
		{
			num = (float)args["time"] * 0.15f;
			num *= iTween.Defaults.updateTimePercentage;
		}
		else
		{
			num = iTween.Defaults.updateTime;
		}
		array[0] = target.transform.eulerAngles;
		if (args.Contains("looktarget"))
		{
			if (args["looktarget"].GetType() == typeof(Transform))
			{
				Transform transform = target.transform;
				Transform transform2 = (Transform)args["looktarget"];
				Vector3? vector = (Vector3?)args["up"];
				transform.LookAt(transform2, (vector == null) ? iTween.Defaults.up : vector.Value);
			}
			else if (args["looktarget"].GetType() == typeof(Vector3))
			{
				Transform transform3 = target.transform;
				Vector3 vector2 = (Vector3)args["looktarget"];
				Vector3? vector3 = (Vector3?)args["up"];
				transform3.LookAt(vector2, (vector3 == null) ? iTween.Defaults.up : vector3.Value);
			}
			array[1] = target.transform.eulerAngles;
			target.transform.eulerAngles = array[0];
			array[3].x = Mathf.SmoothDampAngle(array[0].x, array[1].x, ref array[2].x, num);
			array[3].y = Mathf.SmoothDampAngle(array[0].y, array[1].y, ref array[2].y, num);
			array[3].z = Mathf.SmoothDampAngle(array[0].z, array[1].z, ref array[2].z, num);
			target.transform.eulerAngles = array[3];
			if (args.Contains("axis"))
			{
				array[4] = target.transform.eulerAngles;
				string text = (string)args["axis"];
				if (text != null)
				{
					if (iTween.<>f__switch$map77 == null)
					{
						Dictionary<string, int> dictionary = new Dictionary<string, int>(3);
						dictionary.Add("x", 0);
						dictionary.Add("y", 1);
						dictionary.Add("z", 2);
						iTween.<>f__switch$map77 = dictionary;
					}
					int num2;
					if (iTween.<>f__switch$map77.TryGetValue(text, ref num2))
					{
						switch (num2)
						{
						case 0:
							array[4].y = array[0].y;
							array[4].z = array[0].z;
							break;
						case 1:
							array[4].x = array[0].x;
							array[4].z = array[0].z;
							break;
						case 2:
							array[4].x = array[0].x;
							array[4].y = array[0].y;
							break;
						}
					}
				}
				target.transform.eulerAngles = array[4];
			}
			return;
		}
		Debug.LogError("iTween Error: LookUpdate needs a 'looktarget' property!");
	}

	// Token: 0x060034F7 RID: 13559 RVA: 0x00021708 File Offset: 0x0001F908
	public static void LookUpdate(GameObject target, Vector3 looktarget, float time)
	{
		iTween.LookUpdate(target, iTween.Hash(new object[]
		{
			"looktarget",
			looktarget,
			"time",
			time
		}));
	}

	// Token: 0x060034F8 RID: 13560 RVA: 0x0019D468 File Offset: 0x0019B668
	public static float PathLength(Transform[] path)
	{
		Vector3[] array = new Vector3[path.Length];
		float num = 0f;
		for (int i = 0; i < path.Length; i++)
		{
			array[i] = path[i].position;
		}
		Vector3[] pts = iTween.PathControlPointGenerator(array);
		Vector3 vector = iTween.Interp(pts, 0f);
		int num2 = path.Length * 20;
		for (int j = 1; j <= num2; j++)
		{
			float t = (float)j / (float)num2;
			Vector3 vector2 = iTween.Interp(pts, t);
			num += Vector3.Distance(vector, vector2);
			vector = vector2;
		}
		return num;
	}

	// Token: 0x060034F9 RID: 13561 RVA: 0x0019D504 File Offset: 0x0019B704
	public static float PathLength(Vector3[] path)
	{
		float num = 0f;
		Vector3[] pts = iTween.PathControlPointGenerator(path);
		Vector3 vector = iTween.Interp(pts, 0f);
		int num2 = path.Length * 20;
		for (int i = 1; i <= num2; i++)
		{
			float t = (float)i / (float)num2;
			Vector3 vector2 = iTween.Interp(pts, t);
			num += Vector3.Distance(vector, vector2);
			vector = vector2;
		}
		return num;
	}

	// Token: 0x060034FA RID: 13562 RVA: 0x0019D568 File Offset: 0x0019B768
	public static Texture2D CameraTexture(Color color)
	{
		Texture2D texture2D = new Texture2D(Screen.width, Screen.height, 5, false);
		Color[] array = new Color[Screen.width * Screen.height];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = color;
		}
		texture2D.SetPixels(array);
		texture2D.Apply();
		return texture2D;
	}

	// Token: 0x060034FB RID: 13563 RVA: 0x0002173D File Offset: 0x0001F93D
	public static void PutOnPath(GameObject target, Vector3[] path, float percent)
	{
		target.transform.position = iTween.Interp(iTween.PathControlPointGenerator(path), percent);
	}

	// Token: 0x060034FC RID: 13564 RVA: 0x00021756 File Offset: 0x0001F956
	public static void PutOnPath(Transform target, Vector3[] path, float percent)
	{
		target.position = iTween.Interp(iTween.PathControlPointGenerator(path), percent);
	}

	// Token: 0x060034FD RID: 13565 RVA: 0x0019D5C8 File Offset: 0x0019B7C8
	public static void PutOnPath(GameObject target, Transform[] path, float percent)
	{
		Vector3[] array = new Vector3[path.Length];
		for (int i = 0; i < path.Length; i++)
		{
			array[i] = path[i].position;
		}
		target.transform.position = iTween.Interp(iTween.PathControlPointGenerator(array), percent);
	}

	// Token: 0x060034FE RID: 13566 RVA: 0x0019D620 File Offset: 0x0019B820
	public static void PutOnPath(Transform target, Transform[] path, float percent)
	{
		Vector3[] array = new Vector3[path.Length];
		for (int i = 0; i < path.Length; i++)
		{
			array[i] = path[i].position;
		}
		target.position = iTween.Interp(iTween.PathControlPointGenerator(array), percent);
	}

	// Token: 0x060034FF RID: 13567 RVA: 0x0019D670 File Offset: 0x0019B870
	public static Vector3 PointOnPath(Transform[] path, float percent)
	{
		Vector3[] array = new Vector3[path.Length];
		for (int i = 0; i < path.Length; i++)
		{
			array[i] = path[i].position;
		}
		return iTween.Interp(iTween.PathControlPointGenerator(array), percent);
	}

	// Token: 0x06003500 RID: 13568 RVA: 0x0002176A File Offset: 0x0001F96A
	public static void DrawLine(Vector3[] line)
	{
		if (line.Length > 0)
		{
			iTween.DrawLineHelper(line, iTween.Defaults.color, "gizmos");
		}
	}

	// Token: 0x06003501 RID: 13569 RVA: 0x00021785 File Offset: 0x0001F985
	public static void DrawLine(Vector3[] line, Color color)
	{
		if (line.Length > 0)
		{
			iTween.DrawLineHelper(line, color, "gizmos");
		}
	}

	// Token: 0x06003502 RID: 13570 RVA: 0x0019D6BC File Offset: 0x0019B8BC
	public static void DrawLine(Transform[] line)
	{
		if (line.Length > 0)
		{
			Vector3[] array = new Vector3[line.Length];
			for (int i = 0; i < line.Length; i++)
			{
				array[i] = line[i].position;
			}
			iTween.DrawLineHelper(array, iTween.Defaults.color, "gizmos");
		}
	}

	// Token: 0x06003503 RID: 13571 RVA: 0x0019D714 File Offset: 0x0019B914
	public static void DrawLine(Transform[] line, Color color)
	{
		if (line.Length > 0)
		{
			Vector3[] array = new Vector3[line.Length];
			for (int i = 0; i < line.Length; i++)
			{
				array[i] = line[i].position;
			}
			iTween.DrawLineHelper(array, color, "gizmos");
		}
	}

	// Token: 0x06003504 RID: 13572 RVA: 0x0002176A File Offset: 0x0001F96A
	public static void DrawLineGizmos(Vector3[] line)
	{
		if (line.Length > 0)
		{
			iTween.DrawLineHelper(line, iTween.Defaults.color, "gizmos");
		}
	}

	// Token: 0x06003505 RID: 13573 RVA: 0x00021785 File Offset: 0x0001F985
	public static void DrawLineGizmos(Vector3[] line, Color color)
	{
		if (line.Length > 0)
		{
			iTween.DrawLineHelper(line, color, "gizmos");
		}
	}

	// Token: 0x06003506 RID: 13574 RVA: 0x0019D6BC File Offset: 0x0019B8BC
	public static void DrawLineGizmos(Transform[] line)
	{
		if (line.Length > 0)
		{
			Vector3[] array = new Vector3[line.Length];
			for (int i = 0; i < line.Length; i++)
			{
				array[i] = line[i].position;
			}
			iTween.DrawLineHelper(array, iTween.Defaults.color, "gizmos");
		}
	}

	// Token: 0x06003507 RID: 13575 RVA: 0x0019D714 File Offset: 0x0019B914
	public static void DrawLineGizmos(Transform[] line, Color color)
	{
		if (line.Length > 0)
		{
			Vector3[] array = new Vector3[line.Length];
			for (int i = 0; i < line.Length; i++)
			{
				array[i] = line[i].position;
			}
			iTween.DrawLineHelper(array, color, "gizmos");
		}
	}

	// Token: 0x06003508 RID: 13576 RVA: 0x0002179C File Offset: 0x0001F99C
	public static void DrawLineHandles(Vector3[] line)
	{
		if (line.Length > 0)
		{
			iTween.DrawLineHelper(line, iTween.Defaults.color, "handles");
		}
	}

	// Token: 0x06003509 RID: 13577 RVA: 0x000217B7 File Offset: 0x0001F9B7
	public static void DrawLineHandles(Vector3[] line, Color color)
	{
		if (line.Length > 0)
		{
			iTween.DrawLineHelper(line, color, "handles");
		}
	}

	// Token: 0x0600350A RID: 13578 RVA: 0x0019D768 File Offset: 0x0019B968
	public static void DrawLineHandles(Transform[] line)
	{
		if (line.Length > 0)
		{
			Vector3[] array = new Vector3[line.Length];
			for (int i = 0; i < line.Length; i++)
			{
				array[i] = line[i].position;
			}
			iTween.DrawLineHelper(array, iTween.Defaults.color, "handles");
		}
	}

	// Token: 0x0600350B RID: 13579 RVA: 0x0019D7C0 File Offset: 0x0019B9C0
	public static void DrawLineHandles(Transform[] line, Color color)
	{
		if (line.Length > 0)
		{
			Vector3[] array = new Vector3[line.Length];
			for (int i = 0; i < line.Length; i++)
			{
				array[i] = line[i].position;
			}
			iTween.DrawLineHelper(array, color, "handles");
		}
	}

	// Token: 0x0600350C RID: 13580 RVA: 0x000217CE File Offset: 0x0001F9CE
	public static Vector3 PointOnPath(Vector3[] path, float percent)
	{
		return iTween.Interp(iTween.PathControlPointGenerator(path), percent);
	}

	// Token: 0x0600350D RID: 13581 RVA: 0x000217DC File Offset: 0x0001F9DC
	public static void DrawPath(Vector3[] path)
	{
		if (path.Length > 0)
		{
			iTween.DrawPathHelper(path, iTween.Defaults.color, "gizmos");
		}
	}

	// Token: 0x0600350E RID: 13582 RVA: 0x000217F7 File Offset: 0x0001F9F7
	public static void DrawPath(Vector3[] path, Color color)
	{
		if (path.Length > 0)
		{
			iTween.DrawPathHelper(path, color, "gizmos");
		}
	}

	// Token: 0x0600350F RID: 13583 RVA: 0x0019D814 File Offset: 0x0019BA14
	public static void DrawPath(Transform[] path)
	{
		if (path.Length > 0)
		{
			Vector3[] array = new Vector3[path.Length];
			for (int i = 0; i < path.Length; i++)
			{
				array[i] = path[i].position;
			}
			iTween.DrawPathHelper(array, iTween.Defaults.color, "gizmos");
		}
	}

	// Token: 0x06003510 RID: 13584 RVA: 0x0019D86C File Offset: 0x0019BA6C
	public static void DrawPath(Transform[] path, Color color)
	{
		if (path.Length > 0)
		{
			Vector3[] array = new Vector3[path.Length];
			for (int i = 0; i < path.Length; i++)
			{
				array[i] = path[i].position;
			}
			iTween.DrawPathHelper(array, color, "gizmos");
		}
	}

	// Token: 0x06003511 RID: 13585 RVA: 0x000217DC File Offset: 0x0001F9DC
	public static void DrawPathGizmos(Vector3[] path)
	{
		if (path.Length > 0)
		{
			iTween.DrawPathHelper(path, iTween.Defaults.color, "gizmos");
		}
	}

	// Token: 0x06003512 RID: 13586 RVA: 0x000217F7 File Offset: 0x0001F9F7
	public static void DrawPathGizmos(Vector3[] path, Color color)
	{
		if (path.Length > 0)
		{
			iTween.DrawPathHelper(path, color, "gizmos");
		}
	}

	// Token: 0x06003513 RID: 13587 RVA: 0x0019D814 File Offset: 0x0019BA14
	public static void DrawPathGizmos(Transform[] path)
	{
		if (path.Length > 0)
		{
			Vector3[] array = new Vector3[path.Length];
			for (int i = 0; i < path.Length; i++)
			{
				array[i] = path[i].position;
			}
			iTween.DrawPathHelper(array, iTween.Defaults.color, "gizmos");
		}
	}

	// Token: 0x06003514 RID: 13588 RVA: 0x0019D86C File Offset: 0x0019BA6C
	public static void DrawPathGizmos(Transform[] path, Color color)
	{
		if (path.Length > 0)
		{
			Vector3[] array = new Vector3[path.Length];
			for (int i = 0; i < path.Length; i++)
			{
				array[i] = path[i].position;
			}
			iTween.DrawPathHelper(array, color, "gizmos");
		}
	}

	// Token: 0x06003515 RID: 13589 RVA: 0x0002180E File Offset: 0x0001FA0E
	public static void DrawPathHandles(Vector3[] path)
	{
		if (path.Length > 0)
		{
			iTween.DrawPathHelper(path, iTween.Defaults.color, "handles");
		}
	}

	// Token: 0x06003516 RID: 13590 RVA: 0x00021829 File Offset: 0x0001FA29
	public static void DrawPathHandles(Vector3[] path, Color color)
	{
		if (path.Length > 0)
		{
			iTween.DrawPathHelper(path, color, "handles");
		}
	}

	// Token: 0x06003517 RID: 13591 RVA: 0x0019D8C0 File Offset: 0x0019BAC0
	public static void DrawPathHandles(Transform[] path)
	{
		if (path.Length > 0)
		{
			Vector3[] array = new Vector3[path.Length];
			for (int i = 0; i < path.Length; i++)
			{
				array[i] = path[i].position;
			}
			iTween.DrawPathHelper(array, iTween.Defaults.color, "handles");
		}
	}

	// Token: 0x06003518 RID: 13592 RVA: 0x0019D918 File Offset: 0x0019BB18
	public static void DrawPathHandles(Transform[] path, Color color)
	{
		if (path.Length > 0)
		{
			Vector3[] array = new Vector3[path.Length];
			for (int i = 0; i < path.Length; i++)
			{
				array[i] = path[i].position;
			}
			iTween.DrawPathHelper(array, color, "handles");
		}
	}

	// Token: 0x06003519 RID: 13593 RVA: 0x0019D96C File Offset: 0x0019BB6C
	public static void CameraFadeDepth(int depth)
	{
		if (iTween.cameraFade)
		{
			iTween.cameraFade.transform.position = new Vector3(iTween.cameraFade.transform.position.x, iTween.cameraFade.transform.position.y, (float)depth);
		}
	}

	// Token: 0x0600351A RID: 13594 RVA: 0x00021840 File Offset: 0x0001FA40
	public static void CameraFadeDestroy()
	{
		if (iTween.cameraFade)
		{
			Object.Destroy(iTween.cameraFade);
		}
	}

	// Token: 0x0600351B RID: 13595 RVA: 0x0002185B File Offset: 0x0001FA5B
	public static void CameraFadeSwap(Texture2D texture)
	{
		if (iTween.cameraFade)
		{
			iTween.cameraFade.guiTexture.texture = texture;
		}
	}

	// Token: 0x0600351C RID: 13596 RVA: 0x0019D9CC File Offset: 0x0019BBCC
	public static GameObject CameraFadeAdd(Texture2D texture, int depth)
	{
		if (iTween.cameraFade)
		{
			return null;
		}
		iTween.cameraFade = new GameObject("iTween Camera Fade");
		iTween.cameraFade.transform.position = new Vector3(0.5f, 0.5f, (float)depth);
		iTween.cameraFade.AddComponent("GUITexture");
		iTween.cameraFade.guiTexture.texture = texture;
		iTween.cameraFade.guiTexture.color = new Color(0.5f, 0.5f, 0.5f, 0f);
		return iTween.cameraFade;
	}

	// Token: 0x0600351D RID: 13597 RVA: 0x0019DA68 File Offset: 0x0019BC68
	public static GameObject CameraFadeAdd(Texture2D texture)
	{
		if (iTween.cameraFade)
		{
			return null;
		}
		iTween.cameraFade = new GameObject("iTween Camera Fade");
		iTween.cameraFade.transform.position = new Vector3(0.5f, 0.5f, (float)iTween.Defaults.cameraFadeDepth);
		iTween.cameraFade.AddComponent("GUITexture");
		iTween.cameraFade.guiTexture.texture = texture;
		iTween.cameraFade.guiTexture.color = new Color(0.5f, 0.5f, 0.5f, 0f);
		return iTween.cameraFade;
	}

	// Token: 0x0600351E RID: 13598 RVA: 0x0019DB08 File Offset: 0x0019BD08
	public static GameObject CameraFadeAdd()
	{
		if (iTween.cameraFade)
		{
			return null;
		}
		iTween.cameraFade = new GameObject("iTween Camera Fade");
		iTween.cameraFade.transform.position = new Vector3(0.5f, 0.5f, (float)iTween.Defaults.cameraFadeDepth);
		iTween.cameraFade.AddComponent("GUITexture");
		iTween.cameraFade.guiTexture.texture = iTween.CameraTexture(Color.black);
		iTween.cameraFade.guiTexture.color = new Color(0.5f, 0.5f, 0.5f, 0f);
		return iTween.cameraFade;
	}

	// Token: 0x0600351F RID: 13599 RVA: 0x0019DBB0 File Offset: 0x0019BDB0
	public static void Resume(GameObject target)
	{
		Component[] components = target.GetComponents(typeof(iTween));
		foreach (iTween iTween in components)
		{
			iTween.enabled = true;
		}
	}

	// Token: 0x06003520 RID: 13600 RVA: 0x0019DBF4 File Offset: 0x0019BDF4
	public static void Resume(GameObject target, bool includechildren)
	{
		iTween.Resume(target);
		if (includechildren)
		{
			foreach (object obj in target.transform)
			{
				Transform transform = (Transform)obj;
				iTween.Resume(transform.gameObject, true);
			}
		}
	}

	// Token: 0x06003521 RID: 13601 RVA: 0x0019DC68 File Offset: 0x0019BE68
	public static void Resume(GameObject target, string type)
	{
		Component[] components = target.GetComponents(typeof(iTween));
		foreach (iTween iTween in components)
		{
			string text = iTween.type + iTween.method;
			text = text.Substring(0, type.Length);
			if (text.ToLower() == type.ToLower())
			{
				iTween.enabled = true;
			}
		}
	}

	// Token: 0x06003522 RID: 13602 RVA: 0x0019DCE8 File Offset: 0x0019BEE8
	public static void Resume(GameObject target, string type, bool includechildren)
	{
		Component[] components = target.GetComponents(typeof(iTween));
		foreach (iTween iTween in components)
		{
			string text = iTween.type + iTween.method;
			text = text.Substring(0, type.Length);
			if (text.ToLower() == type.ToLower())
			{
				iTween.enabled = true;
			}
		}
		if (includechildren)
		{
			foreach (object obj in target.transform)
			{
				Transform transform = (Transform)obj;
				iTween.Resume(transform.gameObject, type, true);
			}
		}
	}

	// Token: 0x06003523 RID: 13603 RVA: 0x0019DDD4 File Offset: 0x0019BFD4
	public static void Resume()
	{
		for (int i = 0; i < iTween.tweens.Count; i++)
		{
			Hashtable hashtable = (Hashtable)iTween.tweens[i];
			GameObject target = (GameObject)hashtable["target"];
			iTween.Resume(target);
		}
	}

	// Token: 0x06003524 RID: 13604 RVA: 0x0019DE24 File Offset: 0x0019C024
	public static void Resume(string type)
	{
		ArrayList arrayList = new ArrayList();
		for (int i = 0; i < iTween.tweens.Count; i++)
		{
			Hashtable hashtable = (Hashtable)iTween.tweens[i];
			GameObject gameObject = (GameObject)hashtable["target"];
			arrayList.Insert(arrayList.Count, gameObject);
		}
		for (int j = 0; j < arrayList.Count; j++)
		{
			iTween.Resume((GameObject)arrayList[j], type);
		}
	}

	// Token: 0x06003525 RID: 13605 RVA: 0x0019DEB0 File Offset: 0x0019C0B0
	public static void Pause(GameObject target)
	{
		Component[] components = target.GetComponents(typeof(iTween));
		foreach (iTween iTween in components)
		{
			if (iTween.delay > 0f)
			{
				iTween.delay -= Time.time - iTween.delayStarted;
				iTween.StopCoroutine("TweenDelay");
			}
			iTween.isPaused = true;
			iTween.enabled = false;
		}
	}

	// Token: 0x06003526 RID: 13606 RVA: 0x0019DF30 File Offset: 0x0019C130
	public static void Pause(GameObject target, bool includechildren)
	{
		iTween.Pause(target);
		if (includechildren)
		{
			foreach (object obj in target.transform)
			{
				Transform transform = (Transform)obj;
				iTween.Pause(transform.gameObject, true);
			}
		}
	}

	// Token: 0x06003527 RID: 13607 RVA: 0x0019DFA4 File Offset: 0x0019C1A4
	public static void Pause(GameObject target, string type)
	{
		Component[] components = target.GetComponents(typeof(iTween));
		foreach (iTween iTween in components)
		{
			string text = iTween.type + iTween.method;
			text = text.Substring(0, type.Length);
			if (text.ToLower() == type.ToLower())
			{
				if (iTween.delay > 0f)
				{
					iTween.delay -= Time.time - iTween.delayStarted;
					iTween.StopCoroutine("TweenDelay");
				}
				iTween.isPaused = true;
				iTween.enabled = false;
			}
		}
	}

	// Token: 0x06003528 RID: 13608 RVA: 0x0019E060 File Offset: 0x0019C260
	public static void Pause(GameObject target, string type, bool includechildren)
	{
		Component[] components = target.GetComponents(typeof(iTween));
		foreach (iTween iTween in components)
		{
			string text = iTween.type + iTween.method;
			text = text.Substring(0, type.Length);
			if (text.ToLower() == type.ToLower())
			{
				if (iTween.delay > 0f)
				{
					iTween.delay -= Time.time - iTween.delayStarted;
					iTween.StopCoroutine("TweenDelay");
				}
				iTween.isPaused = true;
				iTween.enabled = false;
			}
		}
		if (includechildren)
		{
			foreach (object obj in target.transform)
			{
				Transform transform = (Transform)obj;
				iTween.Pause(transform.gameObject, type, true);
			}
		}
	}

	// Token: 0x06003529 RID: 13609 RVA: 0x0019E184 File Offset: 0x0019C384
	public static void Pause()
	{
		for (int i = 0; i < iTween.tweens.Count; i++)
		{
			Hashtable hashtable = (Hashtable)iTween.tweens[i];
			GameObject target = (GameObject)hashtable["target"];
			iTween.Pause(target);
		}
	}

	// Token: 0x0600352A RID: 13610 RVA: 0x0019E1D4 File Offset: 0x0019C3D4
	public static void Pause(string type)
	{
		ArrayList arrayList = new ArrayList();
		for (int i = 0; i < iTween.tweens.Count; i++)
		{
			Hashtable hashtable = (Hashtable)iTween.tweens[i];
			GameObject gameObject = (GameObject)hashtable["target"];
			arrayList.Insert(arrayList.Count, gameObject);
		}
		for (int j = 0; j < arrayList.Count; j++)
		{
			iTween.Pause((GameObject)arrayList[j], type);
		}
	}

	// Token: 0x0600352B RID: 13611 RVA: 0x0002187C File Offset: 0x0001FA7C
	public static int Count()
	{
		return iTween.tweens.Count;
	}

	// Token: 0x0600352C RID: 13612 RVA: 0x0019E260 File Offset: 0x0019C460
	public static int Count(string type)
	{
		int num = 0;
		for (int i = 0; i < iTween.tweens.Count; i++)
		{
			Hashtable hashtable = (Hashtable)iTween.tweens[i];
			string text = (string)hashtable["type"] + (string)hashtable["method"];
			text = text.Substring(0, type.Length);
			if (text.ToLower() == type.ToLower())
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x0600352D RID: 13613 RVA: 0x0019E2EC File Offset: 0x0019C4EC
	public static int Count(GameObject target)
	{
		Component[] components = target.GetComponents(typeof(iTween));
		return components.Length;
	}

	// Token: 0x0600352E RID: 13614 RVA: 0x0019E310 File Offset: 0x0019C510
	public static int Count(GameObject target, string type)
	{
		int num = 0;
		Component[] components = target.GetComponents(typeof(iTween));
		foreach (iTween iTween in components)
		{
			string text = iTween.type + iTween.method;
			text = text.Substring(0, type.Length);
			if (text.ToLower() == type.ToLower())
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x0600352F RID: 13615 RVA: 0x0019E394 File Offset: 0x0019C594
	public static void Stop()
	{
		for (int i = 0; i < iTween.tweens.Count; i++)
		{
			Hashtable hashtable = (Hashtable)iTween.tweens[i];
			GameObject target = (GameObject)hashtable["target"];
			iTween.Stop(target);
		}
		iTween.tweens.Clear();
	}

	// Token: 0x06003530 RID: 13616 RVA: 0x0019E3F0 File Offset: 0x0019C5F0
	public static void Stop(string type)
	{
		ArrayList arrayList = new ArrayList();
		for (int i = 0; i < iTween.tweens.Count; i++)
		{
			Hashtable hashtable = (Hashtable)iTween.tweens[i];
			GameObject gameObject = (GameObject)hashtable["target"];
			arrayList.Insert(arrayList.Count, gameObject);
		}
		for (int j = 0; j < arrayList.Count; j++)
		{
			iTween.Stop((GameObject)arrayList[j], type);
		}
	}

	// Token: 0x06003531 RID: 13617 RVA: 0x0019E47C File Offset: 0x0019C67C
	public static void StopByName(string name)
	{
		ArrayList arrayList = new ArrayList();
		for (int i = 0; i < iTween.tweens.Count; i++)
		{
			Hashtable hashtable = (Hashtable)iTween.tweens[i];
			GameObject gameObject = (GameObject)hashtable["target"];
			arrayList.Insert(arrayList.Count, gameObject);
		}
		for (int j = 0; j < arrayList.Count; j++)
		{
			iTween.StopByName((GameObject)arrayList[j], name);
		}
	}

	// Token: 0x06003532 RID: 13618 RVA: 0x0019E508 File Offset: 0x0019C708
	public static void Stop(GameObject target)
	{
		Component[] components = target.GetComponents(typeof(iTween));
		foreach (iTween iTween in components)
		{
			iTween.Dispose();
		}
	}

	// Token: 0x06003533 RID: 13619 RVA: 0x0019E54C File Offset: 0x0019C74C
	public static void Stop(GameObject target, bool includechildren)
	{
		iTween.Stop(target);
		if (includechildren)
		{
			foreach (object obj in target.transform)
			{
				Transform transform = (Transform)obj;
				iTween.Stop(transform.gameObject, true);
			}
		}
	}

	// Token: 0x06003534 RID: 13620 RVA: 0x0019E5C0 File Offset: 0x0019C7C0
	public static void Stop(GameObject target, string type)
	{
		Component[] components = target.GetComponents(typeof(iTween));
		foreach (iTween iTween in components)
		{
			string text = iTween.type + iTween.method;
			text = text.Substring(0, type.Length);
			if (text.ToLower() == type.ToLower())
			{
				iTween.Dispose();
			}
		}
	}

	// Token: 0x06003535 RID: 13621 RVA: 0x0019E640 File Offset: 0x0019C840
	public static void StopByName(GameObject target, string name)
	{
		Component[] components = target.GetComponents(typeof(iTween));
		foreach (iTween iTween in components)
		{
			if (iTween._name == name)
			{
				iTween.Dispose();
			}
		}
	}

	// Token: 0x06003536 RID: 13622 RVA: 0x0019E694 File Offset: 0x0019C894
	public static void Stop(GameObject target, string type, bool includechildren)
	{
		Component[] components = target.GetComponents(typeof(iTween));
		foreach (iTween iTween in components)
		{
			string text = iTween.type + iTween.method;
			text = text.Substring(0, type.Length);
			if (text.ToLower() == type.ToLower())
			{
				iTween.Dispose();
			}
		}
		if (includechildren)
		{
			foreach (object obj in target.transform)
			{
				Transform transform = (Transform)obj;
				iTween.Stop(transform.gameObject, type, true);
			}
		}
	}

	// Token: 0x06003537 RID: 13623 RVA: 0x0019E77C File Offset: 0x0019C97C
	public static void StopByName(GameObject target, string name, bool includechildren)
	{
		Component[] components = target.GetComponents(typeof(iTween));
		foreach (iTween iTween in components)
		{
			if (iTween._name == name)
			{
				iTween.Dispose();
			}
		}
		if (includechildren)
		{
			foreach (object obj in target.transform)
			{
				Transform transform = (Transform)obj;
				iTween.StopByName(transform.gameObject, name, true);
			}
		}
	}

	// Token: 0x06003538 RID: 13624 RVA: 0x0019E83C File Offset: 0x0019CA3C
	public static Hashtable Hash(params object[] args)
	{
		Hashtable hashtable = new Hashtable(args.Length / 2);
		if (args.Length % 2 != 0)
		{
			Debug.LogError("Tween Error: Hash requires an even number of arguments!");
			return null;
		}
		for (int i = 0; i < args.Length - 1; i += 2)
		{
			hashtable.Add(args[i], args[i + 1]);
		}
		return hashtable;
	}

	// Token: 0x06003539 RID: 13625 RVA: 0x00021888 File Offset: 0x0001FA88
	private void Awake()
	{
		this.RetrieveArgs();
		this.lastRealTime = Time.realtimeSinceStartup;
	}

	// Token: 0x0600353A RID: 13626 RVA: 0x0019E890 File Offset: 0x0019CA90
	private IEnumerator Start()
	{
		if (this.delay > 0f)
		{
			yield return base.StartCoroutine("TweenDelay");
		}
		this.TweenStart();
		yield break;
	}

	// Token: 0x0600353B RID: 13627 RVA: 0x0019E8AC File Offset: 0x0019CAAC
	private void Update()
	{
		if (this.isRunning && !this.physics)
		{
			if (!this.reverse)
			{
				if (this.percentage < 1f)
				{
					this.TweenUpdate();
				}
				else
				{
					this.TweenComplete();
				}
			}
			else if (this.percentage > 0f)
			{
				this.TweenUpdate();
			}
			else
			{
				this.TweenComplete();
			}
		}
	}

	// Token: 0x0600353C RID: 13628 RVA: 0x0019E924 File Offset: 0x0019CB24
	private void FixedUpdate()
	{
		if (this.isRunning && this.physics)
		{
			if (!this.reverse)
			{
				if (this.percentage < 1f)
				{
					this.TweenUpdate();
				}
				else
				{
					this.TweenComplete();
				}
			}
			else if (this.percentage > 0f)
			{
				this.TweenUpdate();
			}
			else
			{
				this.TweenComplete();
			}
		}
	}

	// Token: 0x0600353D RID: 13629 RVA: 0x0019E99C File Offset: 0x0019CB9C
	private void LateUpdate()
	{
		if (this.tweenArguments.Contains("looktarget") && this.isRunning && (this.type == "move" || this.type == "shake" || this.type == "punch"))
		{
			iTween.LookUpdate(base.gameObject, this.tweenArguments);
		}
	}

	// Token: 0x0600353E RID: 13630 RVA: 0x0019EA1C File Offset: 0x0019CC1C
	private void OnEnable()
	{
		if (this.isRunning)
		{
			this.EnableKinematic();
		}
		if (this.isPaused)
		{
			this.isPaused = false;
			if (this.delay > 0f)
			{
				this.wasPaused = true;
				this.ResumeDelay();
			}
		}
	}

	// Token: 0x0600353F RID: 13631 RVA: 0x0002189B File Offset: 0x0001FA9B
	private void OnDisable()
	{
		this.DisableKinematic();
	}

	// Token: 0x06003540 RID: 13632 RVA: 0x0019EA6C File Offset: 0x0019CC6C
	private static void DrawLineHelper(Vector3[] line, Color color, string method)
	{
		Gizmos.color = color;
		for (int i = 0; i < line.Length - 1; i++)
		{
			if (method == "gizmos")
			{
				Gizmos.DrawLine(line[i], line[i + 1]);
			}
			else if (method == "handles")
			{
				Debug.LogError("iTween Error: Drawing a line with Handles is temporarily disabled because of compatability issues with Unity 2.6!");
			}
		}
	}

	// Token: 0x06003541 RID: 13633 RVA: 0x0019EAE4 File Offset: 0x0019CCE4
	private static void DrawPathHelper(Vector3[] path, Color color, string method)
	{
		Vector3[] pts = iTween.PathControlPointGenerator(path);
		Vector3 vector = iTween.Interp(pts, 0f);
		Gizmos.color = color;
		int num = path.Length * 20;
		for (int i = 1; i <= num; i++)
		{
			float t = (float)i / (float)num;
			Vector3 vector2 = iTween.Interp(pts, t);
			if (method == "gizmos")
			{
				Gizmos.DrawLine(vector2, vector);
			}
			else if (method == "handles")
			{
				Debug.LogError("iTween Error: Drawing a path with Handles is temporarily disabled because of compatability issues with Unity 2.6!");
			}
			vector = vector2;
		}
	}

	// Token: 0x06003542 RID: 13634 RVA: 0x0019EB70 File Offset: 0x0019CD70
	private static Vector3[] PathControlPointGenerator(Vector3[] path)
	{
		int num = 2;
		Vector3[] array = new Vector3[path.Length + num];
		Array.Copy(path, 0, array, 1, path.Length);
		array[0] = array[1] + (array[1] - array[2]);
		array[array.Length - 1] = array[array.Length - 2] + (array[array.Length - 2] - array[array.Length - 3]);
		if (array[1] == array[array.Length - 2])
		{
			Vector3[] array2 = new Vector3[array.Length];
			Array.Copy(array, array2, array.Length);
			array2[0] = array2[array2.Length - 3];
			array2[array2.Length - 1] = array2[2];
			array = new Vector3[array2.Length];
			Array.Copy(array2, array, array2.Length);
		}
		return array;
	}

	// Token: 0x06003543 RID: 13635 RVA: 0x0019ECA4 File Offset: 0x0019CEA4
	private static Vector3 Interp(Vector3[] pts, float t)
	{
		int num = pts.Length - 3;
		int num2 = Mathf.Min(Mathf.FloorToInt(t * (float)num), num - 1);
		float num3 = t * (float)num - (float)num2;
		Vector3 vector = pts[num2];
		Vector3 vector2 = pts[num2 + 1];
		Vector3 vector3 = pts[num2 + 2];
		Vector3 vector4 = pts[num2 + 3];
		return 0.5f * ((-vector + 3f * vector2 - 3f * vector3 + vector4) * (num3 * num3 * num3) + (2f * vector - 5f * vector2 + 4f * vector3 - vector4) * (num3 * num3) + (-vector + vector3) * num3 + 2f * vector2);
	}

	// Token: 0x06003544 RID: 13636 RVA: 0x0019EDBC File Offset: 0x0019CFBC
	private static void Launch(GameObject target, Hashtable args)
	{
		if (!args.Contains("id"))
		{
			args["id"] = iTween.GenerateID();
		}
		if (!args.Contains("target"))
		{
			args["target"] = target;
		}
		iTween.tweens.Insert(0, args);
		target.AddComponent("iTween");
	}

	// Token: 0x06003545 RID: 13637 RVA: 0x0019EE20 File Offset: 0x0019D020
	private static Hashtable CleanArgs(Hashtable args)
	{
		Hashtable hashtable = new Hashtable(args.Count);
		Hashtable hashtable2 = new Hashtable(args.Count);
		foreach (object obj in args)
		{
			DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
			hashtable.Add(dictionaryEntry.Key, dictionaryEntry.Value);
		}
		foreach (object obj2 in hashtable)
		{
			DictionaryEntry dictionaryEntry2 = (DictionaryEntry)obj2;
			if (dictionaryEntry2.Value.GetType() == typeof(int))
			{
				int num = (int)dictionaryEntry2.Value;
				float num2 = (float)num;
				args[dictionaryEntry2.Key] = num2;
			}
			if (dictionaryEntry2.Value.GetType() == typeof(double))
			{
				double num3 = (double)dictionaryEntry2.Value;
				float num4 = (float)num3;
				args[dictionaryEntry2.Key] = num4;
			}
		}
		foreach (object obj3 in args)
		{
			DictionaryEntry dictionaryEntry3 = (DictionaryEntry)obj3;
			hashtable2.Add(dictionaryEntry3.Key.ToString().ToLower(), dictionaryEntry3.Value);
		}
		args = hashtable2;
		return args;
	}

	// Token: 0x06003546 RID: 13638 RVA: 0x0019EFE8 File Offset: 0x0019D1E8
	private static string GenerateID()
	{
		int num = 15;
		char[] array = new char[]
		{
			'a',
			'b',
			'c',
			'd',
			'e',
			'f',
			'g',
			'h',
			'i',
			'j',
			'k',
			'l',
			'm',
			'n',
			'o',
			'p',
			'q',
			'r',
			's',
			't',
			'u',
			'v',
			'w',
			'x',
			'y',
			'z',
			'A',
			'B',
			'C',
			'D',
			'E',
			'F',
			'G',
			'H',
			'I',
			'J',
			'K',
			'L',
			'M',
			'N',
			'O',
			'P',
			'Q',
			'R',
			'S',
			'T',
			'U',
			'V',
			'W',
			'X',
			'Y',
			'Z',
			'0',
			'1',
			'2',
			'3',
			'4',
			'5',
			'6',
			'7',
			'8'
		};
		int num2 = array.Length - 1;
		string text = string.Empty;
		for (int i = 0; i < num; i++)
		{
			text += array[(int)Mathf.Floor((float)Random.Range(0, num2))];
		}
		return text;
	}

	// Token: 0x06003547 RID: 13639 RVA: 0x0019F04C File Offset: 0x0019D24C
	private void RetrieveArgs()
	{
		foreach (object obj in iTween.tweens)
		{
			Hashtable hashtable = (Hashtable)obj;
			if ((GameObject)hashtable["target"] == base.gameObject)
			{
				this.tweenArguments = hashtable;
				break;
			}
		}
		this.id = (string)this.tweenArguments["id"];
		this.type = (string)this.tweenArguments["type"];
		this._name = (string)this.tweenArguments["name"];
		this.method = (string)this.tweenArguments["method"];
		if (this.tweenArguments.Contains("time"))
		{
			this.time = (float)this.tweenArguments["time"];
		}
		else
		{
			this.time = iTween.Defaults.time;
		}
		if (base.rigidbody != null)
		{
			this.physics = true;
		}
		if (this.tweenArguments.Contains("delay"))
		{
			this.delay = (float)this.tweenArguments["delay"];
		}
		else
		{
			this.delay = iTween.Defaults.delay;
		}
		if (this.tweenArguments.Contains("namedcolorvalue"))
		{
			if (this.tweenArguments["namedcolorvalue"].GetType() == typeof(iTween.NamedValueColor))
			{
				this.namedcolorvalue = (iTween.NamedValueColor)((int)this.tweenArguments["namedcolorvalue"]);
			}
			else
			{
				try
				{
					this.namedcolorvalue = (iTween.NamedValueColor)((int)Enum.Parse(typeof(iTween.NamedValueColor), (string)this.tweenArguments["namedcolorvalue"], true));
				}
				catch
				{
					Debug.LogWarning("iTween: Unsupported namedcolorvalue supplied! Default will be used.");
					this.namedcolorvalue = iTween.NamedValueColor._Color;
				}
			}
		}
		else
		{
			this.namedcolorvalue = iTween.Defaults.namedColorValue;
		}
		if (this.tweenArguments.Contains("looptype"))
		{
			if (this.tweenArguments["looptype"].GetType() == typeof(iTween.LoopType))
			{
				this.loopType = (iTween.LoopType)((int)this.tweenArguments["looptype"]);
			}
			else
			{
				try
				{
					this.loopType = (iTween.LoopType)((int)Enum.Parse(typeof(iTween.LoopType), (string)this.tweenArguments["looptype"], true));
				}
				catch
				{
					Debug.LogWarning("iTween: Unsupported loopType supplied! Default will be used.");
					this.loopType = iTween.LoopType.none;
				}
			}
		}
		else
		{
			this.loopType = iTween.LoopType.none;
		}
		if (this.tweenArguments.Contains("easetype"))
		{
			if (this.tweenArguments["easetype"].GetType() == typeof(iTween.EaseType))
			{
				this.easeType = (iTween.EaseType)((int)this.tweenArguments["easetype"]);
			}
			else
			{
				try
				{
					this.easeType = (iTween.EaseType)((int)Enum.Parse(typeof(iTween.EaseType), (string)this.tweenArguments["easetype"], true));
				}
				catch
				{
					Debug.LogWarning("iTween: Unsupported easeType supplied! Default will be used.");
					this.easeType = iTween.Defaults.easeType;
				}
			}
		}
		else
		{
			this.easeType = iTween.Defaults.easeType;
		}
		if (this.tweenArguments.Contains("space"))
		{
			if (this.tweenArguments["space"].GetType() == typeof(Space))
			{
				this.space = (int)this.tweenArguments["space"];
			}
			else
			{
				try
				{
					this.space = (int)Enum.Parse(typeof(Space), (string)this.tweenArguments["space"], true);
				}
				catch
				{
					Debug.LogWarning("iTween: Unsupported space supplied! Default will be used.");
					this.space = iTween.Defaults.space;
				}
			}
		}
		else
		{
			this.space = iTween.Defaults.space;
		}
		if (this.tweenArguments.Contains("islocal"))
		{
			this.isLocal = (bool)this.tweenArguments["islocal"];
		}
		else
		{
			this.isLocal = iTween.Defaults.isLocal;
		}
		if (this.tweenArguments.Contains("ignoretimescale"))
		{
			this.useRealTime = (bool)this.tweenArguments["ignoretimescale"];
		}
		else
		{
			this.useRealTime = iTween.Defaults.useRealTime;
		}
		this.GetEasingFunction();
	}

	// Token: 0x06003548 RID: 13640 RVA: 0x0019F568 File Offset: 0x0019D768
	private void GetEasingFunction()
	{
		switch (this.easeType)
		{
		case iTween.EaseType.easeInQuad:
			this.ease = new iTween.EasingFunction(this.easeInQuad);
			break;
		case iTween.EaseType.easeOutQuad:
			this.ease = new iTween.EasingFunction(this.easeOutQuad);
			break;
		case iTween.EaseType.easeInOutQuad:
			this.ease = new iTween.EasingFunction(this.easeInOutQuad);
			break;
		case iTween.EaseType.easeInCubic:
			this.ease = new iTween.EasingFunction(this.easeInCubic);
			break;
		case iTween.EaseType.easeOutCubic:
			this.ease = new iTween.EasingFunction(this.easeOutCubic);
			break;
		case iTween.EaseType.easeInOutCubic:
			this.ease = new iTween.EasingFunction(this.easeInOutCubic);
			break;
		case iTween.EaseType.easeInQuart:
			this.ease = new iTween.EasingFunction(this.easeInQuart);
			break;
		case iTween.EaseType.easeOutQuart:
			this.ease = new iTween.EasingFunction(this.easeOutQuart);
			break;
		case iTween.EaseType.easeInOutQuart:
			this.ease = new iTween.EasingFunction(this.easeInOutQuart);
			break;
		case iTween.EaseType.easeInQuint:
			this.ease = new iTween.EasingFunction(this.easeInQuint);
			break;
		case iTween.EaseType.easeOutQuint:
			this.ease = new iTween.EasingFunction(this.easeOutQuint);
			break;
		case iTween.EaseType.easeInOutQuint:
			this.ease = new iTween.EasingFunction(this.easeInOutQuint);
			break;
		case iTween.EaseType.easeInSine:
			this.ease = new iTween.EasingFunction(this.easeInSine);
			break;
		case iTween.EaseType.easeOutSine:
			this.ease = new iTween.EasingFunction(this.easeOutSine);
			break;
		case iTween.EaseType.easeInOutSine:
			this.ease = new iTween.EasingFunction(this.easeInOutSine);
			break;
		case iTween.EaseType.easeInExpo:
			this.ease = new iTween.EasingFunction(this.easeInExpo);
			break;
		case iTween.EaseType.easeOutExpo:
			this.ease = new iTween.EasingFunction(this.easeOutExpo);
			break;
		case iTween.EaseType.easeInOutExpo:
			this.ease = new iTween.EasingFunction(this.easeInOutExpo);
			break;
		case iTween.EaseType.easeInCirc:
			this.ease = new iTween.EasingFunction(this.easeInCirc);
			break;
		case iTween.EaseType.easeOutCirc:
			this.ease = new iTween.EasingFunction(this.easeOutCirc);
			break;
		case iTween.EaseType.easeInOutCirc:
			this.ease = new iTween.EasingFunction(this.easeInOutCirc);
			break;
		case iTween.EaseType.linear:
			this.ease = new iTween.EasingFunction(this.linear);
			break;
		case iTween.EaseType.spring:
			this.ease = new iTween.EasingFunction(this.spring);
			break;
		case iTween.EaseType.easeInBounce:
			this.ease = new iTween.EasingFunction(this.easeInBounce);
			break;
		case iTween.EaseType.easeOutBounce:
			this.ease = new iTween.EasingFunction(this.easeOutBounce);
			break;
		case iTween.EaseType.easeInOutBounce:
			this.ease = new iTween.EasingFunction(this.easeInOutBounce);
			break;
		case iTween.EaseType.easeInBack:
			this.ease = new iTween.EasingFunction(this.easeInBack);
			break;
		case iTween.EaseType.easeOutBack:
			this.ease = new iTween.EasingFunction(this.easeOutBack);
			break;
		case iTween.EaseType.easeInOutBack:
			this.ease = new iTween.EasingFunction(this.easeInOutBack);
			break;
		case iTween.EaseType.easeInElastic:
			this.ease = new iTween.EasingFunction(this.easeInElastic);
			break;
		case iTween.EaseType.easeOutElastic:
			this.ease = new iTween.EasingFunction(this.easeOutElastic);
			break;
		case iTween.EaseType.easeInOutElastic:
			this.ease = new iTween.EasingFunction(this.easeInOutElastic);
			break;
		}
	}

	// Token: 0x06003549 RID: 13641 RVA: 0x0019F8E8 File Offset: 0x0019DAE8
	private void UpdatePercentage()
	{
		if (this.useRealTime)
		{
			this.runningTime += Time.realtimeSinceStartup - this.lastRealTime;
		}
		else
		{
			this.runningTime += Time.deltaTime;
		}
		if (this.reverse)
		{
			this.percentage = 1f - this.runningTime / this.time;
		}
		else
		{
			this.percentage = this.runningTime / this.time;
		}
		this.lastRealTime = Time.realtimeSinceStartup;
	}

	// Token: 0x0600354A RID: 13642 RVA: 0x0019F978 File Offset: 0x0019DB78
	private void CallBack(string callbackType)
	{
		if (this.tweenArguments.Contains(callbackType) && !this.tweenArguments.Contains("ischild"))
		{
			GameObject gameObject;
			if (this.tweenArguments.Contains(callbackType + "target"))
			{
				gameObject = (GameObject)this.tweenArguments[callbackType + "target"];
			}
			else
			{
				gameObject = base.gameObject;
			}
			if (this.tweenArguments[callbackType].GetType() == typeof(string))
			{
				gameObject.SendMessage((string)this.tweenArguments[callbackType], this.tweenArguments[callbackType + "params"], 1);
			}
			else
			{
				Debug.LogError("iTween Error: Callback method references must be passed as a String!");
				Object.Destroy(this);
			}
		}
	}

	// Token: 0x0600354B RID: 13643 RVA: 0x0019FA54 File Offset: 0x0019DC54
	private void Dispose()
	{
		for (int i = 0; i < iTween.tweens.Count; i++)
		{
			Hashtable hashtable = (Hashtable)iTween.tweens[i];
			if ((string)hashtable["id"] == this.id)
			{
				iTween.tweens.RemoveAt(i);
				break;
			}
		}
		Object.Destroy(this);
	}

	// Token: 0x0600354C RID: 13644 RVA: 0x0019FAC4 File Offset: 0x0019DCC4
	private void ConflictCheck()
	{
		Component[] components = base.GetComponents(typeof(iTween));
		foreach (iTween iTween in components)
		{
			if (iTween.type == "value")
			{
				return;
			}
			if (iTween.isRunning && iTween.type == this.type)
			{
				if (iTween.method != this.method)
				{
					return;
				}
				if (iTween.tweenArguments.Count != this.tweenArguments.Count)
				{
					iTween.Dispose();
					return;
				}
				foreach (object obj in this.tweenArguments)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					if (!iTween.tweenArguments.Contains(dictionaryEntry.Key))
					{
						iTween.Dispose();
						return;
					}
					if (!iTween.tweenArguments[dictionaryEntry.Key].Equals(this.tweenArguments[dictionaryEntry.Key]) && (string)dictionaryEntry.Key != "id")
					{
						iTween.Dispose();
						return;
					}
				}
				this.Dispose();
			}
		}
	}

	// Token: 0x0600354D RID: 13645 RVA: 0x0000264F File Offset: 0x0000084F
	private void EnableKinematic()
	{
	}

	// Token: 0x0600354E RID: 13646 RVA: 0x0000264F File Offset: 0x0000084F
	private void DisableKinematic()
	{
	}

	// Token: 0x0600354F RID: 13647 RVA: 0x000218A3 File Offset: 0x0001FAA3
	private void ResumeDelay()
	{
		base.StartCoroutine("TweenDelay");
	}

	// Token: 0x06003550 RID: 13648 RVA: 0x0000377C File Offset: 0x0000197C
	private float linear(float start, float end, float value)
	{
		return Mathf.Lerp(start, end, value);
	}

	// Token: 0x06003551 RID: 13649 RVA: 0x0019FC44 File Offset: 0x0019DE44
	private float clerp(float start, float end, float value)
	{
		float num = 0f;
		float num2 = 360f;
		float num3 = Mathf.Abs((num2 - num) / 2f);
		float result;
		if (end - start < -num3)
		{
			float num4 = (num2 - start + end) * value;
			result = start + num4;
		}
		else if (end - start > num3)
		{
			float num4 = -(num2 - end + start) * value;
			result = start + num4;
		}
		else
		{
			result = start + (end - start) * value;
		}
		return result;
	}

	// Token: 0x06003552 RID: 13650 RVA: 0x0019FCBC File Offset: 0x0019DEBC
	private float spring(float start, float end, float value)
	{
		value = Mathf.Clamp01(value);
		value = (Mathf.Sin(value * 3.1415927f * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) + value) * (1f + 1.2f * (1f - value));
		return start + (end - start) * value;
	}

	// Token: 0x06003553 RID: 13651 RVA: 0x000218B1 File Offset: 0x0001FAB1
	private float easeInQuad(float start, float end, float value)
	{
		end -= start;
		return end * value * value + start;
	}

	// Token: 0x06003554 RID: 13652 RVA: 0x000218BF File Offset: 0x0001FABF
	private float easeOutQuad(float start, float end, float value)
	{
		end -= start;
		return -end * value * (value - 2f) + start;
	}

	// Token: 0x06003555 RID: 13653 RVA: 0x0019FD20 File Offset: 0x0019DF20
	private float easeInOutQuad(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end / 2f * value * value + start;
		}
		value -= 1f;
		return -end / 2f * (value * (value - 2f) - 1f) + start;
	}

	// Token: 0x06003556 RID: 13654 RVA: 0x000218D4 File Offset: 0x0001FAD4
	private float easeInCubic(float start, float end, float value)
	{
		end -= start;
		return end * value * value * value + start;
	}

	// Token: 0x06003557 RID: 13655 RVA: 0x000218E4 File Offset: 0x0001FAE4
	private float easeOutCubic(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return end * (value * value * value + 1f) + start;
	}

	// Token: 0x06003558 RID: 13656 RVA: 0x0019FD78 File Offset: 0x0019DF78
	private float easeInOutCubic(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end / 2f * value * value * value + start;
		}
		value -= 2f;
		return end / 2f * (value * value * value + 2f) + start;
	}

	// Token: 0x06003559 RID: 13657 RVA: 0x00021903 File Offset: 0x0001FB03
	private float easeInQuart(float start, float end, float value)
	{
		end -= start;
		return end * value * value * value * value + start;
	}

	// Token: 0x0600355A RID: 13658 RVA: 0x00021915 File Offset: 0x0001FB15
	private float easeOutQuart(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return -end * (value * value * value * value - 1f) + start;
	}

	// Token: 0x0600355B RID: 13659 RVA: 0x0019FDCC File Offset: 0x0019DFCC
	private float easeInOutQuart(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end / 2f * value * value * value * value + start;
		}
		value -= 2f;
		return -end / 2f * (value * value * value * value - 2f) + start;
	}

	// Token: 0x0600355C RID: 13660 RVA: 0x00021937 File Offset: 0x0001FB37
	private float easeInQuint(float start, float end, float value)
	{
		end -= start;
		return end * value * value * value * value * value + start;
	}

	// Token: 0x0600355D RID: 13661 RVA: 0x0002194B File Offset: 0x0001FB4B
	private float easeOutQuint(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return end * (value * value * value * value * value + 1f) + start;
	}

	// Token: 0x0600355E RID: 13662 RVA: 0x0019FE28 File Offset: 0x0019E028
	private float easeInOutQuint(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end / 2f * value * value * value * value * value + start;
		}
		value -= 2f;
		return end / 2f * (value * value * value * value * value + 2f) + start;
	}

	// Token: 0x0600355F RID: 13663 RVA: 0x0002196E File Offset: 0x0001FB6E
	private float easeInSine(float start, float end, float value)
	{
		end -= start;
		return -end * Mathf.Cos(value / 1f * 1.5707964f) + end + start;
	}

	// Token: 0x06003560 RID: 13664 RVA: 0x0002198E File Offset: 0x0001FB8E
	private float easeOutSine(float start, float end, float value)
	{
		end -= start;
		return end * Mathf.Sin(value / 1f * 1.5707964f) + start;
	}

	// Token: 0x06003561 RID: 13665 RVA: 0x000219AB File Offset: 0x0001FBAB
	private float easeInOutSine(float start, float end, float value)
	{
		end -= start;
		return -end / 2f * (Mathf.Cos(3.1415927f * value / 1f) - 1f) + start;
	}

	// Token: 0x06003562 RID: 13666 RVA: 0x000219D5 File Offset: 0x0001FBD5
	private float easeInExpo(float start, float end, float value)
	{
		end -= start;
		return end * Mathf.Pow(2f, 10f * (value / 1f - 1f)) + start;
	}

	// Token: 0x06003563 RID: 13667 RVA: 0x000219FD File Offset: 0x0001FBFD
	private float easeOutExpo(float start, float end, float value)
	{
		end -= start;
		return end * (-Mathf.Pow(2f, -10f * value / 1f) + 1f) + start;
	}

	// Token: 0x06003564 RID: 13668 RVA: 0x0019FE84 File Offset: 0x0019E084
	private float easeInOutExpo(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end / 2f * Mathf.Pow(2f, 10f * (value - 1f)) + start;
		}
		value -= 1f;
		return end / 2f * (-Mathf.Pow(2f, -10f * value) + 2f) + start;
	}

	// Token: 0x06003565 RID: 13669 RVA: 0x00021A26 File Offset: 0x0001FC26
	private float easeInCirc(float start, float end, float value)
	{
		end -= start;
		return -end * (Mathf.Sqrt(1f - value * value) - 1f) + start;
	}

	// Token: 0x06003566 RID: 13670 RVA: 0x00021A46 File Offset: 0x0001FC46
	private float easeOutCirc(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return end * Mathf.Sqrt(1f - value * value) + start;
	}

	// Token: 0x06003567 RID: 13671 RVA: 0x0019FEF8 File Offset: 0x0019E0F8
	private float easeInOutCirc(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return -end / 2f * (Mathf.Sqrt(1f - value * value) - 1f) + start;
		}
		value -= 2f;
		return end / 2f * (Mathf.Sqrt(1f - value * value) + 1f) + start;
	}

	// Token: 0x06003568 RID: 13672 RVA: 0x0019FF68 File Offset: 0x0019E168
	private float easeInBounce(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		return end - this.easeOutBounce(0f, end, num - value) + start;
	}

	// Token: 0x06003569 RID: 13673 RVA: 0x0019FF94 File Offset: 0x0019E194
	private float easeOutBounce(float start, float end, float value)
	{
		value /= 1f;
		end -= start;
		if (value < 0.36363637f)
		{
			return end * (7.5625f * value * value) + start;
		}
		if (value < 0.72727275f)
		{
			value -= 0.54545456f;
			return end * (7.5625f * value * value + 0.75f) + start;
		}
		if ((double)value < 0.9090909090909091)
		{
			value -= 0.8181818f;
			return end * (7.5625f * value * value + 0.9375f) + start;
		}
		value -= 0.95454544f;
		return end * (7.5625f * value * value + 0.984375f) + start;
	}

	// Token: 0x0600356A RID: 13674 RVA: 0x001A003C File Offset: 0x0019E23C
	private float easeInOutBounce(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		if (value < num / 2f)
		{
			return this.easeInBounce(0f, end, value * 2f) * 0.5f + start;
		}
		return this.easeOutBounce(0f, end, value * 2f - num) * 0.5f + end * 0.5f + start;
	}

	// Token: 0x0600356B RID: 13675 RVA: 0x001A00A4 File Offset: 0x0019E2A4
	private float easeInBack(float start, float end, float value)
	{
		end -= start;
		value /= 1f;
		float num = 1.70158f;
		return end * value * value * ((num + 1f) * value - num) + start;
	}

	// Token: 0x0600356C RID: 13676 RVA: 0x001A00D8 File Offset: 0x0019E2D8
	private float easeOutBack(float start, float end, float value)
	{
		float num = 1.70158f;
		end -= start;
		value = value / 1f - 1f;
		return end * (value * value * ((num + 1f) * value + num) + 1f) + start;
	}

	// Token: 0x0600356D RID: 13677 RVA: 0x001A0118 File Offset: 0x0019E318
	private float easeInOutBack(float start, float end, float value)
	{
		float num = 1.70158f;
		end -= start;
		value /= 0.5f;
		if (value < 1f)
		{
			num *= 1.525f;
			return end / 2f * (value * value * ((num + 1f) * value - num)) + start;
		}
		value -= 2f;
		num *= 1.525f;
		return end / 2f * (value * value * ((num + 1f) * value + num) + 2f) + start;
	}

	// Token: 0x0600356E RID: 13678 RVA: 0x001A0198 File Offset: 0x0019E398
	private float punch(float amplitude, float value)
	{
		if (value == 0f)
		{
			return 0f;
		}
		if (value == 1f)
		{
			return 0f;
		}
		float num = 0.3f;
		float num2 = num / 6.2831855f * Mathf.Asin(0f);
		return amplitude * Mathf.Pow(2f, -10f * value) * Mathf.Sin((value * 1f - num2) * 6.2831855f / num);
	}

	// Token: 0x0600356F RID: 13679 RVA: 0x001A0210 File Offset: 0x0019E410
	private float easeInElastic(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		float num2 = num * 0.3f;
		float num3 = 0f;
		if (value == 0f)
		{
			return start;
		}
		if ((value /= num) == 1f)
		{
			return start + end;
		}
		float num4;
		if (num3 == 0f || num3 < Mathf.Abs(end))
		{
			num3 = end;
			num4 = num2 / 4f;
		}
		else
		{
			num4 = num2 / 6.2831855f * Mathf.Asin(end / num3);
		}
		return -(num3 * Mathf.Pow(2f, 10f * (value -= 1f)) * Mathf.Sin((value * num - num4) * 6.2831855f / num2)) + start;
	}

	// Token: 0x06003570 RID: 13680 RVA: 0x001A02C8 File Offset: 0x0019E4C8
	private float easeOutElastic(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		float num2 = num * 0.3f;
		float num3 = 0f;
		if (value == 0f)
		{
			return start;
		}
		if ((value /= num) == 1f)
		{
			return start + end;
		}
		float num4;
		if (num3 == 0f || num3 < Mathf.Abs(end))
		{
			num3 = end;
			num4 = num2 / 4f;
		}
		else
		{
			num4 = num2 / 6.2831855f * Mathf.Asin(end / num3);
		}
		return num3 * Mathf.Pow(2f, -10f * value) * Mathf.Sin((value * num - num4) * 6.2831855f / num2) + end + start;
	}

	// Token: 0x06003571 RID: 13681 RVA: 0x001A0378 File Offset: 0x0019E578
	private float easeInOutElastic(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		float num2 = num * 0.3f;
		float num3 = 0f;
		if (value == 0f)
		{
			return start;
		}
		if ((value /= num / 2f) == 2f)
		{
			return start + end;
		}
		float num4;
		if (num3 == 0f || num3 < Mathf.Abs(end))
		{
			num3 = end;
			num4 = num2 / 4f;
		}
		else
		{
			num4 = num2 / 6.2831855f * Mathf.Asin(end / num3);
		}
		if (value < 1f)
		{
			return -0.5f * (num3 * Mathf.Pow(2f, 10f * (value -= 1f)) * Mathf.Sin((value * num - num4) * 6.2831855f / num2)) + start;
		}
		return num3 * Mathf.Pow(2f, -10f * (value -= 1f)) * Mathf.Sin((value * num - num4) * 6.2831855f / num2) * 0.5f + end + start;
	}

	// Token: 0x0400405C RID: 16476
	public static ArrayList tweens = new ArrayList();

	// Token: 0x0400405D RID: 16477
	private static GameObject cameraFade;

	// Token: 0x0400405E RID: 16478
	public string id;

	// Token: 0x0400405F RID: 16479
	public string type;

	// Token: 0x04004060 RID: 16480
	public string method;

	// Token: 0x04004061 RID: 16481
	public iTween.EaseType easeType;

	// Token: 0x04004062 RID: 16482
	public float time;

	// Token: 0x04004063 RID: 16483
	public float delay;

	// Token: 0x04004064 RID: 16484
	public iTween.LoopType loopType;

	// Token: 0x04004065 RID: 16485
	public bool isRunning;

	// Token: 0x04004066 RID: 16486
	public bool isPaused;

	// Token: 0x04004067 RID: 16487
	public string _name;

	// Token: 0x04004068 RID: 16488
	private float runningTime;

	// Token: 0x04004069 RID: 16489
	private float percentage;

	// Token: 0x0400406A RID: 16490
	private float delayStarted;

	// Token: 0x0400406B RID: 16491
	private bool kinematic;

	// Token: 0x0400406C RID: 16492
	private bool isLocal;

	// Token: 0x0400406D RID: 16493
	private bool loop;

	// Token: 0x0400406E RID: 16494
	private bool reverse;

	// Token: 0x0400406F RID: 16495
	private bool wasPaused;

	// Token: 0x04004070 RID: 16496
	private bool physics;

	// Token: 0x04004071 RID: 16497
	private Hashtable tweenArguments;

	// Token: 0x04004072 RID: 16498
	private Space space;

	// Token: 0x04004073 RID: 16499
	private iTween.EasingFunction ease;

	// Token: 0x04004074 RID: 16500
	private iTween.ApplyTween apply;

	// Token: 0x04004075 RID: 16501
	private AudioSource audioSource;

	// Token: 0x04004076 RID: 16502
	private Vector3[] vector3s;

	// Token: 0x04004077 RID: 16503
	private Vector2[] vector2s;

	// Token: 0x04004078 RID: 16504
	private Color[,] colors;

	// Token: 0x04004079 RID: 16505
	private float[] floats;

	// Token: 0x0400407A RID: 16506
	private Rect[] rects;

	// Token: 0x0400407B RID: 16507
	private iTween.CRSpline path;

	// Token: 0x0400407C RID: 16508
	private Vector3 preUpdate;

	// Token: 0x0400407D RID: 16509
	private Vector3 postUpdate;

	// Token: 0x0400407E RID: 16510
	private iTween.NamedValueColor namedcolorvalue;

	// Token: 0x0400407F RID: 16511
	private float lastRealTime;

	// Token: 0x04004080 RID: 16512
	private bool useRealTime;

	// Token: 0x02000884 RID: 2180
	public enum EaseType
	{
		// Token: 0x0400408F RID: 16527
		easeInQuad,
		// Token: 0x04004090 RID: 16528
		easeOutQuad,
		// Token: 0x04004091 RID: 16529
		easeInOutQuad,
		// Token: 0x04004092 RID: 16530
		easeInCubic,
		// Token: 0x04004093 RID: 16531
		easeOutCubic,
		// Token: 0x04004094 RID: 16532
		easeInOutCubic,
		// Token: 0x04004095 RID: 16533
		easeInQuart,
		// Token: 0x04004096 RID: 16534
		easeOutQuart,
		// Token: 0x04004097 RID: 16535
		easeInOutQuart,
		// Token: 0x04004098 RID: 16536
		easeInQuint,
		// Token: 0x04004099 RID: 16537
		easeOutQuint,
		// Token: 0x0400409A RID: 16538
		easeInOutQuint,
		// Token: 0x0400409B RID: 16539
		easeInSine,
		// Token: 0x0400409C RID: 16540
		easeOutSine,
		// Token: 0x0400409D RID: 16541
		easeInOutSine,
		// Token: 0x0400409E RID: 16542
		easeInExpo,
		// Token: 0x0400409F RID: 16543
		easeOutExpo,
		// Token: 0x040040A0 RID: 16544
		easeInOutExpo,
		// Token: 0x040040A1 RID: 16545
		easeInCirc,
		// Token: 0x040040A2 RID: 16546
		easeOutCirc,
		// Token: 0x040040A3 RID: 16547
		easeInOutCirc,
		// Token: 0x040040A4 RID: 16548
		linear,
		// Token: 0x040040A5 RID: 16549
		spring,
		// Token: 0x040040A6 RID: 16550
		easeInBounce,
		// Token: 0x040040A7 RID: 16551
		easeOutBounce,
		// Token: 0x040040A8 RID: 16552
		easeInOutBounce,
		// Token: 0x040040A9 RID: 16553
		easeInBack,
		// Token: 0x040040AA RID: 16554
		easeOutBack,
		// Token: 0x040040AB RID: 16555
		easeInOutBack,
		// Token: 0x040040AC RID: 16556
		easeInElastic,
		// Token: 0x040040AD RID: 16557
		easeOutElastic,
		// Token: 0x040040AE RID: 16558
		easeInOutElastic,
		// Token: 0x040040AF RID: 16559
		punch
	}

	// Token: 0x02000885 RID: 2181
	public enum LoopType
	{
		// Token: 0x040040B1 RID: 16561
		none,
		// Token: 0x040040B2 RID: 16562
		loop,
		// Token: 0x040040B3 RID: 16563
		pingPong
	}

	// Token: 0x02000886 RID: 2182
	public enum NamedValueColor
	{
		// Token: 0x040040B5 RID: 16565
		_Color,
		// Token: 0x040040B6 RID: 16566
		_SpecColor,
		// Token: 0x040040B7 RID: 16567
		_Emission,
		// Token: 0x040040B8 RID: 16568
		_ReflectColor
	}

	// Token: 0x02000887 RID: 2183
	public static class Defaults
	{
		// Token: 0x040040B9 RID: 16569
		public static float time = 1f;

		// Token: 0x040040BA RID: 16570
		public static float delay = 0f;

		// Token: 0x040040BB RID: 16571
		public static iTween.NamedValueColor namedColorValue = iTween.NamedValueColor._Color;

		// Token: 0x040040BC RID: 16572
		public static iTween.LoopType loopType = iTween.LoopType.none;

		// Token: 0x040040BD RID: 16573
		public static iTween.EaseType easeType = iTween.EaseType.easeOutExpo;

		// Token: 0x040040BE RID: 16574
		public static float lookSpeed = 3f;

		// Token: 0x040040BF RID: 16575
		public static bool isLocal = false;

		// Token: 0x040040C0 RID: 16576
		public static Space space = 1;

		// Token: 0x040040C1 RID: 16577
		public static bool orientToPath = false;

		// Token: 0x040040C2 RID: 16578
		public static Color color = Color.white;

		// Token: 0x040040C3 RID: 16579
		public static float updateTimePercentage = 0.05f;

		// Token: 0x040040C4 RID: 16580
		public static float updateTime = 1f * iTween.Defaults.updateTimePercentage;

		// Token: 0x040040C5 RID: 16581
		public static int cameraFadeDepth = 999999;

		// Token: 0x040040C6 RID: 16582
		public static float lookAhead = 0.05f;

		// Token: 0x040040C7 RID: 16583
		public static bool useRealTime = false;

		// Token: 0x040040C8 RID: 16584
		public static Vector3 up = Vector3.up;
	}

	// Token: 0x02000888 RID: 2184
	private class CRSpline
	{
		// Token: 0x06003573 RID: 13683 RVA: 0x00021A68 File Offset: 0x0001FC68
		public CRSpline(params Vector3[] pts)
		{
			this.pts = new Vector3[pts.Length];
			Array.Copy(pts, this.pts, pts.Length);
		}

		// Token: 0x06003574 RID: 13684 RVA: 0x001A0518 File Offset: 0x0019E718
		public Vector3 Interp(float t)
		{
			int num = this.pts.Length - 3;
			int num2 = Mathf.Min(Mathf.FloorToInt(t * (float)num), num - 1);
			float num3 = t * (float)num - (float)num2;
			Vector3 vector = this.pts[num2];
			Vector3 vector2 = this.pts[num2 + 1];
			Vector3 vector3 = this.pts[num2 + 2];
			Vector3 vector4 = this.pts[num2 + 3];
			return 0.5f * ((-vector + 3f * vector2 - 3f * vector3 + vector4) * (num3 * num3 * num3) + (2f * vector - 5f * vector2 + 4f * vector3 - vector4) * (num3 * num3) + (-vector + vector3) * num3 + 2f * vector2);
		}

		// Token: 0x040040C9 RID: 16585
		public Vector3[] pts;
	}

	// Token: 0x02000889 RID: 2185
	// (Invoke) Token: 0x06003576 RID: 13686
	private delegate float EasingFunction(float start, float end, float value);

	// Token: 0x0200088A RID: 2186
	// (Invoke) Token: 0x0600357A RID: 13690
	private delegate void ApplyTween();
}
