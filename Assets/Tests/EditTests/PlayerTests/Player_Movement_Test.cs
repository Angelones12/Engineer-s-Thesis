using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Scripts;
public class Player_Movement_Test
{

    [Test]
    public void IsMoving_WithNonZeroInput_ReturnsTrue()
    {
        GameObject playerObject = new GameObject();
        PlayerMovement playerMovement = playerObject.AddComponent<PlayerMovement>();

        playerMovement.UpdateInput(new Vector2(1.0f, 0.0f));
        bool isMoving = playerMovement.IsMoving();

        Assert.IsTrue(isMoving);
    }

    [Test]
    public void IsMoving_WithZeroInput_ReturnsFalse()
    {
        
        GameObject playerObject = new GameObject();
        PlayerMovement playerMovement = playerObject.AddComponent<PlayerMovement>();

        
        playerMovement.UpdateInput(Vector2.zero);
        bool isMoving = playerMovement.IsMoving();

        Assert.IsFalse(isMoving);
    }

}
