using UnityEngine;
using Zenject;

public static class DiContainerExtensions
{
	public static void BindViewController<TView, TPresenter>(this DiContainer container, View view,
	  Transform transform) where TView : View where TPresenter : Presenter
	{
		container.BindView<TView>(view, transform);
		container.BindController<TPresenter>();
	}

	public static void BindView<TView>(this DiContainer container, View view, Transform transform)
	  where TView : View
	{
		container.Bind<TView>().FromComponentInNewPrefab(view).UnderTransform(transform).AsSingle();
	}

	private static void BindController<TPresenter>(this DiContainer container) where TPresenter : Presenter
	{
		container.BindInterfacesAndSelfTo<TPresenter>().AsSingle().NonLazy();
	}
}