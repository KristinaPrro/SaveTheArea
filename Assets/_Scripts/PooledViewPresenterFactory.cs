using System.Linq;
using UnityEngine;
using Zenject;

public class PooledViewPresenterFactory<TPresenter, TView, TPool>
  : IFactory<Transform, TPresenter>
  where TPresenter : PresenterPoolBase<TView>
  where TView : ViewPool
  where TPool : MonoMemoryPoolWithTransform<TView>
{
	private readonly IInstantiator _instantiator;
	private readonly TPool _pool;

	public PooledViewPresenterFactory(IInstantiator instantiator, TPool pool)
	{
		_instantiator = instantiator;
		_pool = pool;
	}

	public virtual TPresenter Create(Transform viewParent)
	{
		return BaseCreate(viewParent);
	}

	protected TPresenter BaseCreate(Transform viewParent, object[] args = null)
	{
		var view = GetPool(args).Spawn(viewParent);
		var parameters = args == null
		  ? new object[] { view }
		  : Enumerable.Repeat((object)view, 1)
			.Concat(args)
			.ToArray();
		var presenter = _instantiator.Instantiate<TPresenter>(parameters);
		presenter.Initialize();
		return presenter;
	}

	protected virtual TPool GetPool(object[] args = null)
	{
		return _pool;
	}
}

public class MonoMemoryPoolWithTransform<TView> : SimpleMonoMemoryPool<Transform, TView>
  where TView : ViewPool
{
	protected override void OnCreated(TView item)
	{
		base.OnCreated(item);
		item.SetPool(this);
	}

	protected override void Reinitialize(Transform parent, TView item)
	{
		base.Reinitialize(parent, item);
		item.transform.SetParent(parent, false);
	}
}

public class SimpleMonoMemoryPool<TParam1, TValue> : MonoMemoryPool<TParam1, TValue>
		where TValue : Component
{
	[Inject]
	public SimpleMonoMemoryPool()
	{
	}

	protected override void OnCreated(TValue item)
	{
		item.gameObject.SetActive(false);
	}

	protected override void OnDespawned(TValue item)
	{
		item.gameObject.SetActive(false);
	}
}