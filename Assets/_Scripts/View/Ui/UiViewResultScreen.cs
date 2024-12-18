using ModestTree;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiViewResultScreen : UiView
{
	[field: SerializeField]
	public Button ButtonNewGame { get; private set; }
	[field: SerializeField]
	public GameObject PanelDefeat { get; private set; }
	[field: SerializeField]
	public GameObject PanelWin { get; private set; }
	[field: SerializeField]
	public TextMeshProUGUI TextDebug { get; private set; }


	public void Awake()
	{
		Assert.IsNotNull(ButtonNewGame);
		Assert.IsNotNull(PanelDefeat);
		Assert.IsNotNull(PanelWin);
		Assert.IsNotNull(TextDebug);
	}
}