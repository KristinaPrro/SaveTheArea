using ModestTree;
using UnityEngine;

public abstract class ViewPoolDamageElement : ViewPool
{
	public Collider2D _collider { get; private set; }
	public Collider2D Collider
	{
		get
		{
			if (_collider == null)
				_collider = GetComponent<Collider2D>();

			return _collider;
		}
	}
}
