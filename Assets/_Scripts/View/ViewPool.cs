using Zenject;

public class ViewPool : View, ISpawnElementsView
{
	private IMemoryPool _pool;
	private int _id= -1;

	public int Id => _id;

	public void SetId(int id) => _id = Id;

	public void SetPool(IMemoryPool pool)
	{
		_pool = pool;
	}

	public void SelfRelease()
	{
		_pool?.Despawn(this);
	}
}