public abstract class PresenterPoolBase<TView> : PresenterBase<TView> where TView : ViewPool
{
    private bool _desposed;
    protected PresenterPoolBase(TView view) : base(view)
	{
        _desposed = false;
    }

	public override void Dispose()
    {
        if (_desposed)
        {
            this.LogError($"{GetHashCode()} ({_desposed})");
            return;
        }

        this.Log($"{GetHashCode()} ({_desposed})");

        _desposed = true;

        View.SelfRelease();
		base.Dispose();
	}
}