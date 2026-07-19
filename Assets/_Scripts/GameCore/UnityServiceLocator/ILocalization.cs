using UnityEngine;

namespace UnityServiceLocator
{
    public interface ILocalization
    {
        string GetLocalizedWord(string key);
    }

    public interface ISerializer
    {
        void Serialize();
    }

    public interface IAudioService
    {
        void Play();
    }
}