using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card",menuName ="Card")]
public class Card : ScriptableObject {

    public string type;
    public int power;

    public Sprite artWork;
}
