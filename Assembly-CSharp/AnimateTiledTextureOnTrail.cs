using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200064B RID: 1611
public class AnimateTiledTextureOnTrail : MonoBehaviour
{
	// Token: 0x060027BB RID: 10171 RVA: 0x0001A337 File Offset: 0x00018537
	public void RegisterCallback(AnimateTiledTextureOnTrail.VoidEvent cbFunction)
	{
		if (this._enableEvents)
		{
			this._voidEventCallbackList.Add(cbFunction);
		}
		else
		{
			Debug.LogWarning("AnimateTiledTextureOnTrail: You are attempting to register a callback but the events of this object are not enabled!");
		}
	}

	// Token: 0x060027BC RID: 10172 RVA: 0x0001A35F File Offset: 0x0001855F
	public void UnRegisterCallback(AnimateTiledTextureOnTrail.VoidEvent cbFunction)
	{
		if (this._enableEvents)
		{
			this._voidEventCallbackList.Remove(cbFunction);
		}
		else
		{
			Debug.LogWarning("AnimateTiledTextureOnTrail: You are attempting to un-register a callback but the events of this object are not enabled!");
		}
	}

	// Token: 0x060027BD RID: 10173 RVA: 0x0013B314 File Offset: 0x00139514
	public void Play()
	{
		if (this._isPlaying)
		{
			base.StopCoroutine("updateTiling");
			this._isPlaying = false;
		}
		base.GetComponent<TrailRenderer>().enabled = true;
		this._index = this._columns;
		base.StartCoroutine(this.updateTiling());
	}

	// Token: 0x060027BE RID: 10174 RVA: 0x0013B364 File Offset: 0x00139564
	public void ChangeMaterial(Material newMaterial, bool newInstance = false)
	{
		if (newInstance)
		{
			if (this._hasMaterialInstance)
			{
				Object.Destroy(base.GetComponent<TrailRenderer>().sharedMaterial);
			}
			this._materialInstance = new Material(newMaterial);
			base.GetComponent<TrailRenderer>().sharedMaterial = this._materialInstance;
			this._hasMaterialInstance = true;
		}
		else
		{
			base.GetComponent<TrailRenderer>().sharedMaterial = newMaterial;
		}
		this.CalcTextureSize();
		base.GetComponent<TrailRenderer>().sharedMaterial.SetTextureScale("_MainTex", this._textureSize);
	}

	// Token: 0x060027BF RID: 10175 RVA: 0x0001A388 File Offset: 0x00018588
	private void Awake()
	{
		if (this._enableEvents)
		{
			this._voidEventCallbackList = new List<AnimateTiledTextureOnTrail.VoidEvent>();
		}
		this.ChangeMaterial(base.GetComponent<TrailRenderer>().sharedMaterial, this._newMaterialInstance);
	}

	// Token: 0x060027C0 RID: 10176 RVA: 0x0001A3B7 File Offset: 0x000185B7
	private void OnDestroy()
	{
		if (this._hasMaterialInstance)
		{
			Object.Destroy(base.GetComponent<TrailRenderer>().sharedMaterial);
			this._hasMaterialInstance = false;
		}
	}

	// Token: 0x060027C1 RID: 10177 RVA: 0x0013B3E8 File Offset: 0x001395E8
	private void HandleCallbacks(List<AnimateTiledTextureOnTrail.VoidEvent> cbList)
	{
		for (int i = 0; i < cbList.Count; i++)
		{
			cbList[i]();
		}
	}

	// Token: 0x060027C2 RID: 10178 RVA: 0x0001A3DB File Offset: 0x000185DB
	private void OnEnable()
	{
		this.CalcTextureSize();
		if (this._playOnEnable)
		{
			this.Play();
		}
	}

	// Token: 0x060027C3 RID: 10179 RVA: 0x0013B418 File Offset: 0x00139618
	private void CalcTextureSize()
	{
		this._textureSize = new Vector2(1f / (float)this._columns, 1f / (float)this._rows);
		this._textureSize.x = this._textureSize.x / this._scale.x;
		this._textureSize.y = this._textureSize.y / this._scale.y;
		this._textureSize -= this._buffer;
	}

	// Token: 0x060027C4 RID: 10180 RVA: 0x0013B4A8 File Offset: 0x001396A8
	private IEnumerator updateTiling()
	{
		this._isPlaying = true;
		int checkAgainst = this._rows * this._columns;
		for (;;)
		{
			if (this._index >= checkAgainst)
			{
				this._index = 0;
				if (this._playOnce)
				{
					if (checkAgainst == this._columns)
					{
						break;
					}
					checkAgainst = this._columns;
				}
			}
			this.ApplyOffset();
			this._index++;
			yield return new WaitForSeconds(1f / this._framesPerSecond);
		}
		if (this._enableEvents)
		{
			this.HandleCallbacks(this._voidEventCallbackList);
		}
		if (this._disableUponCompletion)
		{
			base.gameObject.GetComponent<TrailRenderer>().enabled = false;
		}
		this._isPlaying = false;
		yield break;
		yield break;
	}

	// Token: 0x060027C5 RID: 10181 RVA: 0x0013B4C4 File Offset: 0x001396C4
	private void ApplyOffset()
	{
		Vector2 vector;
		vector..ctor((float)this._index / (float)this._columns - (float)(this._index / this._columns), 1f - (float)(this._index / this._columns) / (float)this._rows);
		if (vector.y == 1f)
		{
			vector.y = 0f;
		}
		vector.x += (1f / (float)this._columns - this._textureSize.x) / 2f;
		vector.y += (1f / (float)this._rows - this._textureSize.y) / 2f;
		vector.x += this._offset.x;
		vector.y += this._offset.y;
		base.GetComponent<TrailRenderer>().sharedMaterial.SetTextureOffset("_MainTex", vector);
	}

	// Token: 0x040031B7 RID: 12727
	public int _columns = 2;

	// Token: 0x040031B8 RID: 12728
	public int _rows = 2;

	// Token: 0x040031B9 RID: 12729
	public Vector2 _scale = new Vector3(1f, 1f);

	// Token: 0x040031BA RID: 12730
	public Vector2 _offset = Vector2.zero;

	// Token: 0x040031BB RID: 12731
	public Vector2 _buffer = Vector2.zero;

	// Token: 0x040031BC RID: 12732
	public float _framesPerSecond = 10f;

	// Token: 0x040031BD RID: 12733
	public bool _playOnce;

	// Token: 0x040031BE RID: 12734
	public bool _disableUponCompletion;

	// Token: 0x040031BF RID: 12735
	public bool _enableEvents;

	// Token: 0x040031C0 RID: 12736
	public bool _playOnEnable = true;

	// Token: 0x040031C1 RID: 12737
	public bool _newMaterialInstance;

	// Token: 0x040031C2 RID: 12738
	private int _index;

	// Token: 0x040031C3 RID: 12739
	private Vector2 _textureSize = Vector2.zero;

	// Token: 0x040031C4 RID: 12740
	private Material _materialInstance;

	// Token: 0x040031C5 RID: 12741
	private bool _hasMaterialInstance;

	// Token: 0x040031C6 RID: 12742
	private bool _isPlaying;

	// Token: 0x040031C7 RID: 12743
	private List<AnimateTiledTextureOnTrail.VoidEvent> _voidEventCallbackList;

	// Token: 0x0200064C RID: 1612
	// (Invoke) Token: 0x060027C7 RID: 10183
	public delegate void VoidEvent();
}
