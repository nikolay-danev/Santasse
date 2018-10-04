using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HandDeck : MonoBehaviour
{
    public IList<GameObject> cards;

	void Start ()
    {
        this.cards = new List<GameObject>();
	}

    public bool ContainsCard(string cardName)
    {
        return this.cards.Any(c => c.name.Split(' ')[0] == cardName);
    }
}
