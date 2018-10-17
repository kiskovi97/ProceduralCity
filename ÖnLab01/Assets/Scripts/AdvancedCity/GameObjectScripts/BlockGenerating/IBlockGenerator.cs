using UnityEngine;
namespace Assets.Scripts.AdvancedCity
{
    public interface IBlockGenerator
    {
        void Clear();
        void SetValues(IValues values);
        void GenerateBuildings(Vector3[] vertexes, IObjectGenerator objectGenerator);
    }
}

