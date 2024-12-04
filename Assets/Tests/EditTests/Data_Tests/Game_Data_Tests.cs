using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Scripts.Data;
using UnityEngine;
using UnityEngine.TestTools;

public class Game_Data_Tests
{
    [Test]
    public void GetPercentageComplete_NoPointsCollected_ReturnsZero()
    {
        // Arrange
        GameData yourClassInstance = new GameData();
        // Zak³adamy, ¿e pointsCollected jest ustawione na zero
        yourClassInstance.pointsCollected = 0;

        // Act
        int result = yourClassInstance.GetPercentageComplete();

        // Assert
        Assert.AreEqual(0, result);
    }

    [Test]
    public void GetPercentageComplete_PointsCollected_ReturnsCorrectPercentage()
    {
        // Arrange
        GameData yourClassInstance = new GameData();
        // Ustawienie liczby punktów zebranych, na przyk³ad 3
        yourClassInstance.pointsCollected = 3;

        // Act
        int result = yourClassInstance.GetPercentageComplete();

        // Assert
        Assert.AreEqual(60, result); // W zale¿noœci od implementacji mo¿esz dostosowaæ oczekiwany wynik
    }

    [Test]
    public void GetPercentageComplete_AllPointsCollected_Returns100()
    {
        // Arrange
        GameData yourClassInstance = new GameData();
        // Ustawienie liczby punktów zebranych równej ca³kowitej liczbie punktów
        yourClassInstance.pointsCollected = 5;

        // Act
        int result = yourClassInstance.GetPercentageComplete();

        // Assert
        Assert.AreEqual(100, result);
    }
}
