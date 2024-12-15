using UnityEngine;
using Zenject;

public static class DiContainerExtensions
{
	public static void BindViewController<TView, TPresenter>(this DiContainer container, UiView view,
	  Transform transform) where TView : UiView where TPresenter : PresenterBase
	{
		container.BindView<TView>(view, transform);
		container.BindController<TPresenter>();
	}

	public static void BindView<TView>(this DiContainer container, UiView view, Transform transform)
	  where TView : UiView
	{
		container.Bind<TView>().FromComponentInNewPrefab(view).UnderTransform(transform).AsSingle();
	}

	private static void BindController<TPresenter>(this DiContainer container) where TPresenter : PresenterBase
	{
		container.BindInterfacesAndSelfTo<TPresenter>().AsSingle().NonLazy();
	}
}