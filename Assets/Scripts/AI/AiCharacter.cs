using System;
using System.Collections.Generic;
using Game.Character;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AI
{
    public class AiCharacter : Character
    {
        [Serializable]
        struct RandomItem
        {
            public float possibility;
            public DroppedItem prefab;
        }
        [TabGroup("DropItem"), AssetsOnly, SerializeField] private List<RandomItem> _randomItems;
        
        public override void Died()
        {
            double randomValue = new System.Random().NextDouble();
            foreach (var item in _randomItems)
            {
                if (item.possibility >= randomValue)
                {
                    Instantiate(item.prefab, transform.position, Quaternion.identity);
                }
            }
            
            base.Died();
        }
    }
}