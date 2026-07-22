using System.Collections;
using UnityEngine;

namespace Systems.Persistence
{


    public interface ISerialize
    {
        string Serialize<T>(T obj);
        T Deserialize<T>(string json);
    }
}