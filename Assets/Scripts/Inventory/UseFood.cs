using UnityEngine;

public class UseFood : MonoBehaviour
{
	public Food food;
	private Indicators _indicators;

	private void Start() => _indicators = Indicators.instance;

	public void EatFood() 
	{
		_indicators.health += food.heal;
		_indicators.satiety += food.satiety;
		_indicators.UpdateAllValues();

		Debug.Log("Eaten, +" + food.heal + " hearth, +" + food.satiety + " satiety");
	}
}
