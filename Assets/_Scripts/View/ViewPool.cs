using Zenject;

public class ViewPool : View
{
	private IMemoryPool _pool;

	public void SetPool(IMemoryPool pool)
	{
		_pool = pool;
	}

	public void SelfRelease()
	{
		_pool?.Despawn(this);
	}
}