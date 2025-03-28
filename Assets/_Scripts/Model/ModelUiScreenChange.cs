﻿using System;
using System.Linq;
using UniRx;

public class ModelUiScreenChange: IDisposable
{	
	protected readonly CompositeDisposable Disposables = new();

	private IUiPresenter[] _presenters;
	protected IUiPresenter _presenterCurrentWindow;

	public ModelUiScreenChange()
	{
	}

	public void Init(IUiPresenter[] presenters)
	{
		_presenters = presenters;
	}

	public virtual void Dispose()
	{
		Disposables.Dispose();
	}

	public virtual void ChangeScreen(WindowType windowType)
	{
		if (_presenterCurrentWindow?.WindowType == windowType)
			return;

		Close(_presenterCurrentWindow);
		Open(windowType);
	}

	private void Open(WindowType windowType)
	{
		if (!TryGetPresenter(windowType, out var presenter))
			return;

		this.Log($"Open({presenter?.WindowType})");
		presenter?.Show();

		_presenterCurrentWindow = presenter;
	}

	public void CloseAll()
	{
		foreach (var presenter in _presenters)
			Close(presenter);
	}

	private void Close(WindowType windowType) 
	{
		if (!TryGetPresenter(windowType, out var presenter))
			return;

		Close(presenter);
	}

	private void Close(IUiPresenter presenter) 
	{
		this.Log($"Close({presenter?.WindowType})");
		presenter?.Hide();
	}

	private bool TryGetPresenter(WindowType windowType, out IUiPresenter presenter)
	{
		presenter = _presenters.FirstOrDefault(p => p.WindowType == windowType);

		if (presenter == null || presenter == default)
		{
			this.LogError($"{nameof(IUiPresenter)} with {windowType} type not found!");
			return false;
		}

		return true;
	}
}