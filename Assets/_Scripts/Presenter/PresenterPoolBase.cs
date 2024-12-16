public abstract class PresenterPoolBase<TView> : PresenterBase<TView> where TView : ViewPool
{
	protected PresenterPoolBase(TView view) : base(view)
	{
	}

	public override void Dispose()
	{
		View.SelfRelease();
		base.Dispose();
	}
}