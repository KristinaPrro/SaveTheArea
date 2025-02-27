using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

public abstract class PresenterPoolDamageElement<TView> : PresenterPoolBase<TView>, ITickable, IDamageElement
	 where TView : ViewPoolDamageElement
{
	protected readonly CompositeDisposable Disposables = new();
	protected readonly SignalBus SignalBus;
	protected readonly DamageElementData DamageElementData;

	protected Vector2 DirectionMovement;

	protected Rigidbody2D Rigidbody => View.Rigidbody;
	protected int Damage => DamageElementData.Damage;
	protected float Speed => DamageElementData.Speed;

	public int Id => DamageElementData.Id;

	public PresenterPoolDamageElement(TView view, DamageElementData damageElementData, SignalBus signalBus) 
		: base(view)
	{
		SignalBus = signalBus;
		DamageElementData = damageElementData;
		View.transform.position = damageElementData.StartPosition.position;
	}

	public override void Initialize()
	{
		base.Initialize();
		View.Collider.OnTriggerEnter2DAsObservable().Subscribe(OnTriggerEnter2D).AddTo(Disposables);
	}

	public override void Dispose()
	{
		DirectionMovement = Vector2.zero;
		Disposables.Dispose();

		base.Dispose();
	}

	public void Tick()
	{
		Rigidbody.MovePosition(Rigidbody.position + DirectionMovement * Speed * Time.deltaTime);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		switch (other.tag)
		{
			case ObjectUtils.ROBOT_TAG:

				if (!other.TryGetTriggerId(out int id))
					break;

				SignalBus.Fire(new SignalEnemyDamage(Id, id, Damage));
				break;

			case ObjectUtils.DISAPPEARANCE_TAG:

				SignalBus.Fire(new SignalDisappearanceDamageElement(Id));
				break;
		}
	}
}