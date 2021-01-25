using System;
using UnityEngine;

// Token: 0x0200054C RID: 1356
public class Swarm : MonoBehaviour
{
	// Token: 0x06002257 RID: 8791 RVA: 0x0010CF08 File Offset: 0x0010B108
	private void Start()
	{
		this.angle = Random.Range(0f, 360f);
		this.lastPosition = this.GetNewPos();
		float num = this.swarmRadius / this.birdsDistance;
		for (int i = 0; i < this.birdsCount; i++)
		{
			Vector3 vector;
			vector..ctor(Random.Range(0f, num) * this.birdsDistance, Random.Range(0f, num) * this.birdsDistance, Random.Range(0f, num) * this.birdsDistance);
			vector += base.transform.position;
			GameObject gameObject = (GameObject)Object.Instantiate(this.bird, vector, base.transform.rotation);
			gameObject.transform.parent = base.transform;
		}
	}

	// Token: 0x06002258 RID: 8792 RVA: 0x0010CFD8 File Offset: 0x0010B1D8
	private void FixedUpdate()
	{
		Vector3 newPos = this.GetNewPos();
		base.transform.position += newPos - this.lastPosition;
		this.lastPosition = newPos;
		this.angle = Mathf.MoveTowardsAngle(this.angle, this.angle + this.speed * Time.deltaTime, this.speed * Time.deltaTime);
	}

	// Token: 0x06002259 RID: 8793 RVA: 0x0010D048 File Offset: 0x0010B248
	private Vector3 GetNewPos()
	{
		Vector3 result;
		result.x = 0f;
		result.y = Mathf.Sin(this.angle * 0.017453292f) * this.amplitude;
		result.z = 0f;
		return result;
	}

	// Token: 0x0400288C RID: 10380
	public GameObject bird;

	// Token: 0x0400288D RID: 10381
	public int birdsCount;

	// Token: 0x0400288E RID: 10382
	public float swarmRadius;

	// Token: 0x0400288F RID: 10383
	public float birdsDistance;

	// Token: 0x04002890 RID: 10384
	public float amplitude;

	// Token: 0x04002891 RID: 10385
	public float speed;

	// Token: 0x04002892 RID: 10386
	private float angle;

	// Token: 0x04002893 RID: 10387
	private Vector3 lastPosition;
}
