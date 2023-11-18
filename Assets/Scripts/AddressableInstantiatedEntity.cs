using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Sabanishi.AddressableTest
{
    /// <summary>
    /// AddressableSystemによって生成されたオブジェクトにアタッチするスクリプト
    /// </summary>
    public class AddressableInstantiatedEntity : MonoBehaviour
    {
        private void OnDestroy()
        {
            Addressables.ReleaseInstance(gameObject);
        }
    }
}