using System;
using UnityEngine;

public class TriggerComponent : MonoBehaviour, ITriggerComponent
{
	private int _id = -1;

	public int Id => _id;

	public void SetId(int id)
	{
		_id = id;
	}
}