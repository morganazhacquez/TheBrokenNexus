using UnityEngine;

namespace TheBrokenNexus.Actors.Enimies
{
    [CreateAssetMenu(fileName = "New Enemy", menuName = "The Broken Nexus/Actors/New Enemy")]
    public class Data : ScriptableObject
    {
        public string Name;
        public int BaseLife;
    }
}