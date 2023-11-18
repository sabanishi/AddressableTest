using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Sabanishi.AddressableTest
{
    public class User:MonoBehaviour
    {
        private void Start()
        {
            UniTask.Void(async () =>
            {
                await FirebaseInitializer.Initialize(this.GetCancellationTokenOnDestroy());
                Debug.Log("Firebaseの初期化に成功しました");
            });
        }
    }
}