using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class Health_Tests
{
    [Test]
    public void Health_SetValueBelowOrEqualToZero_ShouldInvokeOnDeathEvent()
    {
        float defaultHealth = 100f;
        HealthSystem healthSystem = new HealthSystem(defaultHealth);
        bool deathEventInvoked = false;
        healthSystem.OnDeath += () => deathEventInvoked = true;

        healthSystem.Health = 0f;

        Assert.IsTrue(deathEventInvoked);
    }
    [Test]
    public void Health_SetValueWithinRange_ShouldInvokeOnHealthChangedEvent() {
        float defaultHealth = 100f;
        HealthSystem healthSystem = new HealthSystem(defaultHealth);
        bool healthChangedEventInvoked = false;
        healthSystem.OnHealthChanged += () => healthChangedEventInvoked = true;

        healthSystem.Health = 50f;

        Assert.IsTrue(healthChangedEventInvoked);
    }
}
