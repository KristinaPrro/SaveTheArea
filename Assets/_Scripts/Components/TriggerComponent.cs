using UnityEngine;

public class TriggerComponent : MonoBehaviour, ITriggerComponent
{
	public int Id { get; set; } = -1;

	public void SetId(int id)
	{
		Id = id;
	}
}