using System;
using UnityEngine;

public class TriggerComponent : MonoBehaviour, ITriggerComponent
{
	[field: SerializeField] // todo delete
	public int Id { get; set; } = -1;

	public void SetId(int id)
	{
		Id = id;
	}
}