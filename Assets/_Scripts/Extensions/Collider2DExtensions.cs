using UnityEngine;

public static class Collider2DExtensions
{
	public static bool TryGetTriggerId(this Collider2D collider, out int id)
	{
		id = ObjectUtils.DEFAULT_ID;
		
		if (!collider.TryGetComponent<TriggerComponent>(out var trigger))
		{
			collider.LogError($"{nameof(TriggerComponent)} component not found on object with {collider.tag} tag!");
			return false;
		}

		if (trigger.Id <= ObjectUtils.DEFAULT_ID)
		{
			collider.LogError($"{nameof(ITriggerComponent)} component with {collider.tag} tag has invalid id: {trigger.Id}");
			return false;
		}

		if (!trigger.IsVisible)
		{
			collider.LogWarning($"{nameof(TriggerComponent)} component with {collider.tag} tag and {trigger.Id} id is not visible!");
			return false;
		}

		id = trigger.Id;
		return true;
	}
}