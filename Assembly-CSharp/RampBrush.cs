using System;
using UnityEngine;

// Token: 0x0200062A RID: 1578
[AddComponentMenu("Terrain/Terrain Ramp Brush")]
[ExecuteInEditMode]
public class RampBrush : MonoBehaviour
{
	// Token: 0x06002710 RID: 10000 RVA: 0x0012E168 File Offset: 0x0012C368
	public void OnDrawGizmos()
	{
		if (this.turnBrushOnVar)
		{
			Terrain terrain = (Terrain)base.GetComponent(typeof(Terrain));
			if (terrain == null)
			{
				return;
			}
			if (this.isBrushPainting)
			{
				Gizmos.color = Color.red;
			}
			else
			{
				Gizmos.color = Color.white;
			}
			float num = this.brushSize / 4f;
			Gizmos.DrawLine(this.brushPosition + new Vector3(-num, 0f, 0f), this.brushPosition + new Vector3(num, 0f, 0f));
			Gizmos.DrawLine(this.brushPosition + new Vector3(0f, -num, 0f), this.brushPosition + new Vector3(0f, num, 0f));
			Gizmos.DrawLine(this.brushPosition + new Vector3(0f, 0f, -num), this.brushPosition + new Vector3(0f, 0f, num));
			Gizmos.DrawWireCube(this.brushPosition, new Vector3(this.brushSize, 0f, this.brushSize));
			Gizmos.DrawWireSphere(this.brushPosition, this.brushSize / 2f);
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere(this.beginRamp, this.brushSize / 2f);
			Gizmos.DrawLine(this.endRamp + Vector3.up * 10f, this.endRamp + this.backupVector + Vector3.up * 10f);
		}
	}

	// Token: 0x06002711 RID: 10001 RVA: 0x0012E328 File Offset: 0x0012C528
	public int[] terrainCordsToBitmap(TerrainData terData, Vector3 v)
	{
		float num = (float)terData.heightmapWidth;
		float num2 = (float)terData.heightmapHeight;
		Vector3 size = terData.size;
		int num3 = (int)Mathf.Floor(num / size.x * v.x);
		int num4 = (int)Mathf.Floor(num2 / size.z * v.z);
		return new int[]
		{
			num4,
			num3
		};
	}

	// Token: 0x06002712 RID: 10002 RVA: 0x0012E394 File Offset: 0x0012C594
	public float[] bitmapCordsToTerrain(TerrainData terData, int x, int y)
	{
		int heightmapWidth = terData.heightmapWidth;
		int heightmapHeight = terData.heightmapHeight;
		Vector3 size = terData.size;
		float num = (float)x * (size.z / (float)heightmapHeight);
		float num2 = (float)y * (size.x / (float)heightmapWidth);
		return new float[]
		{
			num2,
			num
		};
	}

	// Token: 0x06002713 RID: 10003 RVA: 0x00019CE0 File Offset: 0x00017EE0
	public void toggleBrushOn()
	{
		if (this.turnBrushOnVar)
		{
			this.turnBrushOnVar = false;
		}
		else
		{
			this.turnBrushOnVar = true;
		}
	}

