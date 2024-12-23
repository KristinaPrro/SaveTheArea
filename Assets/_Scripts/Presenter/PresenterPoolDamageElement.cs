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
	}

	public override void Initialize()
	{
		View.Collider.OnTriggerEnter2DAsObservable().Subscribe(OnTriggerEnter2D).AddTo(Disposables);
		View.transform.position = DamageElementData.StartPosition.position;
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

				if (!other.TryGetComponent<ITriggerComponent>(out var enemy))
				{
					this.LogError($"{nameof(ITriggerComponent)} component not found on object with {other.tag} tag!");
					break;
				}

				SignalBus.Fire(new SignalEnemyDamage(Id, enemy.Id, Damage));
				break;

			case ObjectUtils.DISAPPEARANCE_TAG:

				SignalBus.Fire(new SignalDisappearanceDamageElement(Id));
				break;
		}
	}
}