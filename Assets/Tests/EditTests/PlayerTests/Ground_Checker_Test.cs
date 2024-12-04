using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class Ground_Checker_Test
{
    [Test]
    public void IsGrounded_WhenColliderWithTagEnters_ShouldReturnTrue()
    {
        // Arrange
        GameObject gameObject = new GameObject();
        GroundChecker groundChecker = gameObject.AddComponent<GroundChecker>();
        Collider collider = gameObject.AddComponent<BoxCollider>();
        collider.tag = "Ground";

        // Act
        groundChecker.OnTriggerEnter(collider);

        // Assert
        Assert.IsTrue(groundChecker.IsGrounded());
    }

    [Test]
    public void IsGrounded_WhenColliderWithTagExits_ShouldReturnFalse()
    {
        // Arrange
        GameObject gameObject = new GameObject();
        GroundChecker groundChecker = gameObject.AddComponent<GroundChecker>();
        Collider collider = gameObject.AddComponent<BoxCollider>();
        collider.tag = "Ground";

        // Act
        groundChecker.OnTriggerEnter(collider);
        groundChecker.OnTriggerExit(collider);

        // Assert
        Assert.IsFalse(groundChecker.IsGrounded());
    }
}
