using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICollectable
{
    void AddThisToPlayerData(PlayerDataFromJson playerDataFromJson);
    void RemoveThisFromPlayerData(PlayerDataFromJson playerDataFromJson);
}
