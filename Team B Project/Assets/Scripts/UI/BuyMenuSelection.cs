using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyMenuSelection : MonoBehaviour
{
    public Ship.shipType _ship;
    public BuyMenuButton _selectedButton;

    public Text _shipName;
    public Text _shipDesc;
    public Image _shipImage;
    public Slider attackSlider;
    public Slider defenseSlider;
    public Slider moveSpeedSlider;
    public Slider atkSpeedSlider;
    public Slider rangeSlider;
    public Text costText;

    public void UpdateSelection(BuyMenuButton selectedButton)
    {
        _selectedButton = selectedButton;

        costText.text = selectedButton.fuelCost + " Fuel\n" + selectedButton.metalCost + " Metal";

        _ship = _selectedButton.ship;

        _shipName.text = selectedButton.shipName;
        _shipDesc.text = selectedButton.shipDesc;
        _shipImage.sprite = selectedButton.shipImage;
        _shipImage.color = selectedButton.shipImageColor;
        attackSlider.value = selectedButton.attackPts;
        atkSpeedSlider.value = selectedButton.attackPts;
        rangeSlider.value = selectedButton.attackPts;
        defenseSlider.value = selectedButton.defensePts;
        moveSpeedSlider.value = selectedButton.moveSpeedPts;
    }

    public void PurchaseShip()
    {
        //if (player.Resources >= ship.cost) {
        ControlledPlayer.Instance.SpawnUnit(_ship);
        //}
    }
}