	// Token: 0x06002714 RID: 10004 RVA: 0x0012E3E8 File Offset: 0x0012C5E8
	public void rampBrush()
	{
		Terrain terrain = (Terrain)base.GetComponent(typeof(Terrain));
		if (terrain == null)
		{
			Debug.LogError("No terrain component on this GameObject");
			return;
		}
		try
		{
			TerrainData terrainData = terrain.terrainData;
			int heightmapWidth = terrainData.heightmapWidth;
			int heightmapHeight = terrainData.heightmapHeight;
			Vector3 size = terrainData.size;
			if (this.VERBOSE)
			{
				Debug.Log(string.Concat(new object[]
				{
					"terrainData heightmapHeight/heightmapWidth:",
					heightmapWidth,
					" ",
					heightmapWidth
				}));
			}
			if (this.VERBOSE)
			{
				Debug.Log("terrainData heightMapResolution:" + terrainData.heightmapResolution);
			}
			if (this.VERBOSE)
			{
				Debug.Log("terrainData size:" + terrainData.size);
			}
			Vector3 localScale = base.transform.localScale;
			base.transform.localScale = new Vector3(1f, 1f, 1f);
			Vector3 vector = base.transform.InverseTransformPoint(this.beginRamp);
			Vector3 vector2 = base.transform.InverseTransformPoint(this.endRamp);
			base.transform.localScale = localScale;
			int num = (int)Mathf.Floor((float)heightmapWidth / size.z * this.brushSize);
			int num2 = (int)Mathf.Floor((float)heightmapHeight / size.x * this.brushSize);
			int[] array = this.terrainCordsToBitmap(terrainData, vector);
			int[] array2 = this.terrainCordsToBitmap(terrainData, vector2);
			if (array[0] < 0 || array2[0] < 0 || array[1] < 0 || array2[1] < 0 || array[0] >= heightmapWidth || array2[0] >= heightmapWidth || array[1] >= heightmapHeight || array2[1] >= heightmapHeight)
			{
				Debug.LogError("The start point or the end point was out of bounds. Make sure the gizmo is over the terrain before setting the start and end points.Note: that sometimes Unity does not update the collider after changing settings in the 'Set Resolution' dialog. Entering play mode should reset the collider.");
			}
			else
			{
				if (this.VERBOSE)
				{
					float[] array3 = this.bitmapCordsToTerrain(terrainData, array[0], array[1]);
					Debug.Log("Local Begin Pos:" + vector);
					Debug.Log(string.Concat(new object[]
					{
						"pixel begin coord:",
						array[0],
						" ",
						array[0]
					}));
					Debug.Log(string.Concat(new object[]
					{
						"Local begin Pos Rev Transformed:",
						array3[0],
						" ",
						array3[1]
					}));
					array3 = this.bitmapCordsToTerrain(terrainData, array2[0], array2[1]);
					Debug.Log("Local End Pos:" + vector2);
					Debug.Log(string.Concat(new object[]
					{
						"pixel End coord:",
						array2[0],
						" ",
						array2[1]
					}));
					Debug.Log(string.Concat(new object[]
					{
						"Local End Pos Rev Transformed:",
						array3[0],
						" ",
						array3[1]
					}));
					Debug.Log(string.Concat(new object[]
					{
						"Sample Width/height: ",
						num,
						" ",
						num2
					}));
				}
				double num3 = Math.Sqrt((double)((array2[0] - array[0]) * (array2[0] - array[0]) + (array2[1] - array[1]) * (array2[1] - array[1])));
				float[,] heights = terrainData.GetHeights(0, 0, heightmapWidth, heightmapHeight);
				vector2.y = heights[array2[0], array2[1]];
				vector.y = heights[array[0], array[1]];
				Vector3 vector3 = vector2 - vector;
				Vector3 vector4;
				vector4..ctor(-vector3.z, 0f, vector3.x);
				Vector3 vector5 = Vector3.Cross(vector3, vector4);
				vector5.Normalize();
				Vector3 vector6;
				vector6..ctor(vector3.x, 0f, vector3.z);
				float num4;
				if (this.brushSize < 15f)
				{
					num4 = this.brushSize / 6f / vector3.magnitude;
				}
				else
				{
					num4 = (float)(1.0 / num3 * (double)this.brushSampleDensity);
				}
				for (float num5 = 0f; num5 <= 1f; num5 += num4)
				{
					Vector3 v = vector + num5 * vector3;
					int[] array4 = this.terrainCordsToBitmap(terrainData, v);
					int num6 = array4[0] - num / 2;
					int num7 = array4[1] - num2 / 2;
					float[,] array5 = new float[num, num2];
					for (int i = 0; i < num; i++)
					{
						for (int j = 0; j < num2; j++)
						{
							if (num6 + i >= 0 && num7 + j >= 0 && num6 + i < heightmapWidth && num7 + j < heightmapHeight)
							{
								array5[i, j] = heights[num6 + i, num7 + j];
							}
							else
							{
								array5[i, j] = 0f;
							}
						}
					}
					num = array5.GetLength(0);
					num2 = array5.GetLength(1);
					float[,] array6 = (float[,])array5.Clone();
					for (int k = 0; k < num; k++)
					{
						for (int l = 0; l < num2; l++)
						{
							float[] array7 = this.bitmapCordsToTerrain(terrainData, num6 + k, num7 + l);
							bool flag = false;
							if (vector6.x * (array7[0] - vector.x) + vector6.z * (array7[1] - vector.z) < 0f)
							{
								flag = true;
							}
							else if (-vector6.x * (array7[0] - vector2.x) - vector6.z * (array7[1] - vector2.z) < 0f)
							{
								flag = true;
							}
							if (!flag)
							{
								array6[k, l] = vector.y - (vector5.x * (array7[0] - vector.x) + vector5.z * (array7[1] - vector.z)) / vector5.y;
							}
						}
					}
					float num8 = (float)num / 2f;
					for (int m = 0; m < num; m++)
					{
						for (int n = 0; n < num2; n++)
						{
							float num9 = array6[m, n];
							float num10 = array5[m, n];
							float num11 = Vector2.Distance(new Vector2((float)m, (float)n), new Vector2(num8, num8));
							float num12 = 1f - (num11 - (num8 - num8 * this.brushSoftness)) / (num8 * this.brushSoftness);
							if (num12 < 0f)
							{
								num12 = 0f;
							}
							else if (num12 > 1f)
							{
								num12 = 1f;
							}
							num12 *= this.brushOpacity;
							float num13 = num9 * num12 + num10 * (1f - num12);
							array5[m, n] = num13;
						}
					}
					for (int num14 = 0; num14 < num; num14++)
					{
						for (int num15 = 0; num15 < num2; num15++)
						{
							if (num6 + num14 >= 0 && num7 + num15 >= 0 && num6 + num14 < heightmapWidth && num7 + num15 < heightmapHeight)
							{
								heights[num6 + num14, num7 + num15] = array5[num14, num15];
							}
						}
					}
				}
				terrainData.SetHeights(0, 0, heights);
			}
		}
		catch (Exception ex)
		{
			Debug.LogError("A brush error occurred: " + ex);
		}
	}

	// Token: 0x04003038 RID: 12344
	private bool VERBOSE;

	// Token: 0x04003039 RID: 12345
	public bool brushOn;

	// Token: 0x0400303A RID: 12346
	public bool turnBrushOnVar;

	// Token: 0x0400303B RID: 12347
	public bool isBrushHidden;

	// Token: 0x0400303C RID: 12348
	public bool isBrushPainting;

	// Token: 0x0400303D RID: 12349
	public Vector3 brushPosition;

	// Token: 0x0400303E RID: 12350
	public Vector3 beginRamp;

	// Token: 0x0400303F RID: 12351
	public Vector3 endRamp;

	// Token: 0x04003040 RID: 12352
	public bool setBeginRamp;

	// Token: 0x04003041 RID: 12353
	public float brushSize = 50f;

	// Token: 0x04003042 RID: 12354
	public float brushOpacity = 1f;

	// Token: 0x04003043 RID: 12355
	public float brushSoftness = 0.5f;

	// Token: 0x04003044 RID: 12356
	public float brushSampleDensity = 4f;

	// Token: 0x04003045 RID: 12357
	public bool shiftProcessed = true;

	// Token: 0x04003046 RID: 12358
	public Vector3 backupVector;
}
