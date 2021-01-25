using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200067C RID: 1660
[Serializable]
public class DeltaCompressedFrameData
{
	// Token: 0x0600288F RID: 10383 RVA: 0x0014103C File Offset: 0x0013F23C
	public static implicit operator MeshFrameData[](DeltaCompressedFrameData s)
	{
		MeshFrameData[] array = new MeshFrameData[s.frameIndexes.Length / s.vertLength];
		bool flag = s.positionsUShort != null && s.positionsUShort.Length > 0;
		s.positions = new uint[s.compatibilityPositions.Length];
		for (int i = 0; i < s.compatibilityPositions.Length; i++)
		{
			s.positions[i] = (uint)s.compatibilityPositions[i];
		}
		bool flag2 = false;
		for (int j = 0; j < array.Length; j++)
		{
			Vector3[] array2 = new Vector3[s.vertLength];
			for (int k = 0; k < array2.Length; k++)
			{
				Vector3 zero = Vector3.zero;
				int num = s.frameIndexes[j * s.vertLength + k] * 3;
				int num2 = num;
				int num3 = num + 1;
				int num4 = num + 2;
				if (flag2)
				{
					zero.x = (float)s.positionsUShort[num2] / s.accuracy;
					zero.y = (float)s.positionsUShort[num3] / s.accuracy;
					zero.z = (float)s.positionsUShort[num4] / s.accuracy;
				}
				else
				{
					zero.x = s.positions[num2] / s.accuracy;
					zero.y = s.positions[num3] / s.accuracy;
					zero.z = s.positions[num4] / s.accuracy;
				}
				zero.x -= (float)s.sizeOffset;
				zero.y -= (float)s.sizeOffset;
				zero.z -= (float)s.sizeOffset;
				array2[k] = zero;
			}
			array[j] = new MeshFrameData();
			array[j].SetVerts(array2);
		}
		return array;
	}

	// Token: 0x06002890 RID: 10384 RVA: 0x00141220 File Offset: 0x0013F420
	public static implicit operator DeltaCompressedFrameData(MeshFrameData[] frames)
	{
		if (frames.Length == 0)
		{
			return new DeltaCompressedFrameData();
		}
		DeltaCompressedFrameData deltaCompressedFrameData = new DeltaCompressedFrameData
		{
			vertLength = frames[0].verts.Length,
			frameIndexes = new int[frames[0].verts.Length * frames.Length],
			accuracy = DeltaCompressedFrameData.compressionAccuracy
		};
		List<Vector3> list = new List<Vector3>();
		Dictionary<Vector3, int> dictionary = new Dictionary<Vector3, int>();
		int num = 1;
		for (int i = 0; i < frames.Length; i++)
		{
			for (int j = 0; j < frames[i].verts.Length; j++)
			{
				if (!dictionary.ContainsKey(frames[i].verts[j]))
				{
					dictionary.Add(frames[i].verts[j], list.Count);
					list.Add(frames[i].verts[j]);
					while (frames[i].verts[j].x > (float)num)
					{
						num *= 10;
					}
					while (frames[i].verts[j].y > (float)num)
					{
						num *= 10;
					}
					while (frames[i].verts[j].z > (float)num)
					{
						num *= 10;
					}
				}
				deltaCompressedFrameData.frameIndexes[i * deltaCompressedFrameData.vertLength + j] = dictionary[frames[i].verts[j]];
			}
		}
		deltaCompressedFrameData.sizeOffset = num;
		bool flag = true;
		deltaCompressedFrameData.positions = new uint[list.Count * 3];
		for (int k = 0; k < list.Count; k++)
		{
			deltaCompressedFrameData.positions[k * 3] = (uint)((list[k].x + (float)deltaCompressedFrameData.sizeOffset) * deltaCompressedFrameData.accuracy);
			deltaCompressedFrameData.positions[k * 3 + 1] = (uint)((list[k].y + (float)deltaCompressedFrameData.sizeOffset) * deltaCompressedFrameData.accuracy);
			deltaCompressedFrameData.positions[k * 3 + 2] = (uint)((list[k].z + (float)deltaCompressedFrameData.sizeOffset) * deltaCompressedFrameData.accuracy);
			if (flag)
			{
				if (deltaCompressedFrameData.positions[k * 3] > 65535U)
				{
					flag = false;
				}
				else if (deltaCompressedFrameData.positions[k * 3 + 1] > 65535U)
				{
					flag = false;
				}
				else if (deltaCompressedFrameData.positions[k * 3 + 2] > 65535U)
				{
					flag = false;
				}
			}
		}
		deltaCompressedFrameData.compatibilityPositions = new int[deltaCompressedFrameData.positions.Length];
		for (int l = 0; l < deltaCompressedFrameData.positions.Length; l++)
		{
			deltaCompressedFrameData.compatibilityPositions[l] = (int)deltaCompressedFrameData.positions[l];
		}
		if (flag)
		{
			deltaCompressedFrameData.positionsUShort = new ushort[deltaCompressedFrameData.positions.Length];
			for (int m = 0; m < deltaCompressedFrameData.positions.Length; m++)
			{
				deltaCompressedFrameData.positionsUShort[m] = (ushort)deltaCompressedFrameData.positions[m];
			}
			deltaCompressedFrameData.positions = null;
		}
		return deltaCompressedFrameData;
	}

	// Token: 0x040032F7 RID: 13047
	public static float compressionAccuracy = 1000f;

	// Token: 0x040032F8 RID: 13048
	public float accuracy = 1000f;

	// Token: 0x040032F9 RID: 13049
	public int sizeOffset = 1;

	// Token: 0x040032FA RID: 13050
	public int vertLength;

	// Token: 0x040032FB RID: 13051
	[HideInInspector]
	public ushort[] positionsUShort;

	// Token: 0x040032FC RID: 13052
	[HideInInspector]
	public uint[] positions;

	// Token: 0x040032FD RID: 13053
	[HideInInspector]
	public int[] frameIndexes;

	// Token: 0x040032FE RID: 13054
	[HideInInspector]
	public int[] compatibilityPositions;
}
