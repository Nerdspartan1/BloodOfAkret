using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Shop : MonoBehaviour
{
	[Header("Prefabs")]
	public GameObject PerkCardPrefab;

	public List<Perk> UnlockedPerks;
	public int MaxPerksForSale;

	public void PresentPerks()
	{
		List<Perk> randomPerks = new List<Perk>(UnlockedPerks);
		randomPerks.RandomizeList();

		int nbPerksForSale = Mathf.Min(MaxPerksForSale, randomPerks.Count);

		foreach(Transform card in transform)
		{
			Destroy(card.gameObject);
		}

		for (int i = 0; i < nbPerksForSale; ++i)
		{
			PerkCard card = Instantiate(PerkCardPrefab, transform).GetComponent<PerkCard>();
			card.SetPerk(randomPerks[i]);
		}
	}
}
