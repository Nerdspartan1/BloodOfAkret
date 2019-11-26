using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PerkCard : MonoBehaviour
{
	[Header("References")]
	public Text Name;
	public Text Price;
	public Image Icon;
	public Text Description;

	public Perk Perk;
	private Button _button;


	public void SetPerk(Perk perk)
	{
		Perk = perk;
		Name.text = Perk.Name;
		Price.text = $"{Perk.Price} offering points";
		Icon.sprite = Perk.Icon;
		Description.text = Perk.Description;
		_button = GetComponent<Button>();
		if (Perk.Price <= WaveManager.Instance.Points)
		{
			_button.onClick.RemoveAllListeners();
			_button.onClick.AddListener(PurchasePerk);
		}
		else
		{
			_button.interactable = false;
		}
	}

	public void PurchasePerk()
	{
		GameManager.Instance.Player.GetComponent<Player>().PerkUp(Perk);
		WaveManager.Instance.Points -= Perk.Price;
		WaveManager.Instance.CloseShop();
	}
}
